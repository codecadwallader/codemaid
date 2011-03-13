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
            this.relatedFileExtensionsExpressionDefaultLabel = new System.Windows.Forms.Label();
            this.relatedFileExtensionsExpressionResetButton = new System.Windows.Forms.Button();
            this.relatedFileExtensionsExpressionTextBox = new System.Windows.Forms.TextBox();
            this.relatedFileExtensionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // relatedFileExtensionsGroupBox
            //
            this.relatedFileExtensionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsGroupBox.Controls.Add(this.exampleLabel);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsExpressionDefaultLabel);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsExpressionResetButton);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsExpressionTextBox);
            this.relatedFileExtensionsGroupBox.Controls.Add(this.relatedFileExtensionsDescriptionLabel);
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
            this.relatedFileExtensionsDescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.relatedFileExtensionsDescriptionLabel.Location = new System.Drawing.Point(20, 116);
            this.relatedFileExtensionsDescriptionLabel.Name = "relatedFileExtensionsDescriptionLabel";
            this.relatedFileExtensionsDescriptionLabel.Size = new System.Drawing.Size(320, 13);
            this.relatedFileExtensionsDescriptionLabel.TabIndex = 3;
            this.relatedFileExtensionsDescriptionLabel.Text = "Space separates related extensions.  Semicolon separates groups.";
            //
            // exampleLabel
            //
            this.exampleLabel.AutoSize = true;
            this.exampleLabel.Location = new System.Drawing.Point(20, 20);
            this.exampleLabel.Name = "exampleLabel";
            this.exampleLabel.Size = new System.Drawing.Size(50, 13);
            this.exampleLabel.TabIndex = 0;
            this.exampleLabel.Text = "Example:";
            //
            // relatedFileExtensionsExpressionDefaultLabel
            //
            this.relatedFileExtensionsExpressionDefaultLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsExpressionDefaultLabel.AutoEllipsis = true;
            this.relatedFileExtensionsExpressionDefaultLabel.Location = new System.Drawing.Point(76, 20);
            this.relatedFileExtensionsExpressionDefaultLabel.Name = "relatedFileExtensionsExpressionDefaultLabel";
            this.relatedFileExtensionsExpressionDefaultLabel.Size = new System.Drawing.Size(181, 13);
            this.relatedFileExtensionsExpressionDefaultLabel.TabIndex = 1;
            this.relatedFileExtensionsExpressionDefaultLabel.Text = ".cpp .h ; .xaml .xaml.cs ; .xml .xsd";
            //
            // relatedFileExtensionsExpressionResetButton
            //
            this.relatedFileExtensionsExpressionResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsExpressionResetButton.Location = new System.Drawing.Point(263, 15);
            this.relatedFileExtensionsExpressionResetButton.Name = "relatedFileExtensionsExpressionResetButton";
            this.relatedFileExtensionsExpressionResetButton.Size = new System.Drawing.Size(75, 23);
            this.relatedFileExtensionsExpressionResetButton.TabIndex = 4;
            this.relatedFileExtensionsExpressionResetButton.Text = "Reset";
            this.relatedFileExtensionsExpressionResetButton.UseVisualStyleBackColor = true;
            this.relatedFileExtensionsExpressionResetButton.Click += new System.EventHandler(this.relatedFileExtensionsExpressionResetButton_Click);
            //
            // relatedFileExtensionsExpressionTextBox
            //
            this.relatedFileExtensionsExpressionTextBox.AcceptsReturn = true;
            this.relatedFileExtensionsExpressionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.relatedFileExtensionsExpressionTextBox.Location = new System.Drawing.Point(23, 44);
            this.relatedFileExtensionsExpressionTextBox.Multiline = true;
            this.relatedFileExtensionsExpressionTextBox.Name = "relatedFileExtensionsExpressionTextBox";
            this.relatedFileExtensionsExpressionTextBox.Size = new System.Drawing.Size(315, 69);
            this.relatedFileExtensionsExpressionTextBox.TabIndex = 2;
            this.relatedFileExtensionsExpressionTextBox.TextChanged += new System.EventHandler(this.relatedFileExtensionsExpressionTextBox_TextChanged);
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
        private System.Windows.Forms.TextBox relatedFileExtensionsExpressionTextBox;
        private System.Windows.Forms.Label relatedFileExtensionsDescriptionLabel;
        private System.Windows.Forms.Button relatedFileExtensionsExpressionResetButton;
        private System.Windows.Forms.Label relatedFileExtensionsExpressionDefaultLabel;
        private System.Windows.Forms.Label exampleLabel;
    }
}