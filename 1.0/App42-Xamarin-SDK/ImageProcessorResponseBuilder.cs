using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.imageProcessor
{
    public class ImageProcessorResponseBuilder : App42ResponseBuilder
    {

        public Image BuildResponse(String json)
        {
            JObject imageJSONObj = GetServiceJSONObject("image", json);
            Image imageObj = new Image();
            imageObj.SetStrResponse(json);
            imageObj.SetResponseSuccess(IsResponseSuccess(json));
            BuildObjectFromJSONTree(imageObj, imageJSONObj);
            return imageObj;
        }
    }
}