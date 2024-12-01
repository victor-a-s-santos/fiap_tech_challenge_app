namespace FIAP.Crosscutting.Domain.Interfaces.Services
{
    public interface ICryptographyService
    {
        public string Encrypt(string text);
        public string Decrypt(string text);
    }
}
