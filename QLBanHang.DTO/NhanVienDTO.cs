using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanHang.DTO
{
    public class NhanVienDTO
    {
        public int MaNV { get; set; }

        public string TenNV { get; set; }

        public string ChucVu { get; set; }

        public string DienThoai { get; set; }

        public NhanVienDTO() { }

        public NhanVienDTO(int maNV, string tenNV, string chucVu, string dienThoai)
        {
            MaNV = maNV;
            TenNV = tenNV;
            ChucVu = chucVu;
            DienThoai = dienThoai;
        }
    }
}