using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.pushNotification
{
    public sealed class DeviceType
    {
        public static readonly String ANDROID = "ANDROID";
        public static readonly String iOS = "iOS";
        public static readonly String WP7 = "WP7";
        public static Boolean IsValidType(String type)
        {
            if (ANDROID.Equals(type) || iOS.Equals(type) || WP7.Equals(type))
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
    /// The service is for pushing the notifications to any device using GCM(Google Cloud Messaging).
    /// You have to upload your apikey that you received while registering for GCM and you have to store your 
    /// device token with particular username. This service allows you the feature of sending message to 
    /// particular channel, particular user or to all your users.For sending message to any channel, you have to 
    /// create the channel and send the message to channel. The users which have subscribed to that channel will receive
    /// all the notification for that channel. For sending message to particular user, you have to pass username and 
    /// message. Notification will sent to the device of registered user. The most important feature you can send your message
    /// to all your device whether it is iphone, android or blackberry.  
    /// </summary>
    public class PushNotificationService
    {
        private String version = "1.0";
        private String resource = "push";
        private String apiKey;
        private String secretKey;

        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>
        public PushNotificationService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Stores your device token on server with particular username
        /// </summary>
        /// <param name="Username">Username with which you want your device to be registered</param>
        /// <param name="deviceToken">Device id for android phones</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification StoreDeviceToken(String Username, String deviceToken, String deviceType)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(Username, "Username");
            Util.ThrowExceptionIfNullOrBlank(deviceToken, "deviceToken");
            Util.ThrowExceptionIfNullOrBlank(deviceType, "deviceType");

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
            jsonWriter.WriteValue(Username);
            jsonWriter.WritePropertyName("deviceToken");
            jsonWriter.WriteValue(deviceToken);
            jsonWriter.WritePropertyName("type");
            jsonWriter.WriteValue(deviceType);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/storeDeviceToken/" + Username;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Create Channel for app on which user can subscribe and get the notification for that channel
        /// </summary>
        /// <param name="channel">Channel name which you want to create</param>
        /// <param name="description">Description for that channel</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification CreateChannelForApp(String channel, String description)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(channel, "Channel Name");
            Util.ThrowExceptionIfNullOrBlank(description, "Description");
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
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(channel);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(description);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":{\"channel\":").Append(sbJson.ToString())
            .Append("}}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/createAppChannel";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Subscribe to the channel
        /// </summary>
        /// <param name="channel">The channel name which you want to subscribe</param>
        /// <param name="userName">Username which want to subscribe</param>
        /// <returns>PushNotification Object</returns>

        public PushNotification SubscribeToChannel(String channel, String userName)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(channel, "Channel Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "userName");

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
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(channel);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"push\":{\"channel\":").Append(sbJson.ToString())
            .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/subscribeToChannel/" + userName;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Unsubscribe from particular channel
        /// </summary>
        /// <param name="channel">Channel name which you want to unsubscribe</param>
        /// <param name="userName">Username which want to unsubscribe</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification UnsubscribeFromChannel(String channel, String userName)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "Username");
            Util.ThrowExceptionIfNullOrBlank(channel, "channel");

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
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(channel);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"push\":{\"channel\":").Append(sbJson.ToString())
            .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/unsubscribeToChannel/" + userName;
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }

        /// <summary>
        /// </summary>
        /// <param name="map"></param>
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
                    sb.Append("\"" + key + "\"" + ":" + "\"" + value + "\"");
                    if (TotalCount > 1 && i != TotalCount)
                        sb.Append(",");
                }
            }

            return sb.ToString();
        }
        /// <summary>
        /// send push message to channel containing string
        /// </summary>
        /// <param name="channel">Channel name which you want to send the message</param>
        /// <param name="message">Push message in string format</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification SendPushMessageToChannel(String channel, String message)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(channel, "channel");
            Util.ThrowExceptionIfNullOrBlank(message, "message");

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
            jsonWriter.WritePropertyName("channel");
            jsonWriter.WriteValue(channel);
            jsonWriter.WritePropertyName("payload");
            jsonWriter.WriteValue(message);
            jsonWriter.WritePropertyName("expiry");
            jsonWriter.WriteValue(Util.GetUTCFormattedTimestamp());
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":{\"message\":").Append(sbJson.ToString())
                    .Append("}}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/sendPushMessageToChannel/" + channel;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Send push message to all your users 
        /// </summary>
        /// <param name="message">push message</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification SendPushMessageToAll(String message)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(message, "message");

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
            jsonWriter.WritePropertyName("payload");
            jsonWriter.WriteValue(message);
            jsonWriter.WritePropertyName("expiry");
            jsonWriter.WriteValue(Util.GetUTCFormattedTimestamp());
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":{\"message\":").Append(sbJson.ToString())
            .Append("}}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/sendPushMessageToAll";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Send push message to all your users 
        /// </summary>
        /// <param name="message">push message</param>
        /// <param name="type"></param>
        /// <returns>PushNotification Object</returns>
        public PushNotification SendPushMessageToAllByType(String message, String type)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(message, "message");
            Util.ThrowExceptionIfNullOrBlank(type, "type");

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
            jsonWriter.WritePropertyName("payload");
            jsonWriter.WriteValue(message);
            jsonWriter.WritePropertyName("type");
            jsonWriter.WriteValue(type);
            jsonWriter.WritePropertyName("expiry");
            jsonWriter.WriteValue(Util.GetUTCFormattedTimestamp());
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":{\"message\":").Append(sbJson.ToString())
            .Append("}}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/sendMessageToAllByType";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
        /// <summary>
        /// Send Push Message To paticular user in string format
        /// </summary>
        /// <param name="username">Username which you want to send the message</param>
        /// <param name="message">Push message</param>
        /// <returns>PushNotification Object</returns>
        public PushNotification SendPushMessageToUser(String username, String message)
        {
            String response = null;
            PushNotification pushNotification = null;
            Util.ThrowExceptionIfNullOrBlank(username, "Username");
            Util.ThrowExceptionIfNullOrBlank(message, "message");
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
            jsonWriter.WriteValue(username);
            jsonWriter.WritePropertyName("payload");
            jsonWriter.WriteValue(message);
            jsonWriter.WritePropertyName("expiry");
            jsonWriter.WriteValue(Util.GetUTCFormattedTimestamp());
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"push\":{\"message\":").Append(sbJson.ToString())
            .Append("}}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/sendMessage/" + username;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            pushNotification = new PushNotificationResponseBuilder().BuildResponse(response);
            return pushNotification;
        }
    }
}