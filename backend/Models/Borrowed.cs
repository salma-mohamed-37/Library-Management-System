namespace backend.Models
{
    public class Borrowed
    {
        public int User_id { get; set; }
        public int Book_id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Book Book { get; set; }
        public ApplicationUser User { get; set; }
    }
}
