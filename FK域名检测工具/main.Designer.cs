namespace FK域名检测工具
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox_updatelog = new System.Windows.Forms.RichTextBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label_mac = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_elspTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_elspTime = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.button_pause = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_exit = new System.Windows.Forms.Button();
            this.label_account = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_company = new System.Windows.Forms.Label();
            this.button_selectAllProduct = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedListBox_product = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_elspTime)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(830, 443);
            this.splitContainer1.SplitterDistance = 625;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(623, 441);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox_log);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(615, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "日志";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_log.Location = new System.Drawing.Point(6, 6);
            this.richTextBox_log.MaxLength = 204800000;
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.ReadOnly = true;
            this.richTextBox_log.Size = new System.Drawing.Size(603, 401);
            this.richTextBox_log.TabIndex = 0;
            this.richTextBox_log.Text = "";
            this.richTextBox_log.TextChanged += new System.EventHandler(this.richTextBox_log_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox_updatelog);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(615, 415);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "更新说明";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox_updatelog
            // 
            this.richTextBox_updatelog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_updatelog.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_updatelog.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_updatelog.Name = "richTextBox_updatelog";
            this.richTextBox_updatelog.ReadOnly = true;
            this.richTextBox_updatelog.Size = new System.Drawing.Size(615, 415);
            this.richTextBox_updatelog.TabIndex = 2;
            this.richTextBox_updatelog.Text = "";
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label_mac);
            this.splitContainer2.Panel1.Controls.Add(this.label4);
            this.splitContainer2.Panel1.Controls.Add(this.label_elspTime);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.trackBar_elspTime);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.button_pause);
            this.splitContainer2.Panel1.Controls.Add(this.button_start);
            this.splitContainer2.Panel1.Controls.Add(this.button_exit);
            this.splitContainer2.Panel1.Controls.Add(this.label_account);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.label_company);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.button_selectAllProduct);
            this.splitContainer2.Panel2.Controls.Add(this.label5);
            this.splitContainer2.Panel2.Controls.Add(this.checkedListBox_product);
            this.splitContainer2.Size = new System.Drawing.Size(201, 443);
            this.splitContainer2.SplitterDistance = 200;
            this.splitContainer2.TabIndex = 0;
            // 
            // label_mac
            // 
            this.label_mac.Location = new System.Drawing.Point(68, 56);
            this.label_mac.Name = "label_mac";
            this.label_mac.Size = new System.Drawing.Size(127, 23);
            this.label_mac.TabIndex = 11;
            this.label_mac.Text = "00-00-00-00-00-00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "本机MAC：";
            // 
            // label_elspTime
            // 
            this.label_elspTime.AutoSize = true;
            this.label_elspTime.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_elspTime.Location = new System.Drawing.Point(59, 169);
            this.label_elspTime.Name = "label_elspTime";
            this.label_elspTime.Size = new System.Drawing.Size(25, 19);
            this.label_elspTime.TabIndex = 9;
            this.label_elspTime.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(81, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 19);
            this.label3.TabIndex = 8;
            this.label3.Text = "秒";
            // 
            // trackBar_elspTime
            // 
            this.trackBar_elspTime.Location = new System.Drawing.Point(96, 161);
            this.trackBar_elspTime.Maximum = 15;
            this.trackBar_elspTime.Minimum = 1;
            this.trackBar_elspTime.Name = "trackBar_elspTime";
            this.trackBar_elspTime.Size = new System.Drawing.Size(105, 45);
            this.trackBar_elspTime.TabIndex = 7;
            this.trackBar_elspTime.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBar_elspTime.Value = 10;
            this.trackBar_elspTime.Scroll += new System.EventHandler(this.trackBar_time_scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(3, 169);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "超时设置";
            // 
            // button_pause
            // 
            this.button_pause.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_pause.Location = new System.Drawing.Point(112, 101);
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size(64, 48);
            this.button_pause.TabIndex = 5;
            this.button_pause.Text = "暂停";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler(this.button_pause_Click);
            // 
            // button_start
            // 
            this.button_start.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_start.Location = new System.Drawing.Point(16, 101);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(64, 48);
            this.button_start.TabIndex = 4;
            this.button_start.Text = "开始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_exit
            // 
            this.button_exit.BackColor = System.Drawing.SystemColors.Control;
            this.button_exit.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_exit.ForeColor = System.Drawing.Color.Blue;
            this.button_exit.Location = new System.Drawing.Point(148, 5);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(47, 30);
            this.button_exit.TabIndex = 3;
            this.button_exit.Text = "退出";
            this.button_exit.UseVisualStyleBackColor = false;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // label_account
            // 
            this.label_account.AutoSize = true;
            this.label_account.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_account.Location = new System.Drawing.Point(36, 10);
            this.label_account.Name = "label_account";
            this.label_account.Size = new System.Drawing.Size(88, 19);
            this.label_account.TabIndex = 2;
            this.label_account.Text = "test_account";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(-1, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 10);
            this.label1.TabIndex = 1;
            // 
            // label_company
            // 
            this.label_company.AutoSize = true;
            this.label_company.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_company.Location = new System.Drawing.Point(0, 11);
            this.label_company.Name = "label_company";
            this.label_company.Size = new System.Drawing.Size(35, 19);
            this.label_company.TabIndex = 0;
            this.label_company.Text = "A01";
            // 
            // button_selectAllProduct
            // 
            this.button_selectAllProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_selectAllProduct.Location = new System.Drawing.Point(96, 176);
            this.button_selectAllProduct.Name = "button_selectAllProduct";
            this.button_selectAllProduct.Size = new System.Drawing.Size(75, 23);
            this.button_selectAllProduct.TabIndex = 2;
            this.button_selectAllProduct.Text = "全选";
            this.button_selectAllProduct.UseVisualStyleBackColor = true;
            this.button_selectAllProduct.Visible = false;
            this.button_selectAllProduct.Click += new System.EventHandler(this.button_selectAllProduct_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 19);
            this.label5.TabIndex = 1;
            this.label5.Text = "选择部门";
            this.label5.Visible = false;
            // 
            // checkedListBox_product
            // 
            this.checkedListBox_product.CheckOnClick = true;
            this.checkedListBox_product.Enabled = false;
            this.checkedListBox_product.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox_product.FormattingEnabled = true;
            this.checkedListBox_product.Items.AddRange(new object[] {
            "BTT",
            "AA",
            "BY",
            "ZZ",
            "CC",
            "KK",
            "BB",
            "YM"});
            this.checkedListBox_product.Location = new System.Drawing.Point(16, 24);
            this.checkedListBox_product.Name = "checkedListBox_product";
            this.checkedListBox_product.Size = new System.Drawing.Size(171, 184);
            this.checkedListBox_product.TabIndex = 0;
            this.checkedListBox_product.Visible = false;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 443);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "蜂鸟域名监测客户端工具";
            this.Load += new System.EventHandler(this.main_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_elspTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_company;
        private System.Windows.Forms.Label label_account;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Button button_pause;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar_elspTime;
        private System.Windows.Forms.Label label_elspTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox checkedListBox_product;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox richTextBox_updatelog;
        private System.Windows.Forms.Button button_selectAllProduct;
        private System.Windows.Forms.Label label_mac;
        private System.Windows.Forms.Label label4;
    }
}

