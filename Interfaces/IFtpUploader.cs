namespace Barcode_Reports_Task.Interfaces
{
    public interface IFtpUploader
    {
        Task<bool> UploadImageAsync(string localFilePath, string fileName);
    }
}
