using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store_Management.Common
{
    public class CustomerMst
    {
        [Key]
        public int pk_CustId { get; set; }
        public string Name    { get; set; }
        public string Username { get; set; }
        public string MobNo { get; set; }
    }
}