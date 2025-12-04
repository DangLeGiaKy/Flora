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
    public partial class Frmlichsu : Form
    {
        // Connection string - sử dụng giống form bán hàng
        //private string connectionString = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();
        private SqlConnection conn;
        private string selectedMaHoaDon = ""; // Lưu mã hóa đơn được chọn

        public Frmlichsu()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
            InitializeForm();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeForm()
        {
            // Cấu hình DataGridView
            ConfigureDataGridView();

            // Load dữ liệu ban đầu
            LoadDanhSachHoaDon();

            // Cấu hình ComboBox tìm kiếm
            ConfigureComboBoxSearch();

            // Cấu hình DateTimePicker
            dtpNgayTao.Format = DateTimePickerFormat.Short;
            dtpNgayTao.Value = DateTime.Now;

            // Vô hiệu hóa các nút ban đầu
            btnXemHoaDon.Enabled = false;
            btnInHoaDon.Enabled = false;

            // Đăng ký sự kiện
            dgvDanhSachDonHang.SelectionChanged += dgvDanhSachDonHang_SelectionChanged;
        }

        private void ConfigureDataGridView()
        {
            // Xóa các cột cũ nếu có
            dgvDanhSachDonHang.Columns.Clear();
            dgvDanhSachDonHang.AutoGenerateColumns = false;

            // Cấu hình các thuộc tính cơ bản
            dgvDanhSachDonHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhSachDonHang.MultiSelect = false;
            dgvDanhSachDonHang.ReadOnly = true;
            dgvDanhSachDonHang.AllowUserToAddRows = false;
            dgvDanhSachDonHang.RowHeadersVisible = false;

            // Thêm các cột
            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHoaDon",
                HeaderText = "Mã Hóa Đơn",
                Name = "MaHoaDon",
                Width = 150
            });

            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayLap",
                HeaderText = "Ngày Tạo",
                Name = "NgayLap",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenKhachHang",
                HeaderText = "Khách Hàng",
                Name = "TenKhachHang",
                Width = 200
            });

            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTien",
                HeaderText = "Tổng Tiền",
                Name = "TongTien",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng Thái",
                Name = "TrangThai",
                Width = 150
            });

            dgvDanhSachDonHang.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenNhanVien",
                HeaderText = "Nhân Viên",
                Name = "TenNhanVien",
                Width = 150
            });
        }

        private void ConfigureComboBoxSearch()
        {
            // Xóa items cũ nếu có
            cboSearch.Items.Clear();

            // Thêm option mặc định
            cboSearch.Items.Add("-- Tất cả hóa đơn --");
            cboSearch.SelectedIndex = 0;

            // Cấu hình AutoComplete
            cboSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboSearch.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        // Load danh sách tất cả hóa đơn
        private void LoadDanhSachHoaDon(string maHoaDon = "", DateTime? ngayLoc = null)
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();

                string query = @"SELECT 
                                    h.MaHoaDon,
                                    h.NgayLap,
                                    kh.HoTen AS TenKhachHang,
                                    h.TongTien,
                                    h.TrangThai,
                                    u.HoTen AS TenNhanVien
                                FROM HoaDon h
                                LEFT JOIN KhachHang kh ON h.MaKhachHang = kh.MaKhachHang
                                LEFT JOIN [User] u ON h.MaNhanVien = u.MaUser
                                WHERE 1=1";

                // Thêm điều kiện tìm kiếm theo mã hóa đơn
                if (!string.IsNullOrEmpty(maHoaDon) && maHoaDon != "-- Tất cả hóa đơn --")
                {
                    query += " AND h.MaHoaDon LIKE @MaHoaDon";
                }

                // Thêm điều kiện lọc theo ngày
                if (ngayLoc.HasValue)
                {
                    query += " AND CAST(h.NgayLap AS DATE) = @NgayLoc";
                }

                query += " ORDER BY h.NgayLap DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(maHoaDon) && maHoaDon != "-- Tất cả hóa đơn --")
                {
                    cmd.Parameters.AddWithValue("@MaHoaDon", "%" + maHoaDon + "%");
                }

                if (ngayLoc.HasValue)
                {
                    cmd.Parameters.AddWithValue("@NgayLoc", ngayLoc.Value.Date);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvDanhSachDonHang.DataSource = dt;

                // Load danh sách mã hóa đơn vào ComboBox (chỉ load 1 lần khi không có filter)
                if (string.IsNullOrEmpty(maHoaDon) && !ngayLoc.HasValue)
                {
                    LoadMaHoaDonToComboBox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load danh sách hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // Load danh sách mã hóa đơn vào ComboBox
        private void LoadMaHoaDonToComboBox()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();

                string query = "SELECT DISTINCT MaHoaDon FROM HoaDon ORDER BY MaHoaDon DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                // Lưu selected index hiện tại
                int currentIndex = cboSearch.SelectedIndex;

                // Xóa items cũ trừ item đầu tiên
                while (cboSearch.Items.Count > 1)
                {
                    cboSearch.Items.RemoveAt(1);
                }

                // Thêm các mã hóa đơn
                while (reader.Read())
                {
                    cboSearch.Items.Add(reader["MaHoaDon"].ToString());
                }

                reader.Close();

                // Khôi phục selected index
                if (currentIndex >= 0 && currentIndex < cboSearch.Items.Count)
                {
                    cboSearch.SelectedIndex = currentIndex;
                }
                else
                {
                    cboSearch.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load mã hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        // Sự kiện khi chọn hóa đơn trong DataGridView
        private void dgvDanhSachDonHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDanhSachDonHang.CurrentRow != null &&
                dgvDanhSachDonHang.CurrentRow.Index >= 0 &&
                !dgvDanhSachDonHang.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = dgvDanhSachDonHang.CurrentRow;

                if (row.Cells["MaHoaDon"].Value != null &&
                    row.Cells["MaHoaDon"].Value != DBNull.Value &&
                    !string.IsNullOrEmpty(row.Cells["MaHoaDon"].Value.ToString()))
                {
                    selectedMaHoaDon = row.Cells["MaHoaDon"].Value.ToString();

                    // Kích hoạt các nút
                    btnXemHoaDon.Enabled = true;
                    btnInHoaDon.Enabled = true;
                }
                else
                {
                    ClearSelection();
                }
            }
            else
            {
                ClearSelection();
            }
        }

        private void ClearSelection()
        {
            selectedMaHoaDon = "";
            btnXemHoaDon.Enabled = false;
            btnInHoaDon.Enabled = false;
        }

        // Sự kiện tìm kiếm theo mã hóa đơn
        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSearch.SelectedItem != null)
            {
                string searchText = cboSearch.SelectedItem.ToString();

                if (searchText == "-- Tất cả hóa đơn --")
                {
                    LoadDanhSachHoaDon();
                }
                else
                {
                    LoadDanhSachHoaDon(searchText);
                }
            }
        }

        // Sự kiện lọc theo ngày
        private void dtpNgayTao_ValueChanged(object sender, EventArgs e)
        {
            // Lọc theo ngày được chọn
            DateTime ngayLoc = dtpNgayTao.Value.Date;

            string maHoaDon = "";
            if (cboSearch.SelectedItem != null &&
                cboSearch.SelectedItem.ToString() != "-- Tất cả hóa đơn --")
            {
                maHoaDon = cboSearch.SelectedItem.ToString();
            }

            LoadDanhSachHoaDon(maHoaDon, ngayLoc);
        }

        // Xem chi tiết hóa đơn
        private void btnXemHoaDon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedMaHoaDon))
            {
                try
                {
                    frmXemHoaDon frm = new frmXemHoaDon(selectedMaHoaDon);
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi mở form xem hóa đơn: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xem!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // In hóa đơn PDF
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedMaHoaDon))
            {
                try
                {
                    // Gọi class HoaDonPDFExporter để xuất PDF
                    HoaDonPDFExporter.ExportToPDF(selectedMaHoaDon);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi in hóa đơn: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần in!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Sự kiện CellContentClick (giữ lại để tương thích)
        private void dgvDanhSachDonHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Không cần xử lý gì vì đã dùng SelectionChanged
        }

        // Thêm button để reset bộ lọc (optional - bạn có thể thêm button này vào form)
        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            cboSearch.SelectedIndex = 0;
            dtpNgayTao.Value = DateTime.Now;
            LoadDanhSachHoaDon();
        }

        private void Frmlichsu_Load(object sender, EventArgs e)
        {

        }

        private void btnXemHoaDon_Paint(object sender, PaintEventArgs e)
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

        private void btnInHoaDon_Paint(object sender, PaintEventArgs e)
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
    }
}