using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DAL
{
    internal class KhachHangDAL
    {
        //private string connectionString ="Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();
        public DataTable GetAll()
        {
            var dt = new DataTable();
            string sql = "SELECT MaKhachHang, HoTen, SoDienThoai FROM KhachHang";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var da = new SqlDataAdapter(sql, conn))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public DataRow GetById(string ma)
        {
            var dt = new DataTable();
            string sql = "SELECT MaKhachHang, HoTen, SoDienThoai FROM KhachHang WHERE MaKhachHang = @Ma";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", ma);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        
    }
}
