
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using test.BUS;

namespace test.GUI
{
    public partial class frmQuanLyUser : Form
    {
        private QuanLyUser userBUS = new QuanLyUser();
        public frmQuanLyUser()
        {
            InitializeComponent();
            ConfigureDataGridView();
            LoadDataGridView();
            dgvUser.DataError += dgvUser_DataError;

        }
        private void frmQuanLyUser_Load(object sender, EventArgs e)
        {

        }

        // Xử lý lỗi DataGridView
        private void dgvUser_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Bỏ qua lỗi format của cột TrangThai
            if (dgvUser.Columns[e.ColumnIndex].Name == "TrangThai")
            {
                e.ThrowException = false;
                e.Cancel = true;
            }
        }

        // Cấu hình DataGridView
        private void ConfigureDataGridView()
        {
            dgvUser.AutoGenerateColumns = false;
            dgvUser.AllowUserToAddRows = false;
            dgvUser.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUser.MultiSelect = false;
            dgvUser.ReadOnly = true;
            dgvUser.ReadOnly = false; // cho phép nút click
            foreach (DataGridViewColumn col in dgvUser.Columns)
            {
                if (col.Name != "colSua" && col.Name != "colXoa")
                    col.ReadOnly = true;
            }
            dgvUser.RowHeadersVisible = false;
            dgvUser.BackgroundColor = Color.White;
            dgvUser.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            dgvUser.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUser.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUser.EnableHeadersVisualStyles = false;
            dgvUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Xóa tất cả cột cũ (nếu có)
            dgvUser.Columns.Clear();

            // Cột STT
            DataGridViewTextBoxColumn colSTT = new DataGridViewTextBoxColumn();
            colSTT.Name = "STT";
            colSTT.HeaderText = "STT";
            colSTT.Width = 50;
            colSTT.FillWeight = 30;
            dgvUser.Columns.Add(colSTT);

            // Cột MaUser
            DataGridViewTextBoxColumn colMaUser = new DataGridViewTextBoxColumn();
            colMaUser.Name = "MaUser";
            colMaUser.HeaderText = "Mã User";
            colMaUser.DataPropertyName = "MaUser";
            colMaUser.FillWeight = 60;
            dgvUser.Columns.Add(colMaUser);

            // Cột TenDangNhap
            DataGridViewTextBoxColumn colTenDangNhap = new DataGridViewTextBoxColumn();
            colTenDangNhap.Name = "TenDangNhap";
            colTenDangNhap.HeaderText = "Tên đăng nhập";
            colTenDangNhap.DataPropertyName = "TenDangNhap";
            colTenDangNhap.FillWeight = 80;
            dgvUser.Columns.Add(colTenDangNhap);

            DataGridViewTextBoxColumn colMatkhau = new DataGridViewTextBoxColumn();
            colMatkhau.Name = "MatKhau";
            colMatkhau.HeaderText = "Mật khẩu";
            colMatkhau.DataPropertyName = "MatKhau";
            colMatkhau.FillWeight = 80;
            dgvUser.Columns.Add(colMatkhau);

            // Cột HoTen
            DataGridViewTextBoxColumn colHoTen = new DataGridViewTextBoxColumn();
            colHoTen.Name = "HoTen";
            colHoTen.HeaderText = "Họ tên";
            colHoTen.DataPropertyName = "HoTen";
            colHoTen.FillWeight = 100;
            dgvUser.Columns.Add(colHoTen);

            // Cột Email
            DataGridViewTextBoxColumn colEmail = new DataGridViewTextBoxColumn();
            colEmail.Name = "Email";
            colEmail.HeaderText = "Email";
            colEmail.DataPropertyName = "Email";
            colEmail.FillWeight = 100;
            dgvUser.Columns.Add(colEmail);

            // Cột SoDienThoai
            DataGridViewTextBoxColumn colSoDienThoai = new DataGridViewTextBoxColumn();
            colSoDienThoai.Name = "SoDienThoai";
            colSoDienThoai.HeaderText = "Số điện thoại";
            colSoDienThoai.DataPropertyName = "SoDienThoai";
            colSoDienThoai.FillWeight = 70;
            dgvUser.Columns.Add(colSoDienThoai);

            // Cột VaiTro
            DataGridViewTextBoxColumn colVaiTro = new DataGridViewTextBoxColumn();
            colVaiTro.Name = "VaiTro";
            colVaiTro.HeaderText = "Vai trò";
            colVaiTro.DataPropertyName = "VaiTro";
            colVaiTro.FillWeight = 60;
            dgvUser.Columns.Add(colVaiTro);

            // Cột TrangThai - KHÔNG dùng DataPropertyName
            DataGridViewTextBoxColumn colTrangThai = new DataGridViewTextBoxColumn();
            colTrangThai.Name = "TrangThai";
            colTrangThai.HeaderText = "Trạng thái";
            colTrangThai.FillWeight = 60;
            dgvUser.Columns.Add(colTrangThai);

            // Cột NgayTao
            DataGridViewTextBoxColumn colNgayTao = new DataGridViewTextBoxColumn();
            colNgayTao.Name = "NgayTao";
            colNgayTao.HeaderText = "Ngày tạo";
            colNgayTao.DataPropertyName = "NgayTao";
            colNgayTao.DefaultCellStyle.Format = "dd/MM/yyyy";
            colNgayTao.FillWeight = 70;
            dgvUser.Columns.Add(colNgayTao);

            // Cột nút Sửa
            DataGridViewButtonColumn colSua = new DataGridViewButtonColumn();
            colSua.Name = "colSua";
            colSua.HeaderText = "Sửa";
            colSua.Text = "Sửa";
            colSua.UseColumnTextForButtonValue = true;
            colSua.FillWeight = 40;
            colSua.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            colSua.DefaultCellStyle.ForeColor = Color.White;
            dgvUser.Columns.Add(colSua);

            // Cột nút Xóa
            DataGridViewButtonColumn colXoa = new DataGridViewButtonColumn();
            colXoa.Name = "colXoa";
            colXoa.HeaderText = "Xóa";
            colXoa.Text = "Xóa";
            colXoa.UseColumnTextForButtonValue = true;
            colXoa.FillWeight = 40;
            colXoa.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
            colXoa.DefaultCellStyle.ForeColor = Color.White;
            dgvUser.Columns.Add(colXoa);

            
        }

        // Load dữ liệu vào DataGridView
        private void LoadDataGridView()
        {
            try
            {
                DataTable dt = userBUS.GetAllUsers();
                dgvUser.DataSource = null; // Clear trước
                dgvUser.DataSource = dt;

                // Đánh số thứ tự và format trạng thái
                for (int i = 0; i < dgvUser.Rows.Count; i++)
                {
                    dgvUser.Rows[i].Cells["STT"].Value = (i + 1).ToString();

                    // Lấy giá trị TrangThai từ DataTable gốc
                    bool trangThai = Convert.ToBoolean(dt.Rows[i]["TrangThai"]);
                    dgvUser.Rows[i].Cells["TrangThai"].Value = trangThai ? "Active" : "Inactive";
                }

                lblTongSo.Text = "Tổng số: " + dt.Rows.Count + " tài khoản";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý click vào cell trong DataGridView
        //private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    // Bỏ qua click vào header
        //    if (e.RowIndex < 0) return;

        //    try
        //    {
        //        string maUser = dgvUser.Rows[e.RowIndex].Cells["MaUser"].Value.ToString();

        //        // Click cột "Sửa"
        //        if (dgvUser.Columns[e.ColumnIndex].Name == "colSua")
        //        {
        //            frmThemSuaUser frm = new frmThemSuaUser(FormMode.Sua, maUser);
        //            if (frm.ShowDialog() == DialogResult.OK)
        //            {
        //                LoadDataGridView();
        //                MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //        // Click cột "Xóa"
        //        else if (dgvUser.Columns[e.ColumnIndex].Name == "colXoa")
        //        {
        //            string hoTen = dgvUser.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();
        //            DialogResult result = MessageBox.Show(
        //                $"Bạn có chắc chắn muốn xóa tài khoản '{hoTen}'?",
        //                "Xác nhận xóa",
        //                MessageBoxButtons.YesNo,
        //                MessageBoxIcon.Question);

        //            if (result == DialogResult.Yes)
        //            {
        //                if (userBUS.DeleteUser(maUser))
        //                {
        //                    LoadDataGridView();
        //                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //            }
        //        }
        //        // Click cột "Đổi MK"

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        // Nút Thêm tài khoản
        private void btnThemUser_Click(object sender, EventArgs e)
        {
            frmThemSuaUser frm = new frmThemSuaUser(FormMode.Them, null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDataGridView();
                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Nút Tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();

            if (keyword == "")
            {
                MessageBox.Show("Vui lòng nhập thông tin cần tìm!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LoadTimKiem(keyword);
        }
        private void LoadTimKiem(string keyword)
        {
            try
            {
                DataTable dt = userBUS.SearchUsers(keyword);
                dgvUser.DataSource = dt;

                for (int i = 0; i < dgvUser.Rows.Count; i++)
                {
                    dgvUser.Rows[i].Cells["STT"].Value = (i + 1).ToString();

                    bool trangThai = Convert.ToBoolean(dt.Rows[i]["TrangThai"]);
                    dgvUser.Rows[i].Cells["TrangThai"].Value = trangThai ? "Active" : "Inactive";
                }

                lblTongSo.Text = "Tổng: " + dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm: " + ex.Message);
            }
        }


        // Nút Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDataGridView();
        }

        // Tìm kiếm khi nhấn Enter
        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLamMoi_Click_1(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadDataGridView();
        }

        private void btnThemUser_Click_1(object sender, EventArgs e)
        {
            frmThemSuaUser frm = new frmThemSuaUser(FormMode.Them, null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDataGridView();
                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvUser_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy mã user chung cho cả sửa và xóa
            string maUser = dgvUser.Rows[e.RowIndex].Cells["MaUser"].Value.ToString();
            string hoTen = dgvUser.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();

            // ======== NÚT SỬA ========
            if (dgvUser.Columns[e.ColumnIndex].Name == "colSua")
            {
                frmThemSuaUser frm = new frmThemSuaUser(FormMode.Sua, maUser);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadDataGridView();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // ======== NÚT XÓA ========
            else if (dgvUser.Columns[e.ColumnIndex].Name == "colXoa")
            {
                if (MessageBox.Show($"Bạn có chắc chắn muốn xóa '{hoTen}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int kq = userBUS.DeleteUser(maUser);

                    if (kq == -1)
                    {
                        MessageBox.Show("Tài khoản này đang đăng nhập, không thể xóa!",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (kq == 0)
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để xóa.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (kq == -2)
                    {
                        MessageBox.Show("Không thể xóa tài khoản! Vui lòng thử lại.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        LoadDataGridView();
                        MessageBox.Show("Xóa thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnTimKiem_Paint(object sender, PaintEventArgs e)
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

        private void btnLamMoi_Paint(object sender, PaintEventArgs e)
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

        private void btnThemUser_Paint(object sender, PaintEventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form10 f = new Form10();
            f.ShowDialog();
        }

        private void btndoimk_Paint(object sender, PaintEventArgs e)
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
    public enum FormMode
        {
            Them,
            Sua
        }
    }

