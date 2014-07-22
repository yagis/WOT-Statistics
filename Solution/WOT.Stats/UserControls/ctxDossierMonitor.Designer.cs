namespace WOT.Stats
{
    partial class ctxDossierMonitor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CHKBOX_MONITOR = new DevExpress.XtraEditors.CheckEdit();
            this.CHKBOX_MINIMIZE = new DevExpress.XtraEditors.CheckEdit();
            this.CHKBOX_AUTOSTART = new DevExpress.XtraEditors.CheckEdit();
            this.STR_RESET = new DevExpress.XtraEditors.LabelControl();
            this.comboRestart = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CHKBOX_MINIMIZETOTRAY = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MONITOR.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MINIMIZE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_AUTOSTART.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboRestart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MINIMIZETOTRAY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CHKBOX_MONITOR
            // 
            this.CHKBOX_MONITOR.Location = new System.Drawing.Point(3, 4);
            this.CHKBOX_MONITOR.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_MONITOR.Name = "CHKBOX_MONITOR";
            this.CHKBOX_MONITOR.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_MONITOR.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_MONITOR.Properties.AutoWidth = true;
            this.CHKBOX_MONITOR.Properties.Caption = "Monitor Dossier On Startup";
            this.CHKBOX_MONITOR.Size = new System.Drawing.Size(181, 21);
            this.CHKBOX_MONITOR.TabIndex = 0;
            this.CHKBOX_MONITOR.CheckedChanged += new System.EventHandler(this.checkAutoMonitor_CheckedChanged);
            // 
            // CHKBOX_MINIMIZE
            // 
            this.CHKBOX_MINIMIZE.Location = new System.Drawing.Point(3, 31);
            this.CHKBOX_MINIMIZE.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_MINIMIZE.Name = "CHKBOX_MINIMIZE";
            this.CHKBOX_MINIMIZE.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_MINIMIZE.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_MINIMIZE.Properties.AutoWidth = true;
            this.CHKBOX_MINIMIZE.Properties.Caption = "Minimize App On Startup";
            this.CHKBOX_MINIMIZE.Size = new System.Drawing.Size(169, 21);
            this.CHKBOX_MINIMIZE.TabIndex = 1;
            this.CHKBOX_MINIMIZE.CheckedChanged += new System.EventHandler(this.checkMinimze_CheckedChanged);
            // 
            // CHKBOX_AUTOSTART
            // 
            this.CHKBOX_AUTOSTART.Location = new System.Drawing.Point(3, 85);
            this.CHKBOX_AUTOSTART.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_AUTOSTART.Name = "CHKBOX_AUTOSTART";
            this.CHKBOX_AUTOSTART.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_AUTOSTART.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_AUTOSTART.Properties.AutoWidth = true;
            this.CHKBOX_AUTOSTART.Properties.Caption = "Start App When Windows Starts";
            this.CHKBOX_AUTOSTART.Size = new System.Drawing.Size(212, 21);
            this.CHKBOX_AUTOSTART.TabIndex = 2;
            this.CHKBOX_AUTOSTART.CheckedChanged += new System.EventHandler(this.checkWindowsStart_CheckedChanged);
            // 
            // STR_RESET
            // 
            this.STR_RESET.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_RESET.Location = new System.Drawing.Point(5, 117);
            this.STR_RESET.Name = "STR_RESET";
            this.STR_RESET.Size = new System.Drawing.Size(67, 16);
            this.STR_RESET.TabIndex = 3;
            this.STR_RESET.Text = "Restart At :";
            // 
            // comboRestart
            // 
            this.comboRestart.Location = new System.Drawing.Point(78, 112);
            this.comboRestart.Name = "comboRestart";
            this.comboRestart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.comboRestart.Properties.Appearance.Options.UseFont = true;
            this.comboRestart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboRestart.Properties.Items.AddRange(new object[] {
            "12:00 AM",
            "01:00 AM",
            "02:00 AM",
            "03:00 AM",
            "04:00 AM",
            "05:00 AM",
            "06:00 AM",
            "07:00 AM",
            "08:00 AM",
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "12:00 PM",
            "01:00 PM",
            "02:00 PM",
            "03:00 PM",
            "04:00 PM",
            "05:00 PM",
            "06:00 PM",
            "07:00 PM",
            "08:00 PM",
            "09:00 PM",
            "10:00 PM",
            "11:00 PM"});
            this.comboRestart.Properties.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboRestart_Properties_KeyPress);
            this.comboRestart.Size = new System.Drawing.Size(100, 22);
            this.comboRestart.TabIndex = 4;
            this.comboRestart.SelectedValueChanged += new System.EventHandler(this.comboBoxEdit1_SelectedValueChanged);
            // 
            // CHKBOX_MINIMIZETOTRAY
            // 
            this.CHKBOX_MINIMIZETOTRAY.Location = new System.Drawing.Point(3, 58);
            this.CHKBOX_MINIMIZETOTRAY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CHKBOX_MINIMIZETOTRAY.Name = "CHKBOX_MINIMIZETOTRAY";
            this.CHKBOX_MINIMIZETOTRAY.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_MINIMIZETOTRAY.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_MINIMIZETOTRAY.Properties.AutoWidth = true;
            this.CHKBOX_MINIMIZETOTRAY.Properties.Caption = "Minimize To System Tray";
            this.CHKBOX_MINIMIZETOTRAY.Size = new System.Drawing.Size(171, 21);
            this.CHKBOX_MINIMIZETOTRAY.TabIndex = 5;
            this.CHKBOX_MINIMIZETOTRAY.CheckedChanged += new System.EventHandler(this.CHKBOX_MINIMIZETOTRAY_CheckedChanged);
            // 
            // ctxDossierMonitor
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CHKBOX_MINIMIZETOTRAY);
            this.Controls.Add(this.comboRestart);
            this.Controls.Add(this.STR_RESET);
            this.Controls.Add(this.CHKBOX_AUTOSTART);
            this.Controls.Add(this.CHKBOX_MINIMIZE);
            this.Controls.Add(this.CHKBOX_MONITOR);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ctxDossierMonitor";
            this.Size = new System.Drawing.Size(304, 248);
            this.Load += new System.EventHandler(this.ctxDossierMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MONITOR.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MINIMIZE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_AUTOSTART.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboRestart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_MINIMIZETOTRAY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit CHKBOX_MONITOR;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_MINIMIZE;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_AUTOSTART;
        private DevExpress.XtraEditors.LabelControl STR_RESET;
        private DevExpress.XtraEditors.ComboBoxEdit comboRestart;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_MINIMIZETOTRAY;
    }
}
