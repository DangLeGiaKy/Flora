using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace test.DAL
{
    internal class DAL_KhachHang
    {
        //private readonly string connectionString = @"Data Source=ANH-VU\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        //private readonly Connection connectionString = new Connection();
        private string connectionString = Connection.GetConnectionString();

        //private readonly string connectionString = @"Data Source=khanhvinh\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True";
        public DataTable GetAllKhachHang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM KhachHang";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public bool UpdateKhachHang(string ma, string ten, string sdt, string email, string diachi, string ghichu)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE KhachHang SET 
                        HoTen=@ten,
                        SoDienThoai=@sdt,
                        Email=@email,
                        DiaChi=@diachi,
                        GhiChu=@ghichu,
                        NgayCapNhat=GETDATE()
                       WHERE MaKhachHang=@ma";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@ghichu", ghichu);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool DeleteKhachHang(string ma, out string message)
        {
            message = "";
            string sql = "DELETE FROM KhachHang WHERE MaKhachHang = @Ma";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Ma", ma);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            message = "Xóa khách hàng thành công!";
                            return true;
                        }
                        else
                        {
                            message = "Không tìm thấy khách hàng để xóa.";
                            return false;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547) // FK constraint
                    {
                        message = "Không thể xóa khách hàng này vì đang tồn tại hóa đơn liên quan.";
                    }
                    else
                    {
                        message = "Lỗi SQL: " + ex.Message;
                    }
                    return false;
                }
            }
        }
        public string TaoMaKhachHangTuDong()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT TOP 1 MaKhachHang FROM KhachHang ORDER BY MaKhachHang DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result == null)
                    return "KH001";

                string maCu = result.ToString();   // VD: KH005
                int so = int.Parse(maCu.Substring(2));  // lấy 005 → 5
                so++;

                return "KH" + so.ToString("000");  // → KH006
            }
        }

        public bool InsertKhachHang(string ma, string ten, string sdt, string email, string diachi, string ghichu)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO KhachHang 
                                (MaKhachHang, HoTen, SoDienThoai, Email, DiaChi, GhiChu)
                               VALUES
                                (@ma, @ten, @sdt, @email, @diachi, @ghichu)";

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ma", ma);
                cmd.Parameters.AddWithValue("@ten", ten);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@diachi", diachi);
                cmd.Parameters.AddWithValue("@ghichu", ghichu);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
