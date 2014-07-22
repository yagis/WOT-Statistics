using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WOTStatistics.SQLite;
using Newtonsoft.Json.Linq;

namespace WOTStatistics.Core
{

    public delegate void DossierManager_CurrentFileChanged(object sender, EventArgs e);

    public class DossierManager
    {
        private DossierWatcher _dossierWatcher;
        private MessageQueue _messages;
        private string _dossierFilePath;
        private readonly string _playerName;
        private bool _fetchFromFTP;
        //   DossierRestrict _restrictionCheck;



        private readonly string _ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private readonly Dictionary<string, Dictionary<Int32, Int32>> _HistoryFiles = new Dictionary<string, Dictionary<int, Int32>>(StringComparer.CurrentCultureIgnoreCase);
        private const string _HistoryIndicator = "Hist_";

        public event DossierManager_CurrentFileChanged CurrentFileChange;

        public string GetPlayerName { get { return _playerName; } }
        public string GetPlayerID { get { return _playerName.Replace("_", "*"); } }
        public string WatchPath { get { return _dossierFilePath; } }
        public bool FTPFileFetch { get { return _fetchFromFTP; } }
        public bool MonitorStarted { get; private set; }

        protected virtual void OnCurrentFileChange(EventArgs e)
        {
            if (CurrentFileChange != null)
            {
                CurrentFileChange(this, e);
            }
        }



        public DossierManager(string playerName, string watchPath, string fetchFromFTP, MessageQueue messages, System.Windows.Forms.Form owner)
        {
            _messages = messages;

            _playerName = playerName;

            _dossierFilePath = watchPath;
            _fetchFromFTP = fetchFromFTP == "No" ? false : true;
            _dossierWatcher = new DossierWatcher(_dossierFilePath, _playerName, _messages, owner);
            // _restrictionCheck = new DossierRestrict(_playerName);
            _dossierWatcher.Changed += new DossierWatcher_Changed(_dossierWatcher_Changed);
            LoadDossierFiles();
        }

