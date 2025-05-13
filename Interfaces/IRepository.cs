namespace Barcode_Reports_Task.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetLatestAsync();
        void SaveChanges();
    }
}
