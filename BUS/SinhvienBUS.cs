
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test.GUI;
using test.DAL;
using test.DTO;

namespace test.BUS
{
    internal class SinhvienBUS
    {
        private readonly SinhvienDAL dal = new SinhvienDAL();

        public DataTable GetAllSinhVien()
        {
            return dal.GetAllSinhVien();
        }

        public bool InsertSinhVien(SinhvienDTO sv)
        {
            if (string.IsNullOrWhiteSpace(sv.HoTen)) return false;
            return dal.InsertSinhVien(sv);
        }

        public bool UpdateSinhVien(SinhvienDTO sv)
        {
            if (string.IsNullOrWhiteSpace(sv.MaSV)) return false;
            return dal.UpdateSinhVien(sv);
        }

        public bool DeleteSinhVien(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return false;
            return dal.DeleteSinhVien(maSV);
        }
    }
}


