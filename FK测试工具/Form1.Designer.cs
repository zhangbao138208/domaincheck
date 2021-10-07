namespace FK测试工具
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_start = new System.Windows.Forms.Button();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.button_copy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(23, 46);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(143, 142);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "开始测试";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.Location = new System.Drawing.Point(189, 12);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.ReadOnly = true;
            this.richTextBox_log.Size = new System.Drawing.Size(316, 187);
            this.richTextBox_log.TabIndex = 1;
            this.richTextBox_log.Text = "";
            // 
            // button_copy
            // 
            this.button_copy.Location = new System.Drawing.Point(312, 211);
            this.button_copy.Name = "button_copy";
            this.button_copy.Size = new System.Drawing.Size(90, 23);
            this.button_copy.TabIndex = 2;
            this.button_copy.Text = "复制到剪切板";
            this.button_copy.UseVisualStyleBackColor = true;
            this.button_copy.Click += new System.EventHandler(this.button_copy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 246);
            this.Controls.Add(this.button_copy);
            this.Controls.Add(this.richTextBox_log);
            this.Controls.Add(this.button_start);
            this.Name = "Form1";
            this.Text = "FK蜂鸟监测工具测试工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.Button button_copy;
    }
}

