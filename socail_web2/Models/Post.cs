using System.ComponentModel.DataAnnotations;

namespace socail_web2.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string? Description { get; set; }
       
        public string? ApplicationUserId { get; set; }

        [Required(ErrorMessage = "Please upload a proper image")]
        public string Image {get; set; }

        public List<LikeDislike> LikeDislike { get; set; } = new List<LikeDislike>();



    }
}
