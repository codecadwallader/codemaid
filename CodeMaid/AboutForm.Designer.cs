namespace SteveCadwallader.CodeMaid
{
    partial class AboutForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.siteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.twitterLinkLabel = new System.Windows.Forms.LinkLabel();
            this.emailLinkLabel = new System.Windows.Forms.LinkLabel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // siteLinkLabel
            //
            this.siteLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(147)))), ((int)(((byte)(182)))));
            this.siteLinkLabel.AutoSize = true;
            this.siteLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.siteLinkLabel.Font = new System.Drawing.Font("Gordon Heights", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.siteLinkLabel.LinkColor = System.Drawing.Color.Black;
            this.siteLinkLabel.Location = new System.Drawing.Point(198, 119);
            this.siteLinkLabel.Name = "siteLinkLabel";
            this.siteLinkLabel.Size = new System.Drawing.Size(349, 19);
            this.siteLinkLabel.TabIndex = 2;
            this.siteLinkLabel.TabStop = true;
            this.siteLinkLabel.Text = "www.bitbucket.org/s_cadwallader/codemaid/";
            this.siteLinkLabel.VisitedLinkColor = System.Drawing.Color.Black;
            this.siteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.siteLinkLabel_LinkClicked);
            //
            // twitterLinkLabel
            //
            this.twitterLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(147)))), ((int)(((byte)(182)))));
            this.twitterLinkLabel.AutoSize = true;
            this.twitterLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.twitterLinkLabel.Font = new System.Drawing.Font("Gordon Heights", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.twitterLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.twitterLinkLabel.LinkColor = System.Drawing.Color.Black;
            this.twitterLinkLabel.Location = new System.Drawing.Point(218, 166);
            this.twitterLinkLabel.Name = "twitterLinkLabel";
            this.twitterLinkLabel.Size = new System.Drawing.Size(94, 19);
            this.twitterLinkLabel.TabIndex = 3;
            this.twitterLinkLabel.TabStop = true;
            this.twitterLinkLabel.Text = "@codemaid";
            this.twitterLinkLabel.VisitedLinkColor = System.Drawing.Color.Black;
            this.twitterLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.twitterLinkLabel_LinkClicked);
            //
            // emailLinkLabel
            //
            this.emailLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(147)))), ((int)(((byte)(182)))));
            this.emailLinkLabel.AutoSize = true;
            this.emailLinkLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.emailLinkLabel.Font = new System.Drawing.Font("Gordon Heights", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.emailLinkLabel.LinkColor = System.Drawing.Color.Black;
            this.emailLinkLabel.Location = new System.Drawing.Point(379, 166);
            this.emailLinkLabel.Name = "emailLinkLabel";
            this.emailLinkLabel.Size = new System.Drawing.Size(171, 19);
            this.emailLinkLabel.TabIndex = 4;
            this.emailLinkLabel.TabStop = true;
            this.emailLinkLabel.Text = "codemaid@gmail.com";
            this.emailLinkLabel.VisitedLinkColor = System.Drawing.Color.Black;
            this.emailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.emailLinkLabel_LinkClicked);
            //
            // versionLabel
            //
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Font = new System.Drawing.Font("Joyful Juliana", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(147)))), ((int)(((byte)(182)))));
            this.versionLabel.Location = new System.Drawing.Point(350, 72);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(190, 37);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "#v0.0.0#";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // closeButton
            //
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(0, 0);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(0, 0);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            //
            // AboutForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SteveCadwallader.CodeMaid.VSPackage._502;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(550, 191);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.siteLinkLabel);
            this.Controls.Add(this.twitterLinkLabel);
            this.Controls.Add(this.emailLinkLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About CodeMaid v0.3.4";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion Windows Form Designer generated code

        private System.Windows.Forms.LinkLabel siteLinkLabel;
        private System.Windows.Forms.LinkLabel twitterLinkLabel;
        private System.Windows.Forms.LinkLabel emailLinkLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button closeButton;
    }
}