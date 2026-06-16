namespace Project_KPL_ManajemenPassword
{
    partial class FormInputData
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAplikasi = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnSimpanFormInput = new System.Windows.Forms.Button();
            this.btnBatalFormInput = new System.Windows.Forms.Button();
            this.lblstrength = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama Aplikasi";
            // 
            // txtAplikasi
            // 
            this.txtAplikasi.Location = new System.Drawing.Point(81, 60);
            this.txtAplikasi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAplikasi.Name = "txtAplikasi";
            this.txtAplikasi.Size = new System.Drawing.Size(216, 22);
            this.txtAplikasi.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username/Email";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(81, 133);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(216, 22);
            this.txtUsername.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(81, 204);
            this.textPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(175, 22);
            this.textPassword.TabIndex = 5;
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(280, 199);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(75, 33);
            this.btnAuto.TabIndex = 6;
            this.btnAuto.Text = "Auto Gen";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnSimpanFormInput
            // 
            this.btnSimpanFormInput.Location = new System.Drawing.Point(251, 302);
            this.btnSimpanFormInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSimpanFormInput.Name = "btnSimpanFormInput";
            this.btnSimpanFormInput.Size = new System.Drawing.Size(104, 46);
            this.btnSimpanFormInput.TabIndex = 7;
            this.btnSimpanFormInput.Text = "Simpan";
            this.btnSimpanFormInput.UseVisualStyleBackColor = true;
            this.btnSimpanFormInput.Click += new System.EventHandler(this.btnSimpanFormInput_Click);
            // 
            // btnBatalFormInput
            // 
            this.btnBatalFormInput.Location = new System.Drawing.Point(83, 302);
            this.btnBatalFormInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBatalFormInput.Name = "btnBatalFormInput";
            this.btnBatalFormInput.Size = new System.Drawing.Size(104, 46);
            this.btnBatalFormInput.TabIndex = 8;
            this.btnBatalFormInput.Text = "Batal";
            this.btnBatalFormInput.UseVisualStyleBackColor = true;
            this.btnBatalFormInput.Click += new System.EventHandler(this.btnBatalFormInput_Click);
            // 
            // lblstrength
            // 
            this.lblstrength.AutoSize = true;
            this.lblstrength.Location = new System.Drawing.Point(79, 258);
            this.lblstrength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblstrength.Name = "lblstrength";
            this.lblstrength.Size = new System.Drawing.Size(69, 16);
            this.lblstrength.TabIndex = 9;
            this.lblstrength.Text = "Kekuatan :";
            // 
            // FormInputData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 402);
            this.Controls.Add(this.lblstrength);
            this.Controls.Add(this.btnBatalFormInput);
            this.Controls.Add(this.btnSimpanFormInput);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAplikasi);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormInputData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormInputData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAplikasi;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnSimpanFormInput;
        private System.Windows.Forms.Button btnBatalFormInput;
        private System.Windows.Forms.Label lblstrength;
    }
}