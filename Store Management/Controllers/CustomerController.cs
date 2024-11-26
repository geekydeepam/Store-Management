using Store_Management.Common;
using Store_Management.DTO;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    [Authorize]
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

        [HttpGet]
        public ActionResult SaveUpdateCustomer()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult SaveUpdateCustomer(CustomerDTO customerdto)
        {
            if (customerdto.Customer.pk_CustId == 0)
            {
                customerdto.Customer.Username = User.Identity.Name;
                context.CustomerMsts.Add(customerdto.Customer);
                context.SaveChanges();
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    var custInDb = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == customerdto.Customer.pk_CustId);
                    custInDb.Name = customerdto.Customer.Name;
                    custInDb.MobNo = customerdto.Customer.MobNo;

                }
                else
                {
                    var custInDb = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == customerdto.Customer.pk_CustId && a.Username == User.Identity.Name);
                    custInDb.Name = customerdto.Customer.Name;
                    custInDb.MobNo = customerdto.Customer.MobNo;
                }
                context.SaveChanges();

            }

            return RedirectToAction("CustomerList");
        }


        public ActionResult Edit(int id)
        {
            CustomerDTO  cust=new CustomerDTO();
            if (User.IsInRole("Admin"))
            {
                cust.Customer = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id);
            }
            else
            {
                cust.Customer = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id && a.Username == User.Identity.Name);
            }
            return View("SaveUpdateCustomer",cust);
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