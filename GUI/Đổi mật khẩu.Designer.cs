namespace test.GUI
{
    partial class Form10
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
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtNewPassXn = new System.Windows.Forms.TextBox();
            this.txtNewPass = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlShowOldPass = new System.Windows.Forms.Panel();
            this.pnlShowNewPass = new System.Windows.Forms.Panel();
            this.pnlShowConfirmPass = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(772, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 37);
            this.label3.TabIndex = 7;
            this.label3.Text = "Đổi mật khẩu";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::test.Properties.Resources._3cb45bde_8b6f_4ed9_b4e1_117690c56c9f;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(78, 134);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 419);
            this.panel1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(712, 156);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 25);
            this.label6.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(688, 145);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 25);
            this.label5.TabIndex = 15;
            this.label5.Text = "Tên đăng nhập";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(688, 228);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 25);
            this.label4.TabIndex = 14;
            this.label4.Text = "Mật khẩu cũ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Violet;
            this.label2.Image = global::test.Properties.Resources.z7198534940934_0fee76794b688cb80804cf38e3da8eaa;
            this.label2.Location = new System.Drawing.Point(702, 295);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 25);
            this.label2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.label1.Image = global::test.Properties.Resources.z7198534940934_0fee76794b688cb80804cf38e3da8eaa;
            this.label1.Location = new System.Drawing.Point(702, 145);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 12;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(693, 258);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.Size = new System.Drawing.Size(325, 31);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(693, 175);
            this.txtAccount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(325, 31);
            this.txtAccount.TabIndex = 0;
            this.txtAccount.TextChanged += new System.EventHandler(this.txtdangnhap_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(712, 375);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 25);
            this.label7.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(688, 320);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 25);
            this.label8.TabIndex = 22;
            this.label8.Text = "Mật khẩu mới";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(688, 416);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(237, 25);
            this.label9.TabIndex = 21;
            this.label9.Text = "Xác nhận mật khẩu mới";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Violet;
            this.label10.Image = global::test.Properties.Resources.z7198534940934_0fee76794b688cb80804cf38e3da8eaa;
            this.label10.Location = new System.Drawing.Point(702, 514);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 25);
            this.label10.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.label11.Image = global::test.Properties.Resources.z7198534940934_0fee76794b688cb80804cf38e3da8eaa;
            this.label11.Location = new System.Drawing.Point(702, 364);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 25);
            this.label11.TabIndex = 19;
            // 
            // txtNewPassXn
            // 
            this.txtNewPassXn.Location = new System.Drawing.Point(693, 445);
            this.txtNewPassXn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNewPassXn.Name = "txtNewPassXn";
            this.txtNewPassXn.PasswordChar = '●';
            this.txtNewPassXn.Size = new System.Drawing.Size(325, 31);
            this.txtNewPassXn.TabIndex = 3;
            // 
            // txtNewPass
            // 
            this.txtNewPass.Location = new System.Drawing.Point(693, 355);
            this.txtNewPass.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNewPass.Name = "txtNewPass";
            this.txtNewPass.Size = new System.Drawing.Size(325, 31);
            this.txtNewPass.TabIndex = 2;
            this.txtNewPass.TextChanged += new System.EventHandler(this.txtNewPass_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(764, 517);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(168, 36);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Lưu mật khẩu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            this.btnSave.Paint += new System.Windows.Forms.PaintEventHandler(this.btnSave_Paint);
            // 
            // pnlShowOldPass
            // 
            this.pnlShowOldPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShowOldPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlShowOldPass.Location = new System.Drawing.Point(1035, 258);
            this.pnlShowOldPass.Name = "pnlShowOldPass";
            this.pnlShowOldPass.Size = new System.Drawing.Size(57, 31);
            this.pnlShowOldPass.TabIndex = 24;
            this.pnlShowOldPass.Click += new System.EventHandler(this.pnlShowOldPass_Click);
            this.pnlShowOldPass.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlShowOldPass_Paint);
            this.pnlShowOldPass.MouseEnter += new System.EventHandler(this.pnlShowOldPass_MouseEnter);
            this.pnlShowOldPass.MouseLeave += new System.EventHandler(this.pnlShowOldPass_MouseLeave);
            // 
            // pnlShowNewPass
            // 
            this.pnlShowNewPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShowNewPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlShowNewPass.Location = new System.Drawing.Point(1035, 355);
            this.pnlShowNewPass.Name = "pnlShowNewPass";
            this.pnlShowNewPass.Size = new System.Drawing.Size(57, 33);
            this.pnlShowNewPass.TabIndex = 25;
            this.pnlShowNewPass.Click += new System.EventHandler(this.pnlShowNewPass_Click);
            this.pnlShowNewPass.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlShowNewPass_Paint);
            this.pnlShowNewPass.MouseEnter += new System.EventHandler(this.pnlShowNewPass_MouseEnter);
            this.pnlShowNewPass.MouseLeave += new System.EventHandler(this.pnlShowNewPass_MouseLeave);
            // 
            // pnlShowConfirmPass
            // 
            this.pnlShowConfirmPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlShowConfirmPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlShowConfirmPass.Location = new System.Drawing.Point(1035, 445);
            this.pnlShowConfirmPass.Name = "pnlShowConfirmPass";
            this.pnlShowConfirmPass.Size = new System.Drawing.Size(57, 30);
            this.pnlShowConfirmPass.TabIndex = 26;
            this.pnlShowConfirmPass.Click += new System.EventHandler(this.pnlShowConfirmPass_Click);
            this.pnlShowConfirmPass.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlShowConfirmPass_Paint);
            this.pnlShowConfirmPass.MouseEnter += new System.EventHandler(this.pnlShowConfirmPass_MouseEnter);
            this.pnlShowConfirmPass.MouseLeave += new System.EventHandler(this.pnlShowConfirmPass_MouseLeave);
            // 
            // Form10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(1200, 703);
            this.Controls.Add(this.pnlShowConfirmPass);
            this.Controls.Add(this.pnlShowNewPass);
            this.Controls.Add(this.pnlShowOldPass);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtNewPassXn);
            this.Controls.Add(this.txtNewPass);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form10";
            this.Text = "Đổi mật khẩu";
            this.Load += new System.EventHandler(this.Form10_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtNewPassXn;
        private System.Windows.Forms.TextBox txtNewPass;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlShowOldPass;
        private System.Windows.Forms.Panel pnlShowNewPass;
        private System.Windows.Forms.Panel pnlShowConfirmPass;
    }
}