using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class ProductMst
    {
        [Key]
        public int pk_ProductID { get; set; } 

        public int fk_ProductID { get; set; }

        public string ProductName { get; set; }

        public double OriginalPrice { get; set; }

        public double SellingPrice { get; set; }    

        public double ProductQuantity { get; set; }

        public string Username { get; set; }

        public double MinQuantity { get; set; }=0;
    }
}