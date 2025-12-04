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
    public partial class Form1 : Form
    {
        // ✅ DÙNG CONNECTION STRING CHUNG TỪ DatabaseConfig
        private string connectionString = Connection.GetConnectionString();
        //private string connectionString = "Data Source=khanhvinh\\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True";
        //private string connectionString = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private SqlConnection conn;
        private string selectedMaSP = "";
        private string currentMaHoaDon = "";

        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);

            // Khởi tạo form
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Load dữ liệu ban đầu
            LoadSanPham();
            LoadKhachHang();
            ConfigureDataGridViews();

            // Khóa các textbox
            txtMaSP.ReadOnly = true;
            txtTenSP.ReadOnly = true;
            txtGiaBan.ReadOnly = true;
            txtTonKho.ReadOnly = true;
            txtSoDienThoai.ReadOnly = true;
            txtTongHoaDon.ReadOnly = true;
            txtTienThoi.ReadOnly = true;

            // Khóa các button
            btnRemove.Enabled = false;
            btnXemHoaDon.Enabled = false;
            btnInHoaDon.Enabled = false;

            // Kết nối sự kiện SelectionChanged cho DataGridView
            dgvListSP.SelectionChanged += dgvListSP_SelectionChanged;
            dgvShoppingCart.SelectionChanged += dgvShoppingCart_SelectionChanged;
        }

        private void ConfigureDataGridViews()
        {
            // Cấu hình dgvListSP - CHỈ HIỂN THỊ 1 CỘT TÊN SẢN PHẨM
            dgvListSP.AutoGenerateColumns = false;
            dgvListSP.Columns.Clear();

            // Cột ẩn - Mã sản phẩm
            dgvListSP.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSanPham",
                HeaderText = "Mã SP",
                Name = "MaSanPham",
                Visible = false
            });

            // CỘT DUY NHẤT HIỂN THỊ - Tên Sản Phẩm
            dgvListSP.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSanPham",
                HeaderText = "Tên Sản Phẩm",
                Name = "TenSanPham",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // Các cột ẩn - giữ để lấy dữ liệu
            dgvListSP.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaBan",
                Name = "GiaBan",
                Visible = false
            });

            dgvListSP.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaNhap",
                Name = "GiaNhap",
                Visible = false
            });

            dgvListSP.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongTon",
                Name = "SoLuongTon",
                Visible = false
            });

            dgvListSP.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListSP.MultiSelect = false;
            dgvListSP.ReadOnly = true;
            dgvListSP.RowHeadersVisible = false; // Ẩn cột số thứ tự bên trái

            // Cấu hình dgvShoppingCart
            dgvShoppingCart.AutoGenerateColumns = false;
            dgvShoppingCart.Columns.Clear();

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSanPham",
                HeaderText = "Mã SP",
                Name = "MaSanPham",
                Visible = false
            });

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSanPham",
                HeaderText = "Tên Sản Phẩm",
                Name = "TenSanPham",
                Width = 300
            });

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaBan",
                HeaderText = "Giá Bán",
                Name = "GiaBan",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongMua",
                HeaderText = "Số Lượng",
                Name = "SoLuongMua",
                Width = 100
            });

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành Tiền",
                Name = "ThanhTien",
                Width = 180,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvShoppingCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaNhap",
                HeaderText = "GiaNhap",
                Name = "GiaNhap",
                Visible = false
            });

            dgvShoppingCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvShoppingCart.MultiSelect = false;
            dgvShoppingCart.AllowUserToAddRows = false;
            dgvShoppingCart.ReadOnly = true;
        }

        // Load danh sách sản phẩm
        private void LoadSanPham(string searchText = "")
        {
            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();
                string query = @"SELECT MaSanPham, TenSanPham, GiaBan, GiaNhap, SoLuongTon 
                                FROM Kho 
                                WHERE TrangThai = 1";

                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " AND TenSanPham LIKE @SearchText";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(searchText))
                {
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvListSP.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Load danh sách khách hàng
        private void LoadKhachHang()
        {
            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();
                string query = "SELECT MaKhachHang, HoTen, SoDienThoai FROM KhachHang";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboKhachHang.DataSource = dt;
                cboKhachHang.DisplayMember = "HoTen";
                cboKhachHang.ValueMember = "MaKhachHang";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Tìm kiếm sản phẩm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadSanPham(txtSearch.Text.Trim());
        }

        // Chọn sản phẩm từ dgvListSP - SỬ DỤNG SelectionChanged
        private void dgvListSP_SelectionChanged(object sender, EventArgs e)
        {
            // Kiểm tra có dòng được chọn và không phải dòng trống
            if (dgvListSP.CurrentRow != null &&
                dgvListSP.CurrentRow.Index >= 0 &&
                !dgvListSP.CurrentRow.IsNewRow)
            {
                DataGridViewRow row = dgvListSP.CurrentRow;

                // Kiểm tra cell có dữ liệu và không null
                if (row.Cells["MaSanPham"].Value != null &&
                    row.Cells["MaSanPham"].Value != DBNull.Value &&
                    !string.IsNullOrEmpty(row.Cells["MaSanPham"].Value.ToString()))
                {
                    try
                    {
                        selectedMaSP = row.Cells["MaSanPham"].Value.ToString();
                        txtMaSP.Text = selectedMaSP;
                        txtTenSP.Text = row.Cells["TenSanPham"].Value?.ToString() ?? "";

                        // Xử lý an toàn cho giá bán
                        if (row.Cells["GiaBan"].Value != null && row.Cells["GiaBan"].Value != DBNull.Value)
                        {
                            txtGiaBan.Text = Convert.ToDecimal(row.Cells["GiaBan"].Value).ToString("N0");
                        }
                        else
                        {
                            txtGiaBan.Text = "0";
                        }

                        // Xử lý an toàn cho tồn kho
                        if (row.Cells["SoLuongTon"].Value != null && row.Cells["SoLuongTon"].Value != DBNull.Value)
                        {
                            txtTonKho.Text = row.Cells["SoLuongTon"].Value.ToString();
                        }
                        else
                        {
                            txtTonKho.Text = "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi im lặng, không hiển thị thông báo
                        // Hoặc log lỗi nếu cần
                        System.Diagnostics.Debug.WriteLine("Lỗi chọn sản phẩm: " + ex.Message);
                    }
                }
                else
                {
                    // Nếu click vào vùng trống, xóa thông tin
                    ClearProductInfo();
                }
            }
            else
            {
                // Không có dòng nào được chọn hoặc dòng trống
                ClearProductInfo();
            }
        }

        // Hàm xóa thông tin sản phẩm
        private void ClearProductInfo()
        {
            selectedMaSP = "";
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGiaBan.Clear();
            txtTonKho.Clear();
        }

        // Chọn khách hàng
        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboKhachHang.SelectedValue != null && cboKhachHang.SelectedItem != null)
            {
                DataRowView row = (DataRowView)cboKhachHang.SelectedItem;
                txtSoDienThoai.Text = row["SoDienThoai"].ToString();
            }
        }

        // Thêm khách hàng mới
        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            //// TODO: Tạo form frmThemKH
            //MessageBox.Show("Chức năng thêm khách hàng đang được phát triển!\nVui lòng tạo form frmThemKH.",
            //    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //// Code mẫu khi đã có form:
            frmthemkh frmKH = new frmthemkh();
            if (frmKH.ShowDialog() == DialogResult.OK)
            {
                LoadKhachHang(); // Reload lại danh sách khách hàng
            }
        }

        // Thêm sản phẩm vào giỏ hàng
        private void btnAddToShoppingCart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedMaSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtSoLuongMua.Text) || !int.TryParse(txtSoLuongMua.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuongMua.Focus();
                return;
            }

            int tonKho = int.Parse(txtTonKho.Text);
            if (soLuong > tonKho)
            {
                MessageBox.Show("Số lượng mua vượt quá tồn kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();
                string query = "SELECT MaSanPham, TenSanPham, GiaBan, GiaNhap FROM Kho WHERE MaSanPham = @MaSP";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaSP", selectedMaSP);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal giaBan = Convert.ToDecimal(reader["GiaBan"]);
                    decimal giaNhap = Convert.ToDecimal(reader["GiaNhap"]);
                    string tenSP = reader["TenSanPham"].ToString();

                    reader.Close();

                    // Kiểm tra sản phẩm đã có trong giỏ chưa
                    bool found = false;
                    DataTable dt = (DataTable)dgvShoppingCart.DataSource;

                    if (dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["MaSanPham"].ToString() == selectedMaSP)
                            {
                                int currentQty = Convert.ToInt32(row["SoLuongMua"]);
                                int newQty = currentQty + soLuong;

                                if (newQty > tonKho)
                                {
                                    MessageBox.Show("Tổng số lượng vượt quá tồn kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                row["SoLuongMua"] = newQty;
                                row["ThanhTien"] = giaBan * newQty;
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        if (dt == null)
                        {
                            dt = new DataTable();
                            dt.Columns.Add("MaSanPham", typeof(string));
                            dt.Columns.Add("TenSanPham", typeof(string));
                            dt.Columns.Add("GiaBan", typeof(decimal));
                            dt.Columns.Add("GiaNhap", typeof(decimal));
                            dt.Columns.Add("SoLuongMua", typeof(int));
                            dt.Columns.Add("ThanhTien", typeof(decimal));
                        }

                        decimal thanhTien = giaBan * soLuong;
                        dt.Rows.Add(selectedMaSP, tenSP, giaBan, giaNhap, soLuong, thanhTien);
                        dgvShoppingCart.DataSource = dt;
                    }

                    dgvShoppingCart.Refresh();
                    UpdateTongHoaDon();
                    txtSoLuongMua.Clear();
                    txtSoLuongMua.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm vào giỏ hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Chọn dòng trong giỏ hàng - SỬ DỤNG SelectionChanged
        private void dgvShoppingCart_SelectionChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = dgvShoppingCart.CurrentRow != null && dgvShoppingCart.Rows.Count > 0;
        }

        // Xóa sản phẩm khỏi giỏ hàng
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvShoppingCart.CurrentRow != null && dgvShoppingCart.CurrentRow.Index >= 0)
            {
                DataTable dt = (DataTable)dgvShoppingCart.DataSource;
                if (dt != null && dgvShoppingCart.CurrentRow.Index < dt.Rows.Count)
                {
                    dt.Rows.RemoveAt(dgvShoppingCart.CurrentRow.Index);
                    dgvShoppingCart.Refresh();
                    UpdateTongHoaDon();
                    btnRemove.Enabled = false;
                }
            }
        }

        // Cập nhật tổng hóa đơn
        private void UpdateTongHoaDon()
        {
            decimal tongTien = 0;
            DataTable dt = (DataTable)dgvShoppingCart.DataSource;

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    tongTien += Convert.ToDecimal(row["ThanhTien"]);
                }
            }

            txtTongHoaDon.Text = tongTien.ToString("N0");
        }

        // Tính tiền thối
        private void btnTinhTienThoi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTongHoaDon.Text) || txtTongHoaDon.Text == "0")
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTienKhachDua.Text))
            {
                MessageBox.Show("Vui lòng nhập tiền khách đưa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKhachDua.Focus();
                return;
            }

            if (!decimal.TryParse(txtTienKhachDua.Text, out decimal tienKhachDua))
            {
                MessageBox.Show("Số tiền không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKhachDua.Focus();
                return;
            }

            decimal tongHoaDon = decimal.Parse(txtTongHoaDon.Text.Replace(",", ""));

            if (tienKhachDua < tongHoaDon)
            {
                MessageBox.Show("Tiền khách đưa không đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tienThoi = tienKhachDua - tongHoaDon;
            txtTienThoi.Text = tienThoi.ToString("N0");
        }

        // Thanh toán
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvShoppingCart.DataSource;

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtTienThoi.Text))
            {
                MessageBox.Show("Vui lòng tính tiền thối trước khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Tạo mã hóa đơn
                    currentMaHoaDon = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Tính tổng giá nhập và lợi nhuận
                    decimal tongTien = decimal.Parse(txtTongHoaDon.Text.Replace(",", ""));
                    decimal tongGiaNhap = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        decimal giaNhap = Convert.ToDecimal(row["GiaNhap"]);
                        int soLuong = Convert.ToInt32(row["SoLuongMua"]);
                        tongGiaNhap += giaNhap * soLuong;
                    }

                    decimal loiNhuan = tongTien - tongGiaNhap;
                    decimal tienKhachDua = decimal.Parse(txtTienKhachDua.Text);
                    decimal tienThoi = decimal.Parse(txtTienThoi.Text.Replace(",", ""));

                    // Insert hóa đơn
                    string queryHD = @"INSERT INTO HoaDon 
                                      (MaHoaDon, MaKhachHang, MaNhanVien, NgayLap, TongTien, TongGiaNhap, LoiNhuan, TienKhachDua, TienThoiLai, TrangThai)
                                      VALUES (@MaHD, @MaKH, @MaNV, @NgayLap, @TongTien, @TongGiaNhap, @LoiNhuan, @TienKhachDua, @TienThoi, @TrangThai)";

                    SqlCommand cmdHD = new SqlCommand(queryHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@MaHD", currentMaHoaDon);
                    cmdHD.Parameters.AddWithValue("@MaKH", cboKhachHang.SelectedValue);
                    cmdHD.Parameters.AddWithValue("@MaNV", "U003"); // TODO: Lấy từ session đăng nhập
                    cmdHD.Parameters.AddWithValue("@NgayLap", DateTime.Now);
                    cmdHD.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdHD.Parameters.AddWithValue("@TongGiaNhap", tongGiaNhap);
                    cmdHD.Parameters.AddWithValue("@LoiNhuan", loiNhuan);
                    cmdHD.Parameters.AddWithValue("@TienKhachDua", tienKhachDua);
                    cmdHD.Parameters.AddWithValue("@TienThoi", tienThoi);
                    cmdHD.Parameters.AddWithValue("@TrangThai", "Đã thanh toán");
                    cmdHD.ExecuteNonQuery();

                    // Insert chi tiết hóa đơn - SỬA LỖI TRÙNG MÃ CHI TIẾT
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        // Tạo mã chi tiết UNIQUE với timestamp và số thứ tự
                        string maCT = "CT" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + stt.ToString("000");
                        string maSP = row["MaSanPham"].ToString();
                        int soLuong = Convert.ToInt32(row["SoLuongMua"]);

                        SqlCommand cmdSP = new SqlCommand("sp_ThemChiTietHoaDon", conn, transaction);
                        cmdSP.CommandType = CommandType.StoredProcedure;
                        cmdSP.Parameters.AddWithValue("@MaChiTiet", maCT);
                        cmdSP.Parameters.AddWithValue("@MaHoaDon", currentMaHoaDon);
                        cmdSP.Parameters.AddWithValue("@MaSanPham", maSP);
                        cmdSP.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmdSP.ExecuteNonQuery();

                        stt++;

                        // Delay nhỏ để tránh trùng timestamp (nếu cần)
                        System.Threading.Thread.Sleep(10);
                    }

                    transaction.Commit();

                    MessageBox.Show("Thanh toán thành công!\nMã hóa đơn: " + currentMaHoaDon,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở nút xem và in hóa đơn
                    btnXemHoaDon.Enabled = true;
                    btnInHoaDon.Enabled = true;

                    // Reset form
                    ResetForm();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Xem hóa đơn
        private void btnXemHoaDon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentMaHoaDon))
            {
                frmXemHoaDon frm = new frmXemHoaDon(currentMaHoaDon);
                frm.ShowDialog();
            }
        }

        // In hóa đơn - GỌI CLASS CHUNG
        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentMaHoaDon))
            {
                // Gọi class HoaDonPDFExporter để xuất PDF trực tiếp
                HoaDonPDFExporter.ExportToPDF(currentMaHoaDon);
            }
            else
            {
                MessageBox.Show("Chưa có hóa đơn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Reset form sau khi thanh toán
        private void ResetForm()
        {
            dgvShoppingCart.DataSource = null;
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtGiaBan.Clear();
            txtTonKho.Clear();
            txtSoLuongMua.Clear();
            txtTongHoaDon.Clear();
            txtTienKhachDua.Clear();
            txtTienThoi.Clear();
            txtSearch.Clear();
            selectedMaSP = "";
            LoadSanPham();
            btnRemove.Enabled = false;
        }

        // ===== CÁC EVENT HANDLERS CŨ (GIỮ LẠI ĐỂ TƯƠNG THÍCH) =====
        private void label1_Click(object sender, EventArgs e) { }
        private void label16_Click(object sender, EventArgs e) { }
        private void txtMaSP_TextChanged(object sender, EventArgs e) { }
        private void txtGiaBan_TextChanged(object sender, EventArgs e) { }
        private void txtTenSP_TextChanged(object sender, EventArgs e) { }
        private void txtTonKho_TextChanged(object sender, EventArgs e) { }
        private void txtSoDienThoai_TextChanged(object sender, EventArgs e) { }
        private void txtSoLuongMua_TextChanged(object sender, EventArgs e) { }
        private void txtTongHoaDon_TextChanged(object sender, EventArgs e) { }
        private void txtTienKhachDua_TextChanged(object sender, EventArgs e) { }
        private void txtTienThoi_TextChanged(object sender, EventArgs e) { }

        // Event handlers cũ cho DataGridView (giữ lại nhưng không dùng)
        private void dgvListSP_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvShoppingCart_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnXemLichSu_Click(object sender, EventArgs e)
        {
            Frmlichsu frmLS = new Frmlichsu();
            frmLS.ShowDialog();
        }

        private void btnRemove_Paint(object sender, PaintEventArgs e)
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

        private void btnThemKhachHang_Paint(object sender, PaintEventArgs e)
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

        private void btnAddToShoppingCart_Paint(object sender, PaintEventArgs e)
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

        private void btnTinhTienThoi_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTongHoaDon.Text) || txtTongHoaDon.Text == "0")
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtTienKhachDua.Text))
            {
                MessageBox.Show("Vui lòng nhập tiền khách đưa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKhachDua.Focus();
                return;
            }

            if (!decimal.TryParse(txtTienKhachDua.Text, out decimal tienKhachDua))
            {
                MessageBox.Show("Số tiền không hợp lệ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTienKhachDua.Focus();
                return;
            }

            decimal tongHoaDon = decimal.Parse(txtTongHoaDon.Text.Replace(",", ""));

            if (tienKhachDua < tongHoaDon)
            {
                MessageBox.Show("Tiền khách đưa không đủ!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal tienThoi = tienKhachDua - tongHoaDon;
            txtTienThoi.Text = tienThoi.ToString("N0");
        }

        private void btnThanhToan_Click_1(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvShoppingCart.DataSource;

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboKhachHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhachHang.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtTienThoi.Text))
            {
                MessageBox.Show("Vui lòng tính tiền thối trước khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Đóng connection nếu đang mở
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Tạo mã hóa đơn
                    currentMaHoaDon = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    // Tính tổng giá nhập và lợi nhuận
                    decimal tongTien = decimal.Parse(txtTongHoaDon.Text.Replace(",", ""));
                    decimal tongGiaNhap = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        decimal giaNhap = Convert.ToDecimal(row["GiaNhap"]);
                        int soLuong = Convert.ToInt32(row["SoLuongMua"]);
                        tongGiaNhap += giaNhap * soLuong;
                    }

                    decimal loiNhuan = tongTien - tongGiaNhap;
                    decimal tienKhachDua = decimal.Parse(txtTienKhachDua.Text);
                    decimal tienThoi = decimal.Parse(txtTienThoi.Text.Replace(",", ""));

                    // Insert hóa đơn
                    string queryHD = @"INSERT INTO HoaDon 
                                      (MaHoaDon, MaKhachHang, MaNhanVien, NgayLap, TongTien, TongGiaNhap, LoiNhuan, TienKhachDua, TienThoiLai, TrangThai)
                                      VALUES (@MaHD, @MaKH, @MaNV, @NgayLap, @TongTien, @TongGiaNhap, @LoiNhuan, @TienKhachDua, @TienThoi, @TrangThai)";

                    SqlCommand cmdHD = new SqlCommand(queryHD, conn, transaction);
                    cmdHD.Parameters.AddWithValue("@MaHD", currentMaHoaDon);
                    cmdHD.Parameters.AddWithValue("@MaKH", cboKhachHang.SelectedValue);
                    cmdHD.Parameters.AddWithValue("@MaNV", "U003"); // TODO: Lấy từ session đăng nhập
                    cmdHD.Parameters.AddWithValue("@NgayLap", DateTime.Now);
                    cmdHD.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdHD.Parameters.AddWithValue("@TongGiaNhap", tongGiaNhap);
                    cmdHD.Parameters.AddWithValue("@LoiNhuan", loiNhuan);
                    cmdHD.Parameters.AddWithValue("@TienKhachDua", tienKhachDua);
                    cmdHD.Parameters.AddWithValue("@TienThoi", tienThoi);
                    cmdHD.Parameters.AddWithValue("@TrangThai", "Đã thanh toán");
                    cmdHD.ExecuteNonQuery();

                    // Insert chi tiết hóa đơn - SỬA LỖI TRÙNG MÃ CHI TIẾT
                    int stt = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        // Tạo mã chi tiết UNIQUE với timestamp và số thứ tự
                        string maCT = "CT" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + stt.ToString("000");
                        string maSP = row["MaSanPham"].ToString();
                        int soLuong = Convert.ToInt32(row["SoLuongMua"]);

                        SqlCommand cmdSP = new SqlCommand("sp_ThemChiTietHoaDon", conn, transaction);
                        cmdSP.CommandType = CommandType.StoredProcedure;
                        cmdSP.Parameters.AddWithValue("@MaChiTiet", maCT);
                        cmdSP.Parameters.AddWithValue("@MaHoaDon", currentMaHoaDon);
                        cmdSP.Parameters.AddWithValue("@MaSanPham", maSP);
                        cmdSP.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmdSP.ExecuteNonQuery();

                        stt++;

                        // Delay nhỏ để tránh trùng timestamp (nếu cần)
                        System.Threading.Thread.Sleep(10);
                    }

                    transaction.Commit();

                    MessageBox.Show("Thanh toán thành công!\nMã hóa đơn: " + currentMaHoaDon,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Mở nút xem và in hóa đơn
                    btnXemHoaDon.Enabled = true;
                    btnInHoaDon.Enabled = true;

                    // Reset form
                    ResetForm();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnXemLichSu_Click_1(object sender, EventArgs e)
        {
            Frmlichsu frmLS = new Frmlichsu();
            frmLS.ShowDialog();
        }

        private void btnXemHoaDon_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentMaHoaDon))
            {
                frmXemHoaDon frm = new frmXemHoaDon(currentMaHoaDon);
                frm.ShowDialog();
            }
        }

        private void btnInHoaDon_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentMaHoaDon))
            {
                // Gọi class HoaDonPDFExporter để xuất PDF trực tiếp
                HoaDonPDFExporter.ExportToPDF(currentMaHoaDon);
            }
            else
            {
                MessageBox.Show("Chưa có hóa đơn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}