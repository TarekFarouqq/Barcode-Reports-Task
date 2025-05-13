using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;

namespace Barcode_Reports_Task.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public ReportService(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _unitOfWork.Reports.GetAllAsync();
        }

        public async Task<Report> GetReportByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid report ID", nameof(id));
            var report = await _unitOfWork.Reports.GetByIdAsync(id);
            if (report == null) throw new InvalidOperationException($"Report with ID {id} not found.");
            return report;
        }
        public async Task<Report?> GetLatestReportAsync()
        {
            return await _unitOfWork.Reports.GetLatestAsync();
        }


        public async Task SaveReportAsync(Report report, IFormFile file)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "ReportsImage");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            report.ImagePath = "/ReportsImage/" + fileName;
            await _unitOfWork.Reports.AddAsync(report);
            await _unitOfWork.CompleteAsync();
        }

        //private async Task<string> SaveFileAsync(IFormFile file)
        //{
        //    var uploadFolder = Path.Combine(_env.WebRootPath, "images", "reports");
        //    if (!Directory.Exists(uploadFolder))
        //        Directory.CreateDirectory(uploadFolder);

        //    var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    var filePath = Path.Combine(uploadFolder, uniqueFileName);

        //    using var stream = new FileStream(filePath, FileMode.Create);
        //    await file.CopyToAsync(stream);

        //    return $"/images/reports/{uniqueFileName}";
        //}
      
    }
}
