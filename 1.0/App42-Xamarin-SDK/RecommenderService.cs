using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.shephertz.app42.paas.sdk.csharp.util;
using com.shephertz.app42.paas.sdk.csharp.connection;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.recommend
{
    public sealed class RecommenderSimilarity
    {
        public static readonly string EUCLIDEAN_DISTANCE = "EuclideanDistanceSimilarity";
        public static readonly string PEARSON_CORRELATION = "PearsonCorrelationSimilarity";
    }

    /// <summary>
    /// Recommendation engine which provides reommendation based on customer id, item id and the preference
    /// of the customer for a particular Item.
    /// Recommendations can be fetched based on User Similarity which finds similarity based on Users
    /// and Item Similarity which finds similarity based on Items.
    /// The Recommendation Engine currently supports two types of Similarity Algorithms i.e. EuclideanDistanceSimilarity
    /// and PearsonCorrelationSimilarity. By default when similarity is not specified PearsonCorrelationSimilarity is used e.g.
    /// in the method itemBased(String preferenceFileName, Double   userId, int  howMany), it uses PearsonCorrelationSimilarity.
    /// In the method itemBasedBySimilarity(String similarity, String preferenceFileName, Double   userId, int  howMany) one can
    /// specify which similarity algorithm has to be used e.g. Recommender.EUCLIDEAN_DISTANCE or Recommender.PEARSON_CORRELATION.
    /// Preference file can be loaded using the method loadPreferenceFile(String fileName, String preferenceFilePath, String description)
    /// in csv format. This prefernce file has to be uploaded once which can be a batch process
    /// The csv format for the file is given below.
    ///  customerId, itemId, preference
    ///  e.g.
    ///  1,101,5.0
    ///  1,102,3.0
    ///  1,103,2.5
    ///  2,101,2.0
    ///  2,102,2.5
    ///  2,103,5.0
    ///  2,104,2.0
    ///  3,101,2.5
    ///  3,104,4.0
    ///  3,105,4.5
    ///  3,107,5.0
    ///  4,101,5.0
    ///  4,103,3.0
    ///  4,104,4.5
    ///  4,106,4.0
    ///  5,101,4.0
    ///  5,102,3.0
    ///  5,103,2.0
    ///  5,104,4.0
    ///  5,105,3.5
    ///  5,106,4.0
    /// The customer Id and item id can be any alphanumaric character(s) and preference values can be in any range.
    /// If app developers have used the Review Service. The Recommendation Engine can be used in conjunction with Review. 
    /// In this case a CSV preference file need not be uploaded. The customerId, itemId and preference will be taken from
    /// Review where customerId is mapped with userName, itemId is mapped with itemId and preference with rating.
    /// The methods for recommendations based on Reviews are part of the Review service
    /// </summary>
    /// <see cref="RecommenderService">ReviewService</see>
    /// <see cref="RecommenderService">Recommender</see>
    /// <see cref="RecommenderService">RecommenderService</see>


    public class RecommenderService
    {
        private String version = "1.0";
        private String resource = "recommend";
        private String apiKey;
        private String secretKey;

        /// <summary>
        /// The costructor for the Service
        /// </summary>
        /// <param name="apiKey">api Key</param>
        /// <param name="secretKey">secret Key</param>


        public RecommenderService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;

        }

        /// <summary>
        /// Uploads peference file on the cloud. The preference file should be in CSV format.
        /// This prefernce file has to be uploaded once which can be a batch process.
        /// New versions of preference file either can be uploaded in a different name or the older one
        /// has to be removed and the uploaded in the same name.
        /// The csv format for the file is given below.
        ///  customerId, itemId, preference
        ///  e.g.
        ///  1,101,5.0
        ///  1,102,3.0
        ///  1,103,2.5
        ///
        ///  2,101,2.0
        ///  2,102,2.5
        ///  2,103,5.0
        ///  2,104,2.0
        ///
        ///  3,101,2.5
        ///  3,104,4.0
        ///  3,105,4.5
        ///  3,107,5.0
        ///
        ///  4,101,5.0
        ///  4,103,3.0
        ///  4,104,4.5
        ///  4,106,4.0
        ///
        ///  5,101,4.0
        ///  5,102,3.0
        ///  5,103,2.0
        ///  5,104,4.0
        ///  5,105,3.5
        ///  5,106,4.0
        /// The customer Id and item id can be any alphanumaric character(s) and preference values can be in any range.
        /// If the recommendations have to be done based on Reviews then this file need not be uploaded.
        /// </summary>
        /// <param name="name">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="preferenceFilePath">Path of the preference file to be loaded</param>
        /// <param name="description">Description of the preference file to be loaded</param>
        /// <returns>Returns the uploaded preference file details.</returns>
        /// <exception>App42Exception</exception>   

        public App42Response LoadPreferenceFile(String preferenceFilePath)
        {
            String response = null;
            App42Response responseObj = new App42Response();
           // Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(preferenceFilePath,
                    "PreferenceFilePath");
            //Util.ThrowExceptionIfNullOrBlank(description, "Description");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
           // postParams.Add("name", name);
          //  postParams.Add("description", description);
            // Put these params for signing
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource;
            response = Util.MultiPartRequest("preferenceFile", preferenceFilePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }

        /// <summary>
        /// Uploads peference file on the cloud. The preference file should be in CSV format.
        /// This prefernce file has to be uploaded once which can be a batch process.
        /// New versions of preference file either can be uploaded in a different name or the older one
        /// has to be removed and the uploaded in the same name.
        /// The csv format for the file is given below.
        ///  customerId, itemId, preference
        ///  e.g.
        ///  1,101,5.0
        ///  1,102,3.0
        ///  1,103,2.5
        ///
        ///  2,101,2.0
        ///  2,102,2.5
        ///  2,103,5.0
        ///  2,104,2.0
        ///
        ///  3,101,2.5
        ///  3,104,4.0
        ///  3,105,4.5
        ///  3,107,5.0
        ///
        ///  4,101,5.0
        ///  4,103,3.0
        ///  4,104,4.5
        ///  4,106,4.0
        ///
        ///  5,101,4.0
        ///  5,102,3.0
        ///  5,103,2.0
        ///  5,104,4.0
        ///  5,105,3.5
        ///  5,106,4.0
        /// The customer Id and item id can be any alphanumaric character(s) and preference values can be in any range.
        /// If the recommendations have to be done based on Reviews then this file need not be uploaded.
        /// </summary>
        /// <param name="name">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="preferenceFilePath">Path of the preference file to be loaded</param>
        /// <param name="description">Description of the preference file to be loaded</param>
        /// <returns>Returns the uploaded preference file details.</returns>
        /// <exception>App42Exception</exception>   

        public App42Response LoadPreferenceFile(Stream stream)
        {
            String response = null;
            App42Response responseObj = new App42Response();

            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
            // postParams.Add("name", name);
            //  postParams.Add("description", description);
            // Put these params for signing
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource;
            response = Util.MultiPartRequest("preferenceFile", stream,"",queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// User based recommendations based on Neighborhood. Recommendations are found based on similar users in the Neighborhood
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="size">Size of the Neighborhood</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedNeighborhood(Int64 userId, int size, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preferenceFileName");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("size", "" + size);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/userBasedNeighborhood" + "/"  + userId + "/" + size + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }
        /// <summary>
        /// User based neighborhood recommendations based on Threshold. Recommendations are found based on Threshold
        /// where thereshold represents similarity threshold where user are atleast that similar.
        /// Threshold values can vary from -1 to 1
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="threshold">Threshold size. Values can vary from -1 to 1</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedThreshold(Int64 userId,Double threshold, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            //  Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
          //  paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("threshold", "" + threshold);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/userBasedThreshold" + "/" + userId + "/" + threshold + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }
        /// <summary>
        /// User based recommendations based on Neighborhood and Similarity. Recommendations and found based on the similar users in the Neighborhood with 
        /// the specified Similarity Algorithm. Algorithim can be specified using the constants Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION</param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="size">Size of the Neighborhood</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedNeighborhoodBySimilarity(String recommenderSimilarity,Int64 userId, int size, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            Util.ValidateHowMany(howMany);
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }


            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("similarity", "" + recommenderSimilarity);
            paramsDics.Add("howMany", "" + howMany);
            paramsDics.Add("size", "" + size);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                      + "/userBasedNeighborhood" + "/" + recommenderSimilarity + "/"
                      + userId + "/" + size + "/"
                      + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }
        /// <summary>
        /// User based neighborood recommendations based on Threshold. Recommendations are found based on Threshold
        /// where thereshold represents similarity threshold where user are atleast that similar.
        /// Threshold values can vary from -1 to 1
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION</param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="threshold">Threshold size. Values can vary from -1 to 1</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedThresholdBySimilarity(String recommenderSimilarity,Int64 userId, Double threshold,
                int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            Util.ValidateHowMany(howMany);
           // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("similarity", "" + recommenderSimilarity);
            paramsDics.Add("threshold", "" + threshold);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/userBasedThreshold" + "/" + recommenderSimilarity + "/"
                    + userId + "/" + threshold + "/"
                     + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }


        /// <summary>
        ///  Item based recommendations. Recommendations and found based item similarity
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender ItemBased(Int64 userId, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/itemBased" + "/" + userId
                     + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }

        /// <summary>
        /// Recommendations based on SlopeOne Algorithm
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender SlopeOne(Int64 userId, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            //  Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
         //   paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/slopeOne" + "/" + userId
                     + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }

        /// <summary>
        /// User based neighborood recommendations based on Threshold for all Users. Recommendations are found based on Threshold
        /// where thereshold represents similarity threshold where user are atleast that similar.
        /// Threshold values can vary from -1 to 1
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="threshold">Threshold size. Values can vary from -1 to 1</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for all Users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedThresholdForAll(Double threshold, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
          //  paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("threshold", "" + threshold);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/userBasedThreshold" + "/all" + "/" + threshold + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }



        /// <summary>
        /// User based recommendations based on Neighborhood and Similarity for all Users. Recommendations and found based similar users in the Neighborhood with 
        /// the specified Similarity Algorithm. Algorithim can be specified using the constants Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION</param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="size">Size of the Neighborhood</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for all Users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedNeighborhoodBySimilarityForAll(String recommenderSimilarity,int size, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            //  Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
        //    paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("size", "" + size);
            paramsDics.Add("howMany", "" + howMany);
            // paramsDics.Add("similarity", "" + recommenderSimilarity);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "userBasedNeighborhood" + "/all" + "/" + size + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }

        /// <summary>
        ///  User based neighborood recommendations based on Threshold for All. Recommendations are found based on Threshold
        /// where thereshold represents similarity threshold where user are atleast that similar.
        /// Threshold values can vary from -1 to 1
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION</param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="threshold">Threshold size. Values can vary from -1 to 1</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for All</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedThresholdBySimilarityForAll(String recommenderSimilarity,Double threshold, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            Util.ValidateHowMany(howMany);
         //   Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("threshold", "" + threshold);
            paramsDics.Add("howMany", "" + howMany);
            paramsDics.Add("similarity", "" + recommenderSimilarity);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                + "/userBasedThreshold" + "/all" + "/" + recommenderSimilarity + "/"
               + threshold + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }
        /// <summary>
        /// Item based recommendations for all Users. Recommendations and found based item similarity
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for all Users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender ItemBasedForAll(int howMany)
        {
            String response = null;
            Recommender recommender = null;
            // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
          //  paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("howMany", "" + howMany);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                     + "itemBased" + "/all" + "/" 
                     + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }


        /// <summary>
        /// Item based recommendations for all Users. Recommendations and found based one item similarity. Similarity algorithm can be specified.
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION    </param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for all Users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender ItemBasedBySimilarityForAll(String recommenderSimilarity,int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            Util.ValidateHowMany(howMany);
          //  Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("howMany", "" + howMany);
            paramsDics.Add("similarity", recommenderSimilarity);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                     + "itemBased" + "/all" + "/" + recommenderSimilarity + "/"
                     + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }


        /// <summary>
        /// Recommendations based on SlopeOne Algorithm for all Users
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for all Users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender SlopeOneForAll(int howMany)
        {
            String response = null;
            Recommender recommender = null;
            // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
           // paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("howMany", "" + howMany);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                      + "/slopeOne" + "/all" + "/" 
                      + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;

        }



        /// <summary>
        /// Item based recommendations. Recommendations and found based one item similarity. Similarity algorithm can be specified.
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="recommenderSimilarity">Similarity algorithm e.g. Recommender.EUCLIDEAN_DISTANCE and Recommender.PEARSON_CORRELATION</param>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="userId">The user Id for whom recommendations have to be found</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations</returns>
        /// <exception>App42Exception</exception>   

        public Recommender ItemBasedBySimilarity(String recommenderSimilarity,
              Int64 userId, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            Util.ThrowExceptionIfNullOrBlank(recommenderSimilarity, "recommenderSimilarity");
            Util.ValidateHowMany(howMany);
           // Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            if (!RecommenderSimilarity.EUCLIDEAN_DISTANCE.ToString().Equals(recommenderSimilarity) && !RecommenderSimilarity.PEARSON_CORRELATION.ToString().Equals(recommenderSimilarity))
            {
                throw new App42Exception("Not a Valid RecommenderSimilarity");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
          //  paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("howMany", "" + howMany);
            paramsDics.Add("userId", "" + userId);
            paramsDics.Add("similarity", recommenderSimilarity);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/itemBased/" + recommenderSimilarity + "/" + userId + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;

        }
        public App42Response AddOrUpdatePreference(IList<PreferenceData> preferenceDataList)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(preferenceDataList,
                    "preferenceDataList");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            JArray preferenceDataArray = new JArray();
            JObject obj = new JObject();
            for (int i = 0; i < preferenceDataList.Count(); i++)
            {
                PreferenceData preferenceData = preferenceDataList[i];
                JObject obj1 = new JObject();
                obj1.Add("UserId", preferenceData.GetUserId());
                obj1.Add("itemId", preferenceData.GetItemId());
                obj1.Add("preference", preferenceData.GetPreference());

                preferenceDataArray.Add(obj1.ToString());

            }
            App42Log.Debug("preferenceDataArray" + preferenceDataArray);
            JObject preference = new JObject();
            preference.Add("preference", preferenceDataArray);
            obj.Add("preferences", preference);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":").Append(obj.ToString())
                    .Append("}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
			paramsDics.Add("signature", signature);
			//App42Log.Debug("Recommender To String" + sb.toString());
			String resourceUrl = this.version + "/" + this.resource + "/" + "addOrUpdatePreference";
			response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceUrl, queryParams, sb.ToString());
			responseObj.SetStrResponse(response);
			responseObj.SetResponseSuccess(true);
            return responseObj;
        }

        /// <summary>
        /// User based recommendations based on Neighborhood for All Users. Recommendations and found based similar users in the Neighborhood
        /// of the given user. The size of the neighborhood can be found.
        /// </summary>
        /// <param name="preferenceFileName">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <param name="size">Size of the Neighborhood</param>
        /// <param name="howMany">Specifies that how many recommendations have to be found</param>
        /// <returns>Recommendations for All users</returns>
        /// <exception>App42Exception</exception>   

        public Recommender UserBasedNeighborhoodForAll(int size, int howMany)
        {
            String response = null;
            Recommender recommender = null;
            //  Util.ThrowExceptionIfNullOrBlank(preferenceFileName, "preference File Name");
            Util.ValidateHowMany(howMany);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
         //   paramsDics.Add("preferenceFileName", preferenceFileName);
            paramsDics.Add("howMany", "" + howMany);
            paramsDics.Add("size", "" + size);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/userBasedNeighborhood" + "/all" + "/"
                    + size + "/" + howMany;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            recommender = new RecommenderResponseBuilder().BuildResponse(response);
            return recommender;
        }



        /// <summary>
        /// Delete existing preference file.
        /// </summary>
        /// <param name="name">Name of the Prefeence File based on which recommendations have to be found</param>
        /// <returns>File name which has been removed</returns>
        /// <exception>App42Exception</exception>   

        public App42Response DeleteAllPreferences()
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/deleteAllPreferences";

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
    }
}