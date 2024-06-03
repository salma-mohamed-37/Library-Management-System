namespace backend.Dtos.GetDtos
{
    public class GetBorrowerDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
