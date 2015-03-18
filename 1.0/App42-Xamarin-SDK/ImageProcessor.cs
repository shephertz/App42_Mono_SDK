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


    /**
 * The ImageProcessor service is a Image utility service on the Cloud. Developers can 
 * upload files on the cloud and perform various Image Manipulation operations on the Uploaded
 * Images e.g. resize, scale, thumbnail, crop etc. It is especially useful for Mobile Apps when 
 * they dont want to store Images locally and dont want to perform processor intensive operations.
 * It is also useful for web applications who want to perform complex Image Operations
 *
 *
 */
    public class ImageProcessor
    {
        private String version = "1.0";
        private String resource = "image";
        private String apiKey;
        private String secretKey;
        String baseURL;

        public ImageProcessor(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;

        }

        /**
         * Resize image. Returns the original image url and converted image url.
         * Images are stored on the cloud and can be accessed through the urls
         * Resizing is done based on the width and height provided
         * @params name Name of the image to resize
         * @params imagePath Path of the local file to resize
         * @params width Width of the image to resize
         * @params height Height of the image to resize
         * 
         * @returns "The urls for the original and converted images
         */

        public String Resize(String name, String imagePath, Int64 width,
			Int64 height)  {
		String response = null;
		

		Util.ThrowExceptionIfNullOrBlank(name, "Name");
		Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
		Util.ThrowExceptionIfNullOrBlank(width, "Width");
		Util.ThrowExceptionIfNullOrBlank(height, "Height");

        //FileStream file = File.Create(imagePath);
        // String ext = Path.GetExtension(file.Name);
        String ext = Path.GetExtension(imagePath);
        //if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
        //{
        //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
        //}

        //if (!File.Exists(imagePath))
        //{
        //    throw new App42Exception(" File " + imagePath
        //            + " does not exist");
        //}

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
			response = Util.MultiPartRequest("imageFile",imagePath, queryParams,
                    paramsDics, resourceUrl, Config.GetInstance().GetAccept());
		
		return response;

	}

    /**
     * Creates a thumbnail of the image. There is a difference between thumbnail and resize
     * The thumbnail operation is optimized for speed, it removes information of the image which is not necessary for a
     * thumbnail e.g hearder information. Returns the original image url and converted image url.
     * Images are stored on the cloud and can be accessed through the urls
     * Resizing is done based on the width and height provided
     * @params name Name of the image file for which thumbnail has to be created
     * @params imagePath Path of the local file whose thumbnail has to be created
     * @params width Width of the image for thumbnail
     * @params height Height of the image for thumbnail
     * 
     * @returns "The urls for the original and converted images
     */


        public String Thumbnail(String name, String imagePath, Int64 width,
        Int64 height)
        {
            String response = null;


            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            Util.ThrowExceptionIfNullOrBlank(width, "Width");
            Util.ThrowExceptionIfNullOrBlank(height, "Height");

            //FileStream file = File.Create(imagePath);
            // String ext = Path.GetExtension(file.Name);
            String ext = Path.GetExtension(imagePath);
            //if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            //{
            //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            //}

            //if (!File.Exists(imagePath))
            //{
            //    throw new App42Exception(" File " + imagePath
            //            + " does not exist");
            //}

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

            return response;

        }

        /**
    * Scales the image based on width and height. Returns the original image url and converted image url.
    * Images are stored in the cloud and can be accessed through the urls
    * Resizing is done based on the width and height provided
    * @params name Name of the image to scale
    * @params imagePath Path of the local file to scale
    * @params width Width of the image to scale
    * @params height Height of the image to scale
    * 
    * @returns "The urls for the original and converted images
    */
         public String Scale(String name, String imagePath, Int64 width,
        Int64 height)
        {
            String response = null;


            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            Util.ThrowExceptionIfNullOrBlank(width, "Width");
            Util.ThrowExceptionIfNullOrBlank(height, "Height");


            //FileStream file = File.Create(imagePath);
           // String ext = Path.GetExtension(file.Name);
            String ext = Path.GetExtension(imagePath);
            //if (!(ext.Equals(".jpg")) && !(ext.Equals(".jpeg")) && !(ext.Equals(".gif")) && !(ext.Equals(".png")))
            //{
            //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            //}

            //if (!File.Exists(imagePath))
            //{
            //    throw new App42Exception(" File " + imagePath
            //            + " does not exist");
            //}

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

            return response;


        }


        /**
         * Crops image based on width, height and x, y coordinates. Returns the original image url and converted image url.
         * Images are stored in the cloud and can be accessed through the urls
         * Resizing is done based on the width and height provided
         * @params name Name of the image to crop
         * @params imagePath Path of the local file to crop
         * @params width Width of the image to crop
         * @params height Height of the image to crop
         * @params x Coordinate X
         * @params y Coordinate Y
         *
         * @returns "The urls for the original and converted images
         */
         public String Crop(String name, String imagePath, Int64 width,
            Int64 height, Int64 x, Int64 y)
         {
             String response = null;


             Util.ThrowExceptionIfNullOrBlank(name, "Name");
             Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
             Util.ThrowExceptionIfNullOrBlank(width, "Width");
             Util.ThrowExceptionIfNullOrBlank(height, "Height");
             Util.ThrowExceptionIfNullOrBlank(x, "X");
             Util.ThrowExceptionIfNullOrBlank(y, "Y");


            // FileStream file = File.Create(imagePath, 1024);
             String ext = Path.GetExtension(imagePath);
             //if (!(ext.Equals("jpg")) && !(ext.Equals("jpeg")) && !(ext.Equals("gif")) && !(ext.Equals("png")))
             //{
             //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
             //}

             //if (!File.Exists(imagePath))
             //{
             //    throw new App42Exception(" File " + imagePath
             //            + " does not exist");
             //}



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

             return response;
         }
        
   /**
    * Resize image by Percentage. Returns the original image url and converted image url.
    * Images are stored in the cloud and can be accessed through the urls
    * Resizing is done based on the width and height provided
    * @params name Name of the image to resize
    * @params imagePath Path of the local file to resize
    * @params percentage Percentage to which image has to be resized
    * @returns "The urls for the original and converted images
    */


        public String ResizeByPercentage(String name, String imagePath,
			Int64 percentage) {
		String response = null;
		
		Util.ThrowExceptionIfNullOrBlank(name, "Name");
		Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
		Util.ThrowExceptionIfNullOrBlank(percentage, "Percentage");

        // FileStream file = File.Create(imagePath, 1024);
        String ext = Path.GetExtension(imagePath);
        //if (!(ext.Equals("jpg")) && !(ext.Equals("jpeg")) && !(ext.Equals("gif")) && !(ext.Equals("png")))
        //{
        //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
        //}

        //if (!File.Exists(imagePath))
        //{
        //    throw new App42Exception(" File " + imagePath
        //            + " does not exist");
        //}


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
            return response;
		
	}


        /**
    * Creates a thumbnail of the image by Percentage. There is a difference between thumbnail and resize
    * The thumbnail operation is optimized for speed removes information of the image which is not necessary for a 
    * thumbnail to reduce size e.g hearder information. Returns the original image url and converted image url.
    * Images are stored in the cloud and can be accessed through the urls
    * Resizing is done based on the width and height provided
    * @params name Name of the image file for which thumbnail has to be created
    * @params imagePath Path of the local file whose thumbnail has to be created
    * @params percentage Percentage for thumbnail
    * @returns "The urls for the original and converted images
    */

        public String ThumbnailByPercentage(String name, String imagePath,
          Int64 percentage)
        {
            String response = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            Util.ThrowExceptionIfNullOrBlank(percentage, "Percentage");
            // FileStream file = File.Create(imagePath, 1024);
            String ext = Path.GetExtension(imagePath);
            //if (!(ext.Equals("jpg")) && !(ext.Equals("jpeg")) && !(ext.Equals("gif")) && !(ext.Equals("png")))
            //{
            //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            //}

            //if (!File.Exists(imagePath))
            //{
            //    throw new App42Exception(" File " + imagePath
            //            + " does not exist");
            //}



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

            return response;           
        }
                
        /**
     * Scales the image  by Percentage. Returns the original image url and converted image url.
     * Images are stored in the cloud and can be accessed through the urls
     * Resizing is done based on the width and height provided
     * @params name Name of the image file to scale
     * @params imagePath Path of the local file to scale
     * @params percentage Percentage to which image has to be scaled
     * 
     * @returns "The urls for the original and converted images
     */

        public String ScaleByPercentage(String name, String imagePath,
          Int64 percentage)
        {
            String response = null;

            Util.ThrowExceptionIfNullOrBlank(name, "Name");
            Util.ThrowExceptionIfNullOrBlank(imagePath, "Image Path");
            Util.ThrowExceptionIfNullOrBlank(percentage, "Percentage");
            // FileStream file = File.Create(imagePath, 1024);
            String ext = Path.GetExtension(imagePath);
            //if (!(ext.Equals("jpg")) && !(ext.Equals("jpeg")) && !(ext.Equals("gif")) && !(ext.Equals("png")))
            //{
            //    throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            //}

            //if (!File.Exists(imagePath))
            //{
            //    throw new App42Exception(" File " + imagePath
            //            + " does not exist");
            //}



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

            return response;


        }

    }
}
