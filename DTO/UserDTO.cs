using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DTO
{
    internal class UserDTO
    {
        public string MaUser { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string VaiTro { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }

        public UserDTO()
        {
            TrangThai = true;
            NgayTao = DateTime.Now;
            NgayCapNhat = DateTime.Now;
        }

        public UserDTO(string maUser, string tenDangNhap, string matKhau, string hoTen,
                       string email, string soDienThoai, string vaiTro, bool trangThai)
        {
            MaUser = maUser;
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            HoTen = hoTen;
            Email = email;
            SoDienThoai = soDienThoai;
            VaiTro = vaiTro;
            TrangThai = trangThai;
            NgayTao = DateTime.Now;
            NgayCapNhat = DateTime.Now;
        }
    }
}
