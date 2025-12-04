using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.DTO;

namespace test.DAL
{
    internal class UserDAL
    {
        private string connectionString = Connection.GetConnectionString();
        //private string connectionString = "Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        // Hoặc: "Data Source=YOUR_SERVER;Initial Catalog=YOUR_DATABASE;User ID=sa;Password=your_password"

        // Lấy tất cả user
        public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MaUser, TenDangNhap,Matkhau, HoTen, Email, SoDienThoai, VaiTro, TrangThai, NgayTao, NgayCapNhat FROM [User] ORDER BY NgayTao DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách user: " + ex.Message);
            }
            return dt;
        }

        // Lấy user theo ID
        public UserDTO GetUserById(string maUser)
        {
            UserDTO user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM [User] WHERE MaUser = @MaUser";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaUser", maUser);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        user = new UserDTO
                        {
                            MaUser = reader["MaUser"].ToString(),
                            TenDangNhap = reader["TenDangNhap"].ToString(),
                            MatKhau = reader["MatKhau"].ToString(),
                            HoTen = reader["HoTen"].ToString(),
                            Email = reader["Email"].ToString(),
                            SoDienThoai = reader["SoDienThoai"].ToString(),
                            VaiTro = reader["VaiTro"].ToString(),
                            TrangThai = Convert.ToBoolean(reader["TrangThai"]),
                            NgayTao = Convert.ToDateTime(reader["NgayTao"]),
                            NgayCapNhat = Convert.ToDateTime(reader["NgayCapNhat"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin user: " + ex.Message);
            }
            return user;
        }

        // Thêm user mới
        public bool InsertUser(UserDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO [User] (MaUser, TenDangNhap, MatKhau, HoTen, Email, SoDienThoai, VaiTro, TrangThai, NgayTao, NgayCapNhat)
                                   VALUES (@MaUser, @TenDangNhap, @MatKhau, @HoTen, @Email, @SoDienThoai, @VaiTro, @TrangThai, @NgayTao, @NgayCapNhat)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaUser", user.MaUser);
                    cmd.Parameters.AddWithValue("@TenDangNhap", user.TenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", user.MatKhau);
                    cmd.Parameters.AddWithValue("@HoTen", user.HoTen);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoDienThoai", user.SoDienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VaiTro", user.VaiTro);
                    cmd.Parameters.AddWithValue("@TrangThai", user.TrangThai);
                    cmd.Parameters.AddWithValue("@NgayTao", user.NgayTao);
                    cmd.Parameters.AddWithValue("@NgayCapNhat", user.NgayCapNhat);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm user: " + ex.Message);
            }
        }

        // Cập nhật user
        public bool UpdateUser(UserDTO user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"UPDATE [User] SET 
                                   HoTen = @HoTen,
                                   Email = @Email,
                                   SoDienThoai = @SoDienThoai,
                                   VaiTro = @VaiTro,
                                   TrangThai = @TrangThai,
                                   NgayCapNhat = @NgayCapNhat
                                   WHERE MaUser = @MaUser";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaUser", user.MaUser);
                    cmd.Parameters.AddWithValue("@HoTen", user.HoTen);
                    cmd.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoDienThoai", user.SoDienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VaiTro", user.VaiTro);
                    cmd.Parameters.AddWithValue("@TrangThai", user.TrangThai);
                    cmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật user: " + ex.Message);
            }
        }
        public string GenerateMaUser()
        {
            string newID = "U001";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT TOP 1 MaUser FROM [User] ORDER BY MaUser DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string lastID = result.ToString(); // U012
                    int number = int.Parse(lastID.Substring(1)) + 1;
                    newID = "U" + number.ToString("000");
                }
            }
            return newID;
        }
        // Xóa user
        public int DeleteUser(string maUser)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Kiểm tra user có đang đăng nhập không
                    string checkLogin = "SELECT IsLoggedIn FROM [User] WHERE MaUser = @MaUser";
                    using (SqlCommand checkCmd = new SqlCommand(checkLogin, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaUser", maUser);
                        object result = checkCmd.ExecuteScalar();

                        if (result != null && Convert.ToBoolean(result) == true)
                        {
                            return -1;  // user đang đăng nhập
                        }
                    }

                    // 2. Xóa user
                    string query = "DELETE FROM [User] WHERE MaUser = @MaUser";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaUser", maUser);

                        int rows = cmd.ExecuteNonQuery();
                        return rows; // >0 ok, 0 không có user
                    }
                }
            }
            catch
            {
                return -2;  // lỗi database, lỗi kết nối, lỗi bất kỳ
            }
        }


        // Đổi mật khẩu
        public bool ChangePassword(string maUser, string matKhauMoi)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE [User] SET MatKhau = @MatKhau, NgayCapNhat = @NgayCapNhat WHERE MaUser = @MaUser";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaUser", maUser);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhauMoi);
                    cmd.Parameters.AddWithValue("@NgayCapNhat", DateTime.Now);

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đổi mật khẩu: " + ex.Message);
            }
        }

        // Tìm kiếm user
        public DataTable SearchUsers(string keyword)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string sql = @"
                SELECT * FROM [User]
                WHERE 
                    TenDangNhap LIKE @kw OR
                    HoTen LIKE @kw OR
                    Email LIKE @kw OR
                    SoDienThoai LIKE @kw";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tìm kiếm user: " + ex.Message);
            }

            return dt;
        }


        // Kiểm tra tên đăng nhập đã tồn tại
        public bool CheckTenDangNhapExists(string tenDangNhap)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM [User] WHERE TenDangNhap = @TenDangNhap";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TenDangNhap", tenDangNhap);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kiểm tra tên đăng nhập: " + ex.Message);
            }
        }
    }
}
