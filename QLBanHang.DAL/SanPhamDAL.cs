using System;
using System.Collections.Generic;
using System.Linq;
using QLBanHang.DTO;

namespace QLBanHang.DAL
{
    public class SanPhamDAL
    { 
        public List<SanPhamDTO> GetAllSanPham()
        {
            using (var db = new QLBanHangEntities())  
            {
                try
                {
                    var result = from sp in db.SanPhams
                                 select new SanPhamDTO
                                 {
                                     MaSP = sp.MaSP,
                                     TenSP = sp.TenSP,
                                     DonGia = sp.DonGia,
                                     SoLuong = sp.SoLuong,
                                     TrangThai = sp.TrangThai ?? true
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
                }
            }
        }

        public bool ThemSanPham(SanPhamDTO spDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                { 
                    SanPham sp = new SanPham
                    {
                        TenSP = spDto.TenSP,
                        DonGia = spDto.DonGia,
                        SoLuong = spDto.SoLuong,
                        TrangThai = spDto.TrangThai
                    };

                    db.SanPhams.Add(sp);  
                    db.SaveChanges();     

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
                }
            }
        }

        public bool SuaSanPham(SanPhamDTO spDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                { 
                    var sp = db.SanPhams.Find(spDto.MaSP); 

                    if (sp != null)
                    {
                        sp.TenSP = spDto.TenSP;
                        sp.DonGia = spDto.DonGia;
                        sp.SoLuong = spDto.SoLuong;
                        sp.TrangThai = spDto.TrangThai;

                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi sửa sản phẩm: " + ex.Message);
                }
            }
        }

        public bool XoaSanPham(int maSP)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var sp = db.SanPhams.Find(maSP);

                    if (sp != null)
                    {
                        db.SanPhams.Remove(sp); 
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
                }
            }
        }
         
        public List<SanPhamDTO> TimKiemSanPham(string tenSP)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from sp in db.SanPhams
                                 where sp.TenSP.Contains(tenSP)
                                 select new SanPhamDTO
                                 {
                                     MaSP = sp.MaSP,
                                     TenSP = sp.TenSP,
                                     DonGia = sp.DonGia,
                                     SoLuong = sp.SoLuong,
                                     TrangThai = sp.TrangThai ?? true
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
                }
            }
        }
         
        public List<SanPhamDTO> LocSanPhamConHang()
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from sp in db.SanPhams
                                 where sp.SoLuong > 0
                                 select new SanPhamDTO
                                 {
                                     MaSP = sp.MaSP,
                                     TenSP = sp.TenSP,
                                     DonGia = sp.DonGia,
                                     SoLuong = sp.SoLuong,
                                     TrangThai = sp.TrangThai ?? true
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lọc sản phẩm còn hàng: " + ex.Message);
                }
            }
        }
    }
}