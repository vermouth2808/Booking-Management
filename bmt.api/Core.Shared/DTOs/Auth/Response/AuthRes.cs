namespace Core.Shared.DTOs.Auth.Response
{
    public class AuthRes
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
