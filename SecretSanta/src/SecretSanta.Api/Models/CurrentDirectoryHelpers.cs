﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Api.Models
{

    internal class CurrentDirectoryHelpers
    {
        internal static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            internal static extern IntPtr GetModuleHandle(string lpModuleName);

            [System.Runtime.InteropServices.DllImport(AspNetCoreModuleDll)]
            static extern int http_get_application_properties(ref IISConfigurationData iiConfigData);
        }

        internal const string AspNetCoreModuleDll = "aspnetcorev2_inprocess.dll";

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct IISConfigurationData
        {
            public IntPtr pNativeApplication;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzFullApplicationPath;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.BStr)]
            public string pwzVirtualApplicationPath;
            public bool fWindowsAuthEnabled;
            public bool fBasicAuthEnabled;
            public bool fAnonymousAuthEnable;
        }

        public static void SetCurrentDirectory()
        {
            try
            {
                // Check if physical path was provided by ANCM
                var sitePhysicalPath = Environment.GetEnvironmentVariable("ASPNETCORE_IIS_PHYSICAL_PATH");
                if (string.IsNullOrEmpty(sitePhysicalPath))
                {
                    // Skip if not running ANCM InProcess
                    if (NativeMethods.GetModuleHandle(AspNetCoreModuleDll) == IntPtr.Zero)
                    {
                        return;
                    }

                    IISConfigurationData configurationData = default(IISConfigurationData);
                    if (NativeMethods.http_get_application_properties(ref configurationData) != 0)
                    {
                        return;
                    }

                    sitePhysicalPath = configurationData.pwzFullApplicationPath;
                }

                Environment.CurrentDirectory = sitePhysicalPath;
            }
            catch
            {
                // ignore
            }
        }
    }
}