        public void LoadDossierFiles()
        {
            System.Diagnostics.Stopwatch digSW = new Stopwatch();
            digSW.Start();
            _HistoryFiles.Clear();

            string sql = @"select fiID from Files";

            string battleMode = UserSettings.BattleMode;
            if (battleMode != "All")
                sql += " inner join File_TankDetails on fiID = cmFileID inner join File_Battles on cmID = bpParentID ";

            sql += " where fiID <= " + Int32.Parse(DateTime.Now.AddHours(UserSettings.TimeAdjustment).ToString("yyyyMMdd"));

            if (battleMode == "15")
                sql += " and ifnull(bpBattleMode,0) in (15, 0) group by fiID";
            else if (battleMode == "7")
                sql += " and ifnull(bpBattleMode,0) = 7 group by fiID";

            sql += " order by fiID";
            Dictionary<int, int> tempDic = new Dictionary<int, int>();
            using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerName)))
            {
                DataTable dt = db.GetDataTable(sql);
                foreach (DataRow file in dt.Rows)
                {

                    tempDic.Add(Convert.ToInt32(file["fiID"]), Convert.ToInt32(file["fiID"]));
                }
                _HistoryFiles.Add(GetPlayerID, tempDic);
            }
            //DirectoryInfo AppPath = new DirectoryInfo(WOTHelper.GetApplicationData());

            //DirectoryInfo[] subDirs = AppPath.GetDirectories(_HistoryIndicator + "*");
            //int currentFile = Int32.Parse(DateTime.Now.AddHours(UserSettings.TimeAdjustment).ToString("yyyyMMdd"));
            //foreach (DirectoryInfo dir in subDirs)
            //{
            //    if (dir.Name.Replace(_HistoryIndicator, "") == _playerName)
            //    {
            //        FileSystemInfo[] files = dir.GetFileSystemInfos();
            //        Dictionary<int, string> tempDic = new Dictionary<int, string>();
            //        foreach (FileSystemInfo file in files.OrderBy(f => f.Name).Where(w => w.Extension == ".txt"))
            //        {
            //            if (Int32.Parse(file.Name.Replace(file.Extension, "")) <= currentFile)
            //                tempDic.Add(Int32.Parse(file.Name.Replace(file.Extension, "")), file.FullName);
            //        }

            //    }
            //}

            //AppPath = null;
            //subDirs = null;

            digSW.Stop();
             _messages.Add("Diagnostics : Dossier Loading Finished in " + digSW.ElapsedMilliseconds + " miliseconds");
        }

        public void SetValues(string dossierFilePath, bool FetchFromFTP)
        {
            _dossierFilePath = dossierFilePath;
            _fetchFromFTP = FetchFromFTP;
        }

        void _dossierWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (GetPlayerName == WOTHelper.PlayerIdFromDatFile(e.Name))
            {
                if (string.Compare(_dossierFilePath, e.FullPath, true) != 0)
                {
                    using (PlayerListing player = new PlayerListing(_messages))
                    {
                        player.SetPlayerWatchFile(GetPlayerID, e.FullPath);
                        player.Save();
                    }

                    _dossierFilePath = e.FullPath;
                }
                ProcessDossierFile();
            }
        }

        private void ProcessDossierFile()
        {
            System.Diagnostics.Stopwatch digSW = new Stopwatch();
            digSW.Start();
            GC.Collect();
            try
            {
                _messages.Add("Info : Refreshing Dossier File (" + GetPlayerName + ")");

                string tempFile = Guid.NewGuid().ToString();
                FTPDetails ftpDetails = new FTPDetails();
                if (ftpDetails.AllowFTP && _fetchFromFTP == false)
                {
                    try //we only attempting to upload if it fails bad luc k
                    {
                        Task.Factory.StartNew(() => new FTPTools().Upload(ftpDetails.Host, _dossierFilePath, ftpDetails.UserID, ftpDetails.UserPWD, _messages, GetPlayerName));
                    }
                    catch { _messages.Add(String.Format("Error : File failed submitting to FTP site. [{0}]", GetPlayerName)); }
                }

                //if (UserSettings.AllowVBAddictUpload && _fetchFromFTP == false)
                //{
                //    try
                //    {
                //        //disable until we can resolve issue with upload
                //       //Task.Factory.StartNew(() => new VBAddictUpload().UploadToVBAddict(_dossierFilePath, GetPlayerName, _messages));
                //    }
                //    catch {}
                //}

                if (UserSettings.Cloud_Allow)
                {
                    string cloudPath = UserSettings.Cloud_Path;
                    if (Directory.Exists(cloudPath))
                    {
                        FileInfo fi = new FileInfo(_dossierFilePath);
                        if (string.Compare(Path.Combine(cloudPath, fi.Name), _dossierFilePath, true) != 0)
                        {
                            try
                            {
                                File.Copy(_dossierFilePath, Path.Combine(cloudPath, fi.Name), true);
                            }
                            catch (Exception ex)
                            {
                                _messages.Add(String.Format("Error : File failed to copy to sharing folder. [{0}] - {1}", GetPlayerName, ex.Message));
                            }
                        }
                        fi = null;
                    }
                }

                string dossierFilePath = "";
                bool markDSForDelete = false;
                if (ftpDetails.AllowFTP && _fetchFromFTP)
                {
                    try
                    {
                        dossierFilePath = new FTPTools().Download(ftpDetails.Host, _dossierFilePath, ftpDetails.UserID, ftpDetails.UserPWD, _messages, GetPlayerName);
                        markDSForDelete = true;

                    }
                    catch
                    {
                        dossierFilePath = _dossierFilePath;
                        _messages.Add(String.Format("Error : Failed Retrieving file from FTP. Default back to local cache file [{0}]", GetPlayerName));
                    }
                }
                else
                    dossierFilePath = _dossierFilePath;

                //start dosier decrypt
                Process process = new Process();

//#if DEBUG
//                process.StartInfo.CreateNoWindow = false;
//                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
//#else
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//#endif

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
              
                process.StartInfo.WorkingDirectory = WOTHelper.GetPythonDirectory();
                process.StartInfo.FileName = WOTHelper.GetPythonDirectory() + "\\" + WOTHelper.GetPythonFile();
                process.StartInfo.Arguments = String.Format(@"""{0}"" -s -t", dossierFilePath.Replace(@"\", @"\\"));
                //WOTHelper.AddToLog(process.StartInfo.WorkingDirectory);
                //WOTHelper.AddToLog(process.StartInfo.FileName);
                //WOTHelper.AddToLog(process.StartInfo.Arguments);

                try
                {
                    process.Start();
                }
                catch
                {
                    _messages.Add("Error starting WOTDC2J.");
                }
                

                string file = process.StandardOutput.ReadToEnd();

                //WOTHelper.AddToLog("OUT: " + file);

                process.WaitForExit();
                process.Dispose();

     
                //string file = File.ReadAllText(String.Format("{0}\\{1}", WOTHelper.GetTempFolder(), tempFile));
                //File.Delete(String.Format("{0}\\{1}", WOTHelper.GetTempFolder(), tempFile));


                if (markDSForDelete)
                {
                    File.Delete(dossierFilePath);
                }

                try
                {
                    string fileName = DateTime.Now.AddHours(UserSettings.TimeAdjustment).ToString("yyyyMMdd");
                    dynamic obj_testfile = Newtonsoft.Json.JsonConvert.DeserializeObject(file);
                    MemoryTables mt = new MemoryTables(WOTHelper.GetDBPath(_playerName));
                    mt.Fill(obj_testfile, int.Parse(fileName));

                    if (IsNewestFile(fileName, mt))
                    {
                        //string fileName = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTimeStamp).AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours).AddHours(AppSettings.TimeAdjustment).ToString("yyyyMMdd");
                        CreateRecentBattle(mt);
                        SaveFile(fileName, mt);

                        file = null;
                        //lastPlayed.Clear();
                        LoadDossierFiles();
                        OnCurrentFileChange(EventArgs.Empty);
                    }
                    //((IDisposable)obj).Dispose();
                }
                catch (Exception ex)
                {
                    _messages.Add("Error : Unable to parse json (" + GetPlayerName + ") - " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                _messages.Add("Error : Refreshing Dossier File (" + GetPlayerName + ") - " + ex.Message);
                try
                {
                    LoadDossierFiles();
                    OnCurrentFileChange(EventArgs.Empty);
                }
                catch (Exception ex1)
                {

                    _messages.Add("Error : Refreshing Dossier File (" + GetPlayerName + ") - " + ex1.Message);
                }
            }
            _messages.Add("Info : Dossier file refreshed successfully. (" + GetPlayerName + ")");
             digSW.Stop();
             _messages.Add("Diagnostics : Dossier Processing Finished in " + digSW.ElapsedMilliseconds + " miliseconds");

        }

        private bool IsNewestFile(string fileName, MemoryTables mt)
        {
            using (DataTable tankDetail = mt.GetTable("File_TankDetails"))
            {
                using (DataTable battles = mt.GetTable("File_Battles"))
                {
                    int file = GetCurrentPlayerFile();
                    DataTable dt;
                    if (file == 0)
                        return true;
                    else
                    {
                        string sql = String.Format(@"select T1.cmCountryID, T1.cmTankID, sum(T2.bpBattleCount) bpBattleCount from File_TankDetails T1 inner join File_Battles T2 on T1.cmID = T2.bpParentID  where cmFileID = {0} group by T1.cmCountryID, T1.cmTankID --and bpBattleMode in (15, 0)", file);
                        using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerName)))
                        {
                            if (!UserSettings.ClearRecentBattlesFragList.Split(';').Contains(_playerName))
                            {
                                //trying to clean up the mess need to remove this sometime in the future
                                using (DataTable recordDelete = db.GetDataTable("select cmFileID, cmID from File_Battles inner join File_TankDetails on bpParentID = cmID where bpWins > bpBattleCount  group by cmFileID"))
                                {
                                    foreach (DataRow delRows in recordDelete.Rows)
                                    {
                                        db.ExecuteNonQuery(@"delete from File_Achievements where faParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_Battles where bpParentID = " + delRows.GetSafeInt("cmID"));

                                        db.ExecuteNonQuery(@"delete from File_Clan where clParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_Company where fcParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_Historical where hsParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_FragList where fgParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_Total where foParentID = " + delRows.GetSafeInt("cmID"));
                                        db.ExecuteNonQuery(@"delete from File_TankDetails where cmFileID = " + delRows.GetSafeInt("cmFileID"));
                                        db.ExecuteNonQuery(@"delete from Files where fiID  = " + delRows.GetSafeInt("cmFileID"));
                                        db.ExecuteNonQuery(@"delete from recentbattles  where rbBattles > 100");
                                    }
                                    LoadDossierFiles();
                                }

                                db.ExecuteNonQuery(@"delete from File_Battles where bpFrags8P >  bpFrags");
                                db.ExecuteNonQuery(@"delete from File_Battles where bpMaxFrags >  15");


                                db.ExecuteNonQuery(@"update RecentBattles set rbFragList=''");
                                UserSettings.ClearRecentBattlesFragList = UserSettings.ConvertFlatFilesToDB + _playerName + ";";
                            }

                            

                            dt = db.GetDataTable(sql);
                        }
                    }
                    int obj_Sum = 0;
                    int obj_TestFile_Sum = 0;
                    using (TankDescriptions tankDescription = new TankDescriptions(_messages))
                    {
                        foreach (DataRow tank in dt.Rows)
                            if (tankDescription.Active(tank.GetSafeInt("cmCountryID"), tank.GetSafeInt("cmTankID")))
                                obj_Sum += tank.GetSafeInt("bpBattleCount");
                        foreach (DataRow tank in tankDetail.Rows)
                        {
                            if (tankDescription.Active(tank.GetSafeInt("cmCountryID"), tank.GetSafeInt("cmTankID")))
                            {
                                obj_TestFile_Sum += (from x in battles.AsEnumerable()
                                                     where x.GetSafeInt("bpParentID") == tank.GetSafeInt("cmID") //&& (x.GetSafeInt("bpBattleMode") == 15 || x.GetSafeInt("bpBattleMode") == 0)
                                                     select x.GetSafeInt("bpBattleCount")).Sum();
                            }
                        }
                    }
                    if (obj_Sum <= obj_TestFile_Sum)
                        return true;
                    else
                        return false;
                }
            }

        }

        private void CreateRecentBattle(MemoryTables mt)
        {
            //try
            //{
                Dossier fileB = new Dossier(GetCurrentPlayerFile(), _playerName, _messages);
                WOTStats stats = fileB.GetStats();
                double avgTier = stats.AverageTier;
                double vics = stats.Victory_Percentage;
                double defPoints = stats.AverageDefencePoints;

                List<string> cur = (from x in  mt.GetTable("File_Total").AsEnumerable()
                                        select x["foLastBattleTime"].ToString()).ToList();
                List<string> saved ;
                DataTable currentRecord;
                DataTable Kills;
                using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerName), false))
                {
                    saved = (from x in  db.GetDataTable("select foLastBattleTime from File_Total").AsEnumerable()
                                        select x["foLastBattleTime"].ToString()).ToList();

                    currentRecord = db.GetDataTable(@"select
                                  case when T2.bpBattleMode = 7 then 7 else 15 end as bpBattleMode, T1.cmID, T1.cmCountryID, T1.cmTankID, T1.cmUpdated, (T2.bpBattleCount) bpBattleCount, (t2.bpSpotted) bpSpotted,  (t2.bpHits) bpHits,  (t2.bpMaxFrags) bpMaxFrags,  (t2.bpMaxXP) bpMaxXP,  (t2.bpWins) bpWins,  (t2.bpCapturePoints) bpCapturePoints,  (t2.bpLosses) bpLosses,
                                   (t2.bpSurvivedBattles) bpSurvivedBattles, min(T4.foBattleLifeTime) foBattleLifeTime,  (t2.bpDefencePoints) bpDefencePoints, min(t4.foLastBattleTime) foLastBattleTime,  (t2.bpDamageReceived) bpDamageReceived,  (t2.bpShots) bpShots,  (t2.bpWinAndSurvive) bpWinAndSurvive,  (t2.bpFrags8P) bpFrags8P,
                                    (t2.bpXP) bpXP,                                   
                                    (t2.bpDamageDealt) bpDamageDealt,  
                                    (t2.bpDamageAssistedRadio) bpDamageAssistedRadio,  
                                    (t2.bpDamageAssistedTracks) bpDamageAssistedTracks,  
                                    (t2.bpMileage) bpMileage, 
                                    (t2.bpRatingEffWeight)/(t2.bpBattleCount) bpRatingEff,  
                                    (t2.bpRatingBRWeight)/(t2.bpBattleCount) bpRatingBR,  
                                    (t2.bpRatingWN7Weight)/(t2.bpBattleCount) bpRatingWN7,  
                                    (t2.bpRatingWN8Weight)/(t2.bpBattleCount) bpRatingWN8,  
                                   (t2.bpFrags) bpFrags, t4.foTreesCut, t3.faMarkOfMastery
                                  from File_TankDetails T1
                                  inner join File_Battles T2
                                  on T1.cmID = T2.bpParentID
                                  inner join File_Achievements T3
                                  on T1.cmID = T3.faParentID
                                  inner join File_Total T4
                                  on T1.cmID = T4.foParentID
                                  where cmFileID =  " + GetCurrentPlayerFileKey() + " group by T2.bpBattleMode, T1.cmID, T1.cmCountryID, T1.cmTankID, T1.cmUpdated, t4.foTreesCut, t3.faMarkOfMastery");

                    Kills = db.GetDataTable(String.Format(@"select fgParentID, fgCountryID, fgTankID, sum(fgValue) kills from File_TankDetails
                                                                                            inner join File_FragList
                                                                                            on cmID = fgParentID
                                                                                            where cmFileID = {0}
                                                                                            group by  fgParentID, fgCountryID, fgTankID", GetCurrentPlayerFileKey()));
                }

                List<string> diff = cur.Except(saved).ToList();
                RecentBattles lb = new RecentBattles(GetPlayerName, _messages);
                var battleCheck = from x in mt.GetTable("File_TankDetails").AsEnumerable()
                                  join y in mt.GetTable("File_Battles").AsEnumerable()
                                  on x.GetSafeInt("cmID") equals y.GetSafeInt("bpParentID")
                                  join z in mt.GetTable("File_Total").AsEnumerable()
                                  on x.GetSafeInt("cmID") equals z.GetSafeInt("foParentID")
                                  join d in diff
                                  on z["foLastBattleTime"].ToString() equals d
                                  group y by new { ID = x.GetSafeInt("cmID"), countryID = x.GetSafeInt("cmCountryID"), tankID = x.GetSafeInt("cmTankID"), battleTime = z["foLastBattleTime"].ToString(), BattleMode = y["bpBattleMode"] } into tank
                                  select new
                                  {
                                      ID = tank.Key.ID,
                                      CountryID = tank.Key.countryID,
                                      TankID = tank.Key.tankID,
                                      BattleTime = tank.Key.battleTime,
                                      BattleCount = tank.Sum(y => y.GetSafeInt("bpBattleCount")),
                                      Hits = tank.Sum(y => y.GetSafeInt("bpHits")),
                                      Shots = tank.Sum(y => y.GetSafeInt("bpShots")),
                                      Victory = tank.Sum(y => y.GetSafeInt("bpWins")),
                                      Losses = tank.Sum(y => y.GetSafeInt("bpLosses")),
                                      Survived = tank.Sum(y => y.GetSafeInt("bpSurvivedBattles")),
                                      DefencePoints = tank.Sum(y => y.GetSafeInt("bpDefencePoints")),
                                      CapturePoints = tank.Sum(y => y.GetSafeInt("bpCapturePoints")),
                                      Spotted = tank.Sum(y => y.GetSafeInt("bpSpotted")),
                                      DamageDealt = tank.Sum(y => y.GetSafeInt("bpDamageDealt")),
                                      DamageAssistedRadio = tank.Sum(y => y.GetSafeInt("bpDamageAssistedRadio")),
                                      DamageAssistedTracks = tank.Sum(y => y.GetSafeInt("bpDamageAssistedTracks")),
                                      DamageReceived = tank.Sum(y => y.GetSafeInt("bpDamageReceived")),
                                      Kills = tank.Sum(y => y.GetSafeInt("bpFrags")),
                                      Mileage = tank.Sum(y => y.GetSafeInt("bpMileage")),
                                      RatingEff = tank.Sum(y => y.GetSafeInt("bpRatingEff")),
                                      RatingBR = tank.Sum(y => y.GetSafeInt("bpRatingBR")),
                                      RatingWN7 = tank.Sum(y => y.GetSafeInt("bpRatingWN7")),
                                      RatingWN8 = tank.Sum(y => y.GetSafeInt("bpRatingWN8")),
                                      XPReceived = tank.Sum(y => y.GetSafeInt("bpXP")),
                                      BattleMode = int.Parse(tank.Key.BattleMode.ToString())

                                  };


              

                if (currentRecord.Rows.Count > 0)
                {
                    foreach (var item in battleCheck)
                    {
                        //WOTHelper.AddToLog("BM 1: " + item.BattleMode);
                        DataRow dr = currentRecord.Select(String.Format("cmCountryID = {0} and cmTankID = {1} and bpBattleMode = {2}", item.CountryID, item.TankID, item.BattleMode)).DefaultIfEmpty(null).FirstOrDefault();

                        if (dr != null)
                        {
                            if (item.BattleCount - dr.GetSafeInt("bpBattleCount") > 0)
                            {
                                int i = 0;
                                List<string> newKills = new List<string>();
                                foreach (DataRow killRow in mt.GetTable("File_FragList").Select("fgParentID = " + item.ID))
                                {
                                    DataRow dr_oldKills = Kills.Select(String.Format(@"fgParentID = {0} and  fgCountryID = {1} and fgTankID = {2}", dr.GetSafeInt("cmID"), killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID"))).FirstOrDefault();
                                    if (dr_oldKills != null)
                                    {
                                        if (killRow.GetSafeInt("fgValue") > dr_oldKills.GetSafeInt("kills"))
                                        {
                                            newKills.Add(String.Format("{0}:{1}_{2}", i, killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID")));
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        newKills.Add(String.Format("{0}:{1}_{2}", i, killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID")));
                                        i++;
                                    }
                                }

                                int vic = 0;
                                if (item.Victory - dr.GetSafeInt("bpWins") > 0)
                                    vic = 0;
                                else if (item.Losses - dr.GetSafeInt("bpLosses") > 0)
                                    vic = 1;
                                else
                                    vic = 2;
                                //WOTHelper.AddToLog("ASD: " + dr.GetSafeInt("bpRatingWN8"));
                                lb.Add(int.Parse(item.BattleTime), new RecentBattle()
                                {
                                    CountryID = item.CountryID,
                                    TankID = item.TankID,
                                    Battles = item.BattleCount - dr.GetSafeInt("bpBattleCount"),
                                    BattleTime = int.Parse(item.BattleTime),
                                    Hits = item.Hits - dr.GetSafeInt("bpHits"),
                                    Shot = item.Shots - dr.GetSafeInt("bpShots"),
                                    Victory = vic,
                                    Survived = item.Survived - dr.GetSafeInt("bpSurvivedBattles"),
                                    DefencePoints = item.DefencePoints - dr.GetSafeInt("bpDefencePoints"),
                                    CapturePoints = item.CapturePoints - dr.GetSafeInt("bpCapturePoints"),
                                    Spotted = item.Spotted - dr.GetSafeInt("bpSpotted"),
                                    DamageDealt = item.DamageDealt - dr.GetSafeInt("bpDamageDealt"),
                                    DamageAssistedRadio = item.DamageAssistedRadio - dr.GetSafeInt("bpDamageAssistedRadio"),
                                    DamageAssistedTracks = item.DamageAssistedTracks - dr.GetSafeInt("bpDamageAssistedTracks"),
                                    DamageReceived = item.DamageReceived - dr.GetSafeInt("bpDamageReceived"),
                                    Kills = item.Kills - dr.GetSafeInt("bpFrags"),
                                    Mileage = item.Mileage - dr.GetSafeInt("bpMileage"),
                                    RatingEff = item.RatingEff - dr.GetSafeInt("bpRatingEff"),
                                    RatingBR = item.RatingBR - dr.GetSafeInt("bpRatingBR"),
                                    RatingWN7 = item.RatingWN7 - dr.GetSafeInt("bpRatingWN7"),
                                    RatingWN8 = item.RatingWN8 - dr.GetSafeInt("bpRatingWN8"),
                                    XPReceived = item.XPReceived - dr.GetSafeInt("bpXP"),
                                    OriginalBattleCount = item.BattleCount - dr.GetSafeInt("bpBattleCount"),
                                    FragList = string.Join(";", newKills.ToArray().Take(15)),
                                    GlobalAvgTier = avgTier,
                                    GlobalWinPercentage = vics,
                                    GlobalAvgDefencePoints = defPoints,
                                    BattleMode = item.BattleMode,
                                    DBEntry = false
                                });

                                lb.Save();
                            }
                        }
                        else
                        {

                            int i = 0;
                            List<string> newKills = new List<string>();
                            WOTHelper.AddToLog("BM 2: " + item.BattleMode);
                            dr = currentRecord.Select(String.Format("cmCountryID = {0} and cmTankID = {1} and bpBattleMode <> '{2}'", item.CountryID, item.TankID, item.BattleMode)).DefaultIfEmpty(null).FirstOrDefault();

                            if (dr != null)
                            {
                                foreach (DataRow killRow in mt.GetTable("File_FragList").Select("fgParentID = " + item.ID))
                                {

                                    DataRow dr_oldKills = Kills.Select(String.Format(@"fgParentID = {0} and  fgCountryID = {1} and fgTankID = {2}", dr.GetSafeInt("cmID"), killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID"))).FirstOrDefault();
                                    if (dr_oldKills != null)
                                    {
                                        if (killRow.GetSafeInt("fgValue") > dr_oldKills.GetSafeInt("kills"))
                                        {
                                            newKills.Add(String.Format("{0}:{1}_{2}", i, killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID")));
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        newKills.Add(String.Format("{0}:{1}_{2}", i, killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID")));
                                        i++;
                                    }

                                }
                            }
                            else
                            {

                                foreach (DataRow killRow in mt.GetTable("File_FragList").Select("fgParentID = " + item.ID))
                                {

                                    newKills.Add(String.Format("{0}:{1}_{2}", i, killRow.GetSafeInt("fgCountryID"), killRow.GetSafeInt("fgTankID")));
                                    i++;

                                }
                            }
                            int vic = 0;
                            if (item.Victory - 0 > 0)
                                vic = 0;
                            else if (item.Losses - 0 > 0)
                                vic = 1;
                            else
                                vic = 2;

                            lb.Add(int.Parse(item.BattleTime), new RecentBattle()
                            {
                                CountryID = item.CountryID,
                                TankID = item.TankID,
                                Battles = item.BattleCount,
                                BattleTime = int.Parse(item.BattleTime),
                                Hits = item.Hits,
                                Shot = item.Shots,
                                Victory = vic,
                                Survived = item.Survived,
                                DefencePoints = item.DefencePoints,
                                CapturePoints = item.CapturePoints,
                                Spotted = item.Spotted,
                                DamageDealt = item.DamageDealt,
                                DamageAssistedRadio = item.DamageAssistedRadio,
                                DamageAssistedTracks = item.DamageAssistedTracks,
                                DamageReceived = item.DamageReceived,
                                Kills = item.Kills,
                                Mileage = item.Mileage,
                                XPReceived = item.XPReceived,
                                OriginalBattleCount = item.BattleCount,
                                FragList = string.Join(";", newKills.ToArray().Take(15)),
                                GlobalAvgTier = avgTier,
                                GlobalWinPercentage = vics,
                                GlobalAvgDefencePoints = defPoints,
                                BattleMode = item.BattleMode,
                                RatingEff = item.RatingEff,
                                RatingBR = item.RatingBR,
                                RatingWN7 = item.RatingWN7,
                                RatingWN8 = item.RatingWN8,
                                DBEntry = false
                            });

                            lb.Save();
                        }
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    _messages.Add(String.Format("Error : Cannot create last battle ({0}) - {1}", GetPlayerName, ex.Message));
            //}
        }

        private void SaveFile(string fileName, MemoryTables mt)
        {
            string folderName = String.Format(@"{0}\{1}{2}", WOTHelper.GetApplicationData(), _HistoryIndicator, _playerName);
            
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            if (!Directory.Exists(String.Format(@"{0}\{1}{2}\LastBattle", WOTHelper.GetApplicationData(), _HistoryIndicator, _playerName)))
                Directory.CreateDirectory(String.Format(@"{0}\{1}{2}\LastBattle", WOTHelper.GetApplicationData(), _HistoryIndicator, _playerName));

            //if (File.Exists(GetCurrentPlayerFile()))
            //{

            //    foreach (string file in Directory.GetFiles(String.Format(@"{0}\{1}{2}\LastBattle", WOTHelper.GetApplicationData(), _HistoryIndicator, _playerName)).Where(f => f.Contains(".wot") == false && f.Contains(".db") == false))
            //    {
            //        File.Delete(file);
            //    }
            //    File.Copy(GetCurrentPlayerFile(), String.Format(@"{0}\{1}{2}\LastBattle\{3}.txt", WOTHelper.GetApplicationData(), _HistoryIndicator, _playerName, GetCurrentPlayerFileKey()));
            //}

            using (IDBHelpers db = new DBHelpers(WOTHelper.GetDBPath(_playerName), true))
            {
                db.BeginTransaction();
//                db.ExecuteNonQuery("delete from Cache_LastGame");
//                db.ExecuteNonQuery(string.Format(@"insert into Cache_LastGame
//                                                               (clFileID, clCountryID, clTankID, clBattleCount)
//                                                    select cmFileID, cmCountryID, cmTankID, sum(bpBattleCount) battleCount from File_TankDetails
//                                                    inner join File_Battles on cmID = bpParentID
//                                                    where cmFileID = {0} --and ifnull(bpBattleMode,0) in (15,0)
//                                                    group by cmFileID, cmCountryID, cmTankID", GetCurrentPlayerFile()));


                //check to see if we are inserting or updating

                db.ExecuteNonQuery("delete from File_Achievements where faParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_Battles where bpParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_Clan where clParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_Historical where hsParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_Company where fcParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_FragList where fgParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_Total where foParentID in (select cmID from File_TankDetails where cmFileID = " + fileName + ")");
                db.ExecuteNonQuery("delete from File_TankDetails where cmFileID = " + fileName );
                db.ExecuteNonQuery("delete from Files where fiID = " + fileName);

                string[] tables = { "Files", "File_TankDetails", "File_Achievements", "File_Battles", "File_Clan", "File_Historical", "File_Company", "File_FragList", "File_Total" };
                foreach (string table in tables)
                {
                    using (DataTable files = mt.GetTable(table))
                    {
                        List<string> updateFields = new List<string>();
                        List<string> updateValues = new List<string>();

                        foreach (DataColumn column in files.Columns)
                        {
                            updateFields.Add(column.ColumnName);
                        }

                        foreach (DataRow row in files.Rows)
                        {
                            List<string> values = new List<string>();
                            foreach (DataColumn column in files.Columns)
                            {
                                values.Add(GetSafeValue(row, column.ColumnName).ToString());
                            }

                            updateValues.Add("Select " + string.Join(",", values.ToArray()));
                            values.Clear();
                        }

                        if (updateValues.Count >= 400)
                        {
                            foreach (var item in updateValues)
                            {
                                string updateFilesSQL = String.Format("Insert into {2} ({0}) {1}  ", string.Join(", ", updateFields.ToArray()), item, table);
                                db.ExecuteNonQuery(updateFilesSQL);
                            }
                        }
                        else
                        {
                            if (updateValues.Count > 0)
                            {
                                string updateFilesSQL = String.Format("Insert into {2} ({0}) {1}  ", string.Join(", ", updateFields.ToArray()), string.Join(" Union All ", updateValues.ToArray()), table);
                                db.ExecuteNonQuery(updateFilesSQL);
                            }
                        }
                        
                        updateValues.Clear();
                        updateFields.Clear();

                       // string f = string.Join(" union all ", updateValues.Take(100).ToArray();
                        
                    }
                }
                  
               


                db.EndTransaction();
            }

            

        }

        private object GetSafeValue(DataRow row, string columnName)
        {
            if (row[columnName] == null)
                return DBNull.Value;
            else
                return "'" + row[columnName].ToString() + "'";
        }

        public void StartDossierWatch()
        {
            _dossierWatcher.Start();
            MonitorStarted = true;
        }

        public void StopDossierWatch()
        {
            _dossierWatcher.Stop();
            MonitorStarted = false;
        }

        public void RefreshDossier()
        {
            ProcessDossierFile();
        }

        public Dictionary<Int32, Int32> GetAllFilesForPlayer()
        {
            return (from x in _HistoryFiles
                    from y in x.Value
                    where x.Key == GetPlayerID
                    select new KeyValuePair<Int32, Int32>(y.Key, y.Value)).ToDictionary(t => t.Key, t => t.Value) ?? new Dictionary<Int32, Int32>();
        }

        public Dictionary<Int32, String> GetAllFilesForPlayerFriendly()
        {
            return (from x in _HistoryFiles
                    from y in x.Value
                    where x.Key == GetPlayerID
                    select new KeyValuePair<Int32, Int32>(y.Key, y.Value)).ToDictionary(t => t.Key, t => t.Key.ToString().Insert(4, "-").Insert(7, "-")) ?? new Dictionary<Int32, string>();
        }

        public string GetPlayerFileName(Int32 key)
        {
            return (from x in _HistoryFiles
                    from y in x.Value
                    where x.Key == GetPlayerID && y.Key == key
                    select y.Value.ToString().Insert(4, "-").Insert(7, "-")).DefaultIfEmpty(null).FirstOrDefault();
        }

        public int GetFileA()
        {

            PlayerListing pl = new PlayerListing(_messages);
            Player player = pl.GetPlayer(GetPlayerID);
            if (player.PreviousFile == "0")
            {
                return GetCurrentPlayerFile();
            }
            else if (player.PreviousFile == "1")
            {
                return GetPlayerPreviousFile();
            }
            else if (player.PreviousFile == "2")
            {
                DateTime endFile = FormatTextDate(GetCurrentPlayerFile().ToString());
                DateTime startFile = FormatTextDate(GetCurrentPlayerFile().ToString()).AddDays(-7);

                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                             where DateTime.Parse(d.Value) >= startFile && DateTime.Parse(d.Value) <= endFile
                             select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            else if (player.PreviousFile == "3")
            {
                DateTime endFile = FormatTextDate(GetCurrentPlayerFile().ToString());
                DateTime startFile = FormatTextDate(GetCurrentPlayerFile().ToString()).AddDays(-14);

                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                             where DateTime.Parse(d.Value) >= startFile && DateTime.Parse(d.Value) <= endFile
                             select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            else
            {
                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                             where DateTime.Parse(d.Value) == DateTime.Parse(player.PreviousFile.Insert(4,"-").Insert(7,"-"))
                             select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            
        }

        public int GetFileB()
        {
            PlayerListing pl = new PlayerListing(_messages);
            Player player = pl.GetPlayer(GetPlayerID);
            if (player.CurrentFile == "0")
            {
                return GetCurrentPlayerFile();
            }
            else if (player.CurrentFile == "1")
            {
                return GetPlayerPreviousFile();
            }
            else if (player.CurrentFile == "2")
            {
                DateTime endFile = FormatTextDate(GetCurrentPlayerFile().ToString());
                DateTime startFile = FormatTextDate(GetCurrentPlayerFile().ToString()).AddDays(-7);

                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                            where DateTime.Parse(d.Value) >= startFile && DateTime.Parse(d.Value) <= endFile
                            select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            else if (player.CurrentFile == "3")
            {
                DateTime endFile = FormatTextDate(GetCurrentPlayerFile().ToString());
                DateTime startFile = FormatTextDate(GetCurrentPlayerFile().ToString()).AddDays(-14);

                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                             where DateTime.Parse(d.Value) >= startFile && DateTime.Parse(d.Value) <= endFile
                             select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            else
            {
                Dictionary<Int32, string> files = GetAllFilesForPlayerFriendly();
                Int32 key = (from d in files
                             where DateTime.Parse(d.Value) == DateTime.Parse(player.CurrentFile.Insert(4, "-").Insert(7, "-")) 
                             select d.Key).FirstOrDefault();

                int file = (from g in GetAllFilesForPlayer()
                               where g.Key == key
                               select g.Value).FirstOrDefault();

                return file;
            }
            
        }

        public DateTime FormatTextDate(string date)
        {
            try
            {
                return DateTime.Parse(date.Substring(date.LastIndexOf(@"\") + 1).Replace(".txt", "").Insert(4, "-").Insert(7, "-"));
            }
            catch 
            {
                
                return DateTime.Parse("1900-01-01");
            }
        }

        public int GetCurrentPlayerFile()
        {
            Dictionary<Int32, Int32> files = GetAllFilesForPlayer();

            if (files.Count() > 0)
            {
                Int32 maxFile = files.Max(x => x.Key);
                return files.FirstOrDefault(x => x.Value == maxFile).Key;
            }
            else
            {
                return 0;
            }
        }

        public int GetCurrentPlayerFileKey()
        {
            Dictionary<Int32, Int32> files = GetAllFilesForPlayer();

            if (files.Count() > 0)
            {
                Int32 maxFile = files.Max(x => x.Key);
                return files.FirstOrDefault(x => x.Value == maxFile).Key;
            }
            else
            {
                return 0;
            }
        }

        public int GetPlayerFile(Int32 fileKey)
        {
            Dictionary<Int32, Int32> files = GetAllFilesForPlayer();
            return files.FirstOrDefault(x => x.Value == fileKey).Value;
        }

        public int GetPlayerPreviousFile()
        {
            Dictionary<Int32, Int32> files = GetAllFilesForPlayer();

            if (files.Count > 1)
            {
                Int32 maxFile = (from max in files
                                 select max.Key).Max();

                Int32 exclMaxFile = (from max in files
                                     where max.Key != maxFile
                                     select max.Key).Max();

                return (from file in files
                        where file.Key == exclMaxFile
                        select file.Value).FirstOrDefault();
            }
            else
            {
                return 0;
            }
           
        }

        
    }

 

}
