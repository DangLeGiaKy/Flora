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

namespace test.GUI
{
    public partial class frmthemkh : Form
    {
        BUS_KhachHang bus = new BUS_KhachHang();
        public frmthemkh()
        {
            InitializeComponent();
            this.Load += frmthemkh_Load;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void frmthemkh_Load(object sender, EventArgs e)
        {
            txtMaKH.Text = bus.TaoMaKhachHangTuDong();
            txtMaKH.ReadOnly = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnthemkh_Click(object sender, EventArgs e)
        {
            string ma = txtMaKH.Text.Trim();
            string ten = txtHoTen.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string ghichu = txtGhiChu.Text.Trim();

            if (ten == "" || sdt == "")
            {
                MessageBox.Show("Tên và số điện thoại là bắt buộc!");
                return;
            }

            bool ok = bus.ThemKhachHang(ma, ten, sdt, email, diachi, ghichu);

            if (ok)
            {
                MessageBox.Show("Thêm khách hàng thành công!");

                this.DialogResult = DialogResult.OK; // báo về form cha để reload DataGrid
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi: Không thể thêm khách hàng!");
            }
        }

        private void txtMaKH_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDiaChi_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnthemkh_Paint(object sender, PaintEventArgs e)
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
