using System.Data;
using test.DAL;

namespace test.BUS
{
    internal class BUS_KhachHang
    {
        DAL_KhachHang dal = new DAL_KhachHang();

        public DataTable LayDanhSachKhachHang()
        {
            return dal.GetAllKhachHang();
        }
        public bool CapNhatKhachHang(string ma, string ten, string sdt, string email, string diachi, string ghichu)
        {
            return dal.UpdateKhachHang(ma, ten, sdt, email, diachi, ghichu);
        }
        public bool XoaKhachHang(string ma, out string message)
        {
            return dal.DeleteKhachHang(ma, out message);
        }
        public string TaoMaKhachHangTuDong()
        {
            return dal.TaoMaKhachHangTuDong();
        }

        public bool ThemKhachHang(string ma, string ten, string sdt, string email, string diachi, string ghichu)
        {
            return dal.InsertKhachHang(ma, ten, sdt, email, diachi, ghichu);
        }
    }
}
