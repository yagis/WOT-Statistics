using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOTStatistics.Core;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace WOT.Stats
{
    public class WOTTrayApp : Form
    {

        readonly Dictionary<string, DossierManager> dictPlayers = new Dictionary<string, DossierManager>();

        private readonly NotifyIcon trayIcon;
        private readonly ContextMenu trayMenu;

        public WOTTrayApp()
        {
            //WOTHelper.FindandKillProcess("WOT.Stats");

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("WOT Statistics", SetupOnClick);
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "WOT Statistics Monitor";
            trayIcon.Icon = new Icon(new Icon(WOTHelper.GetEXEPath() + @"\wot_statistics.ico"), 40, 40);
            trayIcon.DoubleClick += trayIcon_DoubleClick;
            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            ActivateMainForm();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            //WOTHelper.FindandKillProcess("WOT.Stats");

            using (PlayerListing pl = new PlayerListing(new MessageQueue()))
            {
                foreach (KeyValuePair<string, Player> kv in pl)
                {
                    Player p = kv.Value;
                    DossierManager dm = new DossierManager(p.PlayerID, p.WatchFile, p.Monitor, new MessageQueue(), this);
                    dm.CurrentFileChange += dm_CurrentFileChange;
                    dictPlayers.Add(p.PlayerID.Replace("_", "*"), dm);
                    dm.StartDossierWatch();

                }
            }

            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
            Thread.Sleep(3000);//pause for 3 seconds

            trayIcon.ShowBalloonTip(1000, "WOT Statistics", "WOT Statistics is running in the background.", ToolTipIcon.Info);
        }

        void dm_CurrentFileChange(object sender, EventArgs e)
        {
            //trayIcon.ShowBalloonTip(1000, "WOT Statistics", "WOT Statistics is running in the background.", ToolTipIcon.Info);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ActivateMainForm()
        {
            bool allreadyLoaded = false;
            FormCollection fc = Application.OpenForms;

            foreach (Form loaderFrm in fc)
            {
                if (loaderFrm.GetType() == typeof(frmMain))
                {
                    loaderFrm.Show();
                    loaderFrm.WindowState = FormWindowState.Maximized;
                    allreadyLoaded = true;
                    break;
                }
            }

            if (!allreadyLoaded)
            {
                frmMain frm = new frmMain();
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
            }
            this.Close();
        }

        private void SetupOnClick(object sender, EventArgs e)
        {
            ActivateMainForm();

        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
                trayMenu.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
