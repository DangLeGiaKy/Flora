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
    internal class DonHangDAL
    {
        //private string connectionString ="Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True";
        private string connectionString = Connection.GetConnectionString();
        public DataTable GetAll()
        {
            var dt = new DataTable();
            string sql = @"
              SELECT dh.MaDonHang,
               kh.MaKhachHang,
               kh.HoTen AS TenKhachHang,
               kh.SoDienThoai,
               dh.TongTien,
               dh.NgayDat,
               dh.NgayGiao,
               dh.TrangThai,
               STRING_AGG(sp.TenSanPham + ' x' + CAST(ct.SoLuong AS NVARCHAR), ', ') AS SanPham
               FROM DonHang dh
               JOIN KhachHang kh ON dh.MaKhachHang = kh.MaKhachHang
               JOIN ChiTietDonHang ct ON dh.MaDonHang = ct.MaDonHang
               JOIN Kho sp ON ct.MaSanPham = sp.MaSanPham
               GROUP BY dh.MaDonHang, kh.MaKhach";

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


        public DataTable SearchByKhachHang(string tenKH)
        {
            var dt = new DataTable();
            string sql = @"SELECT d.*, k.HoTen, k.SoDienThoai 
                           FROM DonHang d
                           LEFT JOIN KhachHang k ON d.MaKhachHang = k.MaKhachHang
                           WHERE k.HoTen LIKE @Ten";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ten", "%" + tenKH + "%");
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int Insert(DonHangDTO d)
        {
            string sql = @"INSERT INTO DonHang
                (MaDonHang, MaKhachHang, MaNhanVien, NgayDat, NgayGiao, TongTien, TongGiaNhap, LoiNhuan, TrangThai, GhiChu)
                VALUES (@Ma, @KH, @NV, @NgayDat, @NgayGiao, @TongTien, @TongGiaNhap, @LoiNhuan, @TrangThai, @GhiChu)";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", d.MaDonHang);
                    cmd.Parameters.AddWithValue("@KH", d.MaKhachHang);
                    cmd.Parameters.AddWithValue("@NV", d.MaNhanVien);
                    cmd.Parameters.AddWithValue("@NgayDat", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NgayGiao", (object)d.NgayGiao ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TongTien", d.TongTien);
                    cmd.Parameters.AddWithValue("@TongGiaNhap", d.TongGiaNhap);
                    cmd.Parameters.AddWithValue("@LoiNhuan", d.LoiNhuan);
                    cmd.Parameters.AddWithValue("@TrangThai", d.TrangThai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", d.GhiChu ?? (object)DBNull.Value);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Update(DonHangDTO d)
        {
            string sql = @"UPDATE DonHang SET
                            MaKhachHang=@KH, MaNhanVien=@NV, NgayGiao=@NgayGiao,
                            TongTien=@TongTien, TongGiaNhap=@TongGiaNhap, LoiNhuan=@LoiNhuan,
                            TrangThai=@TrangThai, GhiChu=@GhiChu, NgayCapNhat=GETDATE()
                           WHERE MaDonHang=@Ma";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", d.MaDonHang);
                    cmd.Parameters.AddWithValue("@KH", d.MaKhachHang);
                    cmd.Parameters.AddWithValue("@NV", d.MaNhanVien);
                    cmd.Parameters.AddWithValue("@NgayGiao", (object)d.NgayGiao ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@TongTien", d.TongTien);
                    cmd.Parameters.AddWithValue("@TongGiaNhap", d.TongGiaNhap);
                    cmd.Parameters.AddWithValue("@LoiNhuan", d.LoiNhuan);
                    cmd.Parameters.AddWithValue("@TrangThai", d.TrangThai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@GhiChu", d.GhiChu ?? (object)DBNull.Value);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Delete(string maDonHang)
        {
            string sql = "DELETE FROM DonHang WHERE MaDonHang = @Ma";
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ma", maDonHang);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
