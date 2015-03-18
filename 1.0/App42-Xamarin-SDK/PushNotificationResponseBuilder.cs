using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.pushNotification
{
    public class PushNotificationResponseBuilder : App42ResponseBuilder
    {
        public PushNotification BuildResponse(String json)
        {
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            PushNotification pushObj = new PushNotification();
            IList<PushNotification.Channel> channelList = new List<PushNotification.Channel>();
            pushObj.SetChannelList(channelList);
            pushObj.SetStrResponse(json);
            pushObj.SetResponseSuccess(IsResponseSuccess(json));
            JObject jsonObjPush = (JObject)jsonObjResponse["push"];

            BuildObjectFromJSONTree(pushObj, jsonObjPush);

            if (jsonObjPush["channels"] == null)
                return pushObj;

            JObject jsonPushChannel = (JObject)jsonObjPush["channels"];

            if (jsonPushChannel["channel"] == null)
                return pushObj;

            if (jsonPushChannel["channel"] is JObject)
            {
                JObject jsonObjChannel = (JObject)jsonPushChannel["channel"];
                PushNotification.Channel channelItem = new PushNotification.Channel(pushObj);
                BuildObjectFromJSONTree(channelItem, jsonObjChannel);
            }
            else
            {
                JArray jsonObjChannelArray = (JArray)jsonPushChannel["channel"];
                for (int i = 0; i < jsonObjChannelArray.Count; i++)
                {
                    JObject jsonObjChannel = (JObject)jsonObjChannelArray[i];
                    PushNotification.Channel configItem = new PushNotification.Channel(pushObj);
                    BuildObjectFromJSONTree(configItem, jsonObjChannel);
                }
            }

            return pushObj;
        }
    }
}