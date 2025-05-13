using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace Barcode_Reports_Task.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] GenerateReportPdf(Report report, string webRootPath)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
            var bodyFont = new XFont("Arial", 12);

            // Title
            gfx.DrawString("Report", titleFont, XBrushes.Black,
                new XRect(0, 0, page.Width, 50), XStringFormats.Center);

            // Text
            gfx.DrawString("Title: " + report.Title, bodyFont, XBrushes.Black, new XPoint(20, 80));
            gfx.DrawString("Description: " + report.Description, bodyFont, XBrushes.Black, new XPoint(20, 110));

            // Image
            var imagePath = Path.Combine(webRootPath, report.ImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(imagePath))
            {
                var image = XImage.FromFile(imagePath);
                gfx.DrawImage(image, 20, 140, 200, 200);
            }

            using var stream = new MemoryStream();
            doc.Save(stream, false);
            return stream.ToArray();
        }
    }
}
