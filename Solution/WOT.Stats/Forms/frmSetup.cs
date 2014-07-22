using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using WOTStatistics.Core;
using System.Collections.Generic;
using System.Linq;

namespace WOT.Stats
{
    public partial class frmSetup : DevExpress.XtraEditors.XtraForm
    {

        public Dictionary<string, PropertyFields> _propertyFields = new Dictionary<string, PropertyFields>();

        string _topicID = "100";
        public frmSetup(string page)
        {

            _propertyFields.Add("StartMonOnStartUp", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.StartMonOnStartUp, NewValue = UserSettings.StartMonOnStartUp });
            _propertyFields.Add("LaunchWithWindows", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.LaunchWithWindows, NewValue = UserSettings.LaunchWithWindows });
            _propertyFields.Add("MinimiseonStartup", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.MinimiseonStartup, NewValue = UserSettings.MinimiseonStartup });
            _propertyFields.Add("MinimiseToTray", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.MinimiseToTray, NewValue = UserSettings.MinimiseToTray });
            _propertyFields.Add("LastPlayedCompare", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.LastPlayedCompare, NewValue = UserSettings.LastPlayedCompare });
            _propertyFields.Add("AllowVersionCheck", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AllowVersionCheck, NewValue = UserSettings.AllowVersionCheck });
            _propertyFields.Add("VersionCounter", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.VersionCounter, NewValue = UserSettings.VersionCounter });
            _propertyFields.Add("SystemFont", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.SystemFont, NewValue = UserSettings.SystemFont });
            _propertyFields.Add("HTMLCellFont", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.HTMLCellFont, NewValue = UserSettings.HTMLCellFont });
            _propertyFields.Add("HTMLHeaderFont", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.HTMLHeaderFont, NewValue = UserSettings.HTMLHeaderFont });
            _propertyFields.Add("HTMLTankInfoHeader", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.HTMLTankInfoHeader, NewValue = UserSettings.HTMLTankInfoHeader });
            _propertyFields.Add("HTMLShowMovementPics", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.HTMLShowMovementPics, NewValue = UserSettings.HTMLShowMovementPics });
            _propertyFields.Add("KillCountsShowTierTotals", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.KillCountsShowTierTotals, NewValue = UserSettings.KillCountsShowTierTotals });
            _propertyFields.Add("KillCountsShowRowTotals", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.KillCountsShowRowTotals, NewValue = UserSettings.KillCountsShowRowTotals });
            _propertyFields.Add("KillCountsShowColumnTotals", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.KillCountsShowColumnTotals, NewValue = UserSettings.KillCountsShowColumnTotals });
            _propertyFields.Add("DateFormat", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.DateFormat, NewValue = UserSettings.DateFormat });
            _propertyFields.Add("TimeStamp", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.TimeStamp, NewValue = UserSettings.TimeStamp });
            _propertyFields.Add("TimeFormat", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.TimeFormat, NewValue = UserSettings.TimeFormat });
            _propertyFields.Add("GroupLPT", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.GroupLPT, NewValue = UserSettings.GroupLPT });
            _propertyFields.Add("AllowvBAddictUpload", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AllowvBAddictUpload, NewValue = UserSettings.AllowvBAddictUpload });
            _propertyFields.Add("AllowvBAddictUploadDossier", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AllowvBAddictUploadDossier, NewValue = UserSettings.AllowvBAddictUploadDossier });
            _propertyFields.Add("AllowvBAddictUploadDossierBattleResult", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AllowvBAddictUploadDossierBattleResult, NewValue = UserSettings.AllowvBAddictUploadDossierBattleResult });
            _propertyFields.Add("AllowvBAddictUploadDossierBattleResultReplay", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AllowvBAddictUploadDossierBattleResultReplay, NewValue = UserSettings.AllowvBAddictUploadDossierBattleResultReplay });
            _propertyFields.Add("LastPlayedCompareQuota", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.LastPlayedCompareQuota, NewValue = UserSettings.LastPlayedCompareQuota });
            _propertyFields.Add("ColorPositive", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.ColorPositive, NewValue = UserSettings.ColorPositive });
            _propertyFields.Add("ColorNeutral", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.ColorNeutral, NewValue = UserSettings.ColorNeutral });
            _propertyFields.Add("ColorNegative", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.ColorNegative, NewValue = UserSettings.ColorNegative });
            _propertyFields.Add("ChartAppearance", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.ChartAppearance, NewValue = UserSettings.ChartAppearance });
            _propertyFields.Add("ChartPalette", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.ChartPalette, NewValue = UserSettings.ChartPalette });
            _propertyFields.Add("colorWNClass1", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass1, NewValue = UserSettings.colorWNClass1 });
            _propertyFields.Add("colorWNClass2", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass2, NewValue = UserSettings.colorWNClass2 });
            _propertyFields.Add("colorWNClass3", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass3, NewValue = UserSettings.colorWNClass3 });
            _propertyFields.Add("colorWNClass4", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass4, NewValue = UserSettings.colorWNClass4 });
            _propertyFields.Add("colorWNClass5", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass5, NewValue = UserSettings.colorWNClass5 });
            _propertyFields.Add("colorWNClass6", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass6, NewValue = UserSettings.colorWNClass6 });
            _propertyFields.Add("colorWNClass7", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass7, NewValue = UserSettings.colorWNClass7 });
            _propertyFields.Add("colorWNClass8", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass8, NewValue = UserSettings.colorWNClass8 });
            _propertyFields.Add("colorWNClass9", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.colorWNClass9, NewValue = UserSettings.colorWNClass9 });
            _propertyFields.Add("Cloud_Allow", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.Cloud_Allow, NewValue = UserSettings.Cloud_Allow });
            _propertyFields.Add("Cloud_Path", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.Cloud_Path, NewValue = UserSettings.Cloud_Path });
            _propertyFields.Add("TimeAdjustment", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.TimeAdjustment, NewValue = UserSettings.TimeAdjustment });
            _propertyFields.Add("TopMinPlayed", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.TopMinPlayed, NewValue = UserSettings.TopMinPlayed });
            _propertyFields.Add("LangID", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.LangID, NewValue = UserSettings.LangID });
            //_propertyFields.Add("RatingSystem", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.RatingSystem, NewValue = UserSettings.RatingSystem });

            _propertyFields.Add("AutoCreateSession", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoCreateSession, NewValue = UserSettings.AutoCreateSession });
            _propertyFields.Add("AutoSessionOnStartUp", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionOnStartUp, NewValue = UserSettings.AutoSessionOnStartUp });
            _propertyFields.Add("AutoSessionOnStartUpMessage", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionOnStartUpMessage, NewValue = UserSettings.AutoSessionOnStartUpMessage });
            _propertyFields.Add("AutoSessionXBattles", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXBattles, NewValue = UserSettings.AutoSessionXBattles });
            _propertyFields.Add("AutoSessionXBattlesMessage", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXBattlesMessage, NewValue = UserSettings.AutoSessionXBattlesMessage });
            _propertyFields.Add("AutoSessionXBattlesValue", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXBattlesValue, NewValue = UserSettings.AutoSessionXBattlesValue });
            _propertyFields.Add("AutoSessionXHours", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXHours, NewValue = UserSettings.AutoSessionXHours });
            _propertyFields.Add("AutoSessionXHoursMessage", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXHoursMessage, NewValue = UserSettings.AutoSessionXHoursMessage });
            _propertyFields.Add("AutoSessionXHoursValue", new PropertyFields() { FieldType = typeof(string), OldValue = UserSettings.AutoSessionXHoursValue, NewValue = UserSettings.AutoSessionXHoursValue });


            FTPDetails ftpDetails = new FTPDetails();
            _propertyFields.Add("FTP_AllowFTP", new PropertyFields() { FieldType = typeof(string), OldValue = ftpDetails.AllowFTP, NewValue = ftpDetails.AllowFTP });
            _propertyFields.Add("FTP_Host", new PropertyFields() { FieldType = typeof(string), OldValue = ftpDetails.Host, NewValue = ftpDetails.Host });
            _propertyFields.Add("FTP_UserID", new PropertyFields() { FieldType = typeof(string), OldValue = ftpDetails.UserID, NewValue = ftpDetails.UserID });
            _propertyFields.Add("FTP_UserPWD", new PropertyFields() { FieldType = typeof(string), OldValue = ftpDetails.UserPWD, NewValue = ftpDetails.UserPWD });



            InitializeComponent();
            treeList1.ExpandAll();
            treeList1.Columns[0].SortOrder = SortOrder.None;

            if (page == "LastPlayedTanks")
                SetNodeFocus(Translations.TranslationGet("STR_RECENTBATTLES", "DE", "Recent Battles"));
            else if (page == "KillCounts")
                SetNodeFocus(Translations.TranslationGet("STR_KILLCOUNTS", "DE", "Kill Counts"));
            else if (page == "CustomGroupings")
                SetNodeFocus("Custom Grouping");
            else if (page == "Players")
                SetNodeFocus(Translations.TranslationGet("STR_PLAYERS", "DE", "Players"));
            else
            {

            }

            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
        }

        private void SetNodeFocus(string nodeText)
        {
            TreeListOperationFindNodeByText op;

            op = new TreeListOperationFindNodeByText(treeListColumn1, nodeText);

            treeList1.NodesIterator.DoOperation(op);

            if (op.Node != null)
                treeList1.FocusedNode = op.Node;
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

            ClearControls();
            if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_PLAYERS", "DE", "Players"))
            {
                ctxPlayerSetup ps = new ctxPlayerSetup();
                //ps.Width = panelControl1.Width;
                ps.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ps);
                _topicID = "120";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_DOSFILESHARING", "DE", "Dossier File Sharing"))
            {
                ctxFTPSetup ftp = new ctxFTPSetup();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "140";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_DOSMONITOR", "DE", "Dossier Monitor"))
            {
                ctxDossierMonitor ftp = new ctxDossierMonitor();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "110";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_VERSION", "DE", "Version"))
            {
                ctxVersion ftp = new ctxVersion();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "150";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_KILLCOUNTS", "DE", "Kill Counts"))
            {
                ctxKillCounts ftp = new ctxKillCounts();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "160";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_RECENTBATTLES", "DE", "Recent Battles"))
            {
                ctxLastPlayedGames ftp = new ctxLastPlayedGames();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "170";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_DISPLAY", "DE", "Display"))
            {
                ctxSetup ftp = new ctxSetup();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "130";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_ONLINEANALYZER", "DE", "vBAddict.net"))
            {
                ctxVBAddict ftp = new ctxVBAddict();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "145";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_CHARTS", "DE", "Charts"))
            {
                ctxChartColors ftp = new ctxChartColors();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "180";
            }
            else if (e.Node.GetDisplayText(0) == Translations.TranslationGet("STR_REPORTS", "DE", "Reports") && !e.Node.HasChildren)
            {
                ctxReportColors ftp = new ctxReportColors();
                ftp.Dock = DockStyle.Fill;
                panelControl1.Controls.Add(ftp);
                _topicID = "180";
            }
            else
                _topicID = "100";


            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
        }

        private void ClearControls()
        {
            for (int ix = panelControl1.Controls.Count - 1; ix >= 0; ix--)
                panelControl1.Controls[ix].Dispose();
        }


        internal class TreeListOperationFindNodeByText : TreeListOperation
        {

            private TreeListNode foundNode;

            private TreeListColumn column;

            private string substr;

            public TreeListOperationFindNodeByText(TreeListColumn column, string substr)
            {

                this.foundNode = null;

                this.column = column;

                this.substr = substr;

            }

            public override void Execute(TreeListNode node)
            {

                string s = node[column].ToString();

                if (s.StartsWith(substr))

                    this.foundNode = node;

            }

            public override bool NeedsVisitChildren(TreeListNode node) { return foundNode == null; }

            public TreeListNode Node { get { return foundNode; } }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing) return;


            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                Application.Exit();
                return;
            }

            using (PlayerListing pl = new PlayerListing(new MessageQueue()))
            {
                if (pl.Count > 0)
                {
                    var dirtyFields = from x in _propertyFields
                                      where x.Value.IsDirty == true
                                      select x;
                    if (dirtyFields.Count() > 0)
                    {
                        if (DialogResult.No == DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_SSAVENOTICE", "DE", "Changes you made to Setup would not be saved! " + Environment.NewLine + Environment.NewLine + "Would you like to close Setup window anyway?"), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                        {
                            e.Cancel = true;
                        }
                    }                       
                }
                else
                {
                    if (DialogResult.No == DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_SNOPLAYERNOTICE", "DE", "No players has been selected to monitor. Do you want to continue?"), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
                    {
                        e.Cancel = true;
                        SetNodeFocus("Players");
                    }
                }

            }
        }

        private void frmSetup_Load(object sender, EventArgs e)
        {
            this.Text = Translations.TranslationGet("WNDCAPTION_SETUP", "DE", "Setup");
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");
            BTN_OK.Text = Translations.TranslationGet("BTN_OK", "DE", "Ok");

            TranslateMenu(treeList1.Nodes);
        }

        private void TranslateMenu(TreeListNodes parent)
        {
            foreach (TreeListNode item in parent)
            {
                switch (item.GetDisplayText(0))
                {
                    case "Setup":
                        item.SetValue(0, Translations.TranslationGet("WNDCAPTION_SETUP", "DE", "Setup"));
                        break;
                    case "Players":
                        item.SetValue(0, Translations.TranslationGet("STR_PLAYERS", "DE", "Players"));
                        break;
                    case "Dossier Monitor":
                        item.SetValue(0, Translations.TranslationGet("STR_DOSMONITOR", "DE", "Dossier Monitor"));
                        break;
                    case "Display":
                        item.SetValue(0, Translations.TranslationGet("STR_DISPLAY", "DE", "Display"));
                        break;
                    case "Dossier File Sharing":
                        item.SetValue(0, Translations.TranslationGet("STR_DOSFILESHARING", "DE", "Dossier File Sharing"));
                        break;
                    case "Kill Counts":
                        item.SetValue(0, Translations.TranslationGet("STR_KILLCOUNTS", "DE", "Kill Counts"));
                        break;
                    case "Recent Battles":
                        item.SetValue(0, Translations.TranslationGet("STR_RECENTBATTLES", "DE", "Recent Battles"));
                        break;
                    case "Charts":
                        item.SetValue(0, Translations.TranslationGet("STR_CHARTS", "DE", "Charts"));
                        break;
                    case "Reports":
                        item.SetValue(0, Translations.TranslationGet("STR_REPORTS", "DE", "Reports"));
                        break;
                    case "Colors":
                        item.SetValue(0, Translations.TranslationGet("STR_COLORS", "DE", "Colors"));
                        break;
                    case "vBAddict.net":
                        item.SetValue(0, Translations.TranslationGet("STR_ONLINEANALYZER", "DE", "vBAddict.net Online Analyzer"));
                        break;

                    default:
                        break;
                }
                if (item.HasChildren)
                {
                    TranslateMenu(item.Nodes);
                }
            }
        }

        private void frmSetup_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, _topicID);
            e.Cancel = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }


        private void SaveSettings()
        {
            var dirtyFields = from x in _propertyFields
                              where x.Value.IsDirty == true && !x.Key.Contains("FTP_")
                              select x;
            foreach (var item in dirtyFields)
            {
                switch (item.Key)
                {
                    case "StartMonOnStartUp":
                        UserSettings.StartMonOnStartUp = Convert.ToBoolean(item.Value.NewValue);

                        break;
                    case "LaunchWithWindows":
                        UserSettings.LaunchWithWindows = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "MinimiseonStartup":
                        UserSettings.MinimiseonStartup = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "MinimiseToTray":
                        UserSettings.MinimiseToTray = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "LastPlayedCompare":
                        UserSettings.LastPlayedCompare = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "AllowVersionCheck":
                        UserSettings.AllowVersionCheck = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "VersionCounter":
                        UserSettings.VersionCounter = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "SystemFont":
                        UserSettings.SystemFont = Convert.ToString(item.Value.NewValue);
                        break;
                    case "HTMLCellFont":
                        UserSettings.HTMLCellFont = Convert.ToString(item.Value.NewValue);
                        break;
                    case "HTMLHeaderFont":
                        UserSettings.HTMLHeaderFont = Convert.ToString(item.Value.NewValue);
                        break;
                    case "HTMLTankInfoHeader":
                        UserSettings.HTMLTankInfoHeader = Convert.ToString(item.Value.NewValue);
                        break;
                    case "HTMLShowMovementPics":
                        UserSettings.HTMLShowMovementPics = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "KillCountsShowTierTotals":
                        UserSettings.KillCountsShowTierTotals = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "KillCountsShowRowTotals":
                        UserSettings.KillCountsShowRowTotals = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "KillCountsShowColumnTotals":
                        UserSettings.KillCountsShowColumnTotals = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "DateFormat":
                        UserSettings.DateFormat = Convert.ToString(item.Value.NewValue);
                        break;
                    case "TimeStamp":
                        UserSettings.TimeStamp = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "TimeFormat":
                        UserSettings.TimeFormat = Convert.ToString(item.Value.NewValue);
                        break;
                    case "GroupLPT":
                        UserSettings.GroupLPT = Convert.ToString(item.Value.NewValue);
                        break;
                    case "AllowvBAddictUpload":
                        UserSettings.AllowvBAddictUpload = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AllowvBAddictUploadDossier":
                        UserSettings.AllowvBAddictUploadDossier = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AllowvBAddictUploadDossierBattleResult":
                        UserSettings.AllowvBAddictUploadDossierBattleResult = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AllowvBAddictUploadDossierBattleResultReplay":
                        UserSettings.AllowvBAddictUploadDossierBattleResultReplay = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "LastPlayedCompareQuota":
                        UserSettings.LastPlayedCompareQuota = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "ColorPositive":
                        UserSettings.ColorPositive = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "ColorNeutral":
                        UserSettings.ColorNeutral = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "ColorNegative":
                        UserSettings.ColorNegative = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "ChartAppearance":
                        UserSettings.ChartAppearance = item.Value.NewValue.ToString();
                        break;
                    case "ChartPalette":
                        UserSettings.ChartPalette = item.Value.NewValue.ToString();
                        break;
                    case "colorWNClass1":
                        UserSettings.colorWNClass1 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass2":
                        UserSettings.colorWNClass2 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass3":
                        UserSettings.colorWNClass3 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass4":
                        UserSettings.colorWNClass4 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass5":
                        UserSettings.colorWNClass5 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass6":
                        UserSettings.colorWNClass6 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass7":
                        UserSettings.colorWNClass7 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass8":
                        UserSettings.colorWNClass8 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "colorWNClass9":
                        UserSettings.colorWNClass9 = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "Cloud_Allow":
                        UserSettings.Cloud_Allow = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "Cloud_Path":
                        UserSettings.Cloud_Path = Convert.ToString(item.Value.NewValue);
                        break;
                    case "TimeAdjustment":
                        UserSettings.TimeAdjustment = Convert.ToDouble(item.Value.NewValue);
                        break;
                    case "TopMinPlayed":
                        UserSettings.TopMinPlayed = Convert.ToInt32(item.Value.NewValue);
                        break;
                    //case "RatingSystem":
                    //    UserSettings.RatingSystem = Convert.ToString(item.Value.NewValue);
                    //    break;
                    case "LangID":
                        UserSettings.LangID = Convert.ToString(item.Value.NewValue);
                        DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_LANGIDNOTIFY", "DE", "Please note that the language settings would take effect after application is restarted."), "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case "AutoCreateSession":
                        UserSettings.AutoCreateSession = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionOnStartUp":
                        UserSettings.AutoSessionOnStartUp = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionOnStartUpMessage":
                        UserSettings.AutoSessionOnStartUpMessage = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionXBattles":
                        UserSettings.AutoSessionXBattles = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionXBattlesMessage":
                        UserSettings.AutoSessionXBattlesMessage = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionXBattlesValue":
                        UserSettings.AutoSessionXBattlesValue = Convert.ToInt32(item.Value.NewValue);
                        break;
                    case "AutoSessionXHours":
                        UserSettings.AutoSessionXHours = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionXHoursMessage":
                        UserSettings.AutoSessionXHoursMessage = Convert.ToBoolean(item.Value.NewValue);
                        break;
                    case "AutoSessionXHoursValue":
                        UserSettings.AutoSessionXHoursValue = Convert.ToInt32(item.Value.NewValue);
                        break;
                    default:
                        break;
                }
                item.Value.OldValue = item.Value.NewValue;
            }
            
            var dirtyFieldsFTP = from x in _propertyFields
                                 where x.Value.IsDirty == true && x.Key.Contains("FTP_")
                                 select x;
            if (dirtyFieldsFTP.Count() > 0)
            {
                FTPDetails ftpDetails = new FTPDetails();
                ftpDetails.AllowFTP = Convert.ToBoolean(_propertyFields["FTP_AllowFTP"].NewValue);
                ftpDetails.Host = Convert.ToString(_propertyFields["FTP_Host"].NewValue);
                ftpDetails.UserID = Convert.ToString(_propertyFields["FTP_UserID"].NewValue);
                ftpDetails.UserPWD = Convert.ToString(_propertyFields["FTP_UserPWD"].NewValue);

                _propertyFields["FTP_AllowFTP"].OldValue = _propertyFields["FTP_AllowFTP"].NewValue;
                _propertyFields["FTP_Host"].OldValue = _propertyFields["FTP_Host"].NewValue;
                _propertyFields["FTP_UserID"].OldValue = _propertyFields["FTP_UserID"].NewValue;
                _propertyFields["FTP_UserPWD"].OldValue = _propertyFields["FTP_UserPWD"].NewValue;
                ftpDetails.Save();
            }

        }
    }
}