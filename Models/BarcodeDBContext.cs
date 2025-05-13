using Microsoft.EntityFrameworkCore;

namespace Barcode_Reports_Task.Models
{
    public class BarcodeDBContext : DbContext
    {

        public BarcodeDBContext(DbContextOptions<BarcodeDBContext> options): base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }

    }
}
