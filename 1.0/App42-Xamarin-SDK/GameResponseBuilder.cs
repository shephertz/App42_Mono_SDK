using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.game
{
    public class GameResponseBuilder : App42ResponseBuilder
    {

        public Game BuildResponse(String json)
        {
            JObject gamesJSONObj = GetServiceJSONObject("games", json);
            JObject gameJSONObj = (JObject)gamesJSONObj["game"];
            Game game = BuildGameObject(gameJSONObj);
            game.SetResponseSuccess(IsResponseSuccess(json));
            game.SetStrResponse(json);
            return game;
        }

        public IList<Game> BuildArrayResponse(String json)
        {
            JObject gamesJSONObj = GetServiceJSONObject("games", json);
           
            IList<Game> gameList = new List<Game>();
            if (gamesJSONObj["game"] != null && gamesJSONObj["game"] is JObject)
            {


                //Single Object
                JObject gameJSONObj = (JObject)gamesJSONObj["game"];
                Game game = BuildGameObject(gameJSONObj);
                BuildObjectFromJSONTree(game, gameJSONObj);
                game.SetResponseSuccess(IsResponseSuccess(json));
                game.SetStrResponse(json);
                gameList.Add(game);
            }

            else
            {
                //Multiple Object 
                JArray gameJSONArray = (JArray)gamesJSONObj["game"];
                for (int i = 0; i < gameJSONArray.Count; i++)
                {
                    JObject gameJSONObj = (JObject)gameJSONArray[i];
                    Game game = BuildGameObject(gameJSONObj);
                    game.SetResponseSuccess(IsResponseSuccess(json));
                    game.SetStrResponse(json);
                    gameList.Add(game);
                }
              
            }
            return gameList;
        }

        private Game BuildGameObject(JObject gameJSONObject)
        {
            Game game = new Game();
            BuildObjectFromJSONTree(game, gameJSONObject);
            if (gameJSONObject["scores"] != null && gameJSONObject["scores"]["score"] != null)
            {
                if (gameJSONObject["scores"]["score"] is JObject)
                {
                    JObject scoreJSONObj = (JObject)gameJSONObject["scores"]["score"];
                    Game.Score score = new Game.Score(game);
                    BuildObjectFromJSONTree(score, scoreJSONObj);
                }
                else
                {
                    //Fetch Array of Game
                    JArray scoreJSONArray = (JArray)gameJSONObject["scores"]["score"];
                    for (int i = 0; i < scoreJSONArray.Count; i++)
                    {
                        JObject scoreJSONObj = (JObject)scoreJSONArray[i];
                        Game.Score score = new Game.Score(game);
                        BuildObjectFromJSONTree(score, scoreJSONObj);
                    }
                }
            }
            return game;
        }
    }
}
