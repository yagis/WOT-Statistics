namespace WOT.Stats
{
    partial class ctxKillCounts
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
            this.CHKBOX_TIERTOTAL = new DevExpress.XtraEditors.CheckEdit();
            this.CHKBOX_ROWTOTAL = new DevExpress.XtraEditors.CheckEdit();
            this.CHKBOX_COLTOTALS = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_TIERTOTAL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ROWTOTAL.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_COLTOTALS.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CHKBOX_TIERTOTAL
            // 
            this.CHKBOX_TIERTOTAL.Location = new System.Drawing.Point(3, 4);
            this.CHKBOX_TIERTOTAL.Name = "CHKBOX_TIERTOTAL";
            this.CHKBOX_TIERTOTAL.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_TIERTOTAL.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_TIERTOTAL.Properties.AutoWidth = true;
            this.CHKBOX_TIERTOTAL.Properties.Caption = "Display Tier Totals";
            this.CHKBOX_TIERTOTAL.Size = new System.Drawing.Size(130, 21);
            this.CHKBOX_TIERTOTAL.TabIndex = 1;
            this.CHKBOX_TIERTOTAL.CheckedChanged += new System.EventHandler(this.checkTierTotals_CheckedChanged);
            // 
            // CHKBOX_ROWTOTAL
            // 
            this.CHKBOX_ROWTOTAL.Location = new System.Drawing.Point(3, 29);
            this.CHKBOX_ROWTOTAL.Name = "CHKBOX_ROWTOTAL";
            this.CHKBOX_ROWTOTAL.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_ROWTOTAL.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_ROWTOTAL.Properties.AutoWidth = true;
            this.CHKBOX_ROWTOTAL.Properties.Caption = "Display Row Totals";
            this.CHKBOX_ROWTOTAL.Size = new System.Drawing.Size(134, 21);
            this.CHKBOX_ROWTOTAL.TabIndex = 2;
            this.CHKBOX_ROWTOTAL.CheckedChanged += new System.EventHandler(this.checkRowTotals_CheckedChanged);
            // 
            // CHKBOX_COLTOTALS
            // 
            this.CHKBOX_COLTOTALS.Location = new System.Drawing.Point(3, 54);
            this.CHKBOX_COLTOTALS.Name = "CHKBOX_COLTOTALS";
            this.CHKBOX_COLTOTALS.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHKBOX_COLTOTALS.Properties.Appearance.Options.UseFont = true;
            this.CHKBOX_COLTOTALS.Properties.AutoWidth = true;
            this.CHKBOX_COLTOTALS.Properties.Caption = "Display Column Totals";
            this.CHKBOX_COLTOTALS.Size = new System.Drawing.Size(153, 21);
            this.CHKBOX_COLTOTALS.TabIndex = 3;
            this.CHKBOX_COLTOTALS.CheckedChanged += new System.EventHandler(this.checkColumnTotals_CheckedChanged);
            // 
            // ctxKillCounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CHKBOX_COLTOTALS);
            this.Controls.Add(this.CHKBOX_ROWTOTAL);
            this.Controls.Add(this.CHKBOX_TIERTOTAL);
            this.Name = "ctxKillCounts";
            this.Size = new System.Drawing.Size(586, 150);
            this.Load += new System.EventHandler(this.ctxKillCounts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_TIERTOTAL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_ROWTOTAL.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CHKBOX_COLTOTALS.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit CHKBOX_TIERTOTAL;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_ROWTOTAL;
        private DevExpress.XtraEditors.CheckEdit CHKBOX_COLTOTALS;
    }
}
