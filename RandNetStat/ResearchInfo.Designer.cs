namespace RandNetStat
{
    partial class ResearchInfo
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
            this.researchInfoGrp = new System.Windows.Forms.GroupBox();
            this.researchInfoTable = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.researchInfoGrp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.researchInfoTable)).BeginInit();
            this.SuspendLayout();
            // 
            // researchInfoGrp
            // 
            this.researchInfoGrp.Controls.Add(this.researchInfoTable);
            this.researchInfoGrp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.researchInfoGrp.Location = new System.Drawing.Point(0, 0);
            this.researchInfoGrp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.researchInfoGrp.Name = "researchInfoGrp";
            this.researchInfoGrp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.researchInfoGrp.Size = new System.Drawing.Size(356, 423);
            this.researchInfoGrp.TabIndex = 0;
            this.researchInfoGrp.TabStop = false;
            this.researchInfoGrp.Text = "Research Information";
            // 
            // researchInfoTable
            // 
            this.researchInfoTable.AllowUserToAddRows = false;
            this.researchInfoTable.AllowUserToDeleteRows = false;
            this.researchInfoTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.researchInfoTable.ColumnHeadersVisible = false;
            this.researchInfoTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.valueColumn});
            this.researchInfoTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.researchInfoTable.Location = new System.Drawing.Point(4, 19);
            this.researchInfoTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.researchInfoTable.Name = "researchInfoTable";
            this.researchInfoTable.ReadOnly = true;
            this.researchInfoTable.RowHeadersVisible = false;
            this.researchInfoTable.RowHeadersWidth = 51;
            this.researchInfoTable.Size = new System.Drawing.Size(348, 400);
            this.researchInfoTable.TabIndex = 0;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.MinimumWidth = 6;
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // valueColumn
            // 
            this.valueColumn.HeaderText = "Value";
            this.valueColumn.MinimumWidth = 6;
            this.valueColumn.Name = "valueColumn";
            this.valueColumn.ReadOnly = true;
            this.valueColumn.Width = 130;
            // 
            // ResearchInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.researchInfoGrp);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ResearchInfo";
            this.Size = new System.Drawing.Size(356, 423);
            this.researchInfoGrp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.researchInfoTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox researchInfoGrp;
        private System.Windows.Forms.DataGridView researchInfoTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
    }
}
