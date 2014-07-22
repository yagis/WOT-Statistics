using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace WOTStatistics.Core
{
    public class FTPTools
    {
        public void Upload(string host, string fileName, string userName, string password, MessageQueue message, string playerName)
        {

            try
            {
                FileInfo toUpload = new FileInfo(fileName);

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + host + "/" + toUpload.Name);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    request.Credentials = new NetworkCredential(userName, password);

                Stream ftpStream = request.GetRequestStream();
                //will do this at a later stage
                //byte[] byteFile = WOTStatistics.Core.GZIP.Compress(File.ReadAllBytes(toUpload.FullName));
                byte[] byteFile = File.ReadAllBytes(toUpload.FullName);

                ftpStream.Write(byteFile, 0, byteFile.Length);
                ftpStream.Close();
                // return (String.Format("Info : File submitted to FTP site. [{0}]", playerName));
            }
            catch 
            {
            }
        }

        public string Download(string host, string fileName, string userName, string password, MessageQueue message, string playerName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                WebClient request = new WebClient();

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    request.Credentials = new NetworkCredential(userName, password);

                byte[] fileData = request.DownloadData(@"ftp://" + host + "/" + fi.Name);

                string dropFileName = WOTHelper.GetTempFolder() + @"\" + fi.Name;
                //will dot this at a later stage
                //File.WriteAllBytes(dropFileName, WOTStatistics.Core.GZIP.Decompress(fileData));
                File.WriteAllBytes(dropFileName, fileData);

                message.Add("Info : Retrieved file from FTP. [" + playerName + "]");
                return dropFileName;
            }
            catch (Exception ex)
            {
                message.Add("Error : " + ex.Message);
                return fileName;
            }
        }

        public List<string> DirectoryListing(string host, string userName, string password)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + host);
            
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            List<string> files = new List<string>();
            while (!reader.EndOfStream)
            {
                files.Add(reader.ReadLine());
            }
            reader.Close();
            response.Close();
            return files;
        }
    }

    

    public class VBAddictUpload
    {

        public void UploadToVBAddict(string filePath, string playerID, MessageQueue message)
        {
            try
            {
                FileInfo toUpload = new FileInfo(filePath);
                byte[] data = File.ReadAllBytes(filePath);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.vbaddict.net/xxx.php");
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.Method = "PUT";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Clear();
                request.Referer = toUpload.Name;
                request.UserAgent = "WOTStatistics 2.1.0.0";
                request.ServicePoint.Expect100Continue = false;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = request.GetResponse();
                Stream outStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(outStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                if (responseFromServer.Contains("SUCCESS"))
                {
                    using (PlayerListing pl = new PlayerListing(new MessageQueue()))
                    {
                        if (pl.GetPlayer(playerID).OnlineURL == "#")
                        {
                            pl.SetPlayerOnlineURL(playerID, responseFromServer.Substring(responseFromServer.IndexOf("Link: ") + 6).Replace("SUCCESS", ""));
                            try
                            {
                                pl.Save();
                            }
                            catch
                            {
                            }
                        }
                    }
                }


                response.Close();
            }
            catch (Exception ex)
            {
                message.Add(ex.Message);
            }
            


        }
    }
}
