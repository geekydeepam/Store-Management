using Store_Management.Common;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    }
}  