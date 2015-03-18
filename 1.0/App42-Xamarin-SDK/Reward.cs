using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.reward
{
    public class Reward : App42Response
    {

        public String gameName;
        public String userName;
        public String name;
        public Double points;
        public String description;

        public String GetGameName()
        {
            return gameName;
        }
        public void SetGameName(String gameName)
        {
            this.gameName = gameName;
        }
        public String GetUserName()
        {
            return userName;
        }
        public void SetUserName(String userName)
        {
            this.userName = userName;
        }
        public String GetName()
        {
            return name;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public Double GetPoints()
        {
            return points;
        }
        public void SetPoints(Double points)
        {
            this.points = points;
        }
        public String GetDesription()
        {
            return this.description;
        }

        public void SetDescription(String description)
        {
            this.description = description;
        }
    }
}
