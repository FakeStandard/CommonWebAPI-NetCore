using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.ViewModels
{
    public class ParamsVM
    {
        public int ID { get; set; }
        public int CategoriesID { get; set; }
        public string attrName { get; set; }
        public string attrType { get; set; }
        public List<ParamsTagsVM> attrVals { get; set; }
    }
}
