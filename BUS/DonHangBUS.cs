using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.DAL;
using test.DTO;

namespace test.BUS
{
    internal class DonHangBUS
    {
        private DonHangDAL dal = new DonHangDAL();

        public DataTable GetAll() => dal.GetAll();
        public DataTable SearchByKhachHang(string ten) => dal.SearchByKhachHang(ten);

        public bool Insert(DonHangDTO d) => dal.Insert(d) > 0;
        public bool Update(DonHangDTO d) => dal.Update(d) > 0;
        public bool Delete(string ma) => dal.Delete(ma) > 0;
    }
}
