using System;
using System.Windows.Forms;
using DevExpress.LookAndFeel;

namespace WOT.Stats
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


           // DevExpress.UserSkins.BonusSkins.Register();

            Application.EnableVisualStyles();
            UserLookAndFeel defaultLF = UserLookAndFeel.Default;
            defaultLF.UseWindowsXPTheme = false;
            defaultLF.Style = LookAndFeelStyle.Skin;
            defaultLF.SkinName = "Office 2010 Black";
            UserLookAndFeel.Default.SetSkinStyle("Office 2010 Black");

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();

            
            Application.Run(new clsController());

        }
    }

   
}
