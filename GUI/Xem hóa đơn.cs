using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using test.DAL;

namespace test.GUI
{
    public partial class frmXemHoaDon : Form
    {
        private string connectionString = Connection.GetConnectionString();
        //private string connectionString = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private SqlConnection conn;
        private string maHoaDon;

        // Constructor nhận MaHoaDon
        public frmXemHoaDon(string maHoaDon)
        {
            InitializeComponent();
            this.maHoaDon = maHoaDon;
            conn = new SqlConnection(connectionString);

            LoadHoaDonDetail();
        }

        private void LoadHoaDonDetail()
        {
            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();

                // Load thông tin hóa đơn
                string queryHD = @"SELECT 
                    h.MaHoaDon,
                    h.NgayLap,
                    k.HoTen AS TenKhachHang,
                    k.SoDienThoai,
                    k.DiaChi,
                    u.HoTen AS TenNhanVien,
                    h.TongTien,
                    h.TongGiaNhap,
                    h.LoiNhuan,
                    h.TienKhachDua,
                    h.TienThoiLai,
                    h.GhiChu
                FROM HoaDon h
                LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                LEFT JOIN [User] u ON h.MaNhanVien = u.MaUser
                WHERE h.MaHoaDon = @MaHoaDon";

                SqlCommand cmdHD = new SqlCommand(queryHD, conn);
                cmdHD.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataReader reader = cmdHD.ExecuteReader();

                if (reader.Read())
                {
                    // Hiển thị thông tin hóa đơn
                    lblMaHoaDon.Text = reader["MaHoaDon"].ToString();
                    lblNgayLap.Text = Convert.ToDateTime(reader["NgayLap"]).ToString("dd/MM/yyyy HH:mm:ss");
                    lblTenKhachHang.Text = reader["TenKhachHang"].ToString();
                    lblSoDienThoai.Text = reader["SoDienThoai"].ToString();
                    lblDiaChi.Text = reader["DiaChi"].ToString();
                    lblNhanVien.Text = reader["TenNhanVien"].ToString();
                    lblTongTien.Text = Convert.ToDecimal(reader["TongTien"]).ToString("N0") + " VNĐ";
                    // BỎ lblTongGiaNhap và lblLoiNhuan
                    lblTienKhachDua.Text = Convert.ToDecimal(reader["TienKhachDua"]).ToString("N0") + " VNĐ";
                    lblTienThoiLai.Text = Convert.ToDecimal(reader["TienThoiLai"]).ToString("N0") + " VNĐ";
                    lblGhiChu.Text = reader["GhiChu"].ToString();
                }
                reader.Close();

                // Load chi tiết hóa đơn
                string queryCT = @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY ct.MaChiTiet) AS STT,
                    k.TenSanPham,
                    ct.SoLuong,
                    ct.GiaBan,
                    ct.ThanhTien,
                    ct.GiaNhap,
                    ct.TongGiaNhap,
                    ct.LoiNhuan
                FROM ChiTietHoaDon ct
                INNER JOIN Kho k ON ct.MaSanPham = k.MaSanPham
                WHERE ct.MaHoaDon = @MaHoaDon";

                SqlCommand cmdCT = new SqlCommand(queryCT, conn);
                cmdCT.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmdCT);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvChiTiet.DataSource = dt;

                // Định dạng DataGridView
                ConfigureDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void ConfigureDataGridView()
        {
            if (dgvChiTiet.Columns.Count > 0)
            {
                // Cấu hình các cột
                dgvChiTiet.Columns["STT"].HeaderText = "STT";
                dgvChiTiet.Columns["STT"].Width = 60;
                dgvChiTiet.Columns["STT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvChiTiet.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
                dgvChiTiet.Columns["TenSanPham"].Width = 400;
                dgvChiTiet.Columns["TenSanPham"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số Lượng";
                dgvChiTiet.Columns["SoLuong"].Width = 100;
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgvChiTiet.Columns["GiaBan"].HeaderText = "Đơn Giá";
                dgvChiTiet.Columns["GiaBan"].Width = 190;
                dgvChiTiet.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành Tiền";
                dgvChiTiet.Columns["ThanhTien"].Width = 200;
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // ẨN CÁC CỘT: Giá Nhập, Tổng Giá Nhập, Lợi Nhuận
                dgvChiTiet.Columns["GiaNhap"].Visible = false;
                dgvChiTiet.Columns["TongGiaNhap"].Visible = false;
                dgvChiTiet.Columns["LoiNhuan"].Visible = false;

                // Cấu hình chung
                dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvChiTiet.AllowUserToAddRows = false;
                dgvChiTiet.ReadOnly = true;
                dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvChiTiet.RowHeadersVisible = false; // ẨN CỘT ĐẦU TIÊN (Row Headers)
                dgvChiTiet.EnableHeadersVisualStyles = false;

                // Style cho header
                dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
                dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
                dgvChiTiet.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvChiTiet.ColumnHeadersHeight = 35;

                // Style cho các dòng
                dgvChiTiet.RowTemplate.Height = 30;
                dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
                dgvChiTiet.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(135, 206, 250);
                dgvChiTiet.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ✅✅✅ NÚT IN HÓA ĐƠN - XUẤT PDF ✅✅✅
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            // GỌI CLASS HoaDonPDFExporter ĐỂ XUẤT PDF
            HoaDonPDFExporter.ExportToPDF(maHoaDon);
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}