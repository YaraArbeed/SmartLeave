namespace SmartLeave.API.DTOs
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public bool Is2FactorRequired { get; set; }
        public string? Provider { get; set; }
    }
}
