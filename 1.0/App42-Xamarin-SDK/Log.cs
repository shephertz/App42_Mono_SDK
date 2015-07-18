using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.log
{
    public class Log : App42Response
    {
        private IList<Log.Message> messageList = new List<Log.Message>();

        
        public IList<Log.Message> GetMessageList()
        {
            return messageList;
        }
        public void SetMessageList(IList<Log.Message> messageList)
        {
            this.messageList = messageList;
        }
        public class Message
        {
            public Message(Log log)
            {
                log.messageList.Add(this);
            }
            public String message;
            public String type;
            public DateTime logTime;
            public String module;
            public String GetMessage()
            {
                return message;
            }
            public void SetMessage(String message)
            {
                this.message = message;
            }
            #warning this one hides .net default method .GetType()!
            public String GetType()
            {
                return type;
            }
            public void SetType(String type)
            {
                this.type = type;
            }
            public DateTime GetLogTime()
            {
                return logTime;
            }
            public void SetLogTime(DateTime logTime)
            {
                this.logTime = logTime;
            }
            public String GetModule()
            {
                return module;
            }
            public void SetModule(String appModule)
            {
                this.module = appModule;
            }
            public override String ToString()
            {
                return "Message : " + this.message + " : type : " + this.type + " : AppModule : " + this.module + " : logTime : " + this.logTime;
            }
        }
    }
}