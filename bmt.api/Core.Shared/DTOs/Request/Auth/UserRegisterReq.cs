namespace Core.Shared.DTOs.Request.Auth
{
    public class UserRegisterReq
    {
        public string? UserName { get; set; }
        public string? PassWord { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
