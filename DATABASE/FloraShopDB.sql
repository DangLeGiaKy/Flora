CREATE DATABASE FloraShopDB;
GO

USE FloraShopDB;
GO

-- ===================================
-- 1. BẢNG USER (NGƯỜI DÙNG)
-- ===================================
CREATE TABLE [User] (
    MaUser VARCHAR(20) PRIMARY KEY,
    TenDangNhap VARCHAR(50) UNIQUE NOT NULL,
    MatKhau VARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    SoDienThoai VARCHAR(15),
    VaiTro NVARCHAR(30) NOT NULL CHECK (VaiTro IN (N'Nhân viên', N'Quản lý', N'Admin')),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);

-- ===================================
-- 2. BẢNG KHÁCH HÀNG
-- ===================================
CREATE TABLE KhachHang (
    MaKhachHang VARCHAR(20) PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL UNIQUE,
    Email VARCHAR(100),
    DiaChi NVARCHAR(255),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);

-- ===================================
-- 3. BẢNG NHÀ CUNG CẤP
-- ===================================
CREATE TABLE NhaCungCap (
    MaNhaCungCap VARCHAR(20) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(150) NOT NULL,
    SoDienThoai VARCHAR(15) NOT NULL,
    Email VARCHAR(100),
    DiaChi NVARCHAR(255),
    LoaiHangCungCap NVARCHAR(100),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);

-- ===================================
-- 4. BẢNG KHO (SẢN PHẨM) - ✅ CÓ GIÁ NHẬP VÀ GIÁ BÁN
-- ===================================
CREATE TABLE Kho (
    MaSanPham VARCHAR(20) PRIMARY KEY,
    TenSanPham NVARCHAR(150) NOT NULL,
    LoaiHang NVARCHAR(100) NOT NULL,
    GiaNhap DECIMAL(18, 2) NOT NULL CHECK (GiaNhap >= 0),      -- ✅ GIÁ NHẬP
    GiaBan DECIMAL(18, 2) NOT NULL CHECK (GiaBan >= 0),        -- ✅ GIÁ BÁN
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    DonViTinh NVARCHAR(20) DEFAULT N'Cái',
    HinhAnh VARCHAR(255),
    TrangThai BIT DEFAULT 1,
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE()
);

-- ===================================
-- 5. BẢNG HÓA ĐƠN (BÁN HÀNG TRỰC TIẾP)
-- ===================================
CREATE TABLE HoaDon (
    MaHoaDon VARCHAR(20) PRIMARY KEY,
    MaKhachHang VARCHAR(20),
    MaNhanVien VARCHAR(20) NOT NULL,
    NgayLap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) NOT NULL DEFAULT 0,
    TongGiaNhap DECIMAL(18, 2) NOT NULL DEFAULT 0,             -- ✅ TỔNG GIÁ NHẬP
    LoiNhuan DECIMAL(18, 2) NOT NULL DEFAULT 0,                -- ✅ LỢI NHUẬN
    TienKhachDua DECIMAL(18, 2) DEFAULT 0,
    TienThoiLai DECIMAL(18, 2) DEFAULT 0,
    TrangThai NVARCHAR(30) DEFAULT N'Đã thanh toán',
    GhiChu NVARCHAR(500),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNhanVien) REFERENCES [User](MaUser)
);

