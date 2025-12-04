using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using test.DAL;

namespace test
{
    /// <summary>
    /// Class xuất hóa đơn ra file PDF
    /// </summary>
    public static class HoaDonPDFExporter
    {
        /// <summary>
        /// Xuất hóa đơn ra PDF
        /// </summary>
        /// <param name="maHoaDon">Mã hóa đơn cần xuất</param>
        /// <returns>True nếu xuất thành công, False nếu thất bại</returns>
        public static bool ExportToPDF(string maHoaDon)
        {
            try
            {
                // Tạo SaveFileDialog để chọn nơi lưu file
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"HoaDon_{maHoaDon}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Lưu hóa đơn PDF"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lấy dữ liệu từ database
                    DataRow hoaDonData = GetHoaDonData(maHoaDon);
                    DataTable chiTietData = GetChiTietData(maHoaDon);

                    if (hoaDonData == null)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // Tạo PDF
                    CreatePDF(saveFileDialog.FileName, hoaDonData, chiTietData);

                    // Thông báo thành công
                    DialogResult result = MessageBox.Show(
                        $"Xuất hóa đơn PDF thành công!\n\nFile đã được lưu tại:\n{saveFileDialog.FileName}\n\nBạn có muốn mở file ngay?",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    // Mở file PDF nếu người dùng chọn Yes
                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Lấy thông tin hóa đơn từ database
        /// </summary>
        private static DataRow GetHoaDonData(string maHoaDon)
        {
            using (SqlConnection conn = Connection.connect())
            //using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
            {
                conn.Open();
                string query = @"SELECT 
                    h.MaHoaDon,
                    h.NgayLap,
                    k.HoTen AS TenKhachHang,
                    k.SoDienThoai,
                    k.DiaChi,
                    h.TongTien,
                    h.TienKhachDua,
                    h.TienThoiLai,
                    h.GhiChu
                FROM HoaDon h
                LEFT JOIN KhachHang k ON h.MaKhachHang = k.MaKhachHang
                WHERE h.MaHoaDon = @MaHoaDon";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }

        /// <summary>
        /// Lấy chi tiết hóa đơn từ database
        /// </summary>
        private static DataTable GetChiTietData(string maHoaDon)
        {
            using (SqlConnection conn = Connection.connect())
            //using (SqlConnection conn = new SqlConnection("Data Source=ANH-VU\\MSSQLSERVER01;Initial Catalog=FloraShopDB;Integrated Security=True"))
            {
                conn.Open();
                string query = @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY ct.MaChiTiet) AS STT,
                    k.TenSanPham,
                    ct.SoLuong,
                    ct.GiaBan,
                    ct.ThanhTien
                FROM ChiTietHoaDon ct
                INNER JOIN Kho k ON ct.MaSanPham = k.MaSanPham
                WHERE ct.MaHoaDon = @MaHoaDon";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaHoaDon", maHoaDon);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;
            }
        }

        /// <summary>
        /// Tạo file PDF
        /// </summary>
        private static void CreatePDF(string filePath, DataRow hoaDon, DataTable chiTiet)
        {
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // ===== FONT UNICODE (hỗ trợ tiếng Việt) =====
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
            BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            Font fontSubtitle = new Font(baseFont, 26, Font.BOLD);
            Font fontTitle = new Font(baseFont, 18, Font.BOLD);
            Font fontNormal = new Font(baseFont, 10, Font.NORMAL);
            Font fontBold = new Font(baseFont, 10, Font.BOLD);
            Font fontSmall = new Font(baseFont, 9, Font.NORMAL);

            // ===== HEADER VỚI LOGO =====
            PdfPTable headerTable = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 10
            };
            headerTable.SetWidths(new float[] { 1f, 4f });

            // Thêm Logo vào ô bên trái
            try
            {
                string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "3cb45bde-8b6f-4ed9-b4e1-117690c56c9f.png");
                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(70f, 70f);

                    PdfPCell logoCell = new PdfPCell(logo)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_TOP,
                        PaddingTop = 5
                    };
                    headerTable.AddCell(logoCell);
                }
                else
                {
                    // Nếu không tìm thấy logo, thêm ô trống
                    PdfPCell emptyCell = new PdfPCell(new Phrase(""))
                    {
                        Border = Rectangle.NO_BORDER
                    };
                    headerTable.AddCell(emptyCell);
                }
            }
            catch
            {
                // Nếu lỗi khi load logo, thêm ô trống
                PdfPCell emptyCell = new PdfPCell(new Phrase(""))
                {
                    Border = Rectangle.NO_BORDER
                };
                headerTable.AddCell(emptyCell);
            }

            // Thêm thông tin shop vào ô bên phải
            PdfPCell infoCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP
            };

            infoCell.AddElement(new Paragraph("CỬA HÀNG HOA FLORA SHOP", fontSubtitle) { SpacingBefore = 5 });
            infoCell.AddElement(new Paragraph("HÓA ĐƠN BÁN HÀNG", fontTitle));
            infoCell.AddElement(new Paragraph("Địa chỉ: 19 Nguyễn Hữu Thọ, phường Tân Hưng, TP.HCM", fontSmall) { SpacingBefore = 3 });
            infoCell.AddElement(new Paragraph("Điện thoại: 028.1234.5678", fontSmall));

            headerTable.AddCell(infoCell);
            document.Add(headerTable);

            // Thêm đường kẻ phân cách
            LineSeparator line = new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -2);
            document.Add(new Chunk(line));
            document.Add(new Paragraph(" ") { SpacingAfter = 10 });

            // ===== THÔNG TIN HÓA ĐƠN =====
            PdfPTable infoTable = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            infoTable.SetWidths(new float[] { 1f, 1f });

            // Cột trái
            PdfPCell leftCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 10
            };

            leftCell.AddElement(new Paragraph($"Mã hóa đơn: {hoaDon["MaHoaDon"]}", fontBold));
            leftCell.AddElement(new Paragraph($"Ngày lập: {Convert.ToDateTime(hoaDon["NgayLap"]):dd/MM/yyyy HH:mm:ss}", fontNormal));
            leftCell.AddElement(new Paragraph($"Nhân viên: {UserSession.HoTen}", fontNormal));

            // Cột phải
            PdfPCell rightCell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 10
            };

            rightCell.AddElement(new Paragraph($"Khách hàng: {hoaDon["TenKhachHang"]}", fontBold));
            rightCell.AddElement(new Paragraph($"Điện thoại: {hoaDon["SoDienThoai"]}", fontNormal));
            rightCell.AddElement(new Paragraph($"Địa chỉ: {hoaDon["DiaChi"]}", fontNormal));

            infoTable.AddCell(leftCell);
            infoTable.AddCell(rightCell);
            document.Add(infoTable);

            // ===== CHI TIẾT SẢN PHẨM =====
            PdfPTable detailTable = new PdfPTable(5)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            detailTable.SetWidths(new float[] { 0.5f, 3f, 1f, 1.5f, 1.5f });

            // Header bảng
            string[] headers = { "STT", "Tên sản phẩm", "Số lượng", "Đơn giá", "Thành tiền" };
            foreach (string header in headers)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(header, fontBold))
                {
                    BackgroundColor = new BaseColor(70, 130, 180),
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE,
                    Padding = 5
                };
                detailTable.AddCell(headerCell);
            }

            // Dữ liệu chi tiết
            foreach (DataRow row in chiTiet.Rows)
            {
                // STT
                detailTable.AddCell(new PdfPCell(new Phrase(row["STT"].ToString(), fontNormal))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                // Tên sản phẩm
                detailTable.AddCell(new PdfPCell(new Phrase(row["TenSanPham"].ToString(), fontNormal))
                {
                    Padding = 5
                });

                // Số lượng
                detailTable.AddCell(new PdfPCell(new Phrase(row["SoLuong"].ToString(), fontNormal))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 5
                });

                // Đơn giá
                decimal donGia = Convert.ToDecimal(row["GiaBan"]);
                detailTable.AddCell(new PdfPCell(new Phrase(donGia.ToString("N0"), fontNormal))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5
                });

                // Thành tiền
                decimal thanhTien = Convert.ToDecimal(row["ThanhTien"]);
                detailTable.AddCell(new PdfPCell(new Phrase(thanhTien.ToString("N0"), fontNormal))
                {
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    Padding = 5
                });
            }

            document.Add(detailTable);

            // ===== TỔNG TIỀN =====
            PdfPTable totalTable = new PdfPTable(2)
            {
                WidthPercentage = 50,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                SpacingAfter = 20
            };
            totalTable.SetWidths(new float[] { 2f, 1.5f });

            // Tổng hóa đơn
            totalTable.AddCell(new PdfPCell(new Phrase("Tổng tiền:", fontBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });
            totalTable.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(hoaDon["TongTien"]).ToString("N0") + " VNĐ", fontBold))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });

            // Tiền khách đưa
            totalTable.AddCell(new PdfPCell(new Phrase("Tiền khách đưa:", fontNormal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });
            totalTable.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(hoaDon["TienKhachDua"]).ToString("N0") + " VNĐ", fontNormal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });

            // Tiền thối lại
            totalTable.AddCell(new PdfPCell(new Phrase("Tiền thối lại:", fontNormal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });
            totalTable.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(hoaDon["TienThoiLai"]).ToString("N0") + " VNĐ", fontNormal))
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 5
            });

            document.Add(totalTable);

            // ===== GHI CHÚ =====
            if (!string.IsNullOrEmpty(hoaDon["GhiChu"].ToString()))
            {
                Paragraph ghiChu = new Paragraph($"Ghi chú: {hoaDon["GhiChu"]}", fontSmall)
                {
                    SpacingAfter = 20
                };
                document.Add(ghiChu);
            }

            // ===== FOOTER =====
            Paragraph footer = new Paragraph("Cảm ơn quý khách và hẹn gặp lại!", fontNormal)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 30
            };
            document.Add(footer);

            document.Close();
            writer.Close();
        }
    }
}