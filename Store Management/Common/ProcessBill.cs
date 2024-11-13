using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class ProcessBill
    {
        [Key]
        public int pk_processid { get; set; }

        public string username { get; set; }
        public int fk_custid { get; set; }

        public int trnId { get; set; }

        public int fk_productId { get; set; }

        public double ProductQuantity { get; set; }
        public double PrductCurrentPrice { get; set; }

        public DateTime insertedDate { get; set; }
    }
}