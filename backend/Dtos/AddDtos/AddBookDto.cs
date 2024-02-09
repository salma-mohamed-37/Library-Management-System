
namespace backend.Dtos.AddDtos
{
    public class AddBookDto
    {
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public IFormFile CoverFile { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
    }
}
