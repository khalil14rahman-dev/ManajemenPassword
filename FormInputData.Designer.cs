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
            this.label1.Location = new System.Drawing.Point(58, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama Aplikasi";
            // 
            // txtAplikasi
            // 
            this.txtAplikasi.Location = new System.Drawing.Point(61, 49);
            this.txtAplikasi.Margin = new System.Windows.Forms.Padding(2);
            this.txtAplikasi.Name = "txtAplikasi";
            this.txtAplikasi.Size = new System.Drawing.Size(163, 20);
            this.txtAplikasi.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username/Email";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(61, 108);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(163, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 141);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(61, 166);
            this.textPassword.Margin = new System.Windows.Forms.Padding(2);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(132, 20);
            this.textPassword.TabIndex = 5;
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(210, 162);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(2);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(56, 27);
            this.btnAuto.TabIndex = 6;
            this.btnAuto.Text = "Auto Gen";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnSimpanFormInput
            // 
            this.btnSimpanFormInput.Location = new System.Drawing.Point(188, 245);
            this.btnSimpanFormInput.Margin = new System.Windows.Forms.Padding(2);
            this.btnSimpanFormInput.Name = "btnSimpanFormInput";
            this.btnSimpanFormInput.Size = new System.Drawing.Size(78, 37);
            this.btnSimpanFormInput.TabIndex = 7;
            this.btnSimpanFormInput.Text = "Simpan";
            this.btnSimpanFormInput.UseVisualStyleBackColor = true;
            this.btnSimpanFormInput.Click += new System.EventHandler(this.btnSimpanFormInput_Click);
            // 
            // btnBatalFormInput
            // 
            this.btnBatalFormInput.Location = new System.Drawing.Point(62, 245);
            this.btnBatalFormInput.Margin = new System.Windows.Forms.Padding(2);
            this.btnBatalFormInput.Name = "btnBatalFormInput";
            this.btnBatalFormInput.Size = new System.Drawing.Size(78, 37);
            this.btnBatalFormInput.TabIndex = 8;
            this.btnBatalFormInput.Text = "Batal";
            this.btnBatalFormInput.UseVisualStyleBackColor = true;
            this.btnBatalFormInput.Click += new System.EventHandler(this.btnBatalFormInput_Click);
            // 
            // lblstrength
            // 
            this.lblstrength.AutoSize = true;
            this.lblstrength.Location = new System.Drawing.Point(59, 210);
            this.lblstrength.Name = "lblstrength";
            this.lblstrength.Size = new System.Drawing.Size(59, 13);
            this.lblstrength.TabIndex = 9;
            this.lblstrength.Text = "Kekuatan :";
            this.lblstrength.Click += new System.EventHandler(this.lblstrength_Click);
            // 
            // FormInputData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 327);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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