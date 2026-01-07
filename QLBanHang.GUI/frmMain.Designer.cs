namespace QLBanHang.GUI
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            this.btnSanPham = new System.Windows.Forms.Button();
            this.btnHoaDon = new System.Windows.Forms.Button();
            this.btnBaoCao = new System.Windows.Forms.Button();
            this.btnThoat_Click = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSanPham
            // 
            this.btnSanPham.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSanPham.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnSanPham.Location = new System.Drawing.Point(333, 123);
            this.btnSanPham.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSanPham.Name = "btnSanPham";
            this.btnSanPham.Size = new System.Drawing.Size(267, 74);
            this.btnSanPham.TabIndex = 1;
            this.btnSanPham.Text = "Quản lý\r\nSản phẩm";
            this.btnSanPham.UseVisualStyleBackColor = false;
            // 
            // btnHoaDon
            // 
            this.btnHoaDon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnHoaDon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnHoaDon.Location = new System.Drawing.Point(333, 222);
            this.btnHoaDon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnHoaDon.Name = "btnHoaDon";
            this.btnHoaDon.Size = new System.Drawing.Size(267, 74);
            this.btnHoaDon.TabIndex = 2;
            this.btnHoaDon.Text = "Lập hóa đơn\r\nbán hàng";
            this.btnHoaDon.UseVisualStyleBackColor = false;
            // 
            // btnBaoCao
            // 
            this.btnBaoCao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnBaoCao.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnBaoCao.Location = new System.Drawing.Point(333, 320);
            this.btnBaoCao.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBaoCao.Name = "btnBaoCao";
            this.btnBaoCao.Size = new System.Drawing.Size(267, 74);
            this.btnBaoCao.TabIndex = 3;
            this.btnBaoCao.Text = "Báo cáo\r\nthống kê";
            this.btnBaoCao.UseVisualStyleBackColor = false;
            // 
            // btnThoat_Click
            // 
            this.btnThoat_Click.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnThoat_Click.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.btnThoat_Click.Location = new System.Drawing.Point(333, 418);
            this.btnThoat_Click.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnThoat_Click.Name = "btnThoat_Click";
            this.btnThoat_Click.Size = new System.Drawing.Size(267, 74);
            this.btnThoat_Click.TabIndex = 4;
            this.btnThoat_Click.Text = "Thoát";
            this.btnThoat_Click.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(200, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(568, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "HỆ THỐNG QUẢN LÝ BÁN HÀNG";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 554);
            this.Controls.Add(this.btnThoat_Click);
            this.Controls.Add(this.btnBaoCao);
            this.Controls.Add(this.btnHoaDon);
            this.Controls.Add(this.btnSanPham);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý bán hàng";
            this.Load += new System.EventHandler(this.frmMain_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSanPham;
        private System.Windows.Forms.Button btnHoaDon;
        private System.Windows.Forms.Button btnBaoCao;
        private System.Windows.Forms.Button btnThoat_Click;
        private System.Windows.Forms.Label label1;
    }
}