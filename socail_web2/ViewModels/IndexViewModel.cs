using socail_web2.Models;

namespace socail_web2.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
