using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class RightVM
    {
        public int Id { get; set; }
        public string AuthName { get; set; }
        public string Path { get; set; }
        public byte Level { get; set; }
        public int? ParentId { get; set; }
        public List<RightVM> children { get; set; }
    }
}
