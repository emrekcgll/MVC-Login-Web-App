namespace Login.Models
{
    public class PasswordCode
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string  Code { get; set; }
    }
}
