using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Account
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Fullname is required")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? Type { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
    }
}
