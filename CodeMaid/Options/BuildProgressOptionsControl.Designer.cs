namespace SteveCadwallader.CodeMaid.Options
{
    partial class BuildProgressOptionsControl
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
            this.automaticGroupBox = new System.Windows.Forms.GroupBox();
            this.autoShowBuildProgressCheckBox = new System.Windows.Forms.CheckBox();
            this.autoHideBuildProgressCheckBox = new System.Windows.Forms.CheckBox();
            this.automaticGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // automaticGroupBox
            // 
            this.automaticGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.automaticGroupBox.Controls.Add(this.autoShowBuildProgressCheckBox);
            this.automaticGroupBox.Controls.Add(this.autoHideBuildProgressCheckBox);
            this.automaticGroupBox.Location = new System.Drawing.Point(3, 3);
            this.automaticGroupBox.Name = "automaticGroupBox";
            this.automaticGroupBox.Size = new System.Drawing.Size(344, 69);
            this.automaticGroupBox.TabIndex = 0;
            this.automaticGroupBox.TabStop = false;
            this.automaticGroupBox.Text = "Automatic";
            // 
            // autoShowBuildProgressCheckBox
            // 
            this.autoShowBuildProgressCheckBox.AutoSize = true;
            this.autoShowBuildProgressCheckBox.Location = new System.Drawing.Point(7, 20);
            this.autoShowBuildProgressCheckBox.Name = "autoShowBuildProgressCheckBox";
            this.autoShowBuildProgressCheckBox.Size = new System.Drawing.Size(266, 17);
            this.autoShowBuildProgressCheckBox.TabIndex = 0;
            this.autoShowBuildProgressCheckBox.Text = "Automatically show build progress when build starts";
            this.autoShowBuildProgressCheckBox.UseVisualStyleBackColor = true;
            this.autoShowBuildProgressCheckBox.CheckedChanged += new System.EventHandler(this.autoShowBuildProgressCheckBox_CheckedChanged);
            // 
            // autoHideBuildProgressCheckBox
            // 
            this.autoHideBuildProgressCheckBox.AutoSize = true;
            this.autoHideBuildProgressCheckBox.Location = new System.Drawing.Point(7, 44);
            this.autoHideBuildProgressCheckBox.Name = "autoHideBuildProgressCheckBox";
            this.autoHideBuildProgressCheckBox.Size = new System.Drawing.Size(261, 17);
            this.autoHideBuildProgressCheckBox.TabIndex = 1;
            this.autoHideBuildProgressCheckBox.Text = "Automatically hide build progress when build stops";
            this.autoHideBuildProgressCheckBox.UseVisualStyleBackColor = true;
            this.autoHideBuildProgressCheckBox.CheckedChanged += new System.EventHandler(this.autoHideBuildProgressCheckBox_CheckedChanged);
            // 
            // BuildProgressOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.automaticGroupBox);
            this.Name = "BuildProgressOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.automaticGroupBox.ResumeLayout(false);
            this.automaticGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox automaticGroupBox;
        private System.Windows.Forms.CheckBox autoShowBuildProgressCheckBox;
        private System.Windows.Forms.CheckBox autoHideBuildProgressCheckBox;
    }
}