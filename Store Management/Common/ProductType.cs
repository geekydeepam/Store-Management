using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class ProductType
    {
        [Key]
        public int pk_prodtypeid { get; set; }


        public string Description { get; set; }
    }
}