using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class OrdersVM
    {
        public int ID { get; set; }
        public string OrderNumber { get; set; }
        public int OrderPrice { get; set; }
        public bool OrderPay { get; set; }
        public bool OrderSend { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
