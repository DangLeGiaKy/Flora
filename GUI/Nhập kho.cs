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
        // Chuỗi kết nối - QUAN TRỌNG: Thay đổi theo cấu hình của bạn
        private string connectionString = "Server=khanhvinh\\SQLEXPRESS;Database=FloraShopDB;Integrated Security=True;";

        public frmNhkho()
        {
            InitializeComponent();

            // Đăng ký sự kiện Load
            this.Load += frmNhkho_Load;
        }

        private void frmNhkho_Load(object sender, EventArgs e)
        {
            try
            {
                // Test message
                MessageBox.Show("Form đã load!", "Test");

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

        // Load nhà cung cấp
        private void LoadNhaCungCap()
        {
            try
            {
                if (cboNCC == null) return;

                cboNCC.Items.Clear();

                // Thử load từ database
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT MaNCC, TenNCC FROM NhaCungCap ORDER BY TenNCC";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            cboNCC.Items.Add(reader["TenNCC"].ToString());
                        }
                    }
                }
                catch
                {
                    // Nếu không có bảng hoặc lỗi kết nối, dùng dữ liệu mẫu
                    string[] suppliers = { "Nhà cung cấp A", "Nhà cung cấp B",
                                          "Nhà cung cấp C", "Nhà cung cấp D" };
                    cboNCC.Items.AddRange(suppliers);
                }

                if (cboNCC.Items.Count > 0)
                {
                    cboNCC.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhà cung cấp: " + ex.Message);
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
                // Nếu lỗi database thì tạo mã tạm
                txtMaSanPham.Text = "SP001";
                MessageBox.Show("Không thể kết nối database để tạo mã tự động. Dùng mã mặc định SP001.\n\nLỗi: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Button Nhập Hàng - QUAN TRỌNG
        private void btnNhapHang_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Button Nhập Hàng được click!", "Test");

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
                MessageBox.Show("Lỗi: " + ex.Message + "\n\nStack: " + ex.StackTrace, "Lỗi",
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

                    // Insert
                    string query = @"INSERT INTO Kho 
                                    (MaSanPham, TenSanPham, LoaiHang, GiaNhap, GiaBan, 
                                     SoLuongTon, DonViTinh, MoTa, TrangThai, NgayTao, NgayCapNhat)
                                    VALUES 
                                    (@MaSanPham, @TenSanPham, @LoaiHang, @GiaNhap, @GiaBan, 
                                     @SoLuongTon, @DonViTinh, @MoTa, 1, GETDATE(), GETDATE())";

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
                MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message + "\n\nChi tiết: " + ex.StackTrace,
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

        // Các sự kiện (có thể để trống)
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
}

/*
HƯỚNG DẪN KHẮC PHỤC LỖI:

1. ✅ THAY CONNECTION STRING (dòng 17):
   Ví dụ: "Server=localhost;Database=QuanLyKho;Integrated Security=True;"

2. ✅ KIỂM TRA TÊN CÁC CONTROL trong Properties:
   - txtMaSanPham
   - txtTenSanPham
   - txtLoaiHang
   - txtGiaNhap
   - txtGiaBan
   - txtSoLuong
   - txtMoTa
   - cboDonViTinh
   - cboNCC (ĐÃ ĐỒNG BỘ)
   - dtpNgayNhap
   - btnNhapHang

3. ✅ KIỂM TRA SỰ KIỆN btnNhapHang:
   - Click vào btnNhapHang trong Designer
   - Vào Properties → Events (⚡)
   - Tìm Click event
   - Đảm bảo có "btnNhapHang_Click"

4. ✅ NẾU VẪN LỖI, THỬ:
   - Build lại project (Ctrl + Shift + B)
   - Clean Solution → Rebuild Solution
   - Đóng Visual Studio và mở lại

5. ✅ TEST TỪNG BƯỚC:
   - Khi mở form, có hiện "Form đã load!" không?
   - Khi click btnNhapHang, có hiện "Button Nhập Hàng được click!" không?
   - Nếu không hiện → Sự kiện chưa được gắn đúng

6. ✅ NẾU CÓ LỖI VỀ NULL:
   - Code đã có kiểm tra null (? operator)
   - Nhưng đảm bảo tất cả control đều có tên đúng
*/