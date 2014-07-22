using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;

namespace WOTStatistics.Core
{
    public static class ApplicationSettings
    {
        public static bool AchievementReportVisible
        {
            get
            {
                object v = GetDetails("AchievementReportVisible");
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
                Save("AchievementReportVisible", value);
            }
        }

        public static string WOTStatsWebLink
        {
            get
            {
                object v = GetDetails("WOTStatsWebLink");
                if (v == null)
                {
                    return "http://www.vbaddict.net/wotstatistics";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("WOTStatsWebLink", value);
            }
        }

        public static string EUForumWebLink
        {
            get
            {
                object v = GetDetails("EUForumWebLink");
                if (v == null)
                {
                    return "http://www.vbaddict.net/support.php";
                }
                else
                {
                    return v.ToString();
                }
            }
            set
            {
                Save("EUForumWebLink", value);
            }
        }

        

        public static int MaxNoGamesAllowedRB
        {
            get
            {
                object v = GetDetails("MaxNoGamesAllowedRB");
                if (v == null)
                {
                    return 1000;
                }
                else
                {
                    return int.Parse(v.ToString());
                }
            }
            set
            {
                Save("MaxNoGamesAllowedRB", value);
            }
        }

        private static object GetDetails(string id)
        {
            object retValue = null;
            XmlDocument xmlDoc = new XmlDocument();

            XmlNode node = null;
            try
            {
                xmlDoc.Load(WOTHelper.GetSettingsFile());
                XmlElement root = xmlDoc.DocumentElement;

                node = root.SelectSingleNode(string.Format(@"AppSettings/Setting[@Key=""{0}""]", id));
            }
            catch { return null; }

            if (node != null)
                retValue = node.Attributes["Value"] == null ? null : node.Attributes["Value"].Value;
            else
                retValue = null;

            return retValue;
        }

        private static void Save(string id, object value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetSettingsFile());
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

            xmlDoc.Save(WOTHelper.GetSettingsFile());
        }

        public static string GetPropertyValue(string propertyName)
        {
            Type t = typeof(ApplicationSettings);
            PropertyInfo pi = t.GetProperty(propertyName);
            if (pi != null)
                return pi.GetValue(null, null).ToString();
            else
                return "";
        }
    }
    
}
