using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class Categories
    {
        [Key]
        public int ID { get; set; }
        public string  Name { get; set; }
        public int? ParentID { get; set; }
        public byte Level { get; set; }
        public int Order { get; set; }
        public bool Deleted { get; set; }
    }
}
