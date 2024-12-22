namespace Final.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? RealName { get; set; }
        public string? UserName { get; set; }
        public string? Gender { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? Birthday { get; set; }
        public string? RoleIds { get; set; }
        public int? Deleted { get; set; }

        public string UserId { get; set; }
        public string Password { get; set; }
         


    }
}
