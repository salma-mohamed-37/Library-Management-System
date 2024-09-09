using System.ComponentModel.DataAnnotations;
namespace backend.Dtos.AddDtos
{
    public class AddBorrowDto
    {
        [Required(ErrorMessage = "User id is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Books ids is required")]
        public ICollection<int> BooksIds { get; set; }
    }
}
