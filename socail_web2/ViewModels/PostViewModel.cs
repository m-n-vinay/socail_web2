using System.ComponentModel.DataAnnotations;

namespace socail_web2.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Picture")]
        [Required(ErrorMessage = "Please upload a proper image")]
        public IFormFile Image { get; set; }
    }
}
