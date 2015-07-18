using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp.util;
using com.shephertz.app42.paas.sdk.csharp.connection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using com.shephertz.app42.paas.sdk.csharp.storage;

namespace com.shephertz.app42.paas.sdk.csharp.storage
{
    public class Query : App42Response
    {
        private JObject jsonObject;

        private JArray jsonArray;

        public Query(JObject jsonQuery)
        {
            this.jsonObject = jsonQuery;
        }

        public Query(JArray jsonQuery)
        {
            this.jsonArray = jsonQuery;
        }



        public String Get()
        {
            try
            {
                if (jsonObject != null)
                {
                    return new JArray(jsonObject.ToString()).ToString();
                }
                else
                {
                    return new JArray(jsonArray.ToString()).ToString();
                }
            }
            catch (Exception jsonException)
            {
                throw new App42Exception("JOSN Excepction");
            }
        }

        public String GetStr()
        {
            if (jsonObject != null)
            {
                return new StringBuilder("[").Append(jsonObject.ToString()).Append("]").ToString();
            }
            else
            {
                return jsonArray.ToString();
            }
        }
        #warning this one hides .net default method .GetType()!
        public Object GetType()
        {
            if (jsonObject != null)
            {
                return jsonObject;
            }
            else
            {
                return jsonArray;
            }
        }
    }
}