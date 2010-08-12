namespace SteveCadwallader.CodeMaid.Options
{
    partial class BuildStatusOptionsControl
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
            this.extendBuildStatusMessagesCheckBox = new System.Windows.Forms.CheckBox();
            this.extendBuildStatusMessagesExampleLabel = new System.Windows.Forms.Label();
            this.buildStatusGroupBox = new System.Windows.Forms.GroupBox();
            this.buildStatusGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // extendBuildStatusMessagesCheckBox
            //
            this.extendBuildStatusMessagesCheckBox.AutoSize = true;
            this.extendBuildStatusMessagesCheckBox.Location = new System.Drawing.Point(6, 19);
            this.extendBuildStatusMessagesCheckBox.Name = "extendBuildStatusMessagesCheckBox";
            this.extendBuildStatusMessagesCheckBox.Size = new System.Drawing.Size(318, 17);
            this.extendBuildStatusMessagesCheckBox.TabIndex = 0;
            this.extendBuildStatusMessagesCheckBox.Text = "Extend the build status messages that appear in the status bar";
            this.extendBuildStatusMessagesCheckBox.UseVisualStyleBackColor = true;
            this.extendBuildStatusMessagesCheckBox.CheckedChanged += new System.EventHandler(this.extendBuildStatusMessagesCheckBox_CheckedChanged);
            //
            // extendBuildStatusMessagesExampleLabel
            //
            this.extendBuildStatusMessagesExampleLabel.AutoSize = true;
            this.extendBuildStatusMessagesExampleLabel.Location = new System.Drawing.Point(46, 39);
            this.extendBuildStatusMessagesExampleLabel.Margin = new System.Windows.Forms.Padding(43, 0, 3, 0);
            this.extendBuildStatusMessagesExampleLabel.Name = "extendBuildStatusMessagesExampleLabel";
            this.extendBuildStatusMessagesExampleLabel.Size = new System.Drawing.Size(274, 13);
            this.extendBuildStatusMessagesExampleLabel.TabIndex = 1;
            this.extendBuildStatusMessagesExampleLabel.Text = "Example: Building 11 of 23 \'CodeMaid\' (Debug Any CPU)";
            //
            // buildStatusGroupBox
            //
            this.buildStatusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buildStatusGroupBox.Controls.Add(this.extendBuildStatusMessagesCheckBox);
            this.buildStatusGroupBox.Controls.Add(this.extendBuildStatusMessagesExampleLabel);
            this.buildStatusGroupBox.Location = new System.Drawing.Point(3, 3);
            this.buildStatusGroupBox.Name = "buildStatusGroupBox";
            this.buildStatusGroupBox.Size = new System.Drawing.Size(344, 65);
            this.buildStatusGroupBox.TabIndex = 2;
            this.buildStatusGroupBox.TabStop = false;
            this.buildStatusGroupBox.Text = "Build Status";
            //
            // BuildStatusOptionsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buildStatusGroupBox);
            this.Name = "BuildStatusOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.buildStatusGroupBox.ResumeLayout(false);
            this.buildStatusGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.CheckBox extendBuildStatusMessagesCheckBox;
        private System.Windows.Forms.Label extendBuildStatusMessagesExampleLabel;
        private System.Windows.Forms.GroupBox buildStatusGroupBox;
    }
}