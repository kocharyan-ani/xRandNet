using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class CustomLogger
    {
        public static bool webMode = false;

        public static void Write(object message)
        {
            if(webMode)
            {
                // @ToDo
            } else
            {
                CustomLogger.Write(message);
            }
        }
    }
}
