using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Web;

namespace Core.Utility
{
    public static class CustomLogger
    {
        public static void Write(object message)
        {
            if (HttpContext.Current != null)
            {
                System.Diagnostics.Debug.WriteLine(message);
            } else
            {
                Logger.Write(message);
            }
        }
    }
}
