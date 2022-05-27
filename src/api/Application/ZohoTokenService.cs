using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class ZohoTokenService :  IZohoTokenService
    {
        private readonly IZohoTokenClient client;

        public ZohoTokenService(IZohoTokenClient client)
        {
            this.client = client;
        }

        public Task<Token> GetAccessTokenAsync(string code)
        {
            return client.GetAccessTokenAsync(code);
        }

        public Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken)
        {
            return client.GetAccessTokenFromRefreshTokenAsync(refreshToken);
        }

        public Task<Token> GetAdminAccessTokenAsync()
        {
            return client.GetAdminAccessTokenAsync();
        }

        public async Task RevokeRefreshTokenAsync(string token)
        {
            await client.RevokeRefreshTokenAsync(token);
        }
    }
}