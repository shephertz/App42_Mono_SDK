using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.social
{
    /// <summary>
    /// This class Manage the response which comes from App42 server and set, get values from the response.
    /// </summary>
    public class Social : App42Response
    {
        public String userName;
        public String status;
        public String facebookAppId;
        public String facebookAppSecret;
        public String facebookAccessToken;
        public String twitterConsumerKey;
        public String twitterConsumerSecret;
        public String twitterAccessToken;
        public String twitterAccessTokenSecret;
        public String linkedinApiKey;
        public String linkedinSecretKey;
        public String linkedinAccessToken;
        public String linkedinAccessTokenSecret;
        IList<Friends> friendsList = new List<Friends>();
        /// <summary>
        /// Returns the user name for the social.
        /// </summary>
        /// <returns>The user name for the social.</returns>
        public String GetUserName()
        {
            return userName;
        }
        /// <summary>
        /// Sets the user name for the social.
        /// </summary>
        /// <param name="userName">Name of the user whose social account to be linked.</param>
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }
        /// <summary>
        /// Returns the status for the social.
        /// </summary>
        /// <returns>The status for the social.</returns>
        public String GetStatus()
        {
            return status;
        }
        /// <summary>
        /// Sets the status for the social.
        /// </summary>
        /// <param name="status">Status for the social account which has to be updated.</param>
        public void SetStatus(String status)
        {
            this.status = status;
        }
        /// <summary>
        /// Returns the app id for the Facebook.
        /// </summary>
        /// <returns>The app id for the Facebook.</returns>
        public String GetFacebookAppId()
        {
            return facebookAppId;
        }
        /// <summary>
        /// Sets the app id for the Facebook.
        /// </summary>
        /// <param name="facebookAppId">App id for linking to the facebook account.</param>
        public void SetFacebookAppId(String facebookAppId)
        {
            this.facebookAppId = facebookAppId;
        }
        /// <summary>
        /// Returns the app secret for the Facebook.
        /// </summary>
        /// <returns>The app secret for the Facebook.</returns>
        public String GetFacebookAppSecret()
        {
            return facebookAppSecret;
        }
        /// <summary>
        /// Sets the app secret for the Facebook.
        /// </summary>
        /// <param name="facebookAppSecret">App secret for linking to the facebook account.</param>
        public void SetFacebookAppSecret(String facebookAppSecret)
        {
            this.facebookAppSecret = facebookAppSecret;
        }
        /// <summary>
        /// Returns the access token for the Facebook.
        /// </summary>
        /// <returns>The access token for the Facebook.</returns>
        public String GetFacebookAccessToken()
        {
            return facebookAccessToken;
        }
        /// <summary>
        /// Sets the access token for the Facebook.
        /// </summary>
        /// <param name="facebookAccessToken">Facebook Access Token that has been received after authorization</param>
        public void SetFacebookAccessToken(String facebookAccessToken)
        {
            this.facebookAccessToken = facebookAccessToken;
        }
        /// <summary>
        /// Returns the consumer key for the Twitter.
        /// </summary>
        /// <returns>The consumer key for the Twitter.</returns>
        public String GetTwitterConsumerKey()
        {
            return twitterConsumerKey;
        }
        /// <summary>
        /// Sets the consumer key for the Twitter.
        /// </summary>
        /// <param name="twitterConsumerKey">Consumer key for linking to the twitter account.</param>
        public void SetTwitterConsumerKey(String twitterConsumerKey)
        {
            this.twitterConsumerKey = twitterConsumerKey;
        }
        /// <summary>
        /// Returns the consumer secret key for the Twitter.
        /// </summary>
        /// <returns>The consumer secret key for the Twitter.</returns>
        public String GetTwitterConsumerSecret()
        {
            return twitterConsumerSecret;
        }
        /// <summary>
        /// Sets the consumer secret key for the Twitter.
        /// </summary>
        /// <param name="twitterConsumerSecret">Consumer secret for linking to the twitter account.</param>
        public void SetTwitterConsumerSecret(String twitterConsumerSecret)
        {
            this.twitterConsumerSecret = twitterConsumerSecret;
        }
        /// <summary>
        /// Returns the access token for the Twitter.
        /// </summary>
        /// <returns>The access token for the Twitter.</returns>
        public String GetTwitterAccessToken()
        {
            return twitterAccessToken;
        }
        /// <summary>
        /// Sets the access token for the Twitter.
        /// </summary>
        /// <param name="twitterAccessToken">Twitter Access Token that has been received after authorization.</param>
        public void SetTwitterAccessToken(String twitterAccessToken)
        {
            this.twitterAccessToken = twitterAccessToken;
        }
        /// <summary>
        /// Returns the access token secret for the Twitter.
        /// </summary>
        /// <returns>The access token secret for the Twitter.</returns>
        public String GetTwitterAccessTokenSecret()
        {
            return twitterAccessTokenSecret;
        }
        /// <summary>
        /// Sets the access token secret for the Twitter.
        /// </summary>
        /// <param name="twitterAccessTokenSecret">Twitter Access Token Secret that has been received after authorization.</param>
        public void SetTwitterAccessTokenSecret(String twitterAccessTokenSecret)
        {
            this.twitterAccessTokenSecret = twitterAccessTokenSecret;
        }
        /// <summary>
        ///  Returns the api key for the LinkedIn.
        /// </summary>
        /// <returns>The api key for the LinkedIn.</returns>
        public String GetLinkedinApiKey()
        {
            return linkedinApiKey;
        }
        /// <summary>
        /// Sets the api key for the LinkedIn.
        /// </summary>
        /// <param name="linkedinApiKey">Api key for linking to the linkedIn account.</param>
        public void SetLinkedinApiKey(String linkedinApiKey)
        {
            this.linkedinApiKey = linkedinApiKey;
        }
        /// <summary>
        /// Returns the secret key for the LinkedIn.
        /// </summary>
        /// <returns>The secret key for the LinkedIn.</returns>
        public String GetLinkedinSecretKey()
        {
            return linkedinSecretKey;
        }
        /// <summary>
        /// Sets the secret key for the LinkedIn.
        /// </summary>
        /// <param name="linkedinSecretKey">Secret key for linking to the linkedIn account.</param>
        public void SetLinkedinSecretKey(String linkedinSecretKey)
        {
            this.linkedinSecretKey = linkedinSecretKey;
        }
        /// <summary>
        /// Returns the access token for the LinkedIn.
        /// </summary>
        /// <returns>The access token for the LinkedIn.</returns>
        public String GetLinkedinAccessToken()
        {
            return linkedinAccessToken;
        }
        /// <summary>
        /// Sets the access token for the LinkedIn.
        /// </summary>
        /// <param name="linkedinAccessToken">LinkedIn Access Token that has been received after authorization.</param>
        public void SetLinkedinAccessToken(String linkedinAccessToken)
        {
            this.linkedinAccessToken = linkedinAccessToken;
        }
        /// <summary>
        /// Returns the access token secret for the LinkedIn.
        /// </summary>
        /// <returns>The access token secret for the LinkedIn.</returns>
        public String GetLinkedinAccessTokenSecret()
        {
            return linkedinAccessTokenSecret;
        }
        /// <summary>
        /// Sets the access token secret for the LinkedIn.
        /// </summary>
        /// <param name="linkedinAccessTokenSecret">LinkedIn Access Token Secret that has been received after authorization.</param>
        public void SetLinkedinAccessTokenSecret(String linkedinAccessTokenSecret)
        {
            this.linkedinAccessTokenSecret = linkedinAccessTokenSecret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Friends> GetFriendList()
        {
            return friendsList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="friendsList"></param>
        public void SetFriendList(IList<Friends> friendsList)
        {
            this.friendsList = friendsList;
        }

        public class Friends
        {
            /// <summary>
            /// 
            /// </summary>
            public Friends(Social social)
            {
                social.friendsList.Add(this);
            }
            public String name;
            public String picture;
            public String id;
            public Boolean installed;

            public Boolean GetInstalled()
            {
                return installed;
            }
            public void SetInstalled(Boolean installed)
            {
                this.installed = installed;
            }
            public String GetName()
            {
                return name;
            }
            public void SetName(String name)
            {
                this.name = name;
            }
            public String GetPicture()
            {
                return picture;
            }
            public void SetPicture(String picture)
            {
                this.picture = picture;
            }
            public String GetId()
            {
                return id;
            }
            public void SetId(String id)
            {
                this.id = id;
            }
        }
    }
}