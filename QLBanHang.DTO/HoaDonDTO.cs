using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QLBanHang.DTO
{
    public class HoaDonDTO
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public int MaNV { get; set; }
        public int MaKH { get; set; }

        public HoaDonDTO()
        {
            NgayLap = DateTime.Now;
        }

        public HoaDonDTO(int maHD, DateTime ngayLap, int maNV, int maKH)
        {
            MaHD = maHD;
            NgayLap = ngayLap;
            MaNV = maNV;
            MaKH = maKH;
        }
    }
}