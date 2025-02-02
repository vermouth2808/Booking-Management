namespace Core.Shared.DTOs.Auth.Request
{
    public class UserRegisterReq
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
    }
}
