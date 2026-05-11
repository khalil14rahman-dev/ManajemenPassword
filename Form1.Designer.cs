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
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(272, 70);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(117, 16);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "\"Selamat Datang!\"";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
            // 
            // txtMasterPassword
            // 
            this.txtMasterPassword.Location = new System.Drawing.Point(220, 114);
            this.txtMasterPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMasterPassword.Name = "txtMasterPassword";
            this.txtMasterPassword.Size = new System.Drawing.Size(225, 22);
            this.txtMasterPassword.TabIndex = 1;
            this.txtMasterPassword.UseSystemPasswordChar = true;
            this.txtMasterPassword.TextChanged += new System.EventHandler(this.txtMasterPassword_TextChanged);
            // 
            // btnAction
            // 
            this.btnAction.Location = new System.Drawing.Point(220, 185);
            this.btnAction.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(225, 31);
            this.btnAction.TabIndex = 2;
            this.btnAction.Text = "Masuk";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
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
    }
}

