using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp.util;
using com.shephertz.app42.paas.sdk.csharp.connection;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp.storage;
using com.shephertz.app42.paas.sdk.csharp;


namespace com.shephertz.app42.paas.sdk.csharp.storage
{
    //public sealed class Operator
    //{
    //    public static readonly String EQUALS = "$eq";
    //    public static readonly String NOT_EQUALS = "$ne";
    //    public static readonly String GREATER_THAN = "$gt";
    //    public static readonly String LESS_THAN = "$lt";
    //    public static readonly String GREATER_THAN_EQUALTO = "$gte";
    //    public static readonly String LESS_THAN_EQUALTO = "$lte";
    //    public static readonly String LIKE = "$lk";
    //    public static readonly String AND = "$and";
    //    public static readonly String OR = "$or";
    //    private String value;
    //    private Operator(String value)
    //    {
    //        this.value = value;
    //    }

    //    public String GetValue()
    //    {
    //        return value;
    //    }
    //}
    public sealed class OrderByType
    {
        public static readonly String ASCENDING = "ASCENDING";
        public static readonly String DESCENDING = "DESCENDING";


        public static Boolean isValidType(String type)
        {
            if (ASCENDING.Equals(type) || DESCENDING.Equals(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    /// <summary>
    /// Storage service on cloud provides the way to store the JSON document in NoSQL database running on cloud.
    /// One can store the JSON document, update it , serach it and can apply the map-reduce search on stored documents.
    /// Example : If one will store JSON doc {"date":"5Feb"} it will be stored with unique Object Id and stored JSON object will look like
    /// { "date" : "5Feb" , "_id" : { "$old" : "4f423dcce1603b3f0bd560cf"}}. This old can further be used to access/search the document.
    /// </summary>
    /// <see cref="StorageService">Storage</see>
    /// <see cref="StorageService">App42Response</see>
    public class StorageService
    {
        private String version = "1.0";
        private String resource = "storage";
        private String apiKey;
        private String secretKey;

        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name = "apiKey">apiKey</param>
        /// <param name = "secretKey">secretKey </param>


        public StorageService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Save the JSON document in given database name and collection name.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name.</param>
        /// <param name = "collectionName">Name of collection under which JSON doc has to be saved.</param>
        /// <param name = "json">Target JSON document to be saved.</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage InsertJSONDocument(String dbName, String collectionName, String json)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(json, "Json");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("jsonDoc");
            jsonWriter.WriteValue(json);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"storage\":").Append(sbJson.ToString())
                .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/insert/dbName/" + dbName + "/collectionName/"
                + collectionName;
            response = RESTConnector.getInstance().ExecutePost(signature,
                resourceURL, queryParams, sb.ToString());
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find all documents stored in given database and collection.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name.</param>
        /// <param name = "collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <returns>Storage object.</returns> 
        /// <exception>App42Exception</exception>
        public Storage FindAllDocuments(String dbName, String collectionName)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);


            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                + "/findAll/dbName/" + dbName + "/collectionName/"
                + collectionName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find target document by given unique object id.
        /// </summary>
        /// <param name = "dbName"> Unique handler for storage name.</param>
        /// <param name = "collectionName"> Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "docId">Unique Object Id handler.</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage FindDocumentById(String dbName, String collectionName, String docId)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(docId, "DocumentId");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("docId", docId);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                        + "/findDocById/dbName/" + dbName + "/collectionName/"
                        + collectionName + "/docId/" + docId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find target document using key value search parameter. This key value pair will be searched 
        /// in the JSON doc stored on the cloud and matching Doc will be returned as a result of this method.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name.</param>
        /// <param name = "collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "key">Key to be searched for target JSON doc.</param>
        /// <param name  "value">Value to be searched for target JSON doc.</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage FindDocumentByKeyValue(String dbName, String collectionName, String key, String value)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(key, "Key");
            Util.ThrowExceptionIfNullOrBlank(value, "Value");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("key", key);
            paramsDics.Add("value", value);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                        + "/findDocByKV/dbName/" + dbName + "/collectionName/"
                        + collectionName + "/" + key + "/" + value;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Update target document using key value search parameter. This key value pair will be searched 
        /// in the JSON doc stored in the cloud and matching Doc will be updated with new value passed.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name.</param>
        /// <param name = "collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "key" Key to be searched for target JSON doc.</param>
        /// <param name = "value">Value to be searched for  target JSON doc.</param>
        /// <param namde = "newJsonDoc">New Json document to be added.</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage UpdateDocumentByKeyValue(String dbName, String collectionName, String key, String value, String newJsonDoc)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(key, "Key");
            Util.ThrowExceptionIfNullOrBlank(value, "Value");
            Util.ThrowExceptionIfNullOrBlank(newJsonDoc, "NewJsonDocument");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("key", key);
            paramsDics.Add("value", value);
            // Construct a json body for create user
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("jsonDoc");
            jsonWriter.WriteValue(newJsonDoc);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"storage\":").Append(sbJson.ToString())
                .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/update/dbName/" + dbName + "/collectionName/"
                        + collectionName + "/" + key + "/" + value;
            response = RESTConnector.getInstance().ExecutePut(signature,
                resourceURL, queryParams, sb.ToString());
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Update target document using the document id.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name.</param>
        /// <param name = "collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "docId">Id of the document to be searched for target JSON doc.</param>
        /// <param namde = "newJsonDoc">New Json document to be added.</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage UpdateDocumentByDocId(String dbName, String collectionName, String docId, String newJsonDoc)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(docId, "DocId");
            Util.ThrowExceptionIfNullOrBlank(newJsonDoc, "NewJsonDocument");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("docId", docId);
            // Construct a json body for create user
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("jsonDoc");
            jsonWriter.WriteValue(newJsonDoc);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"storage\":").Append(sbJson.ToString())
                .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/updateByDocId/dbName/" + dbName + "/collectionName/"
                        + collectionName + "/docId/" + docId;
            response = RESTConnector.getInstance().ExecutePut(signature,
                resourceURL, queryParams, sb.ToString());
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Delete target document using Object Id from given db and collection. The Object Id will be 
        /// searched in the JSON doc stored on the cloud and matching Doc will be deleted.
        /// <param name = "dbName"> Unique handler for storage name</param>
        /// <param name = "collectionName"> Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "docId">  Unique Object Id handler.</param>
        /// <returns>App42Response object if deleted successfully.</returns> 
        /// <exception>App42Exception</exception>
        public App42Response DeleteDocumentById(String dbName, String collectionName, String docId)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(docId, "DocumentId");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("docId", docId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/deleteDocById/dbName/" + dbName + "/collectionName/"
                        + collectionName + "/docId/" + docId;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// </summary>
        /// <param name="map">Map</param>
        /// <returns></returns>
        private String GetJsonFromMap(Dictionary<String, String> map)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            if (map != null && map.Count() != 0)
            {
                ICollection<String> keySet = map.Keys;

                int i = 0;
                int TotalCount = keySet.Count();
                foreach (string key in keySet)
                {
                    i++;
                    String value = map[key];
                    App42Log.Debug("VALUE" + value);
                    sb.Append("\"" + key + "\"" + ":" + "\"" + value + "\"");
                    if (TotalCount > 1 && i != TotalCount)
                        sb.Append(",");

                }
            }
            sb.Append("}");

            return sb.ToString();
        }
        /// <summmary>
        /// Save the JSON document in given database name and collection name. It accepts the HashMap containing key-value and convert 
        /// it into JSON.Converted JSON doc further saved on the cloud using given db name and collection name.
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name</param>
        /// <param name = "collectionName">Name of collection under which JSON doc has to be saved.</param>
        /// <param name = "map">HashMap containing key-value pairs</param>
        /// <returns>Storage object</returns> 
        /// <exception>App42Exception</exception>
        public Storage InsertJsonDocUsingMap(String dbName, String collectionName, Dictionary<String, String> map)
        {
            String JsonBody = GetJsonFromMap(map);
            return InsertJSONDocument(dbName, collectionName, JsonBody);
        }
        /// <summary>
        /// Map reduce function to search the target document. Please see detail information 
        /// on map-reduce http://en.wikipedia.org/wiki/MapReduce
        /// </summary>
        /// <param name = "dbName">Unique handler for storage name</param>
        /// <param name = "collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <param name = "mapFunction">Map function to be used to search the document</param>
        /// <param name = "reduceFunction">Reduce function to be used to search the document</param>
        /// <returns>The target JSON document.</returns>
        /// <exception>App42Exception</exception>
        public String MapReduce(String dbName, String collectionName, String mapFunction, String reduceFunction)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(mapFunction, "MapFunction");
            Util.ThrowExceptionIfNullOrBlank(reduceFunction, "ReduceFunction");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);


            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("map");
            jsonWriter.WriteValue(mapFunction);
            jsonWriter.WritePropertyName("reduce");
            jsonWriter.WriteValue(reduceFunction);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"storage\":").Append(sbJson.ToString())
                .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                        + "/mapReduce/dbName/" + dbName + "/collectionName/"
                        + collectionName;
            response = RESTConnector.getInstance().ExecutePost(signature,
                resourceURL, queryParams, sb.ToString());

            return response;
        }
        /// <summary>
        /// Gets the count of all documents stored in given database and collection.
        /// </summary>
        /// <param name="dbName">Unique handler for storage name.</param>
        /// <param name="collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <returns>App42Response object</returns>
        /// <exception>App42Exception</exception>
        public App42Response FindAllDocumentsCount(String dbName, String collectionName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/findAll/count/dbName/" + dbName + "/collectionName/"
                    + collectionName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new StorageResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Find all documents stored in given database and collection.
        /// </summary>
        /// <param name="dbName">Unique handler for storage name.</param>
        /// <param name="collectionName">Name of collection under which JSON doc needs to be searched.</param>
        /// <param name="max">Maximum number of records to be fetched.</param>
        /// <param name="offset">From where the records are to be fetched.</param>
        /// <returns>Storage object</returns>
        /// <exception>App42Exception</exception>.
        public Storage FindAllDocuments(String dbName, String collectionName, int max, int offset)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/findAll/dbName/" + dbName + "/collectionName/"
                    + collectionName + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find target document using Custom Query.
        /// </summary>
        /// <param name="dbName">Unique handler for storage name</param>
        /// <param name="collectionName">Name of collection under which JSON doc needs to be searched</param>
        /// <param name="query">Query Object containing custom query for searching docs</param>
        /// <returns>Storage object</returns>
        /// <exception>App42Exception</exception>
        public Storage FindDocumentsByQuery(String dbName, String collectionName,
                Query query)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(query, "query");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            paramsDics.Add("jsonQuery", query.GetStr());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
           
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/findDocsByQuery/dbName/" + dbName + "/collectionName/"
                    + collectionName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find target document using Custom Query with paging.
        /// </summary>
        /// <param name="dbName">Unique handler for storage name</param>
        /// <param name="collectionName">Name of collection under which JSON doc needs to be searched</param>
        /// <param name="query">Query Object containing custom query for searching docs</param>
        /// <param name="max">Max result parameter</param>
        /// <param name="offset">Offset result parameter</param>
        /// <returns>Storage object</returns>
        /// <exception>App42Exception</exception>
        public Storage FindDocumentsByQueryWithPaging(String dbName, String collectionName,
                Query query, int max, int offset)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(query, "query");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            paramsDics.Add("jsonQuery", query.GetStr());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/findDocsByQuery/dbName/" + dbName + "/collectionName/"
                    + collectionName + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
        /// <summary>
        /// Find target document using Custom Query with paging and orderby.
        /// </summary>
        /// <param name="dbName">Unique handler for storage name</param>
        /// <param name="collectionName">Name of collection under which JSON doc needs to be searched</param>
        /// <param name="query">Query Object containing custom query for searching docs</param>
        /// <param name="max">Max result parameter</param>
        /// <param name="offset">Offset result parameter</param>
        /// <param name="orderByKey"></param>
        /// <param name="type"></param>
        /// <returns>Storage object</returns>
        /// <exception>App42Exception</exception>
        public Storage FindDocsWithQueryPagingOrderBy(String dbName, String collectionName,
                Query query, int max, int offset, String orderByKey, String type)
        {
            String response = null;
            Storage storage = null;
            Util.ThrowExceptionIfNullOrBlank(dbName, "DataBaseName");
            Util.ThrowExceptionIfNullOrBlank(collectionName, "CollectionName");
            Util.ThrowExceptionIfNullOrBlank(query, "query");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            paramsDics.Add("jsonQuery", query.GetStr());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            if (orderByKey != null)
                queryParams.Add("orderByKey", orderByKey);
            if (type != null)
                queryParams.Add("orderByType", OrderByType.ASCENDING);

            paramsDics.Add("dbName", dbName);
            paramsDics.Add("collectionName", collectionName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/findDocsByQuery/dbName/" + dbName + "/collectionName/"
                    + collectionName + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            storage = new StorageResponseBuilder().BuildResponse(response);
            return storage;
        }
    }
}