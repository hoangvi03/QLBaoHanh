using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DTO_QLBaoHanh
{
    public class LinhKien
    {
        public string _id {  get; set; }
        public string ten_lk { get; set; }
        public int don_gia { get; set; }
        public string hang { get; set; }
    }
}
