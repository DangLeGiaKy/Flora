using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.BUS;
using test.DAL;
using test.DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace test.GUI
{
    public partial class frmThemDonHang : Form
    {

        private string connect = Connection.GetConnectionString();
        //private string connect = @"Data Source=ANH-VU\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private frmQuanLyDonHang parentForm;
        private DataTable chiTietTable;


        public frmThemDonHang(frmQuanLyDonHang parent)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            parentForm = parent;

            LoadKhachHang();
            LoadSanPham();
            LoadTrangThai();
            InitChiTietTable();
            txtMaDonHang.ReadOnly = true;


        }
        private void InitChiTietTable()
        {
            chiTietTable = new DataTable();
            chiTietTable.Columns.Add("MaSanPham");
            chiTietTable.Columns.Add("TenSanPham");
            chiTietTable.Columns.Add("SoLuong", typeof(int));
            chiTietTable.Columns.Add("GiaBan", typeof(decimal));
            chiTietTable.Columns.Add("ThanhTien", typeof(decimal));

            dgvCT.DataSource = chiTietTable;
        }
        private void LoadKhachHang()
        {
            string sql = "SELECT MaKhachHang, HoTen, SoDienThoai FROM KhachHang";
            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbKhachHang.DataSource = dt;
                cmbKhachHang.DisplayMember = "HoTen";
                cmbKhachHang.ValueMember = "MaKhachHang";
                cmbKhachHang.SelectedIndex = -1;
            }
        }

        private void LoadSanPham()
        {
            string sql = "SELECT MaSanPham, TenSanPham, SoLuongTon, GiaBan FROM Kho WHERE TrangThai=1";
            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbSanPham.DataSource = dt;
                cmbSanPham.DisplayMember = "TenSanPham";
                cmbSanPham.ValueMember = "MaSanPham";
                cmbSanPham.SelectedIndex = -1;
            }
        }
        private void LoadTrangThai()
        {
            cmbTrangThai.Items.Clear();
            cmbTrangThai.Items.Add("Đang xử lý");
            cmbTrangThai.Items.Add("Đã xác nhận");
            cmbTrangThai.Items.Add("Đang giao");
            cmbTrangThai.Items.Add("Hoàn tất");
            cmbTrangThai.Items.Add("Hủy");
            cmbTrangThai.SelectedIndex = 0; // mặc định
        }
        private bool KiemTraSoLuongKho(string maSP, int soLuong)
        {
            string sql = "SELECT SoLuongTon FROM Kho WHERE MaSanPham=@MaSP";
            using (SqlConnection conn = new SqlConnection(connect))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@MaSP", maSP);
                conn.Open();
                int slTon = Convert.ToInt32(cmd.ExecuteScalar());
                return soLuong <= slTon;
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            frmthemkh f = new frmthemkh();
            f.ShowDialog();
            LoadKhachHang();


        }

        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        

        

        
        private void CalculateTotal()
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnThemDonHang_Click(object sender, EventArgs e)
        {
            if (cmbKhachHang.SelectedIndex == -1)
            {
                MessageBox.Show("Chọn khách hàng!");
                return;
            }

            if (chiTietTable.Rows.Count == 0)
            {
                MessageBox.Show("Hãy thêm ít nhất 1 sản phẩm!");
                return;
            }

            string maKH = cmbKhachHang.SelectedValue.ToString();
            string maNV = "U001";

            string maDonHang = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            DateTime ngayGiao = dtNgayNhan.Value;
            string trangThai = cmbTrangThai.Text;
            decimal tongTien = Convert.ToDecimal(txtTongTien.Text);

            string sqlDH = @"INSERT INTO DonHang(MaDonHang, MaKhachHang, MaNhanVien, NgayDat, NgayGiao, TongTien, TongGiaNhap, LoiNhuan, TrangThai)
                     VALUES(@MaDH,@MaKH,@MaNV,GETDATE(),@NgayGiao,@TongTien,0,0,@TrangThai)";

            string sqlCT = @"INSERT INTO ChiTietDonHang(MaChiTiet, MaDonHang, MaSanPham, SoLuong, GiaNhap, GiaBan, ThanhTien, TongGiaNhap, LoiNhuan)
                     VALUES(@MaCT,@MaDH,@MaSP,@SoLuong,0,@GiaBan,@ThanhTien,0,0)";

            string sqlKho = @"UPDATE Kho SET SoLuongTon = SoLuongTon - @SL WHERE MaSanPham = @MaSP";

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                    // Insert DonHang
                    SqlCommand cmdDH = new SqlCommand(sqlDH, conn, tran);
                    cmdDH.Parameters.AddWithValue("@MaDH", maDonHang);
                    cmdDH.Parameters.AddWithValue("@MaKH", maKH);
                    cmdDH.Parameters.AddWithValue("@MaNV", maNV);
                    cmdDH.Parameters.AddWithValue("@NgayGiao", ngayGiao);
                    cmdDH.Parameters.AddWithValue("@TongTien", tongTien);
                    cmdDH.Parameters.AddWithValue("@TrangThai", trangThai);
                    cmdDH.ExecuteNonQuery();

                    // Insert từng dòng chi tiết
                    foreach (DataRow row in chiTietTable.Rows)
                    {
                        string maCT = "CT" + Guid.NewGuid().ToString("N").Substring(0, 8);

                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, tran);
                        cmdCT.Parameters.AddWithValue("@MaCT", maCT);
                        cmdCT.Parameters.AddWithValue("@MaDH", maDonHang);
                        cmdCT.Parameters.AddWithValue("@MaSP", row["MaSanPham"]);
                        cmdCT.Parameters.AddWithValue("@SoLuong", row["SoLuong"]);
                        cmdCT.Parameters.AddWithValue("@GiaBan", row["GiaBan"]);
                        cmdCT.Parameters.AddWithValue("@ThanhTien", row["ThanhTien"]);
                        cmdCT.ExecuteNonQuery();

                        // Trừ tồn kho
                        SqlCommand cmdKho = new SqlCommand(sqlKho, conn, tran);
                        cmdKho.Parameters.AddWithValue("@MaSP", row["MaSanPham"]);
                        cmdKho.Parameters.AddWithValue("@SL", row["SoLuong"]);
                        cmdKho.ExecuteNonQuery();
                    }

                    tran.Commit();
                    MessageBox.Show("Tạo đơn hàng thành công!");

                    parentForm.LoadDonHang();
                    this.Close();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Lỗi khi tạo đơn!\n" + ex.Message);
                }
            }
        }

        
        

        private void cmbSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedIndex != -1 && int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                decimal giaBan = Convert.ToDecimal(((DataRowView)cmbSanPham.SelectedItem)["GiaBan"]);
                txtTongTien.Text = (giaBan * soLuong).ToString("0.00");
            }
        }
        

        private void cmbKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhachHang.SelectedIndex != -1)
            {
                txtSDT.Text = ((DataRowView)cmbKhachHang.SelectedItem)["SoDienThoai"].ToString();
            }
        }

        private void txtTongTien_TextChanged(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedIndex != -1 && int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                decimal giaBan = Convert.ToDecimal(((DataRowView)cmbSanPham.SelectedItem)["GiaBan"]);
                txtTongTien.Text = (giaBan * soLuong).ToString("0.00");
            }
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedIndex == -1 || !int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                MessageBox.Show("Chọn sản phẩm và nhập số lượng!");
                return;
            }

            string maSP = cmbSanPham.SelectedValue.ToString();

            // Kiểm tra tồn kho
            if (!KiemTraSoLuongKho(maSP, soLuong))
            {
                MessageBox.Show("Số lượng vượt quá tồn kho!");
                return;
            }

            string tenSP = ((DataRowView)cmbSanPham.SelectedItem)["TenSanPham"].ToString();
            decimal giaBan = Convert.ToDecimal(((DataRowView)cmbSanPham.SelectedItem)["GiaBan"]);
            decimal thanhTien = giaBan * soLuong;

            // Thêm vào bảng
            chiTietTable.Rows.Add(maSP, tenSP, soLuong, giaBan, thanhTien);

            // Cập nhật tổng đơn hàng
            decimal tong = chiTietTable.AsEnumerable().Sum(r => r.Field<decimal>("ThanhTien"));
            txtTongTien.Text = tong.ToString("0.00");

            // reset
            txtSoLuong.Clear();
            cmbSanPham.SelectedIndex = -1;
        }

        private void dgvCT_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmThemDonHang_Load(object sender, EventArgs e)
        {

        }

        private void txtMaDonHang_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            LoadKhachHang(); // Gọi lại hàm bạn đã làm sẵn
            MessageBox.Show("Đã làm mới danh sách khách hàng!");
        }

        private void btnthemkhach_Click(object sender, EventArgs e)
        {
            frmthemkh f = new frmthemkh();
            f.ShowDialog();
            LoadKhachHang();
        }

        private void txtTongTien_TextChanged_1(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedIndex != -1 && int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                decimal giaBan = Convert.ToDecimal(((DataRowView)cmbSanPham.SelectedItem)["GiaBan"]);
                txtTongTien.Text = (giaBan * soLuong).ToString("0.00");
            }
        }

        private void txtSoLuong_TextChanged_1(object sender, EventArgs e)
        {
            if (cmbSanPham.SelectedIndex != -1 && int.TryParse(txtSoLuong.Text, out int soLuong))
            {
                decimal giaBan = Convert.ToDecimal(((DataRowView)cmbSanPham.SelectedItem)["GiaBan"]);
                txtTongTien.Text = (giaBan * soLuong).ToString("0.00");
            }
        }

        private void cmbTrangThai_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void cmbKhachHang_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbKhachHang.SelectedIndex != -1)
            {
                txtSDT.Text = ((DataRowView)cmbKhachHang.SelectedItem)["SoDienThoai"].ToString();
            }
        }

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnthemkhach_Paint(object sender, PaintEventArgs e)
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

        private void button2_Paint(object sender, PaintEventArgs e)
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

        private void btnThemSP_Paint(object sender, PaintEventArgs e)
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

        private void btnThem_Paint(object sender, PaintEventArgs e)
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
