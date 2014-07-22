using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class ReleaseNotes : DevExpress.XtraEditors.XtraForm
    {
        public ReleaseNotes()
        {
            InitializeComponent();
        }


        public void LoadPage(string sURL)
        {
            

            WOTHelper.AddToLog("Loading Release Notes: " + sURL);
            
            oWeb.Navigate(sURL);

            WOTHelper.AddToLog("Loaded");

            oWeb.NewWindow += new System.ComponentModel.CancelEventHandler(this.oWeb_NewWindow);
            oWeb.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.oWeb_Navigating);
            
        }


        private void oWeb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(oWeb.StatusText.ToString());
            }
            catch (Exception ex)
            {
                WOTHelper.AddToLog("Cannot open Webpage: " + oWeb.StatusText.ToString() + " " + ex.Message);
            }

            e.Cancel = true;



        }

        private void oWeb_NewWindow(object sender, CancelEventArgs e)
        {

            try
            {
                System.Diagnostics.Process.Start(oWeb.StatusText.ToString());
            }
            catch (Exception ex)
            {
                WOTHelper.AddToLog("Cannot open Webpage: " + oWeb.StatusText.ToString() + " " + ex.Message);
            }

            e.Cancel = true;

        } 


       


        private void btnOK_Click(object sender, EventArgs e)
        {
            Hide();
            Close();
            
        }

 
    }
}