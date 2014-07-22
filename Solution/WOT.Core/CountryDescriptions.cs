using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;

namespace WOTStatistics.Core
{
    public class CountryDescriptions : IEnumerable<KeyValuePair<int, string>>, IDisposable
    {
        private readonly Dictionary<int, string> _countries = new Dictionary<int, string>();
        private MessageQueue _message;

        public CountryDescriptions(MessageQueue message)
        {
            _message = message;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(WOTHelper.GetSettingsFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodes = root.SelectSingleNode(@"Countries").ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    _countries.Add(int.Parse(node.Attributes["Code"].Value), Translations.TranslationGet(node.Attributes["Code"].Value, "DE", node.InnerText));
                }
            }
            catch (Exception)
            {
                File.Copy(Path.Combine(WOTHelper.GetEXEPath(), "settings.xml"), WOTHelper.GetSettingsFile());
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(WOTHelper.GetSettingsFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodes = root.SelectSingleNode(@"Countries").ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    _countries.Add(int.Parse(node.Attributes["Code"].Value), Translations.TranslationGet(node.Attributes["Code"].Value, "DE", node.InnerText));
                }
            }
        }

        public string Description(int id)
        {
            string country = "";
            if (!_countries.TryGetValue(id, out country))
                country = "Unknown";

            return country;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
        {
            return this._countries.GetEnumerator();
        }

        public void Dispose()
        {
            _countries.Clear();
        }
    }
}