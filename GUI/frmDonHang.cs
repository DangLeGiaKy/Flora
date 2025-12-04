using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.BUS;
using test.DAL;
using test.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.IO;


namespace test.GUI
{

    public partial class frmQuanLyDonHang : Form

    {
        //private string connect = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connect = Connection.GetConnectionString();
        private string currentMaHoaDon = "";
        public frmQuanLyDonHang()
        {
            InitializeComponent();
            LoadDonHang();
            LockFields(true);
            LoadTenKhachHang();
        }
        public void LoadDonHang()
        {
            string sql = @"
                SELECT dh.MaDonHang,
                       kh.HoTen AS TenKhachHang,
                       kh.SoDienThoai,
                        STRING_AGG(k.TenSanPham, ', ') AS SanPham,
                       SUM(ct.SoLuong) AS TongSoLuong,
                       dh.TongTien,
                       dh.NgayGiao,
                       dh.TrangThai
                FROM DonHang dh
                JOIN KhachHang kh ON dh.MaKhachHang = kh.MaKhachHang
                JOIN ChiTietDonHang ct ON dh.MaDonHang = ct.MaDonHang
                JOIN Kho k ON ct.MaSanPham = k.MaSanPham
                GROUP BY dh.MaDonHang, kh.HoTen, kh.SoDienThoai, dh.TongTien, dh.NgayGiao, dh.TrangThai
                ORDER BY dh.MaDonHang DESC";

            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvDonHang.DataSource = dt;
            }
        }

