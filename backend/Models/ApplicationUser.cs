using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Fullname is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DateOfBirth { get; set; }
        public List<Borrowed> Borrowed { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }

    }

}
