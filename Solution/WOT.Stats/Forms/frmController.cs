using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class frmController : Form
    {
      public frmController()
        {
            InitializeComponent();

        }

      private void frmController_Load(object sender, EventArgs e)
      {

          Visible = false;

          if (WOTHelper.FindProcess("WOT.Stats"))
          {
              DevExpress.XtraEditors.XtraMessageBox.Show("WOT Statistics is already running", "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.Close();
          }
          else
          {
              if (UserSettings.MinimiseonStartup == true)
              {
                  WOTTrayApp sysTray = new WOTTrayApp();
                  sysTray.Show();
              }
              else
              {
                  using (frmMain wotGUI = new frmMain())
                  {
                      wotGUI.ShowDialog();
                  }
              }

          }

      }
    }
}
