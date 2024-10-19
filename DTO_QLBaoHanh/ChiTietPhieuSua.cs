using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DTO_QLBaoHanh
{
    public class ChiTietPhieuSua
    {
        public ObjectId _id { get; set; }
        public string id_phieu_sua {  get; set; }
        public string id_linh_kien { get; set; }
    }
}
