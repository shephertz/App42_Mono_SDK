using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.social
{
    /// <summary>
    /// SocialResponseBuilder class converts the JSON response retrieved 
    /// from the server to the value object i.e Social
    /// </summary>
    public class SocialResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e Social
        /// </summary>
        /// <param name="json">Response in JSON format.</param>
        /// <returns>Social object filled with json data.</returns>
        public Social BuildResponse(String json)
        {
            JObject socialJSONObject = GetServiceJSONObject("social", json);
            Social social = new Social();
            social.SetStrResponse(json);
            social.SetResponseSuccess(IsResponseSuccess(json));
            BuildObjectFromJSONTree(social, socialJSONObject);
            if (socialJSONObject["friends"] != null)
            {
                if (socialJSONObject["friends"] is JObject)
                {
                    JObject friendsJsonObject = (JObject)socialJSONObject["friends"];
                    Social.Friends friends = new Social.Friends(social);
                    BuildObjectFromJSONTree(friends, friendsJsonObject);
                }
                else
                {
                    JArray friendsJSOnArray = (JArray)socialJSONObject["friends"];
                    for (int i = 0; i < friendsJSOnArray.Count(); i++)
                    {
                        JObject friendJSONObj = (JObject)friendsJSOnArray[i];
                        Social.Friends friends = new Social.Friends(social);
                        BuildObjectFromJSONTree(friends, friendJSONObj);
                    }
                }
            }
            return social;
        }
    }
}