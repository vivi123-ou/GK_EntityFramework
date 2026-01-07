using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanHang.BUS;

namespace QLBanHang.GUI
{

    public partial class frmBaoCao : Form
    {
        // Khai báo BUS
        private HoaDonBUS busHoaDon = new HoaDonBUS();

        public frmBaoCao()
        {
            InitializeComponent();

        }

        /// Sự kiện Load form - khởi tạo giá trị mặc định 
        private void frmBaoCao_Load(object sender, EventArgs e)
        {
            // Đặt khoảng ngày mặc định: 1 tháng trước đến hôm nay
            dtpTuNgay.Value = DateTime.Now.AddMonths(-1);
            dtpDenNgay.Value = DateTime.Now;

            // Đặt năm hiện tại
            nudNam.Value = DateTime.Now.Year;

            // Hiển thị hướng dẫn
            lblKetQua.Text = "Vui lòng chọn loại báo cáo muốn xem";
        }

        /// TRUY VẤN LINQ 1: Danh sách hóa đơn theo khoảng ngày
        /// Yêu cầu đề bài: Hiển thị danh sách hóa đơn trong khoảng thời gian 
        private void btnHoaDonTheoNgay_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                // Kiểm tra khoảng ngày hợp lệ
                if (tuNgay > denNgay)
                {
                    MessageBox.Show("Từ ngày phải nhỏ hơn hoặc bằng Đến ngày!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi BUS để lấy dữ liệu (sử dụng LINQ trong DAO)
                var danhSach = busHoaDon.GetHoaDonTheoNgay(tuNgay, denNgay);

                // Hiển thị kết quả lên DataGridView
                dgvKetQua.DataSource = null;
                dgvKetQua.DataSource = danhSach;

                // Hiển thị thông báo kết quả
                lblKetQua.Text = $"📋 Tìm thấy {danhSach.Count} hóa đơn từ {tuNgay:dd/MM/yyyy} đến {denNgay:dd/MM/yyyy}";

                // Đặt tên cột và format
                if (dgvKetQua.Columns.Count > 0)
                {
                    dgvKetQua.Columns["MaHD"].HeaderText = "Mã HĐ";
                    dgvKetQua.Columns["NgayLap"].HeaderText = "Ngày lập";
                    dgvKetQua.Columns["MaNV"].HeaderText = "Mã NV";
                    dgvKetQua.Columns["MaKH"].HeaderText = "Mã KH";

                    // Ẩn cột ChiTietHoaDon nếu có
                    if (dgvKetQua.Columns.Contains("ChiTietHoaDon"))
                    {
                        dgvKetQua.Columns["ChiTietHoaDon"].Visible = false;
                    }

                    // Format ngày giờ
                    dgvKetQua.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                    // Căn giữa các cột mã
                    dgvKetQua.Columns["MaHD"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvKetQua.Columns["MaNV"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvKetQua.Columns["MaKH"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Độ rộng cột
                    dgvKetQua.Columns["MaHD"].Width = 100;
                    dgvKetQua.Columns["NgayLap"].Width = 200;
                    dgvKetQua.Columns["MaNV"].Width = 100;
                    dgvKetQua.Columns["MaKH"].Width = 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// TRUY VẤN LINQ 2: Top 3 sản phẩm bán chạy nhất
        /// Yêu cầu đề bài: Hiển thị top 3 sản phẩm có tổng số lượng bán nhiều nhất
        /// Sử dụng: group by, Sum, orderby descending, Take(3) 
        private void btnTop3SanPham_Click(object sender, EventArgs e)
        {
            try
            {
                // Gọi BUS để lấy dữ liệu (sử dụng LINQ trong DAO)
                var danhSach = busHoaDon.GetTop3SanPhamBanChay();

                // Kiểm tra có dữ liệu không
                if (danhSach.Count == 0)
                {
                    MessageBox.Show("Chưa có dữ liệu bán hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblKetQua.Text = "Chưa có dữ liệu bán hàng";
                    dgvKetQua.DataSource = null;
                    return;
                }

                // Hiển thị kết quả lên DataGridView
                dgvKetQua.DataSource = null;
                dgvKetQua.DataSource = danhSach;

                // Hiển thị thông báo kết quả
                lblKetQua.Text = "Top 3 sản phẩm bán chạy nhất";

                // Đặt tên cột và format
                if (dgvKetQua.Columns.Count > 0)
                {
                    dgvKetQua.Columns["MaSP"].HeaderText = "Mã SP";
                    dgvKetQua.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                    dgvKetQua.Columns["TongSoLuongBan"].HeaderText = "Tổng SL đã bán";

                    // Căn giữa và format
                    dgvKetQua.Columns["MaSP"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvKetQua.Columns["TongSoLuongBan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvKetQua.Columns["TongSoLuongBan"].DefaultCellStyle.Format = "N0";

                    // Độ rộng cột
                    dgvKetQua.Columns["MaSP"].Width = 100;
                    dgvKetQua.Columns["TenSP"].Width = 500;
                    dgvKetQua.Columns["TongSoLuongBan"].Width = 200;

                    // Tô màu cho top 1, 2, 3
                    if (dgvKetQua.Rows.Count >= 1)
                        dgvKetQua.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.Gold;

                    if (dgvKetQua.Rows.Count >= 2)
                        dgvKetQua.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.Silver;

                    if (dgvKetQua.Rows.Count >= 3)
                        dgvKetQua.Rows[2].DefaultCellStyle.BackColor = System.Drawing.Color.LightSalmon;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
         
        /// TRUY VẤN LINQ 3: Tổng doanh thu theo tháng
        /// Yêu cầu đề bài: Hiển thị tổng doanh thu của từng tháng trong năm
        /// Sử dụng: group by tháng, Sum(SoLuong * DonGia) 
        private void btnDoanhThuTheoThang_Click(object sender, EventArgs e)
        {
            try
            {
                int nam = (int)nudNam.Value;

                // Gọi BUS để lấy dữ liệu (sử dụng LINQ trong DAL)
                var danhSach = busHoaDon.GetDoanhThuTheoThang(nam);

                // Kiểm tra có dữ liệu không
                if (danhSach.Count == 0)
                {
                    MessageBox.Show($"Chưa có dữ liệu bán hàng trong năm {nam}!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblKetQua.Text = $" Chưa có dữ liệu bán hàng trong năm {nam}";
                    dgvKetQua.DataSource = null;
                    return;
                }

                // Hiển thị kết quả lên DataGridView
                dgvKetQua.DataSource = null;
                dgvKetQua.DataSource = danhSach;

                // Tính tổng doanh thu cả năm
                decimal tongDoanhThuNam = 0;
                foreach (var item in danhSach)
                {
                    tongDoanhThuNam += item.TongDoanhThu;
                }

                // Hiển thị thông báo kết quả
                lblKetQua.Text = $"Doanh thu các tháng năm {nam} - Tổng cả năm: {tongDoanhThuNam:N0} đ";

                // Đặt tên cột và format
                if (dgvKetQua.Columns.Count > 0)
                {
                    dgvKetQua.Columns["Thang"].HeaderText = "Tháng";
                    dgvKetQua.Columns["TongDoanhThu"].HeaderText = "Tổng doanh thu (VNĐ)";

                    // Format tiền
                    dgvKetQua.Columns["TongDoanhThu"].DefaultCellStyle.Format = "N0";

                    // Căn giữa và phải
                    dgvKetQua.Columns["Thang"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvKetQua.Columns["TongDoanhThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    // Độ rộng cột
                    dgvKetQua.Columns["Thang"].Width = 150;
                    dgvKetQua.Columns["TongDoanhThu"].Width = 300;

                    // Tô màu cho tháng có doanh thu cao nhất
                    decimal maxDoanhThu = 0;
                    int maxIndex = -1;

                    for (int i = 0; i < dgvKetQua.Rows.Count; i++)
                    {
                        decimal doanhThu = Convert.ToDecimal(dgvKetQua.Rows[i].Cells["TongDoanhThu"].Value);
                        if (doanhThu > maxDoanhThu)
                        {
                            maxDoanhThu = doanhThu;
                            maxIndex = i;
                        }
                    }

                    if (maxIndex >= 0)
                    {
                        dgvKetQua.Rows[maxIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        dgvKetQua.Rows[maxIndex].DefaultCellStyle.Font = new System.Drawing.Font(dgvKetQua.Font, System.Drawing.FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudNam_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}