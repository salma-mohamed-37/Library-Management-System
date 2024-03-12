using System.ComponentModel.DataAnnotations;
namespace backend.Dtos.AddDtos
{
    public class AddReturnDto
    {
        [Required(ErrorMessage = "User email is required")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Book id is required")]
        public int BookId { get; set; }
    }
}
