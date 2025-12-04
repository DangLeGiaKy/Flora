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
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtpass.PasswordChar = chkShowPassword.Checked ? '\0' : '●';
        }

        private void txtpass_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkWinAuth_CheckedChanged(object sender, EventArgs e)
        {
            bool useSql = !chkWinAuth.Checked;
            txtid.Enabled = useSql;
            txtpass.Enabled = useSql;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServer.Text))
            {
                MessageBox.Show("Vui lòng nhập tên server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServer.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtdata.Text))
            {
                MessageBox.Show("Vui lòng nhập tên database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtdata.Focus();
                return;
            }

            if (!chkWinAuth.Checked) // nếu dùng SQL Authentication thì check thêm user/pass
            {
                if (string.IsNullOrWhiteSpace(txtid.Text))
                {
                    MessageBox.Show("Vui lòng nhập User ID!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtid.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtpass.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtpass.Focus();
                    return;
                }
            }

            // Lưu tạm thông tin
            Program.server = txtServer.Text.Trim();
            Program.database = txtdata.Text.Trim();
            Program.uid = txtid.Text.Trim();
            Program.password = txtpass.Text.Trim();
            Program.authen = chkWinAuth.Checked ? "windows" : "sql";

            try
            {
                using (SqlConnection conn = Connection.connect())
                {
                    conn.Open();
                    MessageBox.Show("Kết nối thành công!", "Thành công");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Kết nối thất bại!\nHãy kiểm tra lại thông tin.\n\nChi tiết lỗi:\n" + ex.Message,
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Không đóng form — cho nhập lại
            }
        }

        private void button1_Paint(object sender, PaintEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Nền nút mới
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(231, 255, 219))) // đổi màu nền
            {
                g.FillRectangle(brush, btn.ClientRectangle);
            }

            // Viền đậm
            int borderThickness = 3;
            using (Pen pen = new Pen(Color.Black, borderThickness))
            {
                g.DrawRectangle(pen, 0, 0, btn.Width - 1, btn.Height - 1);
            }

            // Chữ đen đậm, giữ size
            using (Font font = new Font(btn.Font.FontFamily, btn.Font.Size, FontStyle.Bold))
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
