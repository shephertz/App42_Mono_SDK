using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.user
{
    /// <summary>
    /// UserResponseBuilder class converts the JSON response retrieved from the
    /// server to the value object i.e User
    /// </summary>

    public class UserResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e User
        /// </summary>
        /// <param name="json">response in JSON format</param>
        /// <returns>User object filled with json data</returns>
        public User BuildResponse(String json)
        {
            JObject usersJSONObj = GetServiceJSONObject("users", json);
            JObject userJSOnObj = (JObject)usersJSONObj["user"];
            User user = BuildUserObject(userJSOnObj);
            user.SetStrResponse(json);
            user.SetResponseSuccess(IsResponseSuccess(json));
            return user;
        }

        /// <summary>
        /// Converts the User JSON object to the value object i.e User
        /// </summary>
        /// <param name="userJSONObj">user data as JSONObject</param>
        /// <returns>User object filled with json data</returns>
        private User BuildUserObject(JObject userJSONObj)
        {
            User user = new User();
            BuildObjectFromJSONTree(user, userJSONObj);
            if (userJSONObj["profile"] != null)
            {
                JObject profileJSONObj = (JObject)userJSONObj["profile"];
                User.Profile profile = new User.Profile(user);
                BuildObjectFromJSONTree(profile, profileJSONObj);
            }
            if (userJSONObj["role"] != null)
            {
                IList<String> roleList = new List<String>();
                if (userJSONObj["role"] is JArray)
                {

                    JArray roleArr = (JArray)userJSONObj["role"];
                    for (int i = 0; i < roleArr.Count(); i++)
                    {
                        roleList.Add(roleArr[i].ToString());
                    }
                }
                else
                {

                    roleList.Add((String)userJSONObj["role"]);
                }

                user.SetRoleList(roleList);
            }

            return user;
        }

        /// <summary>
        /// Converts the response in JSON format to the list of value objects i.e User
        /// </summary>
        /// <param name="json">response in JSON format</param>
        /// <returns>List of User object filled with json data</returns>
        public IList<User> BuildArrayResponse(String json)
        {
            JObject usersJSONObj = GetServiceJSONObject("users", json);
            IList<User> userList = new List<User>();
            if (usersJSONObj["user"] is JArray)
            {
                JArray userJSONArray = (JArray)usersJSONObj["user"];
                for (int i = 0; i < userJSONArray.Count; i++)
                {
                    JObject userJSONObject = (JObject)userJSONArray[i];
                    User user = BuildUserObject(userJSONObject);
                    BuildObjectFromJSONTree(user, userJSONObject);
                    user.SetStrResponse(json);
                    user.SetResponseSuccess(IsResponseSuccess(json));
                    userList.Add(user);
                }
            }
            else
            {
                JObject userJSONObject = (JObject)usersJSONObj["user"];
                User user = BuildUserObject(userJSONObject);
                user.SetStrResponse(json);
                user.SetResponseSuccess(IsResponseSuccess(json));
                userList.Add(user);

            }
            return userList;
        }
    }
}