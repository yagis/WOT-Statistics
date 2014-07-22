using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using System.Diagnostics;
using System.IO;
using System.Web.Script.Serialization;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace WOT.Stats
{
    public partial class frmFTPBrowser : DevExpress.XtraEditors.XtraForm
    {
        private readonly string _ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        TankDescriptions tankDesc = new TankDescriptions(new MessageQueue());


        public frmFTPBrowser()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
           

        }

        private void ProcessDossierFile(string dossierFilePath, DataTable table, string playerName)
        {
            //System.Diagnostics.Stopwatch digSW = new Stopwatch();
            //digSW.Start();  
            string tempFile = Guid.NewGuid().ToString();

            try
            {
                


                //start dosier decrypt
                Process process = new Process();
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = _ApplicationPath + "\\python\\wotdc2j.exe";
                process.StartInfo.Arguments = String.Format(@"""{0}"" ""{1}\\{2}""", dossierFilePath, WOTHelper.GetTempFolder(), tempFile);
                process.Start();
                process.WaitForExit();
                process.Dispose();

                string file = File.ReadAllText(String.Format("{0}\\{1}", WOTHelper.GetTempFolder(), tempFile));
                //File.Delete(String.Format("{0}\\{1}", WOTHelper.GetTempFolder(), tempFile));
                //File.Delete(dossierFilePath);

                var serializer = new JavaScriptSerializer();
                serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic obj = serializer.Deserialize(file, typeof(object));
  
                foreach (var item in obj.tanks)
                {
                    DataRow row = table.NewRow();
                    row.SetField("PlayerName", playerName);
                    row.SetField("Tank", tankDesc.Description((int)item.countryid, (int)item.tankid));
                    row.SetField("LastPlayed", new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((int)item.updated).AddHours(TimeZoneInfo.Local.BaseUtcOffset.Hours));
                    row.SetField("BattleCount", (int)item.tankdata.battlesCount);
                    row.SetField("Victories", (int)item.tankdata.wins);
                    row.SetField("Defeats", (int)item.tankdata.losses);
                    row.SetField("Draws", (int)item.tankdata.battlesCount - ((int)item.tankdata.wins + (int)item.tankdata.losses));
                    row.SetField("Survived", (int)item.tankdata.survivedBattles);
                    row.SetField("Destroyed", (int)item.tankdata.frags);
                    row.SetField("Detected", (int)item.tankdata.spotted);
                    row.SetField("HitRatio", (double.IsNaN((double)item.tankdata.hits / (double)item.tankdata.shots) ? 0 : (double)item.tankdata.hits / (double)item.tankdata.shots )*100);
                    row.SetField("Damage", (int)item.tankdata.damageDealt );
                    row.SetField("CapturePoints", (int)item.tankdata.capturePoints );
                    row.SetField("DefencePoints", (int)item.tankdata.droppedCapturePoints );
                    row.SetField("TotalExperience", (int)item.tankdata.xp );
                    row.SetField("AvgExperience", (double)item.tankdata.xp / (double)item.tankdata.battlesCount);
                    row.SetField("AvgDamage", (double)item.tankdata.damageDealt / (double)item.tankdata.battlesCount);
                    row.SetField("AvgDamage", (double)item.tankdata.damageDealt / (double)item.tankdata.battlesCount);
                    row.SetField("Hits", (int)item.tankdata.hits);
                    row.SetField("Shots", (int)item.tankdata.shots);
                    table.Rows.Add(row);
                }

                ((IDisposable)obj).Dispose();

            }
            catch 
            {

            }
            finally
            {
                File.Delete(String.Format("{0}\\{1}", WOTHelper.GetTempFolder(), tempFile));
                File.Delete(dossierFilePath);
            }

            // digSW.Stop();
            // _messages.Add("Diagnostics : Dossier Processing Finished in " + digSW.ElapsedMilliseconds + " miliseconds");

        }

        private void barButtonGetFiles_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
            }
            catch { }
            DataTable dtMaster = new DataTable();
            dtMaster.TableName = "master";
            dtMaster.Columns.Add("PlayerName", typeof(string));
            dtMaster.Columns.Add("LastPlayed", typeof(DateTime));
            dtMaster.Columns.Add("BattleCount", typeof(Int32));
            dtMaster.Columns.Add("Victories", typeof(Int32));
            dtMaster.Columns.Add("Defeats", typeof(Int32));
            dtMaster.Columns.Add("Draws", typeof(Int32));
            dtMaster.Columns.Add("Survived", typeof(Int32));
            dtMaster.Columns.Add("Destroyed", typeof(Int32));
            dtMaster.Columns.Add("Detected", typeof(Int32));
            dtMaster.Columns.Add("HitRatio", typeof(double));
            dtMaster.Columns.Add("Damage", typeof(Int32));
            dtMaster.Columns.Add("CapturePoints", typeof(Int32));
            dtMaster.Columns.Add("DefencePoints", typeof(Int32));
            dtMaster.Columns.Add("TotalExperience", typeof(Int32));
            dtMaster.Columns.Add("AvgExperience", typeof(double));
            dtMaster.Columns.Add("AvgDamage", typeof(double));

            DataTable dtChild = new DataTable();
            dtChild.TableName = "child";
            dtChild.Columns.Add("PlayerName", typeof(string));
            dtChild.Columns.Add("Tank", typeof(string));
            dtChild.Columns.Add("LastPlayed", typeof(DateTime));
            dtChild.Columns.Add("BattleCount", typeof(Int32));
            dtChild.Columns.Add("Victories", typeof(Int32));
            dtChild.Columns.Add("Defeats", typeof(Int32));
            dtChild.Columns.Add("Draws", typeof(Int32));
            dtChild.Columns.Add("Survived", typeof(Int32));
            dtChild.Columns.Add("Destroyed", typeof(Int32));
            dtChild.Columns.Add("Detected", typeof(Int32));
            dtChild.Columns.Add("HitRatio", typeof(double));
            dtChild.Columns.Add("Damage", typeof(Int32));
            dtChild.Columns.Add("CapturePoints", typeof(Int32));
            dtChild.Columns.Add("DefencePoints", typeof(Int32));
            dtChild.Columns.Add("TotalExperience", typeof(Int32));
            dtChild.Columns.Add("AvgExperience", typeof(double));
            dtChild.Columns.Add("AvgDamage", typeof(double));
            dtChild.Columns.Add("Hits", typeof(Int32));
            dtChild.Columns.Add("shots", typeof(Int32));

           

            PlayerListing pl = new PlayerListing(new MessageQueue());


            FTPTools ftpTools = new FTPTools();
            FTPDetails ftpDetails = new FTPDetails();
            
            Encoding byteConverter = Encoding.GetEncoding("UTF-8");
            List<string> files = ftpTools.DirectoryListing(ftpDetails.Host, ftpDetails.UserID, ftpDetails.UserPWD);
            foreach (string item in files)
            {
                string playerName = byteConverter.GetString(Base32.Decode(item.Remove(item.IndexOf('.')))).Split(';')[1];
                string fileName = ftpTools.Download(ftpDetails.Host, item, ftpDetails.UserID, ftpDetails.UserPWD, new MessageQueue(), playerName);
                ProcessDossierFile(fileName, dtChild, playerName);

                if (dtChild.Rows.Count > 0)
                {
                    DataRow rowM = dtMaster.NewRow();

                    var totals = (from x in dtChild.AsEnumerable()
                                 group x by x["PlayerName"] into totalgroup
                                  where totalgroup.Key.ToString() == playerName
                                 select new
                                 {
                                     PlayerName = totalgroup.Key,
                                     LastPlayed = totalgroup.Max((r) => DateTime.Parse(r["LastPlayed"].ToString())),
                                     BattleCount = totalgroup.Sum((r) => int.Parse(r["BattleCount"].ToString())),
                                     Victories = totalgroup.Sum((r) => int.Parse(r["Victories"].ToString())),
                                     Defeats = totalgroup.Sum((r) => int.Parse(r["Defeats"].ToString())),
                                     Draws = totalgroup.Sum((r) => int.Parse(r["Draws"].ToString())),
                                     Survived = totalgroup.Sum((r) => int.Parse(r["Survived"].ToString())),
                                     Destroyed = totalgroup.Sum((r) => int.Parse(r["Destroyed"].ToString())),
                                     Detected = totalgroup.Sum((r) => int.Parse(r["Detected"].ToString())),
                                     Hits = totalgroup.Sum((r) => int.Parse(r["Hits"].ToString())),
                                     Shots = totalgroup.Sum((r) => int.Parse(r["Shots"].ToString())),
                                     Damage = totalgroup.Sum((r) => int.Parse(r["Damage"].ToString())),
                                     CapturePoints = totalgroup.Sum((r) => int.Parse(r["CapturePoints"].ToString())),
                                     DefencePoints = totalgroup.Sum((r) => int.Parse(r["DefencePoints"].ToString())),
                                     TotalExperience = totalgroup.Sum((r) => int.Parse(r["TotalExperience"].ToString())),
                                    
                                 }).FirstOrDefault();

                    if (totals != null)
                    {
                        rowM.SetField("PlayerName", totals.PlayerName);
                        rowM.SetField("LastPlayed", totals.LastPlayed);
                        rowM.SetField("BattleCount", totals.BattleCount);
                        rowM.SetField("Victories", totals.Victories);
                        rowM.SetField("Defeats", totals.Defeats);
                        rowM.SetField("Draws", totals.Draws);
                        rowM.SetField("Survived", totals.Survived);
                        rowM.SetField("Destroyed", totals.Destroyed);
                        rowM.SetField("Detected", totals.Detected);
                        rowM.SetField("HitRatio", ((double)totals.Hits / (double)totals.Shots) * 100);
                        rowM.SetField("Damage", totals.Damage);
                        rowM.SetField("CapturePoints", totals.CapturePoints);
                        rowM.SetField("DefencePoints", totals.DefencePoints);
                        rowM.SetField("TotalExperience", totals.TotalExperience);
                        rowM.SetField("AvgExperience", totals.TotalExperience / totals.BattleCount);
                        rowM.SetField("AvgDamage", totals.Damage / totals.BattleCount);
                        dtMaster.Rows.Add(rowM);
                    }
                }
                //gridControl1.RefreshDataSource();
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtMaster);
            ds.Tables.Add(dtChild);

            DataColumn keyColumn = ds.Tables["master"].Columns["PlayerName"];
            DataColumn foreignKeyColumn = ds.Tables["child"].Columns["PlayerName"];
            ds.Relations.Add("Player_Detail", keyColumn, foreignKeyColumn);

            gridControl1.DataSource = ds.Tables["master"];
            gridControl1.ForceInitialize();
            gridControl1.LevelTree.Nodes.Add("Player_Detail", PlayerDetail);


            try
            {
                
                if (splashScreenManager1.IsSplashFormVisible)
                {
                    splashScreenManager1.CloseWaitForm();
                }
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.CloseWaitForm();
            }
            catch { }
            //gridControl1.DataSource = ds.Tables["master"];
            //gridControl1.ForceInitialize();
            //gridControl1.LevelTree.Nodes.Add("Player_Detail", PlayerDetail);

            //PlayerDetail.PopulateColumns(ds.Tables["child"]);

        }

           void repositoryItemCheckEdit1_CheckedChanged(object sender, System.EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("I have been Changed");
        }

           private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
           {
               //if (e.Column.FieldName == "Monitor" && e.IsGetData && e.Value == "true")
               //    ((RepositoryItemButtonEdit)e.Column.ColumnEdit).Buttons[0].Enabled = false;
           }

           private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
           {

               //if (e.Column.FieldName == "Monitor")
                   
               
               //GridView gv = sender as GridView;
               //gv.SetFocusedRowModified();
               //string fieldName = gv.GetFocusedRowCellValue("Monitor").ToString();

               //if (fieldName == "True")
               //{
               //    ((RepositoryItemButtonEdit)gv.Columns["Monitor"].ColumnEdit).Buttons[0].Enabled = false;
               //}
               //else
               //{
               //    ((RepositoryItemButtonEdit)gv.Columns["Monitor"].ColumnEdit).Buttons[0].Enabled = true;
               //}
              
           }

           private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
           {
               //GridView gv = sender as GridView;
               ////if (Convert.ToBoolean(gv.GetRowCellValue(gv.FocusedRowHandle, gv.Columns["Monitor"]).ToString()))
               ////    e.Cancel = true;

               //    ((RepositoryItemButtonEdit)gv.GetFocusedRow().Columns["Monitor"].ColumnEdit).Buttons[0].Enabled = !Convert.ToBoolean(gv.GetRowCellValue(gv.GetFocusedRow(), gv.Columns["Monitor"]).ToString());


           }
    }
}