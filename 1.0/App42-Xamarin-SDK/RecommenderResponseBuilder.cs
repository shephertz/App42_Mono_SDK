using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.recommend
{
    public class RecommenderResponseBuilder : App42ResponseBuilder
    {
        public Recommender BuildResponse(String json)
        {
           
            IList<Recommender.RecommendedItem> recommendedItemList = new List<Recommender.RecommendedItem>();
            Recommender recommenderObj = new Recommender();
            recommenderObj.SetRecommendedItemList(recommendedItemList);
            recommenderObj.SetStrResponse(json);
            App42Log.Debug("json" + json);
            JObject jsonObj = JObject.Parse(json);
            JObject jsonObjApp42 = (JObject)jsonObj["app42"];
            JObject jsonObjResponse = (JObject)jsonObjApp42["response"];
            recommenderObj.SetResponseSuccess((Boolean)jsonObjResponse["success"]);
            JObject jsonObjRecommender = (JObject)jsonObjResponse["recommender"];
            BuildObjectFromJSONTree(recommenderObj, jsonObjRecommender);
            if (jsonObjRecommender["recommended"] == null)
                return recommenderObj;
            if (jsonObjRecommender["recommended"] is JObject)
            {
                // Only One attribute is there
                JObject jsonObjRecommended = (JObject)jsonObjRecommender["recommended"];
                Recommender.RecommendedItem recomItem = new Recommender.RecommendedItem(recommenderObj);
                BuildObjectFromJSONTree(recomItem, jsonObjRecommended);
            }
            else
            {
                // There is an Array of attribute
                JArray jsonObjRecommenderArray = (JArray)jsonObjRecommender["recommended"];
                for (int    i = 0; i < jsonObjRecommenderArray.Count; i++)
                {
                    // Get Individual Attribute Node and set it into Object
                    JObject jsonObjRecommended = (JObject)jsonObjRecommenderArray[i];
                    Recommender.RecommendedItem recomItem = new Recommender.RecommendedItem(recommenderObj);
                    BuildObjectFromJSONTree(recomItem, jsonObjRecommended);
                }
            }
            return recommenderObj;
        }
        public static void main(String[] args)
        {
            Console.Write(new RecommenderResponseBuilder().BuildResponse("{\"app42\":{\"response\":{\"success\":true,\"recommender\":{\"recommended\":{\"item\":104,\"value\":4.257081}}}}}"));
        }
    }
}
