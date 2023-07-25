namespace AlcoBarrier
{
    partial class EditDataBases
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDataBases));
            this.buttonCreateEventsDB = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonSyncBDInnerage = new System.Windows.Forms.Button();
            this.buttonCreateSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCreateEventsDB
            // 
            this.buttonCreateEventsDB.Location = new System.Drawing.Point(12, 12);
            this.buttonCreateEventsDB.Name = "buttonCreateEventsDB";
            this.buttonCreateEventsDB.Size = new System.Drawing.Size(124, 50);
            this.buttonCreateEventsDB.TabIndex = 0;
            this.buttonCreateEventsDB.Text = "Создать БД событий";
            this.buttonCreateEventsDB.UseVisualStyleBackColor = true;
            this.buttonCreateEventsDB.Click += new System.EventHandler(this.buttonCreateEventsDB_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(142, 12);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(310, 197);
            this.textBoxLog.TabIndex = 1;
            // 
            // buttonSyncBDInnerage
            // 
            this.buttonSyncBDInnerage.Location = new System.Drawing.Point(12, 68);
            this.buttonSyncBDInnerage.Name = "buttonSyncBDInnerage";
            this.buttonSyncBDInnerage.Size = new System.Drawing.Size(124, 50);
            this.buttonSyncBDInnerage.TabIndex = 2;
            this.buttonSyncBDInnerage.Text = "Синхронизировать БД с Innerage";
            this.buttonSyncBDInnerage.UseVisualStyleBackColor = true;
            this.buttonSyncBDInnerage.Click += new System.EventHandler(this.buttonSyncBDInnerage_Click);
            // 
            // buttonCreateSettings
            // 
            this.buttonCreateSettings.Location = new System.Drawing.Point(12, 124);
            this.buttonCreateSettings.Name = "buttonCreateSettings";
            this.buttonCreateSettings.Size = new System.Drawing.Size(124, 50);
            this.buttonCreateSettings.TabIndex = 3;
            this.buttonCreateSettings.Text = "Создать БД настроек";
            this.buttonCreateSettings.UseVisualStyleBackColor = true;
            this.buttonCreateSettings.Click += new System.EventHandler(this.buttonCreateSettings_Click);
            // 
            // EditDataBases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 221);
            this.Controls.Add(this.buttonCreateSettings);
            this.Controls.Add(this.buttonSyncBDInnerage);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonCreateEventsDB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditDataBases";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание БД";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCreateEventsDB;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonSyncBDInnerage;
        private System.Windows.Forms.Button buttonCreateSettings;
    }
}