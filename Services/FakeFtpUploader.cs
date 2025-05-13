using Barcode_Reports_Task.Interfaces;

namespace Barcode_Reports_Task.Services
{
    public class FakeFtpUploader : IFtpUploader
    {
        public Task<bool> UploadImageAsync(string localFilePath, string fileName)
        {
            Console.WriteLine($"[SIMULATED FTP] Uploading {fileName} from {localFilePath}");
            return Task.FromResult(true);
        }
    }
}
