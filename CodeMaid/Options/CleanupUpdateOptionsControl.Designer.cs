namespace SteveCadwallader.CodeMaid.Options
{
    partial class CleanupUpdateOptionsControl
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
            this.updateGroupBox = new System.Windows.Forms.GroupBox();
            this.sortUsingStatementsCheckBox = new System.Windows.Forms.CheckBox();
            this.updateEndRegionDirectivesCheckBox = new System.Windows.Forms.CheckBox();
            this.updateEndRegionDirectivesExampleLabel = new System.Windows.Forms.Label();
            this.updateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateGroupBox
            // 
            this.updateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.updateGroupBox.Controls.Add(this.sortUsingStatementsCheckBox);
            this.updateGroupBox.Controls.Add(this.updateEndRegionDirectivesCheckBox);
            this.updateGroupBox.Controls.Add(this.updateEndRegionDirectivesExampleLabel);
            this.updateGroupBox.Location = new System.Drawing.Point(3, 3);
            this.updateGroupBox.Name = "updateGroupBox";
            this.updateGroupBox.Size = new System.Drawing.Size(344, 85);
            this.updateGroupBox.TabIndex = 0;
            this.updateGroupBox.TabStop = false;
            this.updateGroupBox.Text = "Update";
            // 
            // sortUsingStatementsCheckBox
            // 
            this.sortUsingStatementsCheckBox.AutoSize = true;
            this.sortUsingStatementsCheckBox.Location = new System.Drawing.Point(7, 20);
            this.sortUsingStatementsCheckBox.Name = "sortUsingStatementsCheckBox";
            this.sortUsingStatementsCheckBox.Size = new System.Drawing.Size(127, 17);
            this.sortUsingStatementsCheckBox.TabIndex = 0;
            this.sortUsingStatementsCheckBox.Text = "Sort using statements";
            this.sortUsingStatementsCheckBox.UseVisualStyleBackColor = true;
            this.sortUsingStatementsCheckBox.CheckedChanged += new System.EventHandler(this.sortUsingStatementsCheckBox_CheckedChanged);
            // 
            // updateEndRegionDirectivesCheckBox
            // 
            this.updateEndRegionDirectivesCheckBox.AutoSize = true;
            this.updateEndRegionDirectivesCheckBox.Location = new System.Drawing.Point(7, 44);
            this.updateEndRegionDirectivesCheckBox.Name = "updateEndRegionDirectivesCheckBox";
            this.updateEndRegionDirectivesCheckBox.Size = new System.Drawing.Size(242, 17);
            this.updateEndRegionDirectivesCheckBox.TabIndex = 1;
            this.updateEndRegionDirectivesCheckBox.Text = "Update endregion directives with region name";
            this.updateEndRegionDirectivesCheckBox.UseVisualStyleBackColor = true;
            this.updateEndRegionDirectivesCheckBox.CheckedChanged += new System.EventHandler(this.updateEndRegionDirectivesCheckBox_CheckedChanged);
            // 
            // updateEndRegionDirectivesExampleLabel
            // 
            this.updateEndRegionDirectivesExampleLabel.AutoSize = true;
            this.updateEndRegionDirectivesExampleLabel.Location = new System.Drawing.Point(39, 64);
            this.updateEndRegionDirectivesExampleLabel.Margin = new System.Windows.Forms.Padding(43, 0, 3, 0);
            this.updateEndRegionDirectivesExampleLabel.Name = "updateEndRegionDirectivesExampleLabel";
            this.updateEndRegionDirectivesExampleLabel.Size = new System.Drawing.Size(151, 13);
            this.updateEndRegionDirectivesExampleLabel.TabIndex = 2;
            this.updateEndRegionDirectivesExampleLabel.Text = "Example: #endregion Methods";
            // 
            // CleanupUpdateOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updateGroupBox);
            this.Name = "CleanupUpdateOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.updateGroupBox.ResumeLayout(false);
            this.updateGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox updateGroupBox;
        private System.Windows.Forms.CheckBox updateEndRegionDirectivesCheckBox;
        private System.Windows.Forms.CheckBox sortUsingStatementsCheckBox;
        private System.Windows.Forms.Label updateEndRegionDirectivesExampleLabel;
    }
}