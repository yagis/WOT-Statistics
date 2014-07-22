namespace WOT.Stats
{
    partial class ctxLastPlayedGames
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
            this.STR_SHOWLASTBATTLES_1 = new DevExpress.XtraEditors.LabelControl();
            this.textLastPlayedMaxNo = new DevExpress.XtraEditors.TextEdit();
            this.STR_SHOWLASTBATTLES_2 = new DevExpress.XtraEditors.LabelControl();
            this.CHKBOX_COMPARE = new DevExpress.XtraEditors.CheckEdit();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.chkAutoStartSession = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoAfterXHoursMessage = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoOnStartUpMessage = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoOnStartUp = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoAfterXHours = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoAfterXBattles = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoAfterXBattlesMessage = new DevExpress.XtraEditors.CheckEdit();
            this.lblAutoAfterXHours = new DevExpress.XtraEditors.LabelControl();
            this.txtAutoAfterXBattles = new DevExpress.XtraEditors.TextEdit();
            this.lblAutoAfterXBattles = new DevExpress.XtraEditors.LabelControl();
            this.txtAutoAfterXHours = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textLastPlayedMaxNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_COMPARE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoStartSession.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXHoursMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOnStartUpMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOnStartUp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXHours.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXBattles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXBattlesMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoAfterXBattles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoAfterXHours.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // STR_SHOWLASTBATTLES_1
            // 
            this.STR_SHOWLASTBATTLES_1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_SHOWLASTBATTLES_1.Location = new System.Drawing.Point(3, 4);
            this.STR_SHOWLASTBATTLES_1.Name = "STR_SHOWLASTBATTLES_1";
            this.STR_SHOWLASTBATTLES_1.Size = new System.Drawing.Size(57, 16);
            this.STR_SHOWLASTBATTLES_1.TabIndex = 1;
            this.STR_SHOWLASTBATTLES_1.Text = "Show last";
            // 
            // textLastPlayedMaxNo
            // 
            this.textLastPlayedMaxNo.EditValue = "";
            this.textLastPlayedMaxNo.Location = new System.Drawing.Point(71, 1);
            this.textLastPlayedMaxNo.Name = "textLastPlayedMaxNo";
            this.textLastPlayedMaxNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLastPlayedMaxNo.Properties.Appearance.Options.UseFont = true;
            this.textLastPlayedMaxNo.Properties.Appearance.Options.UseTextOptions = true;
            this.textLastPlayedMaxNo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.textLastPlayedMaxNo.Size = new System.Drawing.Size(45, 22);
            this.textLastPlayedMaxNo.TabIndex = 2;
            this.textLastPlayedMaxNo.EditValueChanged += new System.EventHandler(this.textLastPlayedMaxNo_EditValueChanged);
            this.textLastPlayedMaxNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEdit1_KeyPress);
            // 
            // STR_SHOWLASTBATTLES_2
            // 
            this.STR_SHOWLASTBATTLES_2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_SHOWLASTBATTLES_2.Location = new System.Drawing.Point(122, 4);
            this.STR_SHOWLASTBATTLES_2.Name = "STR_SHOWLASTBATTLES_2";
            this.STR_SHOWLASTBATTLES_2.Size = new System.Drawing.Size(39, 16);
            this.STR_SHOWLASTBATTLES_2.TabIndex = 3;
            this.STR_SHOWLASTBATTLES_2.Text = "battles";
            // 
            // CHKBOX_COMPARE
            // 
            this.CHKBOX_COMPARE.Location = new System.Drawing.Point(4, 26);
            this.CHKBOX_COMPARE.Name = "CHKBOX_COMPARE";
            this.CHKBOX_COMPARE.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_COMPARE.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_COMPARE.Properties.AutoWidth = true;
            this.CHKBOX_COMPARE.Properties.Caption = "Compare (Doesn\'t apply when grouped)";
            this.CHKBOX_COMPARE.Size = new System.Drawing.Size(249, 21);
            this.CHKBOX_COMPARE.TabIndex = 4;
            this.CHKBOX_COMPARE.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(24, 50);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "To Overall Stats"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "To Tank Stats")});
            this.radioGroup1.Size = new System.Drawing.Size(816, 48);
            this.radioGroup1.TabIndex = 5;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // chkAutoStartSession
            // 
            this.chkAutoStartSession.Location = new System.Drawing.Point(4, 101);
            this.chkAutoStartSession.Name = "chkAutoStartSession";
            this.chkAutoStartSession.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoStartSession.Properties.Appearance.Options.UseFont = true;
            this.chkAutoStartSession.Properties.AutoWidth = true;
            this.chkAutoStartSession.Properties.Caption = "Auto create new session";
            this.chkAutoStartSession.Size = new System.Drawing.Size(166, 21);
            this.chkAutoStartSession.TabIndex = 6;
            // 
            // chkAutoAfterXHoursMessage
            // 
            this.chkAutoAfterXHoursMessage.Location = new System.Drawing.Point(45, 145);
            this.chkAutoAfterXHoursMessage.Name = "chkAutoAfterXHoursMessage";
            this.chkAutoAfterXHoursMessage.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoAfterXHoursMessage.Properties.Appearance.Options.UseFont = true;
            this.chkAutoAfterXHoursMessage.Properties.AutoWidth = true;
            this.chkAutoAfterXHoursMessage.Properties.Caption = "Display message before creating new session";
            this.chkAutoAfterXHoursMessage.Size = new System.Drawing.Size(288, 21);
            this.chkAutoAfterXHoursMessage.TabIndex = 8;
            // 
            // chkAutoOnStartUpMessage
            // 
            this.chkAutoOnStartUpMessage.Location = new System.Drawing.Point(45, 185);
            this.chkAutoOnStartUpMessage.Name = "chkAutoOnStartUpMessage";
            this.chkAutoOnStartUpMessage.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoOnStartUpMessage.Properties.Appearance.Options.UseFont = true;
            this.chkAutoOnStartUpMessage.Properties.AutoWidth = true;
            this.chkAutoOnStartUpMessage.Properties.Caption = "Display message before creating new session";
            this.chkAutoOnStartUpMessage.Size = new System.Drawing.Size(288, 21);
            this.chkAutoOnStartUpMessage.TabIndex = 9;
            // 
            // chkAutoOnStartUp
            // 
            this.chkAutoOnStartUp.Location = new System.Drawing.Point(24, 165);
            this.chkAutoOnStartUp.Name = "chkAutoOnStartUp";
            this.chkAutoOnStartUp.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoOnStartUp.Properties.Appearance.Options.UseFont = true;
            this.chkAutoOnStartUp.Properties.AutoWidth = true;
            this.chkAutoOnStartUp.Properties.Caption = "On Startup";
            this.chkAutoOnStartUp.Size = new System.Drawing.Size(86, 21);
            this.chkAutoOnStartUp.TabIndex = 9;
            this.chkAutoOnStartUp.TabStop = false;
            // 
            // chkAutoAfterXHours
            // 
            this.chkAutoAfterXHours.Location = new System.Drawing.Point(24, 125);
            this.chkAutoAfterXHours.Name = "chkAutoAfterXHours";
            this.chkAutoAfterXHours.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoAfterXHours.Properties.Appearance.Options.UseFont = true;
            this.chkAutoAfterXHours.Properties.Caption = "";
            this.chkAutoAfterXHours.Size = new System.Drawing.Size(19, 21);
            this.chkAutoAfterXHours.TabIndex = 7;
            this.chkAutoAfterXHours.TabStop = false;
            this.chkAutoAfterXHours.CheckedChanged += new System.EventHandler(this.chkAutoAfterXHours_CheckedChanged_1);
            // 
            // chkAutoAfterXBattles
            // 
            this.chkAutoAfterXBattles.Location = new System.Drawing.Point(24, 211);
            this.chkAutoAfterXBattles.Name = "chkAutoAfterXBattles";
            this.chkAutoAfterXBattles.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoAfterXBattles.Properties.Appearance.Options.UseFont = true;
            this.chkAutoAfterXBattles.Properties.AutoWidth = true;
            this.chkAutoAfterXBattles.Properties.Caption = "After every ";
            this.chkAutoAfterXBattles.Size = new System.Drawing.Size(88, 21);
            this.chkAutoAfterXBattles.TabIndex = 10;
            this.chkAutoAfterXBattles.TabStop = false;
            // 
            // chkAutoAfterXBattlesMessage
            // 
            this.chkAutoAfterXBattlesMessage.Location = new System.Drawing.Point(45, 234);
            this.chkAutoAfterXBattlesMessage.Name = "chkAutoAfterXBattlesMessage";
            this.chkAutoAfterXBattlesMessage.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoAfterXBattlesMessage.Properties.Appearance.Options.UseFont = true;
            this.chkAutoAfterXBattlesMessage.Properties.AutoWidth = true;
            this.chkAutoAfterXBattlesMessage.Properties.Caption = "Display message before creating new session";
            this.chkAutoAfterXBattlesMessage.Size = new System.Drawing.Size(288, 21);
            this.chkAutoAfterXBattlesMessage.TabIndex = 11;
            // 
            // lblAutoAfterXHours
            // 
            this.lblAutoAfterXHours.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoAfterXHours.Location = new System.Drawing.Point(98, 127);
            this.lblAutoAfterXHours.Name = "lblAutoAfterXHours";
            this.lblAutoAfterXHours.Size = new System.Drawing.Size(122, 16);
            this.lblAutoAfterXHours.TabIndex = 13;
            this.lblAutoAfterXHours.Text = "hours after last battle";
            // 
            // txtAutoAfterXBattles
            // 
            this.txtAutoAfterXBattles.EditValue = "";
            this.txtAutoAfterXBattles.Location = new System.Drawing.Point(112, 210);
            this.txtAutoAfterXBattles.Name = "txtAutoAfterXBattles";
            this.txtAutoAfterXBattles.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutoAfterXBattles.Properties.Appearance.Options.UseFont = true;
            this.txtAutoAfterXBattles.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAutoAfterXBattles.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAutoAfterXBattles.Size = new System.Drawing.Size(45, 22);
            this.txtAutoAfterXBattles.TabIndex = 14;
            this.txtAutoAfterXBattles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoAfterXBattles_KeyPress);
            this.txtAutoAfterXBattles.Leave += new System.EventHandler(this.txtAutoAfterXBattles_Leave);
            // 
            // lblAutoAfterXBattles
            // 
            this.lblAutoAfterXBattles.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoAfterXBattles.Location = new System.Drawing.Point(163, 213);
            this.lblAutoAfterXBattles.Name = "lblAutoAfterXBattles";
            this.lblAutoAfterXBattles.Size = new System.Drawing.Size(39, 16);
            this.lblAutoAfterXBattles.TabIndex = 15;
            this.lblAutoAfterXBattles.Text = "battles";
            // 
            // txtAutoAfterXHours
            // 
            this.txtAutoAfterXHours.EditValue = "";
            this.txtAutoAfterXHours.Location = new System.Drawing.Point(47, 124);
            this.txtAutoAfterXHours.Name = "txtAutoAfterXHours";
            this.txtAutoAfterXHours.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAutoAfterXHours.Properties.Appearance.Options.UseFont = true;
            this.txtAutoAfterXHours.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAutoAfterXHours.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAutoAfterXHours.Size = new System.Drawing.Size(45, 22);
            this.txtAutoAfterXHours.TabIndex = 12;
            this.txtAutoAfterXHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAutoAfterXHours_KeyPress);
            this.txtAutoAfterXHours.Leave += new System.EventHandler(this.txtAutoAfterXHours_Leave);
            // 
            // ctxLastPlayedGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAutoAfterXBattles);
            this.Controls.Add(this.txtAutoAfterXBattles);
            this.Controls.Add(this.lblAutoAfterXHours);
            this.Controls.Add(this.txtAutoAfterXHours);
            this.Controls.Add(this.chkAutoAfterXBattlesMessage);
            this.Controls.Add(this.chkAutoAfterXBattles);
            this.Controls.Add(this.chkAutoOnStartUp);
            this.Controls.Add(this.chkAutoOnStartUpMessage);
            this.Controls.Add(this.chkAutoAfterXHoursMessage);
            this.Controls.Add(this.chkAutoStartSession);
            this.Controls.Add(this.radioGroup1);
            this.Controls.Add(this.CHKBOX_COMPARE);
            this.Controls.Add(this.STR_SHOWLASTBATTLES_2);
            this.Controls.Add(this.textLastPlayedMaxNo);
            this.Controls.Add(this.STR_SHOWLASTBATTLES_1);
            this.Controls.Add(this.chkAutoAfterXHours);
            this.Name = "ctxLastPlayedGames";
            this.Size = new System.Drawing.Size(405, 292);
            this.Load += new System.EventHandler(this.ctxLastPlayedGames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textLastPlayedMaxNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_COMPARE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoStartSession.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXHoursMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOnStartUpMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoOnStartUp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXHours.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXBattles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoAfterXBattlesMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoAfterXBattles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAutoAfterXHours.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl STR_SHOWLASTBATTLES_1;
        private DevExpress.XtraEditors.TextEdit textLastPlayedMaxNo;
        private DevExpress.XtraEditors.LabelControl STR_SHOWLASTBATTLES_2;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_COMPARE;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.CheckEdit chkAutoStartSession;
        private DevExpress.XtraEditors.CheckEdit chkAutoAfterXHoursMessage;
        private DevExpress.XtraEditors.CheckEdit chkAutoOnStartUpMessage;
        private DevExpress.XtraEditors.CheckEdit chkAutoOnStartUp;
        private DevExpress.XtraEditors.CheckEdit chkAutoAfterXHours;
        private DevExpress.XtraEditors.CheckEdit chkAutoAfterXBattles;
        private DevExpress.XtraEditors.CheckEdit chkAutoAfterXBattlesMessage;
        private DevExpress.XtraEditors.LabelControl lblAutoAfterXHours;
        private DevExpress.XtraEditors.TextEdit txtAutoAfterXBattles;
        private DevExpress.XtraEditors.LabelControl lblAutoAfterXBattles;
        private DevExpress.XtraEditors.TextEdit txtAutoAfterXHours;
    }
}
