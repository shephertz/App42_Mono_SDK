using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.pushNotification
{
    public class PushNotification : App42Response
    {
        public String message;
        public String userName;
        public String deviceToken;
        public String type;
        public IList<Channel> channelList = new List<Channel>();

        public String GetMessage()
        {
            return message;
        }

        public void SetMessage(String message)
        {
            this.message = message;
        }

        public String GetDeviceToken()
        {
            return deviceToken;
        }

        public void SetDeviceToken(String deviceToken)
        {
            this.deviceToken = deviceToken;
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
        public String GetUserName()
        {
            return userName;
        }
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }

        public IList<Channel> GetChannelList()
        {
            return channelList;
        }

        public void SetChannelList(IList<Channel> channelList)
        {
            this.channelList = channelList;
        }

        public class Channel
        {
            public String channelName;
            public String description;
            public String GetName()
            {
                return channelName;
            }
            public void SetName(String channelName)
            {
                this.channelName = channelName;
            }
            public String GetDescription()
            {
                return description;
            }
            public void SetDescription(String description)
            {
                this.description = description;
            }
            public Channel(PushNotification pushNotification)
            {
                pushNotification.channelList.Add(this);
            }
        }
    }
}