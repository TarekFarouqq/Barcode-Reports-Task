using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using Microsoft.EntityFrameworkCore;

namespace Barcode_Reports_Task.Repositories
{
    public class ReportRepo : IRepository<Report>
    {
        private readonly BarcodeDBContext _context;

        public ReportRepo(BarcodeDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Report entity)
        {
            await _context.Reports.AddAsync(entity);
        }

        public async Task<Report?> GetLatestAsync()
        {
            return await _context.Reports.OrderByDescending(r => r.CreatedAt).FirstOrDefaultAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}