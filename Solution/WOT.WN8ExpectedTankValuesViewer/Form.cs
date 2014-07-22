using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOTStatistics.WN8ExpectedTankValuesViewer
{
    public partial class Form : DevExpress.XtraEditors.XtraForm
    {
        public Form()
        {
            InitializeComponent();
        }


        private void WN8Expected_Load(object sender, EventArgs e)
        {
            
            
            WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
            lblDate.Caption = string.Format("Date: {0}", WN8ExpectedTankList.WN8Date.ToShortDateString());
            lblVersion.Caption = string.Format("Version: {0}", WN8ExpectedTankList.WN8Version);
            gridWN8.DataSource = WN8ExpectedTankList;
            viewWN8.Columns["countryID"].Visible = false;
            viewWN8.Columns["tankID"].Visible = false;
            viewWN8.Columns["IDNum"].Visible = false;
            viewWN8.OptionsBehavior.ReadOnly = true;
            viewWN8.OptionsView.EnableAppearanceEvenRow = true;
                        
        }

        private void btnVisitOnline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.wnefficiency.net/wnexpected");
            }
            catch
            {

            }
        }
    }
}