using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store_Management.DTO
{
    public class DashboardViewModel
    {
        public string username { get; set; }

        public float TotalSalesToday { get; set; }

        public int InvoicesGenerated { get; set; }

        public List<string> Notifications { get; set; } // Alerts or notifications
    }
}