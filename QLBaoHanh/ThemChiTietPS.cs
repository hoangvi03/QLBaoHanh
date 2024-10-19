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
    public partial class ThemChiTietPS : Form
    {
        string maps;

        public ThemChiTietPS(string ma)
        {
            maps = ma;
            InitializeComponent();
            loadcombobox();
        }
        KetNoi conn = new KetNoi();

        void loadcombobox()
        {
            CboLinhKien.DataSource = conn.getLinhKien();
            CboLinhKien.DisplayMember = "Tên linh kiện";
            CboLinhKien.ValueMember = "Mã linh kiện";
        }
        private void btnTimKiemHD_Click(object sender, EventArgs e)
        {
            ChiTietPhieuSua ctps = new ChiTietPhieuSua();
            ctps.id_phieu_sua = maps;
            ctps.id_linh_kien = CboLinhKien.SelectedValue.ToString();
            int dongia = int.Parse(txtDonGia.Text);
            if (conn.ThemChiTietPS(ctps, dongia) == 1)
            {
                MessageBox.Show("Thêm thành công!");
                this.Close();
            }
            else
                MessageBox.Show("Không thể thêm!");
        }

        private void ThemChiTietPS_Load(object sender, EventArgs e)
        {
            txtMaPhieuSua.Text = maps;
            
        }

        private void CboLinhKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            LinhKien linh = new LinhKien();
            linh = conn.get1LinhKien(CboLinhKien.SelectedValue.ToString());
            if (linh != null)
            {
                txtHangSX.Text = linh.hang;
                txtDonGia.Text = linh.don_gia.ToString();
            }
        }
    }
}
