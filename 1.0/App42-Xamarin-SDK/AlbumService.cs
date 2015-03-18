using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;


namespace com.shephertz.app42.paas.sdk.csharp.gallery
{
    /// <summary>
    /// Create Photo Gallery on the cloud. This service allows to manage i.e. create, retrieve and remove albums on the 
    /// cloud. Its useful for Mobile/Device App and Web App developer who want Photo Gallery functionality. It gives them
    /// a complete Photo Gallery out of the box and reduces the footprint on the device. Developers can focus on how the
    /// Photo Gallery will be rendered and this Cloud API will manage the Gallery on the cloud thereby reducing development time.
    /// <see cref="albumService">Album</see>
    /// <see cref="albumService">Photo</see>
    /// </summary>
    public class AlbumService
    {
        private Config config;
        private String version = "1.0";
        private String resource = "gallery";
        private String apiKey;
        private String secretKey;
        private String baseURL;

        /// <summary>
        ///  The costructor for the Service
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>
        public AlbumService(String apiKey, String secretKey, String baseURL)
        {
            config = Config.GetInstance();
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }
        /// <summary>
        /// Creates Album on the cloud.
        /// </summary>
        /// <param name="userName">The user to which the album belongs.</param>
        /// <param name="albumName">Name of the album to be created on the cloud.</param>
        /// <param name="albumDescription">Description of the album to be created.</param>
        /// <returns>Album object containing the album which has been created.</returns>
        /// <exception>App42Excpetion</exception>
        public Album CreateAlbum(String userName, String albumName, String albumDescription)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(albumDescription, "Description");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("name");
            jsonWriter.WriteValue(albumName);
            jsonWriter.WritePropertyName("description");
            jsonWriter.WriteValue(albumDescription);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"album\":").Append(sbJson.ToString()).Append("}}");
            App42Log.Debug(" Json String : " + sb.ToString());
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/album"
                    + "/" + userName;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        /// <summary>
        /// Fetches all the Albums based on the userName.
        /// </summary>
        /// <param name="userName">The user for which the albums have to be fetched.</param>
        /// <returns>List of Album object containing all the album for the given userName</returns>
        /// <exception>App42Exception</exception>
        public IList<Album> GetAlbums(String userName)
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
            String resourceURL = this.version + "/" + this.resource + "/album"
                    + "/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            albumList = new AlbumResponseBuilder().BuildArrayResponse(response);
            return albumList;
        }
        /// <summary>
        /// Fetches all the Albums based on the userName by Paging.
        /// </summary>
        /// <param name="userName">The user for which the albums have to be fetched.</param>
        /// <param name="max">Maximum number of records to be fetched.</param>
        /// <param name="offset">From where the records are to be fetched.</param>
        /// <returns>List of Album object containing all the album for the given userName</returns>
        /// <exception>App42Exception</exception>
        public IList<Album> GetAlbums(String userName, int max, int offset)
        {
            String response = null;
            IList<Album> albumList = null;
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("userName", userName);
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/album"
                      + "/" + userName + "/" + max + "/" + offset;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            albumList = new AlbumResponseBuilder().BuildArrayResponse(response);
            return albumList;
        }
        /// <summary>
        /// Fetch all Albums based on userName and albumName.
        /// </summary>
        /// <param name="userName">The user for which the album has to be fetched.</param>
        /// <param name="albumName">Name of the album that has to be fetched.</param>
        /// <returns>Album object containing album information for the given userName and albumName</returns>
        /// <exception>App42Exception</exception>
        public Album GetAlbumByName(String userName, String albumName)
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
            String resourceURL = this.version + "/" + this.resource + "/album"
                    + "/" + userName + "/" + albumName;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        /// <summary>
        /// Removes a particular album based on the userName and albumName. Note: All photos added to this Album will also be removed.
        /// </summary>
        /// <param name="userName">The user for which the album has to be removed.</param>
        /// <param name="albumName">Name of the album that has to be removed.</param>
        /// <returns>App42Response if removed successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveAlbum(String userName, String albumName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
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
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Fetches the count of all the Albums based on the userName.
        /// </summary>
        /// <param name="userName">The user for which the count of albums have to be fetched.</param>
        /// <returns>App42Response object containing the count of all the albums for the given userName</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAlbumsCount(String userName)
        {
            String response = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            App42Response responseObj = new App42Response();

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/album"
                    + "/" + userName + "/" + "count";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new AlbumResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}