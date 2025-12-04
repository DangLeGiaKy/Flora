using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace test
{
    /// <summary>
    /// Class xuất báo cáo ra PDF
    /// </summary>
    public static class BaoCaoPDFExporter
    {
        public static bool ExportToPDF(DataTable data, string loaiBaoCao, DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    Title = "Lưu báo cáo PDF"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Document document = new Document(PageSize.A4.Rotate(), 25, 25, 30, 30);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                    document.Open();

                    // Font Unicode
                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf");
                    BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                    Font fontTitle = new Font(baseFont, 16, Font.BOLD);
                    Font fontSubtitle = new Font(baseFont, 36, Font.BOLD);
                    Font fontNormal = new Font(baseFont, 9, Font.NORMAL);
                    Font fontBold = new Font(baseFont, 9, Font.BOLD);
                    Font fontSmall = new Font(baseFont, 8, Font.NORMAL);

                    // Thêm Logo và Header trong một bảng
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
                            logo.ScaleToFit(130f, 130f);

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

                    // Thêm tiêu đề vào ô bên phải
                    Paragraph headerText = new Paragraph("CỬA HÀNG HOA FLORA SHOP", fontSubtitle);
                    PdfPCell titleCell = new PdfPCell(headerText)
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    headerTable.AddCell(titleCell);

                    document.Add(headerTable);

                    Paragraph reportTitle = new Paragraph(loaiBaoCao.ToUpper(), fontTitle)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 10
                    };
                    document.Add(reportTitle);

                    // Thời gian báo cáo
                    Paragraph timeRange = new Paragraph(
                        $"Từ ngày: {tuNgay:dd/MM/yyyy} - Đến ngày: {denNgay:dd/MM/yyyy}",
                        fontNormal)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 15
                    };
                    document.Add(timeRange);

                    // Tạo bảng theo loại báo cáo
                    switch (loaiBaoCao)
                    {
                        case "Báo cáo theo doanh thu và lợi nhuận":
                            CreateDoanhThuTable(document, data, fontNormal, fontBold);
                            break;
                        case "Báo cáo theo sản phẩm đã bán ra":
                            CreateSanPhamTable(document, data, fontNormal, fontBold);
                            break;
                        case "Báo cáo nhập hàng":
                            CreateNhapHangTable(document, data, fontNormal, fontBold);
                            break;
                        case "Báo cáo tồn kho":
                            CreateTonKhoTable(document, data, fontNormal, fontBold);
                            break;
                    }

                    // Footer
                    Paragraph footer = new Paragraph(
                        $"Ngày xuất báo cáo: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                        fontSmall)
                    {
                        Alignment = Element.ALIGN_RIGHT,
                        SpacingBefore = 20
                    };
                    document.Add(footer);

                    document.Close();
                    writer.Close();

                    DialogResult result = MessageBox.Show(
                        $"Xuất báo cáo PDF thành công!\n\nFile: {saveFileDialog.FileName}\n\nMở file ngay?",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

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
                MessageBox.Show($"Lỗi xuất PDF: {ex.Message}\n\nStackTrace: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static string GetColumnValue(DataRow row, string columnName, string defaultValue = "")
        {
            try
            {
                if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
                {
                    return row[columnName].ToString();
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static void CreateDoanhThuTable(Document document, DataTable data, Font fontNormal, Font fontBold)
        {
            PdfPTable table = new PdfPTable(6)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            table.SetWidths(new float[] { 1.5f, 1f, 1.5f, 1.5f, 1.5f, 1f });

            string[] headers = { "Ngày", "Số HĐ", "Doanh Thu", "Chi Phí", "Lợi Nhuận", "Tỷ Lệ (%)" };
            foreach (string header in headers)
            {
                AddHeaderCell(table, header, fontBold);
            }

            decimal tongDoanhThu = 0, tongChiPhi = 0, tongLoiNhuan = 0;

            foreach (DataRow row in data.Rows)
            {
                // Sử dụng tên cột chính xác từ query
                string ngay = GetColumnValue(row, "Ngay");
                if (!string.IsNullOrEmpty(ngay))
                {
                    AddCell(table, Convert.ToDateTime(row["Ngay"]).ToString("dd/MM/yyyy"), fontNormal, Element.ALIGN_CENTER);
                }

                AddCell(table, GetColumnValue(row, "SoHoaDon", "0"), fontNormal, Element.ALIGN_CENTER);

                decimal doanhThu = Convert.ToDecimal(GetColumnValue(row, "DoanhThu", "0"));
                AddCell(table, doanhThu.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal chiPhi = Convert.ToDecimal(GetColumnValue(row, "TongChiPhi", "0"));
                AddCell(table, chiPhi.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal loiNhuan = Convert.ToDecimal(GetColumnValue(row, "LoiNhuan", "0"));
                AddCell(table, loiNhuan.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal tyLe = Convert.ToDecimal(GetColumnValue(row, "TyLeLoiNhuan", "0"));
                AddCell(table, tyLe.ToString("N2") + "%", fontNormal, Element.ALIGN_RIGHT);

                tongDoanhThu += doanhThu;
                tongChiPhi += chiPhi;
                tongLoiNhuan += loiNhuan;
            }

            // Tổng cộng
            AddCell(table, "TỔNG CỘNG", fontBold, Element.ALIGN_CENTER);
            AddCell(table, data.Rows.Count.ToString(), fontBold, Element.ALIGN_CENTER);
            AddCell(table, tongDoanhThu.ToString("N0"), fontBold, Element.ALIGN_RIGHT);
            AddCell(table, tongChiPhi.ToString("N0"), fontBold, Element.ALIGN_RIGHT);
            AddCell(table, tongLoiNhuan.ToString("N0"), fontBold, Element.ALIGN_RIGHT);
            AddCell(table, tongChiPhi > 0 ? ((tongLoiNhuan / tongChiPhi * 100).ToString("N2") + "%") : "0%", fontBold, Element.ALIGN_RIGHT);

            document.Add(table);
        }

        private static void CreateSanPhamTable(Document document, DataTable data, Font fontNormal, Font fontBold)
        {
            PdfPTable table = new PdfPTable(8)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            table.SetWidths(new float[] { 1f, 2.5f, 1.5f, 1f, 1.5f, 1.5f, 1.5f, 1f });

            string[] headers = { "Mã SP", "Tên Sản Phẩm", "Loại", "SL Bán", "Doanh Thu", "Chi Phí", "Lợi Nhuận", "TL (%)" };
            foreach (string header in headers)
            {
                AddHeaderCell(table, header, fontBold);
            }

            foreach (DataRow row in data.Rows)
            {
                AddCell(table, GetColumnValue(row, "MaSanPham"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "TenSanPham"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "LoaiHang"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "TongSoLuongBan", "0"), fontNormal, Element.ALIGN_CENTER);

                decimal doanhThu = Convert.ToDecimal(GetColumnValue(row, "DoanhThu", "0"));
                AddCell(table, doanhThu.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal chiPhi = Convert.ToDecimal(GetColumnValue(row, "ChiPhi", "0"));
                AddCell(table, chiPhi.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal loiNhuan = Convert.ToDecimal(GetColumnValue(row, "LoiNhuan", "0"));
                AddCell(table, loiNhuan.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal tyLe = Convert.ToDecimal(GetColumnValue(row, "TyLeLoiNhuan", "0"));
                AddCell(table, tyLe.ToString("N2") + "%", fontNormal, Element.ALIGN_RIGHT);
            }

            document.Add(table);
        }

        private static void CreateNhapHangTable(Document document, DataTable data, Font fontNormal, Font fontBold)
        {
            PdfPTable table = new PdfPTable(7)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            table.SetWidths(new float[] { 1.5f, 1.5f, 2f, 1.5f, 1.5f, 1.2f, 2f });

            string[] headers = { "Mã Phiếu", "Ngày Nhập", "Nhà CC", "Nhân Viên", "Tổng Tiền", "Trạng Thái", "Ghi Chú" };
            foreach (string header in headers)
            {
                AddHeaderCell(table, header, fontBold);
            }

            decimal tongTien = 0;

            foreach (DataRow row in data.Rows)
            {
                AddCell(table, GetColumnValue(row, "MaPhieuNhap"), fontNormal, Element.ALIGN_LEFT);

                string ngayNhap = GetColumnValue(row, "NgayNhap");
                if (!string.IsNullOrEmpty(ngayNhap))
                {
                    AddCell(table, Convert.ToDateTime(row["NgayNhap"]).ToString("dd/MM/yyyy"), fontNormal, Element.ALIGN_CENTER);
                }
                else
                {
                    AddCell(table, "", fontNormal, Element.ALIGN_CENTER);
                }

                AddCell(table, GetColumnValue(row, "TenNhaCungCap"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "NhanVien"), fontNormal, Element.ALIGN_LEFT);

                decimal tien = Convert.ToDecimal(GetColumnValue(row, "TongTien", "0"));
                AddCell(table, tien.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                AddCell(table, GetColumnValue(row, "TrangThai"), fontNormal, Element.ALIGN_CENTER);
                AddCell(table, GetColumnValue(row, "GhiChu"), fontNormal, Element.ALIGN_LEFT);

                tongTien += tien;
            }

            // Tổng
            for (int i = 0; i < 4; i++)
                AddCell(table, i == 0 ? "TỔNG CỘNG" : "", fontBold, Element.ALIGN_CENTER);
            AddCell(table, tongTien.ToString("N0"), fontBold, Element.ALIGN_RIGHT);
            AddCell(table, "", fontBold, Element.ALIGN_CENTER);
            AddCell(table, "", fontBold, Element.ALIGN_CENTER);

            document.Add(table);
        }

        private static void CreateTonKhoTable(Document document, DataTable data, Font fontNormal, Font fontBold)
        {
            PdfPTable table = new PdfPTable(8)
            {
                WidthPercentage = 100,
                SpacingAfter = 15
            };
            table.SetWidths(new float[] { 1f, 2.5f, 1.5f, 1.2f, 1.2f, 1f, 1.5f, 1.2f });

            string[] headers = { "Mã SP", "Tên SP", "Loại", "Giá Nhập", "Giá Bán", "Tồn", "Giá Trị Tồn", "TT" };
            foreach (string header in headers)
            {
                AddHeaderCell(table, header, fontBold);
            }

            foreach (DataRow row in data.Rows)
            {
                AddCell(table, GetColumnValue(row, "MaSanPham"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "TenSanPham"), fontNormal, Element.ALIGN_LEFT);
                AddCell(table, GetColumnValue(row, "LoaiHang"), fontNormal, Element.ALIGN_LEFT);

                decimal giaNhap = Convert.ToDecimal(GetColumnValue(row, "GiaNhap", "0"));
                AddCell(table, giaNhap.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                decimal giaBan = Convert.ToDecimal(GetColumnValue(row, "GiaBan", "0"));
                AddCell(table, giaBan.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                string ton = GetColumnValue(row, "SoLuongTon", "0") + " " + GetColumnValue(row, "DonViTinh");
                AddCell(table, ton, fontNormal, Element.ALIGN_CENTER);

                decimal giaTriTon = Convert.ToDecimal(GetColumnValue(row, "GiaTriTonKho", "0"));
                AddCell(table, giaTriTon.ToString("N0"), fontNormal, Element.ALIGN_RIGHT);

                AddCell(table, GetColumnValue(row, "TrangThaiKho"), fontNormal, Element.ALIGN_CENTER);
            }

            document.Add(table);
        }

        private static void AddHeaderCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                BackgroundColor = new BaseColor(70, 130, 180),
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5
            };
            table.AddCell(cell);
        }

        private static void AddCell(PdfPTable table, string text, Font font, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                HorizontalAlignment = alignment,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5
            };
            table.AddCell(cell);
        }
    }
}