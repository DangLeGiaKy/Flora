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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtAccount.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM [User] WHERE TenDangNhap=@user AND MatKhau=@pass";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                int result = (int)cmd.ExecuteScalar();
                if (result > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    Main f = new Main();
                    f.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
                }
            }
        }

        private void lbldoimk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form10 f = new Form10();
            f.ShowDialog();
        }
    }
}
