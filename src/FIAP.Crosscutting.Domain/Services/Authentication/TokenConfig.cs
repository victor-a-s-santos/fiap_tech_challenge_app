namespace FIAP.Crosscutting.Domain.Services.Authentication
{
    public class TokenConfig
    {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string Issuer { get; set; }
        public string HmacSecretKey { get; set; }
        public string ExpirationHours { get; set; }
    }
}
