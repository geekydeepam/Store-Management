using Store_Management.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class ProductMstDTO
    {
        public List<ProductType> ProductTypeListMst { get; set; }

        public ProductMst ProductMst { get; set; }
    }
}