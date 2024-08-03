namespace backend.Dtos.Account
{
    public class LoggedInUserDto
    {
        public string Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}