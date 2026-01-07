using System;
using System.Collections.Generic;
using System.Linq;
using QLBanHang.DTO;

namespace QLBanHang.DAL
{
    public class KhachHangDAL
    {
        public List<KhachHangDTO> GetAllKhachHang()
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from kh in db.KhachHangs
                                 select new KhachHangDTO
                                 {
                                     MaKH = kh.MaKH,
                                     TenKH = kh.TenKH,
                                     DienThoai = kh.DienThoai,
                                     DiaChi = kh.DiaChi
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy danh sách khách hàng: " + ex.Message);
                }
            }
        }

        public bool ThemKhachHang(KhachHangDTO khDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    KhachHang kh = new KhachHang
                    {
                        TenKH = khDto.TenKH,
                        DienThoai = khDto.DienThoai,
                        DiaChi = khDto.DiaChi
                    };

                    db.KhachHangs.Add(kh);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
                }
            }
        }

        public bool SuaKhachHang(KhachHangDTO khDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var kh = db.KhachHangs.Find(khDto.MaKH);

                    if (kh != null)
                    {
                        kh.TenKH = khDto.TenKH;
                        kh.DienThoai = khDto.DienThoai;
                        kh.DiaChi = khDto.DiaChi;

                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi sửa khách hàng: " + ex.Message);
                }
            }
        }

        public bool XoaKhachHang(int maKH)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var kh = db.KhachHangs.Find(maKH);

                    if (kh != null)
                    {
                        db.KhachHangs.Remove(kh);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
                }
            }
        }
    }
}