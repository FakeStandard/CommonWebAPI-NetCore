using CommonWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class GoodsVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public long Price { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool State { get; set; }
        public int HotNumber { get; set; }
        public bool IsPromote { get; set; }
        public int CategoriesID { get; set; }
        public string? Introduce { get; set; }

        public List<GoodsDetail> GoodsDetails { get; set; }
    }
}
