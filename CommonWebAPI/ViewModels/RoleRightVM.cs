using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class RoleRightVM
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }

        public List<RightVM>? children { get; set; }
    }
}
