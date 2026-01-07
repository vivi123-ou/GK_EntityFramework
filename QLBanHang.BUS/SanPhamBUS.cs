using System;
using System.Collections.Generic;
using QLBanHang.DTO;
using QLBanHang.DAL;

namespace QLBanHang.BUS
{ 
    public class SanPhamBUS
    {
        private SanPhamDAL dalSanPham = new SanPhamDAL();
         
        public List<SanPhamDTO> GetAllSanPham()
        {
            return dalSanPham.GetAllSanPham();
        }
         
        public bool ThemSanPham(SanPhamDTO sp)
        { 
            if (string.IsNullOrWhiteSpace(sp.TenSP))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            if (sp.DonGia <= 0)
            {
                throw new Exception("Đơn giá phải lớn hơn 0!");
            }

            if (sp.SoLuong < 0)
            {
                throw new Exception("Số lượng không được âm!");
            }
             
            return dalSanPham.ThemSanPham(sp);
        }
         
        public bool SuaSanPham(SanPhamDTO sp)
        { 
            if (sp.MaSP <= 0)
            {
                throw new Exception("Mã sản phẩm không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(sp.TenSP))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            if (sp.DonGia <= 0)
            {
                throw new Exception("Đơn giá phải lớn hơn 0!");
            }

            if (sp.SoLuong < 0)
            {
                throw new Exception("Số lượng không được âm!");
            }

            return dalSanPham.SuaSanPham(sp);
        }

        public bool XoaSanPham(int maSP)
        {
            if (maSP <= 0)
            {
                throw new Exception("Mã sản phẩm không hợp lệ!");
            }

            return dalSanPham.XoaSanPham(maSP);
        }


        public List<SanPhamDTO> TimKiemSanPham(string tenSP)
        {
            if (string.IsNullOrWhiteSpace(tenSP))
            {
                return GetAllSanPham();
            }

            return dalSanPham.TimKiemSanPham(tenSP);
        }

        public List<SanPhamDTO> LocSanPhamConHang()
        {
            return dalSanPham.LocSanPhamConHang();
        }
    }
}