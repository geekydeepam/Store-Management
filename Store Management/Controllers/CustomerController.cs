using Store_Management.Common;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    public class CustomerController : Controller
    {

        ApplicationDbContext context=new ApplicationDbContext();
        // GET: Customer
        public ActionResult CustomerList()
        {

            IEnumerable<CustomerMst> CustomerList;
            if(User.IsInRole("Admin"))
            {
                CustomerList = context.CustomerMsts.ToList();
            }
            else
            {
                CustomerList = (context.CustomerMsts.Where(a => a.Username == User.Identity.Name)).ToList();
            }
            

            return View(CustomerList);
        }


        public ActionResult SaveUpdateCustomer()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult SaveUpdateCustomer(CustomerMst customerMst)
        {
            customerMst.Username=User.Identity.Name;

            context.CustomerMsts.Add(customerMst);
            context.SaveChanges();
            return View();
        }
    }
}