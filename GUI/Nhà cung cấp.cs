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
    public partial class Form7 : Form
    {
        nhacungcap bus = new nhacungcap();
        //string connectionString = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        //string connectionString = "Data Source=khanhvinh\\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True";
        string connectionString = Connection.GetConnectionString();

        public Form7()
        {
            InitializeComponent();
            dgvNCC.DataSource = bus.LayDanhSachNCC();

            dgvNCC.Columns["MaNhaCungCap"].HeaderText = "Mã NCC";
            dgvNCC.Columns["TenNhaCungCap"].HeaderText = "Tên NCC";
            dgvNCC.Columns["SoDienThoai"].HeaderText = "Số ĐT";
            dgvNCC.Columns["Email"].HeaderText = "Email";
            dgvNCC.Columns["DiaChi"].HeaderText = "Địa Chỉ";
            dgvNCC.Columns["LoaiHangCungCap"].HeaderText = "Loại hàng";
            dgvNCC.Columns["GhiChu"].HeaderText = "Ghi chú";
            dgvNCC.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            LoadComboBoxNCC();
            SetTextBoxesReadOnly(true);

        }



        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void LoadComboBoxNCC()
        {
            string query = "SELECT TenNhaCungCap FROM NhaCungCap";
            //using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
            using (SqlConnection conn = Connection.connect())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();

                while (reader.Read())
                {
                    string ten = reader["TenNhaCungCap"].ToString();
                    comboBox1.Items.Add(ten);
                    auto.Add(ten);
                }

                comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                comboBox1.AutoCompleteCustomSource = auto;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Frmthemncc f = new Frmthemncc();
            f.ShowDialog();

            // Nếu form thêm đã thêm NCC thành công → reload DataGridView
            if (f.DaThemThanhCong)
            {
                dgvNCC.DataSource = bus.LayDanhSachNCC();
            }
        }
        private void SetTextBoxesReadOnly(bool readOnly)
        {
            txtMaNCC.ReadOnly = readOnly;
            txtTenNCC.ReadOnly = readOnly;
            txtSoDienThoai.ReadOnly = readOnly;

            txtDiaChi.ReadOnly = readOnly;
            txtLoaiHang.ReadOnly = readOnly;
            txtGhiChu.ReadOnly = readOnly;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // tránh lỗi khi click header

            DataGridViewRow row = dgvNCC.Rows[e.RowIndex];

            txtMaNCC.Text = row.Cells["MaNhaCungCap"].Value?.ToString();
            txtTenNCC.Text = row.Cells["TenNhaCungCap"].Value?.ToString();
            txtSoDienThoai.Text = row.Cells["SoDienThoai"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
            txtDiaChi.Text = row.Cells["DiaChi"].Value?.ToString();
            txtLoaiHang.Text = row.Cells["LoaiHangCungCap"].Value?.ToString();
            txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
        }
        private void LoadData()
        {
            //using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
            using (SqlConnection conn = Connection.connect())
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM NhaCungCap", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNCC.DataSource = dt;
            }
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = comboBox1.Text.Trim();

            // Nếu rỗng -> load toàn bộ
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            // Nếu có chữ -> tìm kiếm
            TimKiemTheoTen(keyword);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ten = comboBox1.Text.Trim();
            TimKiemTheoTen(ten);
        }
        private void TimKiemTheoTen(string keyword)
        {
            using (SqlConnection conn = Connection.connect())
            {
                conn.Open();

                string query = @"SELECT * FROM NhaCungCap 
                         WHERE TenNhaCungCap LIKE @kw OR LoaiHangCungCap LIKE @kw";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvNCC.DataSource = dt;
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            SetTextBoxesReadOnly(false);
            txtMaNCC.ReadOnly = true;
        }
        private void LoadNCC()
        {
            ;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM NhaCungCap", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvNCC.DataSource = dt;
            }
            SetTextBoxesReadOnly(true); // khóa TextBox
        }
        private void btnluu_Click(object sender, EventArgs e)
        {
            

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE NhaCungCap 
                       SET TenNhaCungCap=@Ten, SoDienThoai=@SDT, Email=@Email, 
                           DiaChi=@DiaChi, LoaiHangCungCap=@LoaiHang, GhiChu=@GhiChu 
                       WHERE MaNhaCungCap=@MaNCC";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                cmd.Parameters.AddWithValue("@Ten", txtTenNCC.Text);
                cmd.Parameters.AddWithValue("@SDT", txtSoDienThoai.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text);
                cmd.Parameters.AddWithValue("@LoaiHang", txtLoaiHang.Text);
                cmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadNCC(); // refresh DataGridView
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!");
                }
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNCC.Text)) return;

            DialogResult dr = MessageBox.Show("Bạn có chắc muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM NhaCungCap WHERE MaNhaCungCap=@MaNCC", con);
                    cmd.Parameters.AddWithValue("@MaNCC", txtMaNCC.Text);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Xóa thành công!");
                        LoadNCC();
                        // Xóa TextBox
                        txtMaNCC.Clear(); txtTenNCC.Clear(); txtSoDienThoai.Clear();
                        txtEmail.Clear(); txtDiaChi.Clear(); txtLoaiHang.Clear(); txtGhiChu.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại!");
                    }
                }
            }
        }

        private void txtMaNCC_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Paint(object sender, PaintEventArgs e)
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

        private void btnsua_Paint(object sender, PaintEventArgs e)
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

        private void btnxoa_Paint(object sender, PaintEventArgs e)
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

        private void btnluu_Paint(object sender, PaintEventArgs e)
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
