using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;

namespace com.shephertz.app42.paas.sdk.csharp.gallery
{
    /// <summary>
    /// Adds Photo to the created Album on the Cloud
    /// All photos for a given Album can be managed through this service. Photos can be uploaded
    /// to the cloud. Uploaded photos are accessible through the generated URL.
    /// The service also creates a thumbnail for the Photo which has been uploaded.
    /// </summary>
    /// <see cref="PhotoService">Album</see>
    /// <see cref="PhotoService">Photo</see>

    public class PhotoService
    {
        private String version = "1.0";
        private String resource = "gallery";
        private String apiKey;
        private String secretKey;
        /// <summary>
        /// The costructor for the Service
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>
        public PhotoService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Adds Photo for a particular user and album. The Photo is uploaded on the cloud
        /// </summary>
        /// <param name="userName"> Name of the User whose photo has to be added</param>
        /// <param name="albumName"> Name of the Album in which photo has to be added</param>
        /// <param name="photoName"> Name of the Photo that has to be added</param>
        /// <param name="photoDescription"> Description of the Photo that has to be added</param>
        /// <param name="path"> Path from where Photo has to be picked for addition</param>
        /// <returns>Album object containing the Photo which has been added</returns>
        /// <exception>App42Exception</exception>

        public Album AddPhoto(String userName, String albumName, String photoName, String photoDescription, String path)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Util.ThrowExceptionIfNullOrBlank(photoDescription, "Description");
            Util.ThrowExceptionIfNullOrBlank(path, "Path");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("userName", userName);
            postParams.Add("albumName", albumName);
            postParams.Add("name", photoName);
            postParams.Add("description", photoDescription);
            // Put these params for signing
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            paramsDics.Add("name", photoName);
            paramsDics.Add("description", photoDescription);
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL() + version + "/" + resource + "/" + userName;
            response = Util.MultiPartRequest("imageFile", path, queryParams, postParams, resourceUrl, Config.GetInstance().GetAccept());
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        /// <summary>
        /// Adds Photo for a particular user and album. The Photo is uploaded on the cloud
        /// </summary>
        /// <param name="userName"> Name of the User whose photo has to be added</param>
        /// <param name="albumName"> Name of the Album in which photo has to be added</param>
        /// <param name="photoName"> Name of the Photo that has to be added</param>
        /// <param name="photoDescription"> Description of the Photo that has to be added</param>
        /// <param name="stream">Stream for the Photo that has to be added</param>
        /// <returns>Album object containing the Photo which has been added</returns>
        /// <exception>App42Exception</exception>

