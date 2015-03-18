using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.session
{

    /// <summary>
    /// The Session Manager manages user sessions on the server side. It is a persistent Session Manager.
    ///It allows to save attributes in the session and retrieve them.
    ///This Session Manager is especially useful for Mobile/Device Apps which want to do 
    ///session management.
    /// </summary>
    public class SessionManager
    {
        private String version = "1.0";
        private String resource = "session";
        private String apiKey;
        private String secretKey;


        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        public SessionManager(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }

        /// <summary>
        /// Create Session for the User. If the session does not exist it will create a new session
        /// </summary>
        /// <param name="uName">Username for which the session has to be created.</param>
        /// <returns>Returns the session id of the created session. This id has to be used for storing or
        /// retrieving attributes.</returns>
        public String GetSession(String uName)
        {
            String response = null;

            Util.ThrowExceptionIfNullOrBlank(uName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"session\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            return response;
        }


        /// <summary>
        /// Create User Session based on the isCreate boolean parameter.
        ///If isCreate is true and there is an existing session for the user, the existing session
        ///is returned. If there is no existing session for the user a new one is created.
        ///If isCreate is false and there is an existing session, the existing session is returned
        /// if there is no existing session new one is not created
        /// </summary>
        /// <param name="uName">Username for which the session has to be created.</param>
        /// <param name="isCreate">A boolean value for specifying if an existing session is not there, should a new one is to be created or not.</param>
        /// <returns>Returns the session id of the created session. This id has to be used for storing or retrieving attributes.</returns>
        public String GetSession(String uName, Boolean isCreate)
        {
            String response = null;

            Util.ThrowExceptionIfNullOrBlank(uName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(isCreate, "IsCreate");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"session\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + isCreate;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            return response;
        }

        /// <summary>
        /// Invalidate a session based on the session id. All the attributes store in the session will be lost.
        /// </summary>
        /// <param name="sessionId">The session id for which the session has to be invalidated.</param>
        /// <returns>the session id which is invalidated</returns>
        public App42Response Invalidate(String sessionId)
        {
            String response = null;
            Session sessionObj = null;
            Util.ThrowExceptionIfNullOrBlank(sessionId, "Session Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("id");
            jsonWriter.WriteValue(sessionId);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"session\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            sessionObj = new SessionResponseBuilder().BuildResponse(response);
            return sessionObj;
        }

        /// <summary>
        /// Sets attribute in a session whose session id is provided. Attributes are stored in a key value pair.
        /// </summary>
        /// <param name="sessionId">Session id for which the attribute has to be saved.</param>
        /// <param name="attributeName">The attribute key that needs to be stored</param>
        /// <param name="attributeValue">The attribute value that needs to be stored</param>
        /// <returns>the attribute and value which is stored</returns>
        public String SetAttribute(String sessionId, String attributeName,
                String attributeValue)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(sessionId, "Session ID");
            Util.ThrowExceptionIfNullOrBlank(attributeName, "Attribute Name");
            Util.ThrowExceptionIfNullOrBlank(attributeValue, "Attribute Value");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(attributeName);
            jsonWriter.WritePropertyName("value");
            jsonWriter.WriteValue(attributeValue);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"session\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            paramsDics.Add("sessionId", sessionId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/id/"
                    + sessionId;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            return response;
        }


        /// <summary>
        /// Gets the  attribute value in a session whose session id is provided. 
        /// </summary>
        /// <param name="sessionId">The session id for which the attribute has to be fetched</param>
        /// <param name="attributeName">The attribute key that has to be fetched</param>
        /// <returns>the attribute and value which is stored for the session id and attribute name</returns>
        public String GetAttribute(String sessionId, String attributeName)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(sessionId, "sessionId");
            Util.ThrowExceptionIfNullOrBlank(attributeName, "attributeName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("sessionId", sessionId);
            paramsDics.Add("name", attributeName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/id/"
                    + sessionId + "/" + attributeName;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            return response;
        }



        /// <summary>
        /// Gets all the attributes for a given session id
        /// </summary>
        /// <param name="sessionId">The session id for which the attribute has to be fetched</param>
        /// <returns>the attributes and values which are stored for the session id</returns>
        public String GetAllAttributes(String sessionId)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(sessionId, "Session Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("sessionId", sessionId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/id/"
                    + sessionId;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            return response;
        }


        /// <summary>
        /// Removes the  attribute from a session whose session id is provided.
        /// </summary>
        /// <param name="sessionId">The session id for which the attribute has to be removed</param>
        /// <param name="attributeName">The attribute key that has to be removed</param>
        /// <returns>the attribute and value which is removed for the session id and attribute name</returns>
        public App42Response RemoveAttribute(String sessionId, String attributeName)
        {
            String response = null;
            Session sessionObj = null;
            Util.ThrowExceptionIfNullOrBlank(sessionId, "Session Id");
            Util.ThrowExceptionIfNullOrBlank(attributeName, "Attribute Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("sessionId", sessionId);
            paramsDics.Add("name", attributeName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/id/"
                    + sessionId + "/" + attributeName;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            sessionObj = new SessionResponseBuilder().BuildResponse(response);
            return sessionObj;
        }



        /// <summary>
        /// Removes all the attributes for a given session id
        /// </summary>
        /// <param name="sessionId">The session id for which the attributes has to be removed</param>
        /// <returns>All the attributes and values which are removed for the session id</returns>
        public App42Response RemoveAllAttributes(String sessionId)
        {
            String response = null;
            Session sessionObj = null;

            Util.ThrowExceptionIfNullOrBlank(sessionId, "Session Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("sessionId", sessionId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/id/"
                    + sessionId;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            sessionObj = new SessionResponseBuilder().BuildResponse(response);
            return sessionObj;
        }

    }


}
