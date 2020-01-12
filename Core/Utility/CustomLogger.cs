using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web;

namespace Core.Utility
{
    public static class CustomLogger
    {
        public static bool WebMode = false;

        public static void Write(object message)
        {
            if (WebMode)
            {
                Console.WriteLine(message);
            } else
            {
                Logger.Write(message);
            }
        }
    }
}
