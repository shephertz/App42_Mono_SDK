using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.recommend
{
    public class Recommender : App42Response
    {
        public String fileName;

        public IList<RecommendedItem> recommendedItemList = new List<Recommender.RecommendedItem>();


        public IList<RecommendedItem> GetRecommendedItemList()
        {
            return recommendedItemList;
        }

        public void SetRecommendedItemList(
                IList<RecommendedItem> recommendedItemList)
        {
            this.recommendedItemList = recommendedItemList;
        }

        public String GetFileName()
        {
            return fileName;
        }

        public void SetFileName(String fileName)
        {
            this.fileName = fileName;
        }

        public class RecommendedItem
        {

            public String userId;
            public String item;
            public Double value;

            public RecommendedItem(Recommender recommender)
            {
               recommender.recommendedItemList.Add(this);
            }

            public String GetUserId()
            {
                return userId;
            }
            public void SetUserId(String userId)
            {
                this.userId = userId;
            }
            public String GetItem()
            {
                return item;
            }
            public void SetItem(String item)
            {
                this.item = item;
            }
            public Double GetValue()
            {
                return value;
            }
            public void SetValue(Double value)
            {
                this.value = value;
            }

            public override String ToString()
            {
                return "userId : " + this.userId + ": item : " + this.item + " : value : " + this.value;
            }
        }
    }
}