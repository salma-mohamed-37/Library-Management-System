using System.ComponentModel.DataAnnotations;
namespace backend.Dtos.AddDtos
{
    public class AddBorrowDto
    {
        [Required(ErrorMessage = "User email is required")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Book id is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Borrow date is required")]
        public DateTime BorrowDate { get; set; }

        [Required(ErrorMessage = "Return date is required")]
        public DateTime ReturnDate { get; set; }
    }
}
