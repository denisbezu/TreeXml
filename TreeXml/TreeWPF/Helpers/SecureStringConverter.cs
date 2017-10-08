using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TreeWPF.Helpers
{
    public class SecureStringConverter
    {
        public static string ConvertToString(SecureString secureString)
        {
            if (secureString == null)
                return "";
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}