using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class GoodsDetail
    {
        public int ID { get; set; }
        public int GoodsID { get; set; }
        public int CategoriesID { get; set; }
        public int ParamsID { get; set; }
        public bool Type { get; set; }
        public string Content { get; set; }
    }
}
