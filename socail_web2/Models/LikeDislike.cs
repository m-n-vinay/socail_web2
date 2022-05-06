namespace socail_web2.Models
{
    public class LikeDislike
    {
        public int Id { get; set; }
        public bool? IsLiked { get; set; } = null;
        public ApplicationUser ApplicationUser { get; set; }

       // public virtual int PostId { get; set; }
    }
}
