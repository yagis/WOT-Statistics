using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


namespace WOTStatistics.Core
{
    public class CustomGrouping : IEnumerable<KeyValuePair<string, Tuple<string, string>>>
    {
        private readonly Dictionary<string, Tuple<string,string>> _customGroupings = new Dictionary<string, Tuple<string,string>>();
        private MessageQueue _message;
        public int Count { get { return _customGroupings.Count; } }
        public CustomGrouping(MessageQueue message)
        {
            _message = message;
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"CustomGroups").ChildNodes;

            foreach (XmlNode node in nodes)
            {
                if (!_customGroupings.ContainsKey(node.Attributes["Name"] == null ? "" : node.Attributes["Name"].Value))
                    _customGroupings.Add(node.Attributes["Name"] == null ? "" : node.Attributes["Name"].Value, new Tuple<string, string>(node.Attributes["Caption"] == null ? "" : node.Attributes["Caption"].Value, node.InnerText));
                
            }
        }

        public string Description(string id)
        {
            Tuple<string, string> tValue;
            if (!_customGroupings.TryGetValue(id, out tValue))
                return "Unknown";
            else
                return tValue.Item1;
        }

        public string Values(string groupName)
        {
            Tuple<string, string> tValue;
            if (!_customGroupings.TryGetValue(groupName, out tValue))
                return "Unknown";
            else
                return tValue.Item2;
        }

        public void Remove(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList oldCustomGroups = root.SelectNodes(string.Format(@"CustomGroups/CustomGroup[@Name=""{0}""]", name.Replace(" ", "_")));
            try
            {
                foreach (XmlNode item in oldCustomGroups)
                {
                    item.ParentNode.RemoveChild(item);
                }

            }
            catch
            {
                _message.Add("Info : cant find item in settings.");
            }

            xmlDoc.Save(WOTHelper.GetUserFile());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, Tuple<string, string>>> GetEnumerator()
        {
            return this._customGroupings.GetEnumerator();
        }
    }
}