-- ===================================
-- 6. BẢNG CHI TIẾT HÓA ĐƠN
-- ===================================
CREATE TABLE ChiTietHoaDon (
    MaChiTiet VARCHAR(20) PRIMARY KEY,
    MaHoaDon VARCHAR(20) NOT NULL,
    MaSanPham VARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18, 2) NOT NULL,                           -- ✅ GIÁ NHẬP TẠI THỜI ĐIỂM BÁN
    GiaBan DECIMAL(18, 2) NOT NULL,                            -- ✅ GIÁ BÁN TẠI THỜI ĐIỂM BÁN
    ThanhTien DECIMAL(18, 2) NOT NULL,                         -- ✅ THÀNH TIỀN (Số lượng × Giá bán)
    TongGiaNhap DECIMAL(18, 2) NOT NULL,                       -- ✅ TỔNG GIÁ NHẬP (Số lượng × Giá nhập)
    LoiNhuan DECIMAL(18, 2) NOT NULL,                          -- ✅ LỢI NHUẬN (ThanhTien - TongGiaNhap)
    FOREIGN KEY (MaHoaDon) REFERENCES HoaDon(MaHoaDon) ON DELETE CASCADE,
    FOREIGN KEY (MaSanPham) REFERENCES Kho(MaSanPham)
);

-- ===================================
-- 7. BẢNG ĐƠN HÀNG (ĐẶT TRƯỚC/ONLINE)
-- ===================================
CREATE TABLE DonHang (
    MaDonHang VARCHAR(20) PRIMARY KEY,
    MaKhachHang VARCHAR(20) NOT NULL,
    MaNhanVien VARCHAR(20) NOT NULL,
    NgayDat DATETIME DEFAULT GETDATE(),
    NgayGiao DATETIME,
    TongTien DECIMAL(18, 2) NOT NULL DEFAULT 0,
    TongGiaNhap DECIMAL(18, 2) NOT NULL DEFAULT 0,             -- ✅ TỔNG GIÁ NHẬP
    LoiNhuan DECIMAL(18, 2) NOT NULL DEFAULT 0,                -- ✅ LỢI NHUẬN
    TrangThai NVARCHAR(30) DEFAULT N'Đang xử lý' 
        CHECK (TrangThai IN (N'Đang xử lý', N'Đã xác nhận', N'Đang giao', N'Hoàn tất', N'Hủy')),
    GhiChu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    NgayCapNhat DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNhanVien) REFERENCES [User](MaUser)
);

-- ===================================
-- 8. BẢNG CHI TIẾT ĐƠN HÀNG
-- ===================================
CREATE TABLE ChiTietDonHang (
    MaChiTiet VARCHAR(20) PRIMARY KEY,
    MaDonHang VARCHAR(20) NOT NULL,
    MaSanPham VARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    GiaNhap DECIMAL(18, 2) NOT NULL,                           -- ✅ GIÁ NHẬP TẠI THỜI ĐIỂM ĐẶT
    GiaBan DECIMAL(18, 2) NOT NULL,                            -- ✅ GIÁ BÁN TẠI THỜI ĐIỂM ĐẶT
    ThanhTien DECIMAL(18, 2) NOT NULL,                         -- ✅ THÀNH TIỀN
    TongGiaNhap DECIMAL(18, 2) NOT NULL,                       -- ✅ TỔNG GIÁ NHẬP
    LoiNhuan DECIMAL(18, 2) NOT NULL,                          -- ✅ LỢI NHUẬN
    FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang) ON DELETE CASCADE,
    FOREIGN KEY (MaSanPham) REFERENCES Kho(MaSanPham)
);

-- ===================================
-- 9. BẢNG PHIẾU NHẬP HÀNG
-- ===================================
CREATE TABLE PhieuNhapHang (
    MaPhieuNhap VARCHAR(20) PRIMARY KEY,
    MaNhaCungCap VARCHAR(20) NOT NULL,
    MaNhanVien VARCHAR(20) NOT NULL,
    NgayNhap DATETIME DEFAULT GETDATE(),
    TongTien DECIMAL(18, 2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500),
    TrangThai NVARCHAR(30) DEFAULT N'Đã nhập',
    FOREIGN KEY (MaNhaCungCap) REFERENCES NhaCungCap(MaNhaCungCap),
    FOREIGN KEY (MaNhanVien) REFERENCES [User](MaUser)
);

