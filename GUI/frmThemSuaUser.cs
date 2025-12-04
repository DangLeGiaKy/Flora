using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.BUS;
using test.DTO;
using static test.GUI.frmQuanLyUser;

namespace test.GUI
{
    public partial class frmThemSuaUser : Form
    {
        private QuanLyUser userBUS = new QuanLyUser();
        private FormMode mode;
        private string maUser;

        public frmThemSuaUser()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmThemSuaUser_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        public frmThemSuaUser(FormMode mode, string maUser)
        {
            InitializeComponent();
            this.mode = mode;
            this.maUser = maUser;

            InitializeForm();
        }

        // Khởi tạo form
        private void InitializeForm()
        {
            // Cấu hình ComboBox VaiTro
            cboVaiTro.Items.Clear();
            cboVaiTro.Items.Add("Nhân viên");
            cboVaiTro.Items.Add("Quản lý");
            cboVaiTro.Items.Add("Admin");
            cboVaiTro.SelectedIndex = 0;

            if (mode == FormMode.Them)
            {
                this.Text = "Thêm tài khoản mới";
                txtMaUser.Text = userBUS.GenerateMaUser();
                txtMaUser.ReadOnly = true;
                chkTrangThai.Checked = true;

                // Hiện các trường mật khẩu
                lblMatKhau.Visible = true;
                txtMatKhau.Visible = true;
                lblXacNhanMatKhau.Visible = true;
                txtXacNhanMatKhau.Visible = true;
            }
            else // Chế độ Sửa
            {
                this.Text = "Sửa thông tin tài khoản";
                txtMaUser.ReadOnly = true;
                txtTenDangNhap.ReadOnly = true;

                // Ẩn các trường mật khẩu
                lblMatKhau.Visible = true;
                txtMatKhau.ReadOnly = true;
                lblXacNhanMatKhau.Visible = true;
                txtXacNhanMatKhau.ReadOnly = true;

                LoadUserData();
            }
        }

        // Load dữ liệu user khi sửa
        private void LoadUserData()
        {
            try
            {
                UserDTO user = userBUS.GetUserById(maUser);
                if (user != null)
                {
                    txtMaUser.Text = user.MaUser;
                    txtTenDangNhap.Text = user.TenDangNhap;
                    txtMatKhau.Text = user.MatKhau;
                    txtHoTen.Text = user.HoTen;
                    txtEmail.Text = user.Email;
                    txtSoDienThoai.Text = user.SoDienThoai;
                    cboVaiTro.Text = user.VaiTro;
                    chkTrangThai.Checked = user.TrangThai;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        // Nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input cơ bản
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }

                if (mode == FormMode.Them)
                {
                    // Validate mật khẩu khi thêm mới
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        return;
                    }

                    if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtXacNhanMatKhau.Focus();
                        return;
                    }

                    // Tạo user mới
                    UserDTO user = new UserDTO
                    {
                        MaUser = txtMaUser.Text.Trim(),
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau = txtMatKhau.Text,
                        HoTen = txtHoTen.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        VaiTro = cboVaiTro.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    if (userBUS.AddUser(user))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else // Chế độ Sửa
                {
                    UserDTO user = new UserDTO
                    {
                        MaUser = txtMaUser.Text.Trim(),
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau=txtMatKhau.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        VaiTro = cboVaiTro.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    if (userBUS.UpdateUser(user))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Nút Hủy
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Chỉ cho nhập số vào SĐT
        private void txtSoDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate input cơ bản
                if (string.IsNullOrWhiteSpace(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHoTen.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenDangNhap.Focus();
                    return;
                }

                if (mode == FormMode.Them)
                {
                    // Validate mật khẩu khi thêm mới
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtMatKhau.Focus();
                        return;
                    }

                    if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                    {
                        MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtXacNhanMatKhau.Focus();
                        return;
                    }

                    // Tạo user mới
                    UserDTO user = new UserDTO
                    {
                        MaUser = txtMaUser.Text.Trim(),
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        MatKhau = txtMatKhau.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        VaiTro = cboVaiTro.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    if (userBUS.AddUser(user))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else // Chế độ Sửa
                {
                    UserDTO user = new UserDTO
                    {
                        MaUser = txtMaUser.Text.Trim(),
                        TenDangNhap = txtTenDangNhap.Text.Trim(),
                        HoTen = txtHoTen.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SoDienThoai = txtSoDienThoai.Text.Trim(),
                        VaiTro = cboVaiTro.Text,
                        TrangThai = chkTrangThai.Checked
                    };

                    if (userBUS.UpdateUser(user))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtMaUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtXacNhanMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLuu_Paint(object sender, PaintEventArgs e)
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

        private void btnHuy_Paint(object sender, PaintEventArgs e)
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
