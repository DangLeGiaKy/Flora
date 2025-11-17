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
-- 4. BẢNG KHO (SẢN PHẨM) - ĐÃ CÓ LOẠI HÀNG
-- ===================================
CREATE TABLE Kho (
    MaSanPham VARCHAR(20) PRIMARY KEY,
    TenSanPham NVARCHAR(150) NOT NULL,
    LoaiHang NVARCHAR(100) NOT NULL,  -- ✅ THÊM TRỰC TIẾP (Hoa tươi, Chậu cây, Phụ kiện...)
    DonGia DECIMAL(18, 2) NOT NULL CHECK (DonGia >= 0),
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    DonViTinh NVARCHAR(20) DEFAULT N'Cái',
    MoTa NVARCHAR(500),
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
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien DECIMAL(18, 2) NOT NULL,
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
    DonGia DECIMAL(18, 2) NOT NULL,
    ThanhTien DECIMAL(18, 2) NOT NULL,
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

CREATE INDEX idx_kho_loaihang ON Kho(LoaiHang);  -- ✅ Index cho LoaiHang
CREATE INDEX idx_kho_soluongton ON Kho(SoLuongTon);
CREATE INDEX idx_kho_trangthai ON Kho(TrangThai);

CREATE INDEX idx_khachhang_sdt ON KhachHang(SoDienThoai);

CREATE INDEX idx_phieunhap_ngaynhap ON PhieuNhapHang(NgayNhap);
CREATE INDEX idx_phieunhap_ncc ON PhieuNhapHang(MaNhaCungCap);

-- ===================================
-- DỮ LIỆU MẪU
-- ===================================

-- User
INSERT INTO [User] VALUES 
('U001', 'admin', 'admin123', N'Nguyễn Văn Admin', 'admin@flora.vn', '0901234567', N'Admin', 1, GETDATE(), GETDATE()),
('U002', 'quanly01', 'ql123', N'Trần Thị Quản Lý', 'quanly@flora.vn', '0902345678', N'Quản lý', 1, GETDATE(), GETDATE()),
('U003', 'nhanvien01', 'nv123', N'Lê Văn Nhân Viên', 'nv@flora.vn', '0903456789', N'Nhân viên', 1, GETDATE(), GETDATE());

-- Kho (Sản phẩm) - ✅ Có cột LoaiHang trực tiếp
INSERT INTO Kho VALUES 
('SP001', N'Hoa hồng đỏ', N'Hoa tươi', 50000, 100, N'Bó', N'Hoa hồng đỏ nhập khẩu', NULL, 1, GETDATE(), GETDATE()),
('SP002', N'Hoa tulip', N'Hoa tươi', 80000, 50, N'Bó', N'Hoa tulip Hà Lan', NULL, 1, GETDATE(), GETDATE()),
('SP003', N'Chậu lan hồ điệp', N'Chậu cây', 350000, 30, N'Chậu', N'Lan hồ điệp 5 cành', NULL, 1, GETDATE(), GETDATE()),
('SP004', N'Hoa ly trắng', N'Hoa tươi', 120000, 40, N'Bó', N'Hoa ly trắng cao cấp', NULL, 1, GETDATE(), GETDATE()),
('SP005', N'Giỏ hoa chúc mừng', N'Phụ kiện', 500000, 20, N'Giỏ', N'Giỏ hoa mix đẹp', NULL, 1, GETDATE(), GETDATE()),
('SP006', N'Hoa cúc vàng', N'Hoa tươi', 35000, 80, N'Bó', N'Hoa cúc tươi', NULL, 1, GETDATE(), GETDATE()),
('SP007', N'Chậu sen đá', N'Chậu cây', 150000, 25, N'Chậu', N'Sen đá mini', NULL, 1, GETDATE(), GETDATE()),
('SP008', N'Kẹp hoa', N'Phụ kiện', 15000, 100, N'Cái', N'Kẹp trang trí hoa', NULL, 1, GETDATE(), GETDATE()),
('SP009', N'Thiệp chúc mừng', N'Quà tặng', 20000, 200, N'Cái', N'Thiệp cao cấp', NULL, 1, GETDATE(), GETDATE());

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
