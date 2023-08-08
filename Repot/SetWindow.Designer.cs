namespace Repot
{
    partial class SetWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetWindow));
            this.textBoxCheckPoint = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.labelPoint = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.checkBoxEdit = new System.Windows.Forms.CheckBox();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkpoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Edit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxCheckPoint
            // 
            this.textBoxCheckPoint.Location = new System.Drawing.Point(12, 34);
            this.textBoxCheckPoint.Name = "textBoxCheckPoint";
            this.textBoxCheckPoint.Size = new System.Drawing.Size(197, 20);
            this.textBoxCheckPoint.TabIndex = 0;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(12, 73);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(197, 20);
            this.textBoxIP.TabIndex = 1;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(134, 99);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Добавить";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
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
            this.id,
            this.checkpoint,
            this.Ip,
            this.Edit,
            this.Delete});
            this.dataGridView1.Location = new System.Drawing.Point(215, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(475, 243);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // labelPoint
            // 
            this.labelPoint.AutoSize = true;
            this.labelPoint.Location = new System.Drawing.Point(13, 18);
            this.labelPoint.Name = "labelPoint";
            this.labelPoint.Size = new System.Drawing.Size(90, 13);
            this.labelPoint.TabIndex = 5;
            this.labelPoint.Text = "Название двери";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(13, 57);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(117, 13);
            this.labelIP.TabIndex = 6;
            this.labelIP.Text = "IP адрес алкотестера";
            // 
            // checkBoxEdit
            // 
            this.checkBoxEdit.AutoSize = true;
            this.checkBoxEdit.Location = new System.Drawing.Point(12, 238);
            this.checkBoxEdit.Name = "checkBoxEdit";
            this.checkBoxEdit.Size = new System.Drawing.Size(103, 17);
            this.checkBoxEdit.TabIndex = 7;
            this.checkBoxEdit.Text = "Редактировать";
            this.checkBoxEdit.UseVisualStyleBackColor = true;
            this.checkBoxEdit.CheckedChanged += new System.EventHandler(this.checkBoxEdit_CheckedChanged);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // checkpoint
            // 
            this.checkpoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.checkpoint.HeaderText = "Проходная ";
            this.checkpoint.Name = "checkpoint";
            this.checkpoint.ReadOnly = true;
            // 
            // Ip
            // 
            this.Ip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Ip.HeaderText = "IP адрес ";
            this.Ip.Name = "Ip";
            this.Ip.ReadOnly = true;
            // 
            // Edit
            // 
            this.Edit.HeaderText = "";
            this.Edit.Name = "Edit";
            this.Edit.ReadOnly = true;
            this.Edit.Text = "Редактировать";
            this.Edit.UseColumnTextForButtonValue = true;
            this.Edit.Visible = false;
            // 
            // Delete
            // 
            this.Delete.HeaderText = "";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "Удалить";
            this.Delete.UseColumnTextForButtonValue = true;
            this.Delete.Visible = false;
            // 
            // SetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 267);
            this.Controls.Add(this.checkBoxEdit);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.labelPoint);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.textBoxCheckPoint);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetWindow";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCheckPoint;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelPoint;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.CheckBox checkBoxEdit;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn checkpoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ip;
        private System.Windows.Forms.DataGridViewButtonColumn Edit;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
    }
}