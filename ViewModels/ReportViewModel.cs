using System.ComponentModel.DataAnnotations;

namespace Barcode_Reports_Task.ViewModels
{
    public class ReportViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(2000, ErrorMessage = "Description cannot exceed 300 characters")]
        public string Description { get; set; }

        [Required]
        public IFormFile Image { get; set; }


    }
}
