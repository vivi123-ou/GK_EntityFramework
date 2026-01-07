using System;
using System.Collections.Generic;
using System.Linq;
using QLBanHang.DTO;

namespace QLBanHang.DAL
{
    public class HoaDonDAL
    {
        public bool ThemHoaDon(HoaDonDTO hoaDonDto, List<ChiTietHoaDonDTO> chiTietList)
        {
            using (var db = new QLBanHangEntities())
            {
                // Bắt đầu Transaction để đảm bảo nếu lỗi thì rollback tất cả
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Tạo và thêm hóa đơn
                        HoaDon hd = new HoaDon
                        {
                            NgayLap = hoaDonDto.NgayLap,
                            MaNV = hoaDonDto.MaNV,
                            MaKH = hoaDonDto.MaKH
                        };

                        db.HoaDons.Add(hd);
                        db.SaveChanges(); // Lưu để EF tự sinh MaHD

                        // 2. Thêm chi tiết hóa đơn
                        foreach (var ctDto in chiTietList)
                        {
                            ChiTietHoaDon chiTiet = new ChiTietHoaDon
                            {
                                MaHD = hd.MaHD, // MaHD đã có sau khi SaveChanges ở trên
                                MaSP = ctDto.MaSP,
                                SoLuong = ctDto.SoLuong,
                                DonGia = ctDto.DonGia
                            };

                            db.ChiTietHoaDons.Add(chiTiet);

                            // 3. Cập nhật trừ tồn kho
                            var sanPham = db.SanPhams.Find(ctDto.MaSP);
                            if (sanPham != null)
                            {
                                sanPham.SoLuong -= ctDto.SoLuong;
                            }
                        }

                        db.SaveChanges(); // Lưu chi tiết và cập nhật kho
                        transaction.Commit(); // Hoàn tất
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // Gặp lỗi thì quay lại trạng thái đầu
                        throw new Exception("Lỗi khi thêm hóa đơn: " + ex.Message);
                    }
                }
            }
        }

        public List<HoaDonDTO> GetHoaDonTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from hd in db.HoaDons
                                 where hd.NgayLap >= tuNgay && hd.NgayLap <= denNgay
                                 select new HoaDonDTO
                                 {
                                     MaHD = hd.MaHD,
                                     NgayLap = hd.NgayLap,
                                     MaNV = hd.MaNV,
                                     MaKH = hd.MaKH
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy hóa đơn theo ngày: " + ex.Message);
                }
            }
        }

        public List<dynamic> GetTop3SanPhamBanChay()
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = (from ct in db.ChiTietHoaDons
                                  join sp in db.SanPhams on ct.MaSP equals sp.MaSP
                                  group ct by new { ct.MaSP, sp.TenSP } into g
                                  orderby g.Sum(x => x.SoLuong) descending
                                  select new
                                  {
                                      MaSP = g.Key.MaSP,
                                      TenSP = g.Key.TenSP,
                                      TongSoLuongBan = g.Sum(x => x.SoLuong)
                                  })
                                 .Take(3)
                                 .ToList<dynamic>();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy top 3 sản phẩm: " + ex.Message);
                }
            }
        }

        public List<dynamic> GetDoanhThuTheoThang(int nam)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = (from hd in db.HoaDons
                                  join ct in db.ChiTietHoaDons on hd.MaHD equals ct.MaHD
                                  where hd.NgayLap.Year == nam
                                  group ct by hd.NgayLap.Month into g
                                  orderby g.Key
                                  select new
                                  {
                                      Thang = g.Key,
                                      TongDoanhThu = g.Sum(x => x.SoLuong * x.DonGia)
                                  })
                                 .ToList<dynamic>();

                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi tính doanh thu theo tháng: " + ex.Message);
                }
            }
        }

        public List<ChiTietHoaDonDTO> GetChiTietHoaDon(int maHD)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from ct in db.ChiTietHoaDons
                                 join sp in db.SanPhams on ct.MaSP equals sp.MaSP
                                 where ct.MaHD == maHD
                                 select new ChiTietHoaDonDTO
                                 {
                                     MaHD = ct.MaHD,
                                     MaSP = ct.MaSP,
                                     TenSP = sp.TenSP,
                                     SoLuong = ct.SoLuong,
                                     DonGia = ct.DonGia
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message);
                }
            }
        }
    }
}