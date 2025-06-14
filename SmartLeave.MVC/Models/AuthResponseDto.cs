namespace SmartLeave.MVC.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public bool Is2FactorRequired { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
