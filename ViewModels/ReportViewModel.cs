using System.ComponentModel.DataAnnotations;

namespace Barcode_Reports_Task.ViewModels
{
    public class ReportViewModel
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        public string ExistingImagePath { get; set; }
    }
}
