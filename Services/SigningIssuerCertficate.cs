using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace EFCorePostgres.Services
{

    public interface IRsaCertficate 
    {
          Task<SigningCredentials> GetAudienceSigningKey();
          Task<RsaSecurityKey> GetIssuerSigningKey();
    }

    public class SigningIssuerCertficate : IRsaCertficate
    {

        private SigningCredentials _audienceSigningKey;

        private RsaSecurityKey _issuerSigningKey;

        private RSA _rsa = RSA.Create();

        public async Task<SigningCredentials> GetAudienceSigningKey()
        {
            if(_audienceSigningKey != null)
                return _audienceSigningKey;

            var privateKey = await File.ReadAllTextAsync("private_key.pem");

            _rsa.ImportFromPem(privateKey.ToCharArray());
            _audienceSigningKey = new SigningCredentials(key: new RsaSecurityKey(_rsa), algorithm: SecurityAlgorithms.RsaSha256);
            return _audienceSigningKey;
        }

        public async Task<RsaSecurityKey> GetIssuerSigningKey()
        {
            if (_issuerSigningKey != null)
                return _issuerSigningKey;

            var publicKey = await File.ReadAllTextAsync("public_key.pem");

            _rsa.ImportFromPem(publicKey.ToCharArray());
            _issuerSigningKey = new RsaSecurityKey(_rsa);
            return _issuerSigningKey;

        }

    }
}
