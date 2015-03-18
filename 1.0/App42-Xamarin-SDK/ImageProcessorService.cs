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


namespace com.shephertz.app42.paas.sdk.csharp.imageProcessor
{

    /// <summary>
    /// The ImageProcessor service is a Image utility service on the Cloud. Developers can 
    /// upload files on the cloud and perform various Image Manipulation operations on the Uploaded
    /// Images e.g. resize, scale, thumbnail, crop etc. It is especially useful for Mobile Apps when 
    /// they dont want to store Images locally and dont want to perform processor intensive operations.
    /// It is also useful for web applications who want to perform complex Image Operations
    /// </summary>
    /// <see cref="ImageProcessorService">Image</see>

    public class ImageProcessorService
    {
        private String version = "1.0";
        private String resource = "image";
        private String apiKey;
        private String secretKey;
        String baseURL;

        public ImageProcessorService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }

        /// <summary>
        /// Resize image. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name">Name of the image to resize</param>
        /// <param name="imagePath"> Path of the local file to resize</param>
        /// <param name="width"> Width of the image to resize</param>
        /// <param name="height"> Height of the image to resize</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Resize(String name, String imagePath, int width,
            int height)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");

            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/resize";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    paramsDics, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Creates a thumbnail of the image. There is a difference between thumbnail and resize
        /// The thumbnail operation is optimized for speed, it removes information of the image which is not necessary for a
        /// thumbnail e.g hearder information. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file for which thumbnail has to be created</param>
        /// <param name="imagePath"> Path of the local file whose thumbnail has to be created</param>
        /// <param name="width"> Width of the image for thumbnail</param>
        /// <param name="height"> Height of the image for thumbnail</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Thumbnail(String name, String imagePath, int width,
        int height)
        {
            String response = null;
            Image imageObj = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/thumbnail";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    paramsDics, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Scales the image based on width and height. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image to scale</param>
        /// <param name="imagePath">Path of the local file to scale</param>
        /// <param name="width">Width of the image to scale</param>
        /// <param name="height">Height of the image to scale</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Scale(String name, String imagePath, int width,
        int height)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/scale";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Crops image based on width, height and x, y coordinates. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image to crop</param>
        /// <param name="imagePath">Path of the local file to crop</param>
        /// <param name="width"> Width of the image to crop</param>
        /// <param name="height"> Height of the image to crop</param>
        /// <param name="x">Coordinate X</param>
        /// <param name="y"> Coordinate y</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Crop(String name, String imagePath, int width,
            int height, int x, int y)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }



            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");
            postParams.Add("x", x + "");
            postParams.Add("y", y + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");
            paramsDics.Add("x", x + "");
            paramsDics.Add("y", y + "");


            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/crop";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Resize image by Percentage. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name">Name of the image to resize</param>
        /// <param name="imagePath">Path of the local file to resize</param>
        /// <param name="percentage">Percentage to which image has to be resized</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ResizeByPercentage(String name, String imagePath,
            Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }


            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/resizePercentage";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Creates a thumbnail of the image by Percentage. There is a difference between thumbnail and resize
        /// The thumbnail operation is optimized for speed removes information of the image which is not necessary for a 
        /// thumbnail to reduce size e.g hearder information. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file for which thumbnail has to be created</param>
        /// <param name="imagePath"> Path of the local file whose thumbnail has to be created</param>
        /// <param name="percentage">  Percentage for thumbnail</param>
        /// <returns>Image object containing  urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ThumbnailByPercentage(String name, String imagePath,
          Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }



            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");


            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/thumbnailPercentage";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Scales the image  by Percentage. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file to scale</param>
        /// <param name="imagePath"> Path of the local file to scale</param>
        /// <param name="percentage"> Percentage to which image has to be scaled</param>
        /// <returns>Image object containing  urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ScaleByPercentage(String name, String imagePath,
          Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }



            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/scalePercentage";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }
        /// <summary>
        /// Resize image. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name">Name of the image to resize</param>
        /// <param name="imagePath"> Path of the local file to resize</param>
        /// <param name="width"> Width of the image to resize</param>
        /// <param name="height"> Height of the image to resize</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Resize(String name, Stream stream, int width,
            int height)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/resize";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Creates a thumbnail of the image. There is a difference between thumbnail and resize
        /// The thumbnail operation is optimized for speed, it removes information of the image which is not necessary for a
        /// thumbnail e.g hearder information. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file for which thumbnail has to be created</param>
        /// <param name="imagePath"> Path of the local file whose thumbnail has to be created</param>
        /// <param name="width"> Width of the image for thumbnail</param>
        /// <param name="height"> Height of the image for thumbnail</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Thumbnail(String name, Stream stream, int width,
        int height)
        {
            String response = null;
            Image imageObj = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/thumbnail";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                    paramsDics, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Scales the image based on width and height. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image to scale</param>
        /// <param name="imagePath">Path of the local file to scale</param>
        /// <param name="width">Width of the image to scale</param>
        /// <param name="height">Height of the image to scale</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Scale(String name, Stream stream, int width,
        int height)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/scale";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;

        }

        /// <summary>
        /// Crops image based on width, height and x, y coordinates. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image to crop</param>
        /// <param name="imagePath">Path of the local file to crop</param>
        /// <param name="width"> Width of the image to crop</param>
        /// <param name="height"> Height of the image to crop</param>
        /// <param name="x">Coordinate X</param>
        /// <param name="y"> Coordinate y</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image Crop(String name, Stream stream, int width,
            int height, int x, int y)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("width", width + "");
            postParams.Add("height", height + "");
            postParams.Add("x", x + "");
            postParams.Add("y", y + "");

            paramsDics.Add("name", name);
            paramsDics.Add("width", width + "");
            paramsDics.Add("height", height + "");
            paramsDics.Add("x", x + "");
            paramsDics.Add("y", y + "");


            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/crop";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                     postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Resize image by Percentage. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name">Name of the image to resize</param>
        /// <param name="imagePath">Path of the local file to resize</param>
        /// <param name="percentage">Percentage to which image has to be resized</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ResizeByPercentage(String name, Stream stream, Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");

            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/resizePercentage";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                     postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Creates a thumbnail of the image by Percentage. There is a difference between thumbnail and resize
        /// The thumbnail operation is optimized for speed removes information of the image which is not necessary for a 
        /// thumbnail to reduce size e.g hearder information. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file for which thumbnail has to be created</param>
        /// <param name="imagePath"> Path of the local file whose thumbnail has to be created</param>
        /// <param name="percentage">  Percentage for thumbnail</param>
        /// <returns>Image object containing  urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ThumbnailByPercentage(String name, Stream stream,
          Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");


            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/thumbnailPercentage";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }

        /// <summary>
        /// Scales the image  by Percentage. Returns the original image url and converted image url.
        /// Images are stored in the cloud and can be accessed through the urls
        /// Resizing is done based on the width and height provided
        /// </summary>
        /// <param name="name"> Name of the image file to scale</param>
        /// <param name="imagePath"> Path of the local file to scale</param>
        /// <param name="percentage"> Percentage to which image has to be scaled</param>
        /// <returns>Image object containing  urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ScaleByPercentage(String name, Stream stream, Double percentage)
        {
            String response = null;
            Image imageObj = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("percentage", percentage + "");
            paramsDics.Add("name", name);
            paramsDics.Add("percentage", percentage + "");
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/scalePercentage";
            response = Util.MultiPartRequest("imageFile", stream, name, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());

            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }
        /// <summary>
        /// Converts the format of the image. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls
        /// Conversion is done based on the formatToConvert provided
        /// </summary>
        /// <param name="name">Name of the image to convert</param>
        /// <param name="imagePath">Path of the local file to convert</param>
        /// <param name="formatToConvert">To which file needs to be converted</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>

        public Image ConvertFormat(String name, String imagePath, String formatToConvert)
        {
            String response = null;
            Image imageObj = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            Util.ThrowExceptionIfNullOrBlank(formatToConvert, "FormatToConvert");

            String ext = Path.GetExtension(imagePath);
            if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }
            if (!(formatToConvert.Equals(".jpg")) && !(formatToConvert.Equals(".jpeg")) && !(formatToConvert.Equals(".gif")) && !(formatToConvert.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("=====   The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }
            if (!File.Exists(imagePath))
            {
                throw new App42Exception(" File " + imagePath
                        + " does not exist");
            }

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("formatToConvert", formatToConvert);
            paramsDics.Add("name", name);
            paramsDics.Add("formatToConvert", formatToConvert);
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/convertformat";
            response = Util.MultiPartRequest("imageFile", imagePath, queryParams,
                    postParams, resourceUrl, Config.GetInstance().GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }
        /// <summary>
        /// Converts the format of the image. Returns the original image url and converted image url.
        /// Images are stored on the cloud and can be accessed through the urls Conversion is done 
        /// based on the formatToConvert provided
        /// </summary>
        /// <param name="name">Name of the image to convert</param>
        /// <param name="inputStream">InputStream of the local file to convert</param>
        /// <param name="formatToConvert">To which file needs to be converted</param>
        /// <returns>Image object containing urls for the original and converted images</returns>
        /// <exception>App42Exception</exception>
        public Image ConvertFormat(String name, Stream stream, String formatToConvert)
        {
            String response = null;
            Image imageObj = null;
            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNotValidImageExtension(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(formatToConvert, "FormatToConvert");
            if (!(formatToConvert.Equals(".jpg")) && !(formatToConvert.Equals(".jpeg")) && !(formatToConvert.Equals(".gif")) && !(formatToConvert.Equals(".png")))
            {
                throw new ArgumentOutOfRangeException("=====   The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp",
            Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            Dictionary<String, String> postParams = new Dictionary<String, String>();
            postParams.Add("name", name);
            postParams.Add("formatToConvert", formatToConvert);
            paramsDics.Add("name", name);
            paramsDics.Add("formatToConvert", formatToConvert);
            String signature = Util.Sign(this.secretKey, paramsDics);
            queryParams.Add("signature", signature);
            String resourceUrl = Config.GetInstance().GetBaseURL()
                    + this.version + "/" + this.resource + "/convertformat";
            response = Util.MultiPartRequest("imageFile", stream, name,
                    queryParams, postParams, resourceUrl, Config.GetInstance()
                            .GetAccept());
            imageObj = new ImageProcessorResponseBuilder().BuildResponse(response);
            return imageObj;
        }
    }
}