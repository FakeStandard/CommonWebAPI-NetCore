using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class CategoriesVM
    {
        public int CatID { get; set; }
        public string CatName { get; set; }
        public int? CatParentID { get; set; }
        public byte CatLevel { get; set; }
        public int Order { get; set; }
        public bool CatDeleted { get; set; }

        public List<CategoriesVM> children { get; set; }
    }
}
