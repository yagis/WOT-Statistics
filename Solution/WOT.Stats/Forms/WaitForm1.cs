using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;
using WOTStatistics.Core;
using DevExpress.LookAndFeel;

namespace WOTStatistics.Stats
{
    public partial class WaitForm1 : WaitForm
    {
        public WaitForm1()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
            SetDescription(Translations.TranslationGet("WAIT_DESC", "DE", "Loading ..."));
            SetCaption(Translations.TranslationGet("WAIT_CAPT", "DE", "Please Wait"));
        }

        #region Overrides

        UserLookAndFeel lookAndFeel;
        protected override DevExpress.LookAndFeel.UserLookAndFeel TargetLookAndFeel
        {
            get
            {
                if (lookAndFeel == null)
                {
                    lookAndFeel = new UserLookAndFeel(this);
                    lookAndFeel.UseDefaultLookAndFeel = false;
                    lookAndFeel.SkinName = "Office 2010 Black";
                }
                return lookAndFeel;
            }
        }

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }
    }
}