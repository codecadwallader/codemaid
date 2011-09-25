namespace SteveCadwallader.CodeMaid.Options
{
    partial class SpadeOptionsControl
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
            this.displayGroupBox = new System.Windows.Forms.GroupBox();
            this.showItemMetadataCheckBox = new System.Windows.Forms.CheckBox();
            this.showItemMetadataExampleLabel = new System.Windows.Forms.Label();
            this.showItemComplexityCheckBox = new System.Windows.Forms.CheckBox();
            this.warningThresholdLabel = new System.Windows.Forms.Label();
            this.warningThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.alertThresholdLabel = new System.Windows.Forms.Label();
            this.alertThresholdUpDown = new System.Windows.Forms.NumericUpDown();
            this.navigationGroupBox = new System.Windows.Forms.GroupBox();
            this.navigationCenterLabel = new System.Windows.Forms.Label();
            this.navigationCenterOnWholeRadioButton = new System.Windows.Forms.RadioButton();
            this.navigationCenterOnNameRadioButton = new System.Windows.Forms.RadioButton();
            this.displayGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningThresholdUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertThresholdUpDown)).BeginInit();
            this.navigationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayGroupBox
            // 
            this.displayGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.displayGroupBox.Controls.Add(this.showItemMetadataCheckBox);
            this.displayGroupBox.Controls.Add(this.showItemMetadataExampleLabel);
            this.displayGroupBox.Controls.Add(this.showItemComplexityCheckBox);
            this.displayGroupBox.Controls.Add(this.warningThresholdLabel);
            this.displayGroupBox.Controls.Add(this.warningThresholdUpDown);
            this.displayGroupBox.Controls.Add(this.alertThresholdLabel);
            this.displayGroupBox.Controls.Add(this.alertThresholdUpDown);
            this.displayGroupBox.Location = new System.Drawing.Point(3, 56);
            this.displayGroupBox.Name = "displayGroupBox";
            this.displayGroupBox.Size = new System.Drawing.Size(344, 141);
            this.displayGroupBox.TabIndex = 1;
            this.displayGroupBox.TabStop = false;
            this.displayGroupBox.Text = "Display";
            // 
            // showItemMetadataCheckBox
            // 
            this.showItemMetadataCheckBox.AutoSize = true;
            this.showItemMetadataCheckBox.Location = new System.Drawing.Point(7, 20);
            this.showItemMetadataCheckBox.Name = "showItemMetadataCheckBox";
            this.showItemMetadataCheckBox.Size = new System.Drawing.Size(122, 17);
            this.showItemMetadataCheckBox.TabIndex = 0;
            this.showItemMetadataCheckBox.Text = "Show item metadata";
            this.showItemMetadataCheckBox.UseVisualStyleBackColor = true;
            this.showItemMetadataCheckBox.CheckedChanged += new System.EventHandler(this.showItemMetadataCheckBox_CheckedChanged);
            // 
            // showItemMetadataExampleLabel
            // 
            this.showItemMetadataExampleLabel.AutoSize = true;
            this.showItemMetadataExampleLabel.Location = new System.Drawing.Point(40, 40);
            this.showItemMetadataExampleLabel.Margin = new System.Windows.Forms.Padding(43, 0, 3, 0);
            this.showItemMetadataExampleLabel.Name = "showItemMetadataExampleLabel";
            this.showItemMetadataExampleLabel.Size = new System.Drawing.Size(111, 13);
            this.showItemMetadataExampleLabel.TabIndex = 1;
            this.showItemMetadataExampleLabel.Text = "Example: \"s\" for static";
            // 
            // showItemComplexityCheckBox
            // 
            this.showItemComplexityCheckBox.AutoSize = true;
            this.showItemComplexityCheckBox.Location = new System.Drawing.Point(7, 65);
            this.showItemComplexityCheckBox.Name = "showItemComplexityCheckBox";
            this.showItemComplexityCheckBox.Size = new System.Drawing.Size(148, 17);
            this.showItemComplexityCheckBox.TabIndex = 2;
            this.showItemComplexityCheckBox.Text = "Show McCabe complexity";
            this.showItemComplexityCheckBox.UseVisualStyleBackColor = true;
            this.showItemComplexityCheckBox.CheckedChanged += new System.EventHandler(this.showItemComplexityCheckBox_CheckedChanged);
            // 
            // warningThresholdLabel
            // 
            this.warningThresholdLabel.AutoSize = true;
            this.warningThresholdLabel.Location = new System.Drawing.Point(40, 85);
            this.warningThresholdLabel.Name = "warningThresholdLabel";
            this.warningThresholdLabel.Size = new System.Drawing.Size(93, 13);
            this.warningThresholdLabel.TabIndex = 3;
            this.warningThresholdLabel.Text = "Warning threshold";
            // 
            // warningThresholdUpDown
            // 
            this.warningThresholdUpDown.Location = new System.Drawing.Point(141, 83);
            this.warningThresholdUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.warningThresholdUpDown.Name = "warningThresholdUpDown";
            this.warningThresholdUpDown.Size = new System.Drawing.Size(50, 20);
            this.warningThresholdUpDown.TabIndex = 4;
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
            this.alertThresholdLabel.Location = new System.Drawing.Point(40, 111);
            this.alertThresholdLabel.Name = "alertThresholdLabel";
            this.alertThresholdLabel.Size = new System.Drawing.Size(74, 13);
            this.alertThresholdLabel.TabIndex = 5;
            this.alertThresholdLabel.Text = "Alert threshold";
            // 
            // alertThresholdUpDown
            // 
            this.alertThresholdUpDown.Location = new System.Drawing.Point(141, 109);
            this.alertThresholdUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.alertThresholdUpDown.Name = "alertThresholdUpDown";
            this.alertThresholdUpDown.Size = new System.Drawing.Size(50, 20);
            this.alertThresholdUpDown.TabIndex = 6;
            this.alertThresholdUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.alertThresholdUpDown.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.alertThresholdUpDown.ValueChanged += new System.EventHandler(this.alertThresholdUpDown_ValueChanged);
            // 
            // navigationGroupBox
            // 
            this.navigationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.navigationGroupBox.Controls.Add(this.navigationCenterLabel);
            this.navigationGroupBox.Controls.Add(this.navigationCenterOnWholeRadioButton);
            this.navigationGroupBox.Controls.Add(this.navigationCenterOnNameRadioButton);
            this.navigationGroupBox.Location = new System.Drawing.Point(3, 3);
            this.navigationGroupBox.Name = "navigationGroupBox";
            this.navigationGroupBox.Size = new System.Drawing.Size(344, 47);
            this.navigationGroupBox.TabIndex = 0;
            this.navigationGroupBox.TabStop = false;
            this.navigationGroupBox.Text = "Navigation";
            // 
            // navigationCenterLabel
            // 
            this.navigationCenterLabel.AutoSize = true;
            this.navigationCenterLabel.Location = new System.Drawing.Point(9, 23);
            this.navigationCenterLabel.Name = "navigationCenterLabel";
            this.navigationCenterLabel.Size = new System.Drawing.Size(139, 13);
            this.navigationCenterLabel.TabIndex = 0;
            this.navigationCenterLabel.Text = "When navigating, center on";
            // 
            // navigationCenterOnWholeRadioButton
            // 
            this.navigationCenterOnWholeRadioButton.AutoSize = true;
            this.navigationCenterOnWholeRadioButton.Location = new System.Drawing.Point(154, 21);
            this.navigationCenterOnWholeRadioButton.Name = "navigationCenterOnWholeRadioButton";
            this.navigationCenterOnWholeRadioButton.Size = new System.Drawing.Size(75, 17);
            this.navigationCenterOnWholeRadioButton.TabIndex = 1;
            this.navigationCenterOnWholeRadioButton.TabStop = true;
            this.navigationCenterOnWholeRadioButton.Text = "whole item";
            this.navigationCenterOnWholeRadioButton.UseVisualStyleBackColor = true;
            this.navigationCenterOnWholeRadioButton.CheckedChanged += new System.EventHandler(this.navigationCenterOnWholeRadioButton_CheckedChanged);
            // 
            // navigationCenterOnNameRadioButton
            // 
            this.navigationCenterOnNameRadioButton.AutoSize = true;
            this.navigationCenterOnNameRadioButton.Location = new System.Drawing.Point(235, 21);
            this.navigationCenterOnNameRadioButton.Name = "navigationCenterOnNameRadioButton";
            this.navigationCenterOnNameRadioButton.Size = new System.Drawing.Size(73, 17);
            this.navigationCenterOnNameRadioButton.TabIndex = 2;
            this.navigationCenterOnNameRadioButton.TabStop = true;
            this.navigationCenterOnNameRadioButton.Text = "name only";
            this.navigationCenterOnNameRadioButton.UseVisualStyleBackColor = true;
            this.navigationCenterOnNameRadioButton.CheckedChanged += new System.EventHandler(this.navigationCenterOnNameRadioButton_CheckedChanged);
            // 
            // SpadeOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigationGroupBox);
            this.Controls.Add(this.displayGroupBox);
            this.Name = "SpadeOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.displayGroupBox.ResumeLayout(false);
            this.displayGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningThresholdUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alertThresholdUpDown)).EndInit();
            this.navigationGroupBox.ResumeLayout(false);
            this.navigationGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox displayGroupBox;
        private System.Windows.Forms.Label warningThresholdLabel;
        private System.Windows.Forms.NumericUpDown warningThresholdUpDown;
        private System.Windows.Forms.Label alertThresholdLabel;
        private System.Windows.Forms.NumericUpDown alertThresholdUpDown;
        private System.Windows.Forms.GroupBox navigationGroupBox;
        private System.Windows.Forms.Label navigationCenterLabel;
        private System.Windows.Forms.RadioButton navigationCenterOnWholeRadioButton;
        private System.Windows.Forms.RadioButton navigationCenterOnNameRadioButton;
        private System.Windows.Forms.CheckBox showItemComplexityCheckBox;
        private System.Windows.Forms.CheckBox showItemMetadataCheckBox;
        private System.Windows.Forms.Label showItemMetadataExampleLabel;
    }
}