using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXCMCBedrockServerManagerCore.Utils
{
    public class ErrorHandler
    {
        // Called when the app is notifying of an error that it has no way of resolving
        public delegate void OnErrorMessageDelegate(string title, string message);

        // Called when the app is notifying of an error that it can handle, return true if you want the app
        // to handle it
        public delegate bool OnErrorRequestHandleDelegate(string title, string message);

        public static OnErrorMessageDelegate OnErrorMessage { get; set; } = null;
        public static OnErrorRequestHandleDelegate OnErrorRequestHandle { get; set; } = null;

        public static void RaiseErrorMessage(string title, string message)
        {
            if(OnErrorMessage != null)
            {
                OnErrorMessage(title, message);
            }
        }

        public static bool RaiseErrorRequestHandle(string title, string message)
        {
            if(OnErrorRequestHandle != null)
            {
                return OnErrorRequestHandle(title, message);
            }

            return false;
        }
    }
}
