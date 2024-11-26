using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
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
    public class CommonBillController : Controller
    {
        ApplicationDbContext context;
        // GET: CommonBill

        private string username => User.Identity.IsAuthenticated ? User.Identity.GetUserName() : null;

        public CommonBillController()
        {
            context = new ApplicationDbContext();
        }
        
        public ActionResult Index()
        {
            return View();
        }




        [HttpGet]
        public ActionResult SaveUpdateBill(int id)
        {
            CommonBillDTO item=new CommonBillDTO();
            
            CustomerMst cust;
            if(User.IsInRole("Admin"))
            {
                cust = context.CustomerMsts.FirstOrDefault(a=>a.pk_CustId == id);
            }
            else
            {
                cust = context.CustomerMsts.FirstOrDefault(a=>a.pk_CustId == id && a.Username== username);
            }
            item = getProducts();
            item.customerMst = cust;
            return View(item);
        }

        [HttpPost]
        public ActionResult SaveUpdateBill(CommonBillDTO bill)
        {
            BillsItemTemp billAlreadyAdd = context.BillsItemTemps.FirstOrDefault(a => a.fk_custId == bill.customerMst.pk_CustId &&
             a.Fk_ProductId == bill.fk_prodID && a.Username == username);

            ProductMst pro = context.ProductMsts.FirstOrDefault(a => a.pk_ProductID == bill.fk_prodID);
            if (bill.prodQuantity % 1 != 0 && pro.pk_ProductID != 2)
            {
                TempData["Error"] = "Product Quantity should be in number.";
                return RedirectToAction("SaveUpdateBill", new { id = bill.customerMst.pk_CustId });
            }

            if (billAlreadyAdd != null && Convert.ToInt32(bill.pk_tempbillID) == 0)
            {
                TempData["Error"] = "Product Is Already Added.";
                return RedirectToAction("SaveUpdateBill", new { id = bill.customerMst.pk_CustId });
            }

            if (Convert.ToInt32(bill.pk_tempbillID) == 0)
            {
                BillsItemTemp tempItem = new BillsItemTemp();

                tempItem.Fk_ProductId = bill.fk_prodID;
                tempItem.prodQuantity = bill.prodQuantity;
                tempItem.price = bill.price;
                tempItem.fk_custId = bill.customerMst.pk_CustId;
                tempItem.Username = username;

                context.BillsItemTemps.Add(tempItem);
                context.SaveChanges();
            }
            else
            {
                var iteminDb = context.BillsItemTemps.FirstOrDefault(a => a.id == bill.pk_tempbillID);
                iteminDb.Fk_ProductId = bill.fk_prodID;
                iteminDb.prodQuantity = bill.prodQuantity;
                iteminDb.price = bill.price;
                context.SaveChanges();
            }





            return RedirectToAction("TempBillList", new { id = bill.customerMst.pk_CustId });


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
                               where (productList.Username==username && productList.ProductQuantity > 0)
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
                                       where (productList.ProductName.Contains(Productname) && productList.ProductQuantity>  0)
                                       select new ProductDDD_dto
                               {
                                           pk_prodid = productList.pk_ProductID,
                                           ProductName = productList.ProductName
                               };

            }
            else
            {
                 serachedProductList = from productList in context.ProductMsts
                                      where (productList.Username == username && productList.ProductName.Contains(Productname))
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


        public ActionResult TempBillList(int? id)
        {
            if (id == null || id == 0)
            {
                return HttpNotFound();
            }




            tempBillProductLists list = new tempBillProductLists();
            if (User.IsInRole("Admin"))
            {
                list.BillItemTempDTOList = from a in context.BillsItemTemps
                                           join b in context.ProductTypes on a.Fk_ProductId equals b.pk_prodtypeid
                                           join c in context.ProductMsts on a.Fk_ProductId equals c.pk_ProductID
                                           join d in context.CustomerMsts on a.fk_custId equals d.pk_CustId
                                           where a.prodQuantity > 0
                                           select new BillItemTempDTO
                                           {

                                               ProductType = b.Description,
                                               ProductName = c.ProductName,
                                               sellingProductuantity = a.prodQuantity,
                                               RemainingItem = (c.ProductQuantity - a.prodQuantity),
                                               Selingprice = c.SellingPrice,
                                               SelinTotalgprice = a.price
                                           };

                list.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id);
            }
            else
            {
                // Step 1: Get the filtered items from BillsItemTemps
                var billItems = context.BillsItemTemps
                    .Where(a => a.Username == username && a.prodQuantity > 0)
                    .ToList();

                // Step 2: Get related data from ProductMsts based on filtered bill items
                var productIds = billItems.Select(a => a.Fk_ProductId).Distinct().ToList();
                var products = context.ProductMsts
                    .Where(c => productIds.Contains(c.pk_ProductID))
                    .ToList();

                // Step 3: Get related data from ProductTypes based on product foreign keys
                var productTypeIds = products.Select(c => c.fk_ProductID).Distinct().ToList();
                var productTypes = context.ProductTypes
                    .Where(b => productTypeIds.Contains(b.pk_prodtypeid))
                    .ToList();

                // Step 4: Get related data from CustomerMsts based on customer foreign keys
                var customerIds = billItems.Select(a => a.fk_custId).Distinct().ToList();
                var customers = context.CustomerMsts
                    .Where(d => customerIds.Contains(d.pk_CustId))
                    .ToList();

                // Step 5: Join the data in memory
                list.BillItemTempDTOList = (
                    from a in billItems
                    join c in products on a.Fk_ProductId equals c.pk_ProductID
                    join b in productTypes on c.fk_ProductID equals b.pk_prodtypeid
                    join d in customers on a.fk_custId equals d.pk_CustId
                    select new BillItemTempDTO
                    {
                        fk_tempbillid = a.id,
                        ProductType = b.Description,
                        ProductName = c.ProductName,
                        sellingProductuantity = a.prodQuantity,
                        RemainingItem = (c.ProductQuantity - a.prodQuantity),
                        Selingprice = c.SellingPrice,
                        SelinTotalgprice = a.price
                    }
                ).ToList();


                list.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id && a.Username == username);
            }
            if (list.BillItemTempDTOList.ToList().Count() > 0)
            {
                return View(list);
            }
            else
            {
                TempData["Error"] = "No Item is available for " + list.CustomerMst.Name;
                return RedirectToAction("SaveUpdateBill", new { id = id });
            }


        }


        public ActionResult Delete(int pk_tempitemid, int pk_custid)
        {

            var item = context.BillsItemTemps.FirstOrDefault(a => a.id == pk_tempitemid);
            context.BillsItemTemps.Remove(item);
            context.SaveChanges();


            return RedirectToAction("TempBillList", new { id = pk_custid });
        }

        public ActionResult Edit(int id)
        {
            CommonBillDTO item = new CommonBillDTO();
            CustomerMst cust;
            BillsItemTemp itemForEdit = new BillsItemTemp();
            if (itemForEdit == null)
            {
                return HttpNotFound();
            }
            itemForEdit = context.BillsItemTemps.FirstOrDefault(a => a.id == id);
            if (User.IsInRole("Admin"))
            {
                cust = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == id);
            }
            else
            {
                cust = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == itemForEdit.fk_custId && a.Username == username);
            }
            if (cust == null)
            {
                return HttpNotFound();
            }

            item = getProducts();
            item.pk_tempbillID = itemForEdit.id;
            item.fk_prodID = itemForEdit.Fk_ProductId;
            item.price = itemForEdit.price;
            item.prodQuantity = itemForEdit.prodQuantity;


            item.customerMst = cust;
            return View("AddProdForBillList", item);
        }




    }
}