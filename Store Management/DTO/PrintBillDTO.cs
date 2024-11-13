using Store_Management.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class PrintBillDTO
    {
        public double TotalPrice { get; set; }
        public PrintBillDTO()
        {
            ProcessBillDto = new List<DTO.ProcessBillDTO>();
        }
        public CustomerMst CustomerMst { get; set; }
        public IEnumerable<ProcessBillDTO> ProcessBillDto { get; set; }
    }
}