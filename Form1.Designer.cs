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
            this.cmbSecurityQuestion = new System.Windows.Forms.ComboBox();
            this.txtSecurityAnswer = new System.Windows.Forms.TextBox();
            this.lblQuestionHint = new System.Windows.Forms.Label();
            this.lblAnswerHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(306, 88);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(141, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "\"Selamat Datang!\"";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // txtMasterPassword
            // 
            this.txtMasterPassword.Location = new System.Drawing.Point(248, 142);
            this.txtMasterPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMasterPassword.Name = "txtMasterPassword";
            this.txtMasterPassword.Size = new System.Drawing.Size(253, 26);
            this.txtMasterPassword.TabIndex = 1;
            this.txtMasterPassword.UseSystemPasswordChar = true;
            this.txtMasterPassword.TextChanged += new System.EventHandler(this.txtMasterPassword_TextChanged);
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(248, 339);
            this.btnAction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(253, 39);
            this.btnAction.TabIndex = 2;
            this.btnAction.Text = "Masuk";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(532, 143);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(143, 24);
            this.chkShowPassword.TabIndex = 3;
            this.chkShowPassword.Text = "Lihat Password";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // lnkLupaPassword
            // 
            this.lnkLupaPassword.AutoSize = true;
            this.lnkLupaPassword.Location = new System.Drawing.Point(306, 409);
            this.lnkLupaPassword.Name = "lnkLupaPassword";
            this.lnkLupaPassword.Size = new System.Drawing.Size(127, 20);
            this.lnkLupaPassword.TabIndex = 4;
            this.lnkLupaPassword.TabStop = true;
            this.lnkLupaPassword.Text = "Lupa Password?";
            this.lnkLupaPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLupaPassword_LinkClicked);
            // 
            // cmbSecurityQuestion
            // 
            this.cmbSecurityQuestion.FormattingEnabled = true;
            this.cmbSecurityQuestion.Items.AddRange(new object[] {
            "Apa nama hewan peliharaan pertama Anda?",
            "Di kota mana kedua orang tua Anda bertemu?",
            "Apa nama sekolah dasar Anda?"});
            this.cmbSecurityQuestion.Location = new System.Drawing.Point(248, 215);
            this.cmbSecurityQuestion.Name = "cmbSecurityQuestion";
            this.cmbSecurityQuestion.Size = new System.Drawing.Size(253, 28);
            this.cmbSecurityQuestion.TabIndex = 5;
            // 
            // txtSecurityAnswer
            // 
            this.txtSecurityAnswer.Location = new System.Drawing.Point(248, 284);
            this.txtSecurityAnswer.Name = "txtSecurityAnswer";
            this.txtSecurityAnswer.Size = new System.Drawing.Size(253, 26);
            this.txtSecurityAnswer.TabIndex = 6;
            // 
            // lblQuestionHint
            // 
            this.lblQuestionHint.AutoSize = true;
            this.lblQuestionHint.Location = new System.Drawing.Point(244, 192);
            this.lblQuestionHint.Name = "lblQuestionHint";
            this.lblQuestionHint.Size = new System.Drawing.Size(207, 20);
            this.lblQuestionHint.TabIndex = 7;
            this.lblQuestionHint.Text = "Pilih Pertanyaan Keamanan:";
            // 
            // lblAnswerHint
            // 
            this.lblAnswerHint.AutoSize = true;
            this.lblAnswerHint.Location = new System.Drawing.Point(244, 261);
            this.lblAnswerHint.Name = "lblAnswerHint";
            this.lblAnswerHint.Size = new System.Drawing.Size(155, 20);
            this.lblAnswerHint.TabIndex = 8;
            this.lblAnswerHint.Text = "Jawaban Pemulihan:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblAnswerHint);
            this.Controls.Add(this.lblQuestionHint);
            this.Controls.Add(this.txtSecurityAnswer);
            this.Controls.Add(this.cmbSecurityQuestion);
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
        private System.Windows.Forms.ComboBox cmbSecurityQuestion;
        private System.Windows.Forms.TextBox txtSecurityAnswer;
        private System.Windows.Forms.Label lblQuestionHint;
        private System.Windows.Forms.Label lblAnswerHint;
    }
}

