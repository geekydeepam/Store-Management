using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class BillsItemTemp
    {
        [Key]
        public int id { get; set; }

        public string Username { get; set; }

        public int fk_custId { get; set; }
        public int Fk_ProductId { get; set; }
        public double prodQuantity { get; set; }

        public double price { get; set; }
    }
}