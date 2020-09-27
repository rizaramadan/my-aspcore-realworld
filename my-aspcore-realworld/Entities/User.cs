namespace my_aspcore_realworld.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Token { get; set; }
    }
}
