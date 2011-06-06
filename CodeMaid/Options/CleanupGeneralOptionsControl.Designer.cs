namespace SteveCadwallader.CodeMaid.Options
{
    partial class CleanupGeneralOptionsControl
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
            this.wrapCleanupInASingleUndoTransactionCheckBox = new System.Windows.Forms.CheckBox();
            this.runVisualStudioFormatDocumentCommandCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveCheckBox = new System.Windows.Forms.CheckBox();
            this.automaticGroupBox = new System.Windows.Forms.GroupBox();
            this.autoCloseIfOpenedByCleanupCheckBox = new System.Windows.Forms.CheckBox();
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.automaticGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // wrapCleanupInASingleUndoTransactionCheckBox
            // 
            this.wrapCleanupInASingleUndoTransactionCheckBox.AutoSize = true;
            this.wrapCleanupInASingleUndoTransactionCheckBox.Location = new System.Drawing.Point(7, 44);
            this.wrapCleanupInASingleUndoTransactionCheckBox.Name = "wrapCleanupInASingleUndoTransactionCheckBox";
            this.wrapCleanupInASingleUndoTransactionCheckBox.Size = new System.Drawing.Size(225, 17);
            this.wrapCleanupInASingleUndoTransactionCheckBox.TabIndex = 1;
            this.wrapCleanupInASingleUndoTransactionCheckBox.Text = "Wrap cleanup in a single undo transaction";
            this.wrapCleanupInASingleUndoTransactionCheckBox.UseVisualStyleBackColor = true;
            this.wrapCleanupInASingleUndoTransactionCheckBox.CheckedChanged += new System.EventHandler(this.wrapCleanupInASingleUndoTransactionCheckBox_CheckedChanged);
            // 
            // runVisualStudioFormatDocumentCommandCheckBox
            // 
            this.runVisualStudioFormatDocumentCommandCheckBox.AutoSize = true;
            this.runVisualStudioFormatDocumentCommandCheckBox.Location = new System.Drawing.Point(7, 20);
            this.runVisualStudioFormatDocumentCommandCheckBox.Name = "runVisualStudioFormatDocumentCommandCheckBox";
            this.runVisualStudioFormatDocumentCommandCheckBox.Size = new System.Drawing.Size(245, 17);
            this.runVisualStudioFormatDocumentCommandCheckBox.TabIndex = 0;
            this.runVisualStudioFormatDocumentCommandCheckBox.Text = "Run visual studio\'s format document command";
            this.runVisualStudioFormatDocumentCommandCheckBox.UseVisualStyleBackColor = true;
            this.runVisualStudioFormatDocumentCommandCheckBox.CheckedChanged += new System.EventHandler(this.runVisualStudioFormatDocumentCommandCheckBox_CheckedChanged);
            // 
            // autoCleanupOnFileSaveCheckBox
            // 
            this.autoCleanupOnFileSaveCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveCheckBox.Location = new System.Drawing.Point(7, 20);
            this.autoCleanupOnFileSaveCheckBox.Name = "autoCleanupOnFileSaveCheckBox";
            this.autoCleanupOnFileSaveCheckBox.Size = new System.Drawing.Size(204, 17);
            this.autoCleanupOnFileSaveCheckBox.TabIndex = 0;
            this.autoCleanupOnFileSaveCheckBox.Text = "Automatically run cleanup on file save";
            this.autoCleanupOnFileSaveCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveCheckBox_CheckedChanged);
            // 
            // automaticGroupBox
            // 
            this.automaticGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.automaticGroupBox.Controls.Add(this.autoCleanupOnFileSaveCheckBox);
            this.automaticGroupBox.Controls.Add(this.autoCloseIfOpenedByCleanupCheckBox);
            this.automaticGroupBox.Location = new System.Drawing.Point(3, 3);
            this.automaticGroupBox.Name = "automaticGroupBox";
            this.automaticGroupBox.Size = new System.Drawing.Size(344, 69);
            this.automaticGroupBox.TabIndex = 0;
            this.automaticGroupBox.TabStop = false;
            this.automaticGroupBox.Text = "Automatic";
            // 
            // autoCloseIfOpenedByCleanupCheckBox
            // 
            this.autoCloseIfOpenedByCleanupCheckBox.AutoSize = true;
            this.autoCloseIfOpenedByCleanupCheckBox.Location = new System.Drawing.Point(7, 44);
            this.autoCloseIfOpenedByCleanupCheckBox.Name = "autoCloseIfOpenedByCleanupCheckBox";
            this.autoCloseIfOpenedByCleanupCheckBox.Size = new System.Drawing.Size(312, 17);
            this.autoCloseIfOpenedByCleanupCheckBox.TabIndex = 2;
            this.autoCloseIfOpenedByCleanupCheckBox.Text = "Automatically save and close documents opened by cleanup";
            this.autoCloseIfOpenedByCleanupCheckBox.UseVisualStyleBackColor = true;
            this.autoCloseIfOpenedByCleanupCheckBox.CheckedChanged += new System.EventHandler(this.autoCloseIfOpenedByCleanupCheckBox_CheckedChanged);
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.generalGroupBox.Controls.Add(this.runVisualStudioFormatDocumentCommandCheckBox);
            this.generalGroupBox.Controls.Add(this.wrapCleanupInASingleUndoTransactionCheckBox);
            this.generalGroupBox.Location = new System.Drawing.Point(3, 78);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(344, 69);
            this.generalGroupBox.TabIndex = 1;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // CleanupGeneralOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.automaticGroupBox);
            this.Controls.Add(this.generalGroupBox);
            this.Name = "CleanupGeneralOptionsControl";
            this.Size = new System.Drawing.Size(350, 250);
            this.automaticGroupBox.ResumeLayout(false);
            this.automaticGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        private System.Windows.Forms.CheckBox wrapCleanupInASingleUndoTransactionCheckBox;
        private System.Windows.Forms.CheckBox runVisualStudioFormatDocumentCommandCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveCheckBox;
        private System.Windows.Forms.GroupBox automaticGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.CheckBox autoCloseIfOpenedByCleanupCheckBox;
    }
}