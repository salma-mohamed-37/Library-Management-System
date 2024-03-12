namespace backend.Models
{
    public class Borrowed
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool currently_borrowed { get; set; }
        public Book Book { get; set; }
        public ApplicationUser User { get; set; }
    }
}
