using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DTO
{
    internal class ChiTietDonHangDTO
    {
        public string MaChiTiet { get; set; }
        public string MaDonHang { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }
        public decimal GiaBan { get; set; }
        public decimal ThanhTien { get; set; }
        public decimal TongGiaNhap { get; set; }
        public decimal LoiNhuan { get; set; }
    }
}
