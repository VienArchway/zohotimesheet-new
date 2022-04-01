using api.Application.Interfaces;
using api.Infrastructure.Interfaces;
using api.Models;

namespace api.Application
{
    public class TokenService :  ITokenService
    {
        private readonly ITokenClient client;

        public TokenService(ITokenClient client)
        {
            this.client = client;
        }

        public Task<Token> GetAccessTokenAsync(string code)
        {
            return client.GetAccessTokenAsync(code);
        }

        Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken)
        {
            return client.GetAccessTokenAsync(refreshToken);
        }
    }
}