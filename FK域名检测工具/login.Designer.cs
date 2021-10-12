namespace FK域名检测工具
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_account = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label_errorInfo = new System.Windows.Forms.Label();
            this.button_login = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label_MAC = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "账号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "密码：";
            // 
            // textBox_account
            // 
            this.textBox_account.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_account.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox_account.Location = new System.Drawing.Point(112, 35);
            this.textBox_account.MaxLength = 15;
            this.textBox_account.Name = "textBox_account";
            this.textBox_account.ShortcutsEnabled = false;
            this.textBox_account.Size = new System.Drawing.Size(174, 29);
            this.textBox_account.TabIndex = 2;
            this.textBox_account.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_account_KeyPress);
            // 
            // textBox_password
            // 
            this.textBox_password.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_password.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox_password.Location = new System.Drawing.Point(112, 78);
            this.textBox_password.MaxLength = 15;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.ShortcutsEnabled = false;
            this.textBox_password.Size = new System.Drawing.Size(174, 29);
            this.textBox_password.TabIndex = 3;
            this.textBox_password.UseSystemPasswordChar = true;
            this.textBox_password.Enter += new System.EventHandler(this.textBox_password_Enter);
            this.textBox_password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_password_KeyPress);
            this.textBox_password.Leave += new System.EventHandler(this.textBox_password_Leave);
            // 
            // label_errorInfo
            // 
            this.label_errorInfo.AutoSize = true;
            this.label_errorInfo.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_errorInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label_errorInfo.Location = new System.Drawing.Point(108, 159);
            this.label_errorInfo.Name = "label_errorInfo";
            this.label_errorInfo.Size = new System.Drawing.Size(178, 19);
            this.label_errorInfo.TabIndex = 5;
            this.label_errorInfo.Text = "账号或密码错误，请重新输入";
            this.label_errorInfo.Visible = false;
            // 
            // button_login
            // 
            this.button_login.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_login.Location = new System.Drawing.Point(131, 199);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(99, 38);
            this.button_login.TabIndex = 6;
            this.button_login.Text = "登录";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "本机MAC：";
            // 
            // label_MAC
            // 
            this.label_MAC.Location = new System.Drawing.Point(131, 129);
            this.label_MAC.Name = "label_MAC";
            this.label_MAC.Size = new System.Drawing.Size(135, 13);
            this.label_MAC.TabIndex = 8;
            this.label_MAC.Text = "00-00-00-00-00-00";
            this.label_MAC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 249);
            this.Controls.Add(this.label_MAC);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_login);
            this.Controls.Add(this.label_errorInfo);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textBox_account);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "蜂鸟域名检测客户端工具";
            this.Load += new System.EventHandler(this.login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_account;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label_errorInfo;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_MAC;
    }
}