using System;
using System.Collections.Generic;
using System.Linq;
using QLBanHang.DTO;

namespace QLBanHang.DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> GetAllNhanVien()
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var result = from nv in db.NhanViens
                                 select new NhanVienDTO
                                 {
                                     MaNV = nv.MaNV,
                                     TenNV = nv.TenNV,
                                     ChucVu = nv.ChucVu,
                                     DienThoai = nv.DienThoai
                                 };

                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi lấy danh sách nhân viên: " + ex.Message);
                }
            }
        }

        public bool ThemNhanVien(NhanVienDTO nvDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    NhanVien nv = new NhanVien
                    {
                        TenNV = nvDto.TenNV,
                        ChucVu = nvDto.ChucVu,
                        DienThoai = nvDto.DienThoai
                    };

                    db.NhanViens.Add(nv);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi thêm nhân viên: " + ex.Message);
                }
            }
        }

        public bool SuaNhanVien(NhanVienDTO nvDto)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var nv = db.NhanViens.Find(nvDto.MaNV);

                    if (nv != null)
                    {
                        nv.TenNV = nvDto.TenNV;
                        nv.ChucVu = nvDto.ChucVu;
                        nv.DienThoai = nvDto.DienThoai;

                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi sửa nhân viên: " + ex.Message);
                }
            }
        }

        public bool XoaNhanVien(int maNV)
        {
            using (var db = new QLBanHangEntities())
            {
                try
                {
                    var nv = db.NhanViens.Find(maNV);

                    if (nv != null)
                    {
                        db.NhanViens.Remove(nv);
                        db.SaveChanges();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi khi xóa nhân viên: " + ex.Message);
                }
            }
        }
    }
}