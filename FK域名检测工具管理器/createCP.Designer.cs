namespace FK域名检测工具管理器
{
    partial class createCP
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
            this.label_title = new System.Windows.Forms.Label();
            this.comboBox_company = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_checkPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_checkString = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_cancel = new System.Windows.Forms.Button();
            this.button_create = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_title.Location = new System.Drawing.Point(158, 9);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(117, 28);
            this.label_title.TabIndex = 1;
            this.label_title.Text = "创建监测点";
            // 
            // comboBox_company
            // 
            this.comboBox_company.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_company.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_company.FormattingEnabled = true;
            this.comboBox_company.Items.AddRange(new object[] {
            "A01",
            "A02",
            "A03",
            "A04",
            "A05",
            "A06",
            "B01",
            "C01"});
            this.comboBox_company.Location = new System.Drawing.Point(71, 52);
            this.comboBox_company.Name = "comboBox_company";
            this.comboBox_company.Size = new System.Drawing.Size(121, 27);
            this.comboBox_company.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "产品 (*)";
            // 
            // textBox_checkPath
            // 
            this.textBox_checkPath.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_checkPath.Location = new System.Drawing.Point(71, 92);
            this.textBox_checkPath.MaxLength = 64;
            this.textBox_checkPath.Name = "textBox_checkPath";
            this.textBox_checkPath.Size = new System.Drawing.Size(323, 25);
            this.textBox_checkPath.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "路径 (*)";
            // 
            // textBox_checkString
            // 
            this.textBox_checkString.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_checkString.Location = new System.Drawing.Point(71, 134);
            this.textBox_checkString.MaxLength = 64;
            this.textBox_checkString.Name = "textBox_checkString";
            this.textBox_checkString.Size = new System.Drawing.Size(323, 25);
            this.textBox_checkString.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "字符串 (*)";
            // 
            // button_cancel
            // 
            this.button_cancel.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_cancel.Location = new System.Drawing.Point(319, 179);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 35);
            this.button_cancel.TabIndex = 12;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // button_create
            // 
            this.button_create.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_create.Location = new System.Drawing.Point(200, 179);
            this.button_create.Name = "button_create";
            this.button_create.Size = new System.Drawing.Size(75, 35);
            this.button_create.TabIndex = 11;
            this.button_create.Text = "提交";
            this.button_create.UseVisualStyleBackColor = true;
            this.button_create.Click += new System.EventHandler(this.button_create_Click);
            // 
            // createCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(420, 237);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_create);
            this.Controls.Add(this.textBox_checkString);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_checkPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_company);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "createCP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "createCP";
            this.Load += new System.EventHandler(this.createCP_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.ComboBox comboBox_company;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_checkPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_checkString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button button_create;
    }
}