        public Album AddPhoto(String userName, String albumName, String photoName, String photoDescription, Stream stream)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Util.ThrowExceptionIfNullOrBlank(photoDescription, "Description");
           // Util.ThrowExceptionIfNullOrBlank(path, "Path");
            Util.ThrowExceptionIfNotValidExtension(photoName, "Photo Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("userName", userName);
            postParams.Add("albumName", albumName);
            postParams.Add("name", photoName);
            postParams.Add("description", photoDescription);
            // Put these params for signing
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            paramsDics.Add("name", photoName);
            paramsDics.Add("description", photoDescription);
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL() + version + "/" + resource + "/" + userName;
            response = Util.MultiPartRequest("imageFile", stream,photoName, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        ///// <summary>
        ///// Adds Photo for a particular user and album. The Photo is uploaded on the cloud
        ///// </summary>
        ///// <param name="userName"> Name of the User whose photo has to be added</param>
        ///// <param name="albumName"> Name of the Album in which photo has to be added</param>
        ///// <param name="photoName"> Name of the Photo that has to be added</param>
        ///// <param name="photoDescription"> Description of the Photo that has to be added</param>
        ///// <param name="path"> Path from where Photo has to be picked for addition</param>
        ///// <returns>Album object containing the Photo which has been added</returns>
        ///// <exception>App42Exception</exception>

        //public Album AddPhoto(String userName, Stream stream, String albumName, String photoName, String photoDescription, String path)
        //{
        //    String response = null;
        //    Album album = null;
        //    Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
        //    Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
        //    Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
        //    Util.ThrowExceptionIfNullOrBlank(photoDescription, "Description");
        //    Util.ThrowExceptionIfNullOrBlank(path, "Path");
        //    Dictionary<String, String> queryParams = new Dictionary<String, String>();
        //    queryParams.Add("apiKey", this.apiKey);
        //    queryParams.Add("version", this.version);
        //    queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
        //    Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
        //    Dictionary<String, String> postParams = new Dictionary<String, String>();
        //    postParams.Add("userName", userName);
        //    postParams.Add("albumName", albumName);
        //    postParams.Add("name", photoName);
        //    postParams.Add("description", photoDescription);
        //    // Put these params for signing
        //    paramsDics.Add("userName", userName);
        //    paramsDics.Add("albumName", albumName);
        //    paramsDics.Add("name", photoName);
        //    paramsDics.Add("description", photoDescription);
        //    String signature = Util.Sign(this.secretKey, paramsDics);
        //    queryParams.Add("signature", signature);
        //    String resourceUrl = Config.GetInstance().GetBaseURL() + version + "/" + resource + "/" + userName;
        //    response = Util.MultiPartRequest("imageFile", photoName, stream, queryParams,
        //            postParams, resourceUrl, Config.GetInstance().GetAccept());
        //    album = new AlbumResponseBuilder().BuildResponse(response);
        //    return album;
        //}
        ///<summery> Adds tag to the Photo of the user in the album.</summery>
        ///<param name= userName> Name of the User whose name has to be tagged to photo</param>
        ///<param name= albumName>  Album name whose photo is to be tagged</param>
        ///<param name= photoName> Photo name to be tagged</param>
        ///<param name= tagList>  list of tages in Photo</param>
        ///<returns>Album object containing the Photo which has been added </returns>
        /// <exception>App42Exception</exception>

        public Album AddTagToPhoto(String userName, String albumName,
        String photoName, IList<String> tagList)
        {
            String response = null;
            Album album = null;

            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Util.ThrowExceptionIfNullOrBlank(tagList, "TagList");
            if (tagList.Count == 0)
            {
                throw new App42Exception("TagList cannot be empty.Please add the name to be tagged");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(userName);
            jsonWriter.WritePropertyName("albumName");
            jsonWriter.WriteValue(albumName);
            jsonWriter.WritePropertyName("photoName");
            jsonWriter.WriteValue(photoName);

            JArray tagArray = new JArray();
            for (int i = 0; i < tagList.Count(); i++)
            {
                String tag = (String)tagList[i];
                tagArray.Add(tag.ToString());
            }
            jsonWriter.WritePropertyName("tags");
            jsonWriter.WriteValue("{\"tag\":" + tagArray + "}");
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"app42\":{\"photo\":").Append(sbJson.ToString())
            .Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/tag";
            response = RESTConnector.getInstance().ExecutePost(signature,
                      resourceURL, queryParams, sb.ToString());
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }

        /// <summary>
        /// Fetch all the Photos based on the userName
        /// </summary>
        /// <param name="userName"> Name of the User whose photos have to be fetched</param>
        /// <returns>IList of Album object containing all the Photos for the given userName</returns>
        /// <exception>App42Exception</exception>

        public IList<Album> GetPhotos(String userName)
        {
            String response = null;
            IList<Album> albumList = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            albumList = new AlbumResponseBuilder().BuildArrayResponse(response);
            return albumList;
        }


        /// <summary>
        /// Fetch all the Photos based on the userName
        /// </summary>
        /// <param name="userName"> Name of the User whose photos have to be fetched</param>
        /// <param tag="tag"> Tag on which basis photos have to be fetched</param>
        /// <param name="requestCallback"> Callback instance that will have the implementation of OnSucess and onException of App42Callback</param>
        /// <exception>App42Exception</exception>

        public IList<Album> GetTaggedPhotos(String userName, String tag)
        {
            String response = null;
            IList<Album> albumList = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(tag, "Tag");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("tag", tag);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                                      + "tag" + "/" + tag + "/userName/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                      resourceURL, queryParams);
            albumList = new AlbumResponseBuilder().BuildArrayResponse(response);
            return albumList;
        }
        /// <summary>
        /// Fetch all Photos based on the userName and album name
        /// </summary>
        /// <param name="userName">Name of the User whose photos have to be fetched</param>
        /// <param name="albumName"> Name of the Album from which photos have to be fetched</param>
        /// <returns>Album object containing all the Photos for the given userName and albumName</returns>
        /// <exception>App42Exception</exception>

        public Album GetPhotosByAlbumName(String userName, String albumName)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/" + albumName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        ///<summary>
        /// Fetch all Photos based on the userName and album name by paging.
        /// </summary>
        /// <param name="userName">Name of the User whose photos have to be fetched</param>
        /// <param name="albumName"> Name of the Album from which photos have to be fetched</param>
        /// <returns>Album object containing all the Photos for the given userName and albumName</returns>
        /// <exception>App42Exception</exception>

        public Album GetPhotosByAlbumName(String userName, String albumName, int max, int offset)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            ///   Util.ThrowExceptionIfNullOrBlank(max, "Max");
            ///  Util.ThrowExceptionIfNullOrBlank(offset, "Offset");
            Util.ValidateMax(max);

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource +
                "/" + "album" + "/" + userName + "/" + albumName + "/paging"
                + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }

        /// <summary>
        /// Fetch the particular photo based on the userName, album name and photo name
        /// </summary>
        /// <param name="userName"> Name of the User whose photo has to be fetched</param>
        /// <param name="albumName"> Name of the Album from which photo has to be fetched</param>
        /// <param name="photoName"> Name of the Photo that has to be fetched</param>
        /// <returns>Album object containing the Photo for the given userName, albumName and photoName</returns>
        /// <exception>App42Exception</exception>

        public Album GetPhotosByAlbumAndPhotoName(String userName,
                String albumName, String photoName)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            paramsDics.Add("name", photoName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/" + albumName + "/" + photoName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        /// <summary>
        /// Removes the particular Photo from the specified Album for a particular user.
        /// Note: The Photo is removed from the cloud and wont be accessible in future
        /// </summary>
        /// <param name="userName"> Name of the User whose photo has to be removed</param>
        /// <param name="albumName"> Name of the Album in which photo has to be removed</param>
        /// <param name="photoName"> Name of the Photo that has to be removed</param>
        /// <returns>App42Response if removed successfully</returns>
        /// <exception>App42Exception</exception>

        public App42Response RemovePhoto(String userName, String albumName,
                String photoName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            paramsDics.Add("name", photoName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/" + albumName + "/" + photoName;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }


        /// <summary>
        /// Fetch the count of all Photos based on the userName and album name
        /// </summary>
        /// <param name="userName">Name of the User whose count of photos have to be fetched</param>
        /// <param name="albumName">Name of the Album from which count of photos have to be fetched</param>
        /// <returns>App42Response object containing the count of all the Photos for the given userName and albumName</returns>

        public App42Response GetPhotosCountByAlbumName(String userName, String albumName)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            App42Response responseObj = new App42Response();

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            paramsDics.Add("albumName", albumName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + userName + "/" + albumName + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, paramsDics);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new AlbumResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}