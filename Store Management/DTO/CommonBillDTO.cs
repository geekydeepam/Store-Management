using Store_Management.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class CommonBillDTO
    {
        public CommonBillDTO()
        {
            ProductList = new List<ProductDDD_dto>();
        }
        public int pk_tempbillID  { get; set; }
        public int fk_prodID { get; set; }
        public double prodQuantity { get; set; }    
        public double price { get; set; }
        public CustomerMst customerMst { get; set; }
        public IEnumerable<ProductDDD_dto> ProductList { get; set; }
    }
}