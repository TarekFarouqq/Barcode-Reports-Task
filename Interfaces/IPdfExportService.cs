using Barcode_Reports_Task.Models;

namespace Barcode_Reports_Task.Interfaces
{
    public interface IPdfExportService
    {
        byte[] GenerateReportPdf(Report report, string webRootPath);
    }
}
