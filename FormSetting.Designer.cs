namespace Project_KPL_ManajemenPassword
{
    partial class FormSetting
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassLama = new System.Windows.Forms.TextBox();
            this.txtPassBaru = new System.Windows.Forms.TextBox();
            this.txtKonfirmasi = new System.Windows.Forms.TextBox();
            this.btnUpdateFormSetting = new System.Windows.Forms.Button();
            this.btnBatalFormSetting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Master Password Lama";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Master Password Baru";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(64, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(209, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Konfirmasi Password Baru";
            // 
            // txtPassLama
            // 
            this.txtPassLama.Location = new System.Drawing.Point(68, 70);
            this.txtPassLama.Name = "txtPassLama";
            this.txtPassLama.Size = new System.Drawing.Size(183, 22);
            this.txtPassLama.TabIndex = 3;
            this.txtPassLama.UseSystemPasswordChar = true;
            this.txtPassLama.TextChanged += new System.EventHandler(this.txtPassLama_TextChanged);
            // 
            // txtPassBaru
            // 
            this.txtPassBaru.Location = new System.Drawing.Point(68, 144);
            this.txtPassBaru.Name = "txtPassBaru";
            this.txtPassBaru.Size = new System.Drawing.Size(183, 22);
            this.txtPassBaru.TabIndex = 4;
            this.txtPassBaru.UseSystemPasswordChar = true;
            // 
            // txtKonfirmasi
            // 
            this.txtKonfirmasi.Location = new System.Drawing.Point(68, 214);
            this.txtKonfirmasi.Name = "txtKonfirmasi";
            this.txtKonfirmasi.Size = new System.Drawing.Size(195, 22);
            this.txtKonfirmasi.TabIndex = 5;
            this.txtKonfirmasi.UseSystemPasswordChar = true;
            // 
            // btnUpdateFormSetting
            // 
            this.btnUpdateFormSetting.Location = new System.Drawing.Point(180, 258);
            this.btnUpdateFormSetting.Name = "btnUpdateFormSetting";
            this.btnUpdateFormSetting.Size = new System.Drawing.Size(93, 42);
            this.btnUpdateFormSetting.TabIndex = 6;
            this.btnUpdateFormSetting.Text = "Update Password";
            this.btnUpdateFormSetting.UseVisualStyleBackColor = true;
            this.btnUpdateFormSetting.Click += new System.EventHandler(this.btnUpdateFormSetting_Click);
            // 
            // btnBatalFormSetting
            // 
            this.btnBatalFormSetting.Location = new System.Drawing.Point(68, 258);
            this.btnBatalFormSetting.Name = "btnBatalFormSetting";
            this.btnBatalFormSetting.Size = new System.Drawing.Size(91, 42);
            this.btnBatalFormSetting.TabIndex = 7;
            this.btnBatalFormSetting.Text = "Batal";
            this.btnBatalFormSetting.UseVisualStyleBackColor = true;
            this.btnBatalFormSetting.Click += new System.EventHandler(this.btnBatalFormSetting_Click);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 312);
            this.Controls.Add(this.btnBatalFormSetting);
            this.Controls.Add(this.btnUpdateFormSetting);
            this.Controls.Add(this.txtKonfirmasi);
            this.Controls.Add(this.txtPassBaru);
            this.Controls.Add(this.txtPassLama);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormSetting";
            this.Load += new System.EventHandler(this.FormSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassLama;
        private System.Windows.Forms.TextBox txtPassBaru;
        private System.Windows.Forms.TextBox txtKonfirmasi;
        private System.Windows.Forms.Button btnUpdateFormSetting;
        private System.Windows.Forms.Button btnBatalFormSetting;
    }
}