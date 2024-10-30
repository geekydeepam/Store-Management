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

    [Authorize]
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
            if(ptDTO.ProductMst.pk_ProductID==0)
            {
                ptDTO.ProductMst.Username= User.Identity.Name;
                context.ProductMsts.Add(ptDTO.ProductMst);
                context.SaveChanges();
            }
            else
            {
                var dataInDb = context.ProductMsts.FirstOrDefault(a => a.pk_ProductID == ptDTO.ProductMst.pk_ProductID);

                dataInDb.fk_ProductID = ptDTO.ProductMst.fk_ProductID;
                dataInDb.ProductName = ptDTO.ProductMst.ProductName;
                dataInDb.ProductQuantity = ptDTO.ProductMst.ProductQuantity;
                dataInDb.OriginalPrice = ptDTO.ProductMst.OriginalPrice;
                dataInDb.SellingPrice = ptDTO.ProductMst.SellingPrice;
                context.SaveChanges();
            }
            
            return RedirectToAction("ProductList");
        }

        public ActionResult ProductList()
        {
            IEnumerable<ProductListDTO> list = new List<ProductListDTO>();
            if(User.IsInRole("Admin"))
            {

                 list = from a in context.ProductMsts
                                  join b in context.ProductTypes on a.fk_ProductID equals b.pk_prodtypeid
                                  select new ProductListDTO
                                  {
                                      pk_ProductID = a.pk_ProductID,
                                      ProductType = b.Description,
                                      ProductName = a.ProductName,
                                      OriginalPrice = a.OriginalPrice,
                                      SellingPrice = a.SellingPrice,
                                      ProductQuantity = a.ProductQuantity
                                  };
            }
            else
            {
                list = from a in context.ProductMsts
                       join b in context.ProductTypes on a.fk_ProductID equals b.pk_prodtypeid
                       where a.Username==User.Identity.Name
                       select new ProductListDTO
                       {
                           pk_ProductID = a.pk_ProductID,
                           ProductType = b.Description,
                           ProductName = a.ProductName,
                           OriginalPrice = a.OriginalPrice,
                           SellingPrice = a.SellingPrice,
                           ProductQuantity = a.ProductQuantity
                       };
            }
            return View(list);
        }

        public ActionResult Edit(int id)
        {
            var editData = new ProductMstDTO
            {
                ProductMst = context.ProductMsts.FirstOrDefault(a => a.pk_ProductID == id),
                ProductTypeListMst = context.ProductTypes.ToList()
            };

            return View("AddUpdateProduct", editData);
        }

        public ActionResult Delete(int id)
        {
            var dataDelete = context.ProductMsts.FirstOrDefault(a => a.pk_ProductID == id);

            context.ProductMsts.Remove(dataDelete);
            context.SaveChangesAsync();
            return RedirectToAction("ProductList");
        }
    }
}