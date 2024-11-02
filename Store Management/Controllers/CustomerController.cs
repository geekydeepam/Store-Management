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

            IEnumerable<CustomerMst> CustomerList=new List<CustomerMst>();
            if(User.IsInRole("Admin"))
            {
                CustomerList = (from a in context.CustomerMsts
                               select a).ToList();
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
            return RedirectToAction("CustomerList");
        }

        public ActionResult Delete(int id)
        {
            var dataDelete = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id);

            context.CustomerMsts.Remove(dataDelete);
            context.SaveChangesAsync();
            return RedirectToAction("CustomerList");
        }
    }
}