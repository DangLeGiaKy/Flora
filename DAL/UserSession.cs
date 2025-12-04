using System;

namespace test
{
    /// <summary>
    /// Class lưu trữ thông tin phiên đăng nhập của người dùng
    /// </summary>
    public static class UserSession
    {
        public static string MaUser { get; set; }
        public static string TenDangNhap { get; set; }
        public static string HoTen { get; set; }
        public static string VaiTro { get; set; }
        public static string Email { get; set; }
        public static string SoDienThoai { get; set; }
        public static DateTime NgayDangNhap { get; set; }

        /// <summary>
        /// Kiểm tra xem có người dùng đang đăng nhập hay không
        /// </summary>
        public static bool IsLoggedIn
        {
            get { return !string.IsNullOrEmpty(MaUser); }
        }

        /// <summary>
        /// Xóa toàn bộ thông tin phiên đăng nhập
        /// </summary>
        public static void Clear()
        {
            MaUser = null;
            TenDangNhap = null;
            HoTen = null;
            VaiTro = null;
            Email = null;
            SoDienThoai = null;
            NgayDangNhap = DateTime.MinValue;
        }

        /// <summary>
        /// Kiểm tra quyền Admin
        /// </summary>
        public static bool IsAdmin()
        {
            return VaiTro == "Admin";
        }

        /// <summary>
        /// Kiểm tra quyền Quản lý
        /// </summary>
        public static bool IsQuanLy()
        {
            return VaiTro == "Quản lý" || VaiTro == "Admin";
        }
    }
}