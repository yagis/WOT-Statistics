using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;

namespace WOT.Stats
{
    public partial class frmNotice : DevExpress.XtraEditors.XtraForm
    {
        public frmNotice()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmNotice_Load(object sender, EventArgs e)
        {
            Text = Translations.TranslationGet("WNDCAPTION_IMPORTANTNOTICE", "DE", "Important Notice");
            STR_DISCLAIMER.Text = Translations.TranslationGet("STR_DISCLAIMER_1", "DE", "World of Tanks and Wargaming.net are registered trademarks or trademarks of Wargaming.net LLP.") + 
               Environment.NewLine + Translations.TranslationGet("STR_DISCLAIMER_2", "DE", "This application is in no way assiciated with Wargaming.net") ;
            BTN_CLOSE.Text = Translations.TranslationGet("BTN_CLOSE", "DE", "Close");
        }
    }
}