using System.Data;
using test.DAL;

namespace test.BUS
{
    internal class nhacungcap
    {
        DAL_NhaCungCap dal = new DAL_NhaCungCap();

        public DataTable LayDanhSachNCC()
        {
            return dal.GetAllNCC();
        }

        public string TaoMaNhaCungCapTuDong()
        {
            return dal.TaoMaNhaCungCapTuDong();
        }

        public bool ThemNCC(string ma, string ten, string sdt, string email, string diachi, string loai, string ghichu)
        {
            return dal.ThemNCC(ma, ten, sdt, email, diachi, loai, ghichu);
        }
    }
}
