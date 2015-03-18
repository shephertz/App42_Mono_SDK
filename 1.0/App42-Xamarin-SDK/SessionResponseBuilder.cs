using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.session
{
    /// <summary>
    /// SessionResponseBuilder class converts the JSON response 
    /// retrieved from the server to the value object i.e Session.
    /// </summary>
    public class SessionResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e Session
        /// </summary>
        /// <param name="json">Response in JSON format.</param>
        /// <returns>Session object filled with json data.</returns>
        public Session BuildResponse(String json)
        {
            Session sessionObj = new Session();
            IList<Session.Attribute> attributeList = new List<Session.Attribute>();
            sessionObj.SetAttributeList(attributeList);
            sessionObj.SetStrResponse(json);
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            sessionObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);
            JObject jsonObjSession = (JObject)jsonObjResponse["session"];
            BuildObjectFromJSONTree(sessionObj, jsonObjSession);

            if (jsonObjSession["attributes"] == null)
                return sessionObj;

            JObject jsonObjAttributes = (JObject)jsonObjSession["attributes"];

            if (jsonObjAttributes["attribute"] == null)
                return sessionObj;

            if (jsonObjAttributes["attribute"] != null && jsonObjAttributes["attribute"] is JObject)
            {
                JObject jsonObjAttribute = (JObject)jsonObjAttributes["attribute"];
                Session.Attribute attribute = new Session.Attribute(sessionObj);
                BuildObjectFromJSONTree(attribute, jsonObjAttribute);

            }
            else
            {
                JArray jsonObjAttributeArray = (JArray)jsonObjAttributes["attribute"];
                for (int i = 0; i < jsonObjAttributeArray.Count(); i++)
                {
                    JObject jsonObjAttribute = (JObject)jsonObjAttributeArray[i];
                    Session.Attribute attribute = new Session.Attribute(sessionObj);
                    BuildObjectFromJSONTree(attribute, jsonObjAttribute);
                }
            }

            return sessionObj;
        }
        /// <summary>
        /// Main method creating a new Sessionobject resulting into a response that has to be displayed.
        /// </summary>
        /// <param name="args"></param>
        public static void main(String[] args)
        {
            Session seeionObj = new SessionResponseBuilder().BuildResponse("{\"app42\":{\"response\":{\"success\":true,\"session\":{\"userName\":\"test\",\"sessionId\":\"f4e29dc2-6482-43b0-ad23-f499caab08e8\",\"createdOn\":\"2012-03-22T02:05:23.000Z\",\"attributes\": {  \"attribute\": [  {   \"name\": \"language\",  \"value\": \"eng\"   },    {     \"name\": \"version\",       \"value\": \"1.0\"    }    ]  }  }}}}");
            App42Log.Debug(seeionObj.GetAttributeList() + "");
        }
    }
}