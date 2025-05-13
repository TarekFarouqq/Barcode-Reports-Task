using System;
using System.IO;
using System.Collections.Generic;
using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using PdfSharpCore.Drawing;
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

            double margin = 40;
            double y = margin;

            // Font configuration
            var titleFont = new XFont("Verdana", 15, XFontStyle.Bold);
            var labelFont = new XFont("Arial", 12, XFontStyle.Bold);
            var contentFont = new XFont("Arial", 12);
            double lineHeight = contentFont.GetHeight() * 1.2;

            // Image paths
            string headerImagePath = Path.Combine(webRootPath, "images", "logo.jpeg");
            string footerImagePath = Path.Combine(webRootPath, "images", "footer.jpeg");

            // --- Header Section ---
            if (File.Exists(headerImagePath))
            {
                var logo = XImage.FromFile(headerImagePath);
                double logoWidth = 100;
                double logoHeight = logo.PixelHeight * logoWidth / logo.PixelWidth;

                gfx.DrawImage(logo, margin, y, logoWidth, logoHeight);
                gfx.DrawString("Report", titleFont, XBrushes.Black,
                    new XRect(logoWidth + margin + 10, y, page.Width - logoWidth - 2 * margin, logoHeight),
                    XStringFormats.CenterRight);

                y += logoHeight + 40;
            }
            else
            {
                gfx.DrawString("Report", titleFont, XBrushes.Black,
                    new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
                y += 60;
            }

            // Header separator
            gfx.DrawLine(new XPen(XColors.Black, 0.5), margin, y, page.Width - margin, y);
            y += 30;

            // --- Title Section ---
            gfx.DrawString("Title:", labelFont, XBrushes.Black, new XPoint(margin, y));
            y += 20;
            gfx.DrawString(report.Title ?? "-", contentFont, XBrushes.Black, new XPoint(margin, y));
            y += 30;

            // --- Description Section ---
            gfx.DrawString("Description:", labelFont, XBrushes.Black, new XPoint(margin, y));
            y += 20;

            string description = report.Description ?? "-";
            double textWidth = page.Width - 2 * margin;

            foreach (var line in WrapText(description, contentFont, textWidth, gfx))
            {
                if (y > page.Height - 150) break; 
                gfx.DrawString(line, contentFont, XBrushes.Black, new XPoint(margin, y));
                y += lineHeight;
            }
            y += 30;

            // --- Report Image ---
            var reportImagePath = Path.Combine(webRootPath,
                report.ImagePath?.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()) ?? "");

            if (File.Exists(reportImagePath))
            {
                var image = XImage.FromFile(reportImagePath);
                double availableHeight = page.Height - y - 120;
                double availableWidth = page.Width - 2 * margin;

                double scale = Math.Min(
                    availableWidth / image.PixelWidth,
                    availableHeight / image.PixelHeight
                ) * 0.8;

                gfx.DrawImage(image,
                    (page.Width - (image.PixelWidth * scale)) / 2,
                    y,
                    image.PixelWidth * scale,
                    image.PixelHeight * scale
                );
            }

            // --- Footer Section ---
            if (File.Exists(footerImagePath))
            {
                var footer = XImage.FromFile(footerImagePath);
                double footerHeight = page.Height * 0.1;
                double footerWidth = footer.PixelWidth * (footerHeight / footer.PixelHeight);

                gfx.DrawImage(footer,
                    (page.Width - footerWidth) / 2,
                    page.Height - footerHeight,
                    footerWidth,
                    footerHeight
                );
            }

            using var stream = new MemoryStream();
            doc.Save(stream, false);
            return stream.ToArray();
        }

        private List<string> WrapText(string text, XFont font, double maxWidth, XGraphics gfx)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(text)) return lines;

            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                string testLine = currentLine == "" ? word : $"{currentLine} {word}";
                XSize size = gfx.MeasureString(testLine, font);

                if (size.Width > maxWidth && currentLine != "")
                {
                    lines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines;
        }
    }
}