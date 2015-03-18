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
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.game
{
    /// <summary>
    /// ScoreBoard allows storing, retrieving, querying and ranking scores for users and Games across
    /// Game Session.
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
    /// <see cref="ScoreBoardService">Game, RewardPoint, Score, ScoreBoard</see>

    public class ScoreBoardService
    {

        private String version = "1.0";
        private String resource = "game/scoreboard";
        private String apiKey;
        private String secretKey;
        String baseURL;

        /// <summary>
        /// this is a constructor that takes
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>

        public ScoreBoardService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }

        /// <summary>
        /// Saves the User score  for a game
        /// </summary>
        /// <param name="gameName">Name of the game for which score has to be saved</param>
        /// <param name="gameUserName">The user for which score has to be saved</param>
        /// <param name="gameScore">The sore that has to be saved</param>
        /// <returns>The saved score for a game</returns>
        /// <exception>App42Exception</exception>
        public Game SaveUserScore(String gameName, String gameUserName,
                Double gameScore)
        {

            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(gameUserName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(gameScore, "Game Score");

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
            jsonWriter.WriteValue(gameUserName);
            jsonWriter.WritePropertyName("value");
            jsonWriter.WriteValue(gameScore);
            jsonWriter.WriteEndObject();
            StringBuilder sbJson1 = new StringBuilder();
            StringWriter sw1 = new StringWriter(sbJson1);
            JsonWriter jsonWriter1 = new JsonTextWriter(sw1);
            jsonWriter1.WriteStartObject();
            jsonWriter1.WritePropertyName("score");
            jsonWriter1.WriteValue(sbJson.ToString());
            jsonWriter1.WriteEndObject();
            StringBuilder sbJson2 = new StringBuilder();
            StringWriter sw2 = new StringWriter(sbJson2);
            JsonWriter jsonWriter2 = new JsonTextWriter(sw2);
            jsonWriter2.WriteStartObject();
            jsonWriter2.WritePropertyName("scores");
            jsonWriter2.WriteValue(sbJson1.ToString());
            jsonWriter2.WritePropertyName("name");
            jsonWriter2.WriteValue(gameName);
            jsonWriter2.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"game\":").Append(sbJson2).Append("}}");
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// <summary>
        /// Retrieves the scores for a game for the specified name
        /// </summary>
        /// <param name="gameName">Name of the game for which score has to be fetched</param>
        /// <param name="userName">The user for which score has to be fetched</param>
        /// <returns>The game score for the specified user</returns>
        /// <exception>App42Exception</exception>
        public Game GetLastScoresByUser(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>
        /// Retrieves the highest game score for the specified user
        /// </summary>
        /// <param name="gameName">Name of the game for which highest score has to be fetched</param>
        /// <param name="userName">The user for which highest score has to be fetched</param>
        /// <returns>The highest game score for the specified user</returns>
        /// <exception>App42Exception</exception>
        public Game GetHighestScoreByUser(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName + "/highest";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;

        }

        /// <summary>
        /// Retrieves the lowest game score for the specified user
        /// </summary>
        /// <param name="gameName">Name of the game for which lowest score has to be fetched</param>
        /// <param name="userName">The user for which lowest score has to be fetched</param>
        /// <returns>The lowest game score for the specified user</returns>
        /// <exception>App42Exception</exception>
        public Game GetLowestScoreByUser(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName + "/lowest";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                       resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>
        /// Retrieves the Top Rankings for the specified game
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <returns>The Top rankings for a game</returns>
        /// <exception>App42Exception</exception>
        public Game GetTopRankings(String gameName)
        {
            String response = null;
            Game game = null;

            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/ranking";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// </summary>
        /// <param name="gameName">Name of the game for which average score has to be fetched</param>
        /// <param name="userName">The user for which average score has to be fetched</param>
        /// <returns>The average game score for the specified user</returns>
        /// <exception>App42Exception</exception>
        public Game GetAverageScoreByUser(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName + "/average";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// <summary>  
        /// Retrieves the Top Rankings for the specified game 
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <param name="startDate">Start date from which the ranking have to be fetched</param>
        /// <param name="endDate">End date up to which the ranking have to be fetched</param>
        /// <returns>the Top rankings for a game</returns>
        /// <exception>App42Exception</exception>
        public Game GetTopRankings(String gameName, DateTime startDate, DateTime endDate)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String strStartDate = Util.GetUTCFormattedTimestamp(startDate);
            String strEndDate = Util.GetUTCFormattedTimestamp(endDate);
            paramsDics.Add("name", gameName);
            paramsDics.Add("startDate", strStartDate);
            paramsDics.Add("endDate", strEndDate);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/ranking" + "/" + strStartDate + "/"
                    + strEndDate;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>  
        /// Retrieves the Top Rankings for the specified game 
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <param name="startDate">Start date from which the ranking have to be fetched</param>
        /// <param name="endDate">End date up to which the ranking have to be fetched</param>
        /// <returns>the Top rankings for a game</returns>
        /// <exception>App42Exception</exception>
        public Game GetLastScoreByUser(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                        + gameName + "/" + userName + "/lastscore";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>  
        /// Retrieves the Top Rankings for the specified game 
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <param name="startDate">Start date from which the ranking have to be fetched</param>
        /// <param name="endDate">End date up to which the ranking have to be fetched</param>
        /// <returns>the Top rankings for a game</returns>
        /// <exception>App42Exception</exception>
        public Game GetTopRankingsByGroup(String gameName, IList<String> userList)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userList, "UserList");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            
            JArray userArray = new JArray();
            for (int i = 0; i < userList.Count(); i++)
            {
                String user = (String)userList[i];
                userArray.Add(user.ToString());
            }
            jsonWriter.WriteEndObject();
            paramsDics.Add("userList", userArray.ToString());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                       + gameName + "/group";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// <summary>
        /// Retrieves the Top Rankings for the specified game
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <param name="max"></param>
        /// <returns>The Top rankings for a game</returns>
        /// <exception>App42Exception</exception>
        public Game GetTopNRankings(String gameName, int max)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(max, "Max");
            Util.ValidateMax(max);

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            paramsDics.Add("max", "" + max);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/ranking" + "/" + max;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>
        /// Retrieves the User Ranking for the specified game
        /// </summary>
        /// <param name="gameName">Name of the game for which ranks have to be fetched</param>
        /// <param name="userName">Name of the user for which ranks have to be fetched</param>
        /// <returns>The rank of the User</returns>
        /// <exception>App42Exception</exception>
        public Game GetUserRanking(String gameName, String userName)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            paramsDics.Add("userName", "" + userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/" + userName + "/ranking";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Game GetLastGameScore(String userName)
        {

            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                   + userName + "/lastgame";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="max"></param>
        /// <returns></returns>

        public Game GetTopNRankers(String gameName, int max)
        {
            String response = null;
            Game game = null;
            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(max, "Max");
            Util.ValidateMax(max);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", gameName);
            paramsDics.Add("max", "" + max);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + gameName + "/rankers" + "/" + max;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
    }
}