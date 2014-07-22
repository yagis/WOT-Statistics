using DevExpress.XtraEditors;
namespace WOT.Stats
{
    partial class GraphSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphSetup));
            this.textName = new System.Windows.Forms.TextBox();
            this.STR_CHARTNAME = new DevExpress.XtraEditors.LabelControl();
            this.STR_CATEGORY = new DevExpress.XtraEditors.LabelControl();
            this.comboDataField = new DevExpress.XtraEditors.ComboBoxEdit();
            this.butSave = new DevExpress.XtraEditors.SimpleButton();
            this.butClose = new DevExpress.XtraEditors.SimpleButton();
            this.listTanks = new System.Windows.Forms.ListView();
            this.radioGroupPeriod = new DevExpress.XtraEditors.RadioGroup();
            this.STR_GRPPERIOD = new DevExpress.XtraEditors.GroupControl();
            this.STR_GRPSTATSBASE = new DevExpress.XtraEditors.GroupControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.comboDataField.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupPeriod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_GRPPERIOD)).BeginInit();
            this.STR_GRPPERIOD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.STR_GRPSTATSBASE)).BeginInit();
            this.STR_GRPSTATSBASE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(95, 8);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(165, 21);
            this.textName.TabIndex = 1;
            // 
            // STR_CHARTNAME
            // 
            this.STR_CHARTNAME.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_CHARTNAME.Location = new System.Drawing.Point(16, 10);
            this.STR_CHARTNAME.Name = "STR_CHARTNAME";
            this.STR_CHARTNAME.Size = new System.Drawing.Size(73, 16);
            this.STR_CHARTNAME.TabIndex = 2;
            this.STR_CHARTNAME.Text = "Chart Name:";
            // 
            // STR_CATEGORY
            // 
            this.STR_CATEGORY.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_CATEGORY.Location = new System.Drawing.Point(16, 335);
            this.STR_CATEGORY.Name = "STR_CATEGORY";
            this.STR_CATEGORY.Size = new System.Drawing.Size(56, 16);
            this.STR_CATEGORY.TabIndex = 8;
            this.STR_CATEGORY.Text = "Category:";
            this.STR_CATEGORY.Click += new System.EventHandler(this.label4_Click);
            // 
            // comboDataField
            // 
            this.comboDataField.Location = new System.Drawing.Point(75, 332);
            this.comboDataField.Name = "comboDataField";
            this.comboDataField.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboDataField.Properties.Appearance.Options.UseFont = true;
            this.comboDataField.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboDataField.Properties.Items.AddRange(new object[] {
            "Damage Dealt : Average",
            "Damage Dealt : Total",
            "Damage Taken : Average",
            "Damage Taken : Total",
            "Efficiency",
            "Battle Rating",
            "WN7",
            "WN8",
            "Experience : Average",
            "Experience : Total",
            "Kill Ratio",
            "Kill/Death Ratio",
            "Win Ratio"});
            this.comboDataField.Size = new System.Drawing.Size(187, 22);
            this.comboDataField.TabIndex = 9;
            // 
            // butSave
            // 
            this.butSave.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butSave.Appearance.Options.UseFont = true;
            this.butSave.Location = new System.Drawing.Point(55, 401);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 12;
            this.butSave.Text = "Save";
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butClose
            // 
            this.butClose.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butClose.Appearance.Options.UseFont = true;
            this.butClose.Location = new System.Drawing.Point(136, 400);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 13;
            this.butClose.Text = "Close";
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // listTanks
            // 
            this.listTanks.Location = new System.Drawing.Point(271, 6);
            this.listTanks.Name = "listTanks";
            this.listTanks.Size = new System.Drawing.Size(403, 418);
            this.listTanks.TabIndex = 14;
            this.listTanks.UseCompatibleStateImageBehavior = false;
            // 
            // radioGroupPeriod
            // 
            this.radioGroupPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroupPeriod.Location = new System.Drawing.Point(2, 24);
            this.radioGroupPeriod.Name = "radioGroupPeriod";
            this.radioGroupPeriod.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroupPeriod.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroupPeriod.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroupPeriod.Properties.Appearance.Options.UseFont = true;
            this.radioGroupPeriod.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroupPeriod.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("7", "1 Week"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("14", "2 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("21", "3 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("28", "4 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("35", "5 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("42", "6 Weeks"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("92", "3 Months"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("184", "6 Months"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("365", "Year")});
            this.radioGroupPeriod.Size = new System.Drawing.Size(254, 203);
            this.radioGroupPeriod.TabIndex = 15;
            // 
            // STR_GRPPERIOD
            // 
            this.STR_GRPPERIOD.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_GRPPERIOD.Appearance.Options.UseFont = true;
            this.STR_GRPPERIOD.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_GRPPERIOD.AppearanceCaption.Options.UseFont = true;
            this.STR_GRPPERIOD.Controls.Add(this.radioGroupPeriod);
            this.STR_GRPPERIOD.Location = new System.Drawing.Point(4, 97);
            this.STR_GRPPERIOD.Name = "STR_GRPPERIOD";
            this.STR_GRPPERIOD.Size = new System.Drawing.Size(258, 229);
            this.STR_GRPPERIOD.TabIndex = 16;
            this.STR_GRPPERIOD.Text = "Period";
            // 
            // STR_GRPSTATSBASE
            // 
            this.STR_GRPSTATSBASE.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STR_GRPSTATSBASE.AppearanceCaption.Options.UseFont = true;
            this.STR_GRPSTATSBASE.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.STR_GRPSTATSBASE.Controls.Add(this.radioGroup1);
            this.STR_GRPSTATSBASE.Location = new System.Drawing.Point(4, 33);
            this.STR_GRPSTATSBASE.Name = "STR_GRPSTATSBASE";
            this.STR_GRPSTATSBASE.Size = new System.Drawing.Size(258, 58);
            this.STR_GRPSTATSBASE.TabIndex = 17;
            this.STR_GRPSTATSBASE.Text = "Statistic Base";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup1.Location = new System.Drawing.Point(2, 24);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Overall", "Overall"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Tanks", "Tanks")});
            this.radioGroup1.Size = new System.Drawing.Size(254, 32);
            this.radioGroup1.TabIndex = 16;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // GraphSetup
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 431);
            this.Controls.Add(this.STR_GRPSTATSBASE);
            this.Controls.Add(this.STR_GRPPERIOD);
            this.Controls.Add(this.listTanks);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.butSave);
            this.Controls.Add(this.comboDataField);
            this.Controls.Add(this.STR_CATEGORY);
            this.Controls.Add(this.STR_CHARTNAME);
            this.Controls.Add(this.textName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2010 Black";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GraphSetup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chart Setup";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.GraphSetup_HelpButtonClicked);
            this.Load += new System.EventHandler(this.GraphSetup_Load);
            this.ResizeEnd += new System.EventHandler(this.GraphSetup_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.comboDataField.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupPeriod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.STR_GRPPERIOD)).EndInit();
            this.STR_GRPPERIOD.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.STR_GRPSTATSBASE)).EndInit();
            this.STR_GRPSTATSBASE.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textName;
        private LabelControl STR_CHARTNAME;
        private LabelControl STR_CATEGORY;
        private ComboBoxEdit comboDataField;
        private SimpleButton butSave;
        private SimpleButton butClose;
        private System.Windows.Forms.ListView listTanks;
        private RadioGroup radioGroupPeriod;
        private GroupControl STR_GRPPERIOD;
        private GroupControl STR_GRPSTATSBASE;
        private RadioGroup radioGroup1;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}