using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WOTStatistics.Core;
using System.Collections;
using System.Linq;

namespace WOT.Stats
{
    public partial class ctxDossierMonitor : DevExpress.XtraEditors.XtraUserControl
    {
        private readonly Dictionary<string, double> _timeAdj = new Dictionary<string, double>();
        public ctxDossierMonitor()
        {
            InitializeComponent();

            _timeAdj.Clear();
            _timeAdj.Add("12:00 AM", 0);
            _timeAdj.Add("01:00 AM", -1);
            _timeAdj.Add("02:00 AM", -2);
            _timeAdj.Add("03:00 AM", -3);
            _timeAdj.Add("04:00 AM", -4);
            _timeAdj.Add("05:00 AM", -5);
            _timeAdj.Add("06:00 AM", -6);
            _timeAdj.Add("07:00 AM", -7);
            _timeAdj.Add("08:00 AM", -8);
            _timeAdj.Add("09:00 AM", -9);
            _timeAdj.Add("10:00 AM", -10);
            _timeAdj.Add("11:00 AM", -11);
            _timeAdj.Add("12:00 PM", -12);
            _timeAdj.Add("01:00 PM", -13);
            _timeAdj.Add("02:00 PM", -14);
            _timeAdj.Add("03:00 PM", -15);
            _timeAdj.Add("04:00 PM", -16);
            _timeAdj.Add("05:00 PM", -17);
            _timeAdj.Add("06:00 PM", -18);
            _timeAdj.Add("07:00 PM", -19);
            _timeAdj.Add("08:00 PM", -20);
            _timeAdj.Add("09:00 PM", -21);
            _timeAdj.Add("10:00 PM", -22);
            _timeAdj.Add("11:00 PM", -23);
        }

        private void ctxDossierMonitor_Load(object sender, EventArgs e)
        {

            CHKBOX_MONITOR.Text = Translations.TranslationGet("CHKBOX_MONITOR", "DE", "Monitor Dossier On Startup");
            CHKBOX_MINIMIZE.Text = Translations.TranslationGet("CHKBOX_MINIMIZE", "DE", "Minimize App On Startup");
            CHKBOX_AUTOSTART.Text = Translations.TranslationGet("CHKBOX_AUTOSTART", "DE", "Start App When Windows Starts");
            CHKBOX_MINIMIZETOTRAY.Text = Translations.TranslationGet("CHKBOX_MINIMIZETOTRAY", "DE", "Minimize To System Tray");
            STR_RESET.Text = Translations.TranslationGet("STR_RESET", "DE", "Reset At :");

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("StartMonOnStartUp", out field);
            CHKBOX_MONITOR.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("MinimiseonStartup", out field);
            CHKBOX_MINIMIZE.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("LaunchWithWindows", out field);
            CHKBOX_AUTOSTART.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("MinimiseToTray", out field);
            CHKBOX_MINIMIZETOTRAY.Checked = Convert.ToBoolean(field.NewValue);

            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeAdjustment", out field);
           comboRestart.Text = (from x in _timeAdj
                              where x.Value == Convert.ToDouble(field.NewValue)
                              select x.Key).FirstOrDefault();
            
            
        }

        private void checkAutoMonitor_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("StartMonOnStartUp", out field);
            field.NewValue = CHKBOX_MONITOR.Checked;

        }

        private void checkMinimze_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("MinimiseonStartup", out field);
            field.NewValue = CHKBOX_MINIMIZE.Checked;
        }

        private void checkWindowsStart_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("LaunchWithWindows", out field);
            field.NewValue = CHKBOX_AUTOSTART.Checked;
        }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
        {
            double value;
            _timeAdj.TryGetValue(comboRestart.Text, out value);

            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("TimeAdjustment", out field);
            field.NewValue = value;

        }

        private void comboRestart_Properties_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void CHKBOX_MINIMIZETOTRAY_CheckedChanged(object sender, EventArgs e)
        {
            Form pForm = ParentForm;
            PropertyFields field;
            ((frmSetup)pForm)._propertyFields.TryGetValue("MinimiseToTray", out field);
            field.NewValue = CHKBOX_MINIMIZETOTRAY.Checked;
        }
    }
}
