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

namespace com.shephertz.app42.paas.sdk.csharp.message
{
    /// <summary>
    /// Manages Asynchronous queues. Allows to create, delete, purge messages, view pending messages and
    /// get all messages
    /// </summary>
    /// <see cref="QueueService">Queue</see>

    public class QueueService
    {
        private String version = "1.0";
        private String resource = "queue";
        private String messageResource = "message";
        private String apiKey;
        private String secretKey;
        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>
        public QueueService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Creates a type Pull Queue
        /// </summary>
        /// <param name="queueName"> The name of the queue which has to be created</param>
        /// <param name="queueDescription"> The description of the queue</param>
        /// <returns>Queue object containing queue name which has been created </returns>
        /// <exception>App42Exception</exception>   

        public Queue CreatePullQueue(String queueName, String queueDescription)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Util.ThrowExceptionIfNullOrBlank(queueDescription, "Queue Description");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", "pull");
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(queueName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(queueDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"queue\":").Append(sbJson).Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/pull";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Deletes the Pull type Queue
        /// </summary>
        /// <param name="queueName"> The name of the queue which has to be deleted</param>
        /// <returns>App42Response if deleted successfully </returns>
        /// <exception>App42Exception</exception>   

        public App42Response DeletePullQueue(String queueName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", "pull");
            paramsDics.Add("queueName", queueName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/pull"
                    + "/" + queueName;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                       resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Purges message on the Queue. Note: once the Queue is purged the messages
        /// are removed from the Queue and wont be available for dequeueing.
        /// </summary>
        /// <param name="queueName"> The name of the queue which has to be purged</param>
        /// <returns>Queue object containing queue name which has been purged </returns>
        /// <exception>App42Exception</exception>     
        public App42Response PurgePullQueue(String queueName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            paramsDics.Add("type", "pull");
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/pull"
                    + "/purge" + "/" + queueName;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                       resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Messages which are pending to be dequeue. Note: Calling this method does not
        /// </summary>
        /// <param name="queueName"> The name of the queue from which pending messages have to be fetched</param>
        /// <returns>Queue object containing pending messages in the Queue</returns>
        /// <exception>App42Exception</exception>    
        public Queue PendingMessages(String queueName)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/pending" + "/" + queueName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Messages are retrieved and dequeued from the Queue. 
        /// </summary>
        /// <param name="queueName"> The name of the queue which have to be retrieved</param>
        /// <param name="receiveTimeOut"> Receive time out</param>
        /// <returns>Queue object containing messages in the Queue</returns>
        /// <exception>App42Exception</exception>   
        public Queue GetMessages(String queueName, Int64 receiveTimeOut)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            paramsDics.Add("receiveTimeOut", "" + receiveTimeOut);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/messages" + "/" + queueName + "/" + receiveTimeOut;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Message Service Method Start
        /// Send message on the queue with an expiry. The message will expire if it is not pulled/dequeued before the expiry
        /// </summary>
        /// <param name="queueName"> The name of the queue to which the message has to be sent</param>
        /// <param name="msg"> Message that has to be sent</param>
        /// <param name="exp"> Message expiry time</param>
        /// <returns>Queue object containing message which has been sent with its message id and correlation id</returns>
        /// <exception>App42Exception</exception>    
        public Queue SendMessage(String queueName, String msg, Int64 exp)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Util.ThrowExceptionIfNullOrBlank(msg, "Message");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            // Construct a json body for Send Message
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("message");
            jsonWriter.WriteValue(msg);
            jsonWriter.WritePropertyName("expiration");
            jsonWriter.WriteValue(exp);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"payLoad\":").Append(sbJson).Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.messageResource + "/"
                    + queueName;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Pulls all the message from the queue
        /// </summary>
        /// <param name="queueName"> The name of the queue from which messages have to be pulled</param>
        /// <param name="receiveTimeOut"> Receive time out</param>
        /// <returns>Queue object containing  messages which have been pulled</returns>
        /// <exception>App42Exception</exception>   
        public Queue ReceiveMessage(String queueName, Int64 receiveTimeOut)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            paramsDics.Add("receiveTimeOut", "" + receiveTimeOut);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.messageResource + "/"
                    + queueName + "/" + receiveTimeOut;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Pull message based on the correlation id
        /// </summary>
        /// <param name="queueName"> The name of the queue from which the message has to be pulled</param>
        /// <param name="receiveTimeOut"> Receive time out</param>
        /// <param name="correlationId"> Correlation Id for which message has to be pulled</param>
        /// <returns>Queue containing  message which has pulled based on the correlation id</returns>
        /// <exception>App42Exception</exception>   
        public Queue ReceiveMessageByCorrelationId(String queueName,
            Int64 receiveTimeOut, String correlationId)
        {
            String response = null;
            Queue queue = null;
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Util.ThrowExceptionIfNullOrBlank(correlationId, "Correlation Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            paramsDics.Add("receiveTimeOut", "" + receiveTimeOut);
            paramsDics.Add("correlationId", "" + correlationId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.messageResource + "/"
                    + queueName + "/" + receiveTimeOut + "/" + correlationId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            queue = new QueueResponseBuilder().BuildResponse(response);
            return queue;
        }
        /// <summary>
        /// Remove message from the queue based on the message id. Note: Once the message is removed it cannot be pulle
        /// </summary>
        /// <param name="queueName"> The name of the queue from which the message has to be removed</param>
        /// <param name="messageId"> The message id of the message which has to be removed.</param>
        /// <returns>App42Response if removed successfully</returns>
        /// <exception>App42Exception</exception>    

        public App42Response RemoveMessage(String queueName, String messageId)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(queueName, "Queue Name");
            Util.ThrowExceptionIfNullOrBlank(messageId, "Message Id");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("queueName", queueName);
            paramsDics.Add("messageId", messageId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.messageResource + "/"
                    + queueName + "/" + messageId;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                       resourceURL, queryParams);
			responseObj.SetStrResponse(response);
			responseObj.SetResponseSuccess(true);
		return responseObj;
        }
    }
}
