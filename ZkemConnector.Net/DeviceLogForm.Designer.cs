namespace ZkemConnector.NET
{
    partial class DeviceLogForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.logList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailText = new System.Windows.Forms.TextBox();
            this.buttonGetAndConnect = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.bw_deviceManager = new System.ComponentModel.BackgroundWorker();
            this.buttonClearLogs = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.logList);
            this.groupBox1.Location = new System.Drawing.Point(12, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(876, 329);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // logList
            // 
            this.logList.BackColor = System.Drawing.SystemColors.WindowText;
            this.logList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.logList.ForeColor = System.Drawing.SystemColors.Window;
            this.logList.HideSelection = false;
            this.logList.Location = new System.Drawing.Point(6, 19);
            this.logList.Name = "logList";
            this.logList.Size = new System.Drawing.Size(863, 303);
            this.logList.TabIndex = 0;
            this.logList.UseCompatibleStateImageBehavior = false;
            this.logList.View = System.Windows.Forms.View.Details;
            this.logList.SelectedIndexChanged += new System.EventHandler(this.logList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Description";
            this.columnHeader1.Width = 858;
            // 
            // detailText
            // 
            this.detailText.BackColor = System.Drawing.SystemColors.WindowText;
            this.detailText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailText.ForeColor = System.Drawing.SystemColors.Window;
            this.detailText.Location = new System.Drawing.Point(12, 390);
            this.detailText.Multiline = true;
            this.detailText.Name = "detailText";
            this.detailText.Size = new System.Drawing.Size(876, 113);
            this.detailText.TabIndex = 1;
            // 
            // buttonGetAndConnect
            // 
            this.buttonGetAndConnect.Location = new System.Drawing.Point(18, 12);
            this.buttonGetAndConnect.Name = "buttonGetAndConnect";
            this.buttonGetAndConnect.Size = new System.Drawing.Size(169, 26);
            this.buttonGetAndConnect.TabIndex = 1;
            this.buttonGetAndConnect.Text = "Start Processor";
            this.buttonGetAndConnect.UseVisualStyleBackColor = true;
            this.buttonGetAndConnect.Click += new System.EventHandler(this.buttonGetAndConnect_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(205, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(115, 26);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // bw_deviceManager
            // 
            this.bw_deviceManager.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_deviceManager_DoWork);
            this.bw_deviceManager.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_deviceManager_RunWorkerCompleted);
            // 
            // buttonClearLogs
            // 
            this.buttonClearLogs.Location = new System.Drawing.Point(806, 15);
            this.buttonClearLogs.Name = "buttonClearLogs";
            this.buttonClearLogs.Size = new System.Drawing.Size(75, 23);
            this.buttonClearLogs.TabIndex = 3;
            this.buttonClearLogs.Text = "Clear Logs";
            this.buttonClearLogs.UseVisualStyleBackColor = true;
            this.buttonClearLogs.Click += new System.EventHandler(this.buttonClearLogs_Click);
            // 
            // DeviceLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 516);
            this.Controls.Add(this.buttonClearLogs);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonGetAndConnect);
            this.Controls.Add(this.detailText);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DeviceLogForm";
            this.Text = "Device Log";
            this.Load += new System.EventHandler(this.DeviceLogForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox detailText;
        private System.Windows.Forms.Button buttonGetAndConnect;
        private System.Windows.Forms.ListView logList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button buttonStop;
        private System.ComponentModel.BackgroundWorker bw_deviceManager;
        private System.Windows.Forms.Button buttonClearLogs;
    }
}