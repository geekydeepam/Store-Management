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
    [Authorize]
    public class ModuleController : Controller
    {
        ApplicationDbContext context =new ApplicationDbContext();
        // GET: Module
        public ActionResult ModuleList()
        {
            return View(context.ModuleMsts.ToList());
        }

        [HttpGet]
        public ActionResult CreateModule()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateModule(ModuleMst moduleMst)
        {
            context.ModuleMsts.Add(moduleMst);
            context.SaveChanges();

            return RedirectToAction("ModuleList");
        }
       
    }
}