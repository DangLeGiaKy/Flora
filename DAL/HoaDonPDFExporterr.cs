using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace test.DAL
{
    internal class HoaDonPDFExporterr
    {
        public static void ExportToPDF(string maDonHang)
        {
            try
            {
                // Lấy thông tin đơn hàng + chi tiết từ DB
                DataTable dtDonHang = new DataTable();
                DataTable dtChiTiet = new DataTable();
                //using (SqlConnection conn = new SqlConnection(@"Data Source=ANH-VU\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
                using (SqlConnection conn = Connection.connect())
                {
                    conn.Open();

                    string sqlDH = @"SELECT dh.MaDonHang, kh.HoTen, kh.SoDienThoai, dh.NgayDat, dh.TongTien
                                     FROM DonHang dh
                                     JOIN KhachHang kh ON dh.MaKhachHang = kh.MaKhachHang
                                     WHERE dh.MaDonHang=@MaDH";
                    SqlDataAdapter daDH = new SqlDataAdapter(sqlDH, conn);
                    daDH.SelectCommand.Parameters.AddWithValue("@MaDH", maDonHang);
                    daDH.Fill(dtDonHang);

                    string sqlCT = @"SELECT sp.TenSanPham, ct.SoLuong, ct.GiaBan, ct.ThanhTien
                                     FROM ChiTietDonHang ct
                                     JOIN Kho sp ON ct.MaSanPham = sp.MaSanPham
                                     WHERE ct.MaDonHang=@MaDH";
                    SqlDataAdapter daCT = new SqlDataAdapter(sqlCT, conn);
                    daCT.SelectCommand.Parameters.AddWithValue("@MaDH", maDonHang);
                    daCT.Fill(dtChiTiet);
                }

                if (dtDonHang.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn này!");
                    return;
                }

                // Chọn nơi lưu PDF
                SaveFileDialog save = new SaveFileDialog
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{maDonHang}.pdf"
                };
                if (save.ShowDialog() != DialogResult.OK) return;

                Document document = new Document(PageSize.A4, 25, 25, 20, 20);
                PdfWriter.GetInstance(document, new FileStream(save.FileName, FileMode.Create));
                document.Open();

                // Font Unicode
                string fontPathNormal = Path.Combine(Application.StartupPath, "Fonts", "arial.ttf");
                string fontPathBold = Path.Combine(Application.StartupPath, "Fonts", "arialbd.ttf");

                BaseFont bfNormal = BaseFont.CreateFont(fontPathNormal, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont bfBold = BaseFont.CreateFont(fontPathBold, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                Font fontNormal = new Font(bfNormal, 12);
                Font fontBold = new Font(bfBold, 16, Font.BOLD);
                Font fontHeader = new Font(bfBold, 12, Font.BOLD);

                // HEADER TABLE LOGO + TÊN SHOP
                PdfPTable headerTable = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingAfter = 20
                };
                headerTable.SetWidths(new float[] { 1f, 4f });

                // Logo bên trái
                string logoPath = Path.Combine(Application.StartupPath, "Resources", "3cb45bde-8b6f-4ed9-b4e1-117690c56c9f.png");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(100f, 100f);
                    PdfPCell logoCell = new PdfPCell(logo)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    headerTable.AddCell(logoCell);
                }
                else
                {
                    headerTable.AddCell(new PdfPCell(new Phrase("")) { Border = Rectangle.NO_BORDER });
                }

                // Tên shop bên phải
                PdfPCell titleCell = new PdfPCell(new Phrase("CỬA HÀNG HOA FLORA SHOP", fontBold))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                };
                headerTable.AddCell(titleCell);

                // Thêm header table vào document
                document.Add(headerTable);

                // Thông tin khách hàng
                DataRow dh = dtDonHang.Rows[0];
                document.Add(new Paragraph($"Mã Hóa Đơn: {dh["MaDonHang"]}", fontNormal));
                document.Add(new Paragraph($"Khách Hàng: {dh["HoTen"]}", fontNormal));
                document.Add(new Paragraph($"Số điện thoại: {dh["SoDienThoai"]}", fontNormal));
                document.Add(new Paragraph($"Ngày đặt: {Convert.ToDateTime(dh["NgayDat"]).ToString("dd/MM/yyyy HH:mm")}", fontNormal));
                document.Add(new Paragraph("\n"));

                // Bảng sản phẩm
                PdfPTable table = new PdfPTable(4) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 50, 15, 20, 25 });

                string[] headers = { "Sản phẩm", "SL", "Đơn giá", "Thành tiền" };
                foreach (var h in headers)
                    table.AddCell(new PdfPCell(new Phrase(h, fontHeader)) { HorizontalAlignment = Element.ALIGN_CENTER });

                foreach (DataRow row in dtChiTiet.Rows)
                {
                    table.AddCell(new PdfPCell(new Phrase(row["TenSanPham"].ToString(), fontNormal)));
                    table.AddCell(new PdfPCell(new Phrase(row["SoLuong"].ToString(), fontNormal)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["GiaBan"]).ToString("N0"), fontNormal)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(row["ThanhTien"]).ToString("N0"), fontNormal)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                document.Add(table);

                // Tổng tiền
                decimal tongTien = Convert.ToDecimal(dh["TongTien"]);
                Paragraph total = new Paragraph($"\nTỔNG TIỀN: {tongTien:N0} VNĐ", fontBold) { Alignment = Element.ALIGN_RIGHT };
                document.Add(total);

                document.Close();

                MessageBox.Show($"In hóa đơn thành công!\n{save.FileName}");
                System.Diagnostics.Process.Start(save.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message);
            }
        }
    }
}
