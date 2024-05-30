namespace backend.Dtos.Account
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
