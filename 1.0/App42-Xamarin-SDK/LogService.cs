using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;

namespace com.shephertz.app42.paas.sdk.csharp.log
{
    /// <summary>
    /// Centralize logging for your App. This service allows different levels e.g. info,
    /// debug, fatal, error etc. to log a message and query the messages based on 
    /// different parameters.
    /// You can fetch logs based on module, level, message, date range etc.
    /// </summary>
    /// <see cref="LogService">Log</see>

    public class LogService
    {
        private String version = "1.0";
        private String resource = "log";
        private String apiKey;
        private String secretKey;
        /// <summary>
        /// The costructor for the Service
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>


        public LogService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        private Log BuildAndSend(String msg, String module, String level)
        {
            String response = null;
            Log logObj = null;
            Util.ThrowExceptionIfNullOrBlank(msg, "Message");
            Util.ThrowExceptionIfNullOrBlank(module, "App Module");
            Util.ThrowExceptionIfNullOrBlank(level, "Level");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            // Construct a json body 
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("message");
            jsonWriter.WriteValue(msg);
            jsonWriter.WritePropertyName("appModule");
            jsonWriter.WriteValue(module);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"log\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + level;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Logs the info message 
        /// </summary>
        /// <param name="msg"> Message to be logged</param>
        /// <param name="module">Module name for which the message is getting logged</param>
        /// <returns>Log object containing logged message</returns>
        /// <exception>App42Exception</exception>   

        public Log Info(String msg, String module)
        {
            return BuildAndSend(msg, module, "info");
        }
        /// <summary>
        /// Logs the debug message
        /// </summary>
        /// <param name="msg">Message to be logged</param>
        /// <param name="module">Module name for which the message is getting logged</param>
        /// <returns>Log object containing logged message</returns>
        /// <exception>App42Exception</exception>   

        public Log Debug(String msg, String module)
        {
            return BuildAndSend(msg, module, "debug");
        }
        /// <summary>
        /// Logs the fatal message
        /// </summary>
        /// <param name="msg"> Message to be logged</param>
        /// <param name="module"> Module name for which the message is getting logged</param>
        /// <returns>Log object containing logged message</returns>
        /// <exception>App42Exception</exception>   

        public Log Fatal(String msg, String module)
        {
            return BuildAndSend(msg, module, "fatal");
        }
        /// <summary>
        /// Logs the error message
        /// </summary>
        /// <param name="msg">Message to be logged</param>
        /// <param name="module"> Module name for which the message is getting logged</param>
        /// <returns>Log object containing  logged message</returns>
        /// <exception>App42Exception</exception>   

        public Log Error(String msg, String module)
        {
            return BuildAndSend(msg, module, "error");
        }
        /// <summary>
        /// Fetch the log messages based on the Module
        /// </summary>
        /// <param name="moduleName"> Module name for which the messages has to be fetched</param>
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByModule(String moduleName)
        {
            Util.ThrowExceptionIfNullOrBlank(moduleName, "Module");
            String response = null;
            Log logObj = null;
            Util.ThrowExceptionIfNullOrBlank(moduleName, "ModuleName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            StringBuilder sb = new StringBuilder();
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "module" + "/" + moduleName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Fetch log messages based on the Module and Message Text
        /// </summary>
        /// <param name="moduleName"> Module name for which the messages has to be fetched</param>
        /// <param name="text"> The log message on which logs have to be searched</param>
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByModuleAndText(String moduleName, String text)
        {
            Util.ThrowExceptionIfNullOrBlank(moduleName, "Module");
            Util.ThrowExceptionIfNullOrBlank(text, "Text");
            String response = null;
            Log logObj = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            paramsDics.Add("text", text);
            StringBuilder sb = new StringBuilder();
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "module" + "/" + moduleName + "/" + "text" + "/" + text;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }


        /// <summary>
        /// Fetch the log messages based on the Level
        /// </summary>
        /// <param name="level"> The level on which logs have to be searched</param>
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>   

        private Log FetchLogsByLevel(String level)
        {
            Util.ThrowExceptionIfNullOrBlank(level, "Level");
            String response = null;
            Log logObj = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", level);
            StringBuilder sb = new StringBuilder();
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "type" + "/" + level;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Fetch log messages based on Info Level
        /// </summary>
        /// <returns>Log object containing fetched info messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByInfo()
        {
            return FetchLogsByLevel("INFO");
        }
        /// <summary>
        /// Fetch log messages based on Debug Level
        /// </summary>
        /// <returns>Log object containing fetched debug messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByDebug()
        {
            return FetchLogsByLevel("DEBUG");
        }
        /// <summary>
        /// Fetch log messages based on Error Level
        /// </summary>
        /// <returns>Log object containing fetched error messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByError()
        {
            return FetchLogsByLevel("ERROR");
        }
        /// <summary>
        /// Fetch log messages based on Fatal Level
        /// </summary>
        /// <returns>Log object containing fetched Fatal messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByFatal()
        {
            return FetchLogsByLevel("FATAL");
        }
        /// <summary>
        /// Fetch log messages based on Date range
        /// </summary>
        /// <param name="startDate"> Start date from which the log messages have to be fetched</param>
        /// <param name="endDate"> End date upto which the log messages have to be fetched</param>
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogByDateRange(DateTime startDate, DateTime endDate)
        {
            Util.ThrowExceptionIfNullOrBlank(startDate, "Start Date");
            Util.ThrowExceptionIfNullOrBlank(endDate, "End Date");
            String response = null;
            Log logObj = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            String strStartDate = Util.GetUTCFormattedTimestamp(startDate);
            String strEndDate = Util.GetUTCFormattedTimestamp(endDate);
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("startDate", strStartDate);
            paramsDics.Add("endDate", strEndDate);
            StringBuilder sb = new StringBuilder();
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "startDate" + "/" + strStartDate + "/" + "endDate" + "/"
                    + strEndDate;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }

        /// <summary>
        /// Fetch log messages based on Info Level
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>Log object containing fetched info messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByInfo(int max, int offset)
        {
            return FetchLogsByLevel("INFO", max, offset);
        }
        /// <summary>
        /// Fetch log messages based on Debug Level by paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>Log object containing fetched debug messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByDebug(int max, int offset)
        {
            return FetchLogsByLevel("DEBUG", max, offset);
        }
        /// <summary>
        /// Fetch log messages based on Error Level by paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>Log object containing fetched error messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByError(int max, int offset)
        {
            return FetchLogsByLevel("ERROR", max, offset);
        }
        /// <summary>
        /// Fetch log messages based on Fatal Level by paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>Log object containing fetched Fatal messages</returns>
        /// <exception>App42Exception</exception>   

        public Log FetchLogsByFatal(int max, int offset)
        {
            return FetchLogsByLevel("FATAL", max, offset);
        }


        /// <summary>
        /// Fetch the log messages based on the Module by paging.
        /// </summary>
        /// <param name="moduleName">for which the messages has to be fetched</param> 
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>        
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>
        public Log FetchLogsByModule(String moduleName, int max, int offset)
        {
            String response = null;
            Log logObj = null;
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(moduleName, "ModuleName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "paging" + "/" + "module" + "/" + moduleName + "/" + max
                    + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Fetch log messages based on the Module and Message Text by paging.
        /// </summary>
        /// <param name="moduleName">Module name for which the messages has to be fetched</param>
        /// <param name="text">The log message on which logs have to be searched</param>
        ///  <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>        
        /// <returns>Log object containing fetched messages</returns>
        ///  <exception>App42Exception</exception>
        public Log FetchLogsByModuleAndText(String moduleName, String text, int max, int offset)
        {
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(moduleName, "Module");
            Util.ThrowExceptionIfNullOrBlank(text, "Text");
            String response = null;
            Log logObj = null;

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            paramsDics.Add("text", text);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "paging" + "/" + "module" + "/" + moduleName + "/"
                    + "text" + "/" + text + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Fetch the log messages based on the Level by paging.
        /// </summary>       
        /// <param name="level">The level on which logs have to be searched</param>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>
        private Log FetchLogsByLevel(String level, int max, int offset)
        {
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(level, "Level");
            String response = null;
            Log logObj = null;   

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", level);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                + "paging" + "/" + "type" + "/" + level + "/" + max + "/"
                    + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }
        /// <summary>
        /// Fetch log messages based on Date range by paging.
        /// </summary>
        /// <param name="startDate">Start date from which the log messages have to be fetched</param>
        /// <param name="endDate">End date upto which the log messages have to be fetched</param>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>       
        /// <returns>Log object containing fetched messages</returns>
        /// <exception>App42Exception</exception>
        public Log FetchLogByDateRange(DateTime startDate, DateTime endDate, int max, int offset)
        {
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(startDate, "Start Date");
            Util.ThrowExceptionIfNullOrBlank(endDate, "End Date");
            String response = null;
            Log logObj = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            String strStartDate = Util.GetUTCFormattedTimestamp(startDate);
            String strEndDate = Util.GetUTCFormattedTimestamp(endDate);
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("startDate", strStartDate);
            paramsDics.Add("endDate", strEndDate);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "paging" + "/" + "startDate" + "/" + strStartDate + "/"
                    + "endDate" + "/" + strEndDate + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            logObj = new LogResponseBuilder().BuildResponse(response);
            return logObj;
        }


        /// <summary>
        /// Fetch the count of log messages based on the Module
        /// </summary>
        /// <param name="moduleName">Module name for which the count of messages has to be fetched</param>
        /// <returns>App42Response object containing count of fetched messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByModule(String moduleName)
        {
            Util.ThrowExceptionIfNullOrBlank(moduleName, "Module");
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(moduleName, "ModuleName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "module" + "/" + moduleName + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new LogResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Fetch count of log messages based on the Module and Message Text
        /// </summary>
        /// <param name="moduleName">Module name for which the count of messages has to be fetched</param>
        /// <param name="text">The log message on which count of logs have to be searched</param>
        /// <returns>App42Response object containing count of fetched messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByModuleAndText(String moduleName, String text)
        {
            Util.ThrowExceptionIfNullOrBlank(moduleName, "Module");
            Util.ThrowExceptionIfNullOrBlank(text, "Text");
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("moduleName", moduleName);
            paramsDics.Add("text", text);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "module" + "/" + moduleName + "/" + "text" + "/" + text + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new LogResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Fetch the count of log messages based on the Level
        /// </summary>
        /// <param name="level">The level on which count of logs have to be searched</param>
        /// <returns>App42Response object containing count of fetched messages</returns>
        /// <exception>App42Exception</exception>
        private App42Response FetchLogsCountByLevel(String level)
        {
            Util.ThrowExceptionIfNullOrBlank(level, "Level");
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", level);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "type" + "/" + level + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new LogResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Fetch count of log messages based on Info Level
        /// </summary>
        /// <returns>App42Response object containing count of fetched info messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByInfo()
        {
            return FetchLogsCountByLevel("INFO");
        }

        /// <summary>
        /// Fetch  count of log messages based on Debug Level
        /// </summary>
        /// <returns>App42Response object containing count of fetched debug messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByDebug()
        {
            return FetchLogsCountByLevel("DEBUG");
        }

        /// <summary>
        /// Fetch  count of log messages based on Error Level
        /// </summary>
        /// <returns>App42Response object containing  count of fetched error messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByError()
        {
            return FetchLogsCountByLevel("ERROR");
        }

        /// <summary>
        /// Fetch count of log messages based on Fatal Level
        /// </summary>
        /// <returns>App42Response object containing count of fetched Fatal messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogsCountByFatal()
        {
            return FetchLogsCountByLevel("FATAL");
        }
        /// <summary>
        /// Fetch count of log messages based on Date range
        /// </summary>
        /// <param name="startDate">Start date from which the count of log messages have to be fetched</param>
        /// <param name="endDate">End date upto which the count of log messages have to be fetched</param>
        /// <returns>App42Response object containing count of fetched messages</returns>
        /// <exception>App42Exception</exception>
        public App42Response FetchLogCountByDateRange(DateTime startDate, DateTime endDate)
        {
            Util.ThrowExceptionIfNullOrBlank(startDate, "Start Date");
            Util.ThrowExceptionIfNullOrBlank(endDate, "End Date");
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());

            String strStartDate = Util.GetUTCFormattedTimestamp(startDate);
            String strEndDate = Util.GetUTCFormattedTimestamp(endDate);
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("startDate", strStartDate);
            paramsDics.Add("endDate", strEndDate);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "startDate" + "/" + strStartDate + "/" + "endDate" + "/"
                    + strEndDate + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new LogResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}