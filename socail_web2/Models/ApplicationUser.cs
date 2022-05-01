using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace socail_web2.Models
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }


        public string? ProfilePic { get; set; }

        public bool Active { get; set; } = true;

    }
}
