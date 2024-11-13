using Microsoft.SqlServer.Server;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using Store_Management.DTO;
using Store_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;

namespace Store_Management.Service
{
    public class InvoiceService
    {
        public PdfDocument GetInvoice(PrintBillDTO printBillDTO)
        {
            var document = new Document();

            BuildDocument(document,printBillDTO);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;


            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        public void BuildDocument(Document document,PrintBillDTO printBillDTO)
        {
            Section section = document.AddSection();

            var para=section.AddParagraph();
            para.AddText("Store Management System");
            para.AddLineBreak();
            para.AddText("Website: www.storeManagement.com");
            para.AddLineBreak();
            para.AddText($"Email:ddilipduley@gmail.com ");
            para.AddLineBreak();
            para.AddText("Phone: +91-706-650-1610");
            para.Format.SpaceAfter = 20;

            para=section.AddParagraph();
            para.AddText("Invoice no:N358586");
            para.AddLineBreak();
            para.Add(new DateField { Format = "yyyy/MM/dd HH:mm:ss" });
            para.Format.SpaceAfter = 10;

            para=section.AddParagraph();
            para.AddText($"Customer Name : {printBillDTO.CustomerMst.Name}");
            para.AddLineBreak();
            para.AddText($"Mobile No :{printBillDTO.CustomerMst.MobNo}");
            para.Format.SpaceAfter = 20;


            List<ProcessBillDTO> products = new List<ProcessBillDTO>(printBillDTO.ProcessBillDto);

            var table=document.LastSection.AddTable();
            table.Borders.Width = 0.5;

            table.AddColumn("1cm");
            table.AddColumn("9cm");
            table.AddColumn("2cm");
            table.AddColumn("2cm");
            table.AddColumn("2cm");

            MigraDoc.DocumentObjectModel.Tables.Row row=table.AddRow();
            row.HeadingFormat = true;
            row.Format.Font.Bold = true;
            row.Cells[0].AddParagraph("No");
            row.Cells[1].AddParagraph("Item");
            row.Cells[2].AddParagraph("Quantity");
            row.Cells[3].AddParagraph("Unit Price");
            row.Cells[4].AddParagraph("Total");

            
            for(int i=0;i<products.Count(); i++)
            {
                row=table.AddRow();
                row.Cells[0].AddParagraph((i+1).ToString());
                row.Cells[1].AddParagraph(products[i].productName);
                row.Cells[2].AddParagraph(products[i].productQuantity.ToString());
                row.Cells[3].AddParagraph(products[i].productRate.ToString());
                row.Cells[4].AddParagraph(products[i].PiceByQuantity.ToString());

                
            }

            para = section.AddParagraph();
            para.AddText($"Total: {printBillDTO.TotalPrice}");
            para.Format.SpaceBefore = 10;

            para = section.Footers.Primary.AddParagraph();
            para.AddText("Store Management . Badami Haud . Shukrawarpeth . Pune ");
            para.Format.Alignment = ParagraphAlignment.Center;


        }


    }
}