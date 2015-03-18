using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.customcode
{
    public class CustomCodeService
    {

        private String version = "1.0";
        private String resource = "customcode";
        private String apiKey;
        private String secretKey;

        public CustomCodeService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }

        public JObject RunJavaCode(String name, JObject jsonBody)
        {

            String response = null;

            Util.ThrowExceptionIfNullOrBlank(name, "name");
            Util.ThrowExceptionIfNullOrBlank(jsonBody, "jsonBody");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            // Construct a json body for Charge User

            App42Log.Debug(" Json String : " + jsonBody.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/run/java/" + name;
            response = RESTConnector.getInstance().ExecuteCustomCode(signature,
                    resourceURL, queryParams, jsonBody.ToString());
            return JObject.Parse(response);
        }
    }
}