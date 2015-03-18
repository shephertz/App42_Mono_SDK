using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp
{
    public class App42Log
    {
        private static Boolean debug = false;
        public static Boolean IsDebug()
        {
            return debug;
        }

        public static void SetDebug(Boolean debug)
        {
            App42Log.debug = debug;
        }

        public static void Info(String msg)
        {
           App42Log.Debug(msg);
        }
        public static void Debug(String msg)
        {
            if (debug)
               App42Log.Debug(msg);
        }
        public static void Error(String msg)
        {
           App42Log.Debug(msg);
        }
        public static void Fatal(String msg)
        {
           App42Log.Debug(msg);
        }

    }
}