        private void LockFields(bool locked)
        {
            txtSoLuong.ReadOnly = locked;
            txtTongTien.ReadOnly = locked;
            txtSDT.ReadOnly = locked;
            txtKhachHang.ReadOnly = locked;
            txtSanPham.ReadOnly = locked;

            cmbTrangThai.Enabled = !locked;
            dtNgayNhan.Enabled = !locked;
        }
        private void LoadTrangThai()
        {
            cmbTrangThai.Items.Clear();
            cmbTrangThai.Items.AddRange(new string[] {
        "Đang xử lý",
        "Đã xác nhận",
        "Đang giao",
        "Hoàn tất",
        "Hủy"
    });
        }
        private void LoadSanPham()
        {
            string sql = "SELECT MaSanPham, TenSanPham FROM Kho WHERE TrangThai = 1";

            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                // lưu mã sản phẩm
            }
        }

        private void SetHeader(string col, string title)
        {
            if (dgvDonHang.Columns[col] != null)
                dgvDonHang.Columns[col].HeaderText = title;
        }
        private void LoadTenKhachHang()
        {
            string sql = "SELECT MaKhachHang, HoTen FROM KhachHang";

            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboTimKH.DataSource = dt;
                cboTimKH.DisplayMember = "HoTen";        // Hiển thị tên khách
                cboTimKH.ValueMember = "MaKhachHang";    // Lấy mã khách
                cboTimKH.SelectedIndex = -1;             // Không chọn sẵn
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboTimKH.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!");
                return;
            }

            string maKH = cboTimKH.SelectedValue.ToString();

            string sql = @"
        SELECT dh.MaDonHang,
               kh.HoTen AS TenKhachHang,
               kh.SoDienThoai,
               STRING_AGG(kho.TenSanPham, ', ') AS SanPham,
               SUM(ct.SoLuong) AS TongSoLuong,
               dh.TongTien,
               dh.NgayGiao,
               dh.TrangThai
        FROM DonHang dh
        JOIN KhachHang kh ON dh.MaKhachHang = kh.MaKhachHang
        JOIN ChiTietDonHang ct ON dh.MaDonHang = ct.MaDonHang
        JOIN Kho kho ON ct.MaSanPham = kho.MaSanPham
        WHERE kh.MaKhachHang = @MaKH
        GROUP BY dh.MaDonHang, kh.HoTen, kh.SoDienThoai,
                 dh.TongTien, dh.NgayGiao, dh.TrangThai
        ORDER BY dh.MaDonHang DESC";

            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.SelectCommand.Parameters.AddWithValue("@MaKH", maKH);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvDonHang.DataSource = dt;
                // định dạng lại cột nếu cần
            }

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            frmThemDonHang f = new frmThemDonHang(this); // truyền 'this'
            f.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để sửa!");
                return;
            }

            if (cmbTrangThai.Text != "Đang xử lý")
            {
                MessageBox.Show("Chỉ đơn hàng 'Đang xử lý' mới được sửa!");
                return;
            }

            // Mở các trường cần chỉnh
            txtSoLuong.ReadOnly = false;
            txtTongTien.ReadOnly = false;
            txtKhachHang.ReadOnly = true;
            txtSanPham.ReadOnly = true;
            txtSDT.ReadOnly = true;
            txtSanPham.ReadOnly = true;
            dtNgayNhan.Enabled = true;

            cmbTrangThai.Enabled = true;

            // Load sản phẩm vào ComboBox
            LoadSanPham();

            // Load các trạng thái vào ComboBox
            LoadTrangThai();

            // Chọn sản phẩm hiện tại


            // Chọn trạng thái hiện tại
            cmbTrangThai.SelectedItem = dgvDonHang.CurrentRow.Cells["TrangThai"].Value.ToString(); // hàm đánh dấu sản phẩm đã có trong đơn
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow == null) return;

            string maDonHang = dgvDonHang.CurrentRow.Cells["MaDonHang"].Value.ToString();
            string trangThai = dgvDonHang.CurrentRow.Cells["TrangThai"].Value.ToString();

            if (trangThai == "Hoàn tất")
            {
                MessageBox.Show("Không thể xóa đơn hàng đã hoàn tất!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa đơn hàng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connect))
                    {
                        string sql = "DELETE FROM DonHang WHERE MaDonHang=@MaDonHang";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.Add("@MaDonHang", SqlDbType.VarChar, 20).Value = maDonHang;

                        conn.Open();
                        int r = cmd.ExecuteNonQuery();

                        if (r > 0)
                        {
                            MessageBox.Show("Xóa thành công!");
                            LoadDonHang(); // reload DataGridView
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy đơn hàng hoặc đã bị xóa!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa đơn hàng: " + ex.Message);
                }
            }
        }
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {

        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {

        }

        private void dgvDonHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvDonHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow == null) return;

            string maDonHang = dgvDonHang.CurrentRow.Cells["MaDonHang"].Value.ToString();
            string trangThai = cmbTrangThai.Text;

            decimal tongTien;
            int soLuong;
            if (!decimal.TryParse(txtTongTien.Text, out tongTien) ||
                !int.TryParse(txtSoLuong.Text, out soLuong))
            {
                MessageBox.Show("Số lượng hoặc tổng tiền không hợp lệ!");
                return;
            }

            DateTime ngayGiao = dtNgayNhan.Value;

            try
            {
                using (SqlConnection conn = new SqlConnection(connect))
                {
                    conn.Open();

                    // 1️⃣ Cập nhật ChiTietDonHang
                    string sqlCT = @"
                        UPDATE ct
                        SET ct.SoLuong = @SoLuong,
                            ct.ThanhTien = @SoLuong * ct.GiaBan
                        FROM ChiTietDonHang ct
                        WHERE ct.MaDonHang = @MaDonHang";
                    SqlCommand cmdCT = new SqlCommand(sqlCT, conn);
                    cmdCT.Parameters.Add("@MaDonHang", SqlDbType.VarChar, 20).Value = maDonHang;
                    cmdCT.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;
                    cmdCT.ExecuteNonQuery();

                    // 2️⃣ Cập nhật DonHang (tính lại tổng tiền từ ChiTietDonHang)
                    string sqlDH = @"
                        UPDATE DonHang
                        SET TongTien = (SELECT SUM(ThanhTien) FROM ChiTietDonHang WHERE MaDonHang=@MaDonHang),
                            NgayGiao = @NgayGiao,
                            TrangThai = @TrangThai,
                            NgayCapNhat = GETDATE()
                        WHERE MaDonHang = @MaDonHang";
                    SqlCommand cmdDH = new SqlCommand(sqlDH, conn);
                    cmdDH.Parameters.Add("@MaDonHang", SqlDbType.VarChar, 20).Value = maDonHang;
                    cmdDH.Parameters.Add("@NgayGiao", SqlDbType.DateTime).Value = ngayGiao;
                    cmdDH.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 30).Value = trangThai;
                    cmdDH.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thành công!");
                    LoadDonHang(); // reload DataGridView
                    LockFields(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đơn hàng: " + ex.Message);
            }
        }







        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSoluong_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvDonHang_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDonHang.Rows[e.RowIndex];

            txtKhachHang.Text = row.Cells["TenKhachHang"].Value.ToString();
            txtSDT.Text = row.Cells["SoDienThoai"].Value.ToString();
            txtSanPham.Text = row.Cells["SanPham"].Value.ToString();
            txtSoLuong.Text = row.Cells["TongSoLuong"].Value.ToString();
            txtTongTien.Text = row.Cells["TongTien"].Value.ToString();
            dtNgayNhan.Value = row.Cells["NgayGiao"].Value == DBNull.Value
                               ? DateTime.Now
                               : Convert.ToDateTime(row.Cells["NgayGiao"].Value);
            cmbTrangThai.Text = row.Cells["TrangThai"].Value.ToString();

            // ✅ Cập nhật mã hóa đơn hiện tại
            currentMaHoaDon = row.Cells["MaDonHang"].Value.ToString();

            LockFields(true);
        }

        private void cmbKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtMaDonHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboTimKH.SelectedIndex = -1; // bỏ chọn filter
            LoadDonHang();
        }

        private void btnHuyDonHang_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn đơn hàng cần hủy!");
                return;
            }

            string maDH = dgvDonHang.CurrentRow.Cells["MaDonHang"].Value.ToString();

            DialogResult result = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?",
                "Xác nhận", MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Lấy danh sách sản phẩm của đơn
                    string sqlGetCT = @"SELECT MaSanPham, SoLuong FROM ChiTietDonHang WHERE MaDonHang = @MaDH";
                    SqlCommand cmdGet = new SqlCommand(sqlGetCT, conn, tran);
                    cmdGet.Parameters.AddWithValue("@MaDH", maDH);

                    DataTable dtCT = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmdGet))
                    {
                        da.SelectCommand.Transaction = tran;
                        da.Fill(dtCT);
                    }

                    // 2️⃣ Hoàn trả tồn kho
                    foreach (DataRow row in dtCT.Rows)
                    {
                        string maSP = row["MaSanPham"].ToString();
                        int sl = Convert.ToInt32(row["SoLuong"]);

                        string sqlKho = @"UPDATE Kho SET SoLuongTon = SoLuongTon + @SL WHERE MaSanPham = @MaSP";
                        SqlCommand cmdKho = new SqlCommand(sqlKho, conn, tran);
                        cmdKho.Parameters.AddWithValue("@SL", sl);
                        cmdKho.Parameters.AddWithValue("@MaSP", maSP);
                        cmdKho.ExecuteNonQuery();
                    }

                    // 3️⃣ Cập nhật trạng thái đơn hàng → Hủy
                    string sqlHuy = @"UPDATE DonHang SET TrangThai = N'Hủy' WHERE MaDonHang = @MaDH";
                    SqlCommand cmdHuy = new SqlCommand(sqlHuy, conn, tran);
                    cmdHuy.Parameters.AddWithValue("@MaDH", maDH);
                    cmdHuy.ExecuteNonQuery();

                    tran.Commit();
                    MessageBox.Show("Hủy đơn hàng thành công!");

                    LoadDonHang();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Lỗi khi hủy đơn!\n" + ex.Message);
                }
            }
        }

        private void btnInHoaDon_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentMaHoaDon))
            {
                HoaDonPDFExporterr.ExportToPDF(currentMaHoaDon);
            }
            else
            {
                MessageBox.Show("Chưa có hóa đơn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cboTimKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnInHoaDon_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnHuyDonHang_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnThem_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnSua_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnXoa_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnLuu_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnTimKiem_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }

        private void btnRefresh_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút màu (119, 255, 0)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219)))
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm
            using (System.Drawing.Font font = new System.Drawing.Font(btn.Font, FontStyle.Bold)) // chú ý namespace System.Drawing.Font
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                SizeF textSize = g.MeasureString(btn.Text, font);
                g.DrawString(btn.Text, font, brush,
                    (btn.Width - textSize.Width) / 2,
                    (btn.Height - textSize.Height) / 2);
            }
        }
    }
}

