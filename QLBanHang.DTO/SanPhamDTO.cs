using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLBanHang.DTO
{

    public class SanPhamDTO
    {
        public int MaSP { get; set; }

        public string TenSP { get; set; }

        public decimal DonGia { get; set; }

        public int SoLuong { get; set; }
         
        public bool TrangThai { get; set; }

        public SanPhamDTO()
        {
            TrangThai = true;  
        }

        public SanPhamDTO(int maSP, string tenSP, decimal donGia, int soLuong, bool trangThai)
        {
            MaSP = maSP;
            TenSP = tenSP;
            DonGia = donGia;
            SoLuong = soLuong;
            TrangThai = trangThai;
        }
    }
}
