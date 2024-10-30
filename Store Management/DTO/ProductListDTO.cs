using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class ProductListDTO
    {
        public int pk_ProductID { get; set; }

        public string ProductType { get; set; }

        public string ProductName { get; set; }

        public double OriginalPrice { get; set; }

        public double SellingPrice { get; set; }

        public double ProductQuantity { get; set; }
    }
}