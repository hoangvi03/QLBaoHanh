using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLBaoHanh
{
    public class PhieuBaoHanh
    {
        public string _id { get; set; } // "_id" trong MongoDB
        public string id_san_pham { get; set; } // Tham chiếu tới sản phẩm
        public DateTime ngay_cap { get; set; }
        public DateTime ngay_het_han { get; set; }
        public string trang_thai { get; set; }
    }
}
