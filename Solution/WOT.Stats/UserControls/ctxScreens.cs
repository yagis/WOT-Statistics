using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WOT.Stats
{
    public partial class ctxScreens : DevExpress.XtraEditors.XtraUserControl
    {
        public ctxScreens()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //AppSettings.ShowTabGlobal = checkTabGlobal.Checked;
            //AppSettings.ShowTabTanks = checkTabTanks.Checked;
            //AppSettings.ShowTabKillCounts = checkTabKillCounts.Checked;
            //AppSettings.ShowTabTankInfo = checkTabTankInfo.Checked;
            //AppSettings.ShowTabKillSummary = checkTabKillSummary.Checked;
            //AppSettings.ShowTabCustomGrouping = checkTabCustomGroupings.Checked;
            //AppSettings.ShowTabLastGamesPlayed = checkTabLastGamesPlayed.Checked;
            //AppSettings.ShowTabGraphs = checkTabGraph.Checked;
            
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ctxScreens_Load(object sender, EventArgs e)
        {
            //checkTabGlobal.Checked = AppSettings.ShowTabGlobal;
            //checkTabTanks.Checked = AppSettings.ShowTabTanks;
            //checkTabKillCounts.Checked = AppSettings.ShowTabKillCounts;
            //checkTabTankInfo.Checked = AppSettings.ShowTabTankInfo;
            //checkTabKillSummary.Checked = AppSettings.ShowTabKillSummary;
            //checkTabCustomGroupings.Checked = AppSettings.ShowTabCustomGrouping;
            //checkTabLastGamesPlayed.Checked = AppSettings.ShowTabLastGamesPlayed;
            //checkTabGraph.Checked = AppSettings.ShowTabGraphs;
        }
    }
}
