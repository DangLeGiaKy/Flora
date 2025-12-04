using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.DTO;

namespace test.DAL
{
    internal class ChiTietDonHangDAL
    {
        //private string connectionString = "Data Source=khanhvinh\\SQLEXPRESS;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();

        public int Insert(ChiTietDonHangDTO c)
        {
            string sql = @"INSERT INTO ChiTietDonHang
                (MaChiTiet, MaDonHang, MaSanPham, SoLuong, GiaNhap, GiaBan, ThanhTien, TongGiaNhap, LoiNhuan)
                VALUES (@MaCT, @MaDH, @SP, @SL, @GN, @GB, @TT, @TGN, @LN)";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@MaCT", c.MaChiTiet);
                    cmd.Parameters.AddWithValue("@MaDH", c.MaDonHang);
                    cmd.Parameters.AddWithValue("@SP", c.MaSanPham);
                    cmd.Parameters.AddWithValue("@SL", c.SoLuong);
                    cmd.Parameters.AddWithValue("@GN", c.GiaNhap);
                    cmd.Parameters.AddWithValue("@GB", c.GiaBan);
                    cmd.Parameters.AddWithValue("@TT", c.ThanhTien);
                    cmd.Parameters.AddWithValue("@TGN", c.TongGiaNhap);
                    cmd.Parameters.AddWithValue("@LN", c.LoiNhuan);

                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
