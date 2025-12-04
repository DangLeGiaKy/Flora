using System;
using System.Data;
using System.Data.SqlClient;

namespace test.DAL
{
    internal class DAL_NhaCungCap
    {
        
        //private readonly string connectionString =@"Data Source=ANH-VU\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        //private readonly string connectionString = @"Data Source=khanhvinh\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();
        // Lấy danh sách NCC
        public DataTable GetAllNCC()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM NhaCungCap";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Sinh mã NCC tự động
        public string TaoMaNhaCungCapTuDong()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT TOP 1 MaNhaCungCap FROM NhaCungCap ORDER BY MaNhaCungCap DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result == null)
                    return "NCC001";

                string maCu = result.ToString(); // Ví dụ NCC015
                int so = int.Parse(maCu.Substring(3));
                so++;

                return "NCC" + so.ToString("000");
            }
        }

        // Thêm nhà cung cấp
        public bool ThemNCC(string ma, string ten, string sdt, string email, string diachi, string loai, string ghichu)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO NhaCungCap 
                                (MaNhaCungCap, TenNhaCungCap, SoDienThoai, Email, DiaChi, LoaiHangCungCap, GhiChu, NgayTao, NgayCapNhat)
                               VALUES 
                                (@Ma, @Ten, @Sdt, @Email, @DiaChi, @Loai, @GhiChu, GETDATE(), GETDATE())";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Ma", ma);
                cmd.Parameters.AddWithValue("@Ten", ten);
                cmd.Parameters.AddWithValue("@Sdt", sdt);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@DiaChi", diachi);
                cmd.Parameters.AddWithValue("@Loai", loai);
                cmd.Parameters.AddWithValue("@GhiChu", ghichu);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows > 0;
            }
        }
    }
}
