using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web;

namespace Core.Utility
{
    public static class CustomLogger
    {
        public static bool webMode = false;

        public static void Write(object message)
        {
            if (webMode)
            {
                System.Diagnostics.Debug.WriteLine(message);
            } else
            {
                Logger.Write(message);
            }
        }
    }
}
