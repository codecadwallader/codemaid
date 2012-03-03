namespace SteveCadwallader.CodeMaid.Options
{
    partial class ReorganizeOptionsControl
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
            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.runReorganizeAtStartOfCleanupCheckBox = new System.Windows.Forms.CheckBox();
            this.orderGroupBox = new System.Windows.Forms.GroupBox();
            this.alphabetizeMembersOfTheSameGroupCheckBox = new System.Windows.Forms.CheckBox();
            this.generalGroupBox.SuspendLayout();
            this.orderGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.generalGroupBox.Controls.Add(this.runReorganizeAtStartOfCleanupCheckBox);
            this.generalGroupBox.Location = new System.Drawing.Point(3, 3);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(344, 47);
            this.generalGroupBox.TabIndex = 0;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "General";
            // 
            // runReorganizeAtStartOfCleanupCheckBox
            // 
            this.runReorganizeAtStartOfCleanupCheckBox.AutoSize = true;
            this.runReorganizeAtStartOfCleanupCheckBox.Location = new System.Drawing.Point(7, 20);
            this.runReorganizeAtStartOfCleanupCheckBox.Name = "runReorganizeAtStartOfCleanupCheckBox";
            this.runReorganizeAtStartOfCleanupCheckBox.Size = new System.Drawing.Size(186, 17);
            this.runReorganizeAtStartOfCleanupCheckBox.TabIndex = 0;
            this.runReorganizeAtStartOfCleanupCheckBox.Text = "Run reorganize at start of cleanup";
            this.runReorganizeAtStartOfCleanupCheckBox.UseVisualStyleBackColor = true;
            this.runReorganizeAtStartOfCleanupCheckBox.CheckedChanged += new System.EventHandler(this.runReorganizeAtStartOfCleanupCheckBox_CheckedChanged);
            // 
            // orderGroupBox
            // 
            this.orderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orderGroupBox.Controls.Add(this.alphabetizeMembersOfTheSameGroupCheckBox);
            this.orderGroupBox.Location = new System.Drawing.Point(3, 56);
            this.orderGroupBox.Name = "orderGroupBox";
            this.orderGroupBox.Size = new System.Drawing.Size(344, 47);
            this.orderGroupBox.TabIndex = 1;
            this.orderGroupBox.TabStop = false;
            this.orderGroupBox.Text = "Order";
            // 
            // alphabetizeMembersOfTheSameGroupCheckBox
            // 
            this.alphabetizeMembersOfTheSameGroupCheckBox.AutoSize = true;
            this.alphabetizeMembersOfTheSameGroupCheckBox.Location = new System.Drawing.Point(7, 20);
            this.alphabetizeMembersOfTheSameGroupCheckBox.Name = "alphabetizeMembersOfTheSameGroupCheckBox";
            this.alphabetizeMembersOfTheSameGroupCheckBox.Size = new System.Drawing.Size(214, 17);
            this.alphabetizeMembersOfTheSameGroupCheckBox.TabIndex = 0;
            this.alphabetizeMembersOfTheSameGroupCheckBox.Text = "Alphabetize members of the same group";
            this.alphabetizeMembersOfTheSameGroupCheckBox.UseVisualStyleBackColor = true;
            this.alphabetizeMembersOfTheSameGroupCheckBox.CheckedChanged += new System.EventHandler(this.alphabetizeMembersOfTheSameGroupCheckBox_CheckedChanged);
            // 
            // ReorganizeOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.generalGroupBox);
            this.Controls.Add(this.orderGroupBox);
            this.Name = "ReorganizeOptionsControl";
            this.Size = new System.Drawing.Size(350, 200);
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.orderGroupBox.ResumeLayout(false);
            this.orderGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox generalGroupBox;
        private System.Windows.Forms.CheckBox runReorganizeAtStartOfCleanupCheckBox;
        private System.Windows.Forms.GroupBox orderGroupBox;
        private System.Windows.Forms.CheckBox alphabetizeMembersOfTheSameGroupCheckBox;
    }
}
