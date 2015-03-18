using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using com.shephertz.app42.paas.sdk.csharp.connection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.social
{
    /// <summary>
    /// Connect to the User's multiple social accounts. Also used to update the
    /// status individually or all at once for the linked social accounts.
    /// </summary>
    public class SocialService
    {
        private Config config;
        private String version = "1.0";
        private String resource = "social";
        private String apiKey;
        private String secretKey;
        private String baseURL;

        /// <summary>
        /// The constructor for the Service
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>

        public SocialService(String apiKey, String secretKey, String baseURL)
        {
            config = Config.GetInstance();
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }
        /// <summary>
        /// Links the User Facebook access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose Facebook account to be linked.</param>
        /// <param name="accessToken">Facebook Access Token that has been received after authorization.</param>
        /// <param name="appId">Facebook App Id.</param>
        /// <param name="appSecret">Facebook App Secret.</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserFacebookAccount(String userName, String accessToken, String appId, String appSecret)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(appId, "appId");
            Util.ThrowExceptionIfNullOrBlank(appSecret, "appSecret");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WritePropertyName("appId");
            jsonWriter.WriteValue(appId);
            jsonWriter.WritePropertyName("appSecret");
            jsonWriter.WriteValue(appSecret);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/facebook/linkuser";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Links the User Facebook access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose Facebook account to be linked</param>
        /// <param name="accessToken">Facebook Access Token that has been received after authorisation</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserFacebookAccount(String userName, String accessToken)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/facebook/linkuser/accesscredentials";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Updates the Facebook status of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user for whom the status needs to be updated.</param>
        /// <param name="status">Status that has to be updated.</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social UpdateFacebookStatus(String userName, String status)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(status, "status");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(status);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/facebook/updatestatus";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Links the User Twitter access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose Twitter account to be linked.</param>
        /// <param name="accessToken">Twitter Access Token that has been received after authorisation.</param>
        /// <param name="accessTokenSecret">Twitter Access Token Secret that has been received after authorisation.</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserTwitterAccount(String userName, String accessToken, String accessTokenSecret)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
            Util.ThrowExceptionIfNullOrBlank(accessTokenSecret, "accessTokenSecret");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WritePropertyName("accessTokenSecret");
            jsonWriter.WriteValue(accessTokenSecret);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/twitter/linkuser/accesscredentials";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Links the User Twitter access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose Twitter account to be linked</param>
        /// <param name="accessToken">Twitter Access Token that has been received after authorisation</param>
        /// <param name="accessTokenSecret">Twitter Access Token Secret that has been received after authorisation</param>
        /// <param name="consumerKey">Twitter App Consumer Key</param>
        /// <param name="consumerSecret">Twitter App Consumer Secret</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserTwitterAccount(String userName, String accessToken, String accessTokenSecret, String consumerKey,
                String consumerSecret)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(consumerKey, "consumerKey");
            Util.ThrowExceptionIfNullOrBlank(consumerSecret, "consumerSecret");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
            Util.ThrowExceptionIfNullOrBlank(accessTokenSecret, "accessTokenSecret");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("consumerKey");
            jsonWriter.WriteValue(consumerKey);
            jsonWriter.WritePropertyName("consumerSecret");
            jsonWriter.WriteValue(consumerSecret);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WritePropertyName("accessTokenSecret");
            jsonWriter.WriteValue(accessTokenSecret);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString()).Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/twitter/linkuser";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Updates the Twitter status of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user for whom the status needs to be updated</param>
        /// <param name="status">Status that has to be updated</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social UpdateTwitterStatus(String userName, String status)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(status, "status");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(status);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString()).Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/twitter/updatestatus";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Links the User LinkedIn access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose LinkedIn account to be linked.</param>
        /// <param name="apiKey">LinkedIn App API Key</param>
        /// <param name="secretKey">LinkedIn App Secret Key</param>
        /// <param name="accessToken">LinkedIn Access Token that has been received after authorisation.</param>
        /// <param name="accessTokenSecret">LinkedIn Access Token Secret that has been received after authorisation.</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserLinkedInAccount(String userName, String accessToken, String accessTokenSecret, String apiKey, String secretKey)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
            Util.ThrowExceptionIfNullOrBlank(accessTokenSecret, "accessTokenSecret");
            Util.ThrowExceptionIfNullOrBlank(apiKey, "apiKey");
            Util.ThrowExceptionIfNullOrBlank(secretKey, "secretKey");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("apiKey");
            jsonWriter.WriteValue(apiKey);
            jsonWriter.WritePropertyName("secretKey");
            jsonWriter.WriteValue(secretKey);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WritePropertyName("accessTokenSecret");
            jsonWriter.WriteValue(accessTokenSecret);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/linkedin/linkuser";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Links the User LinkedIn access credentials to the App User account.
        /// </summary>
        /// <param name="userName">Name of the user whose LinkedIn account to be linked</param>
        /// <param name="accessToken">LinkedIn Access Token that has been received after authorisation</param>
        /// <param name="accessTokenSecret">LinkedIn Access Token Secret that has been received after authorisation</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social LinkUserLinkedInAccount(String userName, String accessToken, String accessTokenSecret)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(accessToken, "accessToken");
            Util.ThrowExceptionIfNullOrBlank(accessTokenSecret, "accessTokenSecret");
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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("accessToken");
            jsonWriter.WriteValue(accessToken);
            jsonWriter.WritePropertyName("accessTokenSecret");
            jsonWriter.WriteValue(accessTokenSecret);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/linkedin/linkuser/accesscredentials";

            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Updates the LinkedIn status of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user for whom the status needs to be updated</param>
        /// <param name="status">status that has to be updated</param>
        /// <returns>The Social object</returns>
        /// <exception>App42Exception</exception>
        public Social UpdateLinkedInStatus(String userName, String status)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(status, "status");

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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(status);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/linkedin/updatestatus";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// Updates the status for all linked social accounts of the specified user.
        /// </summary>
        /// <param name="userName">Name of the user for whom the status needs to be updated.</param>
        /// <param name="status">status that has to be updated.</param>
        /// <returns>The Social object</returns>
        public Social UpdateSocialStatusForAll(String userName, String status)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");
            Util.ThrowExceptionIfNullOrBlank(status, "status");

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
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(status);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"social\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/social/updatestatus/all";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Social GetFacebookFriendsFromLinkUser(String userName)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "facebook/friends/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public Social GetFacebookFriendsFromAccessToken(String accessToken)
        {
            String response = null;
            Social social = null;
            Util.ThrowExceptionIfNullOrBlank(accessToken, "AccessToken");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("accessToken", accessToken);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "facebook/friends/OAuth/" + accessToken;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            social = new SocialResponseBuilder().BuildResponse(response);
            return social;
        }
    }
}