using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    public class Catalogue : App42Response
    {

        public String name;
        public String description;

        public String GetDescription()
        {
            return description;
        }

        public void SetDescription(String description)
        {
            this.description = description;
        }

        public IList<Category> categoryList = new List<Catalogue.Category>();

        public String GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public IList<Category> GetCategoryList()
        {
            return categoryList;
        }

        public void SetCategoryList(IList<Category> categoryList)
        {
            this.categoryList = categoryList;
        }

        public class Category
        {

            public Category(Catalogue category)
            {
                category.categoryList.Add(this);
            }
            public String name;
            public String description;
            public IList<Item> itemList = new List<Catalogue.Category.Item>();

            public String GetDescription()
            {
                return description;
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

            public IList<Item> GetItemList()
            {
                return itemList;
            }

            public void SetItemList(IList<Item> itemList)
            {
                this.itemList = itemList;
            }

            public override String ToString()
            {
                return " name : " + name + " : description : " + description;
            }

            public class Item
            {

                public Item(Category item)
                {
                    item.itemList.Add(this);
                }
                public String itemId;
                public String name;
                public String description;
                public String url;
                public String tinyUrl;
                public Double price;

                public String GetItemId()
                {
                    return itemId;
                }
                public void SetItemId(String itemId)
                {
                    this.itemId = itemId;
                }
                public String GetName()
                {
                    return name;
                }
                public void SetName(String name)
                {
                    this.name = name;
                }
                public String GetDescription()
                {
                    return description;
                }
                public void SetDescription(String description)
                {
                    this.description = description;
                }
                public String GetUrl()
                {
                    return url;
                }
                public void SetUrl(String url)
                {
                    this.url = url;
                }
                public String GetTinyUrl()
                {
                    return tinyUrl;
                }
                public void SetTinyUrl(String tinyUrl)
                {
                    this.tinyUrl = tinyUrl;
                }
                public Double GetPrice()
                {
                    return price;
                }
                public void GetPrice(Double price)
                {
                    this.price = price;
                }

                public override String ToString()
                {
                    return " itemId : " + itemId + " : name : " + name
                            + " : description : " + description + " : url : " + url + " : price : " + price;
                }
            }
        }
    }
}