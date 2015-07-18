using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp
{
    public class App42Exception : SystemException
    {
        private int    httpErrorCode;
        private int    appErrorCode;

        public App42Exception(string message)
            : base(message)
        {

        }
        public App42Exception(string message, int    httpErrorCode,
            int    appErrorCode)
            : base(message)
        {
            this.httpErrorCode = httpErrorCode;
            this.appErrorCode = appErrorCode;
        }
        /// <summary>
        /// Sets the HttpErrorCode for the Exception
        /// </summary>
        /// <param name="httpErrorCode"> httpErrorCode The http error code e.g. 404, 500, 401 etc.</param>
        public virtual void SetHttpErrorCode(int    httpErrorCode)
        {
            this.httpErrorCode = httpErrorCode;
        }
        /// <summary>
        ///  Gets the HttpErrorCode for the Exception e.g. 404, 500, 401 etc.
        /// </summary>
        /// <returns></returns>
     
        public virtual int GetHttpErrorCode()
        {
            return this.httpErrorCode;
        }
         /// <summary>
         /// Sets the AppErrorCode for the Exception
         /// </summary>
         /// <param name="appErrorCode">
         /// appErrorCode App error codes correspond to the error which specific to the service.
         /// This error code can help App developers to take decisions and take action
         /// when a particular error occurs for a service
         /// </param>
         
        public virtual void SetAppErrorCode(int    appErrorCode)
        {
            this.appErrorCode = appErrorCode;
        }
        /// <summary>
        /// Gets the AppErrorCode for the Exception. App error codes correspond to the error which specific to the service.
        /// This error code can help App developers to take decisions and take action
        /// when a particular error occurs for a service 
        /// </summary>
        /// <returns></returns>
          
        public virtual int GetAppErrorCode()
        {
            return this.appErrorCode;
        }
    }
}
