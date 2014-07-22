using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace WOTStatistics.Core
{
    public class GraphsSettings : IEnumerable<KeyValuePair<string,GraphFields>>
        {
        private readonly Dictionary<string, GraphFields> _graphCollection = new Dictionary<string, GraphFields>();
            private MessageQueue _message;
            public GraphsSettings(MessageQueue message)
            {
                _message = message;
                ReadGraphSettings();
            }

            private void ReadGraphSettings()
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(WOTHelper.GetUserFile());
                XmlElement root = xmlDoc.DocumentElement;

                if (root.SelectSingleNode(@"Graphs") != null)
                {
                    XmlNodeList nodes = root.SelectSingleNode(@"Graphs").ChildNodes;
                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            if (!_graphCollection.ContainsKey(node.Attributes["Name"].Value))
                                _graphCollection.Add(node.Attributes["Name"].Value, new GraphFields()
                                {
                                    Name = node.Attributes["Name"].Value,
                                    Caption = node.Attributes["Caption"].Value,
                                    DataField = node.Attributes["DataField"].Value,
                                    InnerText = node.InnerText,
                                    Period = int.Parse(node.Attributes["Period"].Value),
                                    Type = node.Attributes["Type"].Value,
                                    StatsBase = node.Attributes["StatsBase"].Value
                                });
                        }
                    }
                }
            }

            public void Save(GraphFields fields)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(WOTHelper.GetUserFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList oldCustomGroups = root.SelectNodes("Graphs/Graph[@Name='" + fields.Name + "']");
                XmlNode customGroups = root.SelectSingleNode("Graphs");
                XmlElement customGroup = null;
                if (oldCustomGroups.Count != 0)
                {
                    foreach (XmlNode item in oldCustomGroups)
                    {
                        customGroup = (XmlElement)item;
                    }

                }
                else
                {
                    customGroup = xmlDoc.CreateElement("Graph");
                }

                customGroup.SetAttribute("Name", fields.Name.Replace(" ", "_"));
                customGroup.SetAttribute("Caption", fields.Caption);
                customGroup.SetAttribute("StatsBase", fields.StatsBase);
                customGroup.SetAttribute("Type", fields.Type);
                customGroup.SetAttribute("DataField", fields.DataField);
                customGroup.SetAttribute("Period", fields.Period.ToString());
                customGroup.InnerText = fields.InnerText;

                if (customGroups == null)
                {
                    customGroups = xmlDoc.CreateElement("Graphs");
                    root.AppendChild(customGroups);
                }

                if (oldCustomGroups.Count == 0)
                    customGroups.AppendChild(customGroup);

                xmlDoc.Save(WOTHelper.GetUserFile());
            }

            public void Remove(string name)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(WOTHelper.GetUserFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList oldCustomGroups = root.SelectNodes(string.Format(@"Graphs/Graph[@Name=""{0}""]", name));
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

            public GraphFields FieldValues(string graphName)
            {
                graphName = graphName.Replace(" ", "_");
                GraphFields tValue;
                if (!_graphCollection.TryGetValue(graphName, out tValue))
                    return new GraphFields() {StatsBase = "Overall", Period = 7};
                else
                    return tValue;
            }

            public string InnerText(string graphName)
            {
                graphName = graphName.Replace(" ", "_");
                GraphFields tValue;
                if (!_graphCollection.TryGetValue(graphName, out tValue))
                    return "Unknown";
                else
                    return tValue.InnerText;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<KeyValuePair<string, GraphFields>> GetEnumerator()
            {
                return this._graphCollection.GetEnumerator();
            }
        }
}