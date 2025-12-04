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
    public partial class Form10 : Form
    {
        // Biến theo dõi trạng thái hiển thị mật khẩu cho 3 textbox
        private bool isOldPasswordVisible = false;
        private bool isNewPasswordVisible = false;
        private bool isConfirmPasswordVisible = false;

        public Form10()
        {
            InitializeComponent();
            InitializePasswordControls();
        }

        /// <summary>
        /// Khởi tạo các control mật khẩu
        /// </summary>
        private void InitializePasswordControls()
        {
            // Đặt PasswordChar cho các textbox mật khẩu
            txtPassword.PasswordChar = '●';
            txtNewPass.PasswordChar = '●';
            txtNewPassXn.PasswordChar = '●';

            // Xóa viền của các panel (nếu có)
            pnlShowOldPass.BorderStyle = BorderStyle.None;
            pnlShowNewPass.BorderStyle = BorderStyle.None;
            pnlShowConfirmPass.BorderStyle = BorderStyle.None;

            // Thiết lập cursor cho các panel show password
            pnlShowOldPass.Cursor = Cursors.Hand;
            pnlShowNewPass.Cursor = Cursors.Hand;
            pnlShowConfirmPass.Cursor = Cursors.Hand;
        }

        private void txtdangnhap_TextChanged(object sender, EventArgs e)
        {
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            EnterToTab(this);
        }
        private void EnterToTab(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                c.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        e.SuppressKeyPress = true; // chặn tiếng "ding"
                        this.SelectNextControl((Control)s, true, true, true, true);
                    }
                };

                // Nếu control có chứa control con (GroupBox, Panel…), duyệt tiếp
                if (c.HasChildren)
                {
                    EnterToTab(c);
                }
            }
        }


        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtAccount.Text.Trim();
            string oldPass = txtPassword.Text.Trim();
            string newPass = txtNewPass.Text.Trim();
            string confirm = txtNewPassXn.Text.Trim();

            if (username == "" || oldPass == "" || newPass == "" || confirm == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Kiểm tra độ dài mật khẩu mới
            if (newPass.Length < 8)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 8 ký tự!");
                return;
            }

            if (newPass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            using (SqlConnection conn = Connection.connect())
            {
                conn.Open();

                // Kiểm tra mật khẩu cũ
                string sqlCheck = "SELECT COUNT(*) FROM [User] WHERE TenDangNhap=@u AND MatKhau=@o";
                SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn);
                cmdCheck.Parameters.AddWithValue("@u", username);
                cmdCheck.Parameters.AddWithValue("@o", oldPass);
                int check = (int)cmdCheck.ExecuteScalar();

                if (check == 0)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu cũ không đúng!");
                    return;
                }

                // Update mật khẩu
                string sqlUpdate = "UPDATE [User] SET MatKhau=@n WHERE TenDangNhap=@u";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn);
                cmdUpdate.Parameters.AddWithValue("@n", newPass);
                cmdUpdate.Parameters.AddWithValue("@u", username);
                cmdUpdate.ExecuteNonQuery();

                MessageBox.Show("Đổi mật khẩu thành công!");
                this.Close();
            }
        }

        #region Xử lý hiển thị/ẩn mật khẩu cũ

        /// <summary>
        /// Vẽ icon show/hide cho mật khẩu cũ
        /// </summary>
        private void pnlShowOldPass_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int centerX = panel.Width / 2;
                int centerY = panel.Height / 2;

                if (isOldPasswordVisible)
                {
                    DrawOpenEye(g, centerX, centerY);
                }
                else
                {
                    DrawClosedEye(g, centerX, centerY);
                }
            }
        }

        /// <summary>
        /// Click để hiện/ẩn mật khẩu cũ
        /// </summary>
        private void pnlShowOldPass_Click(object sender, EventArgs e)
        {
            isOldPasswordVisible = !isOldPasswordVisible;
            txtPassword.PasswordChar = isOldPasswordVisible ? '\0' : '●';
            ((Panel)sender).Invalidate();
        }

        private void pnlShowOldPass_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.FromArgb(240, 240, 240);
        }

        private void pnlShowOldPass_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.Transparent;
        }

        #endregion

        #region Xử lý hiển thị/ẩn mật khẩu mới

        /// <summary>
        /// Vẽ icon show/hide cho mật khẩu mới
        /// </summary>
        private void pnlShowNewPass_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int centerX = panel.Width / 2;
                int centerY = panel.Height / 2;

                if (isNewPasswordVisible)
                {
                    DrawOpenEye(g, centerX, centerY);
                }
                else
                {
                    DrawClosedEye(g, centerX, centerY);
                }
            }
        }

        /// <summary>
        /// Click để hiện/ẩn mật khẩu mới
        /// </summary>
        private void pnlShowNewPass_Click(object sender, EventArgs e)
        {
            isNewPasswordVisible = !isNewPasswordVisible;
            txtNewPass.PasswordChar = isNewPasswordVisible ? '\0' : '●';
            ((Panel)sender).Invalidate();
        }

        private void pnlShowNewPass_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.FromArgb(240, 240, 240);
        }

        private void pnlShowNewPass_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.Transparent;
        }

        #endregion

        #region Xử lý hiển thị/ẩn xác nhận mật khẩu mới

        /// <summary>
        /// Vẽ icon show/hide cho xác nhận mật khẩu
        /// </summary>
        private void pnlShowConfirmPass_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel != null)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int centerX = panel.Width / 2;
                int centerY = panel.Height / 2;

                if (isConfirmPasswordVisible)
                {
                    DrawOpenEye(g, centerX, centerY);
                }
                else
                {
                    DrawClosedEye(g, centerX, centerY);
                }
            }
        }

        /// <summary>
        /// Click để hiện/ẩn xác nhận mật khẩu
        /// </summary>
        private void pnlShowConfirmPass_Click(object sender, EventArgs e)
        {
            isConfirmPasswordVisible = !isConfirmPasswordVisible;
            txtNewPassXn.PasswordChar = isConfirmPasswordVisible ? '\0' : '●';
            ((Panel)sender).Invalidate();
        }

        private void pnlShowConfirmPass_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.FromArgb(240, 240, 240);
        }

        private void pnlShowConfirmPass_MouseLeave(object sender, EventArgs e)
        {
            ((Panel)sender).BackColor = Color.Transparent;
        }

        #endregion

        #region Vẽ icon mắt

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

        #endregion

        private void btnSave_Paint(object sender, PaintEventArgs e)
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