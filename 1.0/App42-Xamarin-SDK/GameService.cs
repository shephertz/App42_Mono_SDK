
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
    /// The Game service allows Game, User, Score and ScoreBoard Management on the Cloud.
    /// The service allows Game Developer to create a Game and then do in Game Scoring using the
    /// Score service. It also allows to maintain a Scoreboard across game sessions using the ScoreBoard
    /// service. One can query for average or highest score for user for a Game and highest and average score across users
    /// for a Game. It also gives ranking of the user against other users for a particular game.
    /// The Reward and RewardPoints allows the Game Developer to assign rewards to a user and redeem the rewards. E.g. One can give Swords or Energy etc.
    /// The services Game, Score, ScoreBoard, Reward, RewardPoints can be used in Conjunction for complete Game Scoring and Reward
    /// Management.
    /// <see cref="GameService">Reward, RewardPoint, Score, ScoreBoard</see>
    /// </summary>
    public class GameService
    {

        private String version = "1.0";
        private String resource = "game";
        private String apiKey;
        private String secretKey;
        String baseURL;

        /// <summary>
        /// this is a constructor that takes
        /// </summary>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="baseURL">baseURL</param>
        public GameService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }

        /// <summary>
        /// Creates game on the cloud
        /// </summary>
        /// <param name="gameName"> Name of the game that has to be created</param>
        /// <param name="gameDescription"> Description of the game to be created</param>
        /// <returns>Game object containing the game which has been created</returns>
        /// <exception>App42Exception</exception>

        public Game CreateGame(String gameName, String gameDescription)
        {
            String response = null;
            Game game = null;

            Util.ThrowExceptionIfNullOrBlank(gameName, "Game Name");
            Util.ThrowExceptionIfNullOrBlank(gameDescription, "Game Description");

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
            jsonWriter.WriteValue(gameName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(gameDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"game\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            game = new GameResponseBuilder().BuildResponse(response);

            return game;
        }
        /// <summary>
        /// Fetches all games for the App
        /// </summary>
        /// <returns>IList of Game object containing all the games for the App</returns>
        ///  <exception>App42Exception</exception>


        public IList<Game> GetAllGames()
        {
            String response = null;
            IList<Game> gameList = null;
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
            gameList = new GameResponseBuilder().BuildArrayResponse(response);
            return gameList;
        }


        /// <summary>
        /// Retrieves the game by the specified name
        /// </summary>
        /// <param name="gameName"> Name of the game that has to be fetched</param>
        /// <returns>Game object containing the game which has been created</returns>
        /// <exception>App42Exception</exception>

        public Game GetGameByName(String gameName)
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
                    + gameName;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            game = new GameResponseBuilder().BuildResponse(response);
            return game;
        }
        /// <summary>
        /// Fetches all games for the App by paging. Maximum number of
        /// records to be fetched
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>
        /// <returns>IList of Game object containing all the games for the App</returns>
        /// <exception>App42Exception</exception>
        public IList<Game> GetAllGames(int max, int offset)
        {

            String response = null;
            IList<Game> gameList = null;
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(max, "Max");
            Util.ThrowExceptionIfNullOrBlank(offset, "Offset");
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
            gameList = new GameResponseBuilder().BuildArrayResponse(response);
            return gameList;
        }

        /// <summary>
        /// Fetches the count of all games for the App
        /// </summary>
        /// <returns>App42Response object containing count of all the Game for the App</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAllGamesCount()
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
            responseObj.SetTotalRecords(new GameResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}