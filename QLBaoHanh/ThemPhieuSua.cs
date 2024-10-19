using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO_QLBaoHanh;

namespace QLBaoHanh
{
    public partial class ThemPhieuSua : Form
    {
        public ThemPhieuSua()
        {
            InitializeComponent();
        }
        KetNoi conn = new KetNoi();
        void loadComboboxSP(string ma)
        {
            CboSanPham.DataSource = conn.getSanPham(ma);
            CboSanPham.DisplayMember = "Tên sản phẩm";
            CboSanPham.ValueMember = "Mã sản phẩm";
        }
        private void ThemPhieuSua_Load(object sender, EventArgs e)
        {
            cboKhachHang.DataSource = conn.getKhachHang();
            cboKhachHang.DisplayMember = "Tên khách hàng";
            cboKhachHang.ValueMember = "Mã khách hàng";
        }

        private void btnTimKiemHD_Click(object sender, EventArgs e)
        {
            PhieuSuaChua psc = new PhieuSuaChua();
            psc.san_pham = CboSanPham.SelectedValue.ToString();
            psc.mo_ta = txtMoTa.Text;
            psc.loai_sua_chua = txtLoaiSuaChua.Text;
            psc.ngay_nhan = dateNgayNhan.Value;
            psc.ngay_hen_tra = dateNgayHen.Value;

            if(conn.ThemPSC(psc) == 1)
            {
                MessageBox.Show("Thêm thành công!");
                this.Close();
            }    
            else
            {
                MessageBox.Show("Không thể thêm !!");
            }    
        }

        private void cboKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadComboboxSP(cboKhachHang.SelectedValue.ToString());
            if(CboSanPham.Items.Count > 0)
            {
                CboSanPham.SelectedIndex = 0;
            }    
        }
    }
}
