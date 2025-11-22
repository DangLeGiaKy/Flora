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
    public partial class Kho : Form
    {
        // Chuỗi kết nối - Thay đổi theo cấu hình của bạn
        private string connectionString = "Server=khanhvinh\\SQLEXPRESS;Database=FloraShopDB;Integrated Security=True;";

        public Kho()
        {
            InitializeComponent();
        }

        private void Kho_Load(object sender, EventArgs e)
        {
            // Load dữ liệu khi form mở
            LoadKhoData();

            // Load danh sách loại hàng vào ComboBox (nếu có)
            LoadLoaiHangToComboBox();
        }

        // Load dữ liệu từ database lên DataGridView
        private void LoadKhoData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                                        MaSanPham,
                                        TenSanPham,
                                        LoaiHang,
                                        GiaNhap,
                                        GiaBan,
                                        SoLuongTon,
                                        DonViTinh,
                                        CASE WHEN TrangThai = 1 THEN N'Hoạt động' ELSE N'Ngừng bán' END AS TrangThai,
                                        NgayTao,
                                        NgayCapNhat
                                     FROM Kho
                                     ORDER BY NgayCapNhat DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvTable.DataSource = dataTable;

                    CustomizeDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Tùy chỉnh hiển thị DataGridView
        private void CustomizeDataGridView()
        {
            if (dgvTable.Columns.Count == 0)
                return;

            // Đổi tên cột
            dgvTable.Columns["MaSanPham"].HeaderText = "Mã SP";
            dgvTable.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvTable.Columns["LoaiHang"].HeaderText = "Loại Hàng";
            dgvTable.Columns["GiaNhap"].HeaderText = "Giá Nhập";
            dgvTable.Columns["GiaBan"].HeaderText = "Giá Bán";
            dgvTable.Columns["SoLuongTon"].HeaderText = "Số Lượng";
            dgvTable.Columns["DonViTinh"].HeaderText = "Đơn Vị";
            dgvTable.Columns["TrangThai"].HeaderText = "Trạng Thái";
            dgvTable.Columns["NgayTao"].HeaderText = "Ngày Tạo";
            dgvTable.Columns["NgayCapNhat"].HeaderText = "Cập Nhật";

            // Định dạng giá tiền
            dgvTable.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
            dgvTable.Columns["GiaNhap"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvTable.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
            dgvTable.Columns["GiaBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Định dạng số lượng
            dgvTable.Columns["SoLuongTon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Định dạng ngày tháng
            dgvTable.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvTable.Columns["NgayCapNhat"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // Cài đặt chung
            dgvTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTable.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTable.MultiSelect = false;
            dgvTable.ReadOnly = true;
            dgvTable.AllowUserToAddRows = false;
            dgvTable.RowHeadersVisible = false;
        }

        // Load danh sách loại hàng vào ComboBox
        private void LoadLoaiHangToComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT DISTINCT LoaiHang FROM Kho ORDER BY LoaiHang";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    cboSearch.Items.Clear();
                    cboSearch.Items.Add("Tất cả");

                    while (reader.Read())
                    {
                        cboSearch.Items.Add(reader["LoaiHang"].ToString());
                    }

                    cboSearch.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load loại hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi chọn loại hàng trong ComboBox (Tìm kiếm/Lọc)
        private void cboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboSearch.SelectedItem == null)
                    return;

                string loaiHang = cboSearch.SelectedItem.ToString();

                // Nếu chọn "Tất cả" thì load toàn bộ
                if (loaiHang == "Tất cả")
                {
                    LoadKhoData();
                    return;
                }

                // Lọc theo loại hàng
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"SELECT 
                                        MaSanPham,
                                        TenSanPham,
                                        LoaiHang,
                                        GiaNhap,
                                        GiaBan,
                                        SoLuongTon,
                                        DonViTinh,
                                        CASE WHEN TrangThai = 1 THEN N'Hoạt động' ELSE N'Ngừng bán' END AS TrangThai,
                                        NgayTao,
                                        NgayCapNhat
                                     FROM Kho
                                     WHERE LoaiHang = @LoaiHang
                                     ORDER BY MaSanPham ASC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@LoaiHang", loaiHang);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvTable.DataSource = dataTable;
                    CustomizeDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button Thêm sản phẩm
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mở form nhập kho để thêm sản phẩm mới
                frmNhkho frmnk = new frmNhkho();
                frmnk.ShowDialog();

                // Refresh lại dữ liệu sau khi đóng form
                LoadKhoData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Button Xóa sản phẩm
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sản phẩm chưa
                if (string.IsNullOrEmpty(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm từ danh sách để xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maSP = txtMaSP.Text;
                string tenSP = txtTenSP.Text;

                // Xác nhận xóa
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa sản phẩm:\n\nMã: {maSP}\nTên: {tenSP}",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = "DELETE FROM Kho WHERE MaSanPham = @MaSanPham";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaSanPham", maSP);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Xóa thông tin trên các control
                            ClearControls();

                            // Refresh DataGridView
                            LoadKhoData();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm cần xóa!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện click vào cell trong DataGridView
        private void dgvTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra click vào header
            if (e.RowIndex < 0)
                return;

            // Hiển thị thông tin sản phẩm lên các control
            ShowProductInfo(e.RowIndex);

            // Khóa tất cả control (chế độ xem)
            LockControls();
        }

        // Hiển thị thông tin sản phẩm lên các TextBox và ComboBox
        private void ShowProductInfo(int rowIndex)
        {
            try
            {
                DataGridViewRow row = dgvTable.Rows[rowIndex];

                txtMaSP.Text = row.Cells["MaSanPham"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSanPham"].Value.ToString();
                cboLoaiHang.Text = row.Cells["LoaiHang"].Value.ToString();
                txtGiaNhap.Text = row.Cells["GiaNhap"].Value.ToString();
                txtGiaBan.Text = row.Cells["GiaBan"].Value.ToString();
                txtSoLuongTon.Text = row.Cells["SoLuongTon"].Value.ToString();
                cboDonViTinh.Text = row.Cells["DonViTinh"].Value.ToString();

                // Nếu có DateTimePicker cho NgayCapNhat
                if (row.Cells["NgayCapNhat"].Value != null && row.Cells["NgayCapNhat"].Value != DBNull.Value)
                {
                    dateTimePicker1.Value = Convert.ToDateTime(row.Cells["NgayCapNhat"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Khóa tất cả control (chế độ xem)
        private void LockControls()
        {
            txtMaSP.Enabled = false;      // Luôn luôn khóa
            txtTenSP.Enabled = false;
            cboLoaiHang.Enabled = false;
            txtGiaNhap.Enabled = false;
            txtGiaBan.Enabled = false;
            txtSoLuongTon.Enabled = false;
            cboDonViTinh.Enabled = false;
            dateTimePicker1.Enabled = false;

            // Đổi màu nền để người dùng biết đang bị khóa
            txtMaSP.BackColor = SystemColors.Control;
            txtTenSP.BackColor = SystemColors.Control;
            cboLoaiHang.BackColor = SystemColors.Control;
            txtGiaNhap.BackColor = SystemColors.Control;
            txtGiaBan.BackColor = SystemColors.Control;
            txtSoLuongTon.BackColor = SystemColors.Control;
            cboDonViTinh.BackColor = SystemColors.Control;
            dateTimePicker1.BackColor = SystemColors.Control;
        }

        // Mở khóa control để sửa (trừ txtMaSP)
        private void UnlockControls()
        {
            txtMaSP.ReadOnly = false;      // Luôn luôn khóa
            txtTenSP.Enabled = true;
            cboLoaiHang.Enabled = true;
            txtGiaNhap.Enabled = true;
            txtGiaBan.Enabled = true;
            txtSoLuongTon.Enabled = true;
            cboDonViTinh.Enabled = true;
            dateTimePicker1.Enabled = true;

            // Đổi màu nền khi được mở khóa
            txtMaSP.BackColor = SystemColors.Control;  // Vẫn giữ màu khóa
            txtTenSP.BackColor = Color.White;
            txtGiaNhap.BackColor = Color.White;
            txtGiaBan.BackColor = Color.White;
            txtSoLuongTon.BackColor = Color.White;
        }

        // Button Sửa - Mở khóa các control
        private void btnRepair_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sản phẩm chưa
                if (string.IsNullOrEmpty(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm từ danh sách để sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở khóa các control để cho phép sửa
                UnlockControls();

                MessageBox.Show("Bạn có thể sửa thông tin sản phẩm.\nNhấn 'Lưu' để cập nhật!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Xóa thông tin trên các control
        private void ClearControls()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiHang.SelectedIndex = -1;
            txtGiaNhap.Clear();
            txtGiaBan.Clear();
            txtSoLuongTon.Clear();
            cboDonViTinh.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
        }

        // Các sự kiện TextChanged và SelectedIndexChanged (không cần xử lý gì)
        private void txtMaSP_TextChanged(object sender, EventArgs e) { }
        private void txtTenSP_TextChanged(object sender, EventArgs e) { }
        private void cboLoaiHang_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtGiaNhap_TextChanged(object sender, EventArgs e) { }
        private void txtSoLuongTon_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void cboDonViTinh_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtGiaBan_TextChanged(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenSP.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenSP.Focus();
                    return;
                }

                // Kiểm tra giá nhập
                if (!decimal.TryParse(txtGiaNhap.Text, out decimal giaNhap) || giaNhap < 0)
                {
                    MessageBox.Show("Giá nhập không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaNhap.Focus();
                    return;
                }

                // Kiểm tra giá bán
                if (!decimal.TryParse(txtGiaBan.Text, out decimal giaBan) || giaBan < 0)
                {
                    MessageBox.Show("Giá bán không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtGiaBan.Focus();
                    return;
                }

                // Kiểm tra số lượng
                if (!int.TryParse(txtSoLuongTon.Text, out int soLuong) || soLuong < 0)
                {
                    MessageBox.Show("Số lượng tồn không hợp lệ!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuongTon.Focus();
                    return;
                }

                // Xác nhận lưu
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn lưu thay đổi?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Cập nhật vào database
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string query = @"UPDATE Kho SET 
                                            TenSanPham = @TenSanPham,
                                            LoaiHang = @LoaiHang,
                                            GiaNhap = @GiaNhap,
                                            GiaBan = @GiaBan,
                                            SoLuongTon = @SoLuongTon,
                                            DonViTinh = @DonViTinh,
                                            NgayCapNhat = GETDATE()
                                         WHERE MaSanPham = @MaSanPham";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MaSanPham", txtMaSP.Text);
                        cmd.Parameters.AddWithValue("@TenSanPham", txtTenSP.Text);
                        cmd.Parameters.AddWithValue("@LoaiHang", cboLoaiHang.Text);
                        cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                        cmd.Parameters.AddWithValue("@GiaBan", giaBan);
                        cmd.Parameters.AddWithValue("@SoLuongTon", soLuong);
                        cmd.Parameters.AddWithValue("@DonViTinh", cboDonViTinh.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Khóa lại các control
                            LockControls();

                            // Refresh lại DataGridView
                            LoadKhoData();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sản phẩm để cập nhật!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

/*
HƯỚNG DẪN SỬ DỤNG:

1. ✅ Thay đổi connection string (dòng 17):
   - YOUR_SERVER: tên server SQL (vd: localhost, .\SQLEXPRESS)
   - YOUR_DATABASE: tên database của bạn

2. ✅ Các control cần có trong Form:
   - dgvTable: DataGridView để hiển thị dữ liệu
   - cboSearch: ComboBox để lọc theo loại hàng
   - btnAdd: Button thêm sản phẩm
   - btnRepair: Button sửa sản phẩm
   - btnRemove: Button xóa sản phẩm

3. ✅ Chức năng đã hoàn thiện:
   - Load dữ liệu khi mở form
   - Lọc theo loại hàng (ComboBox)
   - Thêm sản phẩm mới
   - Sửa sản phẩm (cần chọn dòng trước)
   - Xóa sản phẩm (cần chọn dòng trước)
   - Định dạng giá tiền, ngày tháng

4. ⚠️ Lưu ý:
   - Form frmNhkho cần có 2 constructor:
     + public frmNhkho() : Thêm mới
     + public frmNhkho(string maSP) : Sửa sản phẩm
*/