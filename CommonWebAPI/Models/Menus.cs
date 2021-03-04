using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class Menus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string AuthName { get; set; }
        public string? Path { get; set; }
        public byte Level { get; set; }
        public int? ParentId { get; set; }
        public int Order { get; set; }
    }
}
