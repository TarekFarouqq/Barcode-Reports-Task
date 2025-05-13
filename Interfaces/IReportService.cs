using Barcode_Reports_Task.Models;

namespace Barcode_Reports_Task.Interfaces
{
    public interface IReportService
    {
        Task SaveReportAsync(Report report, IFormFile file);
        Task<Report?> GetLatestReportAsync();
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(int id);
    }
}
