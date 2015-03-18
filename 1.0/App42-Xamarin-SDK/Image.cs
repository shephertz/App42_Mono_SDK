using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.imageProcessor
{
    public class Image : App42Response
    {
        public String name;
        public String action;
        public String originalImage;
        public String convertedImage;
        public String originalImageTinyUrl;
        public String convertedImageTinyUrl;
        public Double percentage;
        public Int64 width;
        public Int64 height;
        public Int64 x;
        public Int64 y;
        public String GetName()
        {
            return name;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public String GetAction()
        {
            return action;
        }
        public void SetAction(String action)
        {
            this.action = action;
        }
        public String GetOriginalImage()
        {
            return originalImage;
        }
        public void SetOriginalImage(String originalImage)
        {
            this.originalImage = originalImage;
        }
        public String GetConvertedImage()
        {
            return convertedImage;
        }
        public void SetConvertedImage(String convertedImage)
        {
            this.convertedImage = convertedImage;
        }
        public Double GetPercentage()
        {
            return percentage;
        }
        public String GetOriginalImageTinyUrl()
        {
            return originalImageTinyUrl;
        }

        public void SetOriginalImageTinyUrl(String originalImageTinyUrl)
        {
            this.originalImageTinyUrl = originalImageTinyUrl;
        }

        public String GetConvertedImageTinyUrl()
        {
            return convertedImageTinyUrl;
        }

        public void SetConvertedImageTinyUrl(String convertedImageTinyUrl)
        {
            this.convertedImageTinyUrl = convertedImageTinyUrl;
        }
        public void SetPercentage(Double percentage)
        {
            this.percentage = percentage;
        }
        public Double GetWidth()
        {
            return width;
        }
        public void SetWidth(Int64 width)
        {
            this.width = width;
        }
        public Int64 GetHeight()
        {
            return height;
        }
        public void SetHeight(Int64 height)
        {
            this.height = height;
        }
        public Int64 GetX()
        {
            return x;
        }
        public void SetX(Int64 x)
        {
            this.x = x;
        }
        public Int64 GetY()
        {
            return y;
        }
        public void SetY(Int64 y)
        {
            this.y = y;
        }
        public String GetStringView()
        {
            return " Name : " + name + " : Action : " + action
                    + " : originalImage : " + originalImage + " :convertedImage : "
                    + convertedImage + " : percentage : " + percentage
                    + " : width : " + width + " : height  : " + height + " : x : " + x + " :y : " + y;
        }
    }
}