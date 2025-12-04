using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.DAL;

namespace test.GUI
{
    public partial class Form9 : Form
    {
        private string connectionString = Connection.GetConnectionString();
        //private readonly string connectionString ="Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private SqlConnection conn;
        private DataTable currentReportData;

        public Form9()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            InitializeForm();
        }

        // ================================================================
        //  KHỞI TẠO FORM
        // ================================================================
        private void InitializeForm()
        {
            LoadLoaiBaoCao();
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;

            ConfigureGrid();
            LockExportButtons();
            AddEventHandlers();
        }

        private void LoadLoaiBaoCao()
        {
            cboLoaiBaoCao.Items.Clear();
            cboLoaiBaoCao.Items.Add("Báo cáo theo doanh thu và lợi nhuận");
            cboLoaiBaoCao.Items.Add("Báo cáo theo sản phẩm đã bán ra");
            
            cboLoaiBaoCao.Items.Add("Báo cáo tồn kho");
            cboLoaiBaoCao.SelectedIndex = 0;
        }

        private void ConfigureGrid()
        {
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.RowHeadersVisible = false;

            dgvChiTiet.EnableHeadersVisualStyles = false;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvChiTiet.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvChiTiet.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            dgvChiTiet.ColumnHeadersHeight = 35;

            dgvChiTiet.RowTemplate.Height = 30;
            dgvChiTiet.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255);
        }

        private void AddEventHandlers()
        {
            cboLoaiBaoCao.SelectedIndexChanged += (s, e) => ResetReportData();
            dtpTuNgay.ValueChanged += (s, e) => ResetReportData();
            dtpDenNgay.ValueChanged += (s, e) => ResetReportData();
        }

        // ================================================================
        //  KHÓA / MỞ NÚT XUẤT & IN
        // ================================================================
        private void LockExportButtons()
        {
            btnXuatBaoCao.Enabled = false;
            btnInBaoCao.Enabled = false;

            btnXuatBaoCao.BackColor = Color.LightGray;
            btnXuatBaoCao.ForeColor = Color.DarkGray;

            btnInBaoCao.BackColor = Color.LightGray;
            btnInBaoCao.ForeColor = Color.DarkGray;
        }

        private void UnlockExportButtons()
        {
            btnXuatBaoCao.Enabled = true;
            btnInBaoCao.Enabled = true;

            btnXuatBaoCao.BackColor = Color.FromArgb(40, 167, 69); // xanh đậm
            btnXuatBaoCao.ForeColor = Color.White;

            btnInBaoCao.BackColor = Color.FromArgb(0, 123, 255); // xanh dương đậm
            btnInBaoCao.ForeColor = Color.White;
        }

        private void ResetReportData()
        {
            currentReportData = null;
            dgvChiTiet.DataSource = null;
            LockExportButtons();
        }

        // ================================================================
        //  NÚT XEM BÁO CÁO
        // ================================================================
        private void btnXemBaoCao_Click(object sender, EventArgs e)
        {
            if (cboLoaiBaoCao.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại báo cáo!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string loai = cboLoaiBaoCao.SelectedItem.ToString();

                switch (loai)
                {
                    case "Báo cáo theo doanh thu và lợi nhuận":
                        LoadBaoCaoDoanhThuLoiNhuan(tuNgay, denNgay);
                        break;

                    case "Báo cáo theo sản phẩm đã bán ra":
                        LoadBaoCaoSanPhamBanRa(tuNgay, denNgay);
                        break;

                    case "Báo cáo nhập hàng":
                        LoadBaoCaoNhapHang(tuNgay, denNgay);
                        break;

                    case "Báo cáo tồn kho":
                        LoadBaoCaoTonKho();
                        break;
                }

                if (currentReportData != null && currentReportData.Rows.Count > 0)
                    UnlockExportButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LockExportButtons();
            }
        }

        // ================================================================
        //  CÁC LOẠI BÁO CÁO
        // ================================================================

        private void LoadBaoCaoDoanhThuLoiNhuan(DateTime tu, DateTime den)
        {
            try
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        CAST(NgayLap AS DATE) AS Ngay,
                        COUNT(MaHoaDon) AS SoHoaDon,
                        SUM(TongTien) AS DoanhThu,
                        SUM(TongGiaNhap) AS TongChiPhi,
                        SUM(LoiNhuan) AS LoiNhuan,
                        CASE 
                            WHEN SUM(TongGiaNhap) > 0 
                            THEN ROUND((SUM(LoiNhuan) / SUM(TongGiaNhap) * 100), 2)
                            ELSE 0 
                        END AS TyLeLoiNhuan
                    FROM HoaDon
                    WHERE TrangThai = N'Đã thanh toán'
                        AND NgayLap >= @tu
                        AND NgayLap <= @den
                    GROUP BY CAST(NgayLap AS DATE)
                    ORDER BY Ngay DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tu", tu);
                cmd.Parameters.AddWithValue("@den", den);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                currentReportData = dt;
                dgvChiTiet.DataSource = dt;

                FormatBaoCaoDoanhThu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void FormatBaoCaoDoanhThu()
        {
            if (dgvChiTiet.Columns.Count == 0) return;

            dgvChiTiet.Columns["Ngay"].HeaderText = "Ngày";
            dgvChiTiet.Columns["Ngay"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvChiTiet.Columns["SoHoaDon"].HeaderText = "Số HĐ";
            dgvChiTiet.Columns["DoanhThu"].HeaderText = "Doanh Thu";
            dgvChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["TongChiPhi"].HeaderText = "Tổng Chi";
            dgvChiTiet.Columns["TongChiPhi"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["LoiNhuan"].HeaderText = "Lợi Nhuận";
            dgvChiTiet.Columns["LoiNhuan"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["TyLeLoiNhuan"].HeaderText = "Tỷ Lệ (%)";
            dgvChiTiet.Columns["TyLeLoiNhuan"].DefaultCellStyle.Format = "N2";
        }

        private void LoadBaoCaoSanPhamBanRa(DateTime tu, DateTime den)
        {
            try
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        k.MaSanPham, k.TenSanPham, k.LoaiHang,
                        SUM(ct.SoLuong) AS TongSoLuongBan,
                        SUM(ct.ThanhTien) AS DoanhThu,
                        SUM(ct.TongGiaNhap) AS ChiPhi,
                        SUM(ct.LoiNhuan) AS LoiNhuan,
                        CASE 
                            WHEN SUM(ct.TongGiaNhap) > 0
                            THEN ROUND((SUM(ct.LoiNhuan) / SUM(ct.TongGiaNhap) * 100), 2)
                            ELSE 0
                        END AS TyLeLoiNhuan
                    FROM ChiTietHoaDon ct
                    JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
                    JOIN Kho k ON ct.MaSanPham = k.MaSanPham
                    WHERE h.TrangThai = N'Đã thanh toán'
                      AND h.NgayLap >= @tu
                      AND h.NgayLap <= @den
                    GROUP BY k.MaSanPham, k.TenSanPham, k.LoaiHang
                    ORDER BY DoanhThu DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tu", tu);
                cmd.Parameters.AddWithValue("@den", den);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                currentReportData = dt;
                dgvChiTiet.DataSource = dt;

                FormatBaoCaoSP();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void FormatBaoCaoSP()
        {
            if (dgvChiTiet.Columns.Count == 0) return;

            dgvChiTiet.Columns["MaSanPham"].HeaderText = "Mã SP";
            dgvChiTiet.Columns["TenSanPham"].HeaderText = "Tên SP";
            dgvChiTiet.Columns["LoaiHang"].HeaderText = "Loại";

            dgvChiTiet.Columns["TongSoLuongBan"].HeaderText = "SL Bán";

            dgvChiTiet.Columns["DoanhThu"].HeaderText = "Doanh Thu";
            dgvChiTiet.Columns["DoanhThu"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["ChiPhi"].HeaderText = "Chi Phí";
            dgvChiTiet.Columns["ChiPhi"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["LoiNhuan"].HeaderText = "Lợi Nhuận";
            dgvChiTiet.Columns["LoiNhuan"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["TyLeLoiNhuan"].HeaderText = "Tỷ Lệ (%)";
            dgvChiTiet.Columns["TyLeLoiNhuan"].DefaultCellStyle.Format = "N2";
        }

        private void LoadBaoCaoNhapHang(DateTime tu, DateTime den)
        {
            try
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        pn.MaPhieuNhap,
                        pn.NgayNhap,
                        ncc.TenNhaCungCap,
                        u.HoTen AS NhanVien,
                        pn.TongTien,
                        pn.TrangThai,
                        pn.GhiChu
                    FROM PhieuNhapHang pn
                    JOIN NhaCungCap ncc ON pn.MaNhaCungCap = ncc.MaNhaCungCap
                    JOIN [User] u ON pn.MaNhanVien = u.MaUser
                    WHERE pn.NgayNhap >= @tu AND pn.NgayNhap <= @den
                    ORDER BY pn.NgayNhap DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tu", tu);
                cmd.Parameters.AddWithValue("@den", den);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                currentReportData = dt;
                dgvChiTiet.DataSource = dt;

                FormatBaoCaoNhap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void FormatBaoCaoNhap()
        {
            if (dgvChiTiet.Columns.Count == 0) return;

            dgvChiTiet.Columns["MaPhieuNhap"].HeaderText = "Mã Phiếu";
            dgvChiTiet.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
            dgvChiTiet.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgvChiTiet.Columns["TenNhaCungCap"].HeaderText = "Nhà Cung Cấp";
            dgvChiTiet.Columns["NhanVien"].HeaderText = "Nhân Viên";

            dgvChiTiet.Columns["TongTien"].HeaderText = "Tổng Tiền";
            dgvChiTiet.Columns["TongTien"].DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dgvChiTiet.Columns["GhiChu"].HeaderText = "Ghi Chú";
        }

        private void LoadBaoCaoTonKho()
        {
            try
            {
                conn.Open();
                string sql = @"
                    SELECT 
                        MaSanPham,
                        TenSanPham,
                        LoaiHang,
                        GiaNhap,
                        GiaBan,
                        SoLuongTon,
                        DonViTinh,
                        (GiaNhap * SoLuongTon) AS GiaTriTonKho,
                        CASE 
                            WHEN SoLuongTon = 0 THEN N'Hết hàng'
                            WHEN SoLuongTon < 10 THEN N'Sắp hết'
                            WHEN SoLuongTon < 50 THEN N'Còn ít'
                            ELSE N'Đủ hàng'
                        END AS TrangThaiKho
                    FROM Kho
                    WHERE TrangThai = 1
                    ORDER BY SoLuongTon ASC";

                DataTable dt = new DataTable();
                new SqlDataAdapter(sql, conn).Fill(dt);

                currentReportData = dt;
                dgvChiTiet.DataSource = dt;

                FormatBaoCaoTonKho();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        private void FormatBaoCaoTonKho()
        {
            if (dgvChiTiet.Columns.Count == 0) return;

            dgvChiTiet.Columns["MaSanPham"].HeaderText = "Mã SP";
            dgvChiTiet.Columns["TenSanPham"].HeaderText = "Tên SP";
            dgvChiTiet.Columns["LoaiHang"].HeaderText = "Loại";
            dgvChiTiet.Columns["GiaNhap"].HeaderText = "Giá Nhập";
            dgvChiTiet.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["GiaBan"].HeaderText = "Giá Bán";
            dgvChiTiet.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["SoLuongTon"].HeaderText = "Tồn Kho";
            dgvChiTiet.Columns["DonViTinh"].HeaderText = "ĐVT";
            dgvChiTiet.Columns["GiaTriTonKho"].HeaderText = "Giá Trị Tồn";
            dgvChiTiet.Columns["GiaTriTonKho"].DefaultCellStyle.Format = "N0";
            dgvChiTiet.Columns["TrangThaiKho"].HeaderText = "Trạng Thái";

            foreach (DataGridViewRow row in dgvChiTiet.Rows)
            {
                string st = row.Cells["TrangThaiKho"].Value.ToString();

                if (st == "Hết hàng")
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                else if (st == "Sắp hết")
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        // ================================================================
        //  NÚT XUẤT BÁO CÁO
        // ================================================================
        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            if (currentReportData == null || currentReportData.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng xem báo cáo trước khi xuất!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LockExportButtons();
                return;
            }

            try
            {
                string loai = cboLoaiBaoCao.SelectedItem.ToString();
                BaoCaoPDFExporter.ExportToPDF(currentReportData, loai, dtpTuNgay.Value, dtpDenNgay.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF: " + ex.Message);
            }
        }

        // ================================================================
        //  NÚT IN BÁO CÁO
        // ================================================================
        private void btnInBaoCao_Click(object sender, EventArgs e)
        {
            if (currentReportData == null || currentReportData.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng xem báo cáo trước khi in!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LockExportButtons();
                return;
            }

            MessageBox.Show("Chức năng in sẽ cập nhật sau!", "Thông báo");
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnXemBaoCao_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Đổi màu nền RGB (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Vẽ viền đậm
            int borderThickness = 3; // độ dày viền
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Vẽ chữ đen đậm
            using (Font font = new Font(btn.Font, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnXuatBaoCao_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Đổi màu nền RGB (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Vẽ viền đậm
            int borderThickness = 3; // độ dày viền
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Vẽ chữ đen đậm
            using (Font font = new Font(btn.Font, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnInBaoCao_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Đổi màu nền RGB (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Vẽ viền đậm
            int borderThickness = 3; // độ dày viền
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Vẽ chữ đen đậm
            using (Font font = new Font(btn.Font, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void cboLoaiBaoCao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
