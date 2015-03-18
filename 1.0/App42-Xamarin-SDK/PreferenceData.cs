using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.recommend
{
    public class PreferenceData
    {

        public String userId;
        public String itemId;
        public String preference;

        public String GetUserId()
        {
            return userId;
        }
        public void SetUserId(String userId)
        {
            this.userId = userId;
        }
        public String GetItemId()
        {
            return itemId;
        }
        public void SetItemId(String itemId)
        {
            this.itemId = itemId;
        }
        public String GetPreference()
        {
            return preference;
        }
        public void SetPreference(String preference)
        {
            this.preference = preference;
        }
    }
}
