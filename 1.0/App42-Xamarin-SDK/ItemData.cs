using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
     public class ItemData
    {

        private String itemId;
        private String name;
        private String description;
        private String image;
        private Double price;
        private Stream imageStream;
        private String imageName;

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
        public String GetImage()
        {
            return image;
        }
        public void SetImage(String image)
        {
            this.image = image;
        }
        public Double GetPrice()
        {
            return price;
        }
        public void SetPrice(Double price)
        {
            this.price = price;
        }
        public Stream GetImageStream()
        {
            return imageStream;
        }
        public void SetImageInputStream(Stream imageStream)
        {
            this.imageStream = imageStream;
        }

        public String GetImageName()
        {
            return imageName;
        }
        public void SetImageName(String imageName)
        {
            this.imageName = imageName;
        }
    }
}
