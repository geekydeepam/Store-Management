using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store_Management.Common;
using Store_Management.Models;

namespace Store_Management.Controllers
{
    [Authorize]
    public class ProducttypeController : Controller
    {
        ApplicationDbContext context;
        public ProducttypeController()
        {
                context = new ApplicationDbContext();
        }
        // GET: ProductType
        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateUpdateForm",new ProductType());
        }
        [HttpPost]
        public ActionResult Create(ProductType pt)
        {
            context.ProductTypes.Add(pt);
            context.SaveChanges();
            return View("CreateUpdateForm");
        }


        public ActionResult ProductTypeList()
        {
            var prodList=context.ProductTypes.ToList();
            return View(prodList);
        }

        public ActionResult Edit(int id)
        {
            var productType = context.ProductTypes.FirstOrDefault(a=>a.pk_prodtypeid==id);

            return View("CreateUpdateForm",productType);
        }
    }
}