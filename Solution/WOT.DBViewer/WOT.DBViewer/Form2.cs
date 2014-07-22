using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WOT.DBViewer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fbd = new OpenFileDialog())
            {
                fbd.Filter = "WOT Stats db (WOTSStore.db)|WOTSStore.db.dat";

                if (fbd.ShowDialog(this) == DialogResult.OK)
                {
                    textBox1.Text = fbd.FileName;
                }
            }
        }
    }
}
