namespace test.GUI
{
    partial class Frmlichsu
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.dgvDanhSachDonHang = new System.Windows.Forms.DataGridView();
            this.btnXemHoaDon = new System.Windows.Forms.Button();
            this.btnInHoaDon = new System.Windows.Forms.Button();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.cboSearch = new System.Windows.Forms.ComboBox();
            this.dtpNgayTao = new System.Windows.Forms.DateTimePicker();
            this.lblNgayTao = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachDonHang)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDanhSachDonHang
            // 
            this.dgvDanhSachDonHang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachDonHang.Location = new System.Drawing.Point(15, 140);
            this.dgvDanhSachDonHang.Name = "dgvDanhSachDonHang";
            this.dgvDanhSachDonHang.RowHeadersWidth = 51;
            this.dgvDanhSachDonHang.RowTemplate.Height = 24;
            this.dgvDanhSachDonHang.Size = new System.Drawing.Size(873, 137);
            this.dgvDanhSachDonHang.TabIndex = 0;
            this.dgvDanhSachDonHang.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDanhSachDonHang_CellContentClick);
            // 
            // btnXemHoaDon
            // 
            this.btnXemHoaDon.Location = new System.Drawing.Point(21, 34);
            this.btnXemHoaDon.Name = "btnXemHoaDon";
            this.btnXemHoaDon.Size = new System.Drawing.Size(184, 32);
            this.btnXemHoaDon.TabIndex = 28;
            this.btnXemHoaDon.Text = "Xem chi tiết hóa đơn";
            this.btnXemHoaDon.UseVisualStyleBackColor = true;
            this.btnXemHoaDon.Click += new System.EventHandler(this.btnXemHoaDon_Click);
            this.btnXemHoaDon.Paint += new System.Windows.Forms.PaintEventHandler(this.btnXemHoaDon_Paint);
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.Location = new System.Drawing.Point(231, 34);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(184, 32);
            this.btnInHoaDon.TabIndex = 27;
            this.btnInHoaDon.Text = "In hóa đơn";
            this.btnInHoaDon.UseVisualStyleBackColor = true;
            this.btnInHoaDon.Click += new System.EventHandler(this.btnInHoaDon_Click);
            this.btnInHoaDon.Paint += new System.Windows.Forms.PaintEventHandler(this.btnInHoaDon_Paint);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // cboSearch
            // 
            this.cboSearch.FormattingEnabled = true;
            this.cboSearch.Location = new System.Drawing.Point(94, 11);
            this.cboSearch.Margin = new System.Windows.Forms.Padding(2);
            this.cboSearch.Name = "cboSearch";
            this.cboSearch.Size = new System.Drawing.Size(264, 25);
            this.cboSearch.TabIndex = 29;
            this.cboSearch.Text = "Tìm kiếm theo mã hóa đơn";
            this.cboSearch.SelectedIndexChanged += new System.EventHandler(this.cboSearch_SelectedIndexChanged);
            // 
            // dtpNgayTao
            // 
            this.dtpNgayTao.Location = new System.Drawing.Point(94, 64);
            this.dtpNgayTao.Margin = new System.Windows.Forms.Padding(2);
            this.dtpNgayTao.Name = "dtpNgayTao";
            this.dtpNgayTao.Size = new System.Drawing.Size(264, 25);
            this.dtpNgayTao.TabIndex = 30;
            this.dtpNgayTao.ValueChanged += new System.EventHandler(this.dtpNgayTao_ValueChanged);
            // 
            // lblNgayTao
            // 
            this.lblNgayTao.AutoSize = true;
            this.lblNgayTao.Location = new System.Drawing.Point(25, 70);
            this.lblNgayTao.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNgayTao.Name = "lblNgayTao";
            this.lblNgayTao.Size = new System.Drawing.Size(65, 17);
            this.lblNgayTao.TabIndex = 31;
            this.lblNgayTao.Text = "Ngày tạo";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 32;
            this.label1.Text = "Tìm kiếm";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnInHoaDon);
            this.groupBox1.Controls.Add(this.btnXemHoaDon);
            this.groupBox1.Location = new System.Drawing.Point(441, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 81);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xem và in hóa đơn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 109);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 17);
            this.label2.TabIndex = 34;
            this.label2.Text = "Lịch sử bán hàng";
            // 
            // Frmlichsu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(900, 298);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblNgayTao);
            this.Controls.Add(this.dtpNgayTao);
            this.Controls.Add(this.cboSearch);
            this.Controls.Add(this.dgvDanhSachDonHang);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "Frmlichsu";
            this.Text = "Lịch sử bán hàng";
            this.Load += new System.EventHandler(this.Frmlichsu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachDonHang)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.DataGridView dgvDanhSachDonHang;
        private System.Windows.Forms.Button btnXemHoaDon;
        private System.Windows.Forms.Button btnInHoaDon;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.Windows.Forms.ComboBox cboSearch;
        private System.Windows.Forms.DateTimePicker dtpNgayTao;
        private System.Windows.Forms.Label lblNgayTao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
    }
}