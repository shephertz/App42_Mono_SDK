using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.geo
{
    /// <summary>
    /// Geo Spatial Service on cloud provides the storage, retrieval, querying and updation of geo data. 
    /// One can store the geo data by unique handler on the cloud and can apply search, update and query on it.
    /// Geo spatial query includes finding nearby/In circle target point from given point using geo points stored on the cloud.
    /// </summary>
    /// <see cref="GeoService">Geo</see>

    public class GeoService
    {

        private String version = "1.0";
        private String resource = "geo";
        private String apiKey;
        private String secretKey;
        String baseURL;

        public GeoService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;
        }

        /// <summary>
        /// Stores the geopints with unique handler on the cloud. Geo point data contains lat, Double and marker of the point.        
        /// </summary>
        /// <param name="geoStorageName"> Unique handler for storage name</param>
        /// <param name="geoPointsList"> List of Geo Points to be saved</param>
        /// <returns>Geo object containing IList of Geo Points that have been saved</returns>
        /// <exception>App42Exception</exception>

        public Geo CreateGeoPoints(String geoStorageName,
               IList<GeoPoint> geoPointsList)
        {

            String response = null;
            Geo geoObj = null;

            Util.ThrowExceptionIfNullOrBlank(geoStorageName, "Geo Storage Name");
            Util.ThrowExceptionIfNullOrBlank(geoPointsList, "Geo Points List");


            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("storageName");
            jsonWriter.WriteValue(geoStorageName);

            JArray geoArray = new JArray();
            foreach (GeoPoint geoPoint in geoPointsList)
            {
                App42Log.Debug(" Geo Points JSON " + "" + geoPoint.GetJSONObject());
                geoArray.Add(JObject.Parse("" + geoPoint.GetJSONObject()));

            }
            jsonWriter.WritePropertyName("points");
            jsonWriter.WriteValue("{\"point\":" + geoArray + "}");
            jsonWriter.WriteEndObject();


            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"geo\":{\"storage\":")
            .Append(sbJson.ToString())
                    .Append("}}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/createGeoPoints";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }

        /// <summary>
        /// Search the near by point from specified source point. 
        /// Points to be searched should already be stored on cloud using unique storage name handler. 
        /// </summary>
        /// <param name="storageName"> Unique handler for storage name</param>
        /// <param name="lat"> Lattitude of source point</param>
        /// <param name="lng"> Longitude of source point</param>
        /// <param name="resultLimit"> Maximum number of results to be retrieved </param>
        /// <returns>Geo object containing the target points in ascending order of distance from source point.</returns>
        /// <exception>App42Exception</exception>

        public Geo GetNearByPoint(String storageName, Double lat,
                Double lng, int resultLimit)
        {
            String response = null;
            Geo geoObj = null;

            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            paramsDics.Add("storageName", storageName);
            paramsDics.Add("lat", "" + lat);
            paramsDics.Add("lng", "" + lng);
            paramsDics.Add("resultLimit", "" + resultLimit);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/getNearByPoint" + "/storageName" + "/" + storageName
                    + "/lat" + "/" + lat + "/lng" + "/"
                    + lng + "/limit" + "/" + resultLimit;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }
        /// <summary>
        /// Search the near by point in given range(In KM) from specified source point. 
        /// Points to be searched should already be stored on cloud using unique storage name handler. 
        /// </summary>
        /// <param name="storageName"> Unique handler for storage name</param>
        /// <param name="lat"> Latitude of source point</param>
        /// <param name="lng"> Longitude of source point</param>
        /// <param name="distanceInKM"> Range in KM </param>
        /// <returns>Returns the target points in ascending order of distance from source point.</returns>
        /// <exception>App42Exception</exception>

        public Geo GetNearByPointsByMaxDistance(String storageName,
                Double lat, Double lng, Double distanceInKM)
        {
            String response = null;
            Geo geoObj = null;
            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            paramsDics.Add("storageName", storageName);
            paramsDics.Add("lat", "" + lat);
            paramsDics.Add("lng", "" + lng);
            paramsDics.Add("distanceInKM", "" + distanceInKM);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/getNearByPoints" + "/storageName" + "/" + storageName
                    + "/lat" + "/" + lat + "/lng" + "/" + lng + "/distanceInKM" + "/" + distanceInKM;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }
        /// <summary>
        /// Search the near by point from specified source point with in specified radius. 
        /// Points to be searched should already be stored on cloud using unique storage name handler.
        /// </summary>
        /// <param name="storageName"> Unique handler for storage name</param>
        /// <param name="lat">Lattitude of source point</param>
        /// <param name="lng"> Longitude of source point</param>
        /// <param name="radiusInKM">Radius in KM</param>
        /// <param name="resultLimit"> Maximum number of results to be retrieved </param>
        /// <returns>Geo object containing the target points in ascending order of distance from source point.</returns>
        /// <exception>App42Exception</exception>

        public Geo GetPointsWithInCircle(String storageName, Double lat,
                Double lng, Double radiusInKM, int resultLimit)
        {
            String response = null;
            Geo geoObj = null;
            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);

            paramsDics.Add("storageName", storageName);
            paramsDics.Add("lat", "" + lat);
            paramsDics.Add("lng", "" + lng);
            paramsDics.Add("resultLimit", "" + resultLimit);
            paramsDics.Add("radiusInKM", "" + radiusInKM);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/getPointsWithInCircle" + "/storageName" + "/"
                     + storageName + "/lat" + "/" + lat + "/lng"
                     + "/" + lng + "/radiusInKM" + "/"
                     + radiusInKM + "/limit" + "/" + resultLimit;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }
        /// <summary>
        /// Fetch the name of all storage stored on the cloud.
        /// </summary>
        /// <returns>Geo object containing IList of all the storage created</returns>
        /// <exception>App42Exception</exception>

        public IList<Geo> GetAllStorage()
        {
            String response = null;
            //Geo geoObj = null;
            IList<Geo> geoArray = null;
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            String signature = Util.Sign(this.secretKey, queryParams);
            String resourceURL = this.version + "/" + this.resource
                    + "/storage";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoArray = new GeoResponseBuilder().BuildArrayResponse(response);
            return geoArray;
        }
        /// <summary>
        /// Delete the specifed Geo Storage from Cloud.
        /// </summary>
        /// <param name="storageName"></param>
        /// <returns>Geo object containing the name of the storage that has been deleted</returns>
        /// <exception>App42Exception</exception>

        public App42Response DeleteStorage(String storageName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("storageName", storageName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/storage" + "/" + storageName;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Get All Point from storage.
        /// </summary>
        /// <param name="storageName"></param>
        /// <returns>Geo object containing all the stored Geo Points for the specified storage</returns>
        /// <exception>App42Exception</exception>

        public Geo GetAllPoints(String storageName)
        {
            String response = null;
            Geo geoObj = null;
            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("storageName", storageName);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/points"
                    + "/" + storageName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public IList<Geo> GetAllStorageByPaging(int max, int offset)
        {
            String response = null;
            IList<Geo> geoObjList = null;
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/paging" + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObjList = new GeoResponseBuilder().BuildArrayResponse(response);

            return geoObjList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geoStorageName"></param>
        /// <param name="geoPointsList"></param>
        /// <returns></returns>
        public Geo DeleteGeoPoints(String geoStorageName,
                IList<GeoPoint> geoPointsList)
        {
            String response = null;
            Geo geoObj = null;
            Util.ThrowExceptionIfNullOrBlank(geoStorageName, "Geo Storage Name");
            Util.ThrowExceptionIfNullOrBlank(geoPointsList, "Geo Points List");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();

            JArray geoArray = new JArray();
            foreach (GeoPoint geoPoint in geoPointsList)
            {
                geoArray.Add(JObject.Parse(""+geoPoint.GetJSONObject()));
            }
            jsonWriter.WritePropertyName("points");
            jsonWriter.WriteValue("{\"point\":"+ geoArray+"}");
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"geo\":{\"storage\":").Append(
                    sbJson.ToString()).Append("}}}");
            paramsDics.Add("geoPoints", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/points" + "/" + geoStorageName;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, paramsDics);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }
        public Geo GetAllPointsByPaging(String storageName, int max, int offset)
        {
            String response = null;
            Geo geoObj = null;
            Util.ThrowExceptionIfNullOrBlank(storageName, "Geo Storage Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("storageName", storageName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/paging" + "/points"
                    + "/" + storageName + "/" + max + "/" + offset; ;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            geoObj = new GeoResponseBuilder().BuildResponse(response);
            return geoObj;
        }
    }
}