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

        public Task<Token> GetAccessTokenAsync(String code)
        {
            return client.GetAccessTokenAsync(code);
        }

        public Task<Token> GetAccessTokenFromRefreshTokenAsync(String firstName, String zsUserId)
        {
            return client.GetAccessTokenFromRefreshTokenAsync(firstName, zsUserId);
        }

        public async Task RevokeRefreshTokenAsync()
        {
            await client.RevokeRefreshTokenAsync();
        }
    }
}