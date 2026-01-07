using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QLBanHang.DTO;
using QLBanHang.BUS;

namespace QLBanHang.GUI
{
    public partial class frmSanPham : Form
    {
        // Khai báo BUS để gọi nghiệp vụ
        private SanPhamBUS busSanPham = new SanPhamBUS();

        public frmSanPham()
        {
            InitializeComponent();
            // Đăng ký các event handlers
            this.Load += frmSanPham_Load;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnTimKiem.Click += btnTimKiem_Click;
            btnLocConHang.Click += btnLocConHang_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            dgvSanPham.CellClick += dgvSanPham_CellClick;
        }
         
        /// Sự kiện Load form - chạy khi form được mở 
        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadDanhSachSanPham();
        }
         
        /// Load danh sách sản phẩm lên DataGridView 
        private void LoadDanhSachSanPham()
        {
            try
            {
                // Gọi BUS để lấy danh sách (GUI chỉ gọi BUS)
                List<SanPhamDTO> danhSach = busSanPham.GetAllSanPham();

                // Hiển thị lên DataGridView
                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = danhSach;

                // Đặt tên cột hiển thị
                dgvSanPham.Columns["MaSP"].HeaderText = "Mã SP";
                dgvSanPham.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
                dgvSanPham.Columns["SoLuong"].HeaderText = "Số lượng";
                dgvSanPham.Columns["TrangThai"].HeaderText = "Trạng thái";

                // Format hiển thị tiền (có dấu phẩy ngăn cách)
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                // Căn giữa các cột số
                dgvSanPham.Columns["MaSP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSanPham.Columns["SoLuong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSanPham.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Sự kiện click nút THÊM 
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo đối tượng DTO từ dữ liệu nhập trên form
                SanPhamDTO sp = new SanPhamDTO
                {
                    TenSP = txtTenSP.Text.Trim(),
                    DonGia = decimal.Parse(txtDonGia.Text),
                    SoLuong = int.Parse(txtSoLuong.Text),
                    TrangThai = chkTrangThai.Checked
                };

                // Gọi BUS để thêm (BUS sẽ kiểm tra dữ liệu)
                if (busSanPham.ThemSanPham(sp))
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDanhSachSanPham(); // Refresh lại danh sách
                    ClearForm(); // Xóa form nhập
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Sự kiện click nút SỬA 
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra đã chọn sản phẩm chưa
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng DTO
                SanPhamDTO sp = new SanPhamDTO
                {
                    MaSP = int.Parse(txtMaSP.Text),
                    TenSP = txtTenSP.Text.Trim(),
                    DonGia = decimal.Parse(txtDonGia.Text),
                    SoLuong = int.Parse(txtSoLuong.Text),
                    TrangThai = chkTrangThai.Checked
                };

                // Gọi BUS để sửa
                if (busSanPham.SuaSanPham(sp))
                {
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDanhSachSanPham();
                    ClearForm();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Vui lòng nhập đúng định dạng số!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Sự kiện click nút XÓA 
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận xóa
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int maSP = int.Parse(txtMaSP.Text);

                    // Gọi BUS để xóa
                    if (busSanPham.XoaSanPham(maSP))
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadDanhSachSanPham();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Sự kiện click nút TÌM KIẾM 
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tenSP = txtTimKiem.Text.Trim();

                // Gọi BUS để tìm kiếm
                List<SanPhamDTO> ketQua = busSanPham.TimKiemSanPham(tenSP);

                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = ketQua;

                // Hiển thị kết quả
                lblKetQua.Text = $"Tìm thấy {ketQua.Count} sản phẩm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Sự kiện click nút LỌC CÒN HÀNG 
        private void btnLocConHang_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi BUS để lọc
                List<SanPhamDTO> ketQua = busSanPham.LocSanPhamConHang();

                dgvSanPham.DataSource = null;
                dgvSanPham.DataSource = ketQua;

                // Hiển thị kết quả
                lblKetQua.Text = $"Có {ketQua.Count} sản phẩm còn hàng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        /// Sự kiện click nút LÀM MỚI 
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachSanPham(); // Load lại toàn bộ danh sách
            ClearForm(); // Xóa form nhập
            txtTimKiem.Clear(); // Xóa ô tìm kiếm
            lblKetQua.Text = ""; // Xóa label kết quả
        }
         
        /// Sự kiện click vào dòng trong DataGridView
        /// Hiển thị thông tin lên form để sửa/xóa 
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Kiểm tra có click vào dòng hợp lệ không
                if (e.RowIndex >= 0)
                {
                    // Lấy dòng được chọn
                    DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

                    // Hiển thị dữ liệu lên form
                    txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                    txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                    txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
                    txtSoLuong.Text = row.Cells["SoLuong"].Value.ToString();
                    chkTrangThai.Checked = (bool)row.Cells["TrangThai"].Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// Xóa dữ liệu trên form nhập 
        private void ClearForm()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtDonGia.Clear();
            txtSoLuong.Clear();
            chkTrangThai.Checked = true;
            txtTenSP.Focus(); // Đưa con trỏ về ô Tên SP
        }

      

        private void frmSanPham_Load_2(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmSanPham
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frmSanPham";
            this.Load += new System.EventHandler(this.frmSanPham_Load_1);
            this.ResumeLayout(false);

        }

        private void frmSanPham_Load_1(object sender, EventArgs e)
        {

        }
    }
}