-- ===================================
-- 10. BẢNG CHI TIẾT PHIẾU NHẬP
-- ===================================
CREATE TABLE ChiTietPhieuNhap (
    MaChiTiet VARCHAR(20) PRIMARY KEY,
    MaPhieuNhap VARCHAR(20) NOT NULL,
    MaSanPham VARCHAR(20) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    DonGiaNhap DECIMAL(18, 2) NOT NULL,
    ThanhTien DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (MaPhieuNhap) REFERENCES PhieuNhapHang(MaPhieuNhap) ON DELETE CASCADE,
    FOREIGN KEY (MaSanPham) REFERENCES Kho(MaSanPham)
);

-- ===================================
-- 11. BẢNG LỊCH SỬ ĐỔI MẬT KHẨU
-- ===================================
CREATE TABLE LichSuDoiMatKhau (
    MaLichSu VARCHAR(20) PRIMARY KEY,
    MaUser VARCHAR(20) NOT NULL,
    NgayDoi DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (MaUser) REFERENCES [User](MaUser)
);

-- ===================================
-- INDEXES TỐI ƯU HIỆU SUẤT
-- ===================================
CREATE INDEX idx_hoadon_ngaylap ON HoaDon(NgayLap);
CREATE INDEX idx_hoadon_manhanvien ON HoaDon(MaNhanVien);
CREATE INDEX idx_hoadon_makhachhang ON HoaDon(MaKhachHang);

CREATE INDEX idx_donhang_trangthai ON DonHang(TrangThai);
CREATE INDEX idx_donhang_ngaydat ON DonHang(NgayDat);
CREATE INDEX idx_donhang_makhachhang ON DonHang(MaKhachHang);

CREATE INDEX idx_kho_loaihang ON Kho(LoaiHang);
CREATE INDEX idx_kho_soluongton ON Kho(SoLuongTon);
CREATE INDEX idx_kho_trangthai ON Kho(TrangThai);

CREATE INDEX idx_khachhang_sdt ON KhachHang(SoDienThoai);

CREATE INDEX idx_phieunhap_ngaynhap ON PhieuNhapHang(NgayNhap);
CREATE INDEX idx_phieunhap_ncc ON PhieuNhapHang(MaNhaCungCap);

-- ===================================
-- STORED PROCEDURES VÀ TRIGGERS
-- ===================================

-- ✅ TRIGGER TỰ ĐỘNG TÍNH LỢI NHUẬN CHI TIẾT HÓA ĐƠN
GO
CREATE TRIGGER trg_TinhLoiNhuanChiTietHoaDon
ON ChiTietHoaDon
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE ChiTietHoaDon
    SET 
        ThanhTien = i.SoLuong * i.GiaBan,
        TongGiaNhap = i.SoLuong * i.GiaNhap,
        LoiNhuan = (i.SoLuong * i.GiaBan) - (i.SoLuong * i.GiaNhap)
    FROM ChiTietHoaDon ct
    INNER JOIN inserted i ON ct.MaChiTiet = i.MaChiTiet;
END;
GO

-- ✅ TRIGGER CẬP NHẬT TỔNG TIỀN VÀ LỢI NHUẬN HÓA ĐƠN
GO
CREATE TRIGGER trg_CapNhatTongHoaDon
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    -- Cập nhật cho các hóa đơn có thay đổi
    UPDATE HoaDon
    SET 
        TongTien = ISNULL((SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = HoaDon.MaHoaDon), 0),
        TongGiaNhap = ISNULL((SELECT SUM(TongGiaNhap) FROM ChiTietHoaDon WHERE MaHoaDon = HoaDon.MaHoaDon), 0),
        LoiNhuan = ISNULL((SELECT SUM(LoiNhuan) FROM ChiTietHoaDon WHERE MaHoaDon = HoaDon.MaHoaDon), 0),
        TienThoiLai = TienKhachDua - ISNULL((SELECT SUM(ThanhTien) FROM ChiTietHoaDon WHERE MaHoaDon = HoaDon.MaHoaDon), 0)
    WHERE MaHoaDon IN (
        SELECT MaHoaDon FROM inserted
        UNION
        SELECT MaHoaDon FROM deleted
    );
