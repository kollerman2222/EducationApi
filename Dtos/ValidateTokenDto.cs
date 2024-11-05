namespace FgssrApi.Dtos
{
    public class ValidateTokenDto
    {
        public string? Message { get; set; }
        public bool isTokenValid { get; set; }
        public string? Token { get; set; }
    }
}
