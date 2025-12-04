using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using test.DAL;
using test.DTO;

namespace test.BUS
{
    internal class QuanLyUser
    {
        private UserDAL userDAL = new UserDAL();

        // Lấy tất cả user
        public DataTable GetAllUsers()
        {
            return userDAL.GetAllUsers();
        }

        // Lấy user theo ID
        public UserDTO GetUserById(string maUser)
        {
            if (string.IsNullOrEmpty(maUser))
            {
                throw new Exception("Mã user không được để trống!");
            }
            return userDAL.GetUserById(maUser);
        }

        // Thêm user mới
        public bool AddUser(UserDTO user)
        {
            // Validate
            ValidateUser(user);

            // Kiểm tra tên đăng nhập đã tồn tại
            if (userDAL.CheckTenDangNhapExists(user.TenDangNhap))
            {
                throw new Exception("Tên đăng nhập đã tồn tại!");
            }

            // Mã hóa mật khẩu (khuyến nghị dùng BCrypt hoặc SHA256)
            user.MatKhau = HashPassword(user.MatKhau);

            return userDAL.InsertUser(user);
        }

        // Cập nhật user
        public bool UpdateUser(UserDTO user)
        {
            // Validate (không validate mật khẩu khi update)

            if (string.IsNullOrEmpty(user.MaUser))
            {
                throw new Exception("Mã user không được để trống!");
            }

            if (string.IsNullOrEmpty(user.HoTen))
            {
                throw new Exception("Họ tên không được để trống!");
            }

            if (!string.IsNullOrEmpty(user.Email) && !IsValidEmail(user.Email))
            {
                throw new Exception("Email không hợp lệ!");
            }

            if (!string.IsNullOrEmpty(user.SoDienThoai) && !IsValidPhoneNumber(user.SoDienThoai))
            {
                throw new Exception("Số điện thoại không hợp lệ!");
            }

            if (string.IsNullOrEmpty(user.VaiTro))
            {
                throw new Exception("Vai trò không được để trống!");
            }

            return userDAL.UpdateUser(user);
        }

        // Xóa user
        public int DeleteUser(string maUser)
        {
            if (string.IsNullOrEmpty(maUser))
                return -2;

            return userDAL.DeleteUser(maUser);
        }


        // Đổi mật khẩu
        public bool ChangePassword(string maUser, string matKhauCu, string matKhauMoi)
        {
            if (string.IsNullOrEmpty(maUser))
            {
                throw new Exception("Mã user không được để trống!");
            }

            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi))
            {
                throw new Exception("Mật khẩu không được để trống!");
            }

            if (matKhauMoi.Length < 6)
            {
                throw new Exception("Mật khẩu mới phải có ít nhất 6 ký tự!");
            }

            // Lấy thông tin user
            UserDTO user = userDAL.GetUserById(maUser);
            if (user == null)
            {
                throw new Exception("Không tìm thấy user!");
            }

            // Kiểm tra mật khẩu cũ
            if (user.MatKhau != HashPassword(matKhauCu))
            {
                throw new Exception("Mật khẩu cũ không đúng!");
            }

            // Đổi mật khẩu
            return userDAL.ChangePassword(maUser, HashPassword(matKhauMoi));
        }

        // Tìm kiếm user
        public DataTable SearchUsers(string keyword)
        {
            return userDAL.SearchUsers(keyword);
        }

        // Validate user
        private void ValidateUser(UserDTO user)
        {
            if (string.IsNullOrEmpty(user.TenDangNhap))
            {
                throw new Exception("Tên đăng nhập không được để trống!");
            }

            if (user.TenDangNhap.Length < 6 || user.TenDangNhap.Length > 50)
            {
                throw new Exception("Tên đăng nhập phải từ 6-50 ký tự!");
            }

            if (string.IsNullOrEmpty(user.MatKhau))
            {
                throw new Exception("Mật khẩu không được để trống!");
            }

            if (user.MatKhau.Length < 6)
            {
                throw new Exception("Mật khẩu phải có ít nhất 6 ký tự!");
            }

            if (string.IsNullOrEmpty(user.HoTen))
            {
                throw new Exception("Họ tên không được để trống!");
            }

            if (!string.IsNullOrEmpty(user.Email) && !IsValidEmail(user.Email))
            {
                throw new Exception("Email không hợp lệ!");
            }

            if (!string.IsNullOrEmpty(user.SoDienThoai) && !IsValidPhoneNumber(user.SoDienThoai))
            {
                throw new Exception("Số điện thoại không hợp lệ!");
            }

            if (string.IsNullOrEmpty(user.VaiTro))
            {
                throw new Exception("Vai trò không được để trống!");
            }
        }

        // Kiểm tra email hợp lệ
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        // Kiểm tra số điện thoại hợp lệ
        private bool IsValidPhoneNumber(string phone)
        {
            string pattern = @"^[0-9]{10,11}$";
            return Regex.IsMatch(phone, pattern);
        }

        // Mã hóa mật khẩu (đơn giản - khuyến nghị dùng BCrypt)
        private string HashPassword(string password)
        {

            return password;
            
        }

        // Tạo mã user tự động
        public string GenerateMaUser()
        {
            return userDAL.GenerateMaUser();
        }
    }
}
