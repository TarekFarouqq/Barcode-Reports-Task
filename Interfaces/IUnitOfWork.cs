using Barcode_Reports_Task.Models;

namespace Barcode_Reports_Task.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Report> Reports { get; }
        Task CompleteAsync();
    }
}
