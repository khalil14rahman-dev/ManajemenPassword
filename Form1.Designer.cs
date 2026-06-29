namespace Project_KPL_ManajemenPassword
{
    partial class Form1
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtMasterPassword = new System.Windows.Forms.TextBox();
            this.btnAction = new System.Windows.Forms.Button();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.lnkLupaPassword = new System.Windows.Forms.LinkLabel();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(272, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(117, 16);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "\"Selamat Datang!\"";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMasterPassword
            // 
            this.txtMasterPassword.Location = new System.Drawing.Point(220, 114);
            this.txtMasterPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMasterPassword.Name = "txtMasterPassword";
            this.txtMasterPassword.Size = new System.Drawing.Size(225, 22);
            this.txtMasterPassword.TabIndex = 1;
            this.txtMasterPassword.UseSystemPasswordChar = true;
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(220, 271);
            this.btnAction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(225, 31);
            this.btnAction.TabIndex = 2;
            this.btnAction.Text = "Masuk";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(473, 114);
            this.chkShowPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(120, 20);
            this.chkShowPassword.TabIndex = 3;
            this.chkShowPassword.Text = "Lihat Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // lnkLupaPassword
            // 
            this.lnkLupaPassword.AutoSize = true;
            this.lnkLupaPassword.Location = new System.Drawing.Point(272, 327);
            this.lnkLupaPassword.Name = "lnkLupaPassword";
            this.lnkLupaPassword.Size = new System.Drawing.Size(107, 16);
            this.lnkLupaPassword.TabIndex = 4;
            this.lnkLupaPassword.TabStop = true;
            this.lnkLupaPassword.Text = "Lupa Password?";
            this.lnkLupaPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLupaPassword_LinkClicked);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(220, 68);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(225, 22);
            this.txtUsername.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(217, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(218, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "password";
            // 
            // lnkRegister
            // 
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.Location = new System.Drawing.Point(218, 166);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(250, 20);
            this.lnkRegister.TabIndex = 12;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "Belum punya akun? Daftar di sini";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.lnkRegister);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lnkLupaPassword);
            this.Controls.Add(this.chkShowPassword);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.txtMasterPassword);
            this.Controls.Add(this.lblStatus);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtMasterPassword;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.LinkLabel lnkLupaPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkRegister;
    }
}

