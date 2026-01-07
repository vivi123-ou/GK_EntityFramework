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
        // Khai báo các BUS
        private HoaDonBUS busHoaDon = new HoaDonBUS();
        private SanPhamBUS busSanPham = new SanPhamBUS();
        private NhanVienBUS busNhanVien = new NhanVienBUS();
        private KhachHangBUS busKhachHang = new KhachHangBUS();

        // Danh sách chi tiết hóa đơn tạm (chưa lưu vào DB)
        private List<ChiTietHoaDonDTO> chiTietHoaDon = new List<ChiTietHoaDonDTO>();

        public frmHoaDon()
        {
            InitializeComponent();

        }
         

        /// Sự kiện Load form - khởi tạo dữ liệu ban đầu 
        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            LoadComboNhanVien();
            LoadComboKhachHang();
            LoadComboSanPham();

            // Đặt ngày lập mặc định là hôm nay
            dtpNgayLap.Value = DateTime.Now;

            // Khởi tạo DataGridView rỗng
            dgvChiTiet.DataSource = null;

            // Đặt focus vào combo nhân viên
            cboNhanVien.Focus();
        }

        /// <summary>
        /// Load danh sách nhân viên vào ComboBox
        /// </summary>
        private void LoadComboNhanVien()
        {
            try
            {
                List<NhanVienDTO> dsNhanVien = busNhanVien.GetAllNhanVien();

                cboNhanVien.DataSource = dsNhanVien;
                cboNhanVien.DisplayMember = "TenNV"; // Hiển thị tên
                cboNhanVien.ValueMember = "MaNV";    // Giá trị là mã
                cboNhanVien.SelectedIndex = -1; // Không chọn mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load danh sách khách hàng vào ComboBox
        /// </summary>
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

        /// <summary>
        /// Load danh sách sản phẩm CÒN HÀNG vào ComboBox
        /// </summary>
        private void LoadComboSanPham()
        {
            try
            {
                // Chỉ lấy sản phẩm còn hàng (SoLuong > 0)
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

        /// Sự kiện khi chọn sản phẩm - tự động hiển thị đơn giá và số lượng tồn 
        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSanPham.SelectedIndex >= 0)
            {
                // Lấy sản phẩm được chọn
                SanPhamDTO sp = (SanPhamDTO)cboSanPham.SelectedItem;

                // Hiển thị thông tin
                txtDonGia.Text = sp.DonGia.ToString("N0");
                txtSoLuongTon.Text = sp.SoLuong.ToString();

                // Đặt focus vào ô nhập số lượng
                txtSoLuong.Focus();
            }
            else
            {
                // Nếu không chọn gì thì xóa thông tin
                txtDonGia.Clear();
                txtSoLuongTon.Clear();
            }
        }

        /// Sự kiện click nút THÊM SẢN PHẨM vào chi tiết hóa đơn 
        private void btnThemSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sản phẩm chưa
                if (cboSanPham.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSanPham.Focus();
                    return;
                }

                // Kiểm tra đã nhập số lượng chưa
                if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
                {
                    MessageBox.Show("Vui lòng nhập số lượng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Focus();
                    return;
                }

                int soLuong = int.Parse(txtSoLuong.Text);
                int soLuongTon = int.Parse(txtSoLuongTon.Text);

                // Kiểm tra số lượng hợp lệ
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

                // Lấy thông tin sản phẩm đã chọn
                SanPhamDTO sp = (SanPhamDTO)cboSanPham.SelectedItem;

                // Kiểm tra sản phẩm đã có trong chi tiết chưa
                var spTonTai = chiTietHoaDon.FirstOrDefault(x => x.MaSP == sp.MaSP);

                if (spTonTai != null)
                {
                    // Nếu đã có thì kiểm tra tổng số lượng
                    int tongSoLuongMoi = spTonTai.SoLuong + soLuong;

                    if (tongSoLuongMoi > soLuongTon)
                    {
                        MessageBox.Show($"Tổng số lượng vượt quá tồn kho!\nĐã có {spTonTai.SoLuong}, chỉ còn {soLuongTon} trong kho.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Cộng dồn số lượng
                    spTonTai.SoLuong = tongSoLuongMoi;
                }
                else
                {
                    // Nếu chưa có thì thêm mới vào danh sách
                    ChiTietHoaDonDTO chiTiet = new ChiTietHoaDonDTO
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP,
                        SoLuong = soLuong,
                        DonGia = sp.DonGia
                    };

                    chiTietHoaDon.Add(chiTiet);
                }

                // Hiển thị lại DataGridView
                HienThiChiTietHoaDon();

                // Tính tổng tiền sử dụng LINQ (theo yêu cầu đề bài)
                TinhTongTien();

                // Reset form nhập sản phẩm
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

        /// Hiển thị chi tiết hóa đơn lên DataGridView 
        private void HienThiChiTietHoaDon()
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.DataSource = chiTietHoaDon.ToList(); // ToList() để refresh

            // Đặt tên và định dạng cột
            if (dgvChiTiet.Columns.Count > 0)
            {
                dgvChiTiet.Columns["MaHD"].Visible = false; // Ẩn cột MaHD
                dgvChiTiet.Columns["MaSP"].HeaderText = "Mã SP";
                dgvChiTiet.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvChiTiet.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";

                // Format tiền
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";

                // Căn phải các cột tiền
                dgvChiTiet.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Căn giữa số lượng
                dgvChiTiet.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Độ rộng cột
                dgvChiTiet.Columns["MaSP"].Width = 80;
                dgvChiTiet.Columns["TenSP"].Width = 300;
                dgvChiTiet.Columns["SoLuong"].Width = 100;
                dgvChiTiet.Columns["DonGia"].Width = 150;
                dgvChiTiet.Columns["ThanhTien"].Width = 150;
            }
        }

        /// Tính tổng tiền hóa đơn sử dụng LINQ (THEO YÊU CẦU ĐỀ BÀI)
        /// TongTien = ChiTietHoaDon.Sum(ct => ct.SoLuong * ct.DonGia); 
        private void TinhTongTien()
        {
            if (chiTietHoaDon == null || chiTietHoaDon.Count == 0)
            {
                txtTongTien.Text = "0";
                return;
            }

            // Sử dụng LINQ Sum như yêu cầu đề bài
            decimal tongTien = busHoaDon.TinhTongTien(chiTietHoaDon);

            txtTongTien.Text = tongTien.ToString("N0") + " đ";
        }

        /// Sự kiện click nút XÓA SẢN PHẨM khỏi chi tiết hóa đơn 
        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn dòng nào chưa
                if (dgvChiTiet.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã sản phẩm từ dòng được chọn
                int maSP = (int)dgvChiTiet.CurrentRow.Cells["MaSP"].Value;
                string tenSP = dgvChiTiet.CurrentRow.Cells["TenSP"].Value.ToString();

                // Xác nhận xóa
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa sản phẩm '{tenSP}' khỏi hóa đơn?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Xóa khỏi danh sách
                    chiTietHoaDon.RemoveAll(x => x.MaSP == maSP);

                    // Hiển thị lại
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

        /// Sự kiện click nút LƯU HÓA ĐƠN - lưu vào database 
        private void btnLuuHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn nhân viên chưa
                if (cboNhanVien.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboNhanVien.Focus();
                    return;
                }

                // Kiểm tra đã chọn khách hàng chưa
                if (cboKhachHang.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboKhachHang.Focus();
                    return;
                }

                // Kiểm tra đã có sản phẩm trong hóa đơn chưa
                if (chiTietHoaDon.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm sản phẩm vào hóa đơn!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboSanPham.Focus();
                    return;
                }

                // Xác nhận lưu
                DialogResult result = MessageBox.Show(
                    $"Xác nhận lưu hóa đơn?\nTổng tiền: {txtTongTien.Text}",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Tạo đối tượng hóa đơn
                    HoaDonDTO hoaDon = new HoaDonDTO
                    {
                        NgayLap = dtpNgayLap.Value,
                        MaNV = (int)cboNhanVien.SelectedValue,
                        MaKH = (int)cboKhachHang.SelectedValue
                    };

                    // Gọi BUS để lưu hóa đơn và chi tiết
                    if (busHoaDon.ThemHoaDon(hoaDon, chiTietHoaDon))
                    {
                        MessageBox.Show("Lưu hóa đơn thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Reset form để lập hóa đơn mới
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

        /// Sự kiện click nút HỦY - hủy hóa đơn hiện tại 
        private void btnHuy_Click(object sender, EventArgs e)
        {
            // Kiểm tra có dữ liệu chưa lưu không
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

        /// Reset form về trạng thái ban đầu 
        private void ResetForm()
        {
            // Reset các combo và textbox
            cboNhanVien.SelectedIndex = -1;
            cboKhachHang.SelectedIndex = -1;
            cboSanPham.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
            txtSoLuongTon.Clear();
            txtTongTien.Clear();

            // Xóa danh sách chi tiết
            chiTietHoaDon.Clear();
            dgvChiTiet.DataSource = null;

            // Đặt lại ngày hiện tại
            dtpNgayLap.Value = DateTime.Now;

            // Focus về combo nhân viên
            cboNhanVien.Focus();
        }
 
    }
}
