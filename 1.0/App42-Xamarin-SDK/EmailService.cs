using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.email
{
    /// <summary>
    /// Service to send Email 
    /// </summary>
    /// <see cref="EmailService">Email</see>

    public sealed class EmailMIME
    {
        public static readonly String PLAIN_TEXT_MIME_TYPE = "text/plain";
        public static readonly String HTML_TEXT_MIME_TYPE = "text/html";


        public static Boolean isValidType(String type)
        {
            if (PLAIN_TEXT_MIME_TYPE.Equals(type) || HTML_TEXT_MIME_TYPE.Equals(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    public class EmailService
    {
        private String version = "1.0";
        private String resource = "email";
        private String apiKey;
        private String secretKey;
        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey">This is api key.</param>
        /// <param name="secretKey">This is secretKey</param>

        public EmailService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }

        /// <summary> 
        /// Creates Email Configuration using which in future the App developer can send mail
        /// </summary>
        /// <param name = "emailHost"> Email Host to be used for sending mail</param>
        /// <param name = "emailPort"> Email Port to be used for sending mail</param>
        /// <param name = "mailId"> Email id to be used for sending mail</param>
        /// <param name = "emailPassword"> Email Password to be used for sending mail</param>
        /// <param name = "isSSL"> Should be send using SSL or not</param>
        /// <returns>Email object containing the email configuration which has been created</returns>
        /// <exception>App42Exception</exception>


        public Email CreateMailConfiguration(String emailHost, Int64 emailPort, String mailId, String emailPassword, Boolean isSSL)
        {
            String response = null;
            Email emailObj = null;

            Util.ThrowExceptionIfNullOrBlank(emailHost, "Host");
            Util.ThrowExceptionIfNullOrBlank(emailPort, "Port");
            Util.ThrowExceptionIfNullOrBlank(mailId, "Email Id");
            Util.ThrowExceptionIfEmailNotValid(mailId, "Email Id");
            Util.ThrowExceptionIfNullOrBlank(emailPassword, "Password");
            Util.ThrowExceptionIfNullOrBlank(isSSL, "isSSL");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",

            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);


            /// Construct a json body for Charge User
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("host");
            jsonWriter.WriteValue(emailHost);
            jsonWriter.WritePropertyName("port");
            jsonWriter.WriteValue(emailPort);
            jsonWriter.WritePropertyName("emailId");
            jsonWriter.WriteValue(mailId);
            jsonWriter.WritePropertyName("password");
            jsonWriter.WriteValue(emailPassword);
            jsonWriter.WritePropertyName("ssl");
            jsonWriter.WriteValue(isSSL);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"email\":").Append(sbJson.ToString())
                .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/configuration";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            emailObj = new EmailResponseBuilder().BuildResponse(response);
            return emailObj;
        }

        /// <summary>
        /// Removes email configuration for the given email id. Note: In future the developer
        /// wont be able to send mails through this id
        /// </summary>
        /// <param name = "emailId"> The email id for which the configuration has to be removed</param>
        /// <returns>Email object containing the email id which has been removed</returns>
        /// <exception>App42Exception</exception>

        public App42Response RemoveEmailConfiguration(String emailId)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(emailId, "Email Id");
            Util.ThrowExceptionIfEmailNotValid(emailId, "Email Id");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("emailId", emailId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                + "/configuration" + "/" + emailId;

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }

        /// <summary> 
        /// Gets all Email Configurations for the app
        /// </summary>
        /// <returns>Email object containing all Email Configurations</returns>
        /// <exception>App42Exception</exception>

        public Email GetEmailConfigurations()
        {
            String response = null;
            Email emailObj = null;

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                + "/configuration";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                resourceURL, queryParams);

            emailObj = new EmailResponseBuilder().BuildResponse(response);
            return emailObj;
        }


        /// <summay>
        /// Sends the Email to the specified recipient with the provided detail
        /// </summary>
        /// <param name ="fromEmail"> The Email Id using which the mail(s) has to be sent</param>
        /// <param name = "sendTo"> 
        /// The email ids to which the email has to be sent. Email can be sent to multiple email ids.
        /// Multiple email ids can be passed using comma as the seperator e.g. sid@shephertz.com, info@shephertz.com
        /// </param>
        /// <param name = "sendSubject"> Subject of the Email which has to be sent</param>
        /// <param name = "sendMsg"> Email body which has to be sent</param>
        /// <param name = "emailMIME"> MIME Type to be used for sending mail. EmailMIME available options are PLAIN_TEXT_MIME_TYPE or HTML_TEXT_MIME_TYPE</param>/
        /// <returns>Email object containing all the details used for sending mail</returns>
        /// <exception>App42Exception</exception>

        public Email SendMail(String sendTo, String sendSubject, String sendMsg, String fromEmail, String emailMime)
        {
            String response = null;
            Email emailObj = null;
            Util.ThrowExceptionIfNullOrBlank(sendTo, "Send To");
            Util.ThrowExceptionIfNullOrBlank(sendSubject, "Send Subject");
            Util.ThrowExceptionIfNullOrBlank(sendMsg, "Send Message");
            Util.ThrowExceptionIfNullOrBlank(fromEmail, "From Email");
            Util.ThrowExceptionIfNullOrBlank(emailMime, "emailMime");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);


            // Construct a json body for send mail
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("to");
            jsonWriter.WriteValue(sendTo);
            jsonWriter.WritePropertyName("subject");
            jsonWriter.WriteValue(sendSubject);
            jsonWriter.WritePropertyName("msg");
            jsonWriter.WriteValue(sendMsg);
            jsonWriter.WritePropertyName("emailId");
            jsonWriter.WriteValue(fromEmail);
            jsonWriter.WritePropertyName("mimeType");
            jsonWriter.WriteValue(emailMime);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"email\":").Append(sbJson.ToString())
                .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                resourceURL, queryParams, sb.ToString());
            emailObj = new EmailResponseBuilder().BuildResponse(response);
            return emailObj;
        }
    }
}

