using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;


namespace com.shephertz.app42.paas.sdk.csharp.upload
{
    /// <summary>
    /// UploadResponseBuilder class converts the JSON response retrieved from the
    /// server to the value object i.e User
    /// </summary>
    public class UploadResponseBuilder : App42ResponseBuilder
    {
        /// <summary>
        /// Converts the response in JSON format to the value object i.e Upload	 * 
        /// </summary>
        /// <param name="json">Response in JSON format</param>
        /// <returns>Upload object filled with json data</returns>
        public Upload BuildResponse(String json)
        {
            Upload uploadObj = new Upload();

            uploadObj.SetStrResponse(json);
            uploadObj.SetResponseSuccess(IsResponseSuccess(json));
            JObject jsonObjUpload = GetServiceJSONObject("upload", json);

            // Get File Item Array
            JObject jsonObjFiles = (JObject)jsonObjUpload["files"];

            if (jsonObjFiles["file"] is JObject)
            {
                //
                JObject jsonObjFile = (JObject)jsonObjFiles["file"];
                Upload.File fileObj = new Upload.File(uploadObj);
                BuildObjectFromJSONTree(fileObj, jsonObjFile);

            }
            else
            {
                JArray jsonObjFileArray = (JArray)jsonObjFiles["file"];
                for (int i = 0; i < jsonObjFileArray.Count; i++)
                {
                    Upload.File fileObj = new Upload.File(uploadObj);
                    JObject jsonObjFile = (JObject)jsonObjFileArray[i];
                    BuildObjectFromJSONTree(fileObj, jsonObjFile);
                }
            }
            return uploadObj;
        }
    }
}