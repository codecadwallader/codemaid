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
            this.autoCleanupFilesTypesPanel = new System.Windows.Forms.TableLayoutPanel();
            this.autoCleanupOnFileSaveCSharpCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveHTMLCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveXAMLCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveXMLCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveCPlusPlusCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveCSSCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCleanupOnFileSaveJavaScriptCheckBox = new System.Windows.Forms.CheckBox();
            this.automaticGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.autoCleanupFilesTypesPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // wrapCleanupInASingleUndoTransactionCheckBox
            //
            this.wrapCleanupInASingleUndoTransactionCheckBox.AutoSize = true;
            this.wrapCleanupInASingleUndoTransactionCheckBox.Location = new System.Drawing.Point(6, 42);
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
            this.runVisualStudioFormatDocumentCommandCheckBox.Location = new System.Drawing.Point(6, 19);
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
            this.autoCleanupOnFileSaveCheckBox.Location = new System.Drawing.Point(6, 19);
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
            this.automaticGroupBox.Controls.Add(this.autoCleanupFilesTypesPanel);
            this.automaticGroupBox.Controls.Add(this.autoCloseIfOpenedByCleanupCheckBox);
            this.automaticGroupBox.Location = new System.Drawing.Point(3, 3);
            this.automaticGroupBox.Name = "automaticGroupBox";
            this.automaticGroupBox.Size = new System.Drawing.Size(344, 141);
            this.automaticGroupBox.TabIndex = 0;
            this.automaticGroupBox.TabStop = false;
            this.automaticGroupBox.Text = "Automatic";
            //
            // autoCloseIfOpenedByCleanupCheckBox
            //
            this.autoCloseIfOpenedByCleanupCheckBox.AutoSize = true;
            this.autoCloseIfOpenedByCleanupCheckBox.Location = new System.Drawing.Point(6, 117);
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
            this.generalGroupBox.Location = new System.Drawing.Point(3, 150);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(344, 69);
            this.generalGroupBox.TabIndex = 1;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            //
            // autoCleanupFilesTypesPanel
            //
            this.autoCleanupFilesTypesPanel.AutoSize = true;
            this.autoCleanupFilesTypesPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.autoCleanupFilesTypesPanel.ColumnCount = 5;
            this.autoCleanupFilesTypesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.autoCleanupFilesTypesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.autoCleanupFilesTypesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.autoCleanupFilesTypesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.autoCleanupFilesTypesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveCSharpCheckBox, 0, 0);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveCPlusPlusCheckBox, 2, 0);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveXAMLCheckBox, 0, 1);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveCSSCheckBox, 2, 2);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveXMLCheckBox, 2, 1);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveHTMLCheckBox, 0, 2);
            this.autoCleanupFilesTypesPanel.Controls.Add(this.autoCleanupOnFileSaveJavaScriptCheckBox, 4, 2);
            this.autoCleanupFilesTypesPanel.Location = new System.Drawing.Point(46, 42);
            this.autoCleanupFilesTypesPanel.Name = "autoCleanupFilesTypesPanel";
            this.autoCleanupFilesTypesPanel.RowCount = 3;
            this.autoCleanupFilesTypesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.autoCleanupFilesTypesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.autoCleanupFilesTypesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.autoCleanupFilesTypesPanel.Size = new System.Drawing.Size(218, 69);
            this.autoCleanupFilesTypesPanel.TabIndex = 1;
            //
            // autoCleanupOnFileSaveCSharpCheckBox
            //
            this.autoCleanupOnFileSaveCSharpCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveCSharpCheckBox.Location = new System.Drawing.Point(3, 3);
            this.autoCleanupOnFileSaveCSharpCheckBox.Name = "autoCleanupOnFileSaveCSharpCheckBox";
            this.autoCleanupOnFileSaveCSharpCheckBox.Size = new System.Drawing.Size(40, 17);
            this.autoCleanupOnFileSaveCSharpCheckBox.TabIndex = 0;
            this.autoCleanupOnFileSaveCSharpCheckBox.Text = "C#";
            this.autoCleanupOnFileSaveCSharpCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveCSharpCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveCSharpCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveHTMLCheckBox
            //
            this.autoCleanupOnFileSaveHTMLCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveHTMLCheckBox.Location = new System.Drawing.Point(3, 49);
            this.autoCleanupOnFileSaveHTMLCheckBox.Name = "autoCleanupOnFileSaveHTMLCheckBox";
            this.autoCleanupOnFileSaveHTMLCheckBox.Size = new System.Drawing.Size(56, 17);
            this.autoCleanupOnFileSaveHTMLCheckBox.TabIndex = 4;
            this.autoCleanupOnFileSaveHTMLCheckBox.Text = "HTML";
            this.autoCleanupOnFileSaveHTMLCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveHTMLCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveHTMLCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveXAMLCheckBox
            //
            this.autoCleanupOnFileSaveXAMLCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveXAMLCheckBox.Location = new System.Drawing.Point(3, 26);
            this.autoCleanupOnFileSaveXAMLCheckBox.Name = "autoCleanupOnFileSaveXAMLCheckBox";
            this.autoCleanupOnFileSaveXAMLCheckBox.Size = new System.Drawing.Size(55, 17);
            this.autoCleanupOnFileSaveXAMLCheckBox.TabIndex = 2;
            this.autoCleanupOnFileSaveXAMLCheckBox.Text = "XAML";
            this.autoCleanupOnFileSaveXAMLCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveXAMLCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveXAMLCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveXMLCheckBox
            //
            this.autoCleanupOnFileSaveXMLCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveXMLCheckBox.Location = new System.Drawing.Point(75, 26);
            this.autoCleanupOnFileSaveXMLCheckBox.Name = "autoCleanupOnFileSaveXMLCheckBox";
            this.autoCleanupOnFileSaveXMLCheckBox.Size = new System.Drawing.Size(48, 17);
            this.autoCleanupOnFileSaveXMLCheckBox.TabIndex = 3;
            this.autoCleanupOnFileSaveXMLCheckBox.Text = "XML";
            this.autoCleanupOnFileSaveXMLCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveXMLCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveXMLCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveCPlusPlusCheckBox
            //
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.Location = new System.Drawing.Point(75, 3);
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.Name = "autoCleanupOnFileSaveCPlusPlusCheckBox";
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.Size = new System.Drawing.Size(45, 17);
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.TabIndex = 1;
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.Text = "C++";
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveCPlusPlusCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveCPlusPlusCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveCSSCheckBox
            //
            this.autoCleanupOnFileSaveCSSCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveCSSCheckBox.Location = new System.Drawing.Point(75, 49);
            this.autoCleanupOnFileSaveCSSCheckBox.Name = "autoCleanupOnFileSaveCSSCheckBox";
            this.autoCleanupOnFileSaveCSSCheckBox.Size = new System.Drawing.Size(47, 17);
            this.autoCleanupOnFileSaveCSSCheckBox.TabIndex = 5;
            this.autoCleanupOnFileSaveCSSCheckBox.Text = "CSS";
            this.autoCleanupOnFileSaveCSSCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveCSSCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveCSSCheckBox_CheckedChanged);
            //
            // autoCleanupOnFileSaveJavaScriptCheckBox
            //
            this.autoCleanupOnFileSaveJavaScriptCheckBox.AutoSize = true;
            this.autoCleanupOnFileSaveJavaScriptCheckBox.Location = new System.Drawing.Point(139, 49);
            this.autoCleanupOnFileSaveJavaScriptCheckBox.Name = "autoCleanupOnFileSaveJavaScriptCheckBox";
            this.autoCleanupOnFileSaveJavaScriptCheckBox.Size = new System.Drawing.Size(76, 17);
            this.autoCleanupOnFileSaveJavaScriptCheckBox.TabIndex = 6;
            this.autoCleanupOnFileSaveJavaScriptCheckBox.Text = "JavaScript";
            this.autoCleanupOnFileSaveJavaScriptCheckBox.UseVisualStyleBackColor = true;
            this.autoCleanupOnFileSaveJavaScriptCheckBox.CheckedChanged += new System.EventHandler(this.autoCleanupOnFileSaveJavaScriptCheckBox_CheckedChanged);
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
            this.autoCleanupFilesTypesPanel.ResumeLayout(false);
            this.autoCleanupFilesTypesPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.CheckBox wrapCleanupInASingleUndoTransactionCheckBox;
        private System.Windows.Forms.CheckBox runVisualStudioFormatDocumentCommandCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveCheckBox;
        private System.Windows.Forms.GroupBox automaticGroupBox;
        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.CheckBox autoCloseIfOpenedByCleanupCheckBox;
        private System.Windows.Forms.TableLayoutPanel autoCleanupFilesTypesPanel;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveCSharpCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveCPlusPlusCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveXAMLCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveCSSCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveXMLCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveHTMLCheckBox;
        private System.Windows.Forms.CheckBox autoCleanupOnFileSaveJavaScriptCheckBox;
    }
}