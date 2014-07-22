using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Xml;


namespace WOT.Core
{
    public class VersionChecker
    {
        private const string webBase = "http://www.vbaddict.net/";
        readonly static string _versionURL = @"http://www.vbaddict.net/wotstatistics/sync/version.xml";

        // Member variable to store the MemoryStream Data
        private MemoryStream _versionMemoryStream;

        public bool UpdateAvailable { get; set; }
        public string SiteVersion { get; set; }


        public VersionChecker(string productVersion)
        {
            MemoryStream versionMS = GetWebVersion;
            StreamReader sr = new StreamReader(versionMS);
            string versions = sr.ReadToEnd();
            string webVersion = versions.Split('|')[0];
            Version v1 = new Version(webVersion);
            Version v2 = new Version(productVersion);

            switch (v1.CompareTo(v2))
            {
                case 0:
                    UpdateAvailable = false;
                    break;
                case 1:
                    UpdateAvailable = true;
                    break;
                case -1:
                    UpdateAvailable = false;
                    break;
                default:
                    UpdateAvailable = false;
                    break;
            }

            SiteVersion = webVersion;

            try
            {
                string settingFileVersion = versions.Split('|')[1];
                if (settingFileVersion != UserSettings.SettingsFileVersion)
                {
                    DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData(), "settings.xml");
                    Task.Factory.StartNew(() => VerifyImages());                     
                    UserSettings.SettingsFileVersion = settingFileVersion;

                }

               
            }
            catch { }

            try
            {
                string translationFileVersion = versions.Split('|')[2];
                if (translationFileVersion != UserSettings.TranslationFileVersion)
                {
                    DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData(), "translations.xml");
                    UserSettings.TranslationFileVersion = translationFileVersion;
                    Translations.Reload();
                }
            }
            catch { }

            try
            {
                string WN8ExpectedVersion = versions.Split('|')[5];
                if (WN8ExpectedVersion != UserSettings.WN8ExpectedVersion)
                {
                    DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData(), "expected_wn8.xml");
                    UserSettings.WN8ExpectedVersion = WN8ExpectedVersion;
                }
            }
            catch { }

            try
            {
                string scriptFileVersion = versions.Split('|')[3];
                if (scriptFileVersion != UserSettings.ScriptFileVersion)
                {
                    try
                    {
                        DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData() + @"\Scripts", "CustomScript.wss");
                    }
                    catch {}
                    try
                    {
                        DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData() + @"\Scripts", "tooltips.js");
                    }
                    catch {}
                    try
                    {
                        DownloadFile("wotstatistics/sync/", WOTHelper.GetApplicationData() + @"\Scripts", "sorttable.js");
                    }
                    catch {}
                    UserSettings.ScriptFileVersion = scriptFileVersion;
                    ScriptWrapper.Initialise(WOTHelper.GetCustomScript());
                }
            }
            catch { ScriptWrapper.Initialise(WOTHelper.GetCustomScript()); }
        }

        // Public MemoryStream property containing PDF Data

        private void VerifyImages()
        {
            string tanksWebPath = "wotstatistics/sync/images/tanks/";
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetSettingsFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"Tanks").ChildNodes;

            foreach (XmlNode node in nodes)
            {
                string tankID = String.Format("{0}_{1}", node.Attributes["Country"].Value, node.Attributes["Code"].Value);
                bool updatePicture = false;
                try
                {
                    updatePicture = node.Attributes["Update"] == null ? false : bool.Parse(node.Attributes["Update"].Value);
                }
                catch 
                {
                    //silently handle the error
                    updatePicture = false;
                }
                 
                //first check if we should update
                if (updatePicture)
                {
                    DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + ".png");
                    DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                }
                else
                {
                    //only check the small in appdata it is not there we download both'
                    if (!File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + ".png")))
                    {
                        //ok now check if it exists in the exe path
                        if (!File.Exists(Path.Combine(WOTHelper.GetEXEPath(), "Images", "Tanks", tankID + ".png")))
                        {
                            //Download both the normal and large file
                            DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + ".png");
                            DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                        }

                        //ok now check if the large file exists in the appdata path
                        if (!File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + "_Large.png")))
                        {
                            //ok now check if the large file exists in the exe path
                            if (!File.Exists(Path.Combine(WOTHelper.GetEXEPath(), "Images", "Tanks", tankID + "_Large.png")))
                            {
                                //Download both the large file
                                DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                            }
                        }
                    }
                    else
                    {
                        //Okay we found the small one in appdata now check for big one
                        if (!File.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + "_Large.png")))
                        {
                            //download Large file
                            DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                        }
                    }
                }

            }
        }

        private void DownloadFile(string source, string target, string filename)
        {
            try
            {
                
                using (WebClient client = new WebClient())
                {
                    client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                    client.DownloadFile(webBase + source + filename, Path.Combine(target, filename));
                }
            }
            catch 
            {
                //cannot throw message nobody is listening
                //cleanup and move on
                if (File.Exists(Path.Combine(target, filename)))
                    File.Delete(Path.Combine(target, filename));
           
            }
        }

        private MemoryStream GetWebVersion
        {
            get
            {
                // Check to see if the MemoryStream has already been created,
                // if not, then create memory stream
                if (this._versionMemoryStream == null)
                {
                    
                    using (WebClient client = new WebClient())
                    {
                        client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        client.CachePolicy = new System.Net.Cache.RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                        try
                        {
                            this._versionMemoryStream = new MemoryStream(client.DownloadData(_versionURL));
                        }
                        catch { }
                    }
                }
                return this._versionMemoryStream;
            }
        }


    }

    class DossierRestrict
    {
   
        private const string _URL = @"http://www.vbaddict.net/wotstatistics/misc/";
        private const string _checklistFile = @"checklist.txt";
        private const string _allowedFile = @"allowed.txt";
        private const string _blockedFile = @"blocked.txt";
        private Dictionary<string, int> _servers = new Dictionary<string, int>();


        public DossierRestrict(string playerName)
        {
            try
            {
                //MemoryStream checkFileStream = GetWebFile(_checklistFile);
                //StreamReader checkFileReader = new StreamReader(checkFileStream);
                //MemoryStream allowedFileStream = GetWebFile(_allowedFile);
                //StreamReader allowedFileReader = new StreamReader(allowedFileStream);
                //MemoryStream blockedFileStream = GetWebFile(_blockedFile);
                //StreamReader blockedFileReader = new StreamReader(blockedFileStream);

                //string serverlisting = checkFileReader.ReadToEnd();
                //List<string> allow = allowedFileReader.ReadToEnd().Split(';').ToList();
                //List<string> blocked = blockedFileReader.ReadToEnd().Split(';').ToList();


                //if (!string.IsNullOrEmpty(serverlisting))
                //{
                //    foreach (string keyValuePair in serverlisting.Split(';'))
                //    {
                //        string[] arrayKeyValuePair = keyValuePair.Split('|');
                //        _servers.Add(arrayKeyValuePair[0], int.Parse(arrayKeyValuePair[1]));
                //    }
                //}

                //AllowPlayer = allow.Contains(playerName);
                //BlockPlayer = blocked.Contains(playerName);

                AllowPlayer = true;
                BlockPlayer = false;
            }
            catch 
            {
                AllowPlayer = true;
                BlockPlayer = false;

            }

        }

        public bool AllowPlayer { get; set; }
        public bool BlockPlayer { get; set; }
        public bool AllowServerAccess(string serverName, int dossierVersion)
        {
            int value = 0;
            if (!_servers.TryGetValue(serverName, out value))
                value = 99999;

            if (dossierVersion > value && !AllowPlayer)
                return false;
            else
                return true;
        }

     
        private MemoryStream GetWebFile(string file)
        {
            MemoryStream versionMemoryStream = null;
                // Check to see if the MemoryStream has already been created,
                // if not, then create memory stream
            if (versionMemoryStream == null)
                {
                    
                    using (WebClient client = new WebClient())
                    {
                        client.Proxy.Credentials = CredentialCache.DefaultCredentials;
                        client.CachePolicy = new System.Net.Cache.RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                        try
                        {
                            versionMemoryStream = new MemoryStream(client.DownloadData(_URL + file));
                        }
                        catch (Exception ex) { throw ex; }
                    }
                }
            return versionMemoryStream;
            
        }

              
    }
}
