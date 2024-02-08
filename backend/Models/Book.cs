namespace backend.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public int Category_id { get; set; }
        public Category Category { get; set; }
        public int Author_id { get; set; }
        public Author Author { get; set; }
    }
}
