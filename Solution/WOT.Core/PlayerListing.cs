using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace WOTStatistics.Core
{
    public class PlayerListing : IEnumerable<KeyValuePair<string, Player>>, IDisposable
    {
        private Dictionary<string, Player> _players = new Dictionary<string, Player>(StringComparer.CurrentCultureIgnoreCase);
        private MessageQueue _message;
        public int Count { get { return _players.Count; } }

        public PlayerListing(MessageQueue message)
        {
            _message = message;
            Refresh();
        }

        public Player GetPlayer(string id)
        {
            Player player;
            id = id.Replace("_", "*");
            if (!_players.TryGetValue(id, out player))
                player = new Player() { PlayerID = "Unknown", PlayerRealm="Unknown", WatchFile = "", Monitor = "No", PreviousFile="1", CurrentFile="0", OnlineURL ="#" };

            return player;
        }

        public void SetPlayer(string id, string playerID, string playerRealm, string watchFile, bool monitor)
        {
            Player player;

            if (!_players.TryGetValue(id.Replace("_","*"), out player))
            {
                player = new Player() { PlayerID = playerID, PlayerRealm = playerRealm, WatchFile = watchFile, Monitor = (monitor == true ? "Yes" : "No"), PreviousFile="1", CurrentFile="0"  };
                _players.Add(id, player);
                _message.Add("Added Player : " + playerID);
            }
            else
            {
                player.PlayerID = playerID;
                player.WatchFile = watchFile;
                player.Monitor = (monitor == true ? "Yes" : "No");
            }


        }

        public void SetPlayer(string id, string previousFile, string currentFile)
        {
            Player player;

            if (!_players.TryGetValue(id, out player))
            {
                _message.Add("Error : Player id not found." + id);
            }
            else
            {
                player.PreviousFile = previousFile;
                player.CurrentFile = currentFile;
            }


        }

        public void SetPlayerOnlineURL(string id, string url)
        {
            Player player;

            if (!_players.TryGetValue(id.Replace("_","*"), out player))
            {
                _message.Add("Error : Player id not found." + id);
            }
            else
            {
                player.OnlineURL = url;
            }


        }

        public void SetPlayerWatchFile(string id, string filePath)
        {
            Player player;

            if (!_players.TryGetValue(id.Replace("_", "*"), out player))
            {
                _message.Add("Error : Player id not found." + id);
            }
            else
            {
                player.WatchFile = filePath;
            }
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetUserFile());

            if (xmlDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                XmlDeclaration dec = (XmlDeclaration)xmlDoc.FirstChild;
                dec.Encoding = "UTF-8";
            }
            
            XmlElement root = xmlDoc.DocumentElement;
   
            XmlNode oldPlayers = root.SelectSingleNode(@"Players");
            root.RemoveChild(oldPlayers);

            XmlElement players = xmlDoc.CreateElement("Players");
            root.AppendChild(players);

            XmlElement playerNode;
            foreach (KeyValuePair<string, Player> player in _players)
            {
                playerNode = xmlDoc.CreateElement("Player");
                playerNode.SetAttribute("ID", player.Value.PlayerID);
                playerNode.SetAttribute("Realm", player.Value.PlayerRealm);
                playerNode.SetAttribute("WatchFile", player.Value.WatchFile);
                playerNode.SetAttribute("Monitor", player.Value.Monitor);
                playerNode.SetAttribute("FileA", player.Value.PreviousFile);
                playerNode.SetAttribute("FileB", player.Value.CurrentFile);
                playerNode.SetAttribute("OnlineURL", player.Value.OnlineURL);
                players.AppendChild(playerNode);
            }

            xmlDoc.Save(WOTHelper.GetUserFile());

            
            
        }

        public void Remove(string id)
        {
            _players.Remove(id.Replace("_","*"));
            Save();
        }

        public void Refresh()
        {
            _players.Clear();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"Players").ChildNodes;
            int i = 0;
            foreach (XmlNode node in nodes)
            {
                string sRealm = string.Empty;

                if (node.Attributes["Realm"] == null)
                {
                    //XmlAttribute oAttr = new XmlAttribute();
                    //oAttr.InnerText = "Realm";
                    //node.Attributes.Append(oAttr);
                    sRealm = "worldoftanks.eu";
                }
                else
                {
                    sRealm = node.Attributes["Realm"].Value;
                }


                _players.Add(node.Attributes["ID"].Value.Replace("_", "*"), new Player() { PlayerID = node.Attributes["ID"].Value,
                                                                                           PlayerRealm = sRealm, 
                                                                                            WatchFile = node.Attributes["WatchFile"].Value, 
                                                                                            Monitor = node.Attributes["Monitor"].Value, 
                                                                                            PreviousFile = node.Attributes["FileA"] == null ? "1" : node.Attributes["FileA"].Value, 
                                                                                            CurrentFile = node.Attributes["FileB"] == null ? "0" : node.Attributes["FileB"].Value,
                                                                                          OnlineURL = (node.Attributes["OnlineURL"] == null) ? "#" : node.Attributes["OnlineURL"].Value == "" ? "#" : node.Attributes["OnlineURL"].Value
                });
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, Player>> GetEnumerator()
        {
            return this._players.GetEnumerator();
        }

        public void Dispose()
        {
            _players.Clear();
        }
    }
}
