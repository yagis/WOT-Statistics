using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Xml;

namespace WOTStatistics.Core
{
    public class TankTypeDescription : IEnumerable<KeyValuePair<string, string>>, IDisposable
    {
        private readonly Dictionary<string, string> _tankTypes = new Dictionary<string, string>();
        private MessageQueue _message;

        public TankTypeDescription(MessageQueue message)
        {
            _message = message;
          
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(WOTHelper.GetSettingsFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodes = root.SelectSingleNode(@"TankTypes").ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    _tankTypes.Add(node.Attributes["Code"].Value, Translations.TranslationGet(node.Attributes["Code"].Value, "DE", node.InnerText));
                }
           
        }

        public string Description(string tankType)
        {
            string tValue;
            if (!_tankTypes.TryGetValue(tankType, out tValue))
                return "Unknown";
            else
                return tValue;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this._tankTypes.GetEnumerator();
        }

        public void Dispose()
        {
            _tankTypes.Clear();
        }


    }
}