namespace SteveCadwallader.CodeMaid.Options
{
    partial class CleanupFileTypesOptionsControl
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
            this.includeGroupBox = new System.Windows.Forms.GroupBox();
            this.includeTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.includeCSharpCheckBox = new System.Windows.Forms.CheckBox();
            this.includeCPlusPlusCheckBox = new System.Windows.Forms.CheckBox();
            this.includeXAMLCheckBox = new System.Windows.Forms.CheckBox();
            this.includeXMLCheckBox = new System.Windows.Forms.CheckBox();
            this.includeHTMLCheckBox = new System.Windows.Forms.CheckBox();
            this.includeCSSCheckBox = new System.Windows.Forms.CheckBox();
            this.includeJavaScriptCheckBox = new System.Windows.Forms.CheckBox();
            this.includeGroupBox.SuspendLayout();
            this.includeTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // includeGroupBox
            // 
            this.includeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.includeGroupBox.Controls.Add(this.includeTableLayoutPanel);
            this.includeGroupBox.Location = new System.Drawing.Point(4, 4);
            this.includeGroupBox.Name = "includeGroupBox";
            this.includeGroupBox.Size = new System.Drawing.Size(343, 95);
            this.includeGroupBox.TabIndex = 0;
            this.includeGroupBox.TabStop = false;
            this.includeGroupBox.Text = "Include";
            // 
            // includeTableLayoutPanel
            // 
            this.includeTableLayoutPanel.AutoSize = true;
            this.includeTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.includeTableLayoutPanel.ColumnCount = 5;
            this.includeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.includeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.includeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.includeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.includeTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.includeTableLayoutPanel.Controls.Add(this.includeCSharpCheckBox, 0, 0);
            this.includeTableLayoutPanel.Controls.Add(this.includeCPlusPlusCheckBox, 2, 0);
            this.includeTableLayoutPanel.Controls.Add(this.includeXAMLCheckBox, 0, 1);
            this.includeTableLayoutPanel.Controls.Add(this.includeXMLCheckBox, 2, 1);
            this.includeTableLayoutPanel.Controls.Add(this.includeHTMLCheckBox, 0, 2);
            this.includeTableLayoutPanel.Controls.Add(this.includeCSSCheckBox, 2, 2);
            this.includeTableLayoutPanel.Controls.Add(this.includeJavaScriptCheckBox, 4, 2);
            this.includeTableLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.includeTableLayoutPanel.Name = "includeTableLayoutPanel";
            this.includeTableLayoutPanel.RowCount = 3;
            this.includeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.includeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.includeTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.includeTableLayoutPanel.Size = new System.Drawing.Size(218, 69);
            this.includeTableLayoutPanel.TabIndex = 0;
            // 
            // includeCSharpCheckBox
            // 
            this.includeCSharpCheckBox.AutoSize = true;
            this.includeCSharpCheckBox.Location = new System.Drawing.Point(3, 3);
            this.includeCSharpCheckBox.Name = "includeCSharpCheckBox";
            this.includeCSharpCheckBox.Size = new System.Drawing.Size(40, 17);
            this.includeCSharpCheckBox.TabIndex = 0;
            this.includeCSharpCheckBox.Text = "C#";
            this.includeCSharpCheckBox.UseVisualStyleBackColor = true;
            this.includeCSharpCheckBox.CheckedChanged += new System.EventHandler(this.includeCSharpCheckBox_CheckedChanged);
            // 
            // includeCPlusPlusCheckBox
            // 
            this.includeCPlusPlusCheckBox.AutoSize = true;
            this.includeCPlusPlusCheckBox.Location = new System.Drawing.Point(75, 3);
            this.includeCPlusPlusCheckBox.Name = "includeCPlusPlusCheckBox";
            this.includeCPlusPlusCheckBox.Size = new System.Drawing.Size(45, 17);
            this.includeCPlusPlusCheckBox.TabIndex = 1;
            this.includeCPlusPlusCheckBox.Text = "C++";
            this.includeCPlusPlusCheckBox.UseVisualStyleBackColor = true;
            this.includeCPlusPlusCheckBox.CheckedChanged += new System.EventHandler(this.includeCPlusPlusCheckBox_CheckedChanged);
            // 
            // includeXAMLCheckBox
            // 
            this.includeXAMLCheckBox.AutoSize = true;
            this.includeXAMLCheckBox.Location = new System.Drawing.Point(3, 26);
            this.includeXAMLCheckBox.Name = "includeXAMLCheckBox";
            this.includeXAMLCheckBox.Size = new System.Drawing.Size(55, 17);
            this.includeXAMLCheckBox.TabIndex = 2;
            this.includeXAMLCheckBox.Text = "XAML";
            this.includeXAMLCheckBox.UseVisualStyleBackColor = true;
            this.includeXAMLCheckBox.CheckedChanged += new System.EventHandler(this.includeXAMLCheckBox_CheckedChanged);
            // 
            // includeXMLCheckBox
            // 
            this.includeXMLCheckBox.AutoSize = true;
            this.includeXMLCheckBox.Location = new System.Drawing.Point(75, 26);
            this.includeXMLCheckBox.Name = "includeXMLCheckBox";
            this.includeXMLCheckBox.Size = new System.Drawing.Size(48, 17);
            this.includeXMLCheckBox.TabIndex = 3;
            this.includeXMLCheckBox.Text = "XML";
            this.includeXMLCheckBox.UseVisualStyleBackColor = true;
            this.includeXMLCheckBox.CheckedChanged += new System.EventHandler(this.includeXMLCheckBox_CheckedChanged);
            // 
            // includeHTMLCheckBox
            // 
            this.includeHTMLCheckBox.AutoSize = true;
            this.includeHTMLCheckBox.Location = new System.Drawing.Point(3, 49);
            this.includeHTMLCheckBox.Name = "includeHTMLCheckBox";
            this.includeHTMLCheckBox.Size = new System.Drawing.Size(56, 17);
            this.includeHTMLCheckBox.TabIndex = 4;
            this.includeHTMLCheckBox.Text = "HTML";
            this.includeHTMLCheckBox.UseVisualStyleBackColor = true;
            this.includeHTMLCheckBox.CheckedChanged += new System.EventHandler(this.includeHTMLCheckBox_CheckedChanged);
            // 
            // includeCSSCheckBox
            // 
            this.includeCSSCheckBox.AutoSize = true;
            this.includeCSSCheckBox.Location = new System.Drawing.Point(75, 49);
            this.includeCSSCheckBox.Name = "includeCSSCheckBox";
            this.includeCSSCheckBox.Size = new System.Drawing.Size(47, 17);
            this.includeCSSCheckBox.TabIndex = 5;
            this.includeCSSCheckBox.Text = "CSS";
            this.includeCSSCheckBox.UseVisualStyleBackColor = true;
            this.includeCSSCheckBox.CheckedChanged += new System.EventHandler(this.includeCSSCheckBox_CheckedChanged);
            // 
            // includeJavaScriptCheckBox
            // 
            this.includeJavaScriptCheckBox.AutoSize = true;
            this.includeJavaScriptCheckBox.Location = new System.Drawing.Point(139, 49);
            this.includeJavaScriptCheckBox.Name = "includeJavaScriptCheckBox";
            this.includeJavaScriptCheckBox.Size = new System.Drawing.Size(76, 17);
            this.includeJavaScriptCheckBox.TabIndex = 6;
            this.includeJavaScriptCheckBox.Text = "JavaScript";
            this.includeJavaScriptCheckBox.UseVisualStyleBackColor = true;
            this.includeJavaScriptCheckBox.CheckedChanged += new System.EventHandler(this.includeJavaScriptCheckBox_CheckedChanged);
            // 
            // CleanupFileTypesOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.includeGroupBox);
            this.Name = "CleanupFileTypesOptionsControl";
            this.Size = new System.Drawing.Size(350, 250);
            this.includeGroupBox.ResumeLayout(false);
            this.includeGroupBox.PerformLayout();
            this.includeTableLayoutPanel.ResumeLayout(false);
            this.includeTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion Component Designer generated code

        private System.Windows.Forms.GroupBox includeGroupBox;
        private System.Windows.Forms.TableLayoutPanel includeTableLayoutPanel;
        private System.Windows.Forms.CheckBox includeCSharpCheckBox;
        private System.Windows.Forms.CheckBox includeCPlusPlusCheckBox;
        private System.Windows.Forms.CheckBox includeXAMLCheckBox;
        private System.Windows.Forms.CheckBox includeCSSCheckBox;
        private System.Windows.Forms.CheckBox includeXMLCheckBox;
        private System.Windows.Forms.CheckBox includeHTMLCheckBox;
        private System.Windows.Forms.CheckBox includeJavaScriptCheckBox;
    }
}