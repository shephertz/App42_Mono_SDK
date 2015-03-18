using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using com.shephertz.app42.paas.sdk.csharp.connection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.user
{

    public sealed class UserGender
    {

        public static readonly string MALE = "Male";
        public static readonly string FEMALE = "Female";

    }
    /// <summary>
    /// Creates User for the App. App42 Cloud API's provides a complete User
    /// Management for any mobile or web app. It supports User registration,
    /// retrieval, state management e.g. lock, delete and Authentication.
    /// <see cref="SessionService">SessionService</see>
    /// <see cref="UserService">User</see>
    /// <see cref="App42Response">App42Response</see>
    /// </summary>
    public class UserService
    {
        private String version = "1.0";
        private String resource = "user";
        private String apiKey;
        private String secretKey;

        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        public UserService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        ///  Create a User with userName, password & emailAddress
        /// </summary>
        /// <param name="uName">Name of the User.</param>
        /// <param name="pwd">Password for the User.</param>
        /// <param name="emailAddress">Email address of the user.</param>
        /// <returns>The created User object.</returns>
        /// <exception>App42Exception</exception>

        public User CreateUser(String uName, String pwd, String emailAddress)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(pwd, "Password");
            Util.ThrowExceptionIfNullOrBlank(emailAddress, "EmailAddress");
            Util.ThrowExceptionIfEmailNotValid(emailAddress, "EmailAddress");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("email");
            jsonWriter.WriteValue(emailAddress);
            jsonWriter.WritePropertyName("password");
            jsonWriter.WriteValue(pwd);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }
        /// <summary>
        /// Gets the User details based on userName
        /// </summary>
        /// <param name="userName">Name of the User to retrieve.</param>
        /// <returns>User Object containing the profile information</returns>
        /// <exception>App42Exception</exception>

        public User GetUser(String userName)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }
        /// <summary>
        /// Gets the details of all users
        /// </summary>
        /// <returns>The List that contains all User Object.</returns>
        /// <exception>App42Exception</exception>
        public IList<User> GetAllUsers()
        {
            String response = null;
            IList<User> userList = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;

            response = RESTConnector.getInstance().ExecuteGet(signature,
            resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);
            return userList;
        }
        /// <summary>
        /// Updates the User's Email Address based on userName. Note: Only email can
        /// be updated. Username cannot be updated.
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <param name="emailAddress">Email address of the user.</param>
        /// <returns>Updated User object.</returns>
        /// <exception>App42Exception</exception>
        public User UpdateEmail(String uName, String emailAddress)
        {

            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(emailAddress, "EmailAddress");
            Util.ThrowExceptionIfEmailNotValid(emailAddress, "EmailAddress");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("email");
            jsonWriter.WriteValue(emailAddress);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }
        /// <summary>
        /// Deletes a particular user based on userName.
        /// </summary>
        /// <param name="userName">UserName which should be unique for the App.</param>
        /// <returns>App42Response Object if user deleted successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response DeleteUser(String userName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + userName;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Creates or Updates User Profile. First time the Profile for the user is
        /// created and in future calls user profile will be updated. This will
        /// always update the profile with new value passed in profile object. Call
        /// to this method should have all the values you want to retain in user
        /// profile object, otherwise old values of profile will get updated with
        /// null. Method only updates the profile of user, passing email/password in
        /// user object does not have any significance for this method call.
        /// </summary>
        /// <param name="userObj">User for which profile has to be updated, this should
        /// contain the userName and profile object in it.
        /// </param>
        /// <returns>User Object with updated Profile information.</returns>
        /// <exception>App42Exception</exception>
        public User CreateOrUpdateProfile(User userObj)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(userObj, "User");
            Util.ThrowExceptionIfNullOrBlank(userObj.GetUserName(), "UserName");
            Util.ThrowExceptionIfNullOrBlank(userObj.GetProfile(), "Profile Data");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create profile

            StringBuilder profileJson = new StringBuilder();
            StringWriter sw = new StringWriter(profileJson);
            JsonWriter profileJsonWriter = new JsonTextWriter(sw);
            profileJsonWriter.WriteStartObject();
            profileJsonWriter.WritePropertyName("firstName");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetFirstName());

            profileJsonWriter.WritePropertyName("lastName");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetLastName());

            profileJsonWriter.WritePropertyName("sex");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetSex());

            profileJsonWriter.WritePropertyName("mobile");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetMobile());

            profileJsonWriter.WritePropertyName("line1");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetLine1());

            profileJsonWriter.WritePropertyName("line2");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetLine2());

            profileJsonWriter.WritePropertyName("city");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetCity());

            profileJsonWriter.WritePropertyName("state");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetState());

            profileJsonWriter.WritePropertyName("country");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetCountry());

            profileJsonWriter.WritePropertyName("pincode");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetPincode());

            profileJsonWriter.WritePropertyName("homeLandLine");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetHomeLandLine());

            profileJsonWriter.WritePropertyName("officeLandLine");
            profileJsonWriter.WriteValue(userObj.GetProfile().GetOfficeLandLine());

            profileJsonWriter.WritePropertyName("dateOfBirth");
            profileJsonWriter.WriteValue(Util.GetUTCFormattedTimestamp(userObj.GetProfile().GetDateOfBirth()));

            profileJsonWriter.WriteEndObject();

            StringBuilder profileJsonComplete = new StringBuilder();
            StringWriter swComplete = new StringWriter(profileJsonComplete);
            JsonWriter profileJsonWriterComplete = new JsonTextWriter(swComplete);
            profileJsonWriterComplete.WriteStartObject();

            profileJsonWriterComplete.WritePropertyName("profileData");
            profileJsonWriterComplete.WriteValue(profileJson.ToString());


            profileJsonWriterComplete.WritePropertyName("userName");
            profileJsonWriterComplete.WriteValue(userObj.GetUserName());

            profileJsonWriterComplete.WriteEndObject();

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(profileJsonComplete.ToString())
                    .Append("}}");
            App42Log.Debug("Created JSOn for Profile : " + sb);
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/profile";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }
        /// <summary>
        /// Authenticate user based on userName and password.
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <param name="pwd">Password for the User.</param>
        /// <returns>App42Response Object if authenticated successfully.</returns>
        /// <exception cref="App42Exception">If authentication fails or username/password is blank or null
        /// </exception>
        public App42Response Authenticate(String uName, String pwd)
        {
            String response = null;
            App42Response responseObject = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(pwd, "Password");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("password");
            jsonWriter.WriteValue(pwd);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/authenticate";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            responseObject = new UserResponseBuilder().BuildResponse(response);
            return responseObject;
        }
        /// <summary>
        /// Locks the user based on the userName. Apps can use these feature to lock
        /// a user because of reasons specific to their usercase e.g. If payment not
        /// received and the App wants the user to be inactive
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <returns>The locked User Object.</returns>
        /// <exception>App42Exception</exception>
        public User LockUser(String uName)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/lock";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);
            return user;

        }
        /// <summary>
        /// Unlocks the user based on the userName. App developers can use this
        /// feature to unlock a user because of reasons specific to their usercase
        /// e.g. When payment received, the App developer wants the user to be active.
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <returns>The unlocked User Object.</returns>
        /// <exception>App42Exception</exception>
        public User UnlockUser(String uName)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/unlock";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }
        /// <summary>
        /// Changes the password for user based on the userName.
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <param name="oldPwd">Old Password for the user for authentication.</param>
        /// <param name="newPwd">New Password for the user to change.</param>
        /// <returns>App42Response Object if updated successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response ChangeUserPassword(String uName, String oldPwd, String newPwd)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(oldPwd, "oldPwd");
            Util.ThrowExceptionIfNullOrBlank(newPwd, "newPwd");
            App42Response responseObj = new App42Response();
            if (oldPwd.Equals(newPwd))
            {
                throw new App42Exception("Old password and new password are same");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("newPassword");
            jsonWriter.WriteValue(newPwd);
            jsonWriter.WritePropertyName("oldPassword");
            jsonWriter.WriteValue(oldPwd);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/changeUserPassword";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            responseObj = new UserResponseBuilder().BuildResponse(response);
            return responseObj;
        }
        /// <summary>
        /// Gets the details of all the locked users.
        /// </summary>
        /// <returns>The list containing locked User Objects.</returns>
        /// <exception>App42Exception</exception>
        public IList<User> GetLockedUsers()
        {
            String response = null;
            IList<User> userList = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/locked";

            response = RESTConnector.getInstance().ExecuteGet(signature,
            resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);
            return userList;
        }
        /// <summary>
        /// Gets the count of all the locked users.
        /// </summary>
        /// <returns>The count of locked User exists.</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetLockedUsersCount()
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);

            String resourceURL = this.version + "/" + this.resource
                     + "/count" + "/locked";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new UserResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Gets user details based on emailId.
        /// </summary>
        /// <param name = "emailId">EmailId of the user to be retrieved.</param>
        /// <returns>User Object.</returns>
        /// <exception>App42Exception</exception>
        public User GetUserByEmailId(String emailId)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfEmailNotValid(emailId, "EmailId");
            Util.ThrowExceptionIfNullOrBlank(emailId, "EmailId");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("emailId", emailId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/email" + "/" + emailId;

            response = RESTConnector.getInstance().ExecuteGet(signature,
            resourceURL, queryParams);
            user = new UserResponseBuilder().BuildResponse(response);
            return user;
        }

        /// <summery> 
        /// Create a User with userName, password & emailAddress and assigns the
        /// roles to the created User
        /// </summery>
        /// <param name=uName>Name of the User.</param>
        /// <param name=pwd>Password for the User.</param> 
        /// <param name=emailAddress>Email address of the user. </param> 
        /// <param name=roleList>List of roles to be assigned to User. </param> 
        /// <returns>The created User object with role list.</returns>
        /// <exception>App42Exception</exception>
        public User CreateUser(String uName, String pwd, String emailAddress, IList<String> roleList)
        {
            String response = null;
            User usr = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(pwd, "Password");
            Util.ThrowExceptionIfNullOrBlank(emailAddress, "EmailAddress");
            Util.ThrowExceptionIfEmailNotValid(emailAddress, "EmailAddress");
            Util.ThrowExceptionIfNullOrBlank(roleList, "RoleList");
            if (roleList.Count == 0)
            {
                throw new App42Exception(
                        "RoleList cannot be empty.Please assign atleast one role");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WritePropertyName("password");
            jsonWriter.WriteValue(pwd);
            jsonWriter.WritePropertyName("email");
            jsonWriter.WriteValue(emailAddress);

            JArray roleArray = new JArray();
            for (int i = 0; i < roleList.Count(); i++)
            {
                String role = (String)roleList[i];
                roleArray.Add(role.ToString());
            }
            jsonWriter.WritePropertyName("roles");
            jsonWriter.WriteValue("{\"role\":" + roleArray + "}");
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/role";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            usr = new UserResponseBuilder().BuildResponse(response);
            return usr;
        }
        /// <summary>
        /// Assign Roles to the existing User
        /// </summary>
        ///<param name=uName>Name of the User to whom the roles have to be assigned.</param>
        ///<param name=roleList>List of roles to be added to User.</param> 
        ///<returns>The created User object with assigned roles.</returns>
        /// <exception>App42Exception</exception>
        public User AssignRoles(String uName, IList<String> roleList)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(roleList, "RoleList");
            if (roleList.Count == 0)
            {
                throw new App42Exception(
                        "RoleList cannot be empty.Please assign atleast one role");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);

            JArray roleArray = new JArray();
            for (int i = 0; i < roleList.Count(); i++)
            {
                String role = (String)roleList[i];
                roleArray.Add(role.ToString());
            }
            jsonWriter.WritePropertyName("roles");
            jsonWriter.WriteValue("{\"role\":" + roleArray + "}");
            jsonWriter.WriteEndObject();

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/assignrole";
            App42Log.Debug("resourceURL" + resourceURL);
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            user = new UserResponseBuilder().BuildResponse(response);

            return user;
        }
        /// <summery> 
        /// Get the assigned roles from the specified User
        /// </summery>
        /// <param name=uName>Name of the User for whom roles have to be retrieved</param>
        /// <returns>User Object containing the role information</returns>
        /// <exception>App42Exception</exception>
        public User GetRolesByUser(String userName)
        {
            String response = null;
            User user = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + userName
                + "/roles";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            user = new UserResponseBuilder().BuildResponse(response);

            return user;
        }
        /// <summary> 
        /// Get all the Users who have the specified role assigned
        /// </summary>
        /// <param name=role>Role for which Users needs to be retrieved.</param>
        /// <returns>List of User Object for that particular role.</returns>
        /// <exception>App42Exception</exception>

        public IList<User> GetUsersByRole(String role)
        {
            String response = null;
            IList<User> userList = new List<User>();
            Util.ThrowExceptionIfNullOrBlank(role, "Role");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("role", role);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/role"
                    + "/" + role;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);

            return userList;
        }
        /// <summary>
        /// Gets the details of all the locked users By Paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched.</param>
        /// <param name="offset">From where the records are to be fetched.</param>
        /// <returns>The List containing locked User Objects.</returns>
        /// <exception>App42Exception</exception>
        public IList<User> GetLockedUsers(int max, int offset)
        {
            String response = null;
            IList<User> userList = null;
            Util.ValidateMax(max);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "locked" + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);
            return userList;
        }
        /// <summary>
        /// Gets all users by Paging
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched.</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>The List that contains all User Object</returns> 
        /// <exception>App42Exception</exception>
        public IList<User> GetAllUsers(int max, int offset)
        {
            String response = null;
            IList<User> userList = null;
            Util.ValidateMax(max);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/paging"
                    + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);
            return userList;
        }
        /// <summary>
        /// Gets the count of all the users
        /// </summary>
        /// <returns>The count of all User exists</returns> 
        /// <exception>App42Exception</exception>
        public App42Response GetAllUsersCount()
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            String signature = Util.Sign(this.secretKey, paramsDics);

            String resourceURL = this.version + "/" + this.resource
                     + "/count/all";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new UserResponseBuilder().GetTotalRecords(response));

            return responseObj;
        }
        /// <summary>
        /// Updates the User password based on userName. Username cannot be updated.
        /// </summary>
        /// <param name="uName">UserName which should be unique for the App.</param>
        /// <param name="pwd">Password to be reset.</param>
        /// <returns>App42Response Object.</returns>
        /// <exception>App42Exception</exception>
        public App42Response ResetUserPassword(String uName, String pwd)
        {

            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(uName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(pwd, "Password");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for create user

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(uName);
            jsonWriter.WritePropertyName("password");
            jsonWriter.WriteValue(pwd);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"user\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                     + "/resetUserPassword";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());

            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Gets the list of Users based on Profile Data
        /// </summary>
        /// <param name="profileData">Profile Data key/value for which Users need to be retrieved</param>
        /// <returns>List of User Object for the specified profile data</returns>
        /// <exception>App42Exception</exception>
        public IList<User> GetUsersByProfileData(User.Profile profileData)
        {
            String response = null;

            IList<User> userList = new List<User>();
            String parameters = "";
            parameters = FillParamsWithProfileData(profileData);
            App42Log.Debug("PRAMERTTERS " + parameters);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/profile" + "/" + parameters;
            App42Log.Debug("TEST RL" + resourceURL);
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            userList = new UserResponseBuilder().BuildArrayResponse(response);
            return userList;
        }
        /// <summary>
        /// Builds a Parameter string for the profileData.
        /// </summary>
        /// <param name="profileData">User.Profile object that contains profile information</param>
        /// <returns>String Object which contains the parameter string.</returns>
        private String FillParamsWithProfileData(User.Profile profileData)
        {
            String profileDataCond = "";
            if (profileData.GetCity() != null && !profileData.GetCity().Equals(""))
            {
                profileDataCond += "city:" + profileData.GetCity() + "!";
            }
            if (profileData.GetCountry() != null && !profileData.GetCountry().Equals(""))
            {
                profileDataCond += "country:" + profileData.GetCountry() + "!";
            }
            if (profileData.GetDateOfBirth() != null && !profileData.GetDateOfBirth().Equals(DateTime.MinValue))
            {
                profileDataCond += "date_of_birth:" + profileData.GetDateOfBirth() + "!";
            }
            if (profileData.GetFirstName() != null && !profileData.GetFirstName().Equals(""))
            {
                profileDataCond += "first_name:" + profileData.GetFirstName() + "!";
            }
            if (profileData.GetLastName() != null && !profileData.GetLastName().Equals(""))
            {
                profileDataCond += "last_name:" + profileData.GetLastName() + "!";
            }
            if (profileData.GetHomeLandLine() != null && !profileData.GetHomeLandLine().Equals(""))
            {
                profileDataCond += "home_land_line:" + profileData.GetHomeLandLine() + "!";
            }
            if (profileData.GetLine1() != null && !profileData.GetLine1().Equals(""))
            {
                profileDataCond += "line1:" + profileData.GetLine1() + "!";
            }
            if (profileData.GetLine2() != null && !profileData.GetLine2().Equals(""))
            {
                profileDataCond += "line2:" + profileData.GetLine2() + "!";
            }
            if (profileData.GetMobile() != null && !profileData.GetMobile().Equals(""))
            {
                profileDataCond += "mobile:" + profileData.GetMobile() + "!";
            }
            if (profileData.GetOfficeLandLine() != null && !profileData.GetOfficeLandLine().Equals(""))
            {
                profileDataCond += "office_land_line:" + profileData.GetOfficeLandLine() + "!";
            }
            if (profileData.GetPincode() != null && !profileData.GetPincode().Equals(""))
            {
                profileDataCond += "pincode:" + profileData.GetPincode() + "!";
            }
            if (profileData.GetSex() != null && !profileData.GetSex().Equals(""))
            {
                profileDataCond += "sex:" + profileData.GetSex() + "!";
            }
            if (profileData.GetState() != null && !profileData.GetState().Equals(""))
            {
                profileDataCond += "state:" + profileData.GetState() + "!";
            }
            return profileDataCond;

        }
        /// <summary>
        /// Revokes the specified role from the user.
        /// </summary>
        /// <param name="userName">UserName from whom the role has to be revoked.</param>
        /// <param name="role">Role that has to be revoked</param>
        /// <returns>App42Response of the object that contains the information about
        /// User with its role
        /// </returns>
        /// <exception>App42Exception</exception>
        public App42Response RevokeRole(String userName, String role)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            Util.ThrowExceptionIfNullOrBlank(role, "Role");
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("userName", userName);
            paramsDics.Add("role", role);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/revoke/" + role;

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Revokes all the roles from the user.
        /// </summary>
        /// <param name="userName">Name of the User from whom Roles have to be revoked.</param>
        /// <returns>App42Response of the object that contains the User information</returns>
        /// <exception>App42Exception</exception>
        public App42Response RevokeAllRoles(String userName)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/revoke";

            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
    }
}