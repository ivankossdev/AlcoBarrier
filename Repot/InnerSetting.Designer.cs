namespace Repot
{
    partial class InnerSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InnerSetting));
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.textBoxAuthorization = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInnerOk = new System.Windows.Forms.Button();
            this.textBoxInnerIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(11, 67);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(53, 13);
            this.labelApiKey.TabIndex = 16;
            this.labelApiKey.Text = "Ключ API";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(93, 64);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(185, 20);
            this.textBoxApiKey.TabIndex = 15;
            // 
            // textBoxAuthorization
            // 
            this.textBoxAuthorization.Location = new System.Drawing.Point(93, 38);
            this.textBoxAuthorization.Name = "textBoxAuthorization";
            this.textBoxAuthorization.Size = new System.Drawing.Size(185, 20);
            this.textBoxAuthorization.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Авторизация";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(203, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Закрыть";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInnerOk
            // 
            this.buttonInnerOk.Location = new System.Drawing.Point(122, 90);
            this.buttonInnerOk.Name = "buttonInnerOk";
            this.buttonInnerOk.Size = new System.Drawing.Size(75, 23);
            this.buttonInnerOk.TabIndex = 11;
            this.buttonInnerOk.Text = "OK";
            this.buttonInnerOk.UseVisualStyleBackColor = true;
            this.buttonInnerOk.Click += new System.EventHandler(this.buttonInnerOk_Click);
            // 
            // textBoxInnerIP
            // 
            this.textBoxInnerIP.Location = new System.Drawing.Point(93, 12);
            this.textBoxInnerIP.Name = "textBoxInnerIP";
            this.textBoxInnerIP.Size = new System.Drawing.Size(185, 20);
            this.textBoxInnerIP.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "IP адрес ";
            // 
            // InnerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 124);
            this.Controls.Add(this.labelApiKey);
            this.Controls.Add(this.textBoxApiKey);
            this.Controls.Add(this.textBoxAuthorization);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonInnerOk);
            this.Controls.Add(this.textBoxInnerIP);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(309, 163);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(309, 163);
            this.Name = "InnerSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InnerSetting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.TextBox textBoxAuthorization;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInnerOk;
        private System.Windows.Forms.TextBox textBoxInnerIP;
        private System.Windows.Forms.Label label1;
    }
}