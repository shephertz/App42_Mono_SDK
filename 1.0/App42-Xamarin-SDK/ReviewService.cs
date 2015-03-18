using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.review
{

    /// <summary>
    /// The service is a Review & Rating manager for any item. The item can be anything which has an id
    /// e.g. App on a AppStore/Marketplace, items in a catalogue, articles, blogs etc.
    /// It manages the comments and its associated rating. It also provides methods to fetch average, highest etc. Reviews.
    /// Reviews can be also be muted or unmuted if it has any objectionable content.
    /// </summary>
    /// <see cref="ReviewService">Review</see>
    public class ReviewService
    {
        private String version = "1.0";
        private String resource = "review";
        private String apiKey;
        private String secretKey;
        String baseURL;
        /// <summary>
        /// The costructor for the Service
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>

        public ReviewService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;
        }
        ///<summary>
        /// Creates review for the specified item on the cloud
        /// </summary>
        /// <param name="userId"> The user who has created the review</param>
        /// <param name="itemId"> The item for which the review has to be created</param>
        /// <param name="reviewComment"> The review comment</param>
        /// <param name="reviewRating"> Review rating in Double</param>
        /// <returns>Review object containing the review which has been created</returns>
        /// <exception>App42Exception</exception>   

        public Review CreateReview(String userID, String itemID,
            String reviewComment, Double reviewRating)
        {
            String response = null;
            Review reviewObj = null;
            Util.ThrowExceptionIfNullOrBlank(userID, "User Id");
            Util.ThrowExceptionIfNullOrBlank(itemID, "Item Id");
            Util.ThrowExceptionIfNullOrBlank(reviewComment, "Review Comment");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> queryparams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userId");
            jsonWriter.WriteValue(userID);
            jsonWriter.WritePropertyName("itemId");
            jsonWriter.WriteValue(itemID);
            jsonWriter.WritePropertyName("comment");
            jsonWriter.WriteValue(reviewComment);
            jsonWriter.WritePropertyName("rating");
            jsonWriter.WriteValue(reviewRating);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"review\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature, resourceURL, queryparams, sb.ToString());
            reviewObj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewObj;
        }

        ///<summary>
        /// Fetches all reviews for the App
        ///</summary>
        /// <returns>IList of Review object containing all the reviews for the App</returns>
        /// <exception>App42Exception</exception>   

        public IList<Review> GetAllReviews()
        {
            String response = null;
            IList<Review> reviewList = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryparams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;

            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryparams);
            reviewList = new ReviewResponseBuilder().BuildArrayResponse(response);
            return reviewList;

        }

        /// <summary>
        /// Fetches All Reviews based on the itemId
        /// </summary>
        /// <param name="itemId"> The item for which reviews have to be fetched</param>
        /// <returns>IList of Review object containing all the reviews for a item</returns>
        /// <exception>App42Exception</exception>   

        public IList<Review> GetReviewsByItem(String itemId)
        {

            String response = null;
            IList<Review> reviewList = null;
            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/item"
                    + "/" + itemId;

            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            reviewList = new ReviewResponseBuilder().BuildArrayResponse(response);
            return reviewList;
        }



        /// <summary>
        /// Fetches All Reviews based on the itemId
        /// </summary>
        /// <param name="itemId"> The item for which reviews have to be fetched</param>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>        
        /// <returns>IList of Review object containing all the reviews for a item</returns>
        /// <exception>App42Exception</exception>   

        public IList<Review> GetReviewsByItem(String itemId, int max , int offset)
        {

            String response = null;
            IList<Review> reviewList = null;
            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemId);
            paramsDics.Add("max", ""+max);
            paramsDics.Add("offset", ""+offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/item"
                    + "/" + itemId + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            reviewList = new ReviewResponseBuilder().BuildArrayResponse(response);
            return reviewList;
        }



        ///<summary>
        /// Fetches the highest review for the specified itemId
        /// </summary>
        /// <param name= "itemId"> The item for which the highest review has to be fetched</param>
        /// <returns>Review object containing the highest review for a item</returns>
        /// <exception>App42Exception</exception>   


        public Review GetHighestReviewByItem(String itemId)
        {

            String response = null;
            Review reviewObj = null;
            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryparams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                   + itemId + "/highest";

            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryparams);
            reviewObj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewObj;
        }


        /// <summary>
        /// Fetches the lowest review for the specified itemId
        /// </summary>
        /// <param name = "itemId"> The item for which the lowest review has to be fetched</param>
        /// <returns>Review object containing the lowest review for a item</returns>
        /// <exception>App42Exception</exception>   

        public Review GetLowestReviewByItem(String itemId)
        {

            String response = null;
            Review reviewObj = null;

            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryparams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + itemId + "/lowest";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryparams);
            reviewObj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewObj;
        }

        /// <summary>
        /// Fetches the average review for the specified itemId
        /// </summary>
        /// <param name = "itemId"> The item for which the average review has to be fetched</param>
        /// <returns>Review object containing the average review for a item</returns>
        /// <exception>App42Exception</exception>   

        public Review GetAverageReviewByItem(String itemId)
        {

            String response = null;
            Review reviewObj = null;
            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + itemId + "/average";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            reviewObj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewObj;
        }

        /// <summary>
        /// Mutes the specified review
        /// </summary>
        /// <param name = "reviewId"> The Id of the review which has to be muted</param>
        /// <returns>App42Response if muted successfully</returns>
        /// <exception>App42Exception</exception>   

        public App42Response Mute(String reviewId)
        {

            String response = null;
            Util.ThrowExceptionIfNullOrBlank(reviewId, "Review Id");
            Review reviewobj = null;

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("id");
            jsonWriter.WriteValue(reviewId);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"review\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/mute";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            reviewobj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewobj;
        }

        /// <summary>
        /// UnMutes the specified review
        /// </summary>
        /// <param name = "reviewId"> The Id of the review which has to be unmuted</param>
        /// <returns>App42Response if unmuted successfully</returns>
        /// <exception>App42Exception</exception>   

        public App42Response Unmute(String reviewId)
        {

            String response = null;
            Util.ThrowExceptionIfNullOrBlank(reviewId, "Review Id");
            Review reviewObj = null;

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("id");
            jsonWriter.WriteValue(reviewId);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"review\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/mute";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            reviewObj = new ReviewResponseBuilder().BuildResponse(response);
            return reviewObj;
        }
        /// <summary>
        /// Fetches all reviews for the App by Paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>IList of Review object containing all the reviews for the App</returns>
        /// <exception>App42Exception</exception>

        public IList<Review> GetAllReviews(int max, int offset)
        {

            String response = null;
            IList<Review> reviewList = null;
            Util.ValidateMax(max);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/paging"
                    + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            reviewList = new ReviewResponseBuilder()
                    .BuildArrayResponse(response);
            return reviewList;
        }

        /// <summary>
        /// Fetches count of all reviews for the App
        /// </summary>
        /// <returns>App42Response containing count of all the reviews for the App</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAllReviewsCount()
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
            String resourceURL = this.version + "/" + this.resource + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new ReviewResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }

        /// <summary>
        /// Fetches count of All Reviews based on the itemId
        /// </summary>
        /// <param name="itemId">The item for which count of reviews have to be fetched</param>
        /// <returns>App42Response containing count of all the reviews for a item</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetReviewsCountByItem(String itemId)
        {

            String response = null;
            Util.ThrowExceptionIfNullOrBlank(itemId, "Item Id");
            App42Response responseObj = new App42Response();

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/item"
                    + "/" + itemId + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new ReviewResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}