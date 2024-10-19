using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLBaoHanh
{
    public class SanPham
    {
        public string _id { get; set; } // "_id" trong MongoDB
        public string ten_san_pham { get; set; }
        public string model { get; set; }
        public string so_seri { get; set; }
        public DateTime ngay_mua { get; set; }
        public int thoi_gian_bao_hanh { get; set; } // Thời gian bảo hành tính theo tháng
        public string id_khach_hang { get; set; }
    }
}
