namespace WOT.Stats
{
    partial class CompareSelection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompareSelection));
            this.BTN_OK = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_CLOSE = new DevExpress.XtraEditors.SimpleButton();
            this.OldDossier = new DevExpress.XtraEditors.RadioGroup();
            this.STR_OLDDOSSIER = new DevExpress.XtraEditors.GroupControl();
            this.oldSelection = new DevExpress.XtraEditors.ComboBoxEdit();
            this.NewDossier = new DevExpress.XtraEditors.RadioGroup();
            this.STR_NEWDOSSIER = new DevExpress.XtraEditors.GroupControl();
            this.newSelection = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BTN_USEDEFAULT = new DevExpress.XtraEditors.SimpleButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.OldDossier.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_OLDDOSSIER)).BeginInit();
            this.STR_OLDDOSSIER.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.oldSelection.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewDossier.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_NEWDOSSIER)).BeginInit();
            this.STR_NEWDOSSIER.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newSelection.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BTN_OK
            // 
            this.BTN_OK.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_OK.Appearance.Options.UseFont = true;
            this.BTN_OK.Location = new System.Drawing.Point(265, 167);
            this.BTN_OK.LookAndFeel.SkinName = "Office 2010 Black";
            this.BTN_OK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BTN_OK.Name = "BTN_OK";
            this.BTN_OK.Size = new System.Drawing.Size(75, 23);
            this.BTN_OK.TabIndex = 11;
            this.BTN_OK.Text = "Ok";
            this.BTN_OK.Click += new System.EventHandler(this.butApply_Click);
            // 
            // BTN_CLOSE
            // 
            this.BTN_CLOSE.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CLOSE.Appearance.Options.UseFont = true;
            this.BTN_CLOSE.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTN_CLOSE.Location = new System.Drawing.Point(346, 167);
            this.BTN_CLOSE.LookAndFeel.SkinName = "Office 2010 Black";
            this.BTN_CLOSE.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BTN_CLOSE.Name = "BTN_CLOSE";
            this.BTN_CLOSE.Size = new System.Drawing.Size(75, 23);
            this.BTN_CLOSE.TabIndex = 10;
            this.BTN_CLOSE.Text = "Close";
            this.BTN_CLOSE.Click += new System.EventHandler(this.butClose_Click);
            // 
            // OldDossier
            // 
            this.OldDossier.Location = new System.Drawing.Point(5, 18);
            this.OldDossier.Name = "OldDossier";
            this.OldDossier.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.OldDossier.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OldDossier.Properties.Appearance.Options.UseBackColor = true;
            this.OldDossier.Properties.Appearance.Options.UseFont = true;
            this.OldDossier.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.OldDossier.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "Current"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "Previous"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "1 Week"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("3", "2 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("4", "Custom")});
            this.OldDossier.Size = new System.Drawing.Size(190, 105);
            this.OldDossier.TabIndex = 12;
            this.OldDossier.SelectedIndexChanged += new System.EventHandler(this.OldDossier_SelectedIndexChanged);
            // 
            // STR_OLDDOSSIER
            // 
            this.STR_OLDDOSSIER.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.STR_OLDDOSSIER.Controls.Add(this.oldSelection);
            this.STR_OLDDOSSIER.Controls.Add(this.OldDossier);
            this.STR_OLDDOSSIER.Location = new System.Drawing.Point(12, 12);
            this.STR_OLDDOSSIER.LookAndFeel.SkinName = "Office 2010 Black";
            this.STR_OLDDOSSIER.LookAndFeel.UseDefaultLookAndFeel = false;
            this.STR_OLDDOSSIER.Name = "STR_OLDDOSSIER";
            this.STR_OLDDOSSIER.Size = new System.Drawing.Size(200, 149);
            this.STR_OLDDOSSIER.TabIndex = 15;
            this.STR_OLDDOSSIER.Text = "Old Dossier";
            // 
            // oldSelection
            // 
            this.oldSelection.Location = new System.Drawing.Point(30, 117);
            this.oldSelection.Name = "oldSelection";
            this.oldSelection.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.oldSelection.Properties.Appearance.Options.UseFont = true;
            this.oldSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.oldSelection.Properties.LookAndFeel.SkinName = "Office 2010 Black";
            this.oldSelection.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.oldSelection.Size = new System.Drawing.Size(135, 22);
            this.oldSelection.TabIndex = 13;
            this.oldSelection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.oldSelection_KeyPress_1);
            // 
            // NewDossier
            // 
            this.NewDossier.Location = new System.Drawing.Point(5, 18);
            this.NewDossier.Name = "NewDossier";
            this.NewDossier.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.NewDossier.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewDossier.Properties.Appearance.Options.UseBackColor = true;
            this.NewDossier.Properties.Appearance.Options.UseFont = true;
            this.NewDossier.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.NewDossier.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "Current"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "Previous"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "1 Week"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("3", "2 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("4", "Custom")});
            this.NewDossier.Size = new System.Drawing.Size(190, 105);
            this.NewDossier.TabIndex = 16;
            this.NewDossier.SelectedIndexChanged += new System.EventHandler(this.NewDossier_SelectedIndexChanged);
            // 
            // STR_NEWDOSSIER
            // 
            this.STR_NEWDOSSIER.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.STR_NEWDOSSIER.Controls.Add(this.newSelection);
            this.STR_NEWDOSSIER.Controls.Add(this.NewDossier);
            this.STR_NEWDOSSIER.Location = new System.Drawing.Point(218, 12);
            this.STR_NEWDOSSIER.LookAndFeel.SkinName = "Office 2010 Black";
            this.STR_NEWDOSSIER.LookAndFeel.UseDefaultLookAndFeel = false;
            this.STR_NEWDOSSIER.Name = "STR_NEWDOSSIER";
            this.STR_NEWDOSSIER.Size = new System.Drawing.Size(200, 149);
            this.STR_NEWDOSSIER.TabIndex = 17;
            this.STR_NEWDOSSIER.Text = "New Dossier";
            // 
            // newSelection
            // 
            this.newSelection.Location = new System.Drawing.Point(29, 117);
            this.newSelection.Name = "newSelection";
            this.newSelection.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSelection.Properties.Appearance.Options.UseFont = true;
            this.newSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.newSelection.Properties.LookAndFeel.SkinName = "Office 2010 Black";
            this.newSelection.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.newSelection.Size = new System.Drawing.Size(135, 22);
            this.newSelection.TabIndex = 17;
            this.newSelection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.newSelection_KeyPress_1);
            // 
            // BTN_USEDEFAULT
            // 
            this.BTN_USEDEFAULT.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_USEDEFAULT.Appearance.Options.UseFont = true;
            this.BTN_USEDEFAULT.AutoWidthInLayoutControl = true;
            this.BTN_USEDEFAULT.Location = new System.Drawing.Point(17, 167);
            this.BTN_USEDEFAULT.LookAndFeel.SkinName = "Office 2010 Black";
            this.BTN_USEDEFAULT.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BTN_USEDEFAULT.Name = "BTN_USEDEFAULT";
            this.BTN_USEDEFAULT.Size = new System.Drawing.Size(103, 23);
            this.BTN_USEDEFAULT.TabIndex = 18;
            this.BTN_USEDEFAULT.Text = "Use Default";
            this.BTN_USEDEFAULT.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // CompareSelection
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BTN_CLOSE;
            this.ClientSize = new System.Drawing.Size(433, 198);
            this.Controls.Add(this.BTN_USEDEFAULT);
            this.Controls.Add(this.STR_NEWDOSSIER);
            this.Controls.Add(this.STR_OLDDOSSIER);
            this.Controls.Add(this.BTN_CLOSE);
            this.Controls.Add(this.BTN_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2010 Black";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CompareSelection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare Selection";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.CompareSelection_HelpButtonClicked);
            this.Load += new System.EventHandler(this.CompareSelection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OldDossier.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_OLDDOSSIER)).EndInit();
            this.STR_OLDDOSSIER.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.oldSelection.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewDossier.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_NEWDOSSIER)).EndInit();
            this.STR_NEWDOSSIER.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newSelection.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BTN_OK;
        private DevExpress.XtraEditors.SimpleButton BTN_CLOSE;
        private DevExpress.XtraEditors.RadioGroup OldDossier;
        private DevExpress.XtraEditors.GroupControl STR_OLDDOSSIER;
        private DevExpress.XtraEditors.ComboBoxEdit oldSelection;
        private DevExpress.XtraEditors.RadioGroup NewDossier;
        private DevExpress.XtraEditors.GroupControl STR_NEWDOSSIER;
        private DevExpress.XtraEditors.ComboBoxEdit newSelection;
        private DevExpress.XtraEditors.SimpleButton BTN_USEDEFAULT;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}