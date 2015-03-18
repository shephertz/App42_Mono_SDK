using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using System.IO;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.connection;


namespace com.shephertz.app42.paas.sdk.csharp.upload
{
    public sealed class UploadFileType
    {
        public static readonly String AUDIO = "AUDIO";
        public static readonly String VIDEO = "VIDEO";
        public static readonly String IMAGE = "IMAGE";
        public static readonly String BINARY = "BINARY";
        public static readonly String TXT = "TXT";
        public static readonly String XML = "XML";
        public static readonly String CSV = "CSV";
        public static readonly String JSON = "JSON";
        public static readonly String OTHER = "OTHER";

        public static Boolean IsValidType(String type)
        {
            if (AUDIO.Equals(type) || VIDEO.Equals(type) || IMAGE.Equals(type) || BINARY.Equals(type) || TXT.Equals(type) || XML.Equals(type) || CSV.Equals(type) || JSON.Equals(type) || OTHER.Equals(type))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

    /// <summary>
    /// Uploads file on the cloud. Allows access to the files through url. Its
    /// especially useful for mobile/device apps. It minimizes the App footprint on
    /// the device.
    /// </summary>
    /// <see cref="UploadService">Upload</see>
    /// <see cref="UploadService">App42Response</see>
    public class UploadService
    {
        private String version = "1.0";
        private String resource = "upload";
        private String apiKey;
        private String secretKey;

        /// <summary>
        /// This is a constructor that takes
        /// </summary>
        /// <param name = "apiKey"></param>
        /// <param name = "secretKey"></param>

        public UploadService(String apiKey, String secretKey)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
        }
        /// <summary>
        /// Uploads file on the cloud.
        /// </summary>
        /// <param name ="name">The name of the file which has to be saved. It is used to retrieve the file</param>
        /// <param name ="filePath">The local path for the file.</param>
        /// <param name ="fileType">The type of the file. File can be either Audio, Video,
        /// Image, Binary, Txt, xml, json, csv or other Use the static
        /// constants e.g. Upload.AUDIO, Upload.XML etc.
        /// </param>
        /// <param name ="description">Description of the file to be uploaded.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>

        public Upload UploadFile(String name, String filePath, String fileType,
                String description)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(filePath, "Filepath");
            Util.ThrowExceptionIfNullOrBlank(fileType, "FileType");
            Util.ThrowExceptionIfNullOrBlank(description, "Description");
            if (!UploadFileType.IsValidType(fileType))
            {
                throw new App42Exception("Not a valid file type !");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("type", fileType);
            postParams.Add("description", description);
            // Put these params for signing
            paramsDics.Add("name", name);
            paramsDics.Add("type", fileType);
            paramsDics.Add("description", description);

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource;
            response = Util.MultiPartRequest("uploadFile", filePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }  
       /// <summary>
       /// Uploads file on the cloud via Stream.
       /// </summary>
       /// <param name="name">The name of the file which has to be saved. It is used to retrieve the file</param>
       /// <param name="stream">InputStream of the file to be uploaded.</param>
       /// <param name="fileType">The type of the file. File can be either Audio, Video,
       /// Image, Binary, Txt, xml, json, csv or other Use the static
       /// constants e.g. Upload.AUDIO, Upload.XML etc.
       /// </param>
       /// <param name="description">Description of the file to be uploaded.</param>
       /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        
        public Upload UploadFile(String name, Stream stream,
			String fileType, String description)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(fileType, "FileType");
            Util.ThrowExceptionIfNullOrBlank(description, "Description");
            if (name.IndexOf('.') == -1)
            {
                Util.ThrowExceptionIfNotValidExtension(name, "File Name");
            }
            if (!UploadFileType.IsValidType(fileType))
            {
                throw new App42Exception("Not a valid file type!");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("type", fileType);
            postParams.Add("description", description);
            // Put these params for signing
            paramsDics.Add("name", name);
            paramsDics.Add("type", fileType);
            paramsDics.Add("description", description);

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource;
            response = Util.MultiPartRequest("uploadFile", stream,name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
		/// <summary>
		/// Uploads file on the cloud for given user.
		/// </summary>
		/// <param name="name">The name of the file which has to be saved. It is used to retrieve the file</param>
		/// <param name="userName">The name of the user for which file has to be saved.</param>
		/// <param name="filePath">The local path for the file.</param>
		/// <param name="fileType">The type of the file. File can be either Audio, Video, 
        /// Image, Binary, Txt, xml, json, csv or other Use the static
        /// constants e.g. Upload.AUDIO, Upload.XML etc.
        /// </param>
		/// <param name="description">Description of the file to be uploaded.</param>
		/// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload UploadFileForUser(String name, String userName, String filePath, String fileType,
                String description)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(filePath, "Filepath");
            Util.ThrowExceptionIfNullOrBlank(fileType, "FileType");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(description, "Description");
            if (!UploadFileType.IsValidType(fileType))
            {
                throw new App42Exception("Not a valid file type !");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("userName", userName);
            postParams.Add("type", fileType);
            postParams.Add("description", description);
            // Put these params for signing
            paramsDics.Add("name", name);
            paramsDics.Add("userName", userName);
            paramsDics.Add("type", fileType);
            paramsDics.Add("description", description);

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/" + userName;
            response = Util.MultiPartRequest("uploadFile", filePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Uploads file on the cloud for given user via Stream.
        /// </summary>
        /// <param name="name">The name of the file which has to be saved. It is used to retrieve the file</param>
        /// <param name="userName">The name of the user for which file has to be saved.</param>
        /// <param name="stream">InputStream of the file to be uploaded.</param>
        /// <param name="fileType">The type of the file. File can be either Audio, Video,
        /// Image, Binary, Txt, xml, json, csv or other Use the static
        /// constants e.g. Upload.AUDIO, Upload.XML etc.
        /// </param>
        /// <param name="description">Description of the file to be uploaded.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload UploadFileForUser(String name, String userName, Stream stream, String fileType,
                String description) {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(fileType, "FileType");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Util.ThrowExceptionIfNullOrBlank(description, "Description");
            if (name.IndexOf('.') == -1)
            {

                Util.ThrowExceptionIfNotValidExtension(name, "File Name");
            }
            if (!UploadFileType.IsValidType(fileType))
            {
                throw new App42Exception("Not a valid file type!");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();

            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
                    Util.GetUTCFormattedTimestamp());
            // Put these params for signing
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);


            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("userName", userName);
            postParams.Add("type", fileType);
            postParams.Add("description", description);
            // Put these params for signing
            paramsDics.Add("name", name);
            paramsDics.Add("userName", userName);
            paramsDics.Add("type", fileType);
            paramsDics.Add("description", description);

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/" + userName;
            response = Util.MultiPartRequest("uploadFile", stream, name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Gets all the files for the App.
        /// </summary>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetAllFiles()
        {
            String response = null;
            Upload upload = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Gets the file based on user and file name.
        /// </summary>
        /// <param  name="name">The name of the file which has to be retrieved.</param>
        /// <param  name="userName">The name of the user for which file has to be retrieved.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetFileByUser(String name, String userName)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", name);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + userName + "/" + name;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Get all the file based on user name.
        /// </summary>
        /// <param name="userName">The name of the user for which file has to be retrieved.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetAllFilesByUser(String userName)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(userName, " UserName");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + "user" + "/" + userName;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Gets the file based on file name.
        /// </summary>
        /// <param name="name">The name of the file which has to be retrieved.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetFileByName(String name)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", name);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + name;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Removes the file based on file name and user name.
        /// </summary>
        /// <param  name="name">The name of the file which has to be removed.</param>
        /// <param name= "userName">The name of the user for which file has to be removed.</param> 
        /// <returns>App42Response if deleted successfully.</returns>
        /// <exception>App42Exception</exception>

        public App42Response RemoveFileByUser(String name, String userName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", name);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + userName + "/" + name;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Removes the file based on file name.
        /// </summary>
        /// <param name ="name">The name of the file which has to be removed.</param>
        /// <returns>App42Response if deleted successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveFileByName(String name)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("name", name);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + name;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Removes the files based on user name.
        /// </summary>
        /// <param name="userName">The name of the user for which files has to be removed.</param>
        /// <returns>App42Response if deleted successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveAllFilesByUser(String userName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(userName, "User Name");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userName", userName);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/user" + "/" + userName;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Removes all the files for the App
        /// </summary>
        /// <returns>App42Response if deleted successfully.</returns>
        /// <exception>App42Exception</exception>
        public App42Response RemoveAllFiles()
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecuteDelete(signature, resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;
        }
        /// <summary>
        /// Get the files based on file type.
        /// </summary>
        /// <param name ="fileType">Type of the file e.g. Upload.AUDIO, Upload.XML etc.</param>
        /// <returns>Upload Object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetFilesByType(String fileType)
        {
            String response = null;
            Upload upload = null;
            Util.ThrowExceptionIfNullOrBlank(fileType, "FileType");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", fileType);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/type/" + fileType;
            response = RESTConnector.getInstance().ExecuteGet(signature, resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Gets all the files By Paging for the App
        /// </summary>
        /// <param name="max">Maximum number of records to be fetched.</param>
        /// <param name="offset">From where the records are to be fetched.</param>
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetAllFiles(int max, int offset)
        {
            String response = null;
            Upload upload = null;
            Util.ValidateMax(max);
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("max",""+ max);
            paramsDics.Add("offset",""+ offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + "paging"
                    + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Get all the files based on user name by Paging.
        /// </summary>
        /// <param name="userName">The name of the user for which file has to be retrieved</param>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>       
        /// <returns>Upload object</returns>
        ///<exception>App42Exception</exception>
        public Upload GetAllFilesByUser(String userName, int max, int offset)
        {
            String response = null;
            Upload upload = null;
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
            String resourceURL = this.version + "/" + this.resource + "/"
                    + "user" + "/" + userName + "/" + max + "/" + offset;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Get the files based on file type by Paging.
        /// </summary>
        /// <param name="uploadFileType">Type of the file e.g. Upload.AUDIO, Upload.XML etc.</param>
        /// <param name="max">Maximum number of records to be fetched</param>
        /// <param name="offset">From where the records are to be fetched</param>       
        /// <returns>Upload object</returns>
        /// <exception>App42Exception</exception>
        public Upload GetFilesByType(String uploadFileType, int max, int offset)
        {
            String response = null;
            Upload upload = null;
            Util.ValidateMax(max);
            Util.ThrowExceptionIfNullOrBlank(uploadFileType, "uploadFileType");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", uploadFileType + "");
            paramsDics.Add("max", "" + max);
            paramsDics.Add("offset", "" + offset);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/type/"
                    + uploadFileType + "/" + max + "/" + offset; ;

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            upload = new UploadResponseBuilder().BuildResponse(response);
            return upload;
        }
        /// <summary>
        /// Gets count of all the files for the App
        /// </summary>
        /// <returns>App42Response object</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAllFilesCount()
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new UploadResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Gets the count of file based on user name.
        /// </summary>
        /// <param name="userName">The name of the user for which count of the file has to be retrieved</param>
        /// <returns>App42Response object</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetAllFilesCountByUser(String userName)
        {
            String response = null;
            App42Response responseObj = new App42Response();
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
                    + "user" + "/" + userName + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new UploadResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
        /// <summary>
        /// Get the count of files based on file type.
        /// </summary>
        /// <param name="uploadFileType">Type of the file e.g. Upload.AUDIO, Upload.XML etc.</param>
        /// <returns>App42Response object</returns>
        /// <exception>App42Exception</exception>
        public App42Response GetFilesCountByType(String uploadFileType)
        {
            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(uploadFileType, "uploadFileType");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("type", uploadFileType);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/type/"
                    + uploadFileType + "/count";

            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            responseObj.SetTotalRecords(new UploadResponseBuilder().GetTotalRecords(response));
            return responseObj;
        }
    }
}
