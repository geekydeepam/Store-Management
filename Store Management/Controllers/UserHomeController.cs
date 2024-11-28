using Microsoft.AspNet.Identity;
using Store_Management.Common;
using Store_Management.DTO;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public UserHomeController()
        {
            context = new ApplicationDbContext();
        }
        // GET: UserHome
        public ActionResult DisplayModules()
        {
            List<ModuleMst> Modules;
            if(User.IsInRole("Admin"))
            {
                Modules = context.ModuleMsts.Where(a => a.IsActive == 1).ToList();
            }
            else if(User.IsInRole("General Store"))
            {
                Modules = context.ModuleMsts.Where(a => a.IsActive == 1 && a.pk_moduleID==2).ToList();
            }
            else 
            {
                Modules = context.ModuleMsts.Where(a => a.IsActive == 1 && a.pk_moduleID == 1).ToList();
            }
            return View(Modules);
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.GetUserName(); // Identity framework provides the username

            // Example of how to fetch data (customize based on your database structure)
            var totalSalesToday = await context.ProcessBills
                .Where(i => i.username == userName && DbFunctions.TruncateTime(i.insertedDate) == DateTime.Today)
                .SumAsync(i => (float?)i.PrductCurrentPrice) ?? 0;

           

            var invoicesGenerated = await context.ProcessBills
                .CountAsync(i => i.username == userName && DbFunctions.TruncateTime(i.insertedDate) == DateTime.Today);


            IEnumerable<ProductMst> low_Quantiy= context.ProductMsts.Where(i=>i.Username==userName && i.ProductQuantity<=i.MinQuantity).ToList();

            var notifications = new List<string>();
            foreach (var product in low_Quantiy)
            {
                notifications.Add($"Low Stock For {product.ProductName}");
            }


            // Populate the ViewModel
            var model = new DashboardViewModel
            {
                username = userName,
                TotalSalesToday = totalSalesToday,
                InvoicesGenerated = invoicesGenerated,
                Notifications = notifications
            };

            return View(model);
        }


    }
}  