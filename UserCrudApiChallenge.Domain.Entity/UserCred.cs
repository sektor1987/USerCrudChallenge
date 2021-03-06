namespace UserCrudApiChallenge.Domain.Entity
{
    public class UserCred
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RefreshCred
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}