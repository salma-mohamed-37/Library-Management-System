using backend.Dtos.Account;

namespace backend.Dtos.GetDtos.Book
{
    public class GetBookForLibrarianDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Category_name { get; set; }
        public string Author_name { get; set; }
        public string ImagePath { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool currently_borrowed { get; set; }
        public UserDto? User { get; set; }
    }
}
