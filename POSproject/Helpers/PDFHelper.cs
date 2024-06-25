using System;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

public static class PDFHelper
{
    public static void GenerateInvoice(HttpResponse response)
    {
        // Invoice Data
        string firstName = "Ahsan";
        string lastName = "Muhammad";
        string phoneNumber = "03368892608";
        string nic = "42101161809";
        string[][] products = new string[][]
        {
            new string[] { "Rim", "5", "600" },
            new string[] { "Glass", "2", "100" }
        };
        decimal amountPaid = 500;
        decimal totalAmount = 3200;
        decimal amountDue = totalAmount - amountPaid;

        // Create a PDF document
        Document document = new Document(PageSize.A4, 50, 50, 25, 25);
        MemoryStream memoryStream = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

        // Add watermark
        writer.PageEvent = new PDFWatermark();

        document.Open();

        // Add Logo
        string logoPath = HttpContext.Current.Server.MapPath("~/wwwroot/images/logoinverex.png");
        Image logo = Image.GetInstance(logoPath);
        logo.ScaleToFit(120, 60);
        logo.Alignment = Image.ALIGN_CENTER;
        document.Add(logo);

        // Add Title
        Font titleFont = FontFactory.GetFont("Arial", 24, Font.BOLD);
        Paragraph title = new Paragraph("Invoice", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        title.SpacingAfter = 20;
        document.Add(title);

        // Add Customer Information
        Font headerFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        Font bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

        PdfPTable customerTable = new PdfPTable(1);
        customerTable.WidthPercentage = 100;
        customerTable.SpacingBefore = 20;
        customerTable.SpacingAfter = 20;

        PdfPCell cell = new PdfPCell(new Phrase("Customer Information", headerFont));
        cell.Border = PdfPCell.NO_BORDER;
        customerTable.AddCell(cell);

        customerTable.AddCell(new PdfPCell(new Phrase($"First Name: {firstName}", bodyFont)) { Border = PdfPCell.NO_BORDER });
        customerTable.AddCell(new PdfPCell(new Phrase($"Last Name: {lastName}", bodyFont)) { Border = PdfPCell.NO_BORDER });
        customerTable.AddCell(new PdfPCell(new Phrase($"Phone Number: {phoneNumber}", bodyFont)) { Border = PdfPCell.NO_BORDER });
        customerTable.AddCell(new PdfPCell(new Phrase($"NIC: {nic}", bodyFont)) { Border = PdfPCell.NO_BORDER });

        document.Add(customerTable);

        // Add Products Table
        PdfPTable table = new PdfPTable(3);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 4, 1, 2 });
        table.SpacingBefore = 20;
        table.SpacingAfter = 20;

        // Add Table Headers
        PdfPCell headerCell;

        headerCell = new PdfPCell(new Phrase("Product Name", headerFont));
        headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(headerCell);

        headerCell = new PdfPCell(new Phrase("Quantity", headerFont));
        headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(headerCell);

        headerCell = new PdfPCell(new Phrase("Price", headerFont));
        headerCell.BackgroundColor = BaseColor.LIGHT_GRAY;
        headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
        table.AddCell(headerCell);

        // Add Table Rows
        foreach (var product in products)
        {
            table.AddCell(new PdfPCell(new Phrase(product[0], bodyFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase(product[1], bodyFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            table.AddCell(new PdfPCell(new Phrase(product[2], bodyFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
        }

        document.Add(table);

        // Add Amount Details at the Bottom Right
        PdfPTable amountTable = new PdfPTable(1);
        amountTable.WidthPercentage = 30; // Adjust as needed
        amountTable.HorizontalAlignment = Element.ALIGN_RIGHT;

        Font boldBodyFont = FontFactory.GetFont("Arial", 12, Font.BOLD);

        amountTable.AddCell(new PdfPCell(new Phrase($"Total Amount: PKR {totalAmount}", boldBodyFont)) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
        amountTable.AddCell(new PdfPCell(new Phrase($"Amount Paid: PKR {amountPaid}", boldBodyFont)) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
        amountTable.AddCell(new PdfPCell(new Phrase($"Amount Due: PKR {amountDue}", boldBodyFont)) { Border = PdfPCell.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });

        document.Add(amountTable);

        document.Close();

        byte[] bytes = memoryStream.ToArray();
        memoryStream.Close();

        // Send the PDF to the client
        response.ContentType = "application/pdf";
        response.AddHeader("content-disposition", "attachment;filename=Invoice.pdf");
        response.Buffer = true;
        response.Clear();
        response.OutputStream.Write(bytes, 0, bytes.Length);
        response.OutputStream.Flush();
        response.End();
    }
}

public class PDFWatermark : PdfPageEventHelper
{
    public override void OnEndPage(PdfWriter writer, Document document)
    {
        PdfContentByte under = writer.DirectContentUnder;
        Font watermarkFont = FontFactory.GetFont("Arial", 50, Font.BOLD, new BaseColor(255, 0, 0, 50));
        Phrase watermark = new Phrase("ZA-Traders", watermarkFont);
        ColumnText.ShowTextAligned(under, Element.ALIGN_CENTER, watermark, 297.5f, 421, 45);
    }
}