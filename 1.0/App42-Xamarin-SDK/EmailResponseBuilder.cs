using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace com.shephertz.app42.paas.sdk.csharp.email
{
    public class EmailResponseBuilder : App42ResponseBuilder
    {
        public Email BuildResponse(String json)
        {

            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            Email emailObj = new Email();
            IList<Email.Configuration> configList = new List<Email.Configuration>();
            emailObj.SetConfigList(configList);
            emailObj.SetStrResponse(json);
            emailObj.SetResponseSuccess(IsResponseSuccess(json));
            JObject jsonObjEmail = (JObject)jsonObjResponse["email"];

            BuildObjectFromJSONTree(emailObj, jsonObjEmail);

            if (jsonObjEmail["configurations"] == null)
                return emailObj;

            JObject jsonEmailConfig = (JObject)jsonObjEmail["configurations"];

            if (jsonEmailConfig["config"] == null)
                return emailObj;

            if (jsonEmailConfig["config"] is JObject)
            {
                // Only One attribute is there
                JObject jsonObjConfig = (JObject)jsonEmailConfig["config"];
                Email.Configuration configItem = new Email.Configuration(emailObj);
                BuildObjectFromJSONTree(configItem, jsonObjConfig);
            }
            else
            {
                // There is an Array of attribute
                JArray jsonObjConfigArray = (JArray)jsonEmailConfig["config"];
                for (int i = 0; i < jsonObjConfigArray.Count; i++)
                {
                    // Get Individual Attribute Node and set it into Object
                    JObject jsonObjConfig = (JObject)jsonObjConfigArray[i];
                    Email.Configuration configItem = new Email.Configuration(emailObj);
                    BuildObjectFromJSONTree(configItem, jsonObjConfig);
                }
            }

            return emailObj;
        }

        public static void Main(String[] args)
        {
            Email email = new EmailResponseBuilder().BuildResponse("{\"app42\":{\"response\":{\"success\":\"true\",\"email\":{\"configurations\":{\"config\":{\"emailId\":\"himanshu.sharma@shephertz.co.in2012.04.19 15:29:28:4389\",\"host\":\"smtp.gmail.com\",\"port\":465,\"ssl\":true}}}}}}");
            App42Log.Debug(email + " ");
            App42Log.Debug(email.GetConfigList().ToString());
            App42Log.Debug(email.GetFrom());
            App42Log.Debug(email.GetTo());
            App42Log.Debug(email.GetBody());
            App42Log.Debug(email.GetSubject());
        }
    }
}
