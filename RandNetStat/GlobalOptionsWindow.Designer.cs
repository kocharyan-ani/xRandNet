namespace RandNetStat
{
    partial class GlobalOptionsWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalOptionsWindow));
            this.valuesPanel = new System.Windows.Forms.Panel();
            this.valuesGrd = new System.Windows.Forms.DataGridView();
            this.parameterColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.researchInfo = new RandNetStat.ResearchInfo();
            this.close = new System.Windows.Forms.Button();
            this.save = new System.Windows.Forms.Button();
            this.locationDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.valuesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valuesGrd)).BeginInit();
            this.infoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // valuesPanel
            // 
            this.valuesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valuesPanel.Controls.Add(this.valuesGrd);
            this.valuesPanel.Location = new System.Drawing.Point(384, 15);
            this.valuesPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.valuesPanel.Name = "valuesPanel";
            this.valuesPanel.Size = new System.Drawing.Size(361, 433);
            this.valuesPanel.TabIndex = 2;
            // 
            // valuesGrd
            // 
            this.valuesGrd.AllowUserToAddRows = false;
            this.valuesGrd.AllowUserToDeleteRows = false;
            this.valuesGrd.AllowUserToResizeColumns = false;
            this.valuesGrd.AllowUserToResizeRows = false;
            this.valuesGrd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.valuesGrd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parameterColumn,
            this.valueColumn});
            this.valuesGrd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.valuesGrd.Location = new System.Drawing.Point(0, 0);
            this.valuesGrd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.valuesGrd.Name = "valuesGrd";
            this.valuesGrd.ReadOnly = true;
            this.valuesGrd.RowHeadersVisible = false;
            this.valuesGrd.RowHeadersWidth = 51;
            this.valuesGrd.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.valuesGrd.Size = new System.Drawing.Size(361, 433);
            this.valuesGrd.TabIndex = 0;
            // 
            // parameterColumn
            // 
            this.parameterColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.parameterColumn.HeaderText = "Parameter";
            this.parameterColumn.MinimumWidth = 6;
            this.parameterColumn.Name = "parameterColumn";
            this.parameterColumn.ReadOnly = true;
            // 
            // valueColumn
            // 
            this.valueColumn.HeaderText = "Value";
            this.valueColumn.MinimumWidth = 6;
            this.valueColumn.Name = "valueColumn";
            this.valueColumn.ReadOnly = true;
            this.valueColumn.Width = 125;
            // 
            // infoPanel
            // 
            this.infoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.infoPanel.Controls.Add(this.researchInfo);
            this.infoPanel.Location = new System.Drawing.Point(16, 18);
            this.infoPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(360, 433);
            this.infoPanel.TabIndex = 3;
            // 
            // researchInfo
            // 
            this.researchInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.researchInfo.Location = new System.Drawing.Point(0, 0);
            this.researchInfo.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.researchInfo.Name = "researchInfo";
            this.researchInfo.Size = new System.Drawing.Size(360, 433);
            this.researchInfo.TabIndex = 0;
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(645, 455);
            this.close.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(100, 28);
            this.close.TabIndex = 4;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.save.Location = new System.Drawing.Point(537, 455);
            this.save.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(100, 28);
            this.save.TabIndex = 7;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // GlobalOptionsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 501);
            this.Controls.Add(this.save);
            this.Controls.Add(this.close);
            this.Controls.Add(this.infoPanel);
            this.Controls.Add(this.valuesPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GlobalOptionsWindow";
            this.Load += new System.EventHandler(this.GlobalOptionsWindow_Load);
            this.valuesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.valuesGrd)).EndInit();
            this.infoPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel valuesPanel;
        private System.Windows.Forms.Panel infoPanel;
        private ResearchInfo researchInfo;
        private System.Windows.Forms.DataGridView valuesGrd;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.FolderBrowserDialog locationDlg;

    }
}