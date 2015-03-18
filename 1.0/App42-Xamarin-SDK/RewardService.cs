using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;

namespace com.shephertz.app42.paas.sdk.csharp.reward
{
    /// <summary>
    /// Define a Reward e.g. Sword, Energy etc. Is needed for Reward Points
    /// The Game service allows Game, User, Score and ScoreBoard Management on the Cloud.
    /// The service allows Game Developer to create a Game and then do in Game Scoring using the
    /// Score service. It also allows to maintain a Scoreboard across game sessions using the ScoreBoard
    /// service. One can query for average or highest score for user for a Game and highest and average score across users
    /// for a Game. It also gives ranking of the user against other users for a particular game.
    /// The Reward and RewardPoints allows the Game Developer to assign rewards to a user and redeem the rewards.
    /// E.g. One can give Swords or Energy etc.
    /// The services Game, Score, ScoreBoard, Reward, RewardPoints can be used in Conjunction for complete Game Scoring and Reward
    /// Management.
    /// </summary>
    /// <see cref="RewardService">Game, RewardPoint, Score, ScoreBoard</see>

    public class RewardService
    {

        private String version = "1.0";
        private String resource = "game/reward";
        private String apiKey;
        private String secretKey;
        String baseURL;

        /// <summary>
        /// this is a constructor that takes
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>

        public RewardService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }
        /// <summary>
        /// Creates Reward. Reward can be Sword, Energy etc. When Reward Points have to be 
        /// added the Reward name created using this method has to be specified.
        /// </summary>
        /// <param name="rewardName">The reward that has to be created</param>
        /// <param name="rewardDescription">The description of the reward to be created</param>
        /// <returns>Reward object containing the reward that has been created</returns>
        /// <exception>App42Exception</exception>

        public Reward CreateReward(String rewardName, String rewardDescription)
        {

            String response = null;
            Reward reward = null;
            Util.ThrowExceptionIfNullOrBlank(rewardName, "Reward Name");
            Util.ThrowExceptionIfNullOrBlank(rewardDescription,
                    "Reward Description");


            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                        Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(rewardName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(rewardDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"app42\":{\"rewards\":{\"reward\":").Append(sbJson.ToString())
                    .Append("}}}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            reward = new RewardResponseBuilder().BuildResponse(response);
            return reward;
        }
        /// <summary>
        /// Fetches all the Rewards
        /// </summary>
        /// <returns>IList of Reward objects containing all the rewards of the App</returns>
        /// <exception>App42Exception</exception>

        public IList<Reward> GetAllRewards()
        {
            String response = null;
            IList<Reward> rewardList = null;


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
            rewardList = new RewardResponseBuilder().BuildArrayRewards(response);
            return rewardList;

        }
        /// <summary>
        /// Retrieves the reward for the specified name
        /// </summary>
        /// <param name="rewardName"> Name of the reward that has to be fetched</param>
        /// <returns>Reward object containing the reward based on the rewardName</returns>
        /// <exception>App42Exception</exception>

        public Reward GetRewardByName(String rewardName)
        {
            String response = null;
            Reward reward = null;
            Util.ThrowExceptionIfNullOrBlank(rewardName, "Reward Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", rewardName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + rewardName;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            reward = new RewardResponseBuilder().BuildResponse(response);
            return reward;
        }

        /// <summary>
        /// Adds the reward points to an users account. Reward Points can be earned by the user
        /// which can be redeemed later.
        /// </summary>
        /// <param name="gameName">Name of the game for which reward points have to be added</param>
        /// <param name="gameUserName">The user for whom reward points have to be added</param>
        /// <param name="rewardName">The rewards for which reward points have to be added</param>
        /// <param name="rewardPoints">The points that have to be added</param>
        /// <returns>Reward object containing the rewardpoints that has been added</returns>
        /// <exception>App42Exception</exception>

        public Reward EarnRewards(String gameName, String gameUserName,
            String rewardName, Double rewardPoints)
        {
            String response = null;
            Reward reward = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(gameUserName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(rewardName, "Reward Name");
            Util.ThrowExceptionIfNullOrBlank(rewardPoints, "Reward Points");


            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                  Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("gameName");
            jsonWriter.WriteValue(gameName);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(gameUserName);
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(rewardName);
            jsonWriter.WritePropertyName("points");
            jsonWriter.WriteValue(rewardPoints);

            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"rewards\":{\"reward\":")
                     .Append(sbJson.ToString()).Append("}}}");


            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/earn";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            reward = new RewardResponseBuilder().BuildResponse(response);
            return reward;

        }
        /// <summary>
        /// Deducts the rewardpoints from the earned rewards by a user.
        /// </summary>
        /// <param name="gameName">Name of the game for which reward points have to be deducted</param>
        /// <param name="gameUserName">The user for whom reward points have to be deducted</param>
        /// <param name="rewardName">The rewards for which reward points have to be deducted</param>
        /// <param name="rewardPoints">The points that have to be deducted</param>
        /// <returns>Reward object containing the rewardpoints that has been deducted</returns>
        /// <exception>App42Exception</exception>

        public Reward RedeemRewards(String gameName, String gameUserName,
                String rewardName, Double rewardPoints)
        {
            String response = null;
            Reward reward = null;

            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(gameUserName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(rewardName, "Reward Name");
            Util.ThrowExceptionIfNullOrBlank(rewardPoints, "Reward Points");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                  Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("gameName");
            jsonWriter.WriteValue(gameName);
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(gameUserName);
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(rewardName);
            jsonWriter.WritePropertyName("points");
            jsonWriter.WriteValue(rewardPoints);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"rewards\":{\"reward\":")
                    .Append(sbJson.ToString()).Append("}}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/redeem";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());

            reward = new RewardResponseBuilder().BuildResponse(response);
            return reward;
        }


        /// <summary>
        /// Fetches the reward points for a particular user
        /// </summary>
        /// <param name="gameName">Name of the game for which reward points have to be fetched</param>
        /// <param name="userName">The user for whom reward points have to be fetched</param>
        /// <returns>Reward object containing the reward points for the specified user</returns>
        /// <exception>App42Exception</exception>

        public Reward GetGameRewardPointsForUser(String gameName, String userName)
        {
            String response = null;
            Reward reward = null;

            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");


            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("gameName", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
            resourceURL, queryParams);
            reward = new RewardResponseBuilder().BuildResponse(response);
            return reward;
        }
        /// <summary>
        /// Fetches all the Rewards by paging.
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>IList of Reward objects containing all the rewards of the App</returns>
        /// <exception>App42Exception</exception>

        public IList<Reward> GetAllRewards(int max, int offset)
        {
            String response = null;
            IList<Reward> rewardList = null;
            Util.ValidateMax(max);
     ///       Util.ThrowExceptionIfNullOrBlank(max, "Max");
     ///       Util.ThrowExceptionIfNullOrBlank(offset, "Offset");

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
            rewardList = new RewardResponseBuilder()
                    .BuildArrayRewards(response);
            return rewardList;

        }


        /// <summary>
        /// Fetches all the Rewards
        /// </summary>
        /// <returns>IList of Reward objects containing all the rewards of the App</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAllRewardsCount()
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
            String resourceURL = this.version + "/" + this.resource + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new RewardResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}