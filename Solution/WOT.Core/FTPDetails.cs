using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WOTStatistics.Core
{
    public class FTPDetails
    {
        public string Host = "";
        public string UserID = "";
        public string UserPWD = "";
        public bool AllowFTP = false;

        



        MessageQueue _message;
        public FTPDetails(MessageQueue message)
        {
            _message = message;
            GetFTPDetails();
        }

        public FTPDetails()
        {
            _message = null;
            GetFTPDetails();
        }

     

        private void GetFTPDetails()
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"FTPDetails").ChildNodes;
            int i = 0;
            foreach (XmlNode node in nodes)
            {
                Host = node.Attributes["FTPHost"] == null ? "" : node.Attributes["FTPHost"].Value;
                UserID = node.Attributes["FTPUserID"] == null ? "" : node.Attributes["FTPUserID"].Value;
                UserPWD = WOTHelper.Decrypt(node.Attributes["FTPUserPWD"] == null ? "" : node.Attributes["FTPUserPWD"].Value);
                AllowFTP = node.Attributes["ShareFiles"] == null ? false : node.Attributes["ShareFiles"].Value == "Yes" ? true : false;
                i++;
            }
            
        }

        public void Save()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetUserFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNode oldPlayers = root.SelectSingleNode(@"FTPDetails");
            root.RemoveChild(oldPlayers);

            XmlElement players = xmlDoc.CreateElement("FTPDetails");
            root.AppendChild(players);

            XmlElement playerNode;
                playerNode = xmlDoc.CreateElement("FTP");
                playerNode.SetAttribute("FTPHost", Host);
                playerNode.SetAttribute("FTPUserID", UserID);
                playerNode.SetAttribute("FTPUserPWD", WOTHelper.Encrypt(UserPWD));
                playerNode.SetAttribute("ShareFiles", AllowFTP == true ? "Yes" : "No");
                players.AppendChild(playerNode);
           

            xmlDoc.Save(WOTHelper.GetUserFile());

        }
    }
}
