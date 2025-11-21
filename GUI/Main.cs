using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test.GUI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Login f = new Login();
            f.Show();
            this.Close();
        }

        private void btnbanhang_Click(object sender, EventArgs e)
        {
            
            this.pnlFormLoader.Controls.Clear();
            Form1 frmBanhang = new Form1() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            
            this.pnlFormLoader.Controls.Add(frmBanhang);
            frmBanhang.Show();
            
        }

        private void btndonhang_Click(object sender, EventArgs e)
        {
            this.pnlFormLoader.Controls.Clear();
            Form2 frmdonhang = new Form2() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmdonhang.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmdonhang);
            frmdonhang.Show();
        }

        private void pnlFormLoader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            this.pnlFormLoader.Controls.Clear();
            Form4 frmkhachhang = new Form4() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmkhachhang.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmkhachhang);
            frmkhachhang.Show();
        }

        private void ntmsanpham_Click(object sender, EventArgs e)
        {
            this.pnlFormLoader.Controls.Clear();
            Kho frmsp = new Kho() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmsp.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmsp);
            frmsp.Show();
        }

        private void btnncc_Click(object sender, EventArgs e)
        {
            this.pnlFormLoader.Controls.Clear();
            Form7 frmncc = new Form7() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmncc.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmncc);
            frmncc.Show();
        }

        private void btntk_Click(object sender, EventArgs e)
        {

        }

        private void btnbc_Click(object sender, EventArgs e)
        {
            this.pnlFormLoader.Controls.Clear();
            Form9 frmbc = new Form9() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmbc.FormBorderStyle = FormBorderStyle.None;
            this.pnlFormLoader.Controls.Add(frmbc);
            frmbc.Show();
        }
    }
}
