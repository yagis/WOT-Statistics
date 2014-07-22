using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;

namespace WOT.DBViewer
{
    public partial class Form1 : Form
    {
        string _dbPath;
        string _password;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                fbd.Filter = "WOT Stats db (WOTSStore.db)|WOTSStore.db";

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    _dbPath = fbd.FileName;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DBHelpers db = new DBHelpers(_dbPath))
            {
                dataGridView1.DataSource = db.GetDataTable(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newPath = _dbPath + "_noPassword";
            CreateDatabase.Create(newPath);

            DataTable DT_RB = new DataTable();
            DataTable DT_RB_RS = new DataTable();
            using (DBHelpers db = new DBHelpers(_dbPath))
            {
                using (var sqliteAdapter = new SQLiteDataAdapter(@"SELECT rbid,[rbcountryid],[rbtankid],[rboriginalbattlecount],[rbbattles],
       [rbkills],
       [rbdamagereceived],[rbdamagedealt],[rbxpreceived],[rbspotted],
       [rbcapturepoints],[rbdefencepoints],[rbsurvived],[rbvictory],
       [rbbattletime],[rbshot],[rbhits],[rbtier],[rbbattlespertier],
       [rbvictorycount],[rbdefeatcount],[rbdrawcount],[rbsurviveyescount],
       [rbsurvivenocount],[rbfraglist],Cast([rbbattletimefriendly] AS TEXT) as rbbattletimefriendly,
       [rbglobalavgtier],[rbglobalwinpercentage],[rbsessionid],
       rbglobalavgdefpoints
FROM   recentbattles", db.GetConnection()))
                {
                    sqliteAdapter.AcceptChangesDuringFill = false;
                    sqliteAdapter.Fill(DT_RB);
                }

                using (var sqliteAdapter = new SQLiteDataAdapter(@"SELECT [rsid],[rskey],Cast([rsdatefrom] AS TEXT) as rsdatefrom,Cast([rsdateto] AS TEXT) as rsdateto, 
       [rsuedatefrom],[rsuedateto] 
FROM   recentbattles_session ", db.GetConnection()))
                {
                    sqliteAdapter.AcceptChangesDuringFill = false;
                    sqliteAdapter.Fill(DT_RB_RS);
                }
            }

            using (DBHelpers db = new DBHelpers(newPath))
            {
                var sqliteAdapter = new SQLiteDataAdapter("SELECT * FROM RecentBattles", db.GetConnection(true));
                var cmdBuilder = new SQLiteCommandBuilder(sqliteAdapter) { SetAllValues = true };
                sqliteAdapter.Update(DT_RB);

                sqliteAdapter = new SQLiteDataAdapter("SELECT * FROM RecentBattles_Session", db.GetConnection(true));
                cmdBuilder = new SQLiteCommandBuilder(sqliteAdapter);
                sqliteAdapter.Update(DT_RB_RS);
            }
           
        }
    }
}
