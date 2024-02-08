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
    }
}
