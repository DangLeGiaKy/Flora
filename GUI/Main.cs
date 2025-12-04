using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test.GUI
{
    public partial class Main : Form

    {
        private Button currentButton = null;
        public Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Gọi hàm load thông tin user khi khởi tạo form
            LoadUserInfo();

            // Kiểm tra và áp dụng quyền truy cập
            ApplyRolePermissions();

            // Đăng ký sự kiện FormClosing
            

            this.FormClosing += Main_FormClosing;
        }
        
        private void HighlightButton(Button btn)
        {
            // Reset màu nút cũ
            if (currentButton != null)
            {
                currentButton.BackColor = Color.White;
                currentButton.ForeColor = Color.Black;
                
            }

            // Đổi màu nút được chọn
            btn.BackColor = Color.FromArgb(231, 255, 219);    
            btn.ForeColor = Color.Black;
            

            currentButton = btn;
        }


        /// <summary>
        /// Load và hiển thị thông tin người dùng đang đăng nhập
        /// </summary>
        private void LoadUserInfo()
        {
            try
            {
                // Kiểm tra xem có người dùng đăng nhập hay không
                if (!UserSession.IsLoggedIn)
                {
                    MessageBox.Show("Phiên đăng nhập không hợp lệ!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Quay về màn hình đăng nhập
                    Login loginForm = new Login();
                    loginForm.Show();
                    this.Close();
                    return;
                }

                // Hiển thị thông tin người dùng (giả sử bạn có lblHoTen trên form)
                if (lblHoTen != null)
                {
                    lblHoTen.Text = $"👤 {UserSession.HoTen}";
                    lblHoTen.AutoSize = true;

                    // Màu + font đẹp
                    lblHoTen.ForeColor = Color.FromArgb(15, 15, 15);
                    lblHoTen.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                    // Đặt vào góc phải
                    lblHoTen.Location = new Point(this.Width - lblHoTen.Width - 15, 10);




                }

                // Nếu có thêm label hiển thị vai trò
                // lblVaiTro.Text = $"Vai trò: {UserSession.VaiTro}";

                // Hiển thị thông tin trên title bar
                this.Text = $"Hệ thống quản lý - {UserSession.HoTen} ({UserSession.VaiTro})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load thông tin người dùng:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Áp dụng phân quyền theo vai trò người dùng
        /// </summary>
        private void ApplyRolePermissions()
        {
            try
            {
                if (UserSession.VaiTro == "Admin")
                {
                    // Admin có quyền truy cập tất cả
                    // Tất cả các button đều visible
                    if (btnbanhang != null) btnbanhang.Enabled = true;
                    if (btndonhang != null) btndonhang.Enabled = true;
                    if (btnkhachhang != null) btnkhachhang.Enabled = true;
                    if (ntmsanpham != null) ntmsanpham.Enabled = true;
                    if (btnncc != null) btnncc.Enabled = true;
                    if (btntk != null) btntk.Enabled = true;
                    if (btnbc != null) btnbc.Enabled = true;
                }
                else if (UserSession.VaiTro == "Quản lý")
                {
                    // Quản lý có quyền xem hầu hết, trừ một số chức năng
                    if (btnbanhang != null) btnbanhang.Enabled = true;
                    if (btndonhang != null) btndonhang.Enabled = true;
                    if (btnkhachhang != null) btnkhachhang.Enabled = true;
                    if (ntmsanpham != null) ntmsanpham.Enabled = true;
                    if (btnncc != null) btnncc.Enabled = true;
                    if (btntk != null) btntk.Enabled = false; // Không quản lý tài khoản
                    if (btnbc != null) btnbc.Enabled = true;
                }
                else // Nhân viên
                {
                    // Nhân viên chỉ có quyền truy cập các chức năng cơ bản
                    if (btnbanhang != null) btnbanhang.Enabled = true;
                    if (btndonhang != null) btndonhang.Enabled = true;
                    if (btnkhachhang != null) btnkhachhang.Enabled = true;
                    if (ntmsanpham != null) ntmsanpham.Enabled = true;
                    if (btnncc != null) btnncc.Enabled = false;
                    if (btntk != null) btntk.Enabled = false;
                    if (btnbc != null) btnbc.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi áp dụng phân quyền:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Hàm helper để load form con vào panel
        /// </summary>
        private void LoadFormIntoPanel(Form childForm)
        {
            try
            {
                // Xóa các control cũ trong panel
                this.pnlFormLoader.Controls.Clear();

                // Cấu hình form con
                childForm.Dock = DockStyle.Fill;
                childForm.TopLevel = false;
                childForm.TopMost = true;
                childForm.FormBorderStyle = FormBorderStyle.None;

                // Thêm form vào panel và hiển thị
                this.pnlFormLoader.Controls.Add(childForm);
                childForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải form:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Xác nhận đăng xuất
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn đăng xuất?\n\nNgười dùng: {UserSession.HoTen}",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Xóa thông tin session
                UserSession.Clear();

                // Mở form đăng nhập
                Login f = new Login();
                f.Show();

                // Đóng form Main
                this.Close();

            }
        }

        private void btnbanhang_Click(object sender, EventArgs e)
        {
            HighlightButton(btnbanhang);
            Form1 frmBanhang = new Form1();
            LoadFormIntoPanel(frmBanhang);
        }

        private void btndonhang_Click(object sender, EventArgs e)
        {
            HighlightButton(btndonhang);
            frmQuanLyDonHang frmdonhang = new frmQuanLyDonHang();
            LoadFormIntoPanel(frmdonhang);
        }

        private void pnlFormLoader_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            HighlightButton(btnkhachhang);
            Form4 frmkhachhang = new Form4();
            LoadFormIntoPanel(frmkhachhang);
        }

        private void ntmsanpham_Click(object sender, EventArgs e)
        {
            HighlightButton(ntmsanpham);
            Kho frmsp = new Kho();
            LoadFormIntoPanel(frmsp);
        }

        private void btnncc_Click(object sender, EventArgs e)
        {
            HighlightButton(btnncc);
            // Kiểm tra quyền trước khi mở
            if (!UserSession.IsQuanLy())
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form7 frmncc = new Form7();
            LoadFormIntoPanel(frmncc);
        }

        private void btntk_Click(object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin
            HighlightButton(btntk);
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show("Chỉ Admin mới có quyền quản lý tài khoản!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Load form quản lý tài khoản (cần tạo form này)
            frmQuanLyUser frmsp = new frmQuanLyUser();
            LoadFormIntoPanel(frmsp);
        }

        private void btnbc_Click(object sender, EventArgs e)
        {
            // Kiểm tra quyền
            HighlightButton(btnbc);
            if (!UserSession.IsQuanLy())
            {
                MessageBox.Show("Bạn không có quyền xem báo cáo!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form9 frmbc = new Form9();
            LoadFormIntoPanel(frmbc);
        }

        private void lblHoTen_Click(object sender, EventArgs e)
        {
            // Hiển thị thông tin chi tiết người dùng khi click vào tên
            string userInfo = $"Thông tin tài khoản:\n\n" +
                            $"Mã User: {UserSession.MaUser}\n" +
                            $"Tên đăng nhập: {UserSession.TenDangNhap}\n" +
                            $"Họ tên: {UserSession.HoTen}\n" +
                            $"Vai trò: {UserSession.VaiTro}\n" +
                            $"Email: {UserSession.Email}\n" +
                            $"Số điện thoại: {UserSession.SoDienThoai}\n" +
                            $"Thời gian đăng nhập: {UserSession.NgayDangNhap:dd/MM/yyyy HH:mm:ss}";

            MessageBox.Show(userInfo, "Thông tin tài khoản",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Xử lý khi đóng form
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Nếu đang có user đăng nhập và đóng form Main
            if (UserSession.IsLoggedIn && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có muốn đăng xuất khỏi hệ thống?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    UserSession.Clear();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btndangxuat_Paint(object sender, PaintEventArgs e)
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