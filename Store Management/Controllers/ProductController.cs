using Store_Management.DTO;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();


        // GET: Product
        [HttpGet]
        public ActionResult AddUpdateProduct()
        {
            var pageloadData = new ProductMstDTO
            {
                ProductTypeListMst = context.ProductTypes.ToList()
            };
            return View(pageloadData);
        }

        [HttpPost]
        public ActionResult AddUpdateProduct(ProductMstDTO ptDTO)
        {
            context.ProductMsts.Add(ptDTO.ProductMst);
            context.SaveChangesAsync();
            return RedirectToAction("ProducList");
        }

        public ActionResult ProducList()
        {
            return View(context.ProductMsts.ToList());
        }
    }
}