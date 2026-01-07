using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLBanHang.DTO
{
    public class ChiTietHoaDonDTO
    {
        public int MaHD { get; set; }

        public int MaSP { get; set; }
        public int SoLuong { get; set; }

        public decimal DonGia { get; set; }

        public string TenSP { get; set; }
        public decimal ThanhTien
        {
            get { return SoLuong * DonGia; }
        }

        public ChiTietHoaDonDTO() { }

        public ChiTietHoaDonDTO(int maHD, int maSP, int soLuong, decimal donGia)
        {
            MaHD = maHD;
            MaSP = maSP;
            SoLuong = soLuong;
            DonGia = donGia;
        }
    }
}