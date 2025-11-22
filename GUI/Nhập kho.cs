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

namespace test.GUI
{
    public partial class frmNhkho : Form
    {
        // Chuỗi kết nối
        private string connectionString = "Server=khanhvinh\\SQLEXPRESS;Database=FloraShopDB;Integrated Security=True;";

        public frmNhkho()
        {
            InitializeComponent();
            this.Load += frmNhkho_Load;
        }

        private void frmNhkho_Load(object sender, EventArgs e)
        {
            try
            {
                // Tự động tạo mã sản phẩm
                GenerateProductCode();

                // Thiết lập giá trị mặc định
                if (dtpNgayNhap != null)
                {
                    dtpNgayNhap.Value = DateTime.Now;
                }

                // Load dữ liệu cho ComboBox
                LoadComboBoxData();

                // Khóa textbox mã sản phẩm
                if (txtMaSanPham != null)
                {
                    txtMaSanPham.ReadOnly = true;
                    txtMaSanPham.BackColor = SystemColors.Control;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Load form: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load dữ liệu cho ComboBox
        private void LoadComboBoxData()
        {
            try
            {
                LoadDonViTinh();
                LoadNhaCungCap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ComboBox: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load đơn vị tính
        private void LoadDonViTinh()
        {
            try
            {
                if (cboDonViTinh == null) return;

                cboDonViTinh.Items.Clear();

                // Danh sách đơn vị mặc định
                string[] units = { "Cái", "Chiếc", "Hộp", "Thùng", "Kg", "Gram",
                                   "Lít", "Chai", "Gói", "Bộ", "Cặp", "Tá", "Mét" };

                cboDonViTinh.Items.AddRange(units);

                if (cboDonViTinh.Items.Count > 0)
                {
                    cboDonViTinh.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load đơn vị tính: " + ex.Message);
            }
        }

        // Load nhà cung cấp từ database
        private void LoadNhaCungCap()
        {
            try
            {
                if (cboNCC == null) return;

                cboNCC.Items.Clear();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Tên cột đúng theo cấu trúc database
                    string query = "SELECT MaNhaCungCap, TenNhaCungCap FROM NhaCungCap ORDER BY TenNhaCungCap";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cboNCC.Items.Add(new ComboBoxItem
                        {
                            Value = reader["MaNhaCungCap"].ToString(),
                            Text = reader["TenNhaCungCap"].ToString()
                        });
                    }

                    reader.Close();

                    if (cboNCC.Items.Count > 0)
                    {
                        cboNCC.DisplayMember = "Text";
                        cboNCC.ValueMember = "Value";
                        cboNCC.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Chưa có nhà cung cấp nào trong database!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhà cung cấp: " + ex.Message + "\n\nDùng dữ liệu mẫu thay thế.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Dùng dữ liệu mẫu nếu lỗi
                string[] suppliers = { "Nhà cung cấp A", "Nhà cung cấp B", "Nhà cung cấp C", "Nhà cung cấp D" };
                cboNCC.Items.AddRange(suppliers);

                if (cboNCC.Items.Count > 0)
                {
                    cboNCC.SelectedIndex = 0;
                }
            }
        }

        // Tự động tạo mã sản phẩm
        private void GenerateProductCode()
        {
            try
            {
                if (txtMaSanPham == null) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT TOP 1 MaSanPham FROM Kho WHERE MaSanPham LIKE 'SP%' ORDER BY MaSanPham DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastCode = result.ToString();

                        if (lastCode.StartsWith("SP") && lastCode.Length > 2)
                        {
                            string numberPart = lastCode.Substring(2);

                            if (int.TryParse(numberPart, out int number))
                            {
                                txtMaSanPham.Text = "SP" + (number + 1).ToString("D3");
                            }
                            else
                            {
                                txtMaSanPham.Text = "SP001";
                            }
                        }
                        else
                        {
                            txtMaSanPham.Text = "SP001";
                        }
                    }
                    else
                    {
                        txtMaSanPham.Text = "SP001";
                    }
                }
            }
            catch (Exception ex)
            {
                txtMaSanPham.Text = "SP001";
                MessageBox.Show("Không thể tạo mã tự động. Dùng mã mặc định SP001.\n\nLỗi: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Button Nhập Hàng
        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (!ValidateInput())
                {
                    return;
                }

                // Xác nhận
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn nhập sản phẩm này vào kho?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (InsertProduct())
                    {
                        MessageBox.Show("Nhập hàng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validate dữ liệu
        private bool ValidateInput()
        {
            // Mã sản phẩm
            if (string.IsNullOrWhiteSpace(txtMaSanPham?.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sản phẩm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSanPham?.Focus();
                return false;
            }

            // Tên sản phẩm
            if (string.IsNullOrWhiteSpace(txtTenSanPham?.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSanPham?.Focus();
                return false;
            }

            // Loại hàng
            if (string.IsNullOrWhiteSpace(txtLoaiHang?.Text))
            {
                MessageBox.Show("Vui lòng nhập loại hàng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLoaiHang?.Focus();
                return false;
            }

            // Giá nhập
            if (string.IsNullOrWhiteSpace(txtGiaNhap?.Text))
            {
                MessageBox.Show("Vui lòng nhập giá nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap?.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
            {
                MessageBox.Show("Giá nhập không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaNhap?.Focus();
                return false;
            }

            // Giá bán
            if (string.IsNullOrWhiteSpace(txtGiaBan?.Text))
            {
                MessageBox.Show("Vui lòng nhập giá bán!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan?.Focus();
                return false;
            }

            if (!decimal.TryParse(txtGiaBan.Text, out decimal giaBan) || giaBan < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaBan?.Focus();
                return false;
            }

            // Số lượng
            if (string.IsNullOrWhiteSpace(txtSoLuong?.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong?.Focus();
                return false;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong?.Focus();
                return false;
            }

            // Đơn vị tính
            if (cboDonViTinh?.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboDonViTinh?.Focus();
                return false;
            }

            return true;
        }

        // Thêm sản phẩm vào database
        private bool InsertProduct()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng mã
                    string checkQuery = "SELECT COUNT(*) FROM Kho WHERE MaSanPham = @MaSanPham";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@MaSanPham", txtMaSanPham.Text);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Mã sản phẩm đã tồn tại!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // Insert sản phẩm mới (không có cột MoTa)
                    string query = @"INSERT INTO Kho 
                                    (MaSanPham, TenSanPham, LoaiHang, GiaNhap, GiaBan, 
                                     SoLuongTon, DonViTinh, TrangThai, NgayTao, NgayCapNhat)
                                    VALUES 
                                    (@MaSanPham, @TenSanPham, @LoaiHang, @GiaNhap, @GiaBan, 
                                     @SoLuongTon, @DonViTinh, 1, GETDATE(), GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaSanPham", txtMaSanPham.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenSanPham", txtTenSanPham.Text.Trim());
                    cmd.Parameters.AddWithValue("@LoaiHang", txtLoaiHang.Text.Trim());
                    cmd.Parameters.AddWithValue("@GiaNhap", decimal.Parse(txtGiaNhap.Text));
                    cmd.Parameters.AddWithValue("@GiaBan", decimal.Parse(txtGiaBan.Text));
                    cmd.Parameters.AddWithValue("@SoLuongTon", int.Parse(txtSoLuong.Text));
                    cmd.Parameters.AddWithValue("@DonViTinh", cboDonViTinh.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Xóa form
        private void ClearForm()
        {
            GenerateProductCode();
            txtTenSanPham?.Clear();
            txtLoaiHang?.Clear();
            txtGiaNhap?.Clear();
            txtGiaBan?.Clear();
            txtSoLuong?.Clear();

            if (cboDonViTinh != null && cboDonViTinh.Items.Count > 0)
                cboDonViTinh.SelectedIndex = 0;

            if (dtpNgayNhap != null)
                dtpNgayNhap.Value = DateTime.Now;
        }

        // Các sự kiện TextChanged và SelectedIndexChanged
        private void txtMaSanPham_TextChanged(object sender, EventArgs e) { }
        private void txtTenSanPham_TextChanged(object sender, EventArgs e) { }
        private void txtLoaiHang_TextChanged(object sender, EventArgs e) { }
        private void dtpNgayNhap_ValueChanged(object sender, EventArgs e) { }
        private void cboDonViTinh_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtGiaBan_TextChanged(object sender, EventArgs e) { }
        private void cboNCC_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtSoLuong_TextChanged(object sender, EventArgs e) { }
        private void txtGiaNhap_TextChanged(object sender, EventArgs e) { }
    }

    // Class hỗ trợ cho ComboBox
    public class ComboBoxItem
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

/*
===========================================
HƯỚNG DẪN SỬ DỤNG FILE HOÀN CHỈNH
===========================================

✅ CÁC CONTROL CẦN CÓ TRONG FORM:
   - txtMaSanPham (TextBox) - Mã sản phẩm
   - txtTenSanPham (TextBox) - Tên sản phẩm
   - txtLoaiHang (TextBox) - Loại hàng
   - txtGiaNhap (TextBox) - Giá nhập
   - txtGiaBan (TextBox) - Giá bán
   - txtSoLuong (TextBox) - Số lượng
   - cboDonViTinh (ComboBox) - Đơn vị tính
   - cboNCC (ComboBox) - Nhà cung cấp
   - dtpNgayNhap (DateTimePicker) - Ngày nhập
   - btnNhapHang (Button) - Nút nhập hàng

✅ CHỨC NĂNG:
   - Tự động tạo mã sản phẩm (SP001, SP002,...)
   - Mã sản phẩm bị khóa, không cho sửa
   - Load danh sách đơn vị tính
   - Load danh sách nhà cung cấp từ bảng NhaCungCap
   - Validate đầy đủ trước khi thêm
   - Kiểm tra trùng mã sản phẩm
   - Thêm sản phẩm vào bảng Kho
   - Tự động đóng form sau khi thêm thành công

✅ CẤU TRÚC DATABASE ĐÃ ĐỒNG BỘ:
   - Bảng Kho: MaSanPham, TenSanPham, LoaiHang, GiaNhap, GiaBan, 
               SoLuongTon, DonViTinh, TrangThai, NgayTao, NgayCapNhat
   - Bảng NhaCungCap: MaNhaCungCap, TenNhaCungCap, SoDienThoai, 
                      Email, DiaChi, LoaiHangCungCap, GhiChu

✅ CÁCH SỬ DỤNG:
   1. Copy toàn bộ code này
   2. Paste vào file frmNhkho.cs trong Visual Studio
   3. Save (Ctrl + S)
   4. Build → Rebuild Solution
   5. Chạy (F5)

✅ GỌI TỪ FORM KHO:
   frmNhkho frm = new frmNhkho();
   if (frm.ShowDialog() == DialogResult.OK)
   {
       LoadKhoData(); // Refresh dữ liệu
   }

✅ LƯU Ý:
   - Connection string đã được set đúng theo máy của bạn
   - Code đã loại bỏ cột MoTa vì không có trong form
   - Đã đồng bộ tên cột database: MaNhaCungCap, TenNhaCungCap
   - Có fallback data nếu không kết nối được database
*/