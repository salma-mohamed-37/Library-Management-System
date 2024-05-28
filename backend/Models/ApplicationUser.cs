using Microsoft.AspNetCore.Identity;

namespace backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Borrowed> Borrowed { get; set; }
    }

}
