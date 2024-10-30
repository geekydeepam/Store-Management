using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Store_Management.Common;
using Store_Management.Models;

namespace Store_Management.Controllers
{
    [Authorize(Roles=CustomRoles.A)]
    public class RoleController : Controller
    {
        ApplicationDbContext context=new ApplicationDbContext();    
        // GET: Role
        public ActionResult RoleList()
        {
            var roleList=context.Roles.ToList();
            return View(roleList);
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole(IdentityRole identity)
        {
            context.Roles.Add(identity);
            context.SaveChanges();
            return RedirectToAction("RoleList");
        }
    }
}