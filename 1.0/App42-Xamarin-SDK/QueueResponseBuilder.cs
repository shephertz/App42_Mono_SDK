using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.message
{

   public class QueueResponseBuilder : App42ResponseBuilder
    {
        public Queue BuildResponse(String json)
        {
            JObject queuesJSONObj = GetServiceJSONObject("queues", json);
            JObject queueJSONObj = (JObject)queuesJSONObj["queue"];
            Queue queue = new Queue();
            queue.SetStrResponse(json);
            queue.SetResponseSuccess(IsResponseSuccess(json));
            BuildObjectFromJSONTree(queue, queueJSONObj);
            Object obj = queueJSONObj["messages"];
            if (obj != null)
            {
               // JObject messageJsonObj = (JObject)queueJSONObj["messages"]["message"];
                if (queueJSONObj["messages"]["message"] != null && queueJSONObj["messages"]["message"] is JObject)
                {
                    // Single Entry
					JObject messageJsonObj = (JObject)queueJSONObj["messages"]["message"];
                    Queue.Message messageObj = new Queue.Message(queue);
                    BuildObjectFromJSONTree(messageObj, messageJsonObj);
                }
                else if (queueJSONObj["messages"]["message"] != null && queueJSONObj["messages"]["message"] is JArray)
                {
                    JArray messagesJSONArray = (JArray)queueJSONObj["messages"]["message"];
                    for (int    i = 0; i < messagesJSONArray.Count; i++)
                    {
                        JObject messageJSONObj = (JObject)messagesJSONArray[i];
                        Queue.Message messageObj = new Queue.Message(queue);
                        BuildObjectFromJSONTree(messageObj, messageJSONObj);
                    }
                }
                else
                {
                    return queue;
                }
             }
            return queue;
        }
    }
}
    


