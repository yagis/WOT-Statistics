using System;
using System.Data;
using WOTStatistics.Core;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WOT.Stats
{
    public partial class ctxPlayerSetup : DevExpress.XtraEditors.XtraUserControl
    {
        PlayerListing pl = new PlayerListing(new MessageQueue());

        public ctxPlayerSetup()
        {
            InitializeComponent();

        }

        private void ctxPlayerSetup_Load(object sender, EventArgs e)
        {
            FillTable();
            ControllButtons();

            PlayerID.Caption = Translations.TranslationGet("STR_PLAYERID", "DE", "Player ID");
            PlayerRealm.Caption = Translations.TranslationGet("STR_PLAYERREALM", "DE", "Player Realm");
            ftp.Caption = Translations.TranslationGet("STR_FTPBOOL", "DE", "Retrieve From FTP");
            watchFile.Caption = Translations.TranslationGet("STR_WATCHFILE", "DE", "Watching File");
            butAdd.Text = Translations.TranslationGet("BTN_ADD", "DE", "Add");
            butEdit.Text = Translations.TranslationGet("BTN_EDIT", "DE", "Edit");
            butDelete.Text = Translations.TranslationGet("BTN_REMOVE", "DE", "Remove");
            
        }

        private void ControllButtons()
        {
            butEdit.Enabled = !(pl.Count <= 0);
            butDelete.Enabled = !(pl.Count <= 0);
        }

        private void butDelete_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show("Are you sure you want to remove player: " + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PlayerID") + "?", "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Confirm user wants to delete
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                pl.Remove(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PlayerID").ToString());
                gridView1.DeleteRow(gridView1.FocusedRowHandle);

            }

            ControllButtons();

        }

        private void butAdd_Click(object sender, EventArgs e)
        {
            using (frmAddEditPlayer frm = new frmAddEditPlayer())
            {
                frm.ShowDialog();
            }
            pl.Refresh();
            FillTable();
            ControllButtons();
        }

        private void FillTable()
        {
            DataTable players = new DataTable();
            players.Columns.Add("PlayerID", typeof(string));
            players.Columns.Add("PlayerRealm", typeof(string));
            players.Columns.Add("ftp", typeof(string));
            players.Columns.Add("watchFile", typeof(string));



            foreach (KeyValuePair<string, Player> player in pl)
            {
                DataRow dr = players.NewRow();
                dr["PlayerID"] = player.Value.PlayerID;
                dr["PlayerRealm"] = player.Value.PlayerRealm;
                dr["ftp"] = player.Value.Monitor;
                dr["watchFile"] = player.Value.WatchFile;
                players.Rows.Add(dr);
            }


            gridControl1.DataSource = players;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Player player = pl.GetPlayer(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "PlayerID").ToString());
            using (frmAddEditPlayer frm = new frmAddEditPlayer(player))
            {
                frm.ShowDialog();
            }
            pl.Refresh();
            FillTable();
            ControllButtons();
        }
    }
}
