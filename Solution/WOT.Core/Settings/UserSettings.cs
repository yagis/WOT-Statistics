using System;
using System.Drawing;
using System.Reflection;
using System.Xml;


namespace WOTStatistics.Core
{
    public static class UserSettings
    {

        public static bool StartMonOnStartUp
        {
            get
            {
                object v = GetDetails("StartMonOnStartUp");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("StartMonOnStartUp", value);
            }
        }
        public static bool DBPasswordProtected
        {
            get
            {
                object v = GetDetails("DBPasswordProtected");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("DBPasswordProtected", value);
            }
        }
        public static bool LaunchWithWindows
        {
            get
            {
                object v = GetDetails("LaunchWithWindows");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("LaunchWithWindows", value);
            }
        }
        public static bool MinimiseonStartup
        {
            get
            {
                object v = GetDetails("MinimiseonStartup");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("MinimiseonStartup", value);
            }
        }
        public static bool MinimiseToTray
        {
            get
            {
                object v = GetDetails("MinimiseToTray");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("MinimiseToTray", value);
            }
        }

        public static string ConvertFlatFilesToDB
        {
            get
            {
                object v = GetDetails("ConvertFlatFilesToDB");
                if (v == null)
                {
                    return "";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ConvertFlatFilesToDB", value);
            }
        }

        public static string ClearRecentBattlesFragList
        {
            get
            {
                object v = GetDetails("ClearRecentBattlesFragList");
                if (v == null)
                {
                    return "";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ClearRecentBattlesFragList", value);
            }
        }

     

        public static int LastPlayedCompare
        {
            get
            {
                object v = GetDetails("LastPlayedCompare");
                if (v == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("LastPlayedCompare", value);
            }
        }

        public static int TopXTake
        {
            get
            {
                object v = GetDetails("TopXTake");
                if (v == null)
                {
                    return 7;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("TopXTake", value);
            }
        }
        public static string SettingsFileVersion
        {
            get
            {
                object v = GetDetails("SettingsFileVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("SettingsFileVersion", value);
            }
        }
        public static string ViewSessionID
        {
            get
            {
                object v = GetDetails("ViewSessionID");
                if (v == null)
                {
                    return null;
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ViewSessionID", value);
            }
        }
        public static string TranslationFileVersion
        {
            get
            {
                object v = GetDetails("TranslationFileVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("TranslationFileVersion", value);
            }
        }
        public static string LastReleaseNotes
        {
            get
            {
                object v = GetDetails("LastReleaseNotes");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("LastReleaseNotes", value);
            }
        }
        public static string WN8ExpectedVersion
        {
            get
            {
                object v = GetDetails("WN8ExpectedVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("WN8ExpectedVersion", value);
            }
        }
        public static string RatingVersion
        {
            get
            {
                object v = GetDetails("RatingVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("RatingVersion", value);
            }
        }
        //public static string RatingSystem
        //{
        //    get
        //    {
        //        object v = GetDetails("RatingSystem");
        //        if (v == null)
        //        {
        //            return "EFF";
        //        }
        //        else
        //        {
        //            return v.ToString();
        //        }
        //    }
        //    set
        //    {
        //        Save("RatingSystem", value);
        //    }
        //}
        public static string ScriptFileVersion
        {
            get
            {
                object v = GetDetails("ScriptFileVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ScriptFileVersion", value);
            }
        }
        public static bool AllowVersionCheck
        {
            get
            {
                object v = GetDetails("AllowVersionCheck");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AllowVersionCheck", value);
            }
        }
        public static int VersionCounter
        {
            get
            {
                object v = GetDetails("VersionCounter");
                if (v == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("VersionCounter", value);
            }
        }
        public static string SystemFont
        {
            get
            {
                object v = GetDetails("SystemFont");
                if (v == null)
                {
                    return "Arial Unicode MS";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("SystemFont", value);
            }
        }
        public static string BattleMode
        {
            get
            {
                object v = GetDetails("BattleMode");
                if (v == null)
                {
                    return "All";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("BattleMode", value);
            }
        }
        public static System.Drawing.Font UniCodeFont
        {
            get
            {
                object v = GetDetails("UniCodeFont");
                if (v == null)
                {
                    return new System.Drawing.Font("Arial Unicode MS", 9);
                }
                else
                {
                    return (System.Drawing.Font)v;
                }
            }
            set
            {
                Save("UniCodeFont", value);
            }
        }
        public static string HTMLCellFont
        {
            get
            {
                object v = GetDetails("HTMLCellFont");
                if (v == null)
                {
                    return "14";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("HTMLCellFont", value);
            }
        }
        public static string HTMLHeaderFont
        {
            get
            {
                object v = GetDetails("HTMLHeaderFont");
                if (v == null)
                {
                    return "18";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("HTMLHeaderFont", value);
            }
        }
        public static string HTMLTankInfoHeader
        {
            get
            {
                object v = GetDetails("HTMLTankInfoHeader");
                if (v == null)
                {
                    return "20";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("HTMLTankInfoHeader", value);
            }
        }
        public static bool HTMLShowMovementPics
        {
            get
            {
                object v = GetDetails("HTMLShowMovementPics");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("HTMLShowMovementPics", value);
            }
        }
        public static bool KillCountsShowTierTotals
        {
            get
            {
                object v = GetDetails("KillCountsShowTierTotals");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("KillCountsShowTierTotals", value);
            }
        }
        public static bool KillCountsShowRowTotals
        {
            get
            {
                object v = GetDetails("KillCountsShowRowTotals");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("KillCountsShowRowTotals", value);
            }
        }
        public static bool KillCountsShowColumnTotals
        {
            get
            {
                object v = GetDetails("KillCountsShowColumnTotals");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("KillCountsShowColumnTotals", value);
            }
        }
        public static string DateFormat
        {
            get
            {
                object v = GetDetails("DateFormat");
                if (v == null)
                {
                    return "yyyy-MM-dd";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("DateFormat", value);
            }
        }
        public static bool TimeStamp
        {
            get
            {
                object v = GetDetails("TimeStamp");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("TimeStamp", value);
            }
        }
        public static string TimeFormat
        {
            get
            {
                object v = GetDetails("TimeFormat");
                if (v == null)
                {
                    return "HH:mm";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("TimeFormat", value);
            }
        }
        public static string GroupLPT
        {
            get
            {
                object v = GetDetails("GroupLPT");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("GroupLPT", value);
            }
        }
        public static bool AllowvBAddictUpload
        {
            get
            {
                object v = GetDetails("AllowvBAddictUpload");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AllowvBAddictUpload", value);
            }
        }
        public static bool AllowvBAddictUploadDossier
        {
            get
            {
                object v = GetDetails("AllowvBAddictUploadDossier");
                if (v == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AllowvBAddictUploadDossier", value);
            }
        }
        public static bool AllowvBAddictUploadDossierBattleResult
        {
            get
            {
                object v = GetDetails("AllowvBAddictUploadDossierBattleResult");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AllowvBAddictUploadDossierBattleResult", value);
            }
        }
        public static bool AllowvBAddictUploadDossierBattleResultReplay
        {
            get
            {
                object v = GetDetails("AllowvBAddictUploadDossierBattleResultReplay");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AllowvBAddictUploadDossierBattleResultReplay", value);
            }
        }
        public static int LastPlayedCompareQuota
        {
            get
            {
                object v = GetDetails("LastPlayedCompareQuota");
                if (v == null)
                {
                    return 20;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("LastPlayedCompareQuota", value);
            }
        }
        public static int ColorPositive
        {
            get
            {
                object v = GetDetails("ColorPositive");
                if (v == null)
                {
                    return Color.Green.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("ColorPositive", value);
            }
        }
        public static int ColorNeutral
        {
            get
            {
                object v = GetDetails("ColorNeutral");
                if (v == null)
                {
                    return Color.Yellow.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("ColorNeutral", value);
            }
        }
        public static int ColorNegative
        {
            get
            {
                object v = GetDetails("ColorNegative");
                if (v == null)
                {
                    return Color.Red.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("ColorNegative", value);
            }
        }
        public static string ChartPalette
        {
            get
            {
                object v = GetDetails("ChartPalette");
                if (v == null)
                {
                    return "Northern Lights";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ChartPalette", value);
            }
        }
        public static string ChartAppearance
        {
            get
            {
                object v = GetDetails("ChartAppearance");
                if (v == null)
                {
                    return "Dark";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("ChartAppearance", value);
            }
        }
        public static int colorWNClass1
        {
            get
            {
                object v = GetDetails("colorWNClass1");
                if (v == null)
                {
                    return Color.Black.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass1", value);
            }
        }
        public static int colorWNClass2
        {
            get
            {
                object v = GetDetails("colorWNClass2");
                if (v == null)
                {
                    return Color.DarkRed.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass2", value);
            }
        }
        public static int colorWNClass3
        {
            get
            {
                object v = GetDetails("colorWNClass3");
                if (v == null)
                {
                    return Color.Red.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass3", value);
            }
        }
        public static int colorWNClass4
        {
            get
            {
                object v = GetDetails("colorWNClass4");
                if (v == null)
                {
                    return Color.Yellow.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass4", value);
            }
        }
        public static int colorWNClass5
        {
            get
            {
                object v = GetDetails("colorWNClass5");
                if (v == null)
                {
                    return Color.Green.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass5", value);
            }
        }
        public static int colorWNClass6
        {
            get
            {
                object v = GetDetails("colorWNClass6");
                if (v == null)
                {
                    return Color.DarkGreen.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass6", value);
            }
        }
        public static int colorWNClass7
        {
            get
            {
                object v = GetDetails("colorWNClass7");
                if (v == null)
                {
                    return Color.Blue.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass7", value);
            }
        }
        public static int colorWNClass8
        {
            get
            {
                object v = GetDetails("colorWNClass8");
                if (v == null)
                {
                    return Color.Purple.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass8", value);
            }
        }
        public static int colorWNClass9
        {
            get
            {
                object v = GetDetails("colorWNClass9");
                if (v == null)
                {
                    return Color.Indigo.ToArgb();
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("colorWNClass9", value);
            }
        }
        public static bool Cloud_Allow
        {
            get
            {
                object v = GetDetails("CloudAllow");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("CloudAllow", value);
            }
        }
        public static string Cloud_Path
        {
            get
            {
                object v = GetDetails("CloudPath");
                if (v == null)
                {
                    return "";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("CloudPath", value);
            }
        }
        public static string AppVersion
        {
            get
            {
                Assembly asm = Assembly.LoadFrom("WOTStatistics.Stats.exe");
                return asm.GetName().Version.ToString();
            }
        }
        public static bool NewVersionNotify
        {
            get
            {
                object v = GetDetails("NewVersionNotify");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("NewVersionNotify", value);
            }
        }
        public static string NewAppVersion
        {
            get
            {
                object v = GetDetails("NewAppVersion");
                if (v == null)
                {
                    return "0.0.0.0";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("NewAppVersion", value);
            }
        }

        public static string ActiveDossierUploaderVersion
        {
            get
            {
                object v = GetDetails("ActiveDossierUploaderVersion");
                if (v == null)
                {
                    return "0.0.0.0";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("ActiveDossierUploaderVersion", value);
            }
        }

        public static string DossierVersion
        {
            get
            {
                object v = GetDetails("DossierVersion");
                if (v == null)
                {
                    return "0";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("DossierVersion", value);
            }
        }
        public static int RecentBattlesCurrentSession
        {
            get
            {
                object v = GetDetails("RecentBattlesCurrentSession");
                if (v == null)
                {
                    return 0;
                }
                else
                {
                    int outValue; ;
                    if (!int.TryParse(v.ToString(), out outValue))
                        return 0;

                    return outValue;
                }
            }
            set
            {
                Save("RecentBattlesCurrentSession", value);
            }
        }
        public static string LangID
        {
            get
            {
                object v = GetDetails("LangID");
                if (v == null)
                {
                    return "ENG";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("LangID", value);
            }
        }
        public static string DefaultLangID
        {
            get
            {
                object v = GetDetails("DefaultLangID");
                if (v == null)
                {
                    return "ENG";
                }
                else
                {
                    return (string)v;
                }
            }
            set
            {
                Save("DefaultLangID", value);
            }
        }
        public static double TimeAdjustment
        {
            get
            {
                object v = GetDetails("TimeAdjustment");
                if (v == null)
                {
                    return 0;
                }
                else
                {
                    return double.Parse(v.ToString());
                }
            }
            set
            {
                Save("TimeAdjustment", value);
            }
        }
        public static int TopMinPlayed
        {
            get
            {
                object v = GetDetails("TopMinPlayed");
                if (v == null)
                {
                    return 50;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("TopMinPlayed", value);
            }
        }

        public static bool AutoCreateSession
        {
            get
            {
                object v = GetDetails("AutoCreateSession");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoCreateSession", value);
            }
        }

        public static bool AutoSessionXHours
        {
            get
            {
                object v = GetDetails("AutoSessionXHours");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionXHours", value);
            }
        }
        public static bool AutoSessionXHoursMessage
        {
            get
            {
                object v = GetDetails("AutoSessionXHoursMessage");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionXHoursMessage", value);
            }
        }
        public static int AutoSessionXHoursValue
        {
            get
            {
                object v = GetDetails("AutoSessionXHoursValue");
                if (v == null)
                {
                    return 2;
                }
                else
                {
                    int outValue; ;
                    if (!int.TryParse(v.ToString(), out outValue))
                        return 0;

                    return outValue;
                }
            }
            set
            {
                Save("AutoSessionXHoursValue", value);
            }
        }

        public static bool AutoSessionOnStartUp
        {
            get
            {
                object v = GetDetails("AutoSessionOnStartUp");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionOnStartUp", value);
            }
        }
        public static bool AutoSessionOnStartUpMessage
        {
            get
            {
                object v = GetDetails("AutoSessionOnStartUpMessage");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionOnStartUpMessage", value);
            }
        }

        public static bool AutoSessionXBattles
        {
            get
            {
                object v = GetDetails("AutoSessionXBattles");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionXBattles", value);
            }
        }
        public static bool AutoSessionXBattlesMessage
        {
            get
            {
                object v = GetDetails("AutoSessionXBattlesMessage");
                if (v == null)
                {
                    return false;
                }
                else
                {
                    return bool.Parse(v.ToString().ToLower());
                }
            }
            set
            {
                Save("AutoSessionXBattlesMessage", value);
            }
        }
        public static int AutoSessionXBattlesValue
        {
            get
            {
                object v = GetDetails("AutoSessionXBattlesValue");
                if (v == null)
                {
                    return 10;
                }
                else
                {
                    int outValue; ;
                    if (!int.TryParse(v.ToString(), out outValue))
                        return 0;

                    return outValue;
                }
            }
            set
            {
                Save("AutoSessionXBattlesValue", value);
            }
        }

        private static object GetDetails(string id)
        {
            object retValue = null;
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode node = null;
            try
            {
                xmlDoc.Load(WOTHelper.GetUserFile());
                XmlElement root = xmlDoc.DocumentElement;

                node = root.SelectSingleNode(string.Format(@"AppSettings/Setting[@Key=""{0}""]", id));
            }
            catch { }

            if (node != null)
                retValue = node.Attributes["Value"] == null ? null : node.Attributes["Value"].Value;
            else
                retValue = null;

            return retValue;
        }

        private static void Save(string id, object value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNode settingGroup = root.SelectSingleNode("AppSettings");

            if (settingGroup == null)
            {
                settingGroup = xmlDoc.CreateElement("AppSettings");
                root.AppendChild(settingGroup);
            }

            XmlElement iSetting = (XmlElement)root.SelectSingleNode(string.Format(@"AppSettings/Setting[@Key=""{0}""]", id));

            if (iSetting == null)
            {
                iSetting = xmlDoc.CreateElement("Setting");
                settingGroup.AppendChild(iSetting);
            }


            iSetting.SetAttribute("Key", id);
            iSetting.SetAttribute("Value", value.ToString());

            xmlDoc.Save(WOTHelper.GetUserFile());
        }

        public static string GetPropertyValue(string propertyName)
        {
            Type t = typeof(UserSettings);
            PropertyInfo pi = t.GetProperty(propertyName);
            if (pi != null)
                return pi.GetValue(null, null).ToString();
            else
                return "";
        }

    }

    public class PropertyFields
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public Type FieldType { get; set; }
        public bool IsDirty
        {
            get
            {
                if (OldValue.ToString() != NewValue.ToString())
                    return true;
                else
                    return false;
            }
        }



    }



}
