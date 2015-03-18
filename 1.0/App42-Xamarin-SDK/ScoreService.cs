
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

namespace com.shephertz.app42.paas.sdk.csharp.game
{
    /// <summary>
    /// Allows ingame scoring. It has to be used for scoring for a parituclar Game
    /// Session. If scores have to be stored across Game sessions then the service
    /// ScoreBoard has to be used. It is especially useful for Multiplayer online or
    /// mobile games. The Game service allows Game, User, Score and ScoreBoard
    /// Management on the Cloud. The service allows Game Developer to create a Game
    /// and then do in Game Scoring using the Score service. It also allows to
    /// maintain a Scoreboard across game sessions using the ScoreBoard service. One
    /// can query for average or highest score for user for a Game and highest and
    /// average score across users for a Game. It also gives ranking of the user
    /// against other users for a particular game. The Reward and RewardPoints allows
    /// the Game Developer to assign rewards to a user and redeem the rewards. E.g.
    /// One can give Swords or Energy etc. The services Game, Score, ScoreBoard,
    /// Reward, RewardPoints can be used in Conjunction for complete Game Scoring and
    /// Reward Management.
    /// </summary>
    /// <see cref="ScoreService">Game, RewardPoint, Score, ScoreBoard</see>

    public class ScoreService
    {

        private String version = "1.0";
        private String resource = "game/score";
        private String apiKey;
        private String secretKey;
        String baseURL;
        /// <summary>
        /// Add/Deduct the score while playing the games on the cloud
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>

        public ScoreService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }


        /// <summary>
        /// Adds game score for the specified user.
        /// </summary>
        /// <param name="gameName">Name of the game for which scores have to be added</param>
        /// <param name="gameUserName">The user for whom scores have to be added</param>
        /// <param name="gameScore">The scores that have to be added</param>
        /// <returns>The scores that has been added</returns>
        /// <exception>App42Exception</exception>

        public Game AddScore(String gameName, String gameUserName,
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
            jsonWriter1.WritePropertyName("name");
            jsonWriter1.WriteValue(gameName);
            jsonWriter1.WritePropertyName("scores");
            jsonWriter1.WriteValue("{\"score\":" + sbJson + "}");
            jsonWriter1.WriteEndObject();


            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"game\":").Append(sbJson1).Append("}}");

            paramsDics.Add("body", sb.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/add";
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            game = new GameResponseBuilder().BuildResponse(response);
            return game;

        }

        /// <summary>
        /// Deducts the score from users account for a particular Game
        /// </summary>
        /// <param name="gameName">Name of the game for which scores have to be deducted</param>
        /// <param name="gameUserName">The user for whom scores have to be deducted</param>
        /// <param name="gameScore">The scores that have to be deducted</param>
        /// <returns>The scores that has been deducted</returns>
        /// <exception>App42Exception</exception>

        public Game DeductScore(String gameName, String gameUserName,
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
            jsonWriter1.WritePropertyName("name");
            jsonWriter1.WriteValue(gameName);
            jsonWriter1.WritePropertyName("scores");
            jsonWriter1.WriteValue("{\"score\":" + sbJson + "}");
            jsonWriter1.WriteEndObject();


            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"game\":").Append(sbJson1).Append("}}");

            paramsDics.Add("body", sb.ToString());

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/deduct";
            response = RESTConnector.getInstance().ExecutePost(signature, resourceURL,
                queryParams, sb.ToString());
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
    }
}