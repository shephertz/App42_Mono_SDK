using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.util
{
    class Util
    {
        /// <summary>
        /// This method sorts all the values that are stored in the table.
        /// </summary>
        /// <param name="dics"></param>
        /// <returns>Sorted string</returns>
        private static String SortAndConvertTableToString(Dictionary<String, String> dics)
        {
            ICollection<String> keysList = dics.Keys;
            List<String> sortedValues = new List<String>(keysList);
            sortedValues.Sort();
            App42Log.Debug(sortedValues.ToArray()[0]);
            StringBuilder requestString = new StringBuilder();
            foreach (string key in sortedValues)
            {
                requestString.Append(key);
                requestString.Append(dics[key]);

            }
            App42Log.Debug(requestString.ToString());
            return requestString.ToString();
        }

        /// <summary>
        /// Encoding and decoding of the queries are done using the Hmac Algorithm.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <exception>InvalidKeyException</exception>
        /// <exception>NoSuchAlgorithmException</exception>
        /// <exception>IllegalStateException</exception>
        /// <exception>UnsupportedEncodingException</exception>
        public static string ComputeHmac(string input, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            //String hmacCode = myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            return Convert.ToBase64String(myhmacsha1.ComputeHash(stream));
        }
        /// <summary>
        /// It signs the request that has to be sent in the Hmac format.
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="paramsList"></param>
        /// <exception>InvalidKeyException</exception>
        /// <exception>NoSuchAlgorithmException</exception>
        /// <exception>IllegalStateException</exception>
        /// <exception>UnsupportedEncodingException</exception>
        public static String Sign(String secretKey, Dictionary<String, String> paramsList)
        {
            String sortedParams = SortAndConvertTableToString(paramsList);
            String signature = ComputeHmac(sortedParams, Encoding.UTF8.GetBytes(secretKey));
            App42Log.Debug(signature);

            String value = HttpUtility.UrlEncode(signature);
            return Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());
            // return UpperCaseUrlEncode(System.Web.HttpUtility.UrlEncode(signature)); 
        }

        /// <summary>
        /// Finds the current time
        /// </summary>
        /// <returns>Time format</returns>
        public static String GetUTCFormattedTimestamp()
        {
            DateTime datTimeUTC = DateTime.Now.ToUniversalTime();
            DateTime dt = new DateTime(datTimeUTC.Year, datTimeUTC.Month, datTimeUTC.Day, datTimeUTC.Hour, datTimeUTC.Minute, datTimeUTC.Second, datTimeUTC.Millisecond);
            return String.Format("{0:yyyy-MM-dd'T'HH:mm:ss.fff'Z'}", dt);
        }
        /// <summary>
        /// Finds the particular time for a particular date
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>Time format</returns>
        public static String GetUTCFormattedTimestamp(DateTime dt)
        {
            DateTime datTimeUTC = dt;
            DateTime dtFormat = new DateTime(datTimeUTC.Year, datTimeUTC.Month, datTimeUTC.Day, datTimeUTC.Hour, datTimeUTC.Minute, datTimeUTC.Second, datTimeUTC.Millisecond);
            return String.Format("{0:yyyy-MM-dd'T'HH:mm:ss.fff'Z'}", dtFormat);
        }
        /// <summary>
        /// An exception to check whether the object is null or blank.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        internal static void ThrowExceptionIfNullOrBlank(Object param, string paramName)
        {
            if (param == null)
            {
                throw new App42Exception(paramName + " can not be null");
            }
            if (param is String)
            {
                if (((String)param).Trim().Equals(""))
                {

                    throw new App42Exception(paramName + " can not be blank");
                }
            }

        }


        public static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp);
        }
        /// <summary>
        /// Taking extension out
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>extension</returns>
        public static String extractFileExtension(String fileName)
        {
            String ext = fileName.Substring(fileName.LastIndexOf(".") + 1);
            return ext;
        }
        /// <summary>
        /// To check whether the max value is greater than zero or not.
        /// </summary>
        /// <param name="max"></param>
        public static void ValidateMax(int max)
        {
            if (max < 1)
            {
                throw new App42Exception("Max must be greater than Zero.");
            }
        }
        /// <summary>
        /// To check whether the value of how many is less than 1000 or not.
        /// </summary>
        /// <param name="howMany"></param>
        public static void ValidateHowMany(int howMany)
        {
            if (howMany > 1000)
            {
                throw new App42Exception("How Many Should be less than 1000");
            }
        }
       /// <summary>
       /// Multipart request for all the methods
       /// </summary>
       /// <param name="fileReferenceName"></param>
       /// <param name="filePath"></param>
       /// <param name="queryParams"></param>
       /// <param name="postParams"></param>
       /// <param name="urlPost"></param>
       /// <param name="acceptType"></param>
       /// <returns>string</returns>
        /// <exception>Exception</exception>
        public static string MultiPartRequest(string fileReferenceName, String filePath, Dictionary<string, string> queryParams, Dictionary<string, string> postParams, string urlPost, string acceptType)
        {
            String lineEnd = "\r\n";
            String twoHyphens = "--";
            String boundary = "**********";
            String fileName = System.IO.Path.GetFileName(filePath);

            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection keys = queryParams.Keys;
            foreach (String key in keys)
            {
                String value = queryParams[key];
                queryString.Append(key + "=" + value + "&");
                App42Log.Debug(" Setting value :" + key + " : " + value);
            }

            String urlString = urlPost + queryString;
            // Make a http Request

            App42Log.Debug("Upload URI : " + urlString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Set the ContentType property of the WebRequest.
            request.ContentType = "multipart/form-data;boundary=" + boundary;
            request.Accept = Config.GetInstance().GetAccept();
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                Stream dataStreamRequest = request.GetRequestStream();

                //Start with Writing boundary and Form field
                byte[] byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + fileReferenceName + "\"" + ";"
                        + " filename=\"" + fileName + "\"" + lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes(lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                // Write the data to the request stream.
                byteArray = File.ReadAllBytes(filePath);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes(lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;


                if (postParams != null && postParams.Count > 0)
                {
                    byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;
                }
                else
                {
                    byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + twoHyphens + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;
                }

                ICollection keysParam = postParams.Keys;
                int i = 0;
                foreach (String key in keysParam)
                {
                    i++;
                    String value = postParams[key];
                    byteArray = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key
                            + "\"" + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;


                    byteArray = Encoding.UTF8.GetBytes(lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;

                    byteArray = Encoding.UTF8.GetBytes(value + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;

                    if (i == keysParam.Count)
                    {
                        byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + twoHyphens + lineEnd);
                        dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                        byteArray = null;
                    }
                    else
                    {
                        byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                        dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                        byteArray = null;
                    }
                    App42Log.Debug(" Setting Param  :" + key + " : " + value);
                }

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

        /// <summary>
        /// Multipart request via Stream for all the methods
        /// </summary>
        /// <param name="fileReferenceName"></param>
        /// <param name="fileStream"></param>
        /// <param name="name"></param>
        /// <param name="queryParams"></param>
        /// <param name="postParams"></param>
        /// <param name="urlPost"></param>
        /// <param name="acceptType"></param>
        /// <returns>string</returns>
        /// <exception>Exception</exception>
        public static string MultiPartRequest(string fileReferenceName, Stream fileStream, string name, Dictionary<string, string> queryParams, Dictionary<string, string> postParams, string urlPost, string acceptType)
        {
            String lineEnd = "\r\n";
            String twoHyphens = "--";
            String boundary = "**********";
            // String fileName = System.IO.Path.GetFileName(filePath);

            StringBuilder queryString = new StringBuilder("?");
            // Get all Request parameter and set here
            ICollection keys = queryParams.Keys;
            foreach (String key in keys)
            {
                String value = queryParams[key];
                queryString.Append(key + "=" + value + "&");
                App42Log.Debug(" Setting value :" + key + " : " + value);
            }

            String urlString = urlPost + queryString;
            // Make a http Request

            App42Log.Debug("Upload URI : " + urlString);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Set the ContentType property of the WebRequest.
            request.ContentType = "multipart/form-data;boundary=" + boundary;
            request.Accept = Config.GetInstance().GetAccept();
            // Set the ContentLength property of the WebRequest.
            // Get the response.
            WebResponse response = null;
            string responseFromServer = null;
            try
            {
                Stream dataStreamRequest = request.GetRequestStream();

                //Start with Writing boundary and Form field
                byte[] byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + fileReferenceName + "\"" + ";"
                        + " filename=\"" + name + "\"" + lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes(lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                // Write the data to the request stream.
                //byteArray = File.ReadAllBytes(filePath);
                byteArray = ReadToEnd(fileStream);

                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;

                byteArray = Encoding.UTF8.GetBytes(lineEnd);
                dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                byteArray = null;


                if (postParams != null && postParams.Count > 0)
                {
                    byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;
                }
                else
                {
                    byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + twoHyphens + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;
                }

                ICollection keysParam = postParams.Keys;
                int i = 0;
                foreach (String key in keysParam)
                {
                    i++;
                    String value = postParams[key];
                    byteArray = Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"" + key
                            + "\"" + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;


                    byteArray = Encoding.UTF8.GetBytes(lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;

                    byteArray = Encoding.UTF8.GetBytes(value + lineEnd);
                    dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                    byteArray = null;

                    if (i == keysParam.Count)
                    {
                        byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + twoHyphens + lineEnd);
                        dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                        byteArray = null;
                    }
                    else
                    {
                        byteArray = Encoding.UTF8.GetBytes(twoHyphens + boundary + lineEnd);
                        dataStreamRequest.Write(byteArray, 0, byteArray.Length);
                        byteArray = null;
                    }
                    App42Log.Debug(" Setting Param  :" + key + " : " + value);
                }

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
        /// <summary>
        /// An exception to check whether the email entered is valid or not.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        public static void ThrowExceptionIfEmailNotValid(Object obj, String name)
        {
           
            if (obj == null)
            {
                throw new App42Exception(name + " can not be null");
            }
            String Email = obj.ToString();
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|" +
               @"0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z]" +
               @"[a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            System.Text.RegularExpressions.Match match =
                Regex.Match(Email, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
            }
            else
            {
                throw new App42Exception(name + " is Not Valid");
            }
        }
        /// <summary>
        /// An exception to check if the file has valid extension or not
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        public static void ThrowExceptionIfNotValidExtension(String fileName, String name)
        {

            if (fileName == null)
            {
                throw new App42Exception(name + " parameter can not be null ");
            }
            if (fileName.IndexOf('.') == -1)
                throw new App42Exception(name + " does not contain valid extension. ");
        }
        /// <summary>
        /// To check if the image has a valid extension or not.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        public static void ThrowExceptionIfNotValidImageExtension(String fileName, String name)
        {
            if (fileName == null)
            {
                throw new App42Exception(name + " parameter can not be null ");
            }

            if (fileName.IndexOf('.') == -1)
                throw new App42Exception(name + " does not contain valid extension. ");
            String ext = extractFileExtension(fileName);
            if (!(ext.Equals("jpg")) && !(ext.Equals("jpeg")) && !(ext.Equals("gif")) && !(ext.Equals("png")))
            {
                throw new ArgumentOutOfRangeException("The Request parameters are invalid. Only file with extensions jpg, jpeg, gif and png are supported");
            }

        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}