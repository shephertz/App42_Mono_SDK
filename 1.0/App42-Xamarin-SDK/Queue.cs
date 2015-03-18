using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.message
{
   public  class Queue : App42Response
    {
        public String queueName;
        public String queueType;
        public String description;

        public String GetDescription()
        {
            return description;
        }
        public void SetDescription(String description)
        {
            this.description = description;
        }
        public IList<Message> messageList = new List<Message>();
        public String GetQueueName()
        {
            return queueName;
        }
        public void GetQueueName(String queueName)
        {
            this.queueName = queueName;
        }
        public String GetQueueType()
        {
            return queueType;
        }
        public void SetQueueType(String queueType)
        {
            this.queueType = queueType;
        }
        public IList<Message> GetMessageList()
        {
            return messageList;
        }
        public void  SetMessageList(IList<Message> messageList)
        {
            this.messageList = messageList;
        }
        public class Message
        {
            public Message(Queue queue)
            {
                queue.messageList.Add(this);
            }
            public String correlationId;
            public String payLoad;
            public String messageId;
            public String GetCorrelationId()
            {
                return correlationId;
            }
            public void SetCorrelationId(String correlationId)
            {
                this.correlationId = correlationId;
            }
            public String GetPayLoad()
            {
                return payLoad;
            }
            public void SetPayLoad(String payLoad)
            {
                this.payLoad = payLoad;
            }
            public String GetMessageId()
            {
                return messageId;
            }
            public void SetMessageId(String messageId)
            {
                this.messageId = messageId;
            }

            public String ToString()
            {
                return " correlationId : " + correlationId + " : payLoad : " + payLoad + " : messageId : " + messageId;
            }
        }
    }
}

