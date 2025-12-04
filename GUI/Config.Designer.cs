namespace test.GUI
{
    partial class Config
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
            this.lblserver = new System.Windows.Forms.Label();
            this.lbldata = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.lblPass = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtdata = new System.Windows.Forms.TextBox();
            this.txtid = new System.Windows.Forms.TextBox();
            this.txtpass = new System.Windows.Forms.TextBox();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.chkWinAuth = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblserver
            // 
            this.lblserver.AutoSize = true;
            this.lblserver.Location = new System.Drawing.Point(30, 37);
            this.lblserver.Name = "lblserver";
            this.lblserver.Size = new System.Drawing.Size(47, 17);
            this.lblserver.TabIndex = 0;
            this.lblserver.Text = "Server";
            this.lblserver.Click += new System.EventHandler(this.label1_Click);
            // 
            // lbldata
            // 
            this.lbldata.AutoSize = true;
            this.lbldata.Location = new System.Drawing.Point(15, 88);
            this.lbldata.Name = "lbldata";
            this.lbldata.Size = new System.Drawing.Size(64, 17);
            this.lbldata.TabIndex = 1;
            this.lbldata.Text = "Database";
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(25, 144);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(57, 17);
            this.lblID.TabIndex = 2;
            this.lblID.Text = "User ID";
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Location = new System.Drawing.Point(15, 203);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(66, 17);
            this.lblPass.TabIndex = 3;
            this.lblPass.Text = "Password";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(96, 34);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(256, 25);
            this.txtServer.TabIndex = 4;
            // 
            // txtdata
            // 
            this.txtdata.Location = new System.Drawing.Point(96, 85);
            this.txtdata.Name = "txtdata";
            this.txtdata.Size = new System.Drawing.Size(256, 25);
            this.txtdata.TabIndex = 5;
            // 
            // txtid
            // 
            this.txtid.Location = new System.Drawing.Point(96, 138);
            this.txtid.Name = "txtid";
            this.txtid.Size = new System.Drawing.Size(256, 25);
            this.txtid.TabIndex = 6;
            // 
            // txtpass
            // 
            this.txtpass.Location = new System.Drawing.Point(96, 197);
            this.txtpass.Name = "txtpass";
            this.txtpass.PasswordChar = '●';
            this.txtpass.Size = new System.Drawing.Size(143, 25);
            this.txtpass.TabIndex = 7;
            this.txtpass.TextChanged += new System.EventHandler(this.txtpass_TextChanged);
            // 
            // chkShowPassword
            // 
            this.chkShowPassword.AutoSize = true;
            this.chkShowPassword.Location = new System.Drawing.Point(257, 199);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(93, 21);
            this.chkShowPassword.TabIndex = 8;
            this.chkShowPassword.Text = "Show pass";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // chkWinAuth
            // 
            this.chkWinAuth.AutoSize = true;
            this.chkWinAuth.Location = new System.Drawing.Point(18, 241);
            this.chkWinAuth.Name = "chkWinAuth";
            this.chkWinAuth.Size = new System.Drawing.Size(172, 21);
            this.chkWinAuth.TabIndex = 9;
            this.chkWinAuth.Text = "Windows Authentication";
            this.chkWinAuth.UseVisualStyleBackColor = true;
            this.chkWinAuth.CheckedChanged += new System.EventHandler(this.chkWinAuth_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 24);
            this.button1.TabIndex = 10;
            this.button1.Text = "CONNECT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(185, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "SQL SERVER";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblserver);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.chkWinAuth);
            this.groupBox1.Controls.Add(this.lbldata);
            this.groupBox1.Controls.Add(this.chkShowPassword);
            this.groupBox1.Controls.Add(this.txtdata);
            this.groupBox1.Controls.Add(this.txtpass);
            this.groupBox1.Controls.Add(this.lblID);
            this.groupBox1.Controls.Add(this.lblPass);
            this.groupBox1.Controls.Add(this.txtid);
            this.groupBox1.Location = new System.Drawing.Point(35, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(381, 322);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(462, 392);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Config";
            this.Text = "Cấu hình";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblserver;
        private System.Windows.Forms.Label lbldata;
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtdata;
        private System.Windows.Forms.TextBox txtid;
        private System.Windows.Forms.TextBox txtpass;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.CheckBox chkWinAuth;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}