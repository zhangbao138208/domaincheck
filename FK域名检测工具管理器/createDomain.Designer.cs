namespace FK域名检测工具管理器
{
    partial class createDomain
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
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_company = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_CPName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.richTextBox_Remark = new System.Windows.Forms.RichTextBox();
            this.button_create = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.richTextBox_domain = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_loadDomainFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
            this.label_title.Location = new System.Drawing.Point(209, 10);
            this.label_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(123, 36);
            this.label_title.TabIndex = 0;
            this.label_title.Text = "创建域名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label2.Location = new System.Drawing.Point(27, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "产品 (*)";
            // 
            // comboBox_company
            // 
            this.comboBox_company.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_company.Enabled = false;
            this.comboBox_company.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.comboBox_company.FormattingEnabled = true;
            this.comboBox_company.Items.AddRange(new object[] {"A01", "A02", "A03", "A04", "A05", "A06", "B01", "C01"});
            this.comboBox_company.Location = new System.Drawing.Point(105, 59);
            this.comboBox_company.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox_company.Name = "comboBox_company";
            this.comboBox_company.Size = new System.Drawing.Size(160, 29);
            this.comboBox_company.TabIndex = 2;
            this.comboBox_company.SelectedIndexChanged += new System.EventHandler(this.comboBox_company_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label3.Location = new System.Drawing.Point(27, 188);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "域名 (*)";
            // 
            // comboBox_CPName
            // 
            this.comboBox_CPName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CPName.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.comboBox_CPName.FormattingEnabled = true;
            this.comboBox_CPName.Location = new System.Drawing.Point(105, 105);
            this.comboBox_CPName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox_CPName.Name = "comboBox_CPName";
            this.comboBox_CPName.Size = new System.Drawing.Size(160, 29);
            this.comboBox_CPName.TabIndex = 6;
            this.comboBox_CPName.SelectedIndexChanged += new System.EventHandler(this.comboBox_CPName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(9, 108);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "监测点 (*)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label5.Location = new System.Drawing.Point(51, 343);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 23);
            this.label5.TabIndex = 7;
            this.label5.Text = "备注";
            // 
            // richTextBox_Remark
            // 
            this.richTextBox_Remark.Location = new System.Drawing.Point(105, 344);
            this.richTextBox_Remark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox_Remark.MaxLength = 50;
            this.richTextBox_Remark.Name = "richTextBox_Remark";
            this.richTextBox_Remark.Size = new System.Drawing.Size(429, 64);
            this.richTextBox_Remark.TabIndex = 8;
            this.richTextBox_Remark.Text = "";
            // 
            // button_create
            // 
            this.button_create.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.button_create.Location = new System.Drawing.Point(249, 415);
            this.button_create.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_create.Name = "button_create";
            this.button_create.Size = new System.Drawing.Size(100, 40);
            this.button_create.TabIndex = 9;
            this.button_create.Text = "提交";
            this.button_create.UseVisualStyleBackColor = true;
            this.button_create.Click += new System.EventHandler(this.button_create_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.button_cancel.Location = new System.Drawing.Point(436, 415);
            this.button_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(100, 40);
            this.button_cancel.TabIndex = 10;
            this.button_cancel.Text = "取消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // richTextBox_domain
            // 
            this.richTextBox_domain.Location = new System.Drawing.Point(105, 189);
            this.richTextBox_domain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox_domain.MaxLength = 60000;
            this.richTextBox_domain.Name = "richTextBox_domain";
            this.richTextBox_domain.Size = new System.Drawing.Size(368, 147);
            this.richTextBox_domain.TabIndex = 11;
            this.richTextBox_domain.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (134)));
            this.label1.Location = new System.Drawing.Point(105, 158);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "支持多域名，分号分隔，如   baidu.com;google.com（最大10W字符 ）\r\n";
            // 
            // button_loadDomainFile
            // 
            this.button_loadDomainFile.Location = new System.Drawing.Point(483, 217);
            this.button_loadDomainFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_loadDomainFile.Name = "button_loadDomainFile";
            this.button_loadDomainFile.Size = new System.Drawing.Size(100, 88);
            this.button_loadDomainFile.TabIndex = 13;
            this.button_loadDomainFile.Text = "从文件加载";
            this.button_loadDomainFile.UseVisualStyleBackColor = true;
            this.button_loadDomainFile.Click += new System.EventHandler(this.button_loadDomainFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // createDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(596, 475);
            this.Controls.Add(this.button_loadDomainFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_domain);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_create);
            this.Controls.Add(this.richTextBox_Remark);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox_CPName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_company);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "createDomain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建监测点";
            this.Load += new System.EventHandler(this.createDomain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_company;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_CPName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox richTextBox_Remark;
        private System.Windows.Forms.Button button_create;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.RichTextBox richTextBox_domain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_loadDomainFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}