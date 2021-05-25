using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class Orders
    {
        public int ID { get; set; }
        public string OrderNum { get; set; }
        public int OrderPrice { get; set; }
        public bool IsPay { get; set; }
        public bool IsSend { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
