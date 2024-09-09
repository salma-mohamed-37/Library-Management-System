namespace backend.Dtos.Account
{
    public class ChangePasswordDto
    {
        public required string OldPassword { set; get; }
        public required string NewPassword { set; get; }
        public string? UserId { set; get; }
    }
}
