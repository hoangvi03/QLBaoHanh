using DTO_QLBaoHanh;
using MaterialSkin;
using MaterialSkin.Controls;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QLBaoHanh
{
    public partial class FormQuanLyBaoHanh : MaterialForm
    {
        public FormQuanLyBaoHanh()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.DeepOrange500, Primary.DeepOrange900, Primary.DeepOrange50, Accent.DeepOrange200, TextShade.WHITE);
        }

        KetNoi conn = new KetNoi();

        void loadDataSanPham()
        {
            dgvDSSP.DataSource = null;
            dgvDSSP.DataSource = conn.getSanPham();
        }
        void loadDataKhachHang()
        {
            dgvDSKH.DataSource = null;
            dgvDSKH.DataSource = conn.getKhachHang();
        }
        void loadComboKH()
        {
            CboKH.DataSource = conn.getKhachHang();

            CboKH.DisplayMember = "Tên khách hàng";
            CboKH.ValueMember = "Mã khách hàng";
        }
        void loadDataPhieuBaoHanh()
        {
            dgvDSPhieuBH.DataSource = null;
            dgvDSPhieuBH.DataSource = conn.getPhieuBaoHanh();
            
        }
        void loadDataPhieuSuaChua()
        {
            dgvDSPSC.DataSource = null;
            dgvDSPSC.DataSource = conn.getPhieuSuaChua();

        }
        void loadLichSuBaoHanh()
        {
            dgvLSBH.DataSource = null;
            dgvLSBH.DataSource = conn.getLichSuBaoHanh();

        }
        void loadcomboTrangThai()
        {
            cboTrangThai.Items.Add("Đang xử lý");
            cboTrangThai.Items.Add("Đã xong");
            cboTrangThai.Items.Add("Mới tiếp nhận");
            cboTrangThai.SelectedIndex = -1;
        }
        private void FormQuanLyBaoHanh_Load(object sender, EventArgs e)
        {
            loadDataSanPham();
            loadDataKhachHang();
            loadDataPhieuBaoHanh();
            loadDataPhieuSuaChua();
            loadLichSuBaoHanh();
            loadComboKH();
            loadcomboTrangThai();

        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text != "" || txtTenKH.Text != "" || txtDiaChiKH.Text != "" || txtSDTKH.Text != "" || txtEmail.Text != "")
            {
                string ma = txtMaKH.Text;
                string ten = txtTenKH.Text;
                string diachi = txtDiaChiKH.Text;
                string sdt = txtSDTKH.Text;
                string email = txtEmail.Text;
                if (conn.checkKH(ma) == false)
                {
                    conn.ThemKH(ma, ten, diachi, sdt, email);
                    loadDataKhachHang();
                    MessageBox.Show("Thêm thành công!");
                }
                else
                    MessageBox.Show("Khách hàng đã tồn tại!");
                
                txtMaKH.Text = "";
                txtTenKH.Text = "";
                txtDiaChiKH.Text = "";
                txtSDTKH.Text = "";
                txtEmail.Text = "";
            }    
        }

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (txtMaSP.Text != "" || txtTenSP.Text != "" || txtModelSp.Text != "" || txtSoSeri.Text != "" || txtThoiGianBaoHanh.Text != "")
            {
                SanPham sp = new SanPham();
                sp._id = txtMaSP.Text;
                sp.ten_san_pham = txtTenSP.Text;
                sp.so_seri = txtSoSeri.Text;
                sp.model = txtModelSp.Text;
                sp.ngay_mua = dateNgayMua.Value;
                sp.thoi_gian_bao_hanh = int.Parse(txtThoiGianBaoHanh.Text);
                sp.id_khach_hang = CboKH.SelectedValue.ToString();
                if (conn.checkSP(sp._id) == false)
                {
                    if (conn.ThemSP(sp) == 1)
                    {
                        MessageBox.Show("Thêm thành công!");
                        txtMaSP.Text = "";
                        txtTenSP.Text = "";
                        txtModelSp.Text = "";
                        txtSoSeri.Text = "";
                        txtThoiGianBaoHanh.Text = "";
                        loadDataSanPham(sp.id_khach_hang);
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm!");
                    }
                }
                else
                    MessageBox.Show("Sản phẩm đã tồn tại!");
                
            }
            else
                MessageBox.Show("Chưa nhập đủ thông tin!");
        }
        
        private void dgvDSKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgvDSKH.Rows[e.RowIndex];
            txtMaKH.Text = Convert.ToString(row.Cells["Mã khách hàng"].Value);
            txtTenKH.Text = Convert.ToString(row.Cells["Tên khách hàng"].Value);
            txtDiaChiKH.Text = Convert.ToString(row.Cells["Địa chỉ"].Value);
            txtSDTKH.Text = Convert.ToString(row.Cells["Số điện thoại"].Value);
            txtEmail.Text = Convert.ToString(row.Cells["Email"].Value);
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            string ma = txtMaKH.Text;
            string ten = txtTenKH.Text;
            string diachi = txtDiaChiKH.Text;
            string sdt = txtSDTKH.Text;
            string email = txtEmail.Text;
            conn.SuaKH(ma, ten, diachi, sdt, email);
            loadDataKhachHang();
        }

        void loadDataSanPham(string ma)
        {
            dgvDSSP.DataSource = null;
            dgvDSSP.DataSource = conn.getSanPham(ma);
        }
        private void CboKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ma = CboKH.SelectedValue.ToString();
            loadDataSanPham(ma);
        }

        private void dgvDSSP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgvDSSP.Rows[e.RowIndex];
            txtMaSP.Text = Convert.ToString(row.Cells["Mã sản phẩm"].Value);
            txtTenSP.Text = Convert.ToString(row.Cells["Tên sản phẩm"].Value);
            txtModelSp.Text = Convert.ToString(row.Cells["Model"].Value);
            txtSoSeri.Text = Convert.ToString(row.Cells["Số seri"].Value);
            dateNgayMua.Value = DateTime.Parse(row.Cells["Ngày mua"].Value.ToString());
            txtThoiGianBaoHanh.Text = Convert.ToString(row.Cells["Thời gian bảo hành"].Value);
        }

        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            SanPham sp = new SanPham();
            sp._id = txtMaSP.Text;
            sp.ten_san_pham = txtTenSP.Text;
            sp.model = txtModelSp.Text;
            sp.so_seri = txtSoSeri.Text;
            sp.thoi_gian_bao_hanh = int.Parse(txtThoiGianBaoHanh.Text);
            sp.ngay_mua = dateNgayMua.Value;
            sp.id_khach_hang = CboKH.SelectedValue.ToString();
            conn.SuaSP(sp);
            loadDataSanPham(sp.id_khach_hang);
        }

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tt = cboTrangThai.SelectedItem.ToString();
            loadDataPhieuSuaChua(tt);
        }

        void loadCTPhieuSua(string maps)
        {
            dgvChiTietPS.DataSource = null;
            dgvChiTietPS.DataSource = conn.getChiTietPhieuSau(maps);
        }
        private void loadDataPhieuSuaChua(string tt)
        {
            dgvDSPSC.DataSource = null;
            dgvDSPSC.DataSource = conn.getPhieuSuaChua(tt);
        }

        void setnull(bool x)
        {
            txtMoTa.Enabled = x;
            txtLoaiSuaChua.Enabled = x;
            txtThanhTien.Enabled = x;
            txtGiamGia.Enabled = x;
            dgvChiTietPS.Enabled = x;
            btnThemCTPS.Enabled = x;
            btnXoaCTPS.Enabled = x;
            btnXoaPSC.Enabled = x;
            btnSuaPSC.Enabled = x;
        }

        int vitri;
        string ma;
        private void dgvDSPSC_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            vitri = e.RowIndex;
            DataGridViewRow row = new DataGridViewRow();
            row = dgvDSPSC.Rows[e.RowIndex];
            string mapsc = Convert.ToString(row.Cells[0].Value);
            ma = mapsc;
            PhieuSuaChua ps = conn.Get1PhieuSua(mapsc);
            if(ps.trang_thai == "Đã xong")
            {
                setnull(false);
            }    
            else
            {
                setnull(true);
            }
            txtMoTa.Text = ps.mo_ta;
            txtLoaiSuaChua.Text = ps.loai_sua_chua;
            txtThanhTien.Text = ps.gia_sua_chua.ToString();
            txtGiamGia.Text = ps.tien_giam.ToString();
            loadCTPhieuSua(mapsc);
        }

        private void btnSuaPSC_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dgvDSPSC.Rows[vitri];
            string ma = Convert.ToString(row.Cells[0].Value);
            string mota = txtMoTa.Text;
            string loai = txtLoaiSuaChua.Text;
            int thanhtien = int.Parse(txtThanhTien.Text);
            int giamgia = int.Parse(txtGiamGia.Text);
            if (conn.suaPhieuSuaChua(ma, mota, loai, thanhtien, giamgia) == true)
            {
                MessageBox.Show("Sửa thành công!");
                loadDataPhieuSuaChua();
            }
            else
                MessageBox.Show("Không thể sửa!");
        }

        private void btnThemPSC_Click(object sender, EventArgs e)
        {
            ThemPhieuSua form = new ThemPhieuSua();
            form.FormClosed += new FormClosedEventHandler(this.ThemPhieuSua_FormClosed);
            form.ShowDialog();
        }

        private void ThemPhieuSua_FormClosed(object sender, FormClosedEventArgs e)
        {
            loadDataPhieuSuaChua();
            if(ma != null)
            {
                loadCTPhieuSua(ma);
                PhieuSuaChua psc = conn.Get1PhieuSua(ma);
                txtThanhTien.Text = psc.gia_sua_chua.ToString();
                dgvDSPSC.ClearSelection();
                dgvDSPSC.Rows[vitri].Selected = true;
            }
            else
            {
                txtMoTa.Text = string.Empty;
                txtLoaiSuaChua.Text = string.Empty;
                txtThanhTien.Text = string.Empty;
                txtGiamGia.Text = string.Empty;
            }
        }

        private void btnThemCTPS_Click(object sender, EventArgs e)
        {
            ThemChiTietPS form = new ThemChiTietPS(ma);
            form.FormClosed += new FormClosedEventHandler(this.ThemPhieuSua_FormClosed);
            form.ShowDialog();
        }

        private void btnXoaCTPS_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Xóa chi tiết hóa đơn" + ma + " ?", "Xác nhận xóa",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                if (conn.XoaChiTietPhieuSua(ma) == 1)
                {
                    MessageBox.Show("Xóa thành công!");
                    loadDataPhieuSuaChua();
                    loadCTPhieuSua(ma);
                    PhieuSuaChua psc = conn.Get1PhieuSua(ma);
                    txtThanhTien.Text = psc.gia_sua_chua.ToString();
                    txtGiamGia.Text = psc.tien_giam.ToString();
                    dgvDSPSC.ClearSelection();
                    dgvDSPSC.Rows[vitri].Selected = true;

                }
                else if(conn.XoaChiTietPhieuSua(ma) == 0)
                { MessageBox.Show("Không có dữ liệu để xóa!"); }
                else
                    MessageBox.Show("Không thể xóa!");
            }
        }

        private void btnXoaPSC_Click(object sender, EventArgs e)
        {
            if (conn.XoaPhieuSua(ma) == 1)
            {
                MessageBox.Show("Xóa thành công!");
                loadDataPhieuSuaChua();
            }
            else
                MessageBox.Show("Không thể xóa!");
        }

        private void btnTimKiemSP_Click(object sender, EventArgs e)
        {
            dgvDSSP.DataSource = null;
            dgvDSSP.DataSource = conn.TimTheoTenSP_DSSP(txtTimKiemSP.Text);
        }

        private void btnTimKiemPhieu_Click(object sender, EventArgs e)
        {
            dgvDSPhieuBH.DataSource = null;
            dgvDSPhieuBH.DataSource = conn.TimTheoTenSP_DSPBH(txtTimPhieuBaoHanh.Text);
        }

        private void btnTimKiemKH_Click(object sender, EventArgs e)
        {
            dgvDSKH.DataSource = null;
            dgvDSKH.DataSource = conn.TimKiemKHTheoTen(txtTimKiemKH.Text);
        }

        private void btnTimLichSu_Click(object sender, EventArgs e)
        {
            dgvLSBH.DataSource = null;
            dgvLSBH.DataSource = conn.TimKiemLS_TheoTenKH(txtTimLichSu.Text);
        }

        private void btnSuaXong_Click(object sender, EventArgs e)
        {
            if(conn.ThemLichSuBaoHanh(ma) == 1)
            {
                MessageBox.Show("Phiếu sửa " + ma + " đã hoàn thành!");
                loadDataPhieuSuaChua();
                dgvDSPSC.ClearSelection();
                dgvDSPSC.Rows[vitri].Selected = true;
                setnull(false);
            }
            else
            {
                MessageBox.Show("Phiếu sửa chữa chưa thể hoàn thành!");
            }    
        }

        private void btnTimKiemPSC_Click(object sender, EventArgs e)
        {

        }
    }
}
