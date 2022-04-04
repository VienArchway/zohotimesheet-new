using api.Models;

namespace api.Application.Interfaces
{
    public interface ITokenService
    {
        Task<Token> GetAccessTokenAsync(string code);
        Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken);
    }
}