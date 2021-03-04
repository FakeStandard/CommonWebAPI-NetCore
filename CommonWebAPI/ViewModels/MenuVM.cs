using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class MenuVM
    {
        public int ID { get; set; }
        public string AuthName { get; set; }
        public string? Path { get; set; }
        public List<MenuVM>? children { get; set; }
    }
}
