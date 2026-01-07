using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang.DTO;
using QLBanHang.BUS;

namespace QLBanHang.GUI
{
    public partial class frmHoaDon : Form
    { 
        private HoaDonBUS busHoaDon = new HoaDonBUS();
        private SanPhamBUS busSanPham = new SanPhamBUS();
        private NhanVienBUS busNhanVien = new NhanVienBUS();
        private KhachHangBUS busKhachHang = new KhachHangBUS();
         
        private List<ChiTietHoaDonDTO> chiTietHoaDon = new List<ChiTietHoaDonDTO>();

        public frmHoaDon()
        {
            InitializeComponent();

        }
          
        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadComboNhanVien();
            LoadComboKhachHang();
            LoadComboSanPham();
             
            dtpNgayLap.Value = DateTime.Now;
             
            dgvChiTiet.DataSource = null;
             
            cboNhanVien.Focus();
        }
         
        private void LoadComboNhanVien()
        {
            try
            {
                List<NhanVienDTO> dsNhanVien = busNhanVien.GetAllNhanVien();

                cboNhanVien.DataSource = dsNhanVien;
                cboNhanVien.DisplayMember = "TenNV";  
                cboNhanVien.ValueMember = "MaNV";   
                cboNhanVien.SelectedIndex = -1;  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void LoadComboKhachHang()
        {
            try
            {
                List<KhachHangDTO> dsKhachHang = busKhachHang.GetAllKhachHang();

                cboKhachHang.DataSource = dsKhachHang;
                cboKhachHang.DisplayMember = "TenKH";
                cboKhachHang.ValueMember = "MaKH";
                cboKhachHang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void LoadComboSanPham()
        {
            try
            { 
                List<SanPhamDTO> dsSanPham = busSanPham.LocSanPhamConHang();

                cboSanPham.DataSource = dsSanPham;
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";
                cboSanPham.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedIndex >= 0)
            { 
                SanPhamDTO sp = (SanPhamDTO)cboSanPham.SelectedItem;
                 
                txtDonGia.Text = sp.DonGia.ToString("N0");
                txtSoLuongTon.Text = sp.SoLuong.ToString();
                 
                txtSoLuong.Focus();
            }
            else
            { 
                txtDonGia.Clear();
                txtSoLuongTon.Clear();
            }
        }
         
        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            try
            { 
                if (cboSanPham.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSanPham.Focus();
                    return;
                } 
                if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
                {
                    MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                int soLuong = int.Parse(txtSoLuong.Text);
                int soLuongTon = int.Parse(txtSoLuongTon.Text);
                 
                if (soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                if (soLuong > soLuongTon)
                {
                    MessageBox.Show($"Số lượng mua vượt quá số lượng tồn!\nChỉ còn {soLuongTon} sản phẩm.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }
                 
                SanPhamDTO sp = (SanPhamDTO)cboSanPham.SelectedItem;
                 
                var spTonTai = chiTietHoaDon.FirstOrDefault(x => x.MaSP == sp.MaSP);

                if (spTonTai != null)
                { 
                    int tongSoLuongMoi = spTonTai.SoLuong + soLuong;

                    if (tongSoLuongMoi > soLuongTon)
                    {
                        MessageBox.Show($"Tổng số lượng vượt quá tồn kho!\nĐã có {spTonTai.SoLuong}, chỉ còn {soLuongTon} trong kho.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    } 
                    spTonTai.SoLuong = tongSoLuongMoi;
                }
                else
                { 
                    ChiTietHoaDonDTO chiTiet = new ChiTietHoaDonDTO
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
                        SoLuong = soLuong,
                        DonGia = sp.DonGia
                    };

                    chiTietHoaDon.Add(chiTiet);
                }
                 
                HienThiChiTietHoaDon();
                 
                TinhTongTien();
                 
                cboSanPham.SelectedIndex = -1;
                txtSoLuong.Clear();
                txtDonGia.Clear();
                txtSoLuongTon.Clear();
                cboSanPham.Focus();
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void HienThiChiTietHoaDon()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = chiTietHoaDon.ToList();  
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["MaHD"].Visible = false;  
                dgvChiTiet.Columns["MaSP"].HeaderText = "Mã SP";
                dgvChiTiet.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
                 
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                 
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                 
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                 
                dgvChiTiet.Columns["MaSP"].Width = 80;
                dgvChiTiet.Columns["TenSP"].Width = 300;
                dgvChiTiet.Columns["SoLuong"].Width = 100;
                dgvChiTiet.Columns["DonGia"].Width = 150;
                dgvChiTiet.Columns["ThanhTien"].Width = 150;
            }
        }
         
        private void TinhTongTien()
        {
            if (chiTietHoaDon == null || chiTietHoaDon.Count == 0)
            {
                txtTongTien.Text = "0";
                return;
            }
             
            decimal tongTien = busHoaDon.TinhTongTien(chiTietHoaDon);

            txtTongTien.Text = tongTien.ToString("N0") + " đ";
        }
         
        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            try
            { 
                if (dgvChiTiet.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                 
                int maSP = (int)dgvChiTiet.CurrentRow.Cells["MaSP"].Value;
                string tenSP = dgvChiTiet.CurrentRow.Cells["TenSP"].Value.ToString();
                 
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa sản phẩm '{tenSP}' khỏi hóa đơn?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                { 
                    chiTietHoaDon.RemoveAll(x => x.MaSP == maSP);
                     
                    HienThiChiTietHoaDon();
                    TinhTongTien();

                    MessageBox.Show("Đã xóa sản phẩm khỏi hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            try
            { 
                if (cboNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhanVien.Focus();
                    return;
                }
                 
                if (cboKhachHang.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKhachHang.Focus();
                    return;
                }
                 
                if (chiTietHoaDon.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm sản phẩm vào hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSanPham.Focus();
                    return;
                }
                 
                DialogResult result = MessageBox.Show(
                    $"Xác nhận lưu hóa đơn?\nTổng tiền: {txtTongTien.Text}",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                { 
                    HoaDonDTO hoaDon = new HoaDonDTO
                    {
                        NgayLap = dtpNgayLap.Value,
                        MaNV = (int)cboNhanVien.SelectedValue,
                        MaKH = (int)cboKhachHang.SelectedValue
                    };
                     
                    if (busHoaDon.ThemHoaDon(hoaDon, chiTietHoaDon))
                    {
                        MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                         
                        ResetForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        private void btnHuy_Click(object sender, EventArgs e)
        { 
            if (chiTietHoaDon.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn hủy hóa đơn này?\nDữ liệu chưa lưu sẽ bị mất!",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    ResetForm();
                }
            }
            else
            {
                ResetForm();
            }
        }
         
        private void ResetForm()
        { 
            cboNhanVien.SelectedIndex = -1;
            cboKhachHang.SelectedIndex = -1;
            cboSanPham.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtSoLuongTon.Clear();
            txtTongTien.Clear();
             
            chiTietHoaDon.Clear();
            dgvChiTiet.DataSource = null;
             
            dtpNgayLap.Value = DateTime.Now;
             
            cboNhanVien.Focus();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout(); 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frmHoaDon";
            this.Load += new System.EventHandler(this.frmHoaDon_Load_1);
            this.ResumeLayout(false);

        }

        private void frmHoaDon_Load_1(object sender, EventArgs e)
        {

        }
    }
}
