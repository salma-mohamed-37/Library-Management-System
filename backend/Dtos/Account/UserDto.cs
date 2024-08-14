namespace backend.Dtos.Account
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Fullname { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string City { get; set; }

        public string ImagePath { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
