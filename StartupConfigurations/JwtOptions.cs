namespace EFCorePostgres.StartupConfigurations
{
    public class JwtOptions
    {
        public required string key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string subject { get; set; }
        public required string AccessTokenValidity { get; set; }
        public required string RefreshTokenValidity { get; set; }

    }
}
