using System;
using System.Drawing;
using System.Windows.Forms;
using test.BUS;

namespace test.GUI
{
    public partial class Frmthemncc : Form
    {
        nhacungcap bus = new nhacungcap();

        public bool DaThemThanhCong = false; // báo về form cha

        public Frmthemncc()
        {
            InitializeComponent();
            this.Load += Frmthemncc_Load;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Frmthemncc_Load(object sender, EventArgs e)
        {
            txtMaNCC.Text = bus.TaoMaNhaCungCapTuDong();
            
            txtMaNCC.ReadOnly = true;
        }

        private void btnThemNCC_Click(object sender, EventArgs e)
        {
            string ma = txtMaNCC.Text.Trim();
            string ten = txtTenNCC.Text.Trim();
            string sdt = txtSoDienThoai.Text.Trim();
            string email = txtEmail.Text.Trim();
            string diachi = txtDiaChi.Text.Trim();
            string loai = txtLoaiHang.Text.Trim();
            string ghichu = txtGhiChu.Text.Trim();

            if (ten == "" || sdt == "" || diachi == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!");
                return;
            }

            bool kq = bus.ThemNCC(ma, ten, sdt, email, diachi, loai, ghichu);

            if (kq)
            {
                MessageBox.Show("Thêm nhà cung cấp thành công!");

                DaThemThanhCong = true;
                this.Close();  // 🔥 Đóng form sau khi thêm
            }
            else
            {
                MessageBox.Show("Không thể thêm nhà cung cấp.");
            }
        }

        private void Frmthemncc_Load_1(object sender, EventArgs e)
        {

        }

        private void btnthemncc_Paint(object sender, PaintEventArgs e)
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

        private void Frmthemncc_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
