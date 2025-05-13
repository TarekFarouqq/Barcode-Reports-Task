namespace Barcode_Reports_Task.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T?> GetLatestAsync();
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        void SaveChanges();
    }
}
