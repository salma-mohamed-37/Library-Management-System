using backend.Dtos.Account;

namespace backend.Dtos.GetDtos.Book
{
    public class GetBorrowedBookForUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
