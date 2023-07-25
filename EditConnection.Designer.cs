namespace AlcoBarrier
{
    partial class EditConnection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditConnection));
            this.labelIR = new System.Windows.Forms.Label();
            this.textBoxIR = new System.Windows.Forms.TextBox();
            this.textBoxAlco = new System.Windows.Forms.TextBox();
            this.labelAlco = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelIR
            // 
            this.labelIR.AutoSize = true;
            this.labelIR.Location = new System.Drawing.Point(12, 9);
            this.labelIR.Name = "labelIR";
            this.labelIR.Size = new System.Drawing.Size(64, 13);
            this.labelIR.TabIndex = 0;
            this.labelIR.Text = "IP адрес IR";
            // 
            // textBoxIR
            // 
            this.textBoxIR.Location = new System.Drawing.Point(15, 25);
            this.textBoxIR.Name = "textBoxIR";
            this.textBoxIR.Size = new System.Drawing.Size(257, 20);
            this.textBoxIR.TabIndex = 1;
            // 
            // textBoxAlco
            // 
            this.textBoxAlco.Location = new System.Drawing.Point(15, 64);
            this.textBoxAlco.Name = "textBoxAlco";
            this.textBoxAlco.Size = new System.Drawing.Size(257, 20);
            this.textBoxAlco.TabIndex = 2;
            // 
            // labelAlco
            // 
            this.labelAlco.AutoSize = true;
            this.labelAlco.Location = new System.Drawing.Point(12, 48);
            this.labelAlco.Name = "labelAlco";
            this.labelAlco.Size = new System.Drawing.Size(120, 13);
            this.labelAlco.TabIndex = 3;
            this.labelAlco.Text = "IP адрес Алкобарьера";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(116, 90);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(197, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // EditConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 121);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelAlco);
            this.Controls.Add(this.textBoxAlco);
            this.Controls.Add(this.textBoxIR);
            this.Controls.Add(this.labelIR);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 160);
            this.Name = "EditConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки подключения";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIR;
        private System.Windows.Forms.TextBox textBoxIR;
        private System.Windows.Forms.TextBox textBoxAlco;
        private System.Windows.Forms.Label labelAlco;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}