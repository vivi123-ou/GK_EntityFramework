using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanHang.DTO;
using QLBanHang.DAL;

namespace QLBanHang.BUS
{
    public class NhanVienBUS
    {
        private NhanVienDAL dalNhanVien = new NhanVienDAL();
         
        public List<NhanVienDTO> GetAllNhanVien()
        {
            return dalNhanVien.GetAllNhanVien();
        }

        public bool ThemNhanVien(NhanVienDTO nv)
        { 
            if (string.IsNullOrWhiteSpace(nv.TenNV))
            {
                throw new Exception("Tên nhân viên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(nv.DienThoai))
            {
                throw new Exception("Số điện thoại không được để trống!");
            }
             
            if (nv.DienThoai.Length < 10 || nv.DienThoai.Length > 11)
            {
                throw new Exception("Số điện thoại phải có 10-11 số!");
            }

            return dalNhanVien.ThemNhanVien(nv);
        }

        public bool SuaNhanVien(NhanVienDTO nv)
        {
            if (nv.MaNV <= 0)
            {
                throw new Exception("Mã nhân viên không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(nv.TenNV))
            {
                throw new Exception("Tên nhân viên không được để trống!");
            }

            return dalNhanVien.SuaNhanVien(nv);
        }
        public bool XoaNhanVien(int maNV)
        {
            if (maNV <= 0)
            {
                throw new Exception("Mã nhân viên không hợp lệ!");
            }

            return dalNhanVien.XoaNhanVien(maNV);
        }
    }
}