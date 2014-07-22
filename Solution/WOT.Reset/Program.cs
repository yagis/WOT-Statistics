using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace WOTStatistics.Reset
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FixForm());
        }
    }

    class FixForm : Form
    {
        public FixForm()
        {
            Visible = false;
            ShowInTaskbar = false;
            Width = 0;
            Height = 0;
            WindowState = FormWindowState.Minimized;
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (MessageBox.Show("Do you really want to reset your data?", "WOT Statistics", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Close();
            }



            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "User.xml");
            File.Copy(fileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics", "User_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml"));
            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("WorldOfTanks",
                                                                                       new XElement("CustomGroups"),
                                                                                       new XElement("Graphs"),
                                                                                       new XElement("Players"),
                                                                                       new XElement("FTPDetails")));
            doc.Save(fileName);

            foreach (string  dir in Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WOT Statistics"), "Hist_*"))
            {
                Directory.Move(dir, dir.Replace("Hist", "Backup_" + DateTime.Now.ToString("yyMMddHHmmss")));
                //MessageBox.Show(dir);
            }

            MessageBox.Show("User data reset completed. Please restart now WOT Statistics!", "WOT Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }
    }
}
