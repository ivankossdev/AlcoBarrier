namespace AlcoBarrier
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGetUsers = new System.Windows.Forms.Button();
            this.buttonTestDb = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Info";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(15, 25);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(632, 330);
            this.textBox1.TabIndex = 2;
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(491, 361);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(75, 38);
            this.buttonOpen.TabIndex = 5;
            this.buttonOpen.Text = "Open Door ";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(572, 361);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 38);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGetUsers
            // 
            this.buttonGetUsers.Location = new System.Drawing.Point(15, 361);
            this.buttonGetUsers.Name = "buttonGetUsers";
            this.buttonGetUsers.Size = new System.Drawing.Size(95, 38);
            this.buttonGetUsers.TabIndex = 7;
            this.buttonGetUsers.Text = "Get all users";
            this.buttonGetUsers.UseVisualStyleBackColor = true;
            this.buttonGetUsers.Click += new System.EventHandler(this.buttonGetUsers_Click);
            // 
            // buttonTestDb
            // 
            this.buttonTestDb.Location = new System.Drawing.Point(116, 361);
            this.buttonTestDb.Name = "buttonTestDb";
            this.buttonTestDb.Size = new System.Drawing.Size(95, 38);
            this.buttonTestDb.TabIndex = 8;
            this.buttonTestDb.Text = "Test DB";
            this.buttonTestDb.UseVisualStyleBackColor = true;
            this.buttonTestDb.Click += new System.EventHandler(this.buttonTestDb_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 411);
            this.Controls.Add(this.buttonTestDb);
            this.Controls.Add(this.buttonGetUsers);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(675, 450);
            this.MinimumSize = new System.Drawing.Size(675, 450);
            this.Name = "MainForm";
            this.Text = "AlcoBarrier";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGetUsers;
        private System.Windows.Forms.Button buttonTestDb;
    }
}

