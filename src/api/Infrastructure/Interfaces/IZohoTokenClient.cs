using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IZohoTokenClient
    {
        Task<Token> GetAccessTokenAsync(String code);

        Task<Token> GetAccessTokenFromRefreshTokenAsync(String firstName, String zsUserId);

        Task<Token> GetAdminAccessTokenAsync();

        Task RevokeRefreshTokenAsync();
    }
}