END;
GO

-- ✅ TRIGGER TÍNH LỢI NHUẬN CHI TIẾT ĐƠN HÀNG
GO
CREATE TRIGGER trg_TinhLoiNhuanChiTietDonHang
ON ChiTietDonHang
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE ChiTietDonHang
    SET 
        ThanhTien = i.SoLuong * i.GiaBan,
        TongGiaNhap = i.SoLuong * i.GiaNhap,
        LoiNhuan = (i.SoLuong * i.GiaBan) - (i.SoLuong * i.GiaNhap)
    FROM ChiTietDonHang ct
    INNER JOIN inserted i ON ct.MaChiTiet = i.MaChiTiet;
END;
GO

-- ✅ TRIGGER CẬP NHẬT TỔNG TIỀN VÀ LỢI NHUẬN ĐƠN HÀNG
GO
CREATE TRIGGER trg_CapNhatTongDonHang
ON ChiTietDonHang
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE DonHang
    SET 
        TongTien = ISNULL((SELECT SUM(ThanhTien) FROM ChiTietDonHang WHERE MaDonHang = DonHang.MaDonHang), 0),
        TongGiaNhap = ISNULL((SELECT SUM(TongGiaNhap) FROM ChiTietDonHang WHERE MaDonHang = DonHang.MaDonHang), 0),
        LoiNhuan = ISNULL((SELECT SUM(LoiNhuan) FROM ChiTietDonHang WHERE MaDonHang = DonHang.MaDonHang), 0),
        NgayCapNhat = GETDATE()
    WHERE MaDonHang IN (
        SELECT MaDonHang FROM inserted
        UNION
        SELECT MaDonHang FROM deleted
    );
END;
GO

-- ✅ STORED PROCEDURE: THÊM CHI TIẾT HÓA ĐƠN (TỰ ĐỘNG LẤY GIÁ HIỆN TẠI)
GO
CREATE PROCEDURE sp_ThemChiTietHoaDon
    @MaChiTiet VARCHAR(20),
    @MaHoaDon VARCHAR(20),
    @MaSanPham VARCHAR(20),
    @SoLuong INT
AS
BEGIN
    DECLARE @GiaNhap DECIMAL(18,2), @GiaBan DECIMAL(18,2);
    
    -- Lấy giá nhập và giá bán hiện tại từ Kho
    SELECT @GiaNhap = GiaNhap, @GiaBan = GiaBan
    FROM Kho
    WHERE MaSanPham = @MaSanPham;
    
    -- Thêm chi tiết hóa đơn với giá tại thời điểm bán
    INSERT INTO ChiTietHoaDon (MaChiTiet, MaHoaDon, MaSanPham, SoLuong, GiaNhap, GiaBan, ThanhTien, TongGiaNhap, LoiNhuan)
    VALUES (
        @MaChiTiet,
        @MaHoaDon,
        @MaSanPham,
        @SoLuong,
        @GiaNhap,
        @GiaBan,
        @SoLuong * @GiaBan,
        @SoLuong * @GiaNhap,
        (@SoLuong * @GiaBan) - (@SoLuong * @GiaNhap)
    );
    
    -- Trừ số lượng tồn kho
    UPDATE Kho
    SET SoLuongTon = SoLuongTon - @SoLuong
    WHERE MaSanPham = @MaSanPham;
END;
GO

-- ✅ STORED PROCEDURE: THÊM CHI TIẾT ĐƠN HÀNG
GO
CREATE PROCEDURE sp_ThemChiTietDonHang
    @MaChiTiet VARCHAR(20),
    @MaDonHang VARCHAR(20),
    @MaSanPham VARCHAR(20),
    @SoLuong INT
AS
BEGIN
    DECLARE @GiaNhap DECIMAL(18,2), @GiaBan DECIMAL(18,2);
    
    -- Lấy giá nhập và giá bán hiện tại từ Kho
    SELECT @GiaNhap = GiaNhap, @GiaBan = GiaBan
    FROM Kho
    WHERE MaSanPham = @MaSanPham;
    
    -- Thêm chi tiết đơn hàng với giá tại thời điểm đặt
    INSERT INTO ChiTietDonHang (MaChiTiet, MaDonHang, MaSanPham, SoLuong, GiaNhap, GiaBan, ThanhTien, TongGiaNhap, LoiNhuan)
    VALUES (
        @MaChiTiet,
        @MaDonHang,
        @MaSanPham,
        @SoLuong,
        @GiaNhap,
        @GiaBan,
        @SoLuong * @GiaBan,
        @SoLuong * @GiaNhap,
        (@SoLuong * @GiaBan) - (@SoLuong * @GiaNhap)
    );
END;
GO

-- ✅ VIEW: BÁO CÁO LỢI NHUẬN THEO NGÀY
GO
CREATE VIEW vw_BaoCaoLoiNhuanTheoNgay
AS
SELECT 
    CAST(NgayLap AS DATE) AS Ngay,
    COUNT(MaHoaDon) AS SoHoaDon,
    SUM(TongTien) AS DoanhThu,
    SUM(TongGiaNhap) AS TongChiPhi,
    SUM(LoiNhuan) AS LoiNhuan,
    CASE 
        WHEN SUM(TongGiaNhap) > 0 THEN (SUM(LoiNhuan) / SUM(TongGiaNhap) * 100)
        ELSE 0 
    END AS TyLeLoiNhuan
FROM HoaDon
WHERE TrangThai = N'Đã thanh toán'
GROUP BY CAST(NgayLap AS DATE);
GO

-- ✅ VIEW: BÁO CÁO LỢI NHUẬN THEO SẢN PHẨM
GO
CREATE VIEW vw_BaoCaoLoiNhuanTheoSanPham
AS
SELECT 
    k.MaSanPham,
    k.TenSanPham,
    k.LoaiHang,
    SUM(ct.SoLuong) AS TongSoLuongBan,
    SUM(ct.ThanhTien) AS DoanhThu,
    SUM(ct.TongGiaNhap) AS ChiPhi,
    SUM(ct.LoiNhuan) AS LoiNhuan,
    CASE 
        WHEN SUM(ct.TongGiaNhap) > 0 THEN (SUM(ct.LoiNhuan) / SUM(ct.TongGiaNhap) * 100)
        ELSE 0 
    END AS TyLeLoiNhuan
FROM ChiTietHoaDon ct
INNER JOIN HoaDon h ON ct.MaHoaDon = h.MaHoaDon
INNER JOIN Kho k ON ct.MaSanPham = k.MaSanPham
WHERE h.TrangThai = N'Đã thanh toán'
GROUP BY k.MaSanPham, k.TenSanPham, k.LoaiHang;
GO

-- ===================================
-- DỮ LIỆU MẪU
-- ===================================

-- User
INSERT INTO [User] VALUES 
('U001', 'admin', 'admin123', N'Nguyễn Văn Admin', 'admin@flora.vn', '0901234567', N'Admin', 1, GETDATE(), GETDATE()),
('U002', 'quanly01', 'ql123', N'Trần Thị Quản Lý', 'quanly@flora.vn', '0902345678', N'Quản lý', 1, GETDATE(), GETDATE()),
('U003', 'nhanvien01', 'nv123', N'Lê Văn Nhân Viên', 'nv@flora.vn', '0903456789', N'Nhân viên', 1, GETDATE(), GETDATE());

