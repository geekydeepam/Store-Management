using Microsoft.Ajax.Utilities;
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
    public class CommonBillController : Controller
    {
        ApplicationDbContext context;
        // GET: CommonBill

        public CommonBillController()
        {
            context=new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SaveUpdateBill(int id)
        {
            CommonBillDTO item = new CommonBillDTO();

            CustomerMst cust;
            if(User.IsInRole("Admin"))
            {
                cust = context.CustomerMsts.FirstOrDefault(a=>a.pk_CustId == id);
            }
            else
            {
                cust = context.CustomerMsts.FirstOrDefault(a=>a.pk_CustId == id && a.Username==User.Identity.Name);
            }
            item = getProducts();
            item.customerMst = cust;
            return View(item);
        }

        [HttpPost]
        public ActionResult SaveUpdateBill(CommonBillDTO billData)
        {
            ProductListForBill productDetail = new ProductListForBill();
            productDetail.username = User.Identity.Name;
            productDetail.fk_custId = billData.customerMst.pk_CustId;
            productDetail.Fk_ProductId = billData.fk_prodID;
            productDetail.productQuantity = billData.prodQuantity;
            productDetail.price = billData.price;
            context.ProductListForBills.Add(productDetail);
            context.SaveChanges();
            return View();

        }


        private CommonBillDTO getProducts()
        {
            CommonBillDTO cbd=new CommonBillDTO();
            if(User.IsInRole("Admin"))
            {
                var prodlist = from productList in context.ProductMsts
                               orderby productList.ProductName
                               select new ProductDDD_dto
                               {
                                  pk_prodid= productList.pk_ProductID,
                                  ProductName= productList.ProductName
                               };


                cbd.ProductList = prodlist.ToList();
                
            }
            else
            {
                var prodlist = from productList in context.ProductMsts
                               orderby productList.ProductName
                               where productList.Username==User.Identity.Name
                               select new ProductDDD_dto
                               {
                                   pk_prodid = productList.pk_ProductID,
                                   ProductName = productList.ProductName
                               };


                cbd.ProductList = prodlist.ToList();
            }
            return cbd;
        }

        [HttpGet]
        public JsonResult Get_Search_ProductList(string Productname)
        {
            IEnumerable<ProductDDD_dto> serachedProductList = new List<ProductDDD_dto>();
            if (User.IsInRole("Admin"))
            {
                 serachedProductList = from productList in context.ProductMsts
                                       where productList.ProductName.Contains(Productname)
                                       select new ProductDDD_dto
                               {
                                           pk_prodid = productList.pk_ProductID,
                                           ProductName = productList.ProductName
                               };

            }
            else
            {
                 serachedProductList = from productList in context.ProductMsts
                                      where (productList.Username == User.Identity.Name && productList.ProductName.Contains(Productname))
                                       select new ProductDDD_dto
                               {
                                   pk_prodid = productList.pk_ProductID,
                                   ProductName = productList.ProductName
                               };


                
            }

            return Json(serachedProductList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Get_Product_PRice(int productID,double countOrWeight)
        {
            ProductMst product=context.ProductMsts.FirstOrDefault(m => m.pk_ProductID == productID);

            double price=countOrWeight*product.SellingPrice;
            

            return Json(price, JsonRequestBehavior.AllowGet);
        }


        
    }
}