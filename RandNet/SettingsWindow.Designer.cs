namespace RandNet
{
    partial class SettingsWindow
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
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.CancelSettingsButton = new System.Windows.Forms.Button();
            this.browserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.generalGrp = new System.Windows.Forms.GroupBox();
            this.tracingGrp = new System.Windows.Forms.GroupBox();
            this.neighbourshipRadio = new System.Windows.Forms.RadioButton();
            this.matrixRadio = new System.Windows.Forms.RadioButton();
            this.tracingDirectory = new System.Windows.Forms.Label();
            this.tracingDirectoryTxt = new System.Windows.Forms.TextBox();
            this.tracingBrowse = new System.Windows.Forms.Button();
            this.loggingGrp = new System.Windows.Forms.GroupBox();
            this.loggingDirectory = new System.Windows.Forms.Label();
            this.loggingDirectoryTxt = new System.Windows.Forms.TextBox();
            this.loggingBrowse = new System.Windows.Forms.Button();
            this.storageGrp = new System.Windows.Forms.GroupBox();
            this.storageBrowse = new System.Windows.Forms.Button();
            this.storageDirectoryTxt = new System.Windows.Forms.TextBox();
            this.storageDirectory = new System.Windows.Forms.Label();
            this.generalGrp.SuspendLayout();
            this.tracingGrp.SuspendLayout();
            this.loggingGrp.SuspendLayout();
            this.storageGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Location = new System.Drawing.Point(607, 354);
            this.SaveSettingsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(100, 28);
            this.SaveSettingsButton.TabIndex = 3;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // CancelSettingsButton
            // 
            this.CancelSettingsButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelSettingsButton.Location = new System.Drawing.Point(715, 354);
            this.CancelSettingsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelSettingsButton.Name = "CancelSettingsButton";
            this.CancelSettingsButton.Size = new System.Drawing.Size(100, 28);
            this.CancelSettingsButton.TabIndex = 4;
            this.CancelSettingsButton.Text = "Cancel";
            this.CancelSettingsButton.UseVisualStyleBackColor = true;
            // 
            // generalGrp
            // 
            this.generalGrp.Controls.Add(this.tracingGrp);
            this.generalGrp.Controls.Add(this.loggingGrp);
            this.generalGrp.Controls.Add(this.storageGrp);
            this.generalGrp.Location = new System.Drawing.Point(16, 15);
            this.generalGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.generalGrp.Name = "generalGrp";
            this.generalGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.generalGrp.Size = new System.Drawing.Size(799, 321);
            this.generalGrp.TabIndex = 0;
            this.generalGrp.TabStop = false;
            this.generalGrp.Text = "General";
            // 
            // tracingGrp
            // 
            this.tracingGrp.Controls.Add(this.neighbourshipRadio);
            this.tracingGrp.Controls.Add(this.matrixRadio);
            this.tracingGrp.Controls.Add(this.tracingDirectory);
            this.tracingGrp.Controls.Add(this.tracingDirectoryTxt);
            this.tracingGrp.Controls.Add(this.tracingBrowse);
            this.tracingGrp.Location = new System.Drawing.Point(16, 191);
            this.tracingGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tracingGrp.Name = "tracingGrp";
            this.tracingGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tracingGrp.Size = new System.Drawing.Size(767, 114);
            this.tracingGrp.TabIndex = 0;
            this.tracingGrp.TabStop = false;
            this.tracingGrp.Text = "Tracing";
            // 
            // neighbourshipRadio
            // 
            this.neighbourshipRadio.AutoSize = true;
            this.neighbourshipRadio.Location = new System.Drawing.Point(221, 74);
            this.neighbourshipRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.neighbourshipRadio.Name = "neighbourshipRadio";
            this.neighbourshipRadio.Size = new System.Drawing.Size(121, 21);
            this.neighbourshipRadio.TabIndex = 14;
            this.neighbourshipRadio.Text = "Neighbourship";
            this.neighbourshipRadio.UseVisualStyleBackColor = true;
            // 
            // matrixRadio
            // 
            this.matrixRadio.AutoSize = true;
            this.matrixRadio.Location = new System.Drawing.Point(129, 74);
            this.matrixRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.matrixRadio.Name = "matrixRadio";
            this.matrixRadio.Size = new System.Drawing.Size(66, 21);
            this.matrixRadio.TabIndex = 13;
            this.matrixRadio.Text = "Matrix";
            this.matrixRadio.UseVisualStyleBackColor = true;
            // 
            // tracingDirectory
            // 
            this.tracingDirectory.AutoSize = true;
            this.tracingDirectory.Location = new System.Drawing.Point(8, 34);
            this.tracingDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tracingDirectory.Name = "tracingDirectory";
            this.tracingDirectory.Size = new System.Drawing.Size(114, 17);
            this.tracingDirectory.TabIndex = 10;
            this.tracingDirectory.Text = "Output directory:";
            // 
            // tracingDirectoryTxt
            // 
            this.tracingDirectoryTxt.Location = new System.Drawing.Point(129, 31);
            this.tracingDirectoryTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tracingDirectoryTxt.Name = "tracingDirectoryTxt";
            this.tracingDirectoryTxt.Size = new System.Drawing.Size(516, 22);
            this.tracingDirectoryTxt.TabIndex = 11;
            // 
            // tracingBrowse
            // 
            this.tracingBrowse.Location = new System.Drawing.Point(655, 31);
            this.tracingBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tracingBrowse.Name = "tracingBrowse";
            this.tracingBrowse.Size = new System.Drawing.Size(100, 25);
            this.tracingBrowse.TabIndex = 12;
            this.tracingBrowse.Text = "Browse...";
            this.tracingBrowse.UseVisualStyleBackColor = true;
            this.tracingBrowse.Click += new System.EventHandler(this.tracingBrowseBtn_Click);
            // 
            // loggingGrp
            // 
            this.loggingGrp.Controls.Add(this.loggingDirectory);
            this.loggingGrp.Controls.Add(this.loggingDirectoryTxt);
            this.loggingGrp.Controls.Add(this.loggingBrowse);
            this.loggingGrp.Location = new System.Drawing.Point(16, 20);
            this.loggingGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loggingGrp.Name = "loggingGrp";
            this.loggingGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loggingGrp.Size = new System.Drawing.Size(767, 79);
            this.loggingGrp.TabIndex = 0;
            this.loggingGrp.TabStop = false;
            this.loggingGrp.Text = "Logging";
            // 
            // loggingDirectory
            // 
            this.loggingDirectory.AutoSize = true;
            this.loggingDirectory.Location = new System.Drawing.Point(8, 36);
            this.loggingDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggingDirectory.Name = "loggingDirectory";
            this.loggingDirectory.Size = new System.Drawing.Size(114, 17);
            this.loggingDirectory.TabIndex = 1;
            this.loggingDirectory.Text = "Output directory:";
            // 
            // loggingDirectoryTxt
            // 
            this.loggingDirectoryTxt.Location = new System.Drawing.Point(129, 32);
            this.loggingDirectoryTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loggingDirectoryTxt.Name = "loggingDirectoryTxt";
            this.loggingDirectoryTxt.Size = new System.Drawing.Size(516, 22);
            this.loggingDirectoryTxt.TabIndex = 1;
            // 
            // loggingBrowse
            // 
            this.loggingBrowse.Location = new System.Drawing.Point(655, 32);
            this.loggingBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.loggingBrowse.Name = "loggingBrowse";
            this.loggingBrowse.Size = new System.Drawing.Size(100, 25);
            this.loggingBrowse.TabIndex = 2;
            this.loggingBrowse.Text = "Browse...";
            this.loggingBrowse.UseVisualStyleBackColor = true;
            this.loggingBrowse.Click += new System.EventHandler(this.loggingBrowseButton_Click);
            // 
            // storageGrp
            // 
            this.storageGrp.Controls.Add(this.storageBrowse);
            this.storageGrp.Controls.Add(this.storageDirectoryTxt);
            this.storageGrp.Controls.Add(this.storageDirectory);
            this.storageGrp.Location = new System.Drawing.Point(16, 106);
            this.storageGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageGrp.Name = "storageGrp";
            this.storageGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageGrp.Size = new System.Drawing.Size(767, 78);
            this.storageGrp.TabIndex = 3;
            this.storageGrp.TabStop = false;
            this.storageGrp.Text = "Default storage locations";
            // 
            // storageBrowse
            // 
            this.storageBrowse.Location = new System.Drawing.Point(655, 30);
            this.storageBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageBrowse.Name = "storageBrowse";
            this.storageBrowse.Size = new System.Drawing.Size(100, 25);
            this.storageBrowse.TabIndex = 6;
            this.storageBrowse.Text = "Browse...";
            this.storageBrowse.UseVisualStyleBackColor = true;
            this.storageBrowse.Click += new System.EventHandler(this.storageBrowseButton_Click);
            // 
            // storageDirectoryTxt
            // 
            this.storageDirectoryTxt.Location = new System.Drawing.Point(129, 30);
            this.storageDirectoryTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.storageDirectoryTxt.Name = "storageDirectoryTxt";
            this.storageDirectoryTxt.Size = new System.Drawing.Size(516, 22);
            this.storageDirectoryTxt.TabIndex = 5;
            // 
            // storageDirectory
            // 
            this.storageDirectory.AutoSize = true;
            this.storageDirectory.Location = new System.Drawing.Point(29, 33);
            this.storageDirectory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.storageDirectory.Name = "storageDirectory";
            this.storageDirectory.Size = new System.Drawing.Size(93, 17);
            this.storageDirectory.TabIndex = 4;
            this.storageDirectory.Text = "File directory:";
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this.SaveSettingsButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelSettingsButton;
            this.ClientSize = new System.Drawing.Size(832, 398);
            this.Controls.Add(this.generalGrp);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.CancelSettingsButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.generalGrp.ResumeLayout(false);
            this.tracingGrp.ResumeLayout(false);
            this.tracingGrp.PerformLayout();
            this.loggingGrp.ResumeLayout(false);
            this.loggingGrp.PerformLayout();
            this.storageGrp.ResumeLayout(false);
            this.storageGrp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.Button CancelSettingsButton;
        private System.Windows.Forms.FolderBrowserDialog browserDlg;
        private System.Windows.Forms.GroupBox generalGrp;
        private System.Windows.Forms.GroupBox tracingGrp;
        private System.Windows.Forms.Label tracingDirectory;
        private System.Windows.Forms.TextBox tracingDirectoryTxt;
        private System.Windows.Forms.Button tracingBrowse;
        private System.Windows.Forms.GroupBox loggingGrp;
        private System.Windows.Forms.Label loggingDirectory;
        private System.Windows.Forms.TextBox loggingDirectoryTxt;
        private System.Windows.Forms.Button loggingBrowse;
        private System.Windows.Forms.GroupBox storageGrp;
        private System.Windows.Forms.Button storageBrowse;
        private System.Windows.Forms.TextBox storageDirectoryTxt;
        private System.Windows.Forms.Label storageDirectory;
        private System.Windows.Forms.RadioButton neighbourshipRadio;
        private System.Windows.Forms.RadioButton matrixRadio;

    }
}