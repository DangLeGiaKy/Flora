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
                    MessageBox.Show("Kết nối thành công!");

                    this.Hide();
                    new Sinhvien().ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại:\n" + ex.Message);
            }
        }
    }
}
