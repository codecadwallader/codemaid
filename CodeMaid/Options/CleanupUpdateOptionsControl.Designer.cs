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
            this.label1 = new System.Windows.Forms.Label();
            this.sortUsingStatementsCheckBox = new System.Windows.Forms.CheckBox();
            this.updateRegionDirectivesCheckBox = new System.Windows.Forms.CheckBox();
            this.updateGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // updateGroupBox
            //
            this.updateGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.updateGroupBox.Controls.Add(this.label1);
            this.updateGroupBox.Controls.Add(this.sortUsingStatementsCheckBox);
            this.updateGroupBox.Controls.Add(this.updateRegionDirectivesCheckBox);
            this.updateGroupBox.Location = new System.Drawing.Point(3, 3);
            this.updateGroupBox.Name = "updateGroupBox";
            this.updateGroupBox.Size = new System.Drawing.Size(344, 85);
            this.updateGroupBox.TabIndex = 0;
            this.updateGroupBox.TabStop = false;
            this.updateGroupBox.Text = "Update";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(43, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Example: #endregion Methods";
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
            // updateRegionDirectivesCheckBox
            //
            this.updateRegionDirectivesCheckBox.AutoSize = true;
            this.updateRegionDirectivesCheckBox.Location = new System.Drawing.Point(7, 44);
            this.updateRegionDirectivesCheckBox.Name = "updateRegionDirectivesCheckBox";
            this.updateRegionDirectivesCheckBox.Size = new System.Drawing.Size(141, 17);
            this.updateRegionDirectivesCheckBox.TabIndex = 1;
            this.updateRegionDirectivesCheckBox.Text = "Update region directives";
            this.updateRegionDirectivesCheckBox.UseVisualStyleBackColor = true;
            this.updateRegionDirectivesCheckBox.CheckedChanged += new System.EventHandler(this.updateRegionDirectivesCheckBox_CheckedChanged);
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
        private System.Windows.Forms.CheckBox updateRegionDirectivesCheckBox;
        private System.Windows.Forms.CheckBox sortUsingStatementsCheckBox;
        private System.Windows.Forms.Label label1;
    }
}