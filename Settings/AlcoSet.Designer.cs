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
            this.textBoxInnerInfo = new System.Windows.Forms.TextBox();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.textBoxAuthorization = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInnerOk = new System.Windows.Forms.Button();
            this.textBoxInnerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AlcoSettings = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxAlcoIP = new System.Windows.Forms.TextBox();
            this.textBoxAlcoInfo = new System.Windows.Forms.TextBox();
            this.buttonAlcoOk = new System.Windows.Forms.Button();
            this.buttonAlcoCancel = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.InnerSettings.SuspendLayout();
            this.AlcoSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.InnerSettings);
            this.tabControl1.Controls.Add(this.AlcoSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 284);
            this.tabControl1.TabIndex = 0;
            // 
            // InnerSettings
            // 
            this.InnerSettings.Controls.Add(this.textBoxInnerInfo);
            this.InnerSettings.Controls.Add(this.labelApiKey);
            this.InnerSettings.Controls.Add(this.textBoxApiKey);
            this.InnerSettings.Controls.Add(this.textBoxAuthorization);
            this.InnerSettings.Controls.Add(this.label2);
            this.InnerSettings.Controls.Add(this.buttonCancel);
            this.InnerSettings.Controls.Add(this.buttonInnerOk);
            this.InnerSettings.Controls.Add(this.textBoxInnerIP);
            this.InnerSettings.Controls.Add(this.label1);
            this.InnerSettings.Location = new System.Drawing.Point(4, 22);
            this.InnerSettings.Name = "InnerSettings";
            this.InnerSettings.Padding = new System.Windows.Forms.Padding(3);
            this.InnerSettings.Size = new System.Drawing.Size(432, 258);
            this.InnerSettings.TabIndex = 0;
            this.InnerSettings.Text = "Inner Range";
            this.InnerSettings.UseVisualStyleBackColor = true;
            // 
            // textBoxInnerInfo
            // 
            this.textBoxInnerInfo.Location = new System.Drawing.Point(6, 84);
            this.textBoxInnerInfo.Multiline = true;
            this.textBoxInnerInfo.Name = "textBoxInnerInfo";
            this.textBoxInnerInfo.ReadOnly = true;
            this.textBoxInnerInfo.Size = new System.Drawing.Size(420, 139);
            this.textBoxInnerInfo.TabIndex = 9;
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(3, 61);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(53, 13);
            this.labelApiKey.TabIndex = 8;
            this.labelApiKey.Text = "Ключ API";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(85, 58);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(185, 20);
            this.textBoxApiKey.TabIndex = 7;
            // 
            // textBoxAuthorization
            // 
            this.textBoxAuthorization.Location = new System.Drawing.Point(85, 32);
            this.textBoxAuthorization.Name = "textBoxAuthorization";
            this.textBoxAuthorization.Size = new System.Drawing.Size(185, 20);
            this.textBoxAuthorization.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Авторизация";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(351, 229);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInnerOk
            // 
            this.buttonInnerOk.Location = new System.Drawing.Point(270, 229);
            this.buttonInnerOk.Name = "buttonInnerOk";
            this.buttonInnerOk.Size = new System.Drawing.Size(75, 23);
            this.buttonInnerOk.TabIndex = 2;
            this.buttonInnerOk.Text = "OK";
            this.buttonInnerOk.UseVisualStyleBackColor = true;
            this.buttonInnerOk.Click += new System.EventHandler(this.buttonInnerOk_Click);
            // 
            // textBoxInnerIP
            // 
            this.textBoxInnerIP.Location = new System.Drawing.Point(85, 6);
            this.textBoxInnerIP.Name = "textBoxInnerIP";
            this.textBoxInnerIP.Size = new System.Drawing.Size(185, 20);
            this.textBoxInnerIP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP адрес ";
            // 
            // AlcoSettings
            // 
            this.AlcoSettings.Controls.Add(this.buttonAlcoCancel);
            this.AlcoSettings.Controls.Add(this.buttonAlcoOk);
            this.AlcoSettings.Controls.Add(this.textBoxAlcoInfo);
            this.AlcoSettings.Controls.Add(this.textBoxAlcoIP);
            this.AlcoSettings.Controls.Add(this.label3);
            this.AlcoSettings.Location = new System.Drawing.Point(4, 22);
            this.AlcoSettings.Name = "AlcoSettings";
            this.AlcoSettings.Padding = new System.Windows.Forms.Padding(3);
            this.AlcoSettings.Size = new System.Drawing.Size(432, 258);
            this.AlcoSettings.TabIndex = 1;
            this.AlcoSettings.Text = "Alco Tester";
            this.AlcoSettings.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "IP адрес ";
            // 
            // textBoxAlcoIP
            // 
            this.textBoxAlcoIP.Location = new System.Drawing.Point(65, 6);
            this.textBoxAlcoIP.Name = "textBoxAlcoIP";
            this.textBoxAlcoIP.Size = new System.Drawing.Size(185, 20);
            this.textBoxAlcoIP.TabIndex = 1;
            // 
            // textBoxAlcoInfo
            // 
            this.textBoxAlcoInfo.Location = new System.Drawing.Point(9, 32);
            this.textBoxAlcoInfo.Multiline = true;
            this.textBoxAlcoInfo.Name = "textBoxAlcoInfo";
            this.textBoxAlcoInfo.Size = new System.Drawing.Size(417, 191);
            this.textBoxAlcoInfo.TabIndex = 2;
            // 
            // buttonAlcoOk
            // 
            this.buttonAlcoOk.Location = new System.Drawing.Point(270, 229);
            this.buttonAlcoOk.Name = "buttonAlcoOk";
            this.buttonAlcoOk.Size = new System.Drawing.Size(75, 23);
            this.buttonAlcoOk.TabIndex = 3;
            this.buttonAlcoOk.Text = "OK";
            this.buttonAlcoOk.UseVisualStyleBackColor = true;
            this.buttonAlcoOk.Click += new System.EventHandler(this.buttonAlcoOk_Click);
            // 
            // buttonAlcoCancel
            // 
            this.buttonAlcoCancel.Location = new System.Drawing.Point(351, 229);
            this.buttonAlcoCancel.Name = "buttonAlcoCancel";
            this.buttonAlcoCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonAlcoCancel.TabIndex = 4;
            this.buttonAlcoCancel.Text = "Отмена";
            this.buttonAlcoCancel.UseVisualStyleBackColor = true;
            this.buttonAlcoCancel.Click += new System.EventHandler(this.buttonAlcoCancel_Click);
            // 
            // AlcoSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 312);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(487, 351);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(487, 351);
            this.Name = "AlcoSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Настройки";
            this.tabControl1.ResumeLayout(false);
            this.InnerSettings.ResumeLayout(false);
            this.InnerSettings.PerformLayout();
            this.AlcoSettings.ResumeLayout(false);
            this.AlcoSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage InnerSettings;
        private System.Windows.Forms.TabPage AlcoSettings;
        private System.Windows.Forms.TextBox textBoxAuthorization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInnerOk;
        private System.Windows.Forms.TextBox textBoxInnerIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxInnerInfo;
        private System.Windows.Forms.Button buttonAlcoCancel;
        private System.Windows.Forms.Button buttonAlcoOk;
        private System.Windows.Forms.TextBox textBoxAlcoInfo;
        private System.Windows.Forms.TextBox textBoxAlcoIP;
        private System.Windows.Forms.Label label3;
    }
}

