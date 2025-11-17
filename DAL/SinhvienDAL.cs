using System.Data;
using System.Data.SqlClient;
using test.DTO;

namespace test.DAL
{
    internal class SinhvienDAL
    {
        private readonly Connection conn = new Connection();

        // Lấy tất cả sinh viên
        public DataTable GetAllSinhVien()
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                string query = "SELECT * FROM SinhVien";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Thêm sinh viên
        public bool InsertSinhVien(SinhvienDTO sv)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                string query = @"INSERT INTO SinhVien (MaSV, HoTen, GioiTinh, QueQuan)
                                 VALUES (@MaSV, @HoTen, @GioiTinh, @QueQuan)";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@MaSV", sv.MaSV);
                cmd.Parameters.AddWithValue("@HoTen", sv.HoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", sv.GioiTinh);
                cmd.Parameters.AddWithValue("@QueQuan", sv.QueQuan);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Cập nhật sinh viên
        public bool UpdateSinhVien(SinhvienDTO sv)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                string query = @"UPDATE SinhVien 
                                 SET HoTen=@HoTen, GioiTinh=@GioiTinh, QueQuan=@QueQuan
                                 WHERE MaSV=@MaSV";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@MaSV", sv.MaSV);
                cmd.Parameters.AddWithValue("@HoTen", sv.HoTen);
                cmd.Parameters.AddWithValue("@GioiTinh", sv.GioiTinh);
                cmd.Parameters.AddWithValue("@QueQuan", sv.QueQuan);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Xóa sinh viên
        public bool DeleteSinhVien(string maSV)
        {
            using (SqlConnection connection = conn.GetConnection())
            {
                string query = "DELETE FROM SinhVien WHERE MaSV=@MaSV";
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@MaSV", maSV);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
