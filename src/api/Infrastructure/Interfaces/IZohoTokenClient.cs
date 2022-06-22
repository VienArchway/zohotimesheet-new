using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IZohoTokenClient
    {
        Task<Token> GetAccessTokenAsync(string code);
        Task<Token> GetAccessTokenFromRefreshTokenAsync(string displayName);

        Task<Token> GetAdminAccessTokenAsync();
        Task RevokeRefreshTokenAsync();
    }
}
