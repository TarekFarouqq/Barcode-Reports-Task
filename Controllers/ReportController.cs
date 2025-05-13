using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using Barcode_Reports_Task.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Barcode_Reports_Task.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IFtpUploader _ftpUploader;
        private readonly IPdfExportService _pdfExportService;
        private readonly IWebHostEnvironment _env;

        public ReportController(IReportService reportService, IFtpUploader ftpUploader, IPdfExportService pdfExportService, IWebHostEnvironment env)
        {
            _reportService = reportService;
            _ftpUploader = ftpUploader;
            _pdfExportService = pdfExportService;
            _env = env;
        }

        public async Task<IActionResult> Index() 
        {
            var reports = await _reportService.GetAllReportsAsync();
            return View(new ReportListViewModel { Reports = (List<Report>)reports });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ReportViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Save(ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var report = new Report
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now
            };

            await _reportService.SaveReportAsync(report, model.Image);
            TempData["Success"] = "Report saved successfully!";
            return RedirectToAction("Index");
        
        }

        [HttpPost]
        public async Task<IActionResult> UploadFtp(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();

            var localPath = Path.Combine(_env.WebRootPath, report.ImagePath.TrimStart('/'));
            await _ftpUploader.UploadImageAsync(localPath, Path.GetFileName(report.ImagePath));

            TempData["Success"] = "Image uploaded via FTP successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null) return NotFound();

            var pdfBytes = _pdfExportService.GenerateReportPdf(report, _env.WebRootPath);
            return File(pdfBytes, "application/pdf", $"{report.Title}.pdf");
        }
    }
}
