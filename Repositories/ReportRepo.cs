using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using Barcode_Reports_Task.UnitOfWorks;
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
            Console.WriteLine("Adding report to context");
            await _context.Reports.AddAsync(entity);
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid report ID", nameof(id));
            return await _context.Reports.FindAsync(id);
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