using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using Ionic.Zip;
using WOTStatistics.Core;
using WOTStatistics.SQLite;
using System.Linq;
using System.Data;

namespace WOT.Stats
{


    public class OnlineUpdate : IDisposable
    {

        private static Dictionary<string, string> _onlineVersions = new Dictionary<string, string>();
        private static Dictionary<string, string> _localVersions = new Dictionary<string, string>();
        private static List<string> _missingTanks = new List<string>();


        ManualResetEvent _resetEvents;
        BackgroundWorker _worker;
        DoWorkEventArgs _doWorkEvents;

        public OnlineUpdate(ManualResetEvent resetEvents, BackgroundWorker worker, DoWorkEventArgs doWorkEvents)
        {
            _worker = worker;
            _resetEvents = resetEvents;
            _doWorkEvents = doWorkEvents;
        }

        public void GetUpdateInfo(Dictionary<int, string> downloadURLS, UpdateGUIProgressBar updStatusBar, UPdateGUIListView updListView, DataTable oComponents)
        {
            _onlineVersions.Clear();
            _localVersions.Clear();
            _missingTanks.Clear();

            //1.) Check to see if the folders exsist in appdata.
            WOTStatistics.Core.WOTHelper.CreateAppDataFolder();

            WOTStatistics.Core.WOTHelper.CleanUpLog();

            //2.) get the settings file from the webserver, keep it in memory.


            try
            {
                bool errorHit = false;
                bool needNewVersion = false;
                foreach (DataRow oRow in oComponents.Rows)
                {
                    _resetEvents.WaitOne();

                    if (_worker.CancellationPending)
                    {
                        _doWorkEvents.Cancel = true;
                        return;
                    }

                    updListView(string.Empty, string.Empty, string.Empty);
                    string sShort = oRow["Short"] as string;
                    string sVersionLocal = oRow["Short"] as string;
                    switch (sShort)
                    {
                        case "CheckVersion":
                            updListView(sShort, "Status", "Connecting...");
                            try
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                             
                                try
                                {
                                    byte[] versionMS = webdata.DownloadFromWeb(downloadURLS[1], "wotstatistics/sync/version.xml", updStatusBar);
                                    File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "version.xml"), versionMS);
                                    updListView(sShort, "Status", "Comparing");
                                                               
                                    xmlDoc.XmlResolver = null;
                            
                                    xmlDoc.Load(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "version.xml"));
                              
                                    XmlElement root = xmlDoc.DocumentElement;
                              

                                }
                                catch (Exception ex)
                                {
                                    WOTHelper.AddToLog(sShort + " CV: " + ex.Message);
                                    //versions = new string[5];
                                    updListView(sShort, "Status", "Cannot connect.");
                                    return;
                                }

                                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                                {
                                    updListView(node.Name, "VersionServer", node.InnerText);
                                    _onlineVersions.Add(node.Name, node.InnerText);

                                    DataRow[] oDataRows = oComponents.Select("Short='" + node.Name + "'");
                                    if (oDataRows.Length > 0)
                                    {
                                        DataRow oDataRow = oDataRows[0];
                                        _localVersions.Add(node.Name, oDataRow["VersionLocal"] as string);
                                    }

                                }
                                updListView(sShort, "Status",  "Success");

                             

                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status", "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "AppVersion":
                            try
                            {


                                if (!AppVersionTest(_localVersions[sShort], _onlineVersions[sShort]))
                                {
                                    UserSettings.NewVersionNotify = true;
                                    UserSettings.NewAppVersion = _onlineVersions[sShort];
                                    updListView(sShort, "Status", "Outdated");
                                    needNewVersion = true;
                                    string sMessage = string.Format("WOT Statistics v{0} is available\r\n Download it from http://www.vbaddict.net/wotstatistics", UserSettings.NewAppVersion);

                                    DevExpress.XtraEditors.XtraMessageBox.Show(sMessage, "New Version", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                                }
                                else
                                {
                                    updListView(sShort, "Status", "Current");
                                }

                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                               updListView(sShort, "Status", "Error occurred!");
                                errorHit = true;
                            }

                            break;
                        case "ReleaseNotes":
                            try
                            {

                                string sVersion = "ReleaseNote_" + WOTStatistics.Core.UserSettings.AppVersion.Replace(".", string.Empty) + ".htm";
                                string sFile = Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(),  sVersion);
                                if (WOTStatistics.Core.UserSettings.AppVersion != WOTStatistics.Core.UserSettings.LastReleaseNotes | !System.IO.File.Exists(sFile))
                                {
                                    updListView(sShort, "Status", "Downloading");
                                    

                                    byte[] releaseNote = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/releasenotes/", sVersion, updStatusBar);
                                    File.WriteAllBytes(sFile, releaseNote);
                                    updListView(sShort, "Status", "Updated");
                                    
                                }
                                else
                                {
                                    updListView(sShort, "Status", "Current");
                                }

                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status", "Error occurred!");
                                errorHit = true;
                            }

                            break;
                        case "SettingsFileVersion":
                            updListView(sShort, "Status", "Verifing Settings File");

                            try
                            {
                              
                                if (int.Parse(_localVersions[sShort]) < int.Parse(_onlineVersions[sShort])  || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "settings.xml")))
                                {
                                    updListView(sShort, "Status", "Downloading");

                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                            byte[] settingsfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/", "settings.xml", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "settings.xml"), settingsfile);
                                            WOTStatistics.Core.UserSettings.SettingsFileVersion = _onlineVersions[sShort];
                                           updListView(sShort, "Status", "Updated");
                                        }
                                        else
                                        {
                                          updListView(sShort, "Status", "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status", "Settings File Not Found.");
                                    }
                                }
                                else
                                {
                                   updListView(sShort, "Status", "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                               updListView(sShort, "Status", "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "Images":
                            updListView(sShort, "Status", "Verifing Images");


                            try
                            {
                                VerifyImages();
                                if (_missingTanks.Count > 0)
                                {
                                    int totalImages = _missingTanks.Count;
                                    for (int i = 0; i < _missingTanks.Count; i++)
                                    {
                                        _resetEvents.WaitOne();
                                        if (_worker.CancellationPending)
                                        {
                                            _doWorkEvents.Cancel = true;
                                            return;
                                        }
                                        updListView(sShort, "Status", "Downloading " + (i + 1) + " of " + totalImages);
                                        try
                                        {
                                            WOTHelper.AddToLog("Downloading Tank Icon " + _missingTanks[i]);
                                            byte[] imageFile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/images/tanks/", _missingTanks[i], updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "images", "tanks", _missingTanks[i]), imageFile);
                                        }
                                        catch (Exception ex)
                                        {
                                            WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                           updListView(sShort, "Status", _missingTanks[i] + " File Not Found.");
                                        }
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status",  "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "TranslationFileVersion":
                            updListView(sShort, "Status",  "Verifing Translation File");

                            try
                            {
                                if (int.Parse(_localVersions[sShort]) < int.Parse(_onlineVersions[sShort]) || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "translations.xml")))
                                {
                                    updListView(sShort, "Status",  "Downloading");
                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                            byte[] settingsfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/", "translations.xml", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "translations.xml"), settingsfile);
                                            WOTStatistics.Core.UserSettings.TranslationFileVersion = _onlineVersions[sShort];
                                            updListView(sShort, "Status",  "Updated");
                                        }
                                        else
                                        {
                                            updListView(sShort, "Status",  "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status",  "Translations File Not Found.");
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status",  "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "ActiveDossierUploaderVersion":

                            if (!WOTStatistics.Core.UserSettings.AllowvBAddictUpload)
                            {
                                updListView(sShort, "Status", "Disabled - Check your Setup");
                                break;
                            }

                            updListView(sShort, "Status", "Getting ActiveDossierUploader");
                                //WOTStatistics.Core.WOTHelper.PrintDictionary(_localVersions);
                                //WOTStatistics.Core.WOTHelper.PrintDictionary(_onlineVersions);

                            try
                            {
                                if (!AppVersionTest(_localVersions[sShort], _onlineVersions[sShort]) || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "ActiveDossierUploader.exe")))
                                {
                                    updListView(sShort, "Status", "Downloading");
                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                            WOTHelper.AddToLog(downloadURLS[1] + "adu/" +  "ActiveDossierUploader.exe");
                                            byte[] settingsfile = webdata.DownloadFromWeb(downloadURLS[1] + "adu/", "ActiveDossierUploader.exe", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "ActiveDossierUploader.exe"), settingsfile);
                                            WOTStatistics.Core.UserSettings.ActiveDossierUploaderVersion = _onlineVersions[sShort];
                                            updListView(sShort, "Status", "Updated");
                                        }
                                        else
                                        {
                                            updListView(sShort, "Status", "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status", "ActiveDossierUploader.exe not found.");
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status", "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status", "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "WN8ExpectedVersion":
                            updListView(sShort, "Status",  "Getting WN8 Expected Tank Values");

                            try
                            {


                                if (int.Parse(_localVersions[sShort]) < int.Parse(_onlineVersions[sShort]) || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "expected_wn8.xml")))
                                {
                                    updListView(sShort, "Status",  "Downloading");
                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                            byte[] settingsfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/", "expected_wn8.xml", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "expected_wn8.xml"), settingsfile);
                                            WOTStatistics.Core.UserSettings.WN8ExpectedVersion = _onlineVersions[sShort];
                                            updListView(sShort, "Status",  "Updated");
                                        }
                                        else
                                        {
                                            updListView(sShort, "Status",  "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status",  "WN8 Expected Tank Values File Not Found.");
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status",  "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "ScriptFileVersion":
                            updListView(sShort, "Status",  "Verifing Script File");

                            try
                            {

                                if (int.Parse(_localVersions[sShort]) < int.Parse(_onlineVersions[sShort])
                                     || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "scripts", "sorttable.js"))
                                     || !System.IO.File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "scripts", "tooltips.js"))
                                    )
                                {
                                    updListView(sShort, "Status",  "Downloading 1 of 2");
                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                                                        
                                            updListView(sShort, "Status",  "Downloading 1 of 2");
                                             byte[] scriptfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/scripts/", "sorttable.js", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "scripts", "sorttable.js"), scriptfile);

                                            scriptfile = null;
                                            updListView(sShort, "Status",  "Downloading 2 of 2");
                                            scriptfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/scripts/", "tooltips.js", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "scripts", "tooltips.js"), scriptfile);

                                            WOTStatistics.Core.UserSettings.ScriptFileVersion = _onlineVersions[sShort];
                                            updListView(sShort, "Status",  "Updated");
                                        }
                                        else
                                        {
                                            updListView(sShort, "Status",  "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status",  "Scripting Files Not Found.");
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status",  "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }
                            break;
                        case "DossierVersion":

                            updListView(sShort, "Status",  "Verifing Dossier File");
                            try
                            {

                                if (int.Parse(_localVersions[sShort]) < int.Parse(_onlineVersions[sShort]))
                                {
                                    updListView(sShort, "Status",  "Downloading");
                                    try
                                    {
                                        if (!needNewVersion)
                                        {
                                            byte[] scriptfile = webdata.DownloadFromWeb(downloadURLS[1] + "wotstatistics/sync/", "dossier.zip", updStatusBar);
                                            File.WriteAllBytes(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "temp", "dossier.zip"), scriptfile);

                                            updListView(sShort, "Status",  "Unpacking");
                                            unZip(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "temp", "dossier.zip"), Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "python"));

                                            updListView(sShort, "Status",  "Removing temp files");
                                            File.Delete(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "temp", "dossier.zip"));

                                            WOTStatistics.Core.UserSettings.DossierVersion = _onlineVersions[sShort];
                                            updListView(sShort, "Status",  "Updated");
                                        }
                                        else
                                        {
                                            updListView(sShort, "Status",  "New version required.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                        updListView(sShort, "Status",  "Dossier Files Not Found.");
                                    }
                                }
                                else
                                {
                                    updListView(sShort, "Status",  "Current");
                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }
                            break;
    
                        case "DBMain":
                            try
                            {
                                int x = 1;
                                using (PlayerListing players = new PlayerListing(new MessageQueue()))
                                {

                                    foreach (KeyValuePair<string, Player> player in players)
                                    {

                                        if (!Directory.Exists(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + player.Value.PlayerID, "LastBattle")))
                                        {
                                            Directory.CreateDirectory(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + player.Value.PlayerID, "LastBattle"));

                                        }
                                        updListView(sShort, "Status",  "DB checks for player : " + player.Value.PlayerID);
                                        string storePath = Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + player.Value.PlayerID, "LastBattle", "WOTSStore.db");
                                        if ((File.Exists(storePath)))
                                        {
                                            if (new FileInfo(storePath).Length == 0)
                                            {
                                                File.Delete(storePath);
                                            }
                                        }

                                        if (!(File.Exists(storePath)))
                                        {
                                            updListView(sShort, "Status",  "Creating DB for player : " + player.Value.PlayerID);
                                            DatabaseSanityChecker.Create(storePath);
                                            RecentBattles rb = new RecentBattles(player.Value.PlayerID, new MessageQueue(), true);

                                        }
                                        else
                                        {
                                            updListView(sShort, "Status",  "DB sanity check for player : " + player.Value.PlayerID);
                                            DatabaseSanityChecker.AlterDB(storePath);


                                        }

                                        if (!UserSettings.ConvertFlatFilesToDB.Split(';').Contains(player.Value.PlayerID))
                                        {
                                            string folderName = String.Format(@"{0}\{1}{2}", WOTHelper.GetApplicationData(), "HIST_", player.Value.PlayerID);
                                            updListView(sShort, "Status",  "Converting History Files : " + player.Value.PlayerID);
                                            ConvertFlatFileToDB.DoData(folderName, storePath, updStatusBar);

                                            UserSettings.ConvertFlatFilesToDB = UserSettings.ConvertFlatFilesToDB + player.Value.PlayerID + ";";
                                        }

                                        DatabaseSanityChecker.CorrectBrokenData(storePath);

                                        Dictionary<string, Tuple<int, int, string>> tanksInfo = new Dictionary<string, Tuple<int, int, string>>();

                                        using (TankDescriptions td = new TankDescriptions(new MessageQueue()))
                                        {
                                            foreach (var item in td)
                                            {
                                                int tankType;
                                                switch (item.Value.TankType)
                                                {
                                                    case "LT":
                                                        tankType = 1;
                                                        break;
                                                    case "MT":
                                                        tankType = 2;
                                                        break;
                                                    case "HT":
                                                        tankType = 3;
                                                        break;
                                                    case "TD":
                                                        tankType = 4;
                                                        break;
                                                    case "SPG":
                                                        tankType = 5;
                                                        break;
                                                    default:
                                                        tankType = 1;
                                                        break;
                                                }

                                                tanksInfo.Add(String.Format("{0}_{1}", item.Key.CountryID, item.Key.TankID), new Tuple<int, int, string>(tankType, item.Value.Tier, item.Value.Description));
                                            }

                                            DatabaseSanityChecker.UpdateLegacyData(storePath, tanksInfo);
                                        }

//                                        updListView(sShort, "Status", "Calculating...");
//                                        WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();


//                                        using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(player.Value.PlayerID), true))
//                                        {
//                                            WOTStatistics.Core.UserSettings.RatingVersion = _onlineVersions["RatingVersion"];
//                                            string sqlConvert = @"select * from File_Battles FB
//                                                            inner join File_TankDetails T1 on T1.cmID = FB.bpParentID
//                                                            where bpDamageDealt>0 and (bpRatingExpDamage=0 or bpRatingExpDamage is null OR bpRatingExpDamage = '')";
//                                            //WOTHelper.AddToLog(sqlConvert);
//                                            int iRow = 0;

//                                            using (DataTable dt = db.GetDataTable(sqlConvert))
//                                            {
//                                                if (dt.Rows.Count == 0)
//                                                {
//                                                    continue;
//                                                }


//                                                updListView(sShort, "Status", "Calculating expected values of " + dt.Rows.Count + " items for " + player.Value.PlayerID);
//                                                WOTHelper.AddToLog("Calculating expected values of " + dt.Rows.Count + " items for " + player.Value.PlayerID);
//                                                foreach (DataRow item in dt.Rows)
//                                                {
//                                                    updStatusBar(new Tuple<int, int>(iRow, dt.Rows.Count), 2);
//                                                    iRow++;

//                                                    WN8ExpValue WN8ExpectedTank = null;
//                                                    try
//                                                    {
//                                                        WN8ExpectedTank = WN8ExpectedTankList.GetByTankID(item.GetSafeInt("cmCountryID"), item.GetSafeInt("cmTankID"));
//                                                    }
//                                                    catch (Exception ex)
//                                                    {
//                                                        WOTHelper.AddToLog(ex.Message);
//                                                    }


//                                                    double rDamage = item.GetSafeInt("bpDamageDealt") / (WN8ExpectedTank.expDamage * item.GetSafeInt("bpBattleCount"));
//                                                    double rFrag = item.GetSafeInt("bpFrags") / (WN8ExpectedTank.expFrag * item.GetSafeInt("bpBattleCount"));
//                                                    double rSpot = item.GetSafeInt("bpSpotted") / (WN8ExpectedTank.expSpot * item.GetSafeInt("bpBattleCount"));
//                                                    double rDef = item.GetSafeInt("bpDefencePoints") / (WN8ExpectedTank.expDefense * item.GetSafeInt("bpBattleCount"));
//                                                    double rWins = item.GetSafeInt("bpWins") / item.GetSafeInt("bpBattleCount");
//                                                    double rWin = rWins / (WN8ExpectedTank.expWin * item.GetSafeInt("bpBattleCount"));

//                                                    string sUpdate = string.Format(@"update File_Battles set 
//                                                                                    bpRatingExpDamage='{0}',
//                                                                                    bpRatingExpFrag='{1}',
//                                                                                    bpRatingExpSpot='{2}',
//                                                                                    bpRatingExpDef='{3}',
//                                                                                    bpRatingExpWin='{4}'
//                                                                                    WHERE bpParentID={5}",
//                                                                                    rDamage, 
//                                                                                    rFrag, 
//                                                                                    rSpot, 
//                                                                                    rDef,
//                                                                                    rWin,
//                                                                                    item.GetSafeInt("bpParentID")
//                                                    );
//                                                     //WOTHelper.AddToLog("Update: " + sUpdate);
//                                                    db.ExecuteNonQuery(sUpdate);

//                                                }
//                                            }
//                                        }
                                       // ratingStruct.damageAssistedTracks = item.GetSafeInt("bpDamageAssistedTracks");
                                        //                                                            ratingStruct.damageDealt = item.GetSafeInt("bpDamageDealt");
                                        //                                                            ratingStruct.frags = item.GetSafeInt("bpFrags");
                                        //                                                            ratingStruct.spotted = item.GetSafeInt("bpSpotted");
                                        //                                                            ratingStruct.wins = item.GetSafeInt("bpWins");



                                        //WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
                                        //RatingStructure ratingStructTest = new RatingStructure();
                                        //ratingStructTest.WN8ExpectedTankList = WN8ExpectedTankList;
                                        //ratingStructTest.countryID = 2;
                                        //ratingStructTest.tankID = 39;
                                        //ratingStructTest.tier = 6;
                                        //ratingStructTest.globalTier = 6;

                                        //ratingStructTest.singleTank = true;

                                        //ratingStructTest.battlesCount = 1;
                                        //ratingStructTest.battlesCount8_8 = 0;
                                        //ratingStructTest.capturePoints = 0;
                                        //ratingStructTest.defencePoints = 0;

                                        //ratingStructTest.damageAssistedRadio = 0;
                                        //ratingStructTest.damageAssistedTracks = 0;
                                        //ratingStructTest.damageDealt = 338;
                                        //ratingStructTest.frags = 0;
                                        //ratingStructTest.spotted = 4;
                                        //ratingStructTest.wins = 1;
                                        
                              

                                        //WOTHelper.AddToLog("ratingStruct.winRate" + ratingStruct.winRate);

                                       // //WOTStatistics.Core.Ratings.printRatingStruct(ratingStruct);

                                        //WOTStatistics.Core.Ratings.RatingStorage rWN7Test = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStructTest);


                                       // WOTHelper.AddToLog("RWN7: " + rWN7Test.Value);

                                       // //if (ratingStruct.countryID == 0 & ratingStruct.tankID == 32)
                                       // //{
                                        //WOTHelper.AddToLog("WNx : " + rWN7Test.Value);
                                        //WOTHelper.AddToLog("TIER: " + ratingStructTest.tier);
                                        //WOTHelper.AddToLog("WRAT: " + ratingStructTest.winRate);
                                        //WOTHelper.AddToLog("TDMG: " + rWN7Test.rDAMAGE);
                                        //WOTHelper.AddToLog("TFRG: " + rWN7Test.rFRAG);
                                        //WOTHelper.AddToLog("TSPT: " + rWN7Test.rSPOT);
                                        //WOTHelper.AddToLog("TDEF: " + rWN7Test.rDEF);
                                        //WOTHelper.AddToLog("TWIN: " + rWN7Test.rWIN);
                                        //WOTHelper.AddToLog("TMAL: " + rWN7Test.rTIERMALUS);

                                       //// }
//                                                using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(player.Value.PlayerID), true))
//                                                {

////                                                    string sReset = string.Format(@"update File_Battles set 
////                                                                                bpRatingVersion=0, bpRatingEff=0, bpRatingBR=0, bpRatingWN7=0, bpRatingWN8=0,
////                                                                                bpRatingEffWeight=0, bpRatingBRWeight=0, bpRatingWN7Weight=0, bpRatingWN8Weight=0");
////                                                    db.ExecuteNonQuery(sReset);
//                                                    WOTStatistics.Core.UserSettings.RatingVersion = _onlineVersions["RatingVersion"];
//                                                    string sqlConvert = @"select * from File_Battles FB
//                                                        inner join File_TankDetails T1 on T1.cmID = FB.bpParentID
//                                                        where bpRatingVersion <> " + WOTStatistics.Core.UserSettings.RatingVersion + " OR (bpBattleCount>0 and (bpRatingWN8Weight=0 or bpRatingWN8Weight is null OR bpRatingWN8Weight = ''))";
//                                                    //WOTHelper.AddToLog(sqlConvert);
//                                                    int iRow = 0;

//                                                    using (DataTable dt = db.GetDataTable(sqlConvert))
//                                                    {
//                                                        if (dt.Rows.Count == 0)
//                                                        {
//                                                            continue;
//                                                        }
                                      

//                                                        updListView(sShort, "Status", "Calculating ratings of " + dt.Rows.Count + " items for " + player.Value.PlayerID);
//                                                        WOTHelper.AddToLog("Calculating ratings of " + dt.Rows.Count + " items for " + player.Value.PlayerID);
//                                                        foreach (DataRow item in dt.Rows)
//                                                        {
//                                                            updStatusBar(new Tuple<int, int>(iRow, dt.Rows.Count), 2);
//                                                            iRow++;
//                                                            //WOTHelper.AddToLog("Damage: " + item.GetSafeInt("bpDamageDealt"));


//                                                            RatingStructure ratingStruct = new RatingStructure();
//                                                            ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
//                                                            ratingStruct.countryID = item.GetSafeInt("cmCountryID");
//                                                            ratingStruct.tankID = item.GetSafeInt("cmTankID");
//                                                            ratingStruct.tier = item.GetSafeInt("cmTier");
//                                                            ratingStruct.globalTier = item.GetSafeInt("cmTier");

//                                                            ratingStruct.singleTank = true;

//                                                            ratingStruct.battlesCount = item.GetSafeInt("bpBattleCount");
//                                                            ratingStruct.battlesCount8_8 = item.GetSafeInt("bpBattlesBefore8_8");
//                                                            ratingStruct.capturePoints = item.GetSafeInt("bpCapturePoints");
//                                                            ratingStruct.defencePoints = item.GetSafeInt("bpDefencePoints");

//                                                            ratingStruct.damageAssistedRadio = item.GetSafeInt("bpDamageAssistedRadio");
//                                                            ratingStruct.damageAssistedTracks = item.GetSafeInt("bpDamageAssistedTracks");
//                                                            ratingStruct.damageDealt = item.GetSafeInt("bpDamageDealt");
//                                                            ratingStruct.frags = item.GetSafeInt("bpFrags");
//                                                            ratingStruct.spotted = item.GetSafeInt("bpSpotted");
//                                                            ratingStruct.wins = item.GetSafeInt("bpWins");
//                                                            //ratingStruct.globalWinRate = ratingStruct.winRate;

////WOTHelper.AddToLog("ratingStruct.winRate" + ratingStruct.winRate);

//                                                            //WOTStatistics.Core.Ratings.printRatingStruct(ratingStruct);

//                                                            WOTStatistics.Core.Ratings.RatingStorage rEff = WOTStatistics.Core.Ratings.GetRatingEff(ratingStruct);
//                                                            WOTStatistics.Core.Ratings.RatingStorage rBR = WOTStatistics.Core.Ratings.GetRatingBR(ratingStruct);
//                                                            WOTStatistics.Core.Ratings.RatingStorage rWN7 = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct);
//                                                            WOTStatistics.Core.Ratings.RatingStorage rWN8 = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct);
//                                                            //WOTHelper.AddToLog("BR: " + rBR.Value);
//                                                            //WOTHelper.AddToLog("WN7: " + rWN7.Value);
//                                                            //WOTHelper.AddToLog("WN8: " + rWN8.Value);
//                                                            string sUpdate = string.Format(@"update File_Battles set 
//                                                                bpRatingEff={0}, bpRatingBR={1}, bpRatingWN7={2}, bpRatingWN8={3},
//                                                                bpRatingEffWeight={4}, bpRatingBRWeight={5}, bpRatingWN7Weight={6}, bpRatingWN8Weight={7},
//                                                                bpRatingVersion= {8}
//                                                                WHERE bpParentID={9}",
//                                                                Convert.ToInt32(rEff.Value), Convert.ToInt32(rBR.Value), Convert.ToInt32(rWN7.Value), Convert.ToInt32(rWN8.Value),
//                                                                Convert.ToInt32(rEff.Weight), Convert.ToInt32(rBR.Weight), Convert.ToInt32(rWN7.Weight), rWN8.Weight,
//                                                                WOTStatistics.Core.UserSettings.RatingVersion,
//                                                                item.GetSafeInt("bpParentID")
//                                                            );
//                                                           // WOTHelper.AddToLog("Update: " + sUpdate);
//                                                            db.ExecuteNonQuery(sUpdate);

//                                                        }
//                                                    }
//                                                }
                                   
                                


                                        updListView(sShort, "Status",  "Compressing db for player : " + player.Value.PlayerID);
                                        DatabaseSanityChecker.ShrinkDB(storePath);

                                        updListView(sShort, "Status",  "Re-Indexing db for player : " + player.Value.PlayerID);
                                        DatabaseSanityChecker.ReIndexDB(storePath);

                                        updStatusBar(new Tuple<int, int>(x, players.Count), 2);
                                        x++;
                                    }
                                    updListView(sShort, "Status",  "Completed");

                                }
                            }
                            catch (Exception ex)
                            {
                                WOTHelper.AddToLog(sShort + ": " + ex.Message);
                                updListView(sShort, "Status",  "Error occurred!");
                                errorHit = true;
                            }

                            break;
                        default:
                            break;
                    }

                    if (!errorHit)
                        updListView(string.Empty, string.Empty, string.Empty);

                    errorHit = false;

                }
            }
            catch (Exception)
            {
                updListView(string.Empty, string.Empty, string.Empty);
            }
        }



        private void VerifyImages()
        {

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTStatistics.Core.WOTHelper.GetSettingsFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"Tanks").ChildNodes;

            foreach (XmlNode node in nodes)
            {
                _resetEvents.WaitOne();
                if (_worker.CancellationPending)
                {
                    _doWorkEvents.Cancel = true;
                    return;
                }

                if (bool.Parse(node.Attributes["Active"].Value))
                {
                    string tankID = String.Format("{0}_{1}", node.Attributes["Country"].Value, node.Attributes["Code"].Value);
                    bool updatePicture = false;
                    try
                    {
                        updatePicture = node.Attributes["Update"] == null ? false : bool.Parse(node.Attributes["Update"].Value);
                    }
                    catch (Exception ex)
                    {
                        WOTHelper.AddToLog(ex.Message);
                        //silently handle the error
                        updatePicture = false;
                    }

                    //first check if we should update
                    if (updatePicture)
                    {
                        AddMissingImage(tankID + ".png");
                        AddMissingImage(tankID + "_Large.png");
                        //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + ".png");
                        //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                    }
                    else
                    {
                        //only check the small in appdata it is not there we download both'
                        if (!File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + ".png")))
                        {
                            //ok now check if it exists in the exe path
                            if (!File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetEXEPath(), "Images", "Tanks", tankID + ".png")))
                            {
                                //Download both the normal and large file
                                AddMissingImage(tankID + ".png");
                                AddMissingImage(tankID + "_Large.png");
                                //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + ".png");
                                //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                            }

                            //ok now check if the large file exists in the appdata path
                            if (!File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + "_Large.png")))
                            {
                                //ok now check if the large file exists in the exe path
                                if (!File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetEXEPath(), "Images", "Tanks", tankID + "_Large.png")))
                                {
                                    //Download both the large file
                                    AddMissingImage(tankID + "_Large.png");
                                    //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                                }
                            }
                        }
                        else
                        {
                            //Okay we found the small one in appdata now check for big one
                            if (!File.Exists(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), "Images", "Tanks", tankID + "_Large.png")))
                            {
                                //download Large file
                                AddMissingImage(tankID + "_Large.png");
                                //DownloadFile(tanksWebPath, Path.Combine(WOTHelper.GetApplicationData(), "Images", "Tanks"), tankID + "_Large.png");
                            }
                        }
                    }
                }

            }
        }

        private static void AddMissingImage(string fileName)
        {
            if (!_missingTanks.Contains(fileName))
            {
                _missingTanks.Add(fileName);
            }
        }

        private static string VersionDisplay(string type, string v1, string v2)
        {
            if (string.IsNullOrEmpty(v1)) v1 = "0";
            if (string.IsNullOrEmpty(v2)) v2 = "0";

            if (type == "AppVersion")
            {
                Version verOld = new Version(v1);
                Version verNew = new Version(v2);

                switch (verOld.CompareTo(verNew))
                {
                    case 0:
                        return v1 + " = " + v2;
                    case 1:
                        return v1 + " <= " + v2;
                    case -1:
                        return v1 + " => " + v2;
                    default:
                        return v1 + " = " + v2;
                }

            }
            else
                if (int.Parse(v1) > int.Parse(v2))
                    return v1 + " <= " + v2;
                else if (int.Parse(v1) < int.Parse(v2))
                    return v1 + " => " + v2;
                else if (int.Parse(v1) == int.Parse(v2))
                    return v1 + " = " + v2;
                else
                    return v1 + " = " + v2;
        }

        private static bool AppVersionTest(string v1, string v2)
        {
            Version webVersion = new Version(v1);
            Version productVersion = new Version(v2);

            switch (webVersion.CompareTo(productVersion))
            {
                case 0:
                    return true;
                case 1:
                    return true;
                case -1:
                    return false;
                default:
                    return true;
            }
        }


        /// <summary>Download file from the web immediately</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="downloadTo">Folder on the local machine to download the file to</param>
        /// <param name="unzip">Unzip the contents of the file</param>
        /// <returns>Void</returns>
        public static void installUpdateNow(string downloadsURL, string filename, string downloadTo, bool unzip)
        {

            byte[] downloadSuccess = webdata.DownloadFromWeb(downloadsURL, filename, null);

            if (unzip)
            {

                unZip(downloadTo + filename, downloadTo);

            }

        }


        /// <summary>Starts the update application passing across relevant information</summary>
        /// <param name="downloadsURL">URL to download file from</param>
        /// <param name="filename">Name of the file to download</param>
        /// <param name="destinationFolder">Folder on the local machine to download the file to</param>
        /// <param name="processToEnd">Name of the process to end before applying the updates</param>
        /// <param name="postProcess">Name of the process to restart</param>
        /// <param name="startupCommand">Command line to be passed to the process to restart</param>
        /// <param name="updater"></param>
        /// <returns>Void</returns>
        public static void installUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {

            string cmdLn = "";

            cmdLn += "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadsURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcess|" + postProcess;
            cmdLn += "|command|" + @" / " + startupCommand;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updater;
            startInfo.Arguments = cmdLn;
            Process.Start(startInfo);

        }



        private static List<string> populateInfoFromWeb(string versionFile, string resourceDownloadFolder, int line)
        {

            List<string> tempList = new List<string>();
            int ln;

            ln = 0;

            foreach (string strline in File.ReadAllLines(resourceDownloadFolder + versionFile))
            {

                if (ln == line)
                {

                    string[] parts = strline.Split('|');
                    foreach (string part in parts)
                    {

                        tempList.Add(part);

                    }

                    return tempList;

                }

                ln++;
            }


            return null;

        }




        private static bool unZip(string file, string unZipTo)//, bool deleteZipOnCompletion)
        {
            try
            {

                // Specifying Console.Out here causes diagnostic msgs to be sent to the Console
                // In a WinForms or WPF or Web app, you could specify nothing, or an alternate
                // TextWriter to capture diagnostic messages. 

                using (ZipFile zip = ZipFile.Read(file))
                {
                    // This call to ExtractAll() assumes:
                    //   - none of the entries are password-protected.
                    //   - want to extract all entries to current working directory
                    //   - none of the files in the zip already exist in the directory;
                    //     if they do, the method will throw.
                    zip.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    zip.ExtractAll(unZipTo);
                }

                //if (deleteZipOnCompletion) File.Delete(unZipTo + file);

            }
            catch (System.Exception)
            {
                return false;
            }

            return true;

        }

        /// <summary>Updates the update application by renaming prefixed files</summary>
        /// <param name="updaterPrefix">Prefix of files to be renamed</param>
        /// <param name="containingFolder">Folder on the local machine where the prefixed files exist</param>
        /// <returns>Void</returns>
        //public static void updateMe(string updaterPrefix, string containingFolder)
        //{

        //    DirectoryInfo dInfo = new DirectoryInfo(containingFolder);
        //    FileInfo[] updaterFiles = dInfo.GetFiles(updaterPrefix + "*");
        //    int fileCount = updaterFiles.Length;

        //    foreach (FileInfo file in updaterFiles)
        //    {

        //        string newFile = containingFolder + file.Name;
        //        string origFile = containingFolder + @"\" + file.Name.Substring(updaterPrefix.Length, file.Name.Length - updaterPrefix.Length);

        //        if (File.Exists(origFile)) { File.Delete(origFile); }

        //        File.Move(newFile, origFile);

        //    }

        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (_resetEvents != null)
                {
                    _resetEvents.Dispose();
                    _resetEvents = null;
                }
        }
        ~OnlineUpdate()
        {
            Dispose(false);
        }



    }




}

