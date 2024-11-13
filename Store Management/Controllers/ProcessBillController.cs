using Store_Management.Common;
using Store_Management.DTO;
using Store_Management.Models;
using Store_Management.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult GenearateBill(int pk_custid)
        {
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

            return RedirectToAction("PrintBill", new { trnid = trnId, custId = pk_custid });
        }

        public ActionResult PrintBill(int trnid, int custId)
        {

            PrintBillDTO printBillDTO = new PrintBillDTO();
            printBillDTO.ProcessBillDto = GetDataForPrintBill(trnid);
            printBillDTO.TotalPrice = printBillDTO.ProcessBillDto.Sum(a => a.PiceByQuantity);
            printBillDTO.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == custId);

            return View(printBillDTO);
        }

        private IEnumerable<ProcessBillDTO> GetDataForPrintBill(int trnId)
        {
            IEnumerable<ProcessBillDTO> billData = new List<ProcessBillDTO>();
            billData = from a in context.ProcessBills
                       join b in context.ProductMsts on a.fk_productId equals b.pk_ProductID
                       join c in context.ProductTypes on b.fk_ProductID equals c.pk_prodtypeid
                       where a.trnId == trnId
                       select new ProcessBillDTO
                       {
                           fk_productType = a.fk_productId,
                           productName = b.ProductName,
                           productQuantity = a.ProductQuantity,
                           productType = c.Description,
                           productRate = b.SellingPrice,
                           fk_custid = a.fk_custid,
                           PiceByQuantity = a.PrductCurrentPrice

                       };

            return billData;
        }



        public ActionResult DownloadBill(int pk_custid)
        {
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

            return RedirectToAction("D_Bill", new { trnid = trnId, custId = pk_custid });
        }

        




        public ActionResult D_Bill(int trnid, int custId)
        {
            var invoiceService = new InvoiceService();



            PrintBillDTO printBillDTO = new PrintBillDTO();
            printBillDTO.ProcessBillDto = GetDataForPrintBill(trnid);
            printBillDTO.TotalPrice = printBillDTO.ProcessBillDto.Sum(a => a.PiceByQuantity);
            printBillDTO.CustomerMst = context.CustomerMsts.FirstOrDefault(a => a.pk_CustId == custId);

            var document = invoiceService.GetInvoice(printBillDTO);


            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            Response.ContentType = "application/pdf";
            Response.Headers.Add("content-length", stream.Length.ToString());
            byte[] bytes= stream.ToArray();
            stream.Close();

            return File(bytes, "application/pdf", "Invoice.pdf");
        }
    }
}