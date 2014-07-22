using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using DevExpress.LookAndFeel;

namespace WOT.Stats
{
    public partial class SplashS : SplashScreen
    {
        public SplashS()
        {
            InitializeComponent();
            
           //this.pictureEdit2.EditValue = global::WOTStatistics.Stats.Properties.Resources.wot_statistics;
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

        public override void ProcessCommand(System.Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
            SplashScreenCommand command = (SplashScreenCommand)cmd;
            if (command == SplashScreenCommand.SetCurrentAction)
            {
                //Dim pos As Integer = CInt(Fix(arg))
                //progressBarControl1.Position = pos
                labelControl2.Text = arg.ToString();
            }
        }


        //public override void ProcessCommand(Enum cmd, object arg)
        //{
        //    base.ProcessCommand(cmd, arg);
        //}

        #endregion

        public enum SplashScreenCommand
        {
            SetCurrentAction
        }
    }
}