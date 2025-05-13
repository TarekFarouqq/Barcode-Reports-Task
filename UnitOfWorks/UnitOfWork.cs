using Barcode_Reports_Task.Interfaces;
using Barcode_Reports_Task.Models;
using Barcode_Reports_Task.Repositories;

namespace Barcode_Reports_Task.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BarcodeDBContext _context;
        public IRepository<Report> Reports { get; }

        public UnitOfWork(BarcodeDBContext context)
        {
            _context = context;
            Reports = new ReportRepo(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

