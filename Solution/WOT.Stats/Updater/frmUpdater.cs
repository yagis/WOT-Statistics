using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WOTStatistics.Core;
using System.Data;

namespace WOT.Stats
{
    public delegate void UpdateGUIProgressBar (Tuple<int,int> info, int type);
    public delegate void UPdateGUIListView(string sShort, string sField, string sNewValue);
    
    public partial class frmUpdater : DevExpress.XtraEditors.XtraForm
    {
        private BackgroundWorker _workerThread;
        Dictionary<int, string> _updateServers = new Dictionary<int, string>();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        readonly System.Threading.ManualResetEvent _resetEvents = new System.Threading.ManualResetEvent(true);
        //readonly List<Tuple<string, string, string, string>> _components = new List<Tuple<string, string, string, string>>();
        System.Data.DataTable oTableUpdate = null;
        private bool _done = false;
        private bool _cancel = false;

        public static List<string> info = new List<string>();

        public frmUpdater()
        {




            InitializeComponent();
            WOTHelper.CreateAppDataFolder();

            if (!File.Exists(WOTHelper.GetUserFile()))
            {
                File.Copy(Path.Combine(WOTHelper.GetEXEPath(), "User.xml"), WOTHelper.GetUserFile());
            }

            prgBarProgress.CustomDisplayText += prgBarProgress_CustomDisplayText;
        }

        void prgBarProgress_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (prgBarProgress.Position > 0)
                if (prgBarProgress.Tag.ToString() == "1")
                {
                    e.DisplayText = string.Format("{0} / {1}", FormatBytes(prgBarProgress.Position), FormatBytes(prgBarProgress.Properties.Maximum));
                }
                else
                {
                    e.DisplayText = string.Format("{0} / {1}", prgBarProgress.Position, prgBarProgress.Properties.Maximum);
                }

                else
                    e.DisplayText = "";

            if (_done)
                e.DisplayText = "Update Complete. Starting Application.";

            if (_cancel)
                e.DisplayText = "Update Canceled. Starting Application.";
        }

        public void UpdateProgressBar(Tuple<int,int> info, int type)
        {
            if (this.prgBarProgress.InvokeRequired)
            {
                UpdateGUIProgressBar upd = UpdateProgressBar;
                this.Invoke(upd, new object[] { info, type });
            }
            else
            {
                prgBarProgress.Properties.Minimum = 0;
                prgBarProgress.Properties.Step = 1;
                prgBarProgress.Properties.Maximum = info.Item2;
                prgBarProgress.Tag = type;

                if (type == 1)
                    prgBarProgress.Position = prgBarProgress.Position + ((prgBarProgress.Position + info.Item1) > prgBarProgress.Properties.Maximum ? info.Item1 - ((info.Item1 + prgBarProgress.Position) - prgBarProgress.Properties.Maximum) : info.Item1);
                else
                    prgBarProgress.Position = info.Item1;
                
            }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private static string FormatBytes(int value)
        {
            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1 << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public void UpdateListview(string sShort, string sField, string sNewValue)
        {
            if (this.gridUpdates.InvokeRequired)
            {
                UPdateGUIListView upd = UpdateListview;
                this.Invoke(upd, new object[] {  sShort,  sField,  sNewValue});
            }
            else
            {

                DataRow[] oDataRows = oTableUpdate.Select("Short='" + sShort + "'");
                if (oDataRows.Length > 0)
                {
                    DataRow oDataRow = oDataRows[0];
                    oDataRow.SetField(sField, sNewValue);
                    WOTStatistics.Core.WOTHelper.AddToLog("Updater " + sShort + ": " + sNewValue);
                }

                gridUpdates.DataSource = oTableUpdate;
                viewUpdates.Columns["Short"].Visible = false;
                viewUpdates.Columns["VersionLocal"].Visible=false;
                viewUpdates.Columns["VersionServer"].Visible=false;
                //viewUpdates.Columns["Status"].ColumnEdit = oStatus;
                gridUpdates.Refresh();
  
                viewUpdates.Columns["Name"].Width = 200;

               //viewUpdates.BestFitColumns();
           }   
        }

       
        private void UpdateMe_Load(object sender, EventArgs e)
        {

            //WN8ExpectedGrid dd = new WN8ExpectedGrid();
            //dd.Show();

            WOTStatistics.Core.WOTHelper.AddToLog("########## WOT Statistics initializing");

            _updateServers.Add(1, "http://www.vbaddict.net:82/");

             oTableUpdate = new DataTable();

            oTableUpdate.Columns.Add("Short", System.Type.GetType("System.String"));
            oTableUpdate.Columns.Add("Name", System.Type.GetType("System.String"));
            oTableUpdate.Columns.Add("VersionLocal", System.Type.GetType("System.String"));
            oTableUpdate.Columns.Add("VersionServer", System.Type.GetType("System.String"));
            oTableUpdate.Columns.Add("Status", System.Type.GetType("System.String"));
   
            
               oTableUpdate.BeginLoadData();
            oTableUpdate.LoadDataRow(new object[] { "CheckVersion", "Server Connection"}, true);
            oTableUpdate.LoadDataRow(new object[] { "AppVersion", "Version", WOTStatistics.Core.UserSettings.AppVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "ReleaseNotes", "Release Notes", WOTStatistics.Core.UserSettings.LastReleaseNotes }, true);
            oTableUpdate.LoadDataRow(new object[] { "SettingsFileVersion", "Settings", WOTStatistics.Core.UserSettings.SettingsFileVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "Images", "Image Verification" }, true);
            oTableUpdate.LoadDataRow(new object[] { "TranslationFileVersion", "Translations", WOTStatistics.Core.UserSettings.TranslationFileVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "ScriptFileVersion", "Rating Formula", WOTStatistics.Core.UserSettings.ScriptFileVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "DossierVersion", "Dossier Decrypter", WOTStatistics.Core.UserSettings.DossierVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "WN8ExpectedVersion", "WN8 Expected Tank Values", WOTStatistics.Core.UserSettings.WN8ExpectedVersion }, true);
            //oTableUpdate.LoadDataRow(new object[] { "ActiveDossierUploaderVersion", "Active Dossier Uploader", WOTStatistics.Core.UserSettings.ActiveDossierUploaderVersion }, true);
            oTableUpdate.LoadDataRow(new object[] { "DBMain", "Database State" }, true);
            oTableUpdate.EndLoadData();

           
            //oStatus.SmallImages = imgStatus;
            //oStatus.Items.Clear();
            //oStatus.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Fail", 0, 0));
            //oStatus.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem("Success", 1, 1));


            _workerThread = new BackgroundWorker() { WorkerSupportsCancellation = true };
            _workerThread.DoWork += new DoWorkEventHandler(bgw_DoWork);
            _workerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            _workerThread.RunWorkerAsync();
        }


     

        public void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            using (OnlineUpdate worker = new OnlineUpdate(_resetEvents, _workerThread, e))
            {
                UpdateGUIProgressBar guiPrg = this.UpdateProgressBar;
                UPdateGUIListView guiList = this.UpdateListview;
                worker.GetUpdateInfo(_updateServers, guiPrg, guiList, oTableUpdate);
            }
            
        }

        public  void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _done = true;

            Thread.Sleep(1000);

            //###########
            //this.Close();
            //Close();
            //Application.Exit();
            //###########

           

            this.Hide();
            this.Close();
            if (UserSettings.MinimiseonStartup == true)
            {
                WOTTrayApp sysTray = new WOTTrayApp();
                sysTray.Show();
            }
            else
            {

                if (WOTStatistics.Core.UserSettings.AppVersion != WOTStatistics.Core.UserSettings.LastReleaseNotes)
                {

                try
                {

                    WOTStatistics.Core.UserSettings.LastReleaseNotes = WOTStatistics.Core.UserSettings.AppVersion;
                    string sVersion = "ReleaseNote_" + WOTStatistics.Core.UserSettings.AppVersion.Replace(".", string.Empty) + ".htm";
                    ReleaseNotes oRN = new ReleaseNotes();
                    oRN.LoadPage(Path.Combine(WOTStatistics.Core.WOTHelper.GetApplicationData(), sVersion));
                    oRN.ShowDialog();
                    oRN.BringToFront();

                }
                catch (Exception exrn)
                {
                    WOTHelper.AddToLog("Release Notes: " + exrn.Message);
                }

                }


                frmMain frm = new frmMain();
                WOTHelper.AddToLog("Starting Main Form...");
                frm.Show();
                WOTHelper.AddToLog("Starting Main Form DONE");
            }

            _workerThread.Dispose();
            this.Dispose();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_workerThread.IsBusy)
            {
                _cancel = true;
                _workerThread.CancelAsync();
            }
            else
            {

                this.Hide();
                this.Close();
                frmMain frm = new frmMain();
                frm.Show();
                _workerThread.Dispose();
                this.Dispose();
            }
           
           
        }


        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

 



    }

}