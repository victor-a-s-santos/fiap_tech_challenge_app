using FIAP.Crosscutting.Domain.Interfaces.Services;
using FIAP.Crosscutting.Domain.Security;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Paddings;
using System.Text;

namespace FIAP.Crosscutting.Domain.Services.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        private readonly CryptographyConfig _config;

        public CryptographyService(CryptographyConfig config)
        {
            _config = config;
        }

        public string Encrypt(string plain)
        {
            Encoding encoding = Encoding.ASCII;
            var pkcs = new Pkcs7Padding();
            IBlockCipherPadding padding = pkcs;

            var bcEngine = new BouncyCastleEngine(new AesEngine(), encoding);
            bcEngine.SetPadding(padding);

            return bcEngine.Encrypt(plain, _config.Key);
        }

        public string Decrypt(string cipher)
        {
            Encoding encoding = Encoding.ASCII;
            var pkcs = new Pkcs7Padding();
            IBlockCipherPadding padding = pkcs;

            var bcEngine = new BouncyCastleEngine(new AesEngine(), encoding);
            bcEngine.SetPadding(padding);

            return bcEngine.Decrypt(cipher, _config.Key);
        }
    }
}
