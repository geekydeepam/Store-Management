using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class BillItemTempDTO
    {
        public int fk_tempbillid { get; set; }


        public string ProductType { get; set; }

        public string ProductName { get; set; }

        public string oriPrice { get; set; }
        public double Selingprice { get; set; }

        public double sellingProductuantity { get; set; }

        public double SelinTotalgprice { get; set; }


        public double RemainingItem { get; set; }
    }
}