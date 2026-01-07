using System;
using System.Collections.Generic;
using System.Linq;
using QLBanHang.DTO;
using QLBanHang.DAL;

namespace QLBanHang.BUS
{
    public class HoaDonBUS
    {
        private HoaDonDAL dalHoaDon = new HoaDonDAL(); 
        public bool ThemHoaDon(HoaDonDTO hoaDon, List<ChiTietHoaDonDTO> chiTietList)
        { 
            if (hoaDon.MaNV <= 0)
            {
                throw new Exception("Vui lòng chọn nhân viên!");
            }

            if (hoaDon.MaKH <= 0)
            {
                throw new Exception("Vui lòng chọn khách hàng!");
            }
             
            if (chiTietList == null || chiTietList.Count == 0)
            {
                throw new Exception("Hóa đơn phải có ít nhất một sản phẩm!");
            }
             
            foreach (var ct in chiTietList)
            {
                if (ct.SoLuong <= 0)
                {
                    throw new Exception("Số lượng sản phẩm phải lớn hơn 0!");
                }

                if (ct.DonGia <= 0)
                {
                    throw new Exception("Đơn giá sản phẩm phải lớn hơn 0!");
                }
            }
             
            return dalHoaDon.ThemHoaDon(hoaDon, chiTietList);
        }
         
        public decimal TinhTongTien(List<ChiTietHoaDonDTO> chiTietList)
        {
            if (chiTietList == null || chiTietList.Count == 0)
            {
                return 0;
            }
             
            decimal tongTien = chiTietList.Sum(ct => ct.SoLuong * ct.DonGia);

            return tongTien;
        }
         
        public List<HoaDonDTO> GetHoaDonTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                throw new Exception("Ngày bắt đầu phải nhỏ hơn hoặc bằng ngày kết thúc!");
            }

            return dalHoaDon.GetHoaDonTheoNgay(tuNgay, denNgay);
        }
         
        public List<dynamic> GetTop3SanPhamBanChay()
        {
            return dalHoaDon.GetTop3SanPhamBanChay();
        }
         
        public List<dynamic> GetDoanhThuTheoThang(int nam)
        {
            if (nam < 2000 || nam > DateTime.Now.Year)
            {
                throw new Exception("Năm không hợp lệ!");
            }

            return dalHoaDon.GetDoanhThuTheoThang(nam);
        }
         
        public List<ChiTietHoaDonDTO> GetChiTietHoaDon(int maHD)
        {
            if (maHD <= 0)
            {
                throw new Exception("Mã hóa đơn không hợp lệ!");
            }

            return dalHoaDon.GetChiTietHoaDon(maHD);
        }
    }
}