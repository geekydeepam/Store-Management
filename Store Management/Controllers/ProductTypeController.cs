using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store_Management.Controllers
{
    public class ProductTypeController : Controller
    {
        // GET: Product
        public ActionResult List()
        { 
            List<ProductType> Plist = new List<ProductType>()
            {
                new ProductType(){pk_prodtypeID=1,Discription="Open Product"},
                new ProductType(){pk_prodtypeID=1,Discription="Close Product"},
                new ProductType(){pk_prodtypeID=1,Discription="Cloth Product"},
            };
            return View(Plist);
        }
        public ActionResult Product(int minID,int maxID)
        {
            return Content($"Min Id {minID}, Max Id {maxID}");
        }
    }
}