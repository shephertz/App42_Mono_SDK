using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Reflection;

namespace com.shephertz.app42.paas.sdk.csharp
{
    public class App42ResponseBuilder
    {
        public void BuildObjectFromJSONTree(Object obj, JObject jsonObj)
        {
            IList<String> keys = jsonObj.Properties().Select(p => p.Name).ToList();
            foreach (String token in keys)
            {
                FieldInfo fieledInfo = obj.GetType().GetField(token);
                if (fieledInfo != null)
                {
                    if (fieledInfo.FieldType.FullName.Equals("System.String"))
                        fieledInfo.SetValue(obj, "" + jsonObj[token]);
                    if (fieledInfo.FieldType.FullName.Equals("System.DateTime"))
                    {
                        String dateStr = "" + jsonObj[token];
                        if (!dateStr.Equals(""))
                        {
                            DateTime dt = Convert.ToDateTime("" + jsonObj[token]);
                            fieledInfo.SetValue(obj, dt);
                        }
                    }

                    if (fieledInfo.FieldType.FullName.Equals("System.Boolean"))
                    {
                        fieledInfo.SetValue(obj, Convert.ToBoolean("" + jsonObj[token]));
                    }

                    if (fieledInfo.FieldType.FullName.Equals("System.Double"))
                    {
                        String doubleStr = "" + jsonObj[token];
                        if (!doubleStr.Equals("null") && !doubleStr.Trim().Equals(""))
                            fieledInfo.SetValue(obj, Convert.ToDouble("" + jsonObj[token]));
                    }
                    if (fieledInfo.FieldType.FullName.Equals("System.int"))
                    {
                        fieledInfo.SetValue(obj, Convert.ToInt32("" + jsonObj[token]));
                    }

                    if (fieledInfo.FieldType.FullName.Equals("System.Int64"))
                    {

                        fieledInfo.SetValue(obj, Convert.ToInt64("" + jsonObj[token]));
                    }


                }
            }
        }

        public JObject GetServiceJSONObject(String serviceName, String json)
        {
            App42Log.Debug("" + json);
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjService = (JObject)jsonObj["app42"]["response"][serviceName];
            return jsonObjService;
        }

        /// <summary>
        ///  Throws Exception
        /// </summary>
        /// <param name="json">json</param>
        /// <returns></returns>

        public bool IsResponseSuccess(String json)
        {
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjService = (JObject)jsonObj["app42"]["response"];
            return Convert.ToBoolean("" + jsonObjService["success"]);
        }
        /// <summary>
        ///  Throws Exception
        /// </summary>
        /// <param name="json">json</param>
        /// <returns></returns>
        public int GetTotalRecords(String json)
        {
            int totalRecords = -1;
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];

            if (jsonObjResponse != null && jsonObjResponse["totalRecords"] is JObject)
            {
                totalRecords = (int)jsonObjResponse["totalRecords"];
            }
            return totalRecords;
        }
    }
}