-- Kho (Sản phẩm) - ✅ CÓ GIÁ NHẬP VÀ GIÁ BÁN
INSERT INTO Kho VALUES 
('SP001', N'Hoa hồng đỏ', N'Hoa tươi', 35000, 50000, 100, N'Bó', NULL, 1, GETDATE(), GETDATE()),
('SP002', N'Hoa tulip', N'Hoa tươi', 60000, 80000, 50, N'Bó', NULL, 1, GETDATE(), GETDATE()),
('SP003', N'Chậu lan hồ điệp', N'Chậu cây', 250000, 350000, 30, N'Chậu', NULL, 1, GETDATE(), GETDATE()),
('SP004', N'Hoa ly trắng', N'Hoa tươi', 90000, 120000, 40, N'Bó', NULL, 1, GETDATE(), GETDATE()),
('SP005', N'Giỏ hoa chúc mừng', N'Phụ kiện', 350000, 500000, 20, N'Giỏ', NULL, 1, GETDATE(), GETDATE()),
('SP006', N'Hoa cúc vàng', N'Hoa tươi', 25000, 35000, 80, N'Bó', NULL, 1, GETDATE(), GETDATE()),
('SP007', N'Chậu sen đá', N'Chậu cây', 100000, 150000, 25, N'Chậu', NULL, 1, GETDATE(), GETDATE()),
('SP008', N'Kẹp hoa', N'Phụ kiện', 10000, 15000, 100, N'Cái', NULL, 1, GETDATE(), GETDATE()),
('SP009', N'Thiệp chúc mừng', N'Quà tặng', 12000, 20000, 200, N'Cái', NULL, 1, GETDATE(), GETDATE());

-- Khách hàng
INSERT INTO KhachHang VALUES 
('KH001', N'Nguyễn Thị Lan', '0912345678', 'lan@email.com', N'123 Nguyễn Huệ, Q1, TP.HCM', N'Khách VIP', GETDATE(), GETDATE()),
('KH002', N'Trần Văn Nam', '0923456789', 'nam@email.com', N'456 Lê Lợi, Q3, TP.HCM', NULL, GETDATE(), GETDATE()),
('KH003', N'Phạm Thị Hương', '0934567890', 'huong@email.com', N'789 Pasteur, Q1, TP.HCM', NULL, GETDATE(), GETDATE()),
('KH004', N'Lê Minh Tuấn', '0945678901', 'tuan@email.com', N'321 CMT8, Q10, TP.HCM', NULL, GETDATE(), GETDATE());

-- Nhà cung cấp
INSERT INTO NhaCungCap VALUES 
('NCC001', N'Công ty Hoa Đà Lạt', '0281234567', 'dalat@supplier.vn', N'Đà Lạt, Lâm Đồng', N'Hoa tươi', N'Uy tín, giao hàng đúng hạn', GETDATE(), GETDATE()),
('NCC002', N'Vườn ươm Thái Lan', '0282345678', 'thailand@supplier.vn', N'Bangkok, Thailand', N'Chậu cây', N'Cây nhập khẩu', GETDATE(), GETDATE()),
('NCC003', N'Công ty Phụ kiện Hoa', '0283456789', 'phukien@supplier.vn', N'Q.12, TP.HCM', N'Phụ kiện, Quà tặng', NULL, GETDATE(), GETDATE());

-- ===================================
-- VÍ DỤ SỬ DỤNG STORED PROCEDURE
-- ===================================
/*
-- Tạo hóa đơn mới
INSERT INTO HoaDon (MaHoaDon, MaKhachHang, MaNhanVien, TienKhachDua)
VALUES ('HD001', 'KH001', 'U003', 200000);

-- Thêm chi tiết hóa đơn (tự động lấy giá hiện tại và tính lợi nhuận)
EXEC sp_ThemChiTietHoaDon 'CT001', 'HD001', 'SP001', 2;
EXEC sp_ThemChiTietHoaDon 'CT002', 'HD001', 'SP006', 3;

-- Xem báo cáo lợi nhuận theo ngày
SELECT * FROM vw_BaoCaoLoiNhuanTheoNgay;

-- Xem báo cáo lợi nhuận theo sản phẩm
SELECT * FROM vw_BaoCaoLoiNhuanTheoSanPham
ORDER BY LoiNhuan DESC;
*/