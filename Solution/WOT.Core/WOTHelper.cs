using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace WOTStatistics.Core
{
    public static class WOTHelper
    {

        public static Guid CLSID_InternetSecurityManager = new Guid("7b8a2d94-0ac9-11d1-896c-00c04fb6bfc4");
        public static Guid IID_IInternetSecurityManager = new Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b");
        private static object _securityManager;
        private static IInternetSecurityManager _ism;   // IInternetSecurityManager interface of ecurityManager COM object

        public const UInt32 SZM_CREATE = 0;
        public const UInt32 SZM_DELETE = 0x1;

       

        public static string CleanUpLog()
        {
            AddToLog("Deleting old logfiles");

            foreach (System.IO.FileInfo oFile in new System.IO.DirectoryInfo(WOTHelper.GetApplicationData()).GetFiles("*.log"))
            {
                if (oFile.LastWriteTime < System.DateTime.Now.AddDays(-7))
                {
                    AddToLog("Deleting old logfile " + oFile.Name);

                    try
                    {
                        oFile.Delete();
                    }
                    catch (Exception ex)
                    {
                        AddToLog("Error: Cannot delete old logfile " + oFile.Name + ex.Message.ToString());
                    }

                }


            }

            return string.Empty;
        }

        public static string EnsureDateTimeString(int iDate)
        {
            string sDate = Convert.ToString(iDate);
            if (iDate < 10)
            {
                sDate = "0" + iDate;
            }

            return sDate;
        }

        public static string GetFormattedLogText(String sText)
        {
            System.Reflection.Assembly oAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName oName = oAssembly.GetName();
            return String.Format("{0} {1}: {2}", oName.Version.ToString(), DateTime.Now.ToLongTimeString(), sText);
        }


        public static void AddToLog(String sText)
        {
            sText = sText.Trim();
            if (sText.Length == 0)
            {
                return;
            }

            string sDate = String.Format("{0}{1}{2}", DateTime.Now.Year, EnsureDateTimeString(DateTime.Now.Month), EnsureDateTimeString(DateTime.Now.Day));

            string sLogfile = Path.Combine(WOTHelper.GetApplicationData(), string.Format("Log_{0}.log", sDate));

            sText = GetFormattedLogText(sText);
            Console.WriteLine(sText);
            try
            {
                System.IO.StreamWriter oWriter = new System.IO.StreamWriter(sLogfile, true, Encoding.Unicode);
                oWriter.WriteLine(sText);
                oWriter.Close();
            }
            catch
            {

            }


            if (!File.Exists(sLogfile))
            {

            }

            return;
        }

        public static string GetEXEPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public static string GetApplicationData()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics");
        }

        public static string GetTempFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "temp");
        }

        public static DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static void PrintDictionary(Dictionary<string, string> oDictionary)
        {
            WOTHelper.AddToLog("Printing Dictionary");
            foreach (KeyValuePair<string, string> kvp in oDictionary)
            {
                WOTHelper.AddToLog("Key = {0}, Value = {1}" + kvp.Key + kvp.Value);
            }

        }

         public static void WriteADURegistry(string sKey, string sValue)
         {
            WriteToRegistry(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\ActiveDossierUploader", sKey, sValue);
         }



        public static void WriteToRegistry(Microsoft.Win32.RegistryHive ParentKeyHive, string SubKeyName, string sKey, string sValue)
        {
          Microsoft.Win32.RegistryKey objSubKey = null;

            Microsoft.Win32.RegistryKey objParentKey = null;


            try {
	            switch (ParentKeyHive) {
		            case Microsoft.Win32.RegistryHive.ClassesRoot:
			            objParentKey = Microsoft.Win32.Registry.ClassesRoot;
			            break;
		            case Microsoft.Win32.RegistryHive.CurrentUser:
			            objParentKey = Microsoft.Win32.Registry.CurrentUser;
			            break;
		            case Microsoft.Win32.RegistryHive.LocalMachine:
			            objParentKey = Microsoft.Win32.Registry.LocalMachine;
			            break;
		            case Microsoft.Win32.RegistryHive.Users:
			            objParentKey = Microsoft.Win32.Registry.Users;

			            break;
	            }


	            //Open 
	            objSubKey = objParentKey.OpenSubKey(SubKeyName, true);
	            //create if doesn't exist
	            if (objSubKey == null) {
		            objSubKey = objParentKey.CreateSubKey(SubKeyName);
	            }


	            objSubKey.SetValue(sKey, sValue);
            } catch (UnauthorizedAccessException exae) {
                WOTStatistics.Core.WOTHelper.AddToLog("Unauthorized Access: " + exae.Message);
            } catch (Exception ex) {
                WOTStatistics.Core.WOTHelper.AddToLog("Error: " + ex.Message);
            }

            objSubKey = null;
            objParentKey = null;
          
           }




        public static void CreateAppDataFolder()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics"));
            }

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "temp")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "temp"));
            }

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Images")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Images"));
            }

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Images", "Tanks")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Images", "Tanks"));
            }

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Scripts")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "Scripts"));
            }

            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "python")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "python"));
            }

            CopyCurrentHistFiles();
        }

        public static string GetImagePath(string filename)
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "Images", "Tanks", filename)))
                return Path.Combine(GetApplicationData(), "Images", "Tanks", filename);
            else
                return Path.Combine(GetEXEPath(), "Images", "Tanks", filename);
        }

        //public static string GetCustomScript()
        //{
        //    if (File.Exists(Path.Combine(GetApplicationData(), "Scripts", "CustomScript.wss")))
        //        return Path.Combine(GetApplicationData(), "Scripts", "CustomScript.wss");
        //    else
        //        return Path.Combine(GetEXEPath(), "Scripts", "CustomScript.wss");
        //}

        public static string GetCustomScript(string scriptName)
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "Scripts", scriptName)))
                return Path.Combine(GetApplicationData(), "Scripts", scriptName);
            else
                return Path.Combine(GetEXEPath(), "Scripts", scriptName);
        }

        public static string GetPythonDirectory()
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "python", GetPythonFile())))
                return Path.Combine(GetApplicationData(), "python");
            else
                return Path.Combine(GetEXEPath(), "python");
        }

        public static string GetPythonFile()
        {
           return  "wotdc2j.exe";
        }

        public static string GetDBPath(string _playerID)
        {
            return Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _playerID, "LastBattle", "WOTSStore.db");
        }

        private static void CopyCurrentHistFiles()
        {
            DirectoryInfo AppPath = new DirectoryInfo(GetEXEPath());
            DirectoryInfo[] subDirs = AppPath.GetDirectories("Hist_*");
            foreach (DirectoryInfo dir in subDirs)
            {
                if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", dir.Name)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", dir.Name));

                    FileSystemInfo[] files = dir.GetFileSystemInfos();
                    foreach (FileSystemInfo file in files.OrderBy(f => f.Name))
                    {
                        File.Copy(file.FullName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", dir.Name, file.Name));
                    }
                }
            }
        }

        public static string GetUserFile()
        {
            return Path.Combine(GetApplicationData(), "user.xml");
        }

        public static string GetSettingsFile()
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "settings.xml")))
                return Path.Combine(GetApplicationData(), "settings.xml");
            else
                return Path.Combine(GetEXEPath(), "settings.xml");

        }

        public static string GetWN8ExpectedTankValuesFile()
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "expected_wn8.xml")))
                return Path.Combine(GetApplicationData(), "expected_wn8.xml");
            else
                return Path.Combine(GetEXEPath(), "expected_wn8.xml");

        }

        public static string GetTranslationFile()
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "translations.xml")))
                return Path.Combine(GetApplicationData(), "translations.xml");
            else
                return Path.Combine(GetEXEPath(), "translations.xml");

        }

        public static string GetChartFile()
        {
            if (File.Exists(Path.Combine(GetApplicationData(), "charts.xml")))
                return Path.Combine(GetApplicationData(), "charts.xml");
            else
                return Path.Combine(GetEXEPath(), "charts.xml");

        }

        public static void FindandKillProcess(string name)
        {
            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.Contains(name))
                    {
                        clsProcess.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        public static bool FindProcess(string name)
        {

            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.Contains(name))
                    {
                        if (clsProcess.Id != Process.GetCurrentProcess().Id)
                            return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        internal static string Encrypt(string plainText)
        {
            if (plainText == null) throw new ArgumentNullException("plainText");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            //return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        internal static string Decrypt(string cipher)
        {
            if (cipher == null) throw new ArgumentNullException("cipher");

            //parse base64 string
            try
            {
                byte[] data = Convert.FromBase64String(cipher);

                //decrypt data
                byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(decrypted);
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static void SetInternetZone()
        {
            Type t = Type.GetTypeFromCLSID(CLSID_InternetSecurityManager);
            _securityManager = Activator.CreateInstance(t);
            _ism = (IInternetSecurityManager)_securityManager;

            int result = _ism.SetZoneMapping(0, "about:blank", SZM_CREATE);
        }

        public static string PlayerIdFromDatFile(string filename)
        {
            string decodeName = String.Empty;
            try
            {
                Encoding byteConverter = Encoding.GetEncoding("UTF-8");
                decodeName = byteConverter.GetString(Base32.Decode(filename.Remove(filename.IndexOf('.')).ToLower()));
            }
            catch
            {
                try
                {
                    Encoding byteConverter = Encoding.GetEncoding("UTF-8");
                    decodeName = byteConverter.GetString(Base32.Decode(filename.Remove(filename.IndexOf('.'))));
                }
                catch (Exception)
                {

                }
            }
           
            if (decodeName.Contains(";"))
                return decodeName.Split(';')[1];
            else
                return "";
            
        }

        public static string FormatNumberToString(double value, int decimalPlaces)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                value = 0;
            }
            NumberFormatInfo nfi = new CultureInfo("ru-RU").NumberFormat;
            nfi.NumberDecimalDigits = decimalPlaces;
            return value.ToString("N", nfi).Replace(',','.');
        }

        public static double ConvertToUnixTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch

            // I have removed ToLocalTime() since it returns 1970-01-01 01:00 instead of 12:00, so each returned unix value had one hour more than expected
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0));
            
            //return the total seconds (which is a UNIX timestamp)
            return (double)span.TotalSeconds;
        }
    }

    [ComImport, GuidAttribute("79EAC9EE-BAF9-11CE-8C82-00AA004BA90B"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInternetSecurityManager
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetSecuritySite([In] IntPtr pSite);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetSecuritySite([Out] IntPtr pSite);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int MapUrlToZone([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, out UInt32 pdwZone, UInt32 dwFlags);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetSecurityId([MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, [MarshalAs(UnmanagedType.LPArray)] byte[] pbSecurityId, ref UInt32 pcbSecurityId, uint dwReserved);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ProcessUrlAction([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, UInt32 dwAction, out byte pPolicy, UInt32 cbPolicy, byte pContext, UInt32 cbContext, UInt32 dwFlags, UInt32 dwReserved);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryCustomPolicy([In, MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, ref Guid guidKey, ref byte ppPolicy, ref UInt32 pcbPolicy, ref byte pContext, UInt32 cbContext, UInt32 dwReserved);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetZoneMapping(UInt32 dwZone, [In, MarshalAs(UnmanagedType.LPWStr)] string lpszPattern, UInt32 dwFlags);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetZoneMappings(UInt32 dwZone, out UCOMIEnumString ppenumString, UInt32 dwFlags);
    }

}
