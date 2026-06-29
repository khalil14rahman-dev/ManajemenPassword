namespace Project_KPL_ManajemenPassword
{
    partial class FormForgotPassword
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
            this.btnVerify = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnCari = new System.Windows.Forms.Button();
            this.flowLayoutPanelRecovery = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(41, 305);
            this.btnVerify.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(224, 30);
            this.btnVerify.TabIndex = 2;
            this.btnVerify.Text = "Verifikasi Jawab";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Masukkan usernamee anda";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(41, 69);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(215, 22);
            this.txtUsername.TabIndex = 4;
            // 
            // btnCari
            // 
            this.btnCari.Location = new System.Drawing.Point(313, 65);
            this.btnCari.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(75, 30);
            this.btnCari.TabIndex = 6;
            this.btnCari.Text = "Cari";
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // flowLayoutPanelRecovery
            // 
            this.flowLayoutPanelRecovery.Location = new System.Drawing.Point(41, 100);
            this.flowLayoutPanelRecovery.Name = "flowLayoutPanelRecovery";
            this.flowLayoutPanelRecovery.Size = new System.Drawing.Size(548, 183);
            this.flowLayoutPanelRecovery.TabIndex = 7;
            // 
            // FormForgotPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 361);
            this.Controls.Add(this.flowLayoutPanelRecovery);
            this.Controls.Add(this.btnCari);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnVerify);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormForgotPassword";
            this.Text = "FormForgotPassword";
            this.Load += new System.EventHandler(this.FormForgotPassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnCari;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelRecovery;
    }
}