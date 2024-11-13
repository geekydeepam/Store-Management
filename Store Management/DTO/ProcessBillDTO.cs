using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class ProcessBillDTO
    {
        public int fk_productType { get; set; }
        public string productType { get; set; }

        public int fk_custid { get; set; }

        public int trnid { get; set; }
        public string fk_productId { get; set; }
        public string productName { get; set; }

        public double PiceByQuantity { get; set; }
        public double productQuantity { get; set; }

        public double productRate { get; set; }
    }
}