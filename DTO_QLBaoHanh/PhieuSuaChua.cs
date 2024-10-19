using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_QLBaoHanh
{
    public class PhieuSuaChua
    {
        public string _id {  get; set; }
        public string san_pham { get; set; }
        public string mo_ta { get; set; }
        public string loai_sua_chua { get; set; }
        public int gia_sua_chua { get; set; }
        public int tien_giam { get; set; }
        public DateTime ngay_nhan {  get; set; }
        public DateTime ngay_hen_tra {  get; set; }
        public string trang_thai { get; set; }
    }
}
