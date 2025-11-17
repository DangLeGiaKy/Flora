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
using test.DTO;


namespace test.GUI
{
    public partial class Sinhvien : Form
    {


        private SinhvienBUS bus = new SinhvienBUS();
        public Sinhvien()
        {
            InitializeComponent();
        }
        private void FormSinhVien_Load(object sender, EventArgs e)
        {
            LoadData();
            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
        }
        private void LoadData()
        {
            dataGridView1.DataSource = bus.GetAllSinhVien();
            dataGridView1.Columns[0].HeaderText = "Mã SV";
            dataGridView1.Columns[1].HeaderText = "Họ tên";
            dataGridView1.Columns[2].HeaderText = "Giới tính";
            dataGridView1.Columns[3].HeaderText = "Quê quán";
        }
        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["MaSV"].Value.ToString();

                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["HoTen"].Value.ToString();
                comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["GioiTinh"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["QueQuan"].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string que = textBox4.Text.Trim().ToUpper();
            string gioiTinh = comboBox1.Text.ToUpper();

            if (string.IsNullOrWhiteSpace(que) || string.IsNullOrWhiteSpace(gioiTinh))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Lấy ký hiệu tỉnh: 2 ký tự đầu
            string kyHieuQue = que.Length >= 2 ? que.Substring(0, 2) : que;
            string kyHieuGT = gioiTinh == "NAM" ? "N" : "NU";

            // Lấy số tự tăng dựa trên danh sách hiện có
            int nextID = dataGridView1.Rows.Count + 1;
            string maSV = $"{kyHieuQue}_{kyHieuGT}_{nextID:D4}";

            // Tạo đối tượng sinh viên
            SinhvienDTO sv = new SinhvienDTO
            {
                MaSV = maSV,
                HoTen = textBox3.Text.Trim(),
                GioiTinh = comboBox1.Text,
                QueQuan = textBox4.Text.Trim()
            };

            if (bus.InsertSinhVien(sv))
            {
                MessageBox.Show("✅ Thêm sinh viên thành công!");
                LoadData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("❌ Thêm thất bại!");
            }
        }
        private void ClearForm()
        {
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Chọn sinh viên cần sửa!");
                return;
            }

            SinhvienDTO sv = new SinhvienDTO
            {
                MaSV = textBox2.Text.Trim(),
                HoTen = textBox3.Text.Trim(),
                GioiTinh = comboBox1.Text,
                QueQuan = textBox4.Text.Trim()
            };

            if (bus.UpdateSinhVien(sv))
            {
                MessageBox.Show("✏️ Cập nhật thành công!");
                LoadData();
                ClearForm();
            }
            else
            {
                MessageBox.Show("❌ Cập nhật thất bại!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Chọn sinh viên cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (bus.DeleteSinhVien(textBox2.Text))
                {
                    MessageBox.Show("🗑️ Xóa thành công!");
                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("❌ Xóa thất bại!");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
