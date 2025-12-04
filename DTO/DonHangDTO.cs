using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DTO
{
    internal class DonHangDTO
    {
        public string MaDonHang { get; set; }
        public string MaKhachHang { get; set; }
        public string MaNhanVien { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public decimal TongTien { get; set; }
        public decimal TongGiaNhap { get; set; }
        public decimal LoiNhuan { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}
