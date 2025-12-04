using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.DAL
{
    internal class SanPhamDAL
    {
        //private string connectionString ="Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();
        public DataTable GetAll()
        {
            var dt = new DataTable();
            string sql = "SELECT MaSanPham, TenSanPham, GiaNhap, GiaBan, SoLuongTon FROM Kho WHERE TrangThai = 1";
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

        public DataRow GetById(string maSP)
        {
            var dt = new DataTable();
            string sql = "SELECT MaSanPham, TenSanPham, GiaNhap, GiaBan, SoLuongTon FROM Kho WHERE MaSanPham = @Ma";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maSP);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public int UpdateSoLuong(string maSP, int delta)
        {
            // delta có thể âm (trừ) hoặc dương (tăng)
            string sql = "UPDATE Kho SET SoLuongTon = SoLuongTon + @Delta WHERE MaSanPham = @Ma";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Delta", delta);
                    cmd.Parameters.AddWithValue("@Ma", maSP);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
