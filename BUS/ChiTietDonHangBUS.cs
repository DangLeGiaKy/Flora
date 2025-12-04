using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.DAL;
using test.DTO;

namespace test.BUS
{
    internal class ChiTietDonHangBUS
    {
        private ChiTietDonHangDAL dal = new ChiTietDonHangDAL();
        public bool Insert(ChiTietDonHangDTO c) => dal.Insert(c) > 0;
    }
}
