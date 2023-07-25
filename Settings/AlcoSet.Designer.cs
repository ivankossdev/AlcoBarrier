namespace Settings
{
    partial class AlcoSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlcoSet));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.InnerSettings = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonInnerOk = new System.Windows.Forms.Button();
            this.textBoxInnerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AlcoSettings = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.InnerSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.InnerSettings);
            this.tabControl1.Controls.Add(this.AlcoSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // InnerSettings
            // 
            this.InnerSettings.Controls.Add(this.textBox2);
            this.InnerSettings.Controls.Add(this.label2);
            this.InnerSettings.Controls.Add(this.button2);
            this.InnerSettings.Controls.Add(this.buttonInnerOk);
            this.InnerSettings.Controls.Add(this.textBoxInnerIP);
            this.InnerSettings.Controls.Add(this.label1);
            this.InnerSettings.Location = new System.Drawing.Point(4, 22);
            this.InnerSettings.Name = "InnerSettings";
            this.InnerSettings.Padding = new System.Windows.Forms.Padding(3);
            this.InnerSettings.Size = new System.Drawing.Size(432, 400);
            this.InnerSettings.TabIndex = 0;
            this.InnerSettings.Text = "Inner Range";
            this.InnerSettings.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(65, 42);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(185, 20);
            this.textBox2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(351, 371);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // buttonInnerOk
            // 
            this.buttonInnerOk.Location = new System.Drawing.Point(270, 371);
            this.buttonInnerOk.Name = "buttonInnerOk";
            this.buttonInnerOk.Size = new System.Drawing.Size(75, 23);
            this.buttonInnerOk.TabIndex = 2;
            this.buttonInnerOk.Text = "OK";
            this.buttonInnerOk.UseVisualStyleBackColor = true;
            this.buttonInnerOk.Click += new System.EventHandler(this.buttonInnerOk_Click);
            // 
            // textBoxInnerIP
            // 
            this.textBoxInnerIP.Location = new System.Drawing.Point(65, 6);
            this.textBoxInnerIP.Name = "textBoxInnerIP";
            this.textBoxInnerIP.Size = new System.Drawing.Size(185, 20);
            this.textBoxInnerIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP адрес ";
            // 
            // AlcoSettings
            // 
            this.AlcoSettings.Location = new System.Drawing.Point(4, 22);
            this.AlcoSettings.Name = "AlcoSettings";
            this.AlcoSettings.Padding = new System.Windows.Forms.Padding(3);
            this.AlcoSettings.Size = new System.Drawing.Size(432, 400);
            this.AlcoSettings.TabIndex = 1;
            this.AlcoSettings.Text = "Alco Tester";
            this.AlcoSettings.UseVisualStyleBackColor = true;
            // 
            // AlcoSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 450);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AlcoSet";
            this.Text = "Настройки";
            this.tabControl1.ResumeLayout(false);
            this.InnerSettings.ResumeLayout(false);
            this.InnerSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage InnerSettings;
        private System.Windows.Forms.TabPage AlcoSettings;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonInnerOk;
        private System.Windows.Forms.TextBox textBoxInnerIP;
        private System.Windows.Forms.Label label1;
    }
}

