using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using WOT.Core;

namespace WOT.Stats
{
    public partial class WN8ExpectedGrid : DevExpress.XtraEditors.XtraForm
    {
        public WN8ExpectedGrid()
        {
            InitializeComponent();
        }

       
        private void WN8Expected_Load(object sender, EventArgs e)
        {
            WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
            //WN8ExpValue WN8ExpectedTank = WN8ExpectedTankList.GetByTankID(0, 32);
            //Console.WriteLine("FF: " + WN8ExpectedTank.expWin);
            //List<WN8ExpValue> WN8ExpValues = new List<WN8ExpValue>();
            gridControl1.DataSource = WN8ExpectedTankList;



        }

    }
}