using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace WOTStatistics.Core
{
    public static class Translations
    {
        private static Dictionary<string, string> _translation { get; set; }
        private static Dictionary<string, Languages> _languages { get; set; }
        private static string _langID = "";
        private static string _defLangID = "";

        public static string TranslationGet(string fieldID, string langID, string defaultValue)
        {

            if (string.IsNullOrEmpty(_langID))
                _langID = UserSettings.LangID;

            if (string.IsNullOrEmpty(_defLangID))
                _defLangID = UserSettings.DefaultLangID;
            if (_langID != "SHOW_KEYS")
            {

                string description = "";
                _translation.TryGetValue(String.Format("{0}|{1}", fieldID, _langID), out description);

                if (string.IsNullOrEmpty(description))
                    _translation.TryGetValue(String.Format("{0}|{1}", fieldID, _defLangID), out description);

                if (string.IsNullOrEmpty(description))
                    return defaultValue;
                else
                    return ReplaceTokens(description);
            }
            else
            {
                return fieldID + "(" + defaultValue + ")";
            }

        }

        private static string ReplaceTokens(string value)
        {
            string returnValue = value;
            if (returnValue.Contains("{htmlbreak}"))
                returnValue = returnValue.Replace("{htmlbreak}", "<br/>");

            if (returnValue.Contains("{winbreak}"))
                returnValue = returnValue.Replace("{winbreak}", Environment.NewLine);

            return returnValue;

        }

        public static string LanguageGet(string langID, Language requieredLanguage)
        {
            Languages temp;
            if (_languages.TryGetValue(langID, out temp))
                switch (requieredLanguage)
                {
                    case Language.National:
                        return temp.National;
                    case Language.English:
                        return temp.English;
                    default:
                        return temp.English;
                }
            else
                return "Unknown";
        }

        public static string LanguageKeyGet(string langName, Language requieredLanguage)
        {

            foreach (KeyValuePair<string, Languages> item in _languages)
            {
                switch (requieredLanguage)
                {
                    case Language.National:
                        if (item.Value.National == langName)
                            return item.Key;
                        break;
                    default:
                        if (item.Value.English == langName)
                            return item.Key;
                        break;
                }
            }

            return "unknown";

        }

        public static Dictionary<string, Languages> LanguageListGet()
        {
            return _languages;
        }

        static Translations()
        {
            _translation = new Dictionary<string, string>();
            _languages = new Dictionary<string, Languages>();
            LoadFile();
        }


        public static void Reload()
        {
            LoadFile();
        }
        private static void LoadFile()
        {
            _translation.Clear();
            _languages.Clear();
            try
            {
                //Lets load the xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(WOTHelper.GetTranslationFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodeList = root.SelectSingleNode("//WorldOfTanks").ChildNodes;
                foreach (XmlNode baseNode in nodeList)

                    foreach (XmlNode childNode in baseNode.ChildNodes)
                        if (childNode.Name.ToUpper() == "LANGUAGES")
                            foreach (XmlNode fieldNode in childNode.ChildNodes)
                            {
                                string national = fieldNode.SelectSingleNode("NameNat").InnerText;
                                string english = fieldNode.SelectSingleNode("NameENG").InnerText;
                                WOTHelper.AddToLog("Adding Language to dict: " + english);
                                _languages.Add(fieldNode.Attributes["ID"].Value, new Languages() { National = national, English = english });
                            }
                        else if (childNode.Name.ToUpper() == "FIELDS")
                            foreach (XmlNode fieldNode in childNode.ChildNodes)
                                foreach (XmlNode item in fieldNode)
                                    if (item.Name != "#comment")
                                        if (!_translation.ContainsKey(String.Format("{0}|{1}", fieldNode.Attributes["Name"].Value, item.Attributes["LanID"].Value)))
                                            _translation.Add(String.Format("{0}|{1}", fieldNode.Attributes["Name"].Value, item.Attributes["LanID"].Value), item.InnerText);
            }
            catch (Exception ex)
            {
                WOTHelper.AddToLog("Error with Translation: " + ex.Message);
                // throw ex;
            }
        }
    }

    public enum Language
    {
        National,
        English
    }

    public class Languages
    {
        public string National { get; set; }
        public string English { get; set; }
    }

}
