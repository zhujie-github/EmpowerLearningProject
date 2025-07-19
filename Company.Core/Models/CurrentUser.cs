namespace Company.Core.Models
{
    public class CurrentUser
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime LastLoginTime { get; set; } = DateTime.Now;
    }
}
