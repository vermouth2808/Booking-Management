namespace Core.Shared.DTOs.Response.Auth
{
    public class AuthRes
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
