using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace com.shephertz.app42.paas.sdk.csharp.log
{

    public class LogResponseBuilder : App42ResponseBuilder
    {

        public Log BuildResponse(String json)
        {
            Log logObj = new Log();
            IList<Log.Message> msgList = new List<Log.Message>();
            logObj.SetMessageList(msgList);
            logObj.SetStrResponse(json);
            JObject LogjsonObj = JObject.Parse(json);

            JObject jsonObjApp42 = (JObject)LogjsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];

            logObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);
            JObject jsonObjLog = (JObject)jsonObjResponse["logs"];
            if (jsonObjLog["log"] == null)
                return logObj;
            if (jsonObjLog["log"] != null && jsonObjLog["log"] is JObject)
            {
                // Only One attribute is there
                JObject jsonObjLogMessage = (JObject)jsonObjLog["log"];
                Log.Message messageItem = new Log.Message(logObj);
                BuildObjectFromJSONTree(messageItem, jsonObjLogMessage);
            }
            else
            {
                // There is an Array of attribute   
                JArray jsonObjMessageArray = (JArray)jsonObjLog["log"];
                for (int i = 0; i < jsonObjMessageArray.Count(); i++)
                {
                    // Get Individual Attribute Node and set it into Object
                    JObject jsonObjLogMessage = (JObject)jsonObjMessageArray[i];
                    Log.Message messageItem = new Log.Message(logObj);
                    BuildObjectFromJSONTree(messageItem, jsonObjLogMessage);
                }
            }
            return logObj;
        }

        public static void main(String[] args)
        {
            Log logObj = new LogResponseBuilder().BuildResponse("{\"app42\":{\"response\":{\"success\":true,\"logs\":{\"log\":[{\"message\":\"info logs\",\"type\":\"INFO\",\"logTime\":\"2012-03-23T06:29:23.482Z\",\"module\":\"testmod1\"},{\"message\":\"info logs\",\"logTime\":\"2012-03-23T07:12:59.000Z\",\"module\":\"testmod2\",\"type\":\"INFO\"}]}}}}");
            App42Log.Debug(logObj.GetMessageList().ToString());
            App42Log.Debug(logObj.ToString());
        }
    }
}