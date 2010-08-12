namespace SteveCadwallader.CodeMaid.Options
{
    partial class CleanupRemoveOptionsControl
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
            this.blankLinesGroupBox = new System.Windows.Forms.GroupBox();
            this.removeBlankLinesAtTopCheckBox = new System.Windows.Forms.CheckBox();
            this.removeBlankLinesAtBottomCheckBox = new System.Windows.Forms.CheckBox();
            this.removeBlankLinesAfterOpeningBraceCheckBox = new System.Windows.Forms.CheckBox();
            this.removeBlankLinesBeforeClosingBraceCheckBox = new System.Windows.Forms.CheckBox();
            this.removeMultipleConsecutiveBlankLinesCheckBox = new System.Windows.Forms.CheckBox();
            this.unusedGroupBox = new System.Windows.Forms.GroupBox();
            this.removeUnusedUsingStatementsCheckBox = new System.Windows.Forms.CheckBox();
            this.whitespaceGroupBox = new System.Windows.Forms.GroupBox();
            this.removeEndOfLineWhitespaceCheckBox = new System.Windows.Forms.CheckBox();
            this.blankLinesGroupBox.SuspendLayout();
            this.unusedGroupBox.SuspendLayout();
            this.whitespaceGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // blankLinesGroupBox
            //
            this.blankLinesGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.blankLinesGroupBox.Controls.Add(this.removeBlankLinesAtTopCheckBox);
            this.blankLinesGroupBox.Controls.Add(this.removeBlankLinesAtBottomCheckBox);
            this.blankLinesGroupBox.Controls.Add(this.removeBlankLinesAfterOpeningBraceCheckBox);
            this.blankLinesGroupBox.Controls.Add(this.removeBlankLinesBeforeClosingBraceCheckBox);
            this.blankLinesGroupBox.Controls.Add(this.removeMultipleConsecutiveBlankLinesCheckBox);
            this.blankLinesGroupBox.Location = new System.Drawing.Point(3, 3);
            this.blankLinesGroupBox.Name = "blankLinesGroupBox";
            this.blankLinesGroupBox.Size = new System.Drawing.Size(344, 141);
            this.blankLinesGroupBox.TabIndex = 0;
            this.blankLinesGroupBox.TabStop = false;
            this.blankLinesGroupBox.Text = "Blank lines";
            //
            // removeBlankLinesAtTopCheckBox
            //
            this.removeBlankLinesAtTopCheckBox.AutoSize = true;
            this.removeBlankLinesAtTopCheckBox.Location = new System.Drawing.Point(7, 20);
            this.removeBlankLinesAtTopCheckBox.Name = "removeBlankLinesAtTopCheckBox";
            this.removeBlankLinesAtTopCheckBox.Size = new System.Drawing.Size(177, 17);
            this.removeBlankLinesAtTopCheckBox.TabIndex = 0;
            this.removeBlankLinesAtTopCheckBox.Text = "Remove blank lines at top of file";
            this.removeBlankLinesAtTopCheckBox.UseVisualStyleBackColor = true;
            this.removeBlankLinesAtTopCheckBox.CheckedChanged += new System.EventHandler(this.removeBlankLinesAtTopCheckBox_CheckedChanged);
            //
            // removeBlankLinesAtBottomCheckBox
            //
            this.removeBlankLinesAtBottomCheckBox.AutoSize = true;
            this.removeBlankLinesAtBottomCheckBox.Location = new System.Drawing.Point(7, 44);
            this.removeBlankLinesAtBottomCheckBox.Name = "removeBlankLinesAtBottomCheckBox";
            this.removeBlankLinesAtBottomCheckBox.Size = new System.Drawing.Size(194, 17);
            this.removeBlankLinesAtBottomCheckBox.TabIndex = 1;
            this.removeBlankLinesAtBottomCheckBox.Text = "Remove blank lines at bottom of file";
            this.removeBlankLinesAtBottomCheckBox.UseVisualStyleBackColor = true;
            this.removeBlankLinesAtBottomCheckBox.CheckedChanged += new System.EventHandler(this.removeBlankLinesAtBottomCheckBox_CheckedChanged);
            //
            // removeBlankLinesAfterOpeningBraceCheckBox
            //
            this.removeBlankLinesAfterOpeningBraceCheckBox.AutoSize = true;
            this.removeBlankLinesAfterOpeningBraceCheckBox.Location = new System.Drawing.Point(7, 68);
            this.removeBlankLinesAfterOpeningBraceCheckBox.Name = "removeBlankLinesAfterOpeningBraceCheckBox";
            this.removeBlankLinesAfterOpeningBraceCheckBox.Size = new System.Drawing.Size(214, 17);
            this.removeBlankLinesAfterOpeningBraceCheckBox.TabIndex = 2;
            this.removeBlankLinesAfterOpeningBraceCheckBox.Text = "Remove blank lines after opening brace";
            this.removeBlankLinesAfterOpeningBraceCheckBox.UseVisualStyleBackColor = true;
            this.removeBlankLinesAfterOpeningBraceCheckBox.CheckedChanged += new System.EventHandler(this.removeBlankLinesAfterOpeningBraceCheckBox_CheckedChanged);
            //
            // removeBlankLinesBeforeClosingBraceCheckBox
            //
            this.removeBlankLinesBeforeClosingBraceCheckBox.AutoSize = true;
            this.removeBlankLinesBeforeClosingBraceCheckBox.Location = new System.Drawing.Point(7, 92);
            this.removeBlankLinesBeforeClosingBraceCheckBox.Name = "removeBlankLinesBeforeClosingBraceCheckBox";
            this.removeBlankLinesBeforeClosingBraceCheckBox.Size = new System.Drawing.Size(218, 17);
            this.removeBlankLinesBeforeClosingBraceCheckBox.TabIndex = 3;
            this.removeBlankLinesBeforeClosingBraceCheckBox.Text = "Remove blank lines before closing brace";
            this.removeBlankLinesBeforeClosingBraceCheckBox.UseVisualStyleBackColor = true;
            this.removeBlankLinesBeforeClosingBraceCheckBox.CheckedChanged += new System.EventHandler(this.removeBlankLinesBeforeClosingBraceCheckBox_CheckedChanged);
            //
            // removeMultipleConsecutiveBlankLinesCheckBox
            //
            this.removeMultipleConsecutiveBlankLinesCheckBox.AutoSize = true;
            this.removeMultipleConsecutiveBlankLinesCheckBox.Location = new System.Drawing.Point(7, 116);
            this.removeMultipleConsecutiveBlankLinesCheckBox.Name = "removeMultipleConsecutiveBlankLinesCheckBox";
            this.removeMultipleConsecutiveBlankLinesCheckBox.Size = new System.Drawing.Size(218, 17);
            this.removeMultipleConsecutiveBlankLinesCheckBox.TabIndex = 4;
            this.removeMultipleConsecutiveBlankLinesCheckBox.Text = "Remove multiple consecutive blank lines";
            this.removeMultipleConsecutiveBlankLinesCheckBox.UseVisualStyleBackColor = true;
            this.removeMultipleConsecutiveBlankLinesCheckBox.CheckedChanged += new System.EventHandler(this.removeMultipleConsecutiveBlankLinesCheckBox_CheckedChanged);
            //
            // unusedGroupBox
            //
            this.unusedGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.unusedGroupBox.Controls.Add(this.removeUnusedUsingStatementsCheckBox);
            this.unusedGroupBox.Location = new System.Drawing.Point(3, 150);
            this.unusedGroupBox.Name = "unusedGroupBox";
            this.unusedGroupBox.Size = new System.Drawing.Size(344, 47);
            this.unusedGroupBox.TabIndex = 1;
            this.unusedGroupBox.TabStop = false;
            this.unusedGroupBox.Text = "Unused";
            //
            // removeUnusedUsingStatementsCheckBox
            //
            this.removeUnusedUsingStatementsCheckBox.AutoSize = true;
            this.removeUnusedUsingStatementsCheckBox.Location = new System.Drawing.Point(7, 20);
            this.removeUnusedUsingStatementsCheckBox.Name = "removeUnusedUsingStatementsCheckBox";
            this.removeUnusedUsingStatementsCheckBox.Size = new System.Drawing.Size(186, 17);
            this.removeUnusedUsingStatementsCheckBox.TabIndex = 0;
            this.removeUnusedUsingStatementsCheckBox.Text = "Remove unused using statements";
            this.removeUnusedUsingStatementsCheckBox.UseVisualStyleBackColor = true;
            this.removeUnusedUsingStatementsCheckBox.CheckedChanged += new System.EventHandler(this.removeUnusedUsingStatementsCheckBox_CheckedChanged);
            //
            // whitespaceGroupBox
            //
            this.whitespaceGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.whitespaceGroupBox.Controls.Add(this.removeEndOfLineWhitespaceCheckBox);
            this.whitespaceGroupBox.Location = new System.Drawing.Point(4, 204);
            this.whitespaceGroupBox.Name = "whitespaceGroupBox";
            this.whitespaceGroupBox.Size = new System.Drawing.Size(343, 45);
            this.whitespaceGroupBox.TabIndex = 2;
            this.whitespaceGroupBox.TabStop = false;
            this.whitespaceGroupBox.Text = "Whitespace";
            //
            // removeEndOfLineWhitespaceCheckBox
            //
            this.removeEndOfLineWhitespaceCheckBox.AutoSize = true;
            this.removeEndOfLineWhitespaceCheckBox.Location = new System.Drawing.Point(6, 20);
            this.removeEndOfLineWhitespaceCheckBox.Name = "removeEndOfLineWhitespaceCheckBox";
            this.removeEndOfLineWhitespaceCheckBox.Size = new System.Drawing.Size(175, 17);
            this.removeEndOfLineWhitespaceCheckBox.TabIndex = 0;
            this.removeEndOfLineWhitespaceCheckBox.Text = "Remove end of line whitespace";
            this.removeEndOfLineWhitespaceCheckBox.UseVisualStyleBackColor = true;
            this.removeEndOfLineWhitespaceCheckBox.CheckedChanged += new System.EventHandler(this.removeEndOfLineWhitespaceCheckBox_CheckedChanged);
            //
            // CleanupRemoveOptionsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.blankLinesGroupBox);
            this.Controls.Add(this.unusedGroupBox);
            this.Controls.Add(this.whitespaceGroupBox);
            this.Name = "CleanupRemoveOptionsControl";
            this.Size = new System.Drawing.Size(350, 250);
            this.blankLinesGroupBox.ResumeLayout(false);
            this.blankLinesGroupBox.PerformLayout();
            this.unusedGroupBox.ResumeLayout(false);
            this.unusedGroupBox.PerformLayout();
            this.whitespaceGroupBox.ResumeLayout(false);
            this.whitespaceGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox blankLinesGroupBox;
        private System.Windows.Forms.CheckBox removeBlankLinesAtTopCheckBox;
        private System.Windows.Forms.CheckBox removeBlankLinesAtBottomCheckBox;
        private System.Windows.Forms.CheckBox removeBlankLinesAfterOpeningBraceCheckBox;
        private System.Windows.Forms.CheckBox removeBlankLinesBeforeClosingBraceCheckBox;
        private System.Windows.Forms.CheckBox removeMultipleConsecutiveBlankLinesCheckBox;
        private System.Windows.Forms.GroupBox unusedGroupBox;
        private System.Windows.Forms.CheckBox removeUnusedUsingStatementsCheckBox;
        private System.Windows.Forms.GroupBox whitespaceGroupBox;
        private System.Windows.Forms.CheckBox removeEndOfLineWhitespaceCheckBox;
    }
}