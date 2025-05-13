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

        public async Task<int> SaveReportAsync(Report report, IFormFile file)
        {
            report.ImagePath = await SaveFileAsync(file);
            await _unitOfWork.Reports.AddAsync(report);
            await _unitOfWork.CompleteAsync();
            return report.Id;
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "images", "reports");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/reports/{uniqueFileName}";
        }

        public async Task<Report?> GetLatestReportAsync()
        {
            return await _unitOfWork.Reports.GetLatestAsync();
        }
    }
}
