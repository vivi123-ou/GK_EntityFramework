using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanHang.DTO;
using QLBanHang.DAL;

namespace QLBanHang.BUS
{
    public class KhachHangBUS
    {
        private KhachHangDAL dalKhachHang = new KhachHangDAL();


        public List<KhachHangDTO> GetAllKhachHang()
        {
            return dalKhachHang.GetAllKhachHang();
        }

        public bool ThemKhachHang(KhachHangDTO kh)
        {
            if (string.IsNullOrWhiteSpace(kh.TenKH))
            {
                throw new Exception("Tên khách hàng không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(kh.DienThoai))
            {
                throw new Exception("Số điện thoại không được để trống!");
            }

            if (kh.DienThoai.Length < 10 || kh.DienThoai.Length > 11)
            {
                throw new Exception("Số điện thoại phải có 10-11 số!");
            }

            return dalKhachHang.ThemKhachHang(kh);
        }
        public bool SuaKhachHang(KhachHangDTO kh)
        {
            if (kh.MaKH <= 0)
            {
                throw new Exception("Mã khách hàng không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(kh.TenKH))
            {
                throw new Exception("Tên khách hàng không được để trống!");
            }

            return dalKhachHang.SuaKhachHang(kh);
        }

        public bool XoaKhachHang(int maKH)
        {
            if (maKH <= 0)
            {
                throw new Exception("Mã khách hàng không hợp lệ!");
            }

            return dalKhachHang.XoaKhachHang(maKH);
        }
    }
}