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
    public partial class Kho : Form
    {
        public Kho()
        {
            InitializeComponent();
        }

        private void Kho_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            frmNhkho frmnk = new frmNhkho() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            
            frmnk.Show();
        }
    }
}
