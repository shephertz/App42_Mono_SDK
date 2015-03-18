using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.reward
{
    public class RewardResponseBuilder : App42ResponseBuilder
    {
        public Reward BuildResponse(String json)
        {
            JObject rewardsJSONObj = GetServiceJSONObject("rewards", json);
            JObject rewardJSONObj = (JObject)rewardsJSONObj["reward"];
            Reward reward = BuildRewardObject(rewardJSONObj);
            reward.SetStrResponse(json);
            reward.SetResponseSuccess(IsResponseSuccess(json));
            return reward;
        }

        private Reward BuildRewardObject(JObject rewardJSONObj)
        {
            Reward reward = new Reward();
            BuildObjectFromJSONTree(reward, rewardJSONObj);
            return reward;
        }

        public IList<Reward> BuildArrayRewards(String json)
        {
            IList<Reward> rewardList = new List<Reward>();
            JObject rewardsJSONObj = GetServiceJSONObject("rewards", json);
            if (rewardsJSONObj["reward"] != null && rewardsJSONObj["reward"] is JObject)
            {

                //Single Object
                JObject rewardJSONObj = (JObject)rewardsJSONObj["reward"];
                Reward reward = new Reward();
                reward.SetStrResponse(json);
                reward.SetResponseSuccess(IsResponseSuccess(json));
                BuildObjectFromJSONTree(reward, rewardJSONObj);
                rewardList.Add(reward);
            }

            else
            {
                //Multiple Object 
                JArray rewardsJSONArray = (JArray)rewardsJSONObj["reward"];
                for (int i = 0; i < rewardsJSONArray.Count; i++)
                {
                    JObject rewardJSONObj = (JObject)rewardsJSONArray[i];
                    Reward reward = BuildRewardObject(rewardJSONObj);
                    reward.SetStrResponse(json);
                    reward.SetResponseSuccess(IsResponseSuccess(json));
                    rewardList.Add(reward);
                }
            }
            return rewardList;
        }
    }
}
