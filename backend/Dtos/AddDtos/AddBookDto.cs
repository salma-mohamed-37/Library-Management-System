
namespace backend.Dtos.AddDtos
{
    public class AddBookDto
    {
        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public int Category_id { get; set; }
        public int Author_id { get; set; }
    }
}
