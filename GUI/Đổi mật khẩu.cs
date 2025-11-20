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

namespace test.GUI
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void txtdangnhap_TextChanged(object sender, EventArgs e)
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

            if (newPass != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            using (SqlConnection conn = new SqlConnection("Data Source=khanhvinh\\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True"))
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
    }
}
