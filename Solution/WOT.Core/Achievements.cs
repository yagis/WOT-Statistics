using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

namespace WOTStatistics.Core
{
    public class Achievements : IEnumerable<KeyValuePair<int, Achievement>>, IDisposable
    {
        private readonly Dictionary<int, Achievement> _achievements = new Dictionary<int, Achievement>();

        public Achievements()
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetSettingsFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"Achievements").ChildNodes;

            foreach (XmlNode node in nodes)
            {
                string trKey = node.Attributes["Key"] == null ? "" : node.Attributes["Key"].Value;
                _achievements.Add(int.Parse(node.Attributes["Order"].Value), new Achievement() { Name = Translations.TranslationGet(trKey,"DE",node.Attributes["Name"].Value), 
                                                                                                   Value = Convert.ToInt16(node.Attributes["Value"].Value), 
                                                                                                   Tanks = node.Attributes["Tanks"].Value.Split('|') });
            }
        }

        public Achievement Description(int id)
        {
            Achievement achievement;
            if (!_achievements.TryGetValue(id, out achievement))
                new Achievement() {Name = "Unknown", Tanks = { }, Value = 0 };

            return achievement;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<int, Achievement>> GetEnumerator()
        {
            return this._achievements.GetEnumerator();
        }

        public void Dispose()
        {
            _achievements.Clear();
        }
    }

    public class Achievement
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string[] Tanks { get; set; }
    }
}
