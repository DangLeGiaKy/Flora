namespace test.GUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtMaSP = new System.Windows.Forms.TextBox();
            this.txtTenSP = new System.Windows.Forms.TextBox();
            this.txtGiaBan = new System.Windows.Forms.TextBox();
            this.txtTonKho = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSoLuongMua = new System.Windows.Forms.TextBox();
            this.btnAddToShoppingCart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvShoppingCart = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnThemKhachHang = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cboKhachHang = new System.Windows.Forms.ComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dgvListSP = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnXemLichSu = new System.Windows.Forms.Button();
            this.btnXemHoaDon = new System.Windows.Forms.Button();
            this.btnInHoaDon = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnTinhTienThoi = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTienKhachDua = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTienThoi = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTongHoaDon = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShoppingCart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListSP)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tìm sản phẩm";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtMaSP
            // 
            this.txtMaSP.Location = new System.Drawing.Point(99, 18);
            this.txtMaSP.Name = "txtMaSP";
            this.txtMaSP.Size = new System.Drawing.Size(103, 31);
            this.txtMaSP.TabIndex = 4;
            this.txtMaSP.TextChanged += new System.EventHandler(this.txtMaSP_TextChanged);
            // 
            // txtTenSP
            // 
            this.txtTenSP.Location = new System.Drawing.Point(99, 46);
            this.txtTenSP.Name = "txtTenSP";
            this.txtTenSP.Size = new System.Drawing.Size(102, 31);
            this.txtTenSP.TabIndex = 5;
            this.txtTenSP.TextChanged += new System.EventHandler(this.txtTenSP_TextChanged);
            // 
            // txtGiaBan
            // 
            this.txtGiaBan.Location = new System.Drawing.Point(275, 18);
            this.txtGiaBan.Name = "txtGiaBan";
            this.txtGiaBan.Size = new System.Drawing.Size(103, 31);
            this.txtGiaBan.TabIndex = 6;
            this.txtGiaBan.TextChanged += new System.EventHandler(this.txtGiaBan_TextChanged);
            // 
            // txtTonKho
            // 
            this.txtTonKho.Location = new System.Drawing.Point(275, 47);
            this.txtTonKho.Name = "txtTonKho";
            this.txtTonKho.Size = new System.Drawing.Size(102, 31);
            this.txtTonKho.TabIndex = 7;
            this.txtTonKho.TextChanged += new System.EventHandler(this.txtTonKho_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mã sản phẩm";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 24);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tên sản phẩm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(217, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 24);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tồn kho";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(529, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 24);
            this.label6.TabIndex = 13;
            this.label6.Text = "Số lượng khách mua";
            // 
            // txtSoLuongMua
            // 
            this.txtSoLuongMua.Location = new System.Drawing.Point(655, 145);
            this.txtSoLuongMua.Name = "txtSoLuongMua";
            this.txtSoLuongMua.Size = new System.Drawing.Size(131, 31);
            this.txtSoLuongMua.TabIndex = 12;
            this.txtSoLuongMua.TextChanged += new System.EventHandler(this.txtSoLuongMua_TextChanged);
            // 
            // btnAddToShoppingCart
            // 
            this.btnAddToShoppingCart.Location = new System.Drawing.Point(792, 145);
            this.btnAddToShoppingCart.Name = "btnAddToShoppingCart";
            this.btnAddToShoppingCart.Size = new System.Drawing.Size(79, 22);
            this.btnAddToShoppingCart.TabIndex = 14;
            this.btnAddToShoppingCart.Text = "Thêm ";
            this.btnAddToShoppingCart.UseVisualStyleBackColor = true;
            this.btnAddToShoppingCart.Click += new System.EventHandler(this.btnAddToShoppingCart_Click);
            this.btnAddToShoppingCart.Paint += new System.Windows.Forms.PaintEventHandler(this.btnAddToShoppingCart_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvShoppingCart);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(59, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 136);
            this.panel1.TabIndex = 17;
            // 
            // dgvShoppingCart
            // 
            this.dgvShoppingCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShoppingCart.Location = new System.Drawing.Point(13, 12);
            this.dgvShoppingCart.Name = "dgvShoppingCart";
            this.dgvShoppingCart.RowHeadersWidth = 51;
            this.dgvShoppingCart.RowTemplate.Height = 24;
            this.dgvShoppingCart.Size = new System.Drawing.Size(781, 121);
            this.dgvShoppingCart.TabIndex = 16;
            this.dgvShoppingCart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvShoppingCart_CellContentClick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, -3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "Giỏ hàng";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(421, 41);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(101, 22);
            this.btnRemove.TabIndex = 18;
            this.btnRemove.Text = "Xóa";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            this.btnRemove.Paint += new System.Windows.Forms.PaintEventHandler(this.btnRemove_Paint);
            // 
            // btnThemKhachHang
            // 
            this.btnThemKhachHang.Location = new System.Drawing.Point(421, 69);
            this.btnThemKhachHang.Name = "btnThemKhachHang";
            this.btnThemKhachHang.Size = new System.Drawing.Size(101, 40);
            this.btnThemKhachHang.TabIndex = 19;
            this.btnThemKhachHang.Text = "Thêm khách hàng";
            this.btnThemKhachHang.UseVisualStyleBackColor = true;
            this.btnThemKhachHang.Click += new System.EventHandler(this.btnThemKhachHang_Click);
            this.btnThemKhachHang.Paint += new System.Windows.Forms.PaintEventHandler(this.btnThemKhachHang_Paint);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(234, 25);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(126, 24);
            this.label15.TabIndex = 71;
            this.label15.Text = "Số điện thoại";
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Location = new System.Drawing.Point(317, 22);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(170, 31);
            this.txtSoDienThoai.TabIndex = 70;
            this.txtSoDienThoai.TextChanged += new System.EventHandler(this.txtSoDienThoai_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(19, 25);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(119, 24);
            this.label16.TabIndex = 69;
            this.label16.Text = "Khách hàng";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // cboKhachHang
            // 
            this.cboKhachHang.FormattingEnabled = true;
            this.cboKhachHang.Location = new System.Drawing.Point(96, 22);
            this.cboKhachHang.Margin = new System.Windows.Forms.Padding(2);
            this.cboKhachHang.Name = "cboKhachHang";
            this.cboKhachHang.Size = new System.Drawing.Size(116, 32);
            this.cboKhachHang.TabIndex = 72;
            this.cboKhachHang.SelectedIndexChanged += new System.EventHandler(this.cboKhachHang_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(104, 6);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 31);
            this.txtSearch.TabIndex = 73;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // dgvListSP
            // 
            this.dgvListSP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListSP.Location = new System.Drawing.Point(13, 24);
            this.dgvListSP.Name = "dgvListSP";
            this.dgvListSP.RowHeadersWidth = 51;
            this.dgvListSP.RowTemplate.Height = 24;
            this.dgvListSP.Size = new System.Drawing.Size(347, 92);
            this.dgvListSP.TabIndex = 3;
            this.dgvListSP.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListSP_CellContentClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dgvListSP);
            this.groupBox1.Location = new System.Drawing.Point(528, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 124);
            this.groupBox1.TabIndex = 78;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sản phẩm";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, -18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "Giá bản";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtGiaBan);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTonKho);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtTenSP);
            this.groupBox2.Controls.Add(this.txtMaSP);
            this.groupBox2.Location = new System.Drawing.Point(26, 33);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 76);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông tin sản phẩm";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(221, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 24);
            this.label7.TabIndex = 12;
            this.label7.Text = "Giá bán";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboKhachHang);
            this.groupBox3.Controls.Add(this.txtSoDienThoai);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Location = new System.Drawing.Point(26, 119);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(493, 61);
            this.groupBox3.TabIndex = 80;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thông tin khách hàng";
            // 
            // btnXemLichSu
            // 
            this.btnXemLichSu.Location = new System.Drawing.Point(40, 349);
            this.btnXemLichSu.Name = "btnXemLichSu";
            this.btnXemLichSu.Size = new System.Drawing.Size(144, 28);
            this.btnXemLichSu.TabIndex = 83;
            this.btnXemLichSu.Text = "Xem lịch sử ";
            this.btnXemLichSu.UseVisualStyleBackColor = true;
            this.btnXemLichSu.Click += new System.EventHandler(this.btnXemLichSu_Click_1);
            // 
            // btnXemHoaDon
            // 
            this.btnXemHoaDon.Location = new System.Drawing.Point(190, 349);
            this.btnXemHoaDon.Name = "btnXemHoaDon";
            this.btnXemHoaDon.Size = new System.Drawing.Size(144, 28);
            this.btnXemHoaDon.TabIndex = 82;
            this.btnXemHoaDon.Text = "Xem chi tiết hóa đơn";
            this.btnXemHoaDon.UseVisualStyleBackColor = true;
            this.btnXemHoaDon.Click += new System.EventHandler(this.btnXemHoaDon_Click_1);
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.Location = new System.Drawing.Point(317, 21);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(84, 28);
            this.btnInHoaDon.TabIndex = 75;
            this.btnInHoaDon.Text = "In hóa đơn";
            this.btnInHoaDon.UseVisualStyleBackColor = true;
            this.btnInHoaDon.Click += new System.EventHandler(this.btnInHoaDon_Click_1);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.Location = new System.Drawing.Point(778, 372);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(96, 29);
            this.btnThanhToan.TabIndex = 81;
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = true;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click_1);
            // 
            // btnTinhTienThoi
            // 
            this.btnTinhTienThoi.Location = new System.Drawing.Point(647, 44);
            this.btnTinhTienThoi.Name = "btnTinhTienThoi";
            this.btnTinhTienThoi.Size = new System.Drawing.Size(96, 28);
            this.btnTinhTienThoi.TabIndex = 73;
            this.btnTinhTienThoi.Text = "Tính tiền thối";
            this.btnTinhTienThoi.UseVisualStyleBackColor = true;
            this.btnTinhTienThoi.Click += new System.EventHandler(this.btnTinhTienThoi_Click_1);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(459, 348);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(149, 24);
            this.label12.TabIndex = 80;
            this.label12.Text = "Tiền khách đưa";
            // 
            // txtTienKhachDua
            // 
            this.txtTienKhachDua.Location = new System.Drawing.Point(557, 345);
            this.txtTienKhachDua.Name = "txtTienKhachDua";
            this.txtTienKhachDua.Size = new System.Drawing.Size(88, 31);
            this.txtTienKhachDua.TabIndex = 79;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(673, 348);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 24);
            this.label11.TabIndex = 78;
            this.label11.Text = "Tiền thối lại";
            // 
            // txtTienThoi
            // 
            this.txtTienThoi.Location = new System.Drawing.Point(752, 344);
            this.txtTienThoi.Name = "txtTienThoi";
            this.txtTienThoi.Size = new System.Drawing.Size(122, 31);
            this.txtTienThoi.TabIndex = 77;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(471, 379);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 24);
            this.label10.TabIndex = 76;
            this.label10.Text = "Tổng hóa đơn";
            // 
            // txtTongHoaDon
            // 
            this.txtTongHoaDon.Location = new System.Drawing.Point(557, 376);
            this.txtTongHoaDon.Name = "txtTongHoaDon";
            this.txtTongHoaDon.Size = new System.Drawing.Size(88, 31);
            this.txtTongHoaDon.TabIndex = 74;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnTinhTienThoi);
            this.groupBox4.Controls.Add(this.btnInHoaDon);
            this.groupBox4.Location = new System.Drawing.Point(29, 328);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(872, 81);
            this.groupBox4.TabIndex = 84;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Thanh toán";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(255)))), ((int)(((byte)(219)))));
            this.ClientSize = new System.Drawing.Size(913, 421);
            this.Controls.Add(this.btnXemLichSu);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnXemHoaDon);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnThemKhachHang);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnAddToShoppingCart);
            this.Controls.Add(this.txtTienKhachDua);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtTienThoi);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtSoLuongMua);
            this.Controls.Add(this.txtTongHoaDon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Bán Hàng";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShoppingCart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListSP)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtMaSP;
        private System.Windows.Forms.TextBox txtTenSP;
        private System.Windows.Forms.TextBox txtGiaBan;
        private System.Windows.Forms.TextBox txtTonKho;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSoLuongMua;
        private System.Windows.Forms.Button btnAddToShoppingCart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnThemKhachHang;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboKhachHang;
        private System.Windows.Forms.DataGridView dgvShoppingCart;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvListSP;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnXemLichSu;
        private System.Windows.Forms.Button btnXemHoaDon;
        private System.Windows.Forms.Button btnInHoaDon;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnTinhTienThoi;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTienKhachDua;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTienThoi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTongHoaDon;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}