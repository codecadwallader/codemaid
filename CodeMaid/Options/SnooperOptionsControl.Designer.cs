namespace SteveCadwallader.CodeMaid.Options
{
    partial class SnooperOptionsControl
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
            this.complexityGroupBox = new System.Windows.Forms.GroupBox();
            this.warningThresholdLabel = new System.Windows.Forms.Label();
            this.warningThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.alertThresholdLabel = new System.Windows.Forms.Label();
            this.alertThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.complexityGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningThresholdUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertThresholdUpDown)).BeginInit();
            this.SuspendLayout();
            //
            // complexityGroupBox
            //
            this.complexityGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.complexityGroupBox.Controls.Add(this.warningThresholdLabel);
            this.complexityGroupBox.Controls.Add(this.warningThresholdUpDown);
            this.complexityGroupBox.Controls.Add(this.alertThresholdLabel);
            this.complexityGroupBox.Controls.Add(this.alertThresholdUpDown);
            this.complexityGroupBox.Location = new System.Drawing.Point(3, 3);
            this.complexityGroupBox.Name = "complexityGroupBox";
            this.complexityGroupBox.Size = new System.Drawing.Size(344, 77);
            this.complexityGroupBox.TabIndex = 0;
            this.complexityGroupBox.TabStop = false;
            this.complexityGroupBox.Text = "Complexity (McCabe)";
            //
            // warningThresholdLabel
            //
            this.warningThresholdLabel.AutoSize = true;
            this.warningThresholdLabel.Location = new System.Drawing.Point(9, 23);
            this.warningThresholdLabel.Name = "warningThresholdLabel";
            this.warningThresholdLabel.Size = new System.Drawing.Size(93, 13);
            this.warningThresholdLabel.TabIndex = 0;
            this.warningThresholdLabel.Text = "Warning threshold";
            //
            // warningThresholdUpDown
            //
            this.warningThresholdUpDown.Location = new System.Drawing.Point(110, 21);
            this.warningThresholdUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.warningThresholdUpDown.Name = "warningThresholdUpDown";
            this.warningThresholdUpDown.Size = new System.Drawing.Size(50, 20);
            this.warningThresholdUpDown.TabIndex = 1;
            this.warningThresholdUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.warningThresholdUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.warningThresholdUpDown.ValueChanged += new System.EventHandler(this.warningThresholdUpDown_ValueChanged);
            //
            // alertThresholdLabel
            //
            this.alertThresholdLabel.AutoSize = true;
            this.alertThresholdLabel.Location = new System.Drawing.Point(9, 49);
            this.alertThresholdLabel.Name = "alertThresholdLabel";
            this.alertThresholdLabel.Size = new System.Drawing.Size(74, 13);
            this.alertThresholdLabel.TabIndex = 2;
            this.alertThresholdLabel.Text = "Alert threshold";
            //
            // alertThresholdUpDown
            //
            this.alertThresholdUpDown.Location = new System.Drawing.Point(110, 47);
            this.alertThresholdUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.alertThresholdUpDown.Name = "alertThresholdUpDown";
            this.alertThresholdUpDown.Size = new System.Drawing.Size(50, 20);
            this.alertThresholdUpDown.TabIndex = 3;
            this.alertThresholdUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.alertThresholdUpDown.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.alertThresholdUpDown.ValueChanged += new System.EventHandler(this.alertThresholdUpDown_ValueChanged);
            //
            // SnooperOptionsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.complexityGroupBox);
            this.Name = "SnooperOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.complexityGroupBox.ResumeLayout(false);
            this.complexityGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningThresholdUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertThresholdUpDown)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox complexityGroupBox;
        private System.Windows.Forms.Label warningThresholdLabel;
        private System.Windows.Forms.NumericUpDown warningThresholdUpDown;
        private System.Windows.Forms.Label alertThresholdLabel;
        private System.Windows.Forms.NumericUpDown alertThresholdUpDown;
    }
}