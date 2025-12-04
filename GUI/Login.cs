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
    public partial class Login : Form
    {
        private bool isPasswordVisible = false; // Biến theo dõi trạng thái hiển thị mật khẩu

        public Login()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Xóa session cũ khi load form login
            UserSession.Clear();

            // Đặt focus vào ô tài khoản
            txtAccount.Focus();

            // Đặt password char cho ô mật khẩu
            txtPassword.PasswordChar = '●';

            // Xóa viền của panel (nếu có)
            pnlShowPassword.BorderStyle = BorderStyle.None;

            // Thiết lập cursor cho panel show password
            pnlShowPassword.Cursor = Cursors.Hand;
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtAccount.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAccount.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            string username = txtAccount.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                using (SqlConnection conn = Connection.connect())
                //using (SqlConnection conn = new SqlConnection("Data Source=khanhvinh\\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True"))
                //using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
                {

                    conn.Open();

                    // Query lấy thông tin user và kiểm tra trạng thái
                    string query = @"SELECT MaUser, TenDangNhap, HoTen, VaiTro, Email, SoDienThoai 
                                   FROM [User] 
                                   WHERE TenDangNhap=@user AND MatKhau=@pass AND TrangThai=1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Lưu thông tin user vào session
                        UserSession.MaUser = reader["MaUser"].ToString();
                        UserSession.TenDangNhap = reader["TenDangNhap"].ToString();
                        UserSession.HoTen = reader["HoTen"].ToString();
                        UserSession.VaiTro = reader["VaiTro"].ToString();
                        UserSession.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                        UserSession.SoDienThoai = reader["SoDienThoai"] != DBNull.Value ? reader["SoDienThoai"].ToString() : "";
                        UserSession.NgayDangNhap = DateTime.Now;

                        reader.Close();

                        // Cập nhật thời gian đăng nhập cuối
                        string updateQuery = "UPDATE [User] SET NgayCapNhat=@ngay WHERE MaUser=@ma";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@ngay", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@ma", UserSession.MaUser);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show($"Đăng nhập thành công!\nXin chào {UserSession.HoTen}",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Mở form Main
                        Main f = new Main();
                        f.Show();
                        this.Hide();

                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!\nHoặc tài khoản đã bị khóa.",
                            "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Xóa mật khẩu và focus lại
                        txtPassword.Clear();
                        txtAccount.Focus();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi không xác định:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbldoimk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form10 f = new Form10();
            f.ShowDialog();
        }

        /// <summary>
        /// Xử lý sự kiện Paint của panel show password
        /// </summary>
        private void pnlShowPassword_Paint(object sender, PaintEventArgs e)
        {
            // Vẽ icon con mắt
            Panel panel = sender as Panel;
            if (panel != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Vị trí trung tâm panel
                int centerX = panel.Width / 2;
                int centerY = panel.Height / 2;

                if (isPasswordVisible)
                {
                    // Vẽ icon con mắt MỞ (đang hiển thị mật khẩu)
                    DrawOpenEye(g, centerX, centerY);
                }
                else
                {
                    // Vẽ icon con mắt ĐÓNG (đang ẩn mật khẩu)
                    DrawClosedEye(g, centerX, centerY);
                }
            }
        }

        /// <summary>
        /// Vẽ icon con mắt mở
        /// </summary>
        private void DrawOpenEye(Graphics g, int centerX, int centerY)
        {
            Pen pen = new Pen(Color.Gray, 2);
            Brush brush = new SolidBrush(Color.Gray);

            // Vẽ viền mắt (hình oval)
            Rectangle eyeRect = new Rectangle(centerX - 12, centerY - 6, 24, 12);
            g.DrawEllipse(pen, eyeRect);

            // Vẽ con ngươi (hình tròn nhỏ ở giữa)
            Rectangle pupilRect = new Rectangle(centerX - 4, centerY - 4, 8, 8);
            g.FillEllipse(brush, pupilRect);

            pen.Dispose();
            brush.Dispose();
        }

        /// <summary>
        /// Vẽ icon con mắt đóng (có gạch chéo)
        /// </summary>
        private void DrawClosedEye(Graphics g, int centerX, int centerY)
        {
            Pen pen = new Pen(Color.Gray, 2);
            Brush brush = new SolidBrush(Color.Gray);

            // Vẽ viền mắt (hình oval)
            Rectangle eyeRect = new Rectangle(centerX - 12, centerY - 6, 24, 12);
            g.DrawEllipse(pen, eyeRect);

            // Vẽ con ngươi
            Rectangle pupilRect = new Rectangle(centerX - 4, centerY - 4, 8, 8);
            g.FillEllipse(brush, pupilRect);

            // Vẽ gạch chéo (dấu gạch qua mắt)
            g.DrawLine(pen, centerX - 15, centerY - 8, centerX + 15, centerY + 8);

            pen.Dispose();
            brush.Dispose();
        }

        /// <summary>
        /// Xử lý sự kiện Click của panel show password
        /// </summary>
        private void pnlShowPassword_Click(object sender, EventArgs e)
        {
            // Đảo trạng thái hiển thị mật khẩu
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                // Hiển thị mật khẩu
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                // Ẩn mật khẩu
                txtPassword.PasswordChar = '●';
            }

            // Vẽ lại panel để cập nhật icon
            pnlShowPassword.Invalidate();
        }

        /// <summary>
        /// Xử lý sự kiện MouseEnter - Khi chuột di chuyển vào panel
        /// </summary>
        private void pnlShowPassword_MouseEnter(object sender, EventArgs e)
        {
            pnlShowPassword.BackColor = Color.FromArgb(240, 240, 240); // Màu xám nhạt khi hover
        }

        /// <summary>
        /// Xử lý sự kiện MouseLeave - Khi chuột rời khỏi panel
        /// </summary>
        private void pnlShowPassword_MouseLeave(object sender, EventArgs e)
        {
            pnlShowPassword.BackColor = Color.Transparent; // Trở về màu trong suốt
        }

        private void txtAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void login_p(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Paint(object sender, PaintEventArgs e)
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

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}