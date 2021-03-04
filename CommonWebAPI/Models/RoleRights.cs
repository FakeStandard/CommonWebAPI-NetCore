using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Models
{
    public class RoleRights
    {
        [Key]
        [Column(Order = 1)]
        public int RoleID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int RightID { get; set; }
    }
}
