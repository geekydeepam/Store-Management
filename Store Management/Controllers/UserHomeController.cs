using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    public class UserHomeController : Controller
    {
        ApplicationDbContext context=new ApplicationDbContext();
        // GET: UserHome
        public ActionResult DisplayModules()
        {
            var mod = context.ModuleMsts.Where(a => a.IsActive == 1).ToList();
            return View(mod);
        }
    }
}