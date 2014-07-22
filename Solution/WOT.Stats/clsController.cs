using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WOTStatistics.Core;

namespace WOT.Stats
{
    class clsController:Form
    {
        private bool _error = false;
        public clsController()
        {
            //Visible = false;

            if (WOTHelper.FindProcess("WOT.Stats"))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("WOT Statistics is already running", "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                _error = true;
                Application.Exit();
            }
            else
            {

                using (frmUpdater wotGUI = new frmUpdater())
                {
                    wotGUI.ShowDialog();
                }


            }
        }

        protected override void OnLoad(EventArgs e)
        {
                
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
            if (_error)
                Close();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                
            }

            base.Dispose(isDisposing);
        }
        
    }
}
