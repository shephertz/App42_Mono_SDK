using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp
{
    /// <summary>
    /// This exception maps to Http Error Code 413. It is thrown when any of the limits are crossed. E.g. No. Of calls to API or storage limits.
    /// </summary>
    public class App42LimitException : App42Exception
    {
        private int httpErrorCode;
        private int appErrorCode;

        public App42LimitException(string message)
            : base(message)
        {

        }
        public App42LimitException(string message, int httpErrorCode,
            int appErrorCode)
            : base(message, httpErrorCode, appErrorCode)
        {
            this.httpErrorCode = httpErrorCode;
            this.appErrorCode = appErrorCode;
        }

        /// <summary>
        /// Sets the HttpErrorCode for the Exception 
        /// </summary>
        /// <param name="httpErrorCode">httpErrorCode The http error code e.g. 404, 500, 401 etc.</param>

        public void SetHttpErrorCode(int httpErrorCode)
        {
            this.httpErrorCode = httpErrorCode;
        }

        /// <summary>
        /// Gets the HttpErrorCode for the Exception e.g. 404, 500, 401 etc.
        /// </summary>

        public int GetHttpErrorCode()
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
        public void SetAppErrorCode(int appErrorCode)
        {
            this.appErrorCode = appErrorCode;
        }

        /// <summary>
        /// Gets the AppErrorCode for the Exception. App error codes correspond to the error which specific to the service.
        /// This error code can help App developers to take decisions and take action
        /// when a particular error occurs for a service 
        /// </summary>

        public int GetAppErrorCode()
        {
            return this.appErrorCode;
        }
    }
}
