using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.game
{
    public class Game : App42Response
    {

        public String name;
        public String description;
        public IList<Score> scoreList = new List<Score>();

        public String GetDesription()
        {
            return this.description;
        }

        public void SetDescription(String description)
        {
            this.description = description;
        }

        public String GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public IList<Score> GetScoreList()
        {
            return scoreList;
        }

        public void SetScoreList(IList<Score> scoreList)
        {
            this.scoreList = scoreList;
        }

        public class Score
        {
            public Score(Game game)
            {
               game.scoreList.Add(this);
                
            }
            public String userName;
            public String rank;
            public Double value;
            public DateTime createdOn;
            public String GetUserName()
            {
                return userName;
            }
            public void SetUserName(String userName)
            {
                this.userName = userName;
            }
            public String GetRank()
            {
                return rank;
            }
            public void SetRank(String rank)
            {
                this.rank = rank;
            }
            public Double GetValue()
            {
                return value;
            }
            public void SetValue(Double value)
            {
                this.value = value;
            }
            public DateTime GetCreatedOn()
            {
                return createdOn;
            }
            public void SetCreatedOn(DateTime createdOn)
            {
                this.createdOn = createdOn;
            }

        }
    }
}
