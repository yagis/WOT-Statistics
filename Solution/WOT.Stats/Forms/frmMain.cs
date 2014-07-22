using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using WOTStatistics.Core;
using WOTStatistics.SQLite;

namespace WOT.Stats
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmMain : DevExpress.XtraEditors.XtraForm
    {

       
            
        //private bool _HasPlayerError = false;
        MessageQueue _message = new MessageQueue();
        WebBrowser _browser = new WebBrowser();
        ChartControl _chart = new ChartControl();
        private string _currentSelectedChart = "";
        private string _currentChartGroup = null;
        private string _tankInfo_SelectedTankID = "";
        private readonly ToolTipController toolTipController1 = new ToolTipController();
        private string _topicID = "100";
        private bool _wait = false;

        string _currentPlayer = "";
        string _currentComparePlayer = "";
        string _currentPage = "";
        string _battleModeHolder = "All";

        private const string _title = "World Of Tanks Statistics";

        private WOTStats _compareStatA = null;
        private WOTStats _compareStatB = null;

        private string _compareTankIDA = "";
        private string _compareTankIDB = "";



        Dictionary<string, DossierManager> dictPlayers = new Dictionary<string, DossierManager>();
        Dictionary<string, WOTCompare> _currentStatsFile = new Dictionary<string, WOTCompare>();

        public void SetTextOnSplashScreen(string sText)
        {

            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(SplashS), true, true);

            }
            catch (Exception ex)
            {
                WOTStatistics.Core.WOTHelper.AddToLog("Error with Splash: " + ex.Message);
            }


            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SendCommand(SplashS.SplashScreenCommand.SetCurrentAction, sText);
            }
            catch (Exception ex)
            {
                WOTStatistics.Core.WOTHelper.AddToLog("Error with Splash: " + ex.Message);
            }

        }


        public frmMain()
        {
            _message.Add("frmMain loaded");



            SetTextOnSplashScreen("Initializing Components");

            WOTHelper.SetInternetZone();
            InitializeComponent();
            repositoryItemRadioGroup2.SelectedIndexChanged += new EventHandler(repositoryItemRadioGroup2_SelectedIndexChanged);
            repositoryItemRecentBattlesDisplayList.SelectedIndexChanged += new EventHandler(repositoryItemRecentBattlesDisplayList_SelectedIndexChanged);

            _chart.ObjectHotTracked += new HotTrackEventHandler(_chart_ObjectHotTracked);
            _browser.ObjectForScripting = this;

            _message = new MessageQueue();
            _message.ItemAdded += new MessageQueue_OnAdd(NewMessage);

#if DEBUG
            _browser.IsWebBrowserContextMenuEnabled = true;
#else
            _browser.IsWebBrowserContextMenuEnabled = false;
#endif



            //dockPanel3.Visibility = DockVisibility.Hidden;
            //dockPanel4.Visibility = DockVisibility.Hidden;
            //dockPanelGraph.Visibility = DockVisibility.Hidden;
            //dockPanel2.Visibility = DockVisibility.Hidden;

            bar6.Visible = false;
            bar6.Offset = 0;
            barLastPlayedGames.Visible = false;
            barLastPlayedGames.Offset = 0;
            helpProvider1.HelpNamespace = Path.Combine(WOTHelper.GetEXEPath(), "Help", "WoT_Stats.chm");
            //ScriptWrapper.Initialise(WOTHelper.GetCustomScript());
            using (PlayerListing players = new PlayerListing(_message))
            {
                if (players.Count() > 0)
                {
                    
                    string errorMessage = "";
                    foreach (KeyValuePair<string, Player> player in players)
                    {
                        SetTextOnSplashScreen("Initializing Dossier of " + player.Value.PlayerID);
                        if (!File.Exists(player.Value.WatchFile))
                        {
                            string dir = player.Value.WatchFile.Remove(player.Value.WatchFile.LastIndexOf('\\'));
                            if (Directory.Exists(dir))
                            {
                                foreach (FileInfo item in new DirectoryInfo(dir).GetFiles().Where(x => x.Extension.ToLower() == ".dat"))
                                {
                                    if (WOTHelper.PlayerIdFromDatFile(item.Name) == player.Value.PlayerID)
                                    {
                                        player.Value.WatchFile = item.FullName;
                                        players.SetPlayerWatchFile(player.Value.PlayerID, item.FullName);
                                        players.Save();
                                        break;
                                    }
                                }
                            }
                        }

                        if (File.Exists(player.Value.WatchFile))
                        {
                            DossierManager dm = new DossierManager(player.Value.PlayerID, player.Value.WatchFile, player.Value.Monitor, _message, this);
                            dm.CurrentFileChange += DossierFileChanged;
                            dictPlayers.Add(player.Value.PlayerID.Replace("_", "*"), dm);

                            //try
                            //{
                                _currentStatsFile.Add(player.Value.PlayerID.Replace("_", "*"), new WOTCompare(new Dossier(dm.GetFileB(), dm.GetPlayerName, _message).GetStats(), new Dossier(dm.GetFileA(), dm.GetPlayerName, _message).GetStats()));
                                dm.RefreshDossier();
                            //}
                            //catch (Exception ex)
                            //{

                            //    _message.Add("Error: cannot refresh dossier file. - " + ex.Message);
                            //}

                            //BarButtonItem buttonOpen = new BarButtonItem(barManager1, player.Value.PlayerID);
                            //buttonOpen.ItemClick += new ItemClickEventHandler(buttonOpen_ItemClick);
                            //buttonOpen.Name = player.Value.PlayerID;
                            //barCompare.AddItem(buttonOpen);

                            CreatePlayerMenu(player.Value.PlayerID);

                            SetTextOnSplashScreen("Initializing Monitor");

                            if (UserSettings.StartMonOnStartUp == true)
                            {
                                dm.StartDossierWatch();
                                barStaticItem1.Glyph = WOTStatistics.Stats.Properties.Resources.bullet_square_green;
                                barStaticItem1.Caption = "Monitor Status : Running";
                            }
                            else
                            {
                                barStaticItem1.Glyph = WOTStatistics.Stats.Properties.Resources.bullet_square_red;
                                barStaticItem1.Caption = "Monitor Status : Stopped";
                            }
                        }
                        else
                        {

                           // _HasPlayerError = true;
                            if (errorMessage.Length > 1)
                                errorMessage += Environment.NewLine + Environment.NewLine;

                            errorMessage += Translations.TranslationGet("STR_PLAYERERROR", "DE", "Dossier file not found for player : " + player.Value.PlayerID + Environment.NewLine + "Please check the dossier file location on the edit player screen in setup." + Environment.NewLine + "Player not loaded.").Replace("{playerid}", player.Value.PlayerID);
                        }

                        if (errorMessage.Length > 1)
                            DevExpress.XtraEditors.XtraMessageBox.Show(errorMessage, "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch {}
            
           // if (splashScreenManagerWaitForm.IsSplashFormVisible)
                //splashScreenManagerWaitForm.CloseWaitForm();
             _message.Add("frmMain DONE");

             try
             {
                 DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
             }
             catch { }
           
        }



        public void Redirect(string tankid)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            _currentPage = "TankInfo";
            
            //_battleModeHolder = UserSettings.BattleMode;
            //UserSettings.BattleMode = "All";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            CreateWebPage("TankInfo", _currentPlayer);
            _topicID = "240";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);

            navBarControl1.Groups[_currentPlayer].SelectedLink = navBarControl1.Groups[_currentPlayer].ItemLinks[3];

            NavBarGroup group = navBarControl2.Groups["group_" + tankid.Split('_')[0]];
            group.Expanded = true;

            foreach (NavBarItemLink item in group.ItemLinks)
            {
                if (item.ItemName == tankid)
                {
                    item.PerformClick();
                    navBarControl2.Groups["group_" + tankid.Split('_')[0]].SelectedLink = item;
                }
            }

            //UserSettings.BattleMode = _battleModeHolder;
            //barBattleMode.Enabled = false;

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }

        }

        void _chart_ObjectHotTracked(object sender, HotTrackEventArgs e)
        {
            SeriesPoint point = e.AdditionalObject as SeriesPoint;

            if (point != null)
            {



                string s = String.Format("Name: {0}{1}Value: {2}{1}Date: {3}", e.Object, Environment.NewLine, point.Values[0], point.DateTimeArgument.ToString(UserSettings.DateFormat));
                toolTipController1.ShowHint(s);
            }
            else
                toolTipController1.HideHint();
        }

        void NewMessage(object sender, MessageEventArgs e)
        {
            lastMessage.Caption = e.Value;
            _message.Remove(e.Key);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            barButtonRefresh.Caption = Translations.TranslationGet("STR_REFRESH", "DE", "Refresh");
            barButtonRefresh.SuperTip.Items.Clear();
            barButtonRefresh.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_REFRESH", "DE", "Refresh") + " (F5)" });// =   Translations.TranslationGet("STR_REFRESH", "DE", "Refresh");

            barButtonSelection.Caption = Translations.TranslationGet("STR_PERSELECT", "DE", "Period Selection");
            barButtonSelection.SuperTip.Items.Clear();
            barButtonSelection.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_PERSELECT", "DE", "Period Selection") + " (F6)" });// =   Translations.TranslationGet("STR_REFRESH", "DE", "Refresh");

            STR_DOSMONITOR.Caption = Translations.TranslationGet("STR_DOSMONITOR", "DE", "Dossier Monitor");
            STR_DMSTOP.Caption = Translations.TranslationGet("STR_DMSTOP", "DE", "Stop");
            STR_DMSTART.Caption = Translations.TranslationGet("STR_DMSTART", "DE", "Start");
            STR_COMPARE.Caption = Translations.TranslationGet("STR_COMPARE", "DE", "Compare");
            STR_ONLINEANALYZER.Caption = Translations.TranslationGet("STR_ONLINEANALYZER", "DE", "Online Analyzer");

            STR_SETUP.Caption = Translations.TranslationGet("STR_SETUP", "DE", "Setup");
            STR_SETUP.SuperTip.Items.Clear();
            STR_SETUP.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_SETUP", "DE", "Setup") + " (F8)" });// =   Translations.TranslationGet("STR_REFRESH", "DE", "Refresh");

            STR_HELP.Caption = Translations.TranslationGet("STR_HELP", "DE", "Help");
            subSTR_HELP.Caption = Translations.TranslationGet("STR_HELP", "DE", "Help");
            STR_IMPNOTICE.Caption = Translations.TranslationGet("STR_IMPNOTICE", "DE", "Important Notice");
            STR_CREDITS.Caption = Translations.TranslationGet("STR_CREDITS", "DE", "Credits");
            STR_DEFGROUPINGS.Caption = Translations.TranslationGet("STR_DEFGROUPINGS", "DE", "Define Groupings");
            STR_DEFCHARTS.Caption = Translations.TranslationGet("STR_DEFCHARTS", "DE", "Define Charts");
            //STR_RB_CLEAR.Caption = Translations.TranslationGet("STR_RB_CLEAR", "DE", "Start New Session");
            STR_RB_CLEAR.Caption = Translations.TranslationGet("STR_RB_NEWSESSION", "DE", "Start New Session");
            STR_RB_DISPLAYLIST.Caption = Translations.TranslationGet("STR_RB_DISPLAYLIST", "DE", "Display List");
            STR_RB_GROUPING.Caption = Translations.TranslationGet("STR_RB_GROUPING", "DE", "Grouping");
            BTN_CLOSE.Caption = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");
            dockPanel1.Text = Translations.TranslationGet("STR_PLAYERS", "DE", "Players");
            dockPanel2.Text = Translations.TranslationGet("STR_TANKS", "DE", "Tanks");
            dockPanelGraph.Text = Translations.TranslationGet("STR_CHARTS", "DE", "Charts");

            STR_CAP_BATTLEMODE.Caption = Translations.TranslationGet("STR_CAP_BATTLEMODE", "DE", "Battle Mode");
            //STR_CAP_RATING.Caption = Translations.TranslationGet("STR_CAP_RATING", "DE", "Rating");


            repositoryItemRadioGroup2.Items[0].Description = Translations.TranslationGet("RADIO_RB_NONE", "DE", "None");
            repositoryItemRadioGroup2.Items[1].Description = Translations.TranslationGet("RADIO_RB_TANK", "DE", "By Tank");
            repositoryItemRadioGroup2.Items[2].Description = Translations.TranslationGet("RADIO_RB_TIER", "DE", "By Tier");
            repositoryItemRadioGroup2.Items[3].Description = Translations.TranslationGet("RADIO_RB_COUTNRY", "DE", "By Country");

            STR_WOTSTATSLINK.Caption = Translations.TranslationGet("STR_WOTSTATSLINK", "DE", "WOT Statistics Website");
            STR_WOTSTATSLINK.SuperTip.Items.Clear();
            STR_WOTSTATSLINK.SuperTip.Items.Add(new ToolTipItem() { Text = ApplicationSettings.WOTStatsWebLink });

            STR_EUFORMLINK.Caption = Translations.TranslationGet("STR_EUFORMLINK", "DE", "EU Forum");
            STR_EUFORMLINK.SuperTip.Items.Clear();
            STR_EUFORMLINK.SuperTip.Items.Add(new ToolTipItem() { Text = ApplicationSettings.EUForumWebLink });


            barButRBMoveFirst.SuperTip.Items.Clear();
            barButRBMoveFirst.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_RB_MOVEFIRST", "DE", "First") });
            barButRBMovePrevious.SuperTip.Items.Clear();
            barButRBMovePrevious.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_RB_MOVEPREVIOUS", "DE", "Previous") });
            barButRBMoveNext.SuperTip.Items.Clear();
            barButRBMoveNext.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_RB_MOVENEXT", "DE", "Next") });
            barButRBMoveLast.SuperTip.Items.Clear();
            barButRBMoveLast.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_RB_MOVELAST", "DE", "Last") });
            barButRBMoveTo.SuperTip.Items.Clear();
            barButRBMoveTo.SuperTip.Items.Add(new ToolTipItem() { Text = Translations.TranslationGet("STR_RB_MOVEGOTO", "DE", "Go To") });
            repButRBMoveTo.KeyPress += new KeyPressEventHandler(repButRBMoveTo_KeyPress);

            _browser.Dock = DockStyle.Fill;
            panelControl1.Controls.Add(_browser);
            try
            {
                navBarControl1.Groups[0].Expanded = true;
                navBarControl1.Groups[0].ItemLinks[0].PerformClick();
                navBarControl1.Groups[0].SelectedLink = navBarControl1.Groups[0].ItemLinks[0];
            }
            catch
            {
            }

            try
            {
                if (UserSettings.AutoCreateSession)
                {
                    if (UserSettings.AutoSessionOnStartUp)
                    {
                        RecentBattles lb = new RecentBattles(_currentPlayer, _message);
                        string sessionID = lb.GetSession();
                       
                        //if (!lb.IsEmptySession(sessionID))
                        //{

                            if (UserSettings.AutoSessionOnStartUpMessage)
                            {
                                if (DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_NEWSESSIONNOTIFY", "DE", "Would you like to create a new recent battles session."), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    UserSettings.ViewSessionID = lb.NewSession();
                                    UserSettings.RecentBattlesCurrentSession = 0;
                                }
                            }
                            else
                            {
                                UserSettings.ViewSessionID = lb.NewSession();
                                UserSettings.RecentBattlesCurrentSession = 0;
                            } 
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                _message.Add(ex.Message);
            }
            finally
            {
                       try
                       {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                splashScreenManagerWaitForm.CloseWaitForm();
            }
            catch { }
            }


        }

        void repButRBMoveTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > 31 && (e.KeyChar < '0' || e.KeyChar > '9'))
                e.Handled = true;
        }

        void buttonOpen_ItemClick(object sender, ItemClickEventArgs e)
        {
            _currentPage = "Compare";
            _topicID = "300";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            if (panelControl1.Controls.Contains(_chart))
            {
                panelControl1.Controls.Remove(_chart);
                panelControl1.Controls.Add(_browser);
                _browser.Dock = DockStyle.Fill;
            }

            WOTCompare wotA;
            _currentStatsFile.TryGetValue(_currentPlayer.Replace("_", "*"), out wotA);
            WOTCompare wotB;
            _currentComparePlayer = e.Item.Name;
            _currentStatsFile.TryGetValue(e.Item.Name.Replace("_", "*"), out wotB);
            FillCompare(wotA.WOTCurrent, wotB.WOTCurrent);
            dockPanel3.Text = _currentPlayer;
            dockPanel3.Visible = true;
            dockPanel4.Text = e.Item.Name;
            dockPanel4.Visible = true;
            dockPanel2.Visible = false;
            dockPanel1.Visible = false;
            dockPanelGraph.Visible = false;
            bar3.Visible = false;
            bar3.Offset = 0;
            barLastPlayedGames.Visible = false;
            barLastPlayedGames.Offset = 0;

            bar6.Visible = true;
            bar6.Offset = 0;
            bar5.Visible = false;
            bar5.Offset = 0;
            try
            {
                NavBarGroup groupA = navBarControl3.ActiveGroup;
                groupA.ItemLinks[0].PerformClick();
            }
            catch { }

            try
            {
                NavBarGroup groupB = navBarControl4.ActiveGroup;
                groupB.ItemLinks[0].PerformClick();
            }
            catch { }
        }

        private void CreatePlayerMenu(string playerID)
        {

            NavBarGroup group = new NavBarGroup(playerID) { Name = playerID };

            navBarControl1.Groups.Add(group);
            NavBarItem globalItem = new NavBarItem(Translations.TranslationGet("STR_OVERALL", "DE", "Overall")) { Tag = playerID };
            globalItem.LinkClicked += globalItem_LinkClicked;
            group.ItemLinks.Add(globalItem);

            NavBarItem tankItem = new NavBarItem(Translations.TranslationGet("STR_TANKS", "DE", "Tanks")) { Tag = playerID };
            tankItem.LinkClicked += tankItem_LinkClicked;
            group.ItemLinks.Add(tankItem);

            NavBarItem KillCountsItem = new NavBarItem(Translations.TranslationGet("STR_KILLCOUNTS", "DE", "Kill Counts")) { Tag = playerID };
            KillCountsItem.LinkClicked += KillCountsItem_LinkClicked;
            group.ItemLinks.Add(KillCountsItem);

            NavBarItem TankStatsItem = new NavBarItem(Translations.TranslationGet("STR_TANKINFO", "DE", "Tank Info")) { Name = "mnuTankInfo", Tag = playerID };
            TankStatsItem.LinkClicked += TankStatsItem_LinkClicked;
            group.ItemLinks.Add(TankStatsItem);

            NavBarItem KillSummaryItem = new NavBarItem(Translations.TranslationGet("STR_KILLSUMMARY", "DE", "Kill Summary")) { Tag = playerID };
            KillSummaryItem.LinkClicked += KillSummaryItem_LinkClicked;
            group.ItemLinks.Add(KillSummaryItem);

            NavBarItem CustomGroupingItem = new NavBarItem(Translations.TranslationGet("STR_CUSTGROUPINGS", "DE", "Custom Groupings")) { Tag = playerID };
            CustomGroupingItem.LinkClicked += CustomGroupingItem_LinkClicked;
            group.ItemLinks.Add(CustomGroupingItem);

            NavBarItem LastPlayedGamedItem = new NavBarItem(Translations.TranslationGet("STR_RECENTBATTLES", "DE", "Recent Battles")) { Tag = playerID };
            LastPlayedGamedItem.LinkClicked += LastPlayedGamedItem_LinkClicked;
            group.ItemLinks.Add(LastPlayedGamedItem);

            if (ApplicationSettings.AchievementReportVisible)
            {
                NavBarItem AchievementsItem = new NavBarItem(Translations.TranslationGet("STR_ACHIEVPROG", "DE", "Achievement Progress")) { Tag = playerID };
                AchievementsItem.LinkClicked += AchievementsItem_LinkClicked;
                group.ItemLinks.Add(AchievementsItem);
            }

            NavBarItem ChartsItem = new NavBarItem(Translations.TranslationGet("STR_CHARTS", "DE", "Charts")) { Tag = playerID };
            ChartsItem.LinkClicked += ChartsItem_LinkClicked;
            group.ItemLinks.Add(ChartsItem);

            //NavBarItem zedChart = new NavBarItem(Translations.TranslationGet("STR_CHARTS1", "DE", "Zed Charts")) { Tag = playerID };
            //zedChart.LinkClicked += new NavBarLinkEventHandler(zedChartsItem_LinkClicked);
            //group.ItemLinks.Add(zedChart);
        }

        //private void zedChartsItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        //{
        //    ZedGraphContainer f = new ZedGraphContainer();
        //    f.Dock = DockStyle.Fill;
        //    panelControl1.Controls.Clear();
        //    panelControl1.Controls.Add(f);
        //    f.Show();

        //}
        void AchievementsItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

            

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            _currentPage = "Achievements";
            CreateTitle();
            CreateWebPage("Achievements", name);
            _topicID = "275";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void ChartsItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

           

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            _currentPage = "Charts";
            CreateTitle();
            CreateWebPage("Charts", name);
            _topicID = "280";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void LastPlayedGamedItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

           

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            _currentPage = "LastPlayedTanks";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            CreateTitle();
            CreateWebPage("LastPlayedTanks", name);
            _topicID = "270";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            barBattleMode.Enabled = true;

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }

        }

        void CustomGroupingItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

           

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            _currentPage = "CustomGroupings";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            CreateWebPage("CustomGroupings", name);
            _topicID = "260";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            barBattleMode.Enabled = true;

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }

        }

        void KillSummaryItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            
              string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);

            

            _currentPage = "KillSummary";
            _battleModeHolder = UserSettings.BattleMode;
            UserSettings.BattleMode = "All";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);
           
            
            CreateWebPage("KillSummary", name);
            _topicID = "250";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            UserSettings.BattleMode = _battleModeHolder;
            barBattleMode.Enabled = false;
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }

        }

        void TankStatsItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);


            _currentPage = "TankInfo";
            //_battleModeHolder = UserSettings.BattleMode;
            //UserSettings.BattleMode = "All";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);

            CreateWebPage("TankInfo", name);
            _topicID = "240";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            //UserSettings.BattleMode = _battleModeHolder;
            //barBattleMode.Enabled = false;

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void KillCountsItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            _currentPage = "KillCounts";
            _battleModeHolder = UserSettings.BattleMode;
            UserSettings.BattleMode = "All";

            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);


            CreateWebPage("KillCounts", name);
            _topicID = "230";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            UserSettings.BattleMode = _battleModeHolder;
            barBattleMode.Enabled = false;
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void globalItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

           

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);
            _currentPage = "Global";

            CreateWebPage("Global", name);
            _topicID = "210";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            barBattleMode.Enabled = true;

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void tankItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

          

            string name = ((NavBarItem)sender).Tag.ToString();
            ResetPlayerVariables(name);
            _currentPage = "Tanks";
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            ReloadDossier(dm);
            CreateWebPage("Tanks", name);
            _topicID = "220";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
            barBattleMode.Enabled = true;
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        void ResetPlayerVariables(string name)
        {
            if (_currentPlayer != name)
            {
                _currentPlayer = name;
                _tankInfo_SelectedTankID = "";
            }
        }

        void DossierFileChanged(object sender, EventArgs e)
        {
            ReloadDossier(sender as DossierManager);
        }

        private void ReloadDossier(DossierManager sender)
        {
         try
            {
                if (dictPlayers.Count > 0)
                {
                    DossierManager dm = (DossierManager)sender;
                    Dossier fileB = new Dossier(dm.GetFileB(), dm.GetPlayerName, _message);
                    Dossier fileA = new Dossier(dm.GetFileA(), dm.GetPlayerName, _message);

                    _currentStatsFile[dm.GetPlayerID.Replace("_", "*")] = new WOTCompare(fileB.GetStats(), fileA.GetStats());

                    CreateWebPage(_currentPage, _currentPlayer);
                }
            }
            catch (Exception ex)
            {
                _message.Add("Error: cannot refresh dossier file. - " + ex.Message);
            }
        }
     

        private void CreateWebPage(string pageName, string playerName)
        {

            CreateTitle();
            if (pageName != "CustomGroupings")
            {
                bar3.Visible = false;
                bar3.Offset = 0; 
            }

            if (pageName != "TankInfo")
                dockPanel2.Visible = false;

            if (pageName != "Charts")
            {
                bar5.Visible = false;
                bar5.Offset = 0;
                dockPanelGraph.Visible = false;
            }

            
            if (pageName != "LastPlayedTanks")
            {
                barLastPlayedGames.Visible = false;
                barLastPlayedGames.Offset = 0;
            }
            //ClearControls();
            string[] plainHTML = { "Tanks", "Global", "KillCounts", "KillSummary", "LastPlayedTanks", "Achievements" };
            if (plainHTML.Contains(pageName))
            {
                if (panelControl1.Controls.Contains(_chart))
                {
                    panelControl1.Controls.Remove(_chart);
                    panelControl1.Controls.Add(_browser);
                    _browser.Dock = DockStyle.Fill;
                }

                if (pageName == "LastPlayedTanks")
                {
                    barLastPlayedGames.Visible = true;
                    barLastPlayedGames.Offset = 0;
                    barEditItem2.EditValue = UserSettings.GroupLPT;
                    barRecentBattlesView.EditValue = UserSettings.RecentBattlesCurrentSession.ToString();

                    if (barRecentBattlesView.EditValue.ToString() == "0")
                    {
                        RecentBattlesNavigations(true);
                        RecentBattlesNavigate("rsID = (select max(rsID) from Recentbattles_Session)");
                    }
                    else
                    {
                        RecentBattlesNavigations(false);
                        WriteHTMLDocument(pageName, playerName, null, null);
                    }


                    _browser.Focus();
                    return;
                }

                WriteHTMLDocument(pageName, playerName, null, null);
                _browser.Focus();
            }
            else
            {
                if (pageName == "TankInfo")
                {
                    if (panelControl1.Controls.Contains(_chart))
                    {
                        panelControl1.Controls.Remove(_chart);
                        panelControl1.Controls.Add(_browser);
                        _browser.Dock = DockStyle.Fill;
                    }

                    dockPanel2.Visible = true;
                    BuildTankInfo(playerName);
                    _browser.Focus();
                }
                else
                    if (pageName == "CustomGroupings")
                    {
                        if (panelControl1.Controls.Contains(_chart))
                        {
                            panelControl1.Controls.Remove(_chart);
                            panelControl1.Controls.Add(_browser);
                            _browser.Dock = DockStyle.Fill;
                        }

                        bar3.Visible = true;
                        bar3.Offset = 0;
                        BuildCustomGrouping(playerName);
                        _browser.Focus();
                    }
                    else if (pageName == "Charts")
                    {
                        if (panelControl1.Controls.Contains(_browser))
                        {
                            panelControl1.Controls.Remove(_browser);
                            panelControl1.Controls.Add(_chart);
                            _chart.Dock = DockStyle.Fill;
                        }

                        bar5.Visible = true;
                        bar5.Offset = 0;
                        dockPanelGraph.Visible = true;
                        BuildGraphForm(playerName);
                        _chart.Focus();
                    }
            }


        }

        private void BuildGraphForm(string playerName)
        {

            _chart.AppearanceNameSerializable = "Dark";

            navBarCharts.Items.Clear();
            navBarCharts.Groups.Clear();
            GraphsSettings gs = new GraphsSettings(_message);
            NavBarGroup group = null;
            NavBarItem groupItem = null;



            foreach (KeyValuePair<string, GraphFields> graph in gs.OrderBy(x => x.Value.StatsBase))
            {
                if (group == null)
                {
                    group = new NavBarGroup(graph.Value.StatsBase);
                    group.Name = "graph_" + graph.Value.StatsBase;
                    navBarCharts.Groups.Add(group);
                }

                if (group.Name != "graph_" + graph.Value.StatsBase)
                {
                    group = new NavBarGroup(graph.Value.StatsBase);
                    group.Name = "graph_" + graph.Value.StatsBase;
                    navBarCharts.Groups.Add(group);

                    groupItem = new NavBarItem(graph.Value.Caption);
                    groupItem.Name = graph.Key;

                    group.ItemLinks.Add(groupItem);
                }
                else
                {
                    groupItem = new NavBarItem(graph.Value.Caption);
                    groupItem.Name = graph.Key;

                    group.ItemLinks.Add(groupItem);
                }
                groupItem.LinkClicked += GraphItem_LinkClicked;
            }
            NavBarGroup activeGroup;
            if (_currentChartGroup == null)
                activeGroup = navBarCharts.ActiveGroup;
            else
                activeGroup = navBarCharts.Groups[_currentChartGroup];
            try
            {
                bool found = false;
                activeGroup.Expanded = true;
                foreach (NavBarItemLink item in activeGroup.ItemLinks)
                {
                    if (item.ItemName == _currentSelectedChart)
                    {
                        item.PerformClick();
                        activeGroup.SelectedLink = item;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    activeGroup.ItemLinks[0].PerformClick();
                    activeGroup.SelectedLink = activeGroup.ItemLinks[0];
                }
            }
            catch
            {
                try
                {
                    if (splashScreenManagerWaitForm.IsSplashFormVisible)
                    {
                        splashScreenManagerWaitForm.CloseWaitForm();
                    }
                }
                catch { }
                finally
                {
                    _chart.Series.Clear();
                    _chart.Titles.Clear(); ;
                }
            }
        }


        void GraphItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {

            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

            _currentSelectedChart = e.Link.ItemName;
            _currentChartGroup = e.Link.Group.Name;
            ChartData.GetChartData(_currentPlayer, e.Link.ItemName, e.Link.Group.Caption, _chart);
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        private void WriteHTMLDocument(string pageName, string playerName, CustomGrouping customGrouping, Tank tank)
        {
            WOTCompare wotCompare;

            _currentStatsFile.TryGetValue(playerName.Replace("_", "*"), out wotCompare);
            if (wotCompare != null)
            {

                if (pageName == "Global")
                {
                    _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("Global", wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                }
                else
                    if (pageName == "Tanks")
                    {
                        _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("Tanks", wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                    }
                    else
                        if (pageName == "KillCounts") //add new stats for tab
                        {
                            _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("KillCounts", wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                        }
                        else
                            if (pageName == "CustomGroupings") //add new stats for tab
                            {
                                _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("CustomGroupings", customGrouping, wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                            }
                            else
                                if (pageName == "KillSummary") //add new stats for tab
                                {
                                    _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("KillSummary", wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                                }
                                else
                                    if (pageName == "LastPlayedTanks") //add new stats for tab
                                    {
                                        _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("LastPlayedTanks", playerName);
                                        if (_browser.DocumentText.StartsWith("<HTML></HTML>"))
                                            _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("LastPlayedTanks", playerName);

                                    }
                                    else
                                        if (pageName == "TankInfo")
                                        {
                                            _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("TankInfo", tank);
                                        }
                                        else
                                            if (pageName == "Achievements")
                                            {
                                                _browser.DocumentText = new WOTHtml(_message).GetGlobalHTML("Achievements", wotCompare.WOTCurrent, wotCompare.WOTPrevious, wotCompare.Delta);
                                            }
            }
        }


        private void BuildTankInfo(string playerName)
        {
            WOTCompare wotCompare;
            _currentStatsFile.TryGetValue(playerName.Replace("_", "*"), out wotCompare);

            navBarControl2.Visible = true;
            WOTStats ws = wotCompare.WOTCurrent;

            tankimage.ColorDepth = ColorDepth.Depth32Bit;
            tankimage.ImageSize = new Size(50, 24);

            FillListView(ws);
            try
            {
                if (_tankInfo_SelectedTankID == "")
                {
                    NavBarGroup group = navBarControl2.ActiveGroup;
                    //group.Expanded = true;
                    group.SelectedLink = group.ItemLinks[0];
                    group.ItemLinks[0].PerformClick();
                }
                else
                {
                    navBarControl1.Groups[_currentPlayer].SelectedLink = navBarControl1.Groups[_currentPlayer].ItemLinks[3];

                    NavBarGroup group = navBarControl2.Groups["group_" + _tankInfo_SelectedTankID.Split('_')[0]];
                    group.Expanded = true;

                    foreach (NavBarItemLink item in group.ItemLinks)
                    {
                        if (item.ItemName == _tankInfo_SelectedTankID)
                        {
                            item.PerformClick();
                            navBarControl2.Groups["group_" + _tankInfo_SelectedTankID.Split('_')[0]].SelectedLink = item;
                            return;
                        }
                    }

                    group = navBarControl2.ActiveGroup;
                    group.Expanded = true;
                    group.SelectedLink = group.ItemLinks[0];
                    group.ItemLinks[0].PerformClick();
                }
            }
            catch
            {
            }
        }

        private void FillListView(WOTStats ws)
        {
            navBarControl2.Items.Clear();
            navBarControl2.Groups.Clear();
            NavBarGroup group = null;
            NavBarItem groupItem = null;
            foreach (Tank tank in ws.tanks.OrderBy(y => y.CountryID).ThenByDescending(x => x.Tier))
            {
                try
                {
                    if (!tankimage.Images.ContainsKey(String.Format("{0}_{1}", tank.CountryID, tank.TankID)))
                        tankimage.Images.Add(String.Format("{0}_{1}", tank.CountryID, tank.TankID), Image.FromFile(String.Format(@"{0}", WOTHelper.GetImagePath(tank.CountryID + "_" + tank.TankID + "_Large.png"))));

                    if (!tankimage.Images.ContainsKey(String.Format("{0}", tank.CountryID)))
                        tankimage.Images.Add(String.Format("{0}", tank.CountryID), Image.FromFile(String.Format(@"{0}\Images\Countries\{1}.png", WOTHelper.GetEXEPath(), tank.CountryID)));
                }
                catch
                { }


                if (group == null)
                {
                    group = new NavBarGroup(tank.Country_Description);
                    group.Name = "group_" + tank.CountryID;

                    if (tankimage.Images[String.Format("{0}", tank.CountryID)] != null)
                        group.SmallImage = tankimage.Images[String.Format("{0}", tank.CountryID)];

                    navBarControl2.Groups.Add(group);
                }

                if (group.Name != "group_" + tank.CountryID)
                {
                    group = new NavBarGroup(tank.Country_Description);
                    group.Name = "group_" + tank.CountryID;
                    group.SmallImage = tankimage.Images[String.Format("{0}", tank.CountryID)];
                    navBarControl2.Groups.Add(group);
                    groupItem = new NavBarItem(tank.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tank.CountryID, tank.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                else
                {
                    groupItem = new NavBarItem(tank.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tank.CountryID, tank.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                groupItem.LinkClicked += groupItem_LinkClicked;
            }

        }

        private void FillCompare(WOTStats wsA, WOTStats wsB)
        {
            navBarControl3.Items.Clear();
            navBarControl3.Groups.Clear();

            navBarControl4.Items.Clear();
            navBarControl4.Groups.Clear();

            tankimage.ColorDepth = ColorDepth.Depth32Bit;
            tankimage.ImageSize = new Size(50, 24);

            NavBarGroup group = null;
            NavBarItem groupItem = null;
            foreach (Tank tank in wsA.tanks.OrderBy(y => y.CountryID).ThenByDescending(x => x.Tier))
            {
                try
                {
                    if (!tankimage.Images.ContainsKey(String.Format("{0}_{1}", tank.CountryID, tank.TankID)))
                        tankimage.Images.Add(String.Format("{0}_{1}", tank.CountryID, tank.TankID), Image.FromFile(String.Format(@"{0}", WOTHelper.GetImagePath(tank.CountryID + "_" + tank.TankID + "_Large.png"))));

                    if (!tankimage.Images.ContainsKey(String.Format("{0}", tank.CountryID)))
                        tankimage.Images.Add(String.Format("{0}", tank.CountryID), Image.FromFile(String.Format(@"{0}\Images\Countries\{1}.png", WOTHelper.GetEXEPath(), tank.CountryID)));
                }
                catch
                { }


                if (group == null)
                {
                    group = new NavBarGroup(tank.Country_Description);
                    group.Name = "group_" + tank.CountryID;

                    if (tankimage.Images[String.Format("{0}", tank.CountryID)] != null)
                        group.SmallImage = tankimage.Images[String.Format("{0}", tank.CountryID)];

                    navBarControl3.Groups.Add(group);
                }

                if (group.Name != "group_" + tank.CountryID)
                {
                    group = new NavBarGroup(tank.Country_Description);
                    group.Name = "group_" + tank.CountryID;
                    group.SmallImage = tankimage.Images[String.Format("{0}", tank.CountryID)];
                    navBarControl3.Groups.Add(group);
                    groupItem = new NavBarItem(tank.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tank.CountryID, tank.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                else
                {
                    groupItem = new NavBarItem(tank.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tank.CountryID, tank.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tank.CountryID, tank.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                groupItem.LinkClicked += CompareItemA_LinkClicked;
            }

            group = null;
            groupItem = null;
            foreach (Tank tankB in wsB.tanks.OrderBy(y => y.CountryID).ThenByDescending(x => x.Tier))
            {
                try
                {
                    if (!tankimage.Images.ContainsKey(String.Format("{0}_{1}", tankB.CountryID, tankB.TankID)))
                        tankimage.Images.Add(String.Format("{0}_{1}", tankB.CountryID, tankB.TankID), Image.FromFile(String.Format(@"{0}\Images\Tanks\{1}_{2}_Large.png", WOTHelper.GetEXEPath(), tankB.CountryID, tankB.TankID)));

                    if (!tankimage.Images.ContainsKey(String.Format("{0}", tankB.CountryID)))
                        tankimage.Images.Add(String.Format("{0}", tankB.CountryID), Image.FromFile(String.Format(@"{0}\Images\Countries\{1}.png", WOTHelper.GetEXEPath(), tankB.CountryID)));
                }
                catch
                { }


                if (group == null)
                {
                    group = new NavBarGroup(tankB.Country_Description);
                    group.Name = "group_" + tankB.CountryID;

                    if (tankimage.Images[String.Format("{0}", tankB.CountryID)] != null)
                        group.SmallImage = tankimage.Images[String.Format("{0}", tankB.CountryID)];

                    navBarControl4.Groups.Add(group);
                }

                if (group.Name != "group_" + tankB.CountryID)
                {
                    group = new NavBarGroup(tankB.Country_Description);
                    group.Name = "group_" + tankB.CountryID;
                    group.SmallImage = tankimage.Images[String.Format("{0}", tankB.CountryID)];
                    navBarControl4.Groups.Add(group);
                    groupItem = new NavBarItem(tankB.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tankB.CountryID, tankB.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tankB.CountryID, tankB.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tankB.CountryID, tankB.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                else
                {
                    groupItem = new NavBarItem(tankB.Tank_Description);
                    groupItem.Name = String.Format("{0}_{1}", tankB.CountryID, tankB.TankID);

                    if (tankimage.Images[String.Format("{0}_{1}", tankB.CountryID, tankB.TankID)] != null)
                        groupItem.SmallImage = tankimage.Images[String.Format("{0}_{1}", tankB.CountryID, tankB.TankID)];

                    group.ItemLinks.Add(groupItem);
                }
                groupItem.LinkClicked += new NavBarLinkEventHandler(CompareItemB_LinkClicked);
            }

        }

        void CompareItemA_LinkClicked(object sender, NavBarLinkEventArgs e)
        {

            WOTCompare wotCompare;
            _currentStatsFile.TryGetValue(dockPanel3.Text.Replace("_", "*"), out wotCompare);
            WOTStats ws = wotCompare.WOTCurrent;
            _compareStatA = ws;
            _compareTankIDA = e.Link.ItemName;

            if (_compareStatB != null)
                _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, e.Link.ItemName, ws, dockPanel4.Text, _compareTankIDB, _compareStatB);
        }

        void CompareItemB_LinkClicked(object sender, NavBarLinkEventArgs e)
        {

            WOTCompare wotCompare;
            _currentStatsFile.TryGetValue(dockPanel4.Text.Replace("_", "*"), out wotCompare);
            WOTStats ws = wotCompare.WOTCurrent;
            _compareStatB = ws;
            _compareTankIDB = e.Link.ItemName;

            if (_compareStatA != null)
                _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, _compareTankIDA, _compareStatA, dockPanel4.Text, e.Link.ItemName, ws);
        }

        void groupItem_LinkClicked(object sender, NavBarLinkEventArgs e)
        {

            WOTCompare wotCompare;
            _currentStatsFile.TryGetValue(_currentPlayer.Replace("_", "*"), out wotCompare);
            WOTStats ws = wotCompare.WOTCurrent;
            Tank selTank = (from x in ws.tanks
                            where x.CountryID == int.Parse(e.Link.ItemName.Split('_')[0])
                            && x.TankID == int.Parse(e.Link.ItemName.Split('_')[1])
                            select x).FirstOrDefault();

            WriteHTMLDocument("TankInfo", _currentPlayer, null, selTank);
            _tankInfo_SelectedTankID = e.Link.ItemName;
        }

        private void BuildCustomGrouping(string playerName)
        {
            CustomGrouping cg = new CustomGrouping(_message);
            WriteHTMLDocument("CustomGroupings", playerName, cg, null);
        }

        private void ClearControls()
        {
            for (int ix = panelControl1.Controls.Count - 1; ix >= 0; ix--)
                if (panelControl1.Controls[ix] is ListView || panelControl1.Controls[ix] is ListBox)
                    panelControl1.Controls[ix].Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.TaskManagerClosing)
                return;


            if (UserSettings.MinimiseToTray)
            {
                this.Hide();

                WOTTrayApp trayApp = new WOTTrayApp();
                trayApp.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private void navBarControl1_MouseClick(object sender, MouseEventArgs e)
        {
            NavBarHitInfo hitInfo = navBarControl1.CalcHitInfo(new Point(e.X, e.Y));
            if (hitInfo.InGroupCaption || hitInfo.InExpandButton)
            {
                NavBarGroup group = hitInfo.Group;
                if (group.Caption != "Compare")
                {
                    if (group.Expanded)
                    {
                        group.Expanded = false;
                    }
                    else
                    {
                        group.Expanded = true;
                        if (group.ItemLinks.Count > 0)
                        {
                            group.ItemLinks[group.TopVisibleLinkIndex].PerformClick();
                            group.SelectedLink = group.ItemLinks[group.TopVisibleLinkIndex];
                        }

                    }
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            dm.RefreshDossier();
        }

        private void barButtonSelection_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);

            using (CompareSelection frm = new CompareSelection(_message, dm))
            {
                frm.ShowDialog(this);
            }
            dm.RefreshDossier();
        }



        void CreateTitle()
        {
            this.Text = _title + " (Version : " + Application.ProductVersion + ")";
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);


            barButtonItem1.Enabled = new FTPDetails().AllowFTP;


            if (UserSettings.AllowvBAddictUpload || UserSettings.AllowvBAddictUploadDossier || UserSettings.AllowvBAddictUploadDossierBattleResult || UserSettings.AllowvBAddictUploadDossierBattleResultReplay)
            {
                STR_ONLINEANALYZER.Enabled = true;
                PlayerListing pl = new PlayerListing(_message);
                if (pl.GetPlayer(_currentPlayer).OnlineURL == "#")
                    STR_ONLINEANALYZER.Enabled = false;
            }
            else
                STR_ONLINEANALYZER.Enabled = false;

            string caption = _currentPlayer + " (";
            if (dm != null)
            {

                

                if (dm.GetFileA() != 0 && dm.GetFileB() != 0)
                {
                    caption += String.Format("{0} To {1}", DateTime.Parse(dm.GetPlayerFileName(dm.GetFileA())).ToString(UserSettings.DateFormat), DateTime.Parse(dm.GetPlayerFileName(dm.GetFileB())).ToString(UserSettings.DateFormat));
                }
                else
                {

                    if (dm.GetFileA() != 0 && dm.GetFileB() == 0)
                    {

                        caption += String.Format("{0} To {1}", DateTime.Parse(dm.GetPlayerFileName(dm.GetFileA())).ToString(UserSettings.DateFormat), DateTime.Parse(dm.GetPlayerFileName(dm.GetFileA())).ToString(UserSettings.DateFormat));
                    }
                    else if (dm.GetFileB() != 0 && dm.GetFileA() == 0)
                    {

                        caption += String.Format("{0} To {1}", DateTime.Parse(dm.GetPlayerFileName(dm.GetFileB())).ToString(UserSettings.DateFormat), DateTime.Parse(dm.GetPlayerFileName(dm.GetFileB())).ToString(UserSettings.DateFormat));
                    }
                    else
                        caption += "No Data";
                }

                barCurrentView.Caption = caption + ")";

                Graphics graphics = this.CreateGraphics();
                SizeF textSize = graphics.MeasureString(barCurrentView.Caption, barCurrentView.Font);

                barCurrentView.LeftIndent = int.Parse(Math.Round((Width / 2) - (textSize.Width / 2), 0).ToString());
            }
        }

        private void navBarControl2_MouseClick(object sender, MouseEventArgs e)
        {
            NavBarHitInfo hitInfo = navBarControl2.CalcHitInfo(new Point(e.X, e.Y));
            if (hitInfo.InGroupCaption || hitInfo.InExpandButton)
            {
                NavBarGroup group = hitInfo.Group;

                if (group.Expanded)
                {
                    group.Expanded = false;

                }
                else
                {
                    group.Expanded = true;
                    if (group.ItemLinks.Count > 0)
                    {
                        group.ItemLinks[group.TopVisibleLinkIndex].PerformClick();
                        group.SelectedLink = group.ItemLinks[group.TopVisibleLinkIndex];
                    }
                }

            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void dockPanel1_VisibilityChanged(object sender, DevExpress.XtraBars.Docking.VisibilityChangedEventArgs e)
        {

        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (DossierManager item in dictPlayers.Values)
            {
                item.StartDossierWatch();
                barStaticItem1.Glyph = WOTStatistics.Stats.Properties.Resources.bullet_square_green;
                barStaticItem1.Caption = "Monitor Status : Running";
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (DossierManager item in dictPlayers.Values)
            {
                item.StopDossierWatch();
                barStaticItem1.Glyph = WOTStatistics.Stats.Properties.Resources.bullet_square_red;
                barStaticItem1.Caption = "Monitor Status : Stopped";
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (frmDefineGroupings frm = new frmDefineGroupings())
            {
                frm.ShowDialog();
            }

            CreateWebPage(_currentPage, _currentPlayer);
        }

        private void comparePlayers1_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            dockPanel3.Hide();
            dockPanel4.Hide();
            dockPanel1.Show();
            navBarControl1.Groups[_currentPlayer].ItemLinks[0].PerformClick();
            navBarControl1.Groups[_currentPlayer].SelectedLink = navBarControl1.Groups[_currentPlayer].ItemLinks[0];
            bar6.Visible = false;
            bar6.Offset = 0;
            //_topicID = "10";
            helpProvider1.SetHelpNavigator(this, HelpNavigator.TopicId);
            helpProvider1.SetHelpKeyword(this, _topicID);
        }

        private void barCompare_GetItemData(object sender, EventArgs e)
        {
            STR_COMPARE.Reset();
            foreach (DossierManager dm in dictPlayers.Values)
            {
                BarButtonItem buttonOpen = new BarButtonItem(barManager, _currentPlayer + " -> " + dm.GetPlayerName);
                buttonOpen.ItemClick += buttonOpen_ItemClick;
                buttonOpen.Name = dm.GetPlayerName;
                STR_COMPARE.AddItem(buttonOpen);
            }
        }

        private void navBarControl4_MouseClick(object sender, MouseEventArgs e)
        {
            NavBarHitInfo hitInfo = navBarControl4.CalcHitInfo(new Point(e.X, e.Y));
            if (hitInfo.InGroupCaption || hitInfo.InExpandButton)
            {
                NavBarGroup group = hitInfo.Group;
                if (group.Expanded)
                {
                    group.Expanded = false;

                }
                else
                {
                    group.Expanded = true;
                    if (group.ItemLinks.Count > 0)
                    {
                        group.ItemLinks[group.TopVisibleLinkIndex].PerformClick();
                        group.SelectedLink = group.ItemLinks[group.TopVisibleLinkIndex];
                    }
                }

            }
        }

        private void navBarControl3_MouseClick(object sender, MouseEventArgs e)
        {
            NavBarHitInfo hitInfo = navBarControl3.CalcHitInfo(new Point(e.X, e.Y));
            if (hitInfo.InGroupCaption || hitInfo.InExpandButton)
            {
                NavBarGroup group = hitInfo.Group;

                if (group.Expanded)
                {
                    group.Expanded = false;

                }
                else
                {
                    group.Expanded = true;
                    if (group.ItemLinks.Count > 0)
                    {
                        group.ItemLinks[group.TopVisibleLinkIndex].PerformClick();
                        group.SelectedLink = group.ItemLinks[group.TopVisibleLinkIndex];
                    }
                }

            }
        }

        private void barSetup_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmSetup frm = new frmSetup(_currentPage))
            {
                frm.ShowDialog();
                PlayerListing pl = new PlayerListing(_message);
            }
            PlayerRefresh();
        }

        private void PlayerRefresh()
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            using (PlayerListing pl = new PlayerListing(_message))
            {
                if (pl.Count() == dictPlayers.Count)
                {
                    foreach (DossierManager item in dictPlayers.Values)
                    {
                        Player player = pl.GetPlayer(item.GetPlayerName);
                        if (item.WatchPath != player.WatchFile || item.FTPFileFetch != (player.Monitor.ToUpper() == "YES" ? true : false))
                        {
                            item.SetValues(player.WatchFile, player.Monitor.ToUpper() == "YES" ? true : false);
                            _currentStatsFile[player.PlayerID.Replace("_", "*")] = new WOTCompare(new Dossier(item.GetFileB(), item.GetPlayerName, _message).GetStats(), new Dossier(item.GetFileA(), item.GetPlayerName, _message).GetStats());
                            item.RefreshDossier();
                        }
                    }
                }
                else
                {
                    if (pl.Count() > dictPlayers.Count)
                    {
                        foreach (KeyValuePair<string, Player> player in pl)
                        {
                            if (!dictPlayers.ContainsKey(player.Value.PlayerID.Replace("_", "*")))
                            {
                                DossierManager dm = new DossierManager(player.Value.PlayerID, player.Value.WatchFile, player.Value.Monitor, _message, this);
                                dm.CurrentFileChange += new DossierManager_CurrentFileChanged(DossierFileChanged);
                                dm.StartDossierWatch();
                                dictPlayers.Add(player.Value.PlayerID.Replace("_", "*"), dm);

                                _currentStatsFile.Add(player.Value.PlayerID.Replace("_", "*"), new WOTCompare(new Dossier(dm.GetFileB(), dm.GetPlayerName, _message).GetStats(), new Dossier(dm.GetFileA(), dm.GetPlayerName, _message).GetStats()));
                                dm.RefreshDossier();

                                CreatePlayerMenu(player.Value.PlayerID);
                            }
                        }
                    }
                    else
                    {
                        List<string> keysToRemove = new List<string>();
                        foreach (KeyValuePair<string, DossierManager> item in dictPlayers)
                        {
                            if (pl.GetPlayer(item.Value.GetPlayerName).PlayerID == "Unknown")
                            {
                                keysToRemove.Add(item.Key);
                                _currentStatsFile.Remove(item.Key);
                                navBarControl1.Groups.Remove(navBarControl1.Groups[item.Value.GetPlayerName]);
                            }
                        }

                        foreach (string key in keysToRemove)
                        {
                            dictPlayers.Remove(key);
                        }

                        _currentPlayer = "";
                        _currentPage = "";
                    }
                }
            }

            if (_currentPlayer == "")
            {
                try
                {
                    navBarControl1.Groups[0].Expanded = true;
                    navBarControl1.Groups[0].ItemLinks[0].PerformClick();
                    navBarControl1.Groups[0].SelectedLink = navBarControl1.Groups[0].ItemLinks[0];
                }
                catch
                {
                    if (panelControl1.Controls.Contains(_chart))
                    {
                        panelControl1.Controls.Remove(_chart);
                        panelControl1.Controls.Add(_browser);
                        _browser.Dock = DockStyle.Fill;
                    }

                    bar3.Visible = false;
                    bar3.Offset = 0;
                    dockPanel2.Visible = false;
                    bar5.Visible = false;
                    bar5.Offset = 0;
                    dockPanelGraph.Visible = false;
                    barLastPlayedGames.Visible = false;
                    barLastPlayedGames.Offset = 0;
                    _browser.DocumentText = new WOTHtml(_message).Blank();
                }
            }
            else
            {
                if (_currentPage != "Compare")
                    CreateWebPage(_currentPage, _currentPlayer);


            }
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        private void barButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
        {

            Help.ShowHelp(this, helpProvider1.HelpNamespace, HelpNavigator.TopicId, _topicID);
        }

        private void barCharts_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmDefineCharts frm = new frmDefineCharts(_message))
            {
                frm.ShowDialog();
            }

            BuildGraphForm(_currentPlayer);
        }

        private void navBarCharts_MouseClick(object sender, MouseEventArgs e)
        {
            NavBarHitInfo hitInfo = navBarCharts.CalcHitInfo(new Point(e.X, e.Y));
            if (hitInfo.InGroupCaption || hitInfo.InExpandButton)
            {
                NavBarGroup group = hitInfo.Group;

                if (group.Expanded)
                {
                    group.Expanded = false;

                }
                else
                {
                    group.Expanded = true;
                    if (group.ItemLinks.Count > 0)
                    {
                        group.ItemLinks[group.TopVisibleLinkIndex].PerformClick();
                        group.SelectedLink = group.ItemLinks[group.TopVisibleLinkIndex];
                    }
                }

            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmCredits frm = new frmCredits())
            {
                frm.ShowDialog();
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmNotice frm = new frmNotice())
            {
                frm.ShowDialog();
            }
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            if (UserSettings.NewVersionNotify)
            {
                UserSettings.NewVersionNotify = false;
                _message.Add("New update available: v" + UserSettings.NewAppVersion + ". Visit http://www.vbaddict.net/wotstatistics");
                if (DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_VERSIONNOTIFY", "DE", "New update available: v" + UserSettings.NewAppVersion + "  "+ Environment.NewLine + "Would you like to open the web page www.vbaddict.net/wotstatistics").Replace("{version}", UserSettings.NewAppVersion), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    OpenWebPage("WOTStatsWebLink");
                }
            }

            using (PlayerListing players = new PlayerListing(_message))
            {
                if (players.Count == 0)
                {
                    using (frmSetup setup = new frmSetup("Players"))
                    {
                        setup.ShowDialog();
                    }
                    PlayerRefresh();
                }
            }
            barBattleMode.BeginUpdate();
            barBattleMode.EditValue = UserSettings.BattleMode;
            barBattleMode.EndUpdate();

            //barRatingSystem.BeginUpdate();
            //barRatingSystem.EditValue = UserSettings.RatingSystem;
            //barRatingSystem.EndUpdate();
        }

        private void barButtonClearLastPlayedGames_ItemClick(object sender, ItemClickEventArgs e)
        {
            //System.Windows.Forms.DialogResult result = MessageBox.Show(Translations.TranslationGet("STR_CLEARRECENTNOTIFY", "DE", "Are you sure you want to clear your recent battle list?"), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Confirm user wants to delete
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(Translations.TranslationGet("STR_NEWSESSIONRECENTNOTIFY", "DE", "Do you want to start a new session?"), "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                RecentBattles lb = new RecentBattles(_currentPlayer, _message);
                string sessionID = lb.NewSession();
                UserSettings.RecentBattlesCurrentSession = 0;
                RecentBattlesNavigate("rsID = (select max(rsID) from Recentbattles_Session)");
            }

        }

     
        void repositoryItemRadioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {

            RadioGroup rg = ((RadioGroup)sender);
            UserSettings.GroupLPT = rg.EditValue.ToString();

            WriteHTMLDocument("LastPlayedTanks", _currentPlayer, null, null);
            barManager.CloseMenus();
        }

        private void barButtonItem1_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            using (PlayerListing pl = new PlayerListing(_message))
            {
                try
                {
                    Player p = pl.GetPlayer(_currentPlayer);
                    System.Diagnostics.Process.Start(p.OnlineURL);
                }
                catch { }
            }

        }

        private void barButtonItem1_ItemClick_2(object sender, ItemClickEventArgs e)
        {
            frmFTPBrowser frm = new frmFTPBrowser();
            frm.Show();
        }

        private void barEditItem3_ItemClick(object sender, ItemClickEventArgs e)
        {


        }
        void repositoryItemRecentBattlesDisplayList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_wait)
            {
                try
                {
                    splashScreenManagerWaitForm.ShowWaitForm();
                }
                catch { }

                _wait = true;
                RadioGroup rg = ((RadioGroup)sender);
                UserSettings.RecentBattlesCurrentSession = int.Parse(rg.EditValue.ToString());

                if (rg.EditValue.ToString() == "0")
                {
                    RecentBattlesNavigations(true);
                    RecentBattlesNavigate("rsID = (select max(rsID) from Recentbattles_Session)");
                }
                else
                {
                    RecentBattlesNavigations(false);
                    WriteHTMLDocument("LastPlayedTanks", _currentPlayer, null, null);
                }

                barManager.CloseMenus();
                
                _browser.Focus();
                _wait = false;


                try
                {
                    if (splashScreenManagerWaitForm.IsSplashFormVisible)
                    {
                        splashScreenManagerWaitForm.CloseWaitForm();
                    }
                }
                catch { }
    

            }

        }

        private void barSubItem1_Popup(object sender, EventArgs e)
        {
            //repositoryItemRecentBattlesDisplayList.Items[1].Description = Translations.TranslationGet("STR_CAP_LASTGAMESPLD", "DE", "Last {NOGAMES} PLayed"); 
            //repositoryItemRecentBattlesDisplayList.Items[1].Description = repositoryItemRecentBattlesDisplayList.Items[1].Description.Replace("{NOGAMES}", AppSettings.LastPlayedCompareQuota.ToString());
            STR_RB_DISPLAYLIST.BeginUpdate();
            barRecentBattlesView.BeginUpdate();
            /*
             *1) Current Session 2) Today's battles 3) Last 3 days 4) Last week
how does that sound to you?
and 5) Last X battles
 
             * 
             */


            repositoryItemRecentBattlesDisplayList.Items[0].Description = Translations.TranslationGet("STR_CAP_RBCS", "DE", "Current Session");
            repositoryItemRecentBattlesDisplayList.Items[1].Description = Translations.TranslationGet("STR_CAP_RBTB", "DE", "Today's battles");
            repositoryItemRecentBattlesDisplayList.Items[2].Description = Translations.TranslationGet("STR_CAP_RBLTD", "DE", "Last 3 days");
            repositoryItemRecentBattlesDisplayList.Items[3].Description = Translations.TranslationGet("STR_CAP_RBLW", "DE", "Last week");
            repositoryItemRecentBattlesDisplayList.Items[4].Description = Translations.TranslationGet("STR_CAP_RBLX", "DE", "Last {X} battles").Replace("{X}", UserSettings.LastPlayedCompareQuota.ToString());
           
            barRecentBattlesView.EditValue = UserSettings.RecentBattlesCurrentSession.ToString();


            if (barRecentBattlesView.EditValue.ToString() == "0")
                RecentBattlesNavigations(true);
            else
                RecentBattlesNavigations(false);


            barRecentBattlesView.EndUpdate();
            STR_RB_DISPLAYLIST.EndUpdate();
            
        }

   

        private void STR_WOTSTATSLINK_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenWebPage("WOTStatsWebLink");
        }

        private void STR_EUFORMLINK_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenWebPage("EUForumWebLink");
        }

        private void STR_NAFORUMLINK_ItemClick(object sender, ItemClickEventArgs e)
        {
            OpenWebPage("NAForumWebLink");
        }

        private void OpenWebPage(string referal)
        {
            System.Diagnostics.Process.Start(ApplicationSettings.GetPropertyValue(referal));
        }

    

        private void RecentBattlesNavigate(string where)
        {
            using (DBHelpers helper = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _currentPlayer, "LastBattle", "WOTSStore.db")))
            {
                using (DataTable dt = helper.GetDataTable("select rsID, rsKey from Recentbattles_Session where " + where))
                {
                    if (dt.Rows.Count > 0)
                    {
                        barButRBMoveTo.EditValue = dt.Rows[0]["rsID"];
                        barButRBMoveTo.Tag = dt.Rows[0]["rsKey"];
                        UserSettings.ViewSessionID = dt.Rows[0]["rsKey"].ToString();
                    }
                    else
                    {
                        using (DataTable dtFallback = helper.GetDataTable("select rsID, rsKey from Recentbattles_Session where rsID = (select max(rsID) from Recentbattles_Session where rsID < 99999999)"))
                        {
                            if (dtFallback.Rows.Count > 0)
                            {
                                barButRBMoveTo.BeginUpdate();
                                barButRBMoveTo.EditValue = dtFallback.Rows[0]["rsID"];
                                barButRBMoveTo.Tag = dtFallback.Rows[0]["rsKey"];
                                barButRBMoveTo.EndUpdate();
                                UserSettings.ViewSessionID = dtFallback.Rows[0]["rsKey"].ToString();
                            }
                        }
                    }
                }

                RecentBattlesNavigations(true);

               

                

                //lb.Save();
                
                WriteHTMLDocument("LastPlayedTanks", _currentPlayer, null, null);
               
            }
        }

        private void RecentBattlesNavigations(bool value)
        {
            if (value != false)
            {
                using (DBHelpers helper = new DBHelpers(Path.Combine(WOTHelper.GetApplicationData(), "Hist_" + _currentPlayer, "LastBattle", "WOTSStore.db")))
                {

                    using (DataTable minMax = helper.GetDataTable(@"select min(rsID) minID, max(rsID) maxID from Recentbattles_Session"))
                    {
                        if (minMax.Rows.Count > 0)
                        {


                            barButRBMoveFirst.Enabled = (Convert.ToInt32((minMax.Rows[0]["minID"] == DBNull.Value) ? 0 : minMax.Rows[0]["minID"]) == Convert.ToInt32(barButRBMoveTo.EditValue ?? 0) ? false : value);
                            barButRBMovePrevious.Enabled = (Convert.ToInt32(minMax.Rows[0]["minID"] == DBNull.Value ? 0 : minMax.Rows[0]["minID"]) == Convert.ToInt32(barButRBMoveTo.EditValue ?? 0) ? false : value); ;
                            barButRBMoveTo.Enabled = value;
                            barButRBMoveNext.Enabled = (Convert.ToInt32(minMax.Rows[0]["maxID"] == DBNull.Value ? 0 : minMax.Rows[0]["maxID"]) == Convert.ToInt32(barButRBMoveTo.EditValue ?? 0) ? false : value); ;
                            barButRBMoveLast.Enabled = (Convert.ToInt32(minMax.Rows[0]["maxID"] == DBNull.Value ? 0 : minMax.Rows[0]["maxID"]) == Convert.ToInt32(barButRBMoveTo.EditValue ?? 0) ? false : value); ;


                        }
                        else
                        {
                            barButRBMoveFirst.Enabled = false;
                            barButRBMovePrevious.Enabled = false;
                            barButRBMoveTo.Enabled = false;
                            barButRBMoveNext.Enabled = false;
                            barButRBMoveLast.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                barButRBMoveFirst.Enabled = false;
                barButRBMovePrevious.Enabled = false;
                barButRBMoveTo.Enabled = false;
                barButRBMoveNext.Enabled = false;
                barButRBMoveLast.Enabled = false;
            }
           
        }

        private void barButRBMoveFirst_ItemClick(object sender, ItemClickEventArgs e)
        {
            RecentBattlesNavigate("rsID = (select min(rsID) from Recentbattles_Session)");
        }

        private void barButRBMovePrevious_ItemClick(object sender, ItemClickEventArgs e)
        {

            RecentBattlesNavigate("rsID = (select max(rsID) from Recentbattles_Session where rsID < " + barButRBMoveTo.EditValue + ")");

        }

        private void barButRBMoveLast_ItemClick(object sender, ItemClickEventArgs e)
        {
            RecentBattlesNavigate("rsID = (select max(rsID) from Recentbattles_Session)");
        }

        private void barButRBMoveNext_ItemClick(object sender, ItemClickEventArgs e)
        {
            RecentBattlesNavigate("rsID = (select min(rsID) from Recentbattles_Session where rsID > " + barButRBMoveTo.EditValue + ")");
        }

        private void barButRBMoveTo_EditValueChanged(object sender, EventArgs e)
        {

            RecentBattlesNavigate("rsID = " + ((BarEditItem)sender).EditValue );
        }

        private void barBattleMode_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }
            UserSettings.BattleMode = barBattleMode.EditValue.ToString();
            DossierManager dm;
            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            dm.RefreshDossier();

            if (_currentPage == "Compare")
            {
                WOTCompare wotA;
                _currentStatsFile.TryGetValue(_currentPlayer.Replace("_", "*"), out wotA);
                _compareStatA = wotA.WOTCurrent;
                WOTCompare wotB;
                _currentStatsFile.TryGetValue(_currentComparePlayer, out wotB);
                _compareStatB = wotB.WOTCurrent;
                FillCompare(wotA.WOTCurrent, wotB.WOTCurrent);


                try
                {
                    if (_compareStatA != null)
                        _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, _compareTankIDA, _compareStatA, dockPanel4.Text, _compareTankIDB, wotB.WOTCurrent);

                }
                catch
                {
                    try
                    {
                        NavBarGroup groupA = navBarControl3.ActiveGroup;
                        groupA.ItemLinks[0].PerformClick();

                    }
                    catch 
                    {
                      
                    }
                }

                try
                {
                    if (_compareStatB != null)
                        _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, _compareTankIDA, wotA.WOTCurrent, dockPanel4.Text, _compareTankIDB, _compareStatB);
                }
                catch
                {
                    try
                    {
                        NavBarGroup groupB = navBarControl4.ActiveGroup;
                        groupB.ItemLinks[0].PerformClick();
                    }
                    catch
                    {

                    }
                }

            }
            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                {
                    splashScreenManagerWaitForm.CloseWaitForm();
                }
            }
            catch { }
        }

        private void barRatingSystem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                splashScreenManagerWaitForm.ShowWaitForm();
            }
            catch { }

            //UserSettings.RatingSystem = barRatingSystem.EditValue.ToString();
            DossierManager dm;

            dictPlayers.TryGetValue(_currentPlayer.Replace("_", "*"), out dm);
            dm.RefreshDossier();

            if (_currentPage == "Compare")
            {
                WOTCompare wotA;
                _currentStatsFile.TryGetValue(_currentPlayer.Replace("_", "*"), out wotA);
                _compareStatA = wotA.WOTCurrent;
                WOTCompare wotB;
                _currentStatsFile.TryGetValue(_currentComparePlayer, out wotB);
                _compareStatB = wotB.WOTCurrent;
                FillCompare(wotA.WOTCurrent, wotB.WOTCurrent);

                try
                {
                    if (_compareStatA != null)
                        _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, _compareTankIDA, _compareStatA, dockPanel4.Text, _compareTankIDB, wotB.WOTCurrent);

                }
                catch
                {
                    try
                    {
                        NavBarGroup groupA = navBarControl3.ActiveGroup;
                        groupA.ItemLinks[0].PerformClick();

                    }
                    catch
                    {

                    }
                }

                try
                {
                    if (_compareStatB != null)
                        _browser.DocumentText = new WOTHtml(_message).CompareHTML(dockPanel3.Text, _compareTankIDA, wotA.WOTCurrent, dockPanel4.Text, _compareTankIDB, _compareStatB);
                }
                catch
                {
                    try
                    {
                        NavBarGroup groupB = navBarControl4.ActiveGroup;
                        groupB.ItemLinks[0].PerformClick();
                    }
                    catch
                    {

                    }
                }
            }
           

            try
            {
                if (splashScreenManagerWaitForm.IsSplashFormVisible)
                splashScreenManagerWaitForm.CloseWaitForm();
            }
            catch { }
        }

    

    
    }
}
