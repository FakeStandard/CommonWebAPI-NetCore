using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class Params
    {
        public int ID { get; set; }
        public int CategoriesID { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
    }
}
