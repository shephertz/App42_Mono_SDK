using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp
{
    class Config
    {
        private static Config config;
        private String protocol = "http://";
        private String baseURL;
        private String serverName =  "/App42_API_SERVER/cloud/";
        private String contentType = "application/json";
        private String accept = "application/json";
        private String customCodeURL = null;


        private Config()  {
            this.baseURL = "https://api.shephertz.com/cloud/";
            this.customCodeURL = "https://customcode.shephertz.com/";
            App42Log.Debug("Configuration Properties " + this.baseURL);
        }


        public static Config GetInstance() {
            if(config == null){     
				config = new Config();			
            }
        return config;
        }

        public void SetBaseURL(String protocol, String host, Int32 port)
        {
            this.baseURL = protocol + host + ":" + port + serverName;
        }
        public String GetBaseURL()
        {
            return this.baseURL;
        }

        public void SetContentType(String contentType)
        {
            this.contentType = contentType;
        }
        public String GetContentType()
        {
            return this.contentType;
        }

        public void SetAccept(String accept)
        {
            this.accept = accept;
        }
        public String GetAccept()
        {
            return this.accept;
        }

        public String GetCustomCodeURL()
        {
            return this.customCodeURL;
        }

        public void SetCustomCodeURL(String customCodeURL)
        {
            this.customCodeURL = customCodeURL;
        }
    }
}
