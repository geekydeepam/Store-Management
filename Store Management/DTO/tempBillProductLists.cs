using Store_Management.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class tempBillProductLists
    {
        public tempBillProductLists()
        {
            BillItemTempDTOList = new List<BillItemTempDTO>();
        }

        public IEnumerable<BillItemTempDTO> BillItemTempDTOList { get; set; }
        public CustomerMst CustomerMst { get; set; }
    }
}