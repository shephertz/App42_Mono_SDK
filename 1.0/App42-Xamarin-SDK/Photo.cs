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
    /**
      * Adds Photo to the created Album on the Cloud
      * All photos for a given Album can be managed through this service. Photos can be uploaded
      * to the cloud. Uploaded photos are accessible through the generated URL.
      * The service also creates a thumbnail for the Photo which has been uploaded.
      * @see Album
      */
    public class Photo
    {
        private String version = "1.0";
        private String resource = "gallery";
        private String apiKey;
        private String secretKey;
        /**
         * The costructor for the Service
         * @param apiKey
         * @param secretKey
         * @param baseURL
         */
        public Photo(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        public String testUpload(String path)
        {
            return null;
        }
        /**
         *Adds Photo for a particular user and album. The Photo is uploaded on the cloud
         * @param userName Name of the User whose photo has to be added
         * @param albumName Name of the Album in which photo has to be added
         * @param photoName Name of the Photo that has to be added
         * @param photoDescription Description of the Photo that has to be added
         * @param path Path from where Photo has to be picked for addition
         * @return Returns the Photo which has been added
         */
        public Album AddPhoto(String userName, String albumName, String photoName,
              String photoDescription, String path)
        {
            String response = null;
            Album album = null;
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(albumName, "Album Name");
            Util.ThrowExceptionIfNullOrBlank(photoName, "Photo Name");
            Util.ThrowExceptionIfNullOrBlank(photoDescription, "Description");
            Util.ThrowExceptionIfNullOrBlank(path,"Path");
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
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/" + userName;
            response = Util.MultiPartRequest("imageFile", path, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
        /**
       * Fetch all the Photos based on the userName
       * @param userName Name of the User whose photos have to be fetched
       * @return Returns all the Photos for the given userName
       */
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
        /**
         * Fetch all Photos based on the userName and album name
         * @param userName Name of the User whose photos have to be fetched
         * @param albumName Name of the Album from which photos have to be fetched
         * @return Returns all the Photos for the given userName and albumName
         */
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
        /**
         * Fetch the particular photo based on the userName, album name and photo name
         * @param userName Name of the User whose photo has to be fetched
         * @param albumName Name of the Album from which photo has to be fetched
         * @param photoName Name of the Photo that has to be fetched
         * @return Returns the Photo for the given userName, albumName and photoName
         */
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
        /**
         * Removes the particular Photo from the specified Album for a particular user.
         * Note: The Photo is removed from the cloud and wont be accessible in future
         * @param userName Name of the User whose photo has to be removed
         * @param albumName Name of the Album in which photo has to be removed
         * @param photoName Name of the Photo that has to be removed
         * @return Returns the Photo which has been removed
         */
        public Album RemovePhoto(String userName, String albumName,
            String photoName)
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
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            album = new AlbumResponseBuilder().BuildResponse(response);
            return album;
        }
    }
}