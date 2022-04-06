using api.Models;

namespace api.Application.Interfaces
{
    public interface IZohoTokenService
    {
        Task<Token> GetAccessTokenAsync(string code);
        Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string accessToken);
    }
}