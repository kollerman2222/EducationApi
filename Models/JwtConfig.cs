namespace FgssrApi.Models
{
    public class JwtConfig
    {
            public string Key { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public string JwtCookieEncryptionKey { get; set; }


    }
}
