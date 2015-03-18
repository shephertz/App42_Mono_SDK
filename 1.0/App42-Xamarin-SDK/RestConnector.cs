using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.connection
{
    class RESTConnector
    {
        private String baseURL = null;
        private String customCodeURL = null;

        private static RESTConnector restCon = null;

        private RESTConnector()
        {
            this.baseURL = Config.GetInstance().GetBaseURL();
            this.customCodeURL = Config.GetInstance().GetCustomCodeURL();
        }

        public static RESTConnector getInstance()
        {
            if (restCon == null)
                return new RESTConnector();
            return restCon;

        }


        public String ExecuteGet(String signature, String url, Dictionary<String, String> paramsDics)
        {

            // Dictionary<String, String> queryParams = new Dictionary<String, String>();
            //paramsDics.Add("signature", signature);
            String apiKey = paramsDics["apiKey"];
            String timeStamp = paramsDics["timeStamp"];
            paramsDics.Remove("apiKey");
            paramsDics.Remove("timeStamp");
            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection<String> keys = paramsDics.Keys;
            foreach (String key in keys)
            {
                String value = paramsDics[key];
                queryString.Append(System.Web.HttpUtility.UrlEncode(key) + "="
                    + System.Web.HttpUtility.UrlEncode(value) + "&");
            }
            String encodedUrl = System.Web.HttpUtility.UrlEncode(url);
            App42Log.Debug(" QueryString is " + queryString);
            String uri = this.baseURL + encodedUrl.Replace("+", "%20").Replace("%2F", "/") + queryString;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = "GET";
            // Set the ContentType property of the WebRequest.
            request.ContentType = Config.GetInstance().GetContentType();
            request.Accept = Config.GetInstance().GetAccept();
            request.Headers.Add("apiKey", apiKey);
            request.Headers.Add("timeStamp", timeStamp);
            request.Headers.Add("signature", signature);
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                response = request.GetResponse();
                // Display the status.
                App42Log.Debug(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                App42Log.Debug(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                var trace = new System.Diagnostics.StackTrace(e);
                App42Log.Debug("Exception Occured : " + trace);

                if (e.Response == null)
                {
                    throw e;
                }


                using (WebResponse responseError = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                    HttpStatusCode statusCode = httpResponse.StatusCode;

                    App42Log.Debug("Error code: {0}" + httpResponse.StatusCode);
                    using (Stream data = responseError.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        App42Log.Debug(text);
                        JObject jObj = JObject.Parse(text);
                        JObject errorCodes = (JObject)jObj["app42Fault"];
                        if (errorCodes != null)
                        {
                            App42Log.Debug(" appErrorCode " + errorCodes["appErrorCode"]);
                            App42Log.Debug(" httpErrorCode " + errorCodes["httpErrorCode"]);
                            String appErrorCode = errorCodes["appErrorCode"].ToString();
                            String httpErrorCode = errorCodes["httpErrorCode"].ToString();

                            if (statusCode == HttpStatusCode.NotFound)
                            {
                                throw new App42NotFoundException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.BadRequest)
                            {
                                throw new App42BadParameterException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new App42SecurityException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.RequestEntityTooLarge)
                            {
                                throw new App42LimitException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.InternalServerError)
                            {
                                throw new App42Exception(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }
                            else
                            {
                                throw new App42Exception(text);
                            }
                        }

                        else
                        {
                            throw new App42Exception(text);
                        }

                    }
                }

            }

            return responseFromServer;
        }


        public String ExecutePost(String signature, String url, Dictionary<String, String> paramsDics, String bodyPayLoad)
        {
            // Dictionary<String, String> queryParams = new Dictionary<String, String>();
            //paramsDics.Add("signature", signature);
            String apiKey = paramsDics["apiKey"];
            String timeStamp = paramsDics["timeStamp"];

            paramsDics.Remove("apiKey");
            paramsDics.Remove("timeStamp");
            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection<String> keys = paramsDics.Keys;
            foreach (String key in keys)
            {
                String value = paramsDics[key];
                queryString.Append(System.Web.HttpUtility.UrlEncode(key) + "="
                    + System.Web.HttpUtility.UrlEncode(value) + "&");
            }
            String encodedUrl = System.Web.HttpUtility.UrlEncode(url);
            App42Log.Debug(" QueryString is " + queryString);
            String uri = this.baseURL + encodedUrl.Replace("+", "%20").Replace("%2F", "/") + queryString;
            App42Log.Debug("POST URI : " + uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Set the ContentType property of the WebRequest.
            request.ContentType = Config.GetInstance().GetContentType();
            request.Accept = Config.GetInstance().GetAccept();
            request.Headers.Add("apiKey", apiKey);
            request.Headers.Add("timeStamp", timeStamp);
            request.Headers.Add("signature", signature);
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                Stream dataStreamRequest = request.GetRequestStream();
                // Write the data to the request stream.
                byte[] byteArray = Encoding.UTF8.GetBytes(bodyPayLoad);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStreamRequest.Close();
                response = request.GetResponse();
                // Display the status.
                App42Log.Debug(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                App42Log.Debug(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw e;
                }

                using (WebResponse responseError = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                    HttpStatusCode statusCode = httpResponse.StatusCode;

                    App42Log.Debug("Error code: {0}" + httpResponse.StatusCode);
                    using (Stream data = responseError.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        App42Log.Debug(text);
                        JObject jObj = JObject.Parse(text);
                        JObject errorCodes = (JObject)jObj["app42Fault"];
                        if (errorCodes != null)
                        {
                            App42Log.Debug(" appErrorCode " + errorCodes["appErrorCode"]);
                            App42Log.Debug(" httpErrorCode " + errorCodes["httpErrorCode"]);
                            String appErrorCode = errorCodes["appErrorCode"].ToString();
                            String httpErrorCode = errorCodes["httpErrorCode"].ToString();

                            if (statusCode == HttpStatusCode.NotFound)
                            {
                                throw new App42NotFoundException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.BadRequest)
                            {
                                throw new App42BadParameterException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new App42SecurityException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.RequestEntityTooLarge)
                            {
                                throw new App42LimitException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.InternalServerError)
                            {
                                throw new App42Exception(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }
                            else
                            {
                                throw new App42Exception(text);
                            }
                        }

                        else
                        {
                            throw new App42Exception(text);
                        }

                    }
                }

            }

            return responseFromServer;
        }



        public String ExecutePut(String signature, String url, Dictionary<String, String> paramsDics, String bodyPayLoad)
        {
            // Dictionary<String, String> queryParams = new Dictionary<String, String>();
            //paramsDics.Add("signature", signature);
            String apiKey = paramsDics["apiKey"];
            String timeStamp = paramsDics["timeStamp"];

            paramsDics.Remove("apiKey");
            paramsDics.Remove("timeStamp");
            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection<String> keys = paramsDics.Keys;
            foreach (String key in keys)
            {
                String value = paramsDics[key];
                queryString.Append(System.Web.HttpUtility.UrlEncode(key) + "="
                    + System.Web.HttpUtility.UrlEncode(value) + "&");
            }
            String encodedUrl = System.Web.HttpUtility.UrlEncode(url);
            App42Log.Debug(" QueryString is " + queryString);
            String uri = this.baseURL + encodedUrl.Replace("+", "%20").Replace("%2F", "/") + queryString;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = "PUT";
            // Set the ContentType property of the WebRequest.
            request.ContentType = Config.GetInstance().GetContentType();
            request.Accept = Config.GetInstance().GetAccept();
            request.Headers.Add("apiKey", apiKey);
            request.Headers.Add("timeStamp", timeStamp);
            request.Headers.Add("signature", signature);
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                Stream dataStreamRequest = request.GetRequestStream();
                // Write the data to the request stream.
                byte[] byteArray = Encoding.UTF8.GetBytes(bodyPayLoad);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStreamRequest.Close();
                response = request.GetResponse();
                // Display the status.
                App42Log.Debug(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                App42Log.Debug(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw e;
                }

                using (WebResponse responseError = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                    HttpStatusCode statusCode = httpResponse.StatusCode;

                    App42Log.Debug("Error code: {0}" + httpResponse.StatusCode);
                    using (Stream data = responseError.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        App42Log.Debug(text);
                        JObject jObj = JObject.Parse(text);
                        JObject errorCodes = (JObject)jObj["app42Fault"];
                        if (errorCodes != null)
                        {
                            App42Log.Debug(" appErrorCode " + errorCodes["appErrorCode"]);
                            App42Log.Debug(" httpErrorCode " + errorCodes["httpErrorCode"]);
                            String appErrorCode = errorCodes["appErrorCode"].ToString();
                            String httpErrorCode = errorCodes["httpErrorCode"].ToString();

                            if (statusCode == HttpStatusCode.NotFound)
                            {
                                throw new App42NotFoundException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.BadRequest)
                            {
                                throw new App42BadParameterException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new App42SecurityException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.RequestEntityTooLarge)
                            {
                                throw new App42LimitException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.InternalServerError)
                            {
                                throw new App42Exception(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }
                            else
                            {
                                throw new App42Exception(text);
                            }
                        }

                        else
                        {
                            throw new App42Exception(text);
                        }

                    }
                }

            }

            return responseFromServer;
        }


        public String ExecuteDelete(String signature, String url, Dictionary<String, String> paramsDics)
        {
            // Dictionary<String, String> queryParams = new Dictionary<String, String>();
            //paramsDics.Add("signature", signature);
            String apiKey = paramsDics["apiKey"];
            String timeStamp = paramsDics["timeStamp"];

            paramsDics.Remove("apiKey");
            paramsDics.Remove("timeStamp");
            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection<String> keys = paramsDics.Keys;
            foreach (String key in keys)
            {
                String value = paramsDics[key];
                queryString.Append(System.Web.HttpUtility.UrlEncode(key) + "="
                    + System.Web.HttpUtility.UrlEncode(value) + "&");
            }
            String encodedUrl = System.Web.HttpUtility.UrlEncode(url);
            App42Log.Debug(" QueryString is " + queryString);
            String uri = this.baseURL + encodedUrl.Replace("+", "%20").Replace("%2F", "/") + queryString;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = "DELETE";
            // Set the ContentType property of the WebRequest.
            request.ContentType = Config.GetInstance().GetContentType();
            request.Accept = Config.GetInstance().GetAccept();
            request.Headers.Add("apiKey", apiKey);
            request.Headers.Add("timeStamp", timeStamp);
            request.Headers.Add("signature", signature);
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                response = request.GetResponse();
                App42Log.Debug(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                App42Log.Debug(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw e;
                }

                using (WebResponse responseError = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                    HttpStatusCode statusCode = httpResponse.StatusCode;

                    App42Log.Debug("Error code: {0}" + httpResponse.StatusCode);
                    using (Stream data = responseError.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        App42Log.Debug(text);
                        JObject jObj = JObject.Parse(text);
                        JObject errorCodes = (JObject)jObj["app42Fault"];
                        if (errorCodes != null)
                        {
                            App42Log.Debug(" appErrorCode " + errorCodes["appErrorCode"]);
                            App42Log.Debug(" httpErrorCode " + errorCodes["httpErrorCode"]);
                            String appErrorCode = errorCodes["appErrorCode"].ToString();
                            String httpErrorCode = errorCodes["httpErrorCode"].ToString();

                            if (statusCode == HttpStatusCode.NotFound)
                            {
                                throw new App42NotFoundException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.BadRequest)
                            {
                                throw new App42BadParameterException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new App42SecurityException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.RequestEntityTooLarge)
                            {
                                throw new App42LimitException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.InternalServerError)
                            {
                                throw new App42Exception(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }
                            else
                            {
                                throw new App42Exception(text);
                            }
                        }

                        else
                        {
                            throw new App42Exception(text);
                        }

                    }
                }

            }

            return responseFromServer;
        }

        public string ExecuteCustomCode(String signature, String url, Dictionary<String, String> paramsDics, String bodyPayLoad)
        {
            // Dictionary<String, String> queryParams = new Dictionary<String, String>();
            //paramsDics.Add("signature", signature);
            String apiKey = paramsDics["apiKey"];
            String timeStamp = paramsDics["timeStamp"];

            paramsDics.Remove("apiKey");
            paramsDics.Remove("timeStamp");

            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection<String> keys = paramsDics.Keys;
            foreach (String key in keys)
            {
                String value = paramsDics[key];
                queryString.Append(System.Web.HttpUtility.UrlEncode(key) + "="
                    + System.Web.HttpUtility.UrlEncode(value) + "&");
            }

            String encodedUrl = System.Web.HttpUtility.UrlEncode(url);
            App42Log.Debug(" QueryString is " + queryString);
            String uri = this.customCodeURL + encodedUrl.Replace("+", "%20").Replace("%2F", "/") + queryString;
            App42Log.Debug("POST URI : " + uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Set the ContentType property of the WebRequest.
            request.ContentType = Config.GetInstance().GetContentType();
            request.Accept = Config.GetInstance().GetAccept();
            request.Headers.Add("apiKey", apiKey);
            request.Headers.Add("timeStamp", timeStamp);
            request.Headers.Add("signature", signature);
            // Set the ContentLength property of the WebRequest.
            // Get the response.

            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                Stream dataStreamRequest = request.GetRequestStream();
                // Write the data to the request stream.
                byte[] byteArray = Encoding.UTF8.GetBytes(bodyPayLoad);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStreamRequest.Close();
                response = request.GetResponse();
                // Display the status.
                App42Log.Debug(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                App42Log.Debug(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw e;
                }

                using (WebResponse responseError = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)responseError;
                    HttpStatusCode statusCode = httpResponse.StatusCode;

                    App42Log.Debug("Error code: {0}" + httpResponse.StatusCode);
                    using (Stream data = responseError.GetResponseStream())
                    {
                        string text = new StreamReader(data).ReadToEnd();
                        App42Log.Debug(text);
                        JObject jObj = JObject.Parse(text);
                        JObject errorCodes = (JObject)jObj["app42Fault"];
                        if (errorCodes != null)
                        {
                            App42Log.Debug(" appErrorCode " + errorCodes["appErrorCode"]);
                            App42Log.Debug(" httpErrorCode " + errorCodes["httpErrorCode"]);
                            String appErrorCode = errorCodes["appErrorCode"].ToString();
                            String httpErrorCode = errorCodes["httpErrorCode"].ToString();

                            if (statusCode == HttpStatusCode.NotFound)
                            {
                                throw new App42NotFoundException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.BadRequest)
                            {
                                throw new App42BadParameterException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.Unauthorized)
                            {
                                throw new App42SecurityException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.RequestEntityTooLarge)
                            {
                                throw new App42LimitException(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }

                            else if (statusCode == HttpStatusCode.InternalServerError)
                            {
                                throw new App42Exception(text, Convert.ToInt32(httpErrorCode), Convert.ToInt32(appErrorCode));
                            }
                            else
                            {
                                throw new App42Exception(text);
                            }
                        }
                        else
                        {
                            throw new App42Exception(text);
                        }
                    }
                }
            }
            return responseFromServer;
        }
    }
}
