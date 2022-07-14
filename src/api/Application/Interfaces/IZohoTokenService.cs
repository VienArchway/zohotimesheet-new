using api.Models;

namespace api.Application.Interfaces
{
    public interface IZohoTokenService
    {
        Task<Token> GetAccessTokenAsync(String code);

        Task<Token> GetAccessTokenByRefreshTokenAsync(String firstName, String zsUserId);

        Task RevokeRefreshTokenAsync();
    }
}