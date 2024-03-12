using backend.Models;

namespace backend.Dtos.GetDtos
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Category_name { get; set; }
        public string Author_name { get; set; }
        public string ImagePath { get; set; } 
        public bool currently_borrowed { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string UserEmail { get; set; }
    }
}
