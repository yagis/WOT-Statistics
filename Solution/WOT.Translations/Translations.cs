using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Windows.Forms;

namespace WOT.Translations
{
    class Translations
    {
        DataTable _dtTranslations;
        private Dictionary<string, string> translation { get; set; }
        private Dictionary<string, Languages> languages { get; set; }

        public string TranslationGet(string fieldID, string langID, string defaultValue)
        {
            string description = "";
            if (translation.TryGetValue(String.Format("{0}|{1}", fieldID, langID), out description))
                return description;
            else
                return defaultValue;
        }

        public string LanguageGet(string langID, Language requieredLanguage)
        {
            Languages temp;
            if (languages.TryGetValue(langID, out temp))
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

        public Dictionary<string, Languages> LanguageListGet()
        {
            return languages;
        }

        public DataTable GetData()
        {
            return _dtTranslations;
        }

        public Translations(string file)
        {
            try
            {
                _dtTranslations = new DataTable();
                _dtTranslations.Columns.Add(new DataColumn() { Caption = "Key", ColumnName = "Key", DataType = typeof(string) });
                languages = new Dictionary<string, Languages>();

                //translation = new Dictionary<string, string>();
                //languages = new Dictionary<string, Languages>();

                //Lets load the xml
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodeList = root.SelectSingleNode("//WorldOfTanks").ChildNodes;
                foreach (XmlNode baseNode in nodeList)
                    foreach (XmlNode childNode in baseNode.ChildNodes)
                        if (childNode.Name.ToUpper() == "LANGUAGES")
                            foreach (XmlNode fieldNode in childNode.ChildNodes)
                            {
                                string national = fieldNode.SelectSingleNode("NameNat").InnerText;
                                string english = fieldNode.SelectSingleNode("NameENG").InnerText;
                                if (fieldNode.Attributes["ID"].Value != "SHOW_KEYS")
                                {
                                    _dtTranslations.Columns.Add(new DataColumn() { Caption = english + " - " + national, ColumnName = fieldNode.Attributes["ID"].Value, DataType = typeof(string) });
                                    languages.Add(fieldNode.Attributes["ID"].Value, new Languages() { National = national, English = english });
                                }
                            }
                        else if (childNode.Name.ToUpper() == "FIELDS")
                        {
                            foreach (XmlNode fieldNode in childNode.ChildNodes)
                            {
                                foreach (XmlNode item in fieldNode)
                                {
                                    if (item.Name != "#comment")
                                        if (_dtTranslations.Select("key = '" + fieldNode.Attributes["Name"].Value + "'").Count() <= 0)
                                        {
                                            DataRow dr = _dtTranslations.NewRow();
                                            dr["key"] = fieldNode.Attributes["Name"].Value;
                                            dr[item.Attributes["LanID"].Value] = item.InnerText;
                                            _dtTranslations.Rows.Add(dr);
                                        }
                                        else
                                        {
                                            DataRow[] dr = _dtTranslations.Select("key = '" + fieldNode.Attributes["Name"].Value + "'");
                                           //Console.WriteLine("L " + item.Attributes["LanID"].Value);
                                            //Console.WriteLine("I " + item.InnerText);
                                            dr[0][item.Attributes["LanID"].Value] = item.InnerText;
                                        }
                                }
                            }
                        }
            }
            catch (Exception ex)
            {

                DevExpress.XtraEditors.XtraMessageBox.Show("Error loading file." + Environment.NewLine + ex.Message);
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
