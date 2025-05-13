using Barcode_Reports_Task.Models;

namespace Barcode_Reports_Task.Interfaces
{
    public interface IReportService
    {
        Task<int> SaveReportAsync(Report report, IFormFile file);
        Task<Report?> GetLatestReportAsync();
    }
}
