namespace SteveCadwallader.CodeMaid.Options
{
    partial class SwitchFileOptionsControl
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
            this.relatedFileExtensionsGroupBox = new System.Windows.Forms.GroupBox();
            this.relatedFileExtensionsDescriptionLabel = new System.Windows.Forms.Label();
            this.exampleLabel = new System.Windows.Forms.Label();
            this.relatedFileExtensionsDefaultLabel = new System.Windows.Forms.Label();
            this.relatedFileExtensionsResetButton = new System.Windows.Forms.Button();
            this.relatedFileExtensionsTextBox = new System.Windows.Forms.TextBox();
            this.relatedFileExtensionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // relatedFileExtensionsGroupBox
            //
            this.relatedFileExtensionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsDescriptionLabel);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.exampleLabel);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsDefaultLabel);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsResetButton);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsTextBox);
            this.relatedFileExtensionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.relatedFileExtensionsGroupBox.Name = "relatedFileExtensionsGroupBox";
            this.relatedFileExtensionsGroupBox.Size = new System.Drawing.Size(344, 140);
            this.relatedFileExtensionsGroupBox.TabIndex = 0;
            this.relatedFileExtensionsGroupBox.TabStop = false;
            this.relatedFileExtensionsGroupBox.Text = "Related File Extensions";
            //
            // relatedFileExtensionsDescriptionLabel
            //
            this.relatedFileExtensionsDescriptionLabel.AutoSize = true;
            this.relatedFileExtensionsDescriptionLabel.Location = new System.Drawing.Point(20, 20);
            this.relatedFileExtensionsDescriptionLabel.Name = "relatedFileExtensionsDescriptionLabel";
            this.relatedFileExtensionsDescriptionLabel.Size = new System.Drawing.Size(307, 13);
            this.relatedFileExtensionsDescriptionLabel.TabIndex = 0;
            this.relatedFileExtensionsDescriptionLabel.Text = "Space between related extensions; Semicolon between groups.";
            //
            // exampleLabel
            //
            this.exampleLabel.AutoSize = true;
            this.exampleLabel.Location = new System.Drawing.Point(20, 41);
            this.exampleLabel.Name = "exampleLabel";
            this.exampleLabel.Size = new System.Drawing.Size(50, 13);
            this.exampleLabel.TabIndex = 1;
            this.exampleLabel.Text = "Example:";
            //
            // relatedFileExtensionsDefaultLabel
            //
            this.relatedFileExtensionsDefaultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsDefaultLabel.AutoEllipsis = true;
            this.relatedFileExtensionsDefaultLabel.Location = new System.Drawing.Point(76, 41);
            this.relatedFileExtensionsDefaultLabel.Name = "relatedFileExtensionsDefaultLabel";
            this.relatedFileExtensionsDefaultLabel.Size = new System.Drawing.Size(181, 13);
            this.relatedFileExtensionsDefaultLabel.TabIndex = 2;
            this.relatedFileExtensionsDefaultLabel.Text = ".cpp .h ; .xaml .xaml.cs ; .xml .xsd";
            //
            // relatedFileExtensionsResetButton
            //
            this.relatedFileExtensionsResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsResetButton.Location = new System.Drawing.Point(263, 36);
            this.relatedFileExtensionsResetButton.Name = "relatedFileExtensionsResetButton";
            this.relatedFileExtensionsResetButton.Size = new System.Drawing.Size(75, 23);
            this.relatedFileExtensionsResetButton.TabIndex = 4;
            this.relatedFileExtensionsResetButton.Text = "Reset";
            this.relatedFileExtensionsResetButton.UseVisualStyleBackColor = true;
            this.relatedFileExtensionsResetButton.Click += new System.EventHandler(this.relatedFileExtensionsResetButton_Click);
            //
            // relatedFileExtensionsTextBox
            //
            this.relatedFileExtensionsTextBox.AcceptsReturn = true;
            this.relatedFileExtensionsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsTextBox.Location = new System.Drawing.Point(23, 65);
            this.relatedFileExtensionsTextBox.Multiline = true;
            this.relatedFileExtensionsTextBox.Name = "relatedFileExtensionsTextBox";
            this.relatedFileExtensionsTextBox.Size = new System.Drawing.Size(315, 69);
            this.relatedFileExtensionsTextBox.TabIndex = 3;
            this.relatedFileExtensionsTextBox.TextChanged += new System.EventHandler(this.relatedFileExtensionsTextBox_TextChanged);
            //
            // SwitchFileOptionsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.relatedFileExtensionsGroupBox);
            this.Name = "SwitchFileOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.relatedFileExtensionsGroupBox.ResumeLayout(false);
            this.relatedFileExtensionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox relatedFileExtensionsGroupBox;
        private System.Windows.Forms.TextBox relatedFileExtensionsTextBox;
        private System.Windows.Forms.Label relatedFileExtensionsDescriptionLabel;
        private System.Windows.Forms.Button relatedFileExtensionsResetButton;
        private System.Windows.Forms.Label relatedFileExtensionsDefaultLabel;
        private System.Windows.Forms.Label exampleLabel;
    }
}