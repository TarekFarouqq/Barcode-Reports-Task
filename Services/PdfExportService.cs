using System;
using System.IO;
using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace Barcode_Reports_Task.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] GenerateReportPdf(Report report, string webRootPath)
        {
            var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);

            double margin = 40;
            double y = margin;

            // Fonts  
            var titleFont = new XFont("Verdana", 18, XFontStyle.Bold);
            var labelFont = new XFont("Arial", 12, XFontStyle.Bold);
            var contentFont = new XFont("Arial", 12);

            // Images  
            string headerImagePath = Path.Combine(webRootPath, "images", "logo.jpeg");
            string footerImagePath = Path.Combine(webRootPath, "images", "footer.jpeg");

            // --- Header ---  
            double headerHeight = 0;
            if (File.Exists(headerImagePath))
            {
                var logo = XImage.FromFile(headerImagePath);
                double logoWidth = 100;
                double logoHeight = logo.PixelHeight * logoWidth / logo.PixelWidth;
                gfx.DrawImage(logo, margin, y, logoWidth, logoHeight);

                gfx.DrawString("Report", titleFont, XBrushes.Black,
                    new XRect(logoWidth + margin + 10, y, page.Width - logoWidth - 2 * margin, logoHeight),
                    XStringFormats.CenterRight);

                headerHeight = logoHeight;
                y += logoHeight + 20;
            }
            else
            {
                gfx.DrawString("Report", titleFont, XBrushes.Black,
                    new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
                headerHeight = 40;
                y += 50;
            }

            // --- Header Border ---  
            var pen = new XPen(XColors.Black, 0.5);
            gfx.DrawLine(pen, margin, y, page.Width - margin, y);
            y += 60;

            // --- Title ---
            gfx.DrawString("Title:", labelFont, XBrushes.Black, new XPoint(margin, y));
            y += 10;
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(report.Title ?? "-", contentFont, XBrushes.Black,
                new XRect(margin, y, page.Width - 2 * margin, 20));
            y += 50;

            // --- Description ---
            gfx.DrawString("Description:", labelFont, XBrushes.Black, new XPoint(margin, y));
            y += 10;
            string description = report.Description ?? "-";
            double descBoxHeight = 100;
            var descRect = new XRect(margin, y, page.Width - 2 * margin, descBoxHeight);

            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(description, contentFont, XBrushes.Black, descRect);


            // Use DrawString with wrapping disabled to avoid stretching
            tf.DrawString(description, contentFont, XBrushes.Black, descRect);
            y += descBoxHeight + 20;

            // --- Report Image ---  
            var reportImagePath = Path.Combine(webRootPath,
                report.ImagePath?.TrimStart('/')?.Replace("/", Path.DirectorySeparatorChar.ToString()) ?? "");
            if (File.Exists(reportImagePath))
            {
                var image = XImage.FromFile(reportImagePath);
                double availableHeight = page.Height - y - 120; // leave room for footer  
                double availableWidth = page.Width - 2 * margin;

                double scale = Math.Min(availableWidth / image.PixelWidth, availableHeight / image.PixelHeight) * 0.8;
                double imgWidth = image.PixelWidth * scale;
                double imgHeight = image.PixelHeight * scale;
                double centerX = (page.Width - imgWidth) / 2;

                gfx.DrawImage(image, centerX, y, imgWidth, imgHeight);
                y += imgHeight + 20;
            }

            // --- Footer ---  
            if (File.Exists(footerImagePath))
            {
                var footer = XImage.FromFile(footerImagePath);
                double footerWidth = page.Width;
                double footerHeight = footer.PixelHeight * footerWidth / footer.PixelWidth;
                gfx.DrawImage(footer, 0, page.Height - footerHeight, footerWidth, footerHeight);
            }

            // Save and return  
            using var stream = new MemoryStream();
            doc.Save(stream, false);
            return stream.ToArray();
        }
    }
}