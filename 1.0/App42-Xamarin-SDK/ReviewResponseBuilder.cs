using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.review
{
    public class ReviewResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">json</param>
        /// <returns></returns
        public Review BuildResponse(String json)
        {
            JObject reviewsJSONObject = GetServiceJSONObject("reviews", json);
            JObject reviewJSONObject = (JObject)reviewsJSONObject["review"];
            Review reviewObj = new Review();
            reviewObj.SetStrResponse(json);
            reviewObj.SetResponseSuccess(IsResponseSuccess(json));
            BuildObjectFromJSONTree(reviewObj, reviewJSONObject);

            return reviewObj;
        }


        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">json</param>
        /// <returns></returns
        public IList<Review> BuildArrayResponse(String json)
        {
            IList<Review> reviewList = new List<Review>();
            JObject reviewsJSONObject = GetServiceJSONObject("reviews", json);
            if (reviewsJSONObject["review"] != null && reviewsJSONObject["review"] is JObject)
            {
                //Single Object
                JObject reviewJSONObject = (JObject)reviewsJSONObject["review"];
                Review reviewObj = new Review();
                reviewObj.SetStrResponse(json);
                reviewObj.SetResponseSuccess(IsResponseSuccess(json));
                BuildObjectFromJSONTree(reviewObj, reviewJSONObject);
                reviewList.Add(reviewObj);

            }
            else
            {
                //Multiple Object 
                JArray reviewJSONArray = (JArray)reviewsJSONObject["review"];
                for (int    i = 0; i < reviewJSONArray.Count(); i++)
                {
                    JObject reviewJSONObj = (JObject)reviewJSONArray[i];
                    Review reviewObj = new Review();
                    reviewObj.SetStrResponse(json);
                    reviewObj.SetResponseSuccess(IsResponseSuccess(json));
                    BuildObjectFromJSONTree(reviewObj, reviewJSONObj);
                    reviewList.Add(reviewObj);

                }
            }

            return reviewList;
        }
    }
}
