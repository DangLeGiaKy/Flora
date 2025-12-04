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
using test.BUS;
using test.DAL;

namespace test.GUI
{
    public partial class Form4 : Form
    {
        DataTable dtKhachHang;

        BUS_KhachHang bus = new BUS_KhachHang();
        public Form4()
        {
            InitializeComponent();
            LoadData();
            LoadComboBoxSearch();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnthemkh_Click(object sender, EventArgs e)
        {
            frmthemkh f = new frmthemkh();

            if (f.ShowDialog() == DialogResult.OK)
            {
                dgvKhachHang.DataSource = bus.LayDanhSachKhachHang(); // load lại bảng
            }
        }
        private void LoadData()
        {
            dtKhachHang = bus.LayDanhSachKhachHang();  // lưu toàn bộ dữ liệu
            dgvKhachHang.DataSource = dtKhachHang;

            dgvKhachHang.Columns["MaKhachHang"].HeaderText = "Mã KH";
            dgvKhachHang.Columns["HoTen"].HeaderText = "Họ tên";
            dgvKhachHang.Columns["SoDienThoai"].HeaderText = "Số ĐT";
            dgvKhachHang.Columns["Email"].HeaderText = "Email";
            dgvKhachHang.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dgvKhachHang.Columns["GhiChu"].HeaderText = "Ghi chú";

            dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                txtMaKH.Text = row.Cells["MaKhachHang"].Value.ToString();
                txtHoTen.Text = row.Cells["HoTen"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();

                SetTextboxReadOnly(true);
            }
        }
        private void SetTextboxReadOnly(bool readOnly)
        {
            txtMaKH.ReadOnly = true;        // Mã KH luôn khóa
            txtHoTen.ReadOnly = readOnly;
            txtSoDienThoai.ReadOnly = readOnly;
            txtEmail.ReadOnly = readOnly;
            txtDiaChi.ReadOnly = readOnly;
            txtGhiChu.ReadOnly = readOnly;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SetTextboxReadOnly(false);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            bool ok = bus.CapNhatKhachHang(
               txtMaKH.Text,
               txtHoTen.Text,
               txtSoDienThoai.Text,
               txtEmail.Text,
               txtDiaChi.Text,
               txtGhiChu.Text
            );

            if (ok)
            {
                MessageBox.Show("Cập nhật thành công!");
                dgvKhachHang.DataSource = bus.LayDanhSachKhachHang();
                SetTextboxReadOnly(true);
            }
            else
            {
                MessageBox.Show("Lỗi khi cập nhật!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?",
        "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string message;
                bool ok = bus.XoaKhachHang(txtMaKH.Text, out message); // lấy message từ DAL

                MessageBox.Show(message); // luôn show thông báo đúng

                if (ok)
                {
                    dgvKhachHang.DataSource = bus.LayDanhSachKhachHang();
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string keyword = comboBox1.Text.Trim();
            TimKiemKhachHang(keyword);
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = comboBox1.Text.Trim();

            // Nếu rỗng → tải lại toàn bộ
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            // Nếu có chữ → tìm kiếm
            TimKiemKhachHang(keyword);
        }
        private void TimKiemKhachHang(string keyword)
        {
            using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
            {
                conn.Open();

                string query = @"SELECT * FROM KhachHang
                         WHERE HoTen LIKE @kw 
                            OR SoDienThoai LIKE @kw";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvKhachHang.DataSource = dt;
            }
        }
        private void LoadComboBoxSearch()
        {
            comboBox1.Items.Clear();

            DataTable dt = bus.LayDanhSachKhachHang();

            foreach (DataRow row in dt.Rows)
            {
                comboBox1.Items.Add(row["HoTen"].ToString());
                comboBox1.Items.Add(row["SoDienThoai"].ToString());
            }
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

        private void btnSua_Paint(object sender, PaintEventArgs e)
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

        private void btnXoa_Paint(object sender, PaintEventArgs e)
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

        private void groupBox1_Paint(object sender, PaintEventArgs e)
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
    }
}

