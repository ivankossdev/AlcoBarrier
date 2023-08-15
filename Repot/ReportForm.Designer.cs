namespace Repot
{
    partial class ReportForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CardCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSortByDate = new System.Windows.Forms.Button();
            this.buttonAllRecords = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAddInnerServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSynchroServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolReadMem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxNumCard = new System.Windows.Forms.TextBox();
            this.labelNumCard = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSort = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(12, 27);
            this.monthCalendar1.Margin = new System.Windows.Forms.Padding(3);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.Time,
            this.Result,
            this.CardCode,
            this.fio});
            this.dataGridView1.Location = new System.Drawing.Point(182, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(606, 411);
            this.dataGridView1.TabIndex = 2;
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Date.HeaderText = "Дата";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Time.HeaderText = "Время";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Result
            // 
            this.Result.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Result.HeaderText = "Концентрация";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            // 
            // CardCode
            // 
            this.CardCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CardCode.HeaderText = "Карточка";
            this.CardCode.Name = "CardCode";
            this.CardCode.ReadOnly = true;
            // 
            // fio
            // 
            this.fio.HeaderText = "ФИО";
            this.fio.Name = "fio";
            this.fio.ReadOnly = true;
            // 
            // buttonSortByDate
            // 
            this.buttonSortByDate.Location = new System.Drawing.Point(12, 195);
            this.buttonSortByDate.Name = "buttonSortByDate";
            this.buttonSortByDate.Size = new System.Drawing.Size(164, 23);
            this.buttonSortByDate.TabIndex = 3;
            this.buttonSortByDate.Text = "Сортировка по дате";
            this.buttonSortByDate.UseVisualStyleBackColor = true;
            this.buttonSortByDate.Click += new System.EventHandler(this.buttonSortByDate_Click);
            // 
            // buttonAllRecords
            // 
            this.buttonAllRecords.Location = new System.Drawing.Point(12, 224);
            this.buttonAllRecords.Name = "buttonAllRecords";
            this.buttonAllRecords.Size = new System.Drawing.Size(164, 23);
            this.buttonAllRecords.TabIndex = 4;
            this.buttonAllRecords.Text = "Показать все записи";
            this.buttonAllRecords.UseVisualStyleBackColor = true;
            this.buttonAllRecords.Click += new System.EventHandler(this.buttonAllRecords_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAdd,
            this.toolStripMenuItem5});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(79, 20);
            this.toolStripMenuItem1.Text = "Настройки";
            // 
            // toolAdd
            // 
            this.toolAdd.Name = "toolAdd";
            this.toolAdd.Size = new System.Drawing.Size(142, 22);
            this.toolAdd.Text = "Алкотестер";
            this.toolAdd.Click += new System.EventHandler(this.toolAdd_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddInnerServer,
            this.toolStripSynchroServer});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem5.Text = "Inner сервер";
            // 
            // toolStripAddInnerServer
            // 
            this.toolStripAddInnerServer.Name = "toolStripAddInnerServer";
            this.toolStripAddInnerServer.Size = new System.Drawing.Size(215, 22);
            this.toolStripAddInnerServer.Text = "Насиройки подключения";
            this.toolStripAddInnerServer.Click += new System.EventHandler(this.toolStripAddInnerServer_Click);
            // 
            // toolStripSynchroServer
            // 
            this.toolStripSynchroServer.Name = "toolStripSynchroServer";
            this.toolStripSynchroServer.Size = new System.Drawing.Size(215, 22);
            this.toolStripSynchroServer.Text = "Синхронизировать";
            this.toolStripSynchroServer.Click += new System.EventHandler(this.toolStripSynchroServer_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolReadMem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(82, 20);
            this.toolStripMenuItem2.Text = "Алкотестер";
            // 
            // toolReadMem
            // 
            this.toolReadMem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripMenuItem4});
            this.toolReadMem.Name = "toolReadMem";
            this.toolReadMem.Size = new System.Drawing.Size(175, 22);
            this.toolReadMem.Text = "Прочитать память";
            this.toolReadMem.MouseHover += new System.EventHandler(this.toolReadMem_MouseHover);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(200, 23);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItem4.Text = "Прочитать";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(140, 20);
            this.toolStripMenuItem3.Text = "Импортировать в CSV";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(182, 49);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(606, 389);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // textBoxNumCard
            // 
            this.textBoxNumCard.Location = new System.Drawing.Point(6, 32);
            this.textBoxNumCard.Name = "textBoxNumCard";
            this.textBoxNumCard.Size = new System.Drawing.Size(152, 20);
            this.textBoxNumCard.TabIndex = 9;
            // 
            // labelNumCard
            // 
            this.labelNumCard.AutoSize = true;
            this.labelNumCard.Location = new System.Drawing.Point(6, 16);
            this.labelNumCard.Name = "labelNumCard";
            this.labelNumCard.Size = new System.Drawing.Size(75, 13);
            this.labelNumCard.TabIndex = 10;
            this.labelNumCard.Text = "Номер карты";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSort);
            this.groupBox1.Controls.Add(this.labelNumCard);
            this.groupBox1.Controls.Add(this.textBoxNumCard);
            this.groupBox1.Location = new System.Drawing.Point(12, 253);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 185);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сортировка по параметрам";
            // 
            // buttonSort
            // 
            this.buttonSort.Location = new System.Drawing.Point(76, 58);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(82, 23);
            this.buttonSort.TabIndex = 11;
            this.buttonSort.Text = "Сортировать";
            this.buttonSort.UseVisualStyleBackColor = true;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "Отчет";
            this.saveFileDialog1.Filter = "csv files(*.csv)|*.csv|All files(*.*)|*.*";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonAllRecords);
            this.Controls.Add(this.buttonSortByDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отчет";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonSortByDate;
        private System.Windows.Forms.Button buttonAllRecords;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolReadMem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem toolAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn CardCode;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxNumCard;
        private System.Windows.Forms.Label labelNumCard;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSort;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripAddInnerServer;
        private System.Windows.Forms.ToolStripMenuItem toolStripSynchroServer;
        private System.Windows.Forms.DataGridViewTextBoxColumn fio;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

