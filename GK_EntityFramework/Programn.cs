using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanHang.GUI
{
    internal static class Program
    { 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            /*
             // Application.Run(new frmSanPham());
            // Application.Run(new frmHoaDon());
            // Application.Run(new frmBaoCao());
             */
        }
    }
}
