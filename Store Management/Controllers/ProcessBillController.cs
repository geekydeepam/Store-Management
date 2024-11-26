using Microsoft.AspNet.Identity;
using PdfSharp.Pdf.Advanced;
using Store_Management.Common;
using Store_Management.DTO;
using Store_Management.Models;
using Store_Management.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;



namespace Store_Management.Controllers
{
    [Authorize]
    public class ProcessBillController : Controller
    {
        ApplicationDbContext context;
        public ProcessBillController()
        {
            context = new ApplicationDbContext();
        }

        private int ProcessBillList(List<BillsItemTemp> ProductForProcess)
        {
            int trnid = Convert.ToInt32(context.ProcessBills.Select(p => p.trnId).DefaultIfEmpty(0).Max()) + 1;
            foreach (BillsItemTemp item in ProductForProcess)
            {
                ProcessBill itemForAdd = new ProcessBill();
                itemForAdd.trnId = trnid;
                itemForAdd.insertedDate = System.DateTime.Now;
                itemForAdd.fk_custid = item.fk_custId;
                itemForAdd.fk_productId = item.Fk_ProductId;
                itemForAdd.PrductCurrentPrice = item.price;
                itemForAdd.ProductQuantity = item.prodQuantity;
                itemForAdd.username = User.Identity.Name;
                context.ProcessBills.Add(itemForAdd);
                context.SaveChanges();

                var itemindb = context.ProductMsts.FirstOrDefault(a => a.pk_ProductID == item.Fk_ProductId);
                itemindb.ProductQuantity = (itemindb.ProductQuantity - item.prodQuantity);
                context.SaveChanges();
                context.BillsItemTemps.Remove(item);
                context.SaveChanges();

            }
            return trnid;
        }

        //public ActionResult GenearateBills(int pk_custid)
        //{
        //    List<BillsItemTemp> ProductForProcess = new List<BillsItemTemp>();
        //    if (User.IsInRole("Admin"))
        //    {
        //        ProductForProcess = context.BillsItemTemps.Where(a => a.fk_custId == pk_custid).ToList();
        //    }
        //    else
        //    {
        //        ProductForProcess = context.BillsItemTemps.Where(a => a.fk_custId == pk_custid && a.Username == User.Identity.Name).ToList();
        //    }

        //    int trnId = ProcessBillList(ProductForProcess);

        //    return RedirectToAction("PrintBill", new { trnid = trnId, custId = pk_custid });
        //}

        //public ActionResult PrintBill(int trnid, int custId)
        //{

        //    PrintBillDTO printBillDTO = new PrintBillDTO();
        //    printBillDTO.ProcessBillDto = GetDataForPrintBill(trnid);
        //    printBillDTO.TotalPrice = printBillDTO.ProcessBillDto.Sum(a => a.PiceByQuantity);
        //    printBillDTO.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == custId);

        //    return View(printBillDTO);
        //}

        private IEnumerable<ProcessBillDTO> GetDataForPrintBill(int trnId)
        {
            
            // Step 1: Get filtered records from ProcessBills based on trnId
            var processBills = context.ProcessBills
                .Where(a => a.trnId == trnId)
                .ToList();

            // Step 2: Get related data from ProductMsts based on product IDs in processBills
            var productIds = processBills.Select(a => a.fk_productId).Distinct().ToList();
            var products = context.ProductMsts
                .Where(b => productIds.Contains(b.pk_ProductID))
                .ToList();

            // Step 3: Get related data from ProductTypes based on foreign keys in products
            var productTypeIds = products.Select(b => b.fk_ProductID).Distinct().ToList();
            var productTypes = context.ProductTypes
                .Where(c => productTypeIds.Contains(c.pk_prodtypeid))
                .ToList();

            // Step 4: Perform in-memory joins to create the ProcessBillDTO list
            var billData = (
                from a in processBills
                join b in products on a.fk_productId equals b.pk_ProductID
                join c in productTypes on b.fk_ProductID equals c.pk_prodtypeid
                select new ProcessBillDTO
                {
                    fk_productType = a.fk_productId,
                    productName = b.ProductName,
                    productQuantity = a.ProductQuantity,
                    productType = c.Description,
                    productRate = b.SellingPrice,
                    fk_custid = a.fk_custid,
                    PiceByQuantity = a.PrductCurrentPrice
                }
            ).ToList();


            return billData;
        }

        public System.Security.Principal.IPrincipal GetUser()
        {
            return User;
        }

        public ActionResult GenearateBill(int pk_custid)
        {

            var address=User.Identity.Name;

            List<BillsItemTemp> ProductForProcess = new List<BillsItemTemp>();
            if (User.IsInRole("Admin"))
            {
                ProductForProcess = context.BillsItemTemps.Where(a => a.fk_custId == pk_custid).ToList();
            }
            else
            {
                ProductForProcess = context.BillsItemTemps.Where(a => a.fk_custId == pk_custid && a.Username == User.Identity.Name).ToList();
            }

            int trnId = ProcessBillList(ProductForProcess);

            var invoiceService = new InvoiceService();



            PrintBillDTO printBillDTO = new PrintBillDTO();
            printBillDTO.ProcessBillDto = GetDataForPrintBill(trnId);
            printBillDTO.TotalPrice = printBillDTO.ProcessBillDto.Sum(a => a.PiceByQuantity);
            printBillDTO.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == pk_custid);

            var document = invoiceService.GetInvoice(printBillDTO);



            //document.Save("D:\\Download/Invoice.pdf");
            //return document;


            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-length", stream.Length.ToString());
            byte[] bytes = stream.ToArray();
            stream.Close();


            return File(bytes, "application/pdf", "Invoice.pdf");
        }

        




       

        public ActionResult RedirectPage()
        {
            return RedirectToAction("Customer", "CustomerList");
        }
    }
}