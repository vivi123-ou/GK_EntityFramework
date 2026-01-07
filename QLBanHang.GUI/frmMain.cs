using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanHang.GUI
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            // Đăng ký các event handlers
            btnSanPham.Click += btnSanPham_Click;
            btnHoaDon.Click += btnHoaDon_Click;
            btnBaoCao.Click += btnBaoCao_Click;
            btnThoat_Click.Click += btnThoat_Click_Click;
        }

        private void btnSanPham_Click(object sender, EventArgs e)
        {
            frmSanPham frm = new frmSanPham();
            frm.ShowDialog();
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            frmHoaDon frm = new frmHoaDon();
            frm.ShowDialog();
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            frmBaoCao frm = new frmBaoCao();
            frm.ShowDialog();
        }

        private void btnThoat_Click_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void frmMain_Load_1(object sender, EventArgs e)
        {

        }
    }
}
