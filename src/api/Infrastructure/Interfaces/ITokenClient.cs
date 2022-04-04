using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Infrastructure.Interfaces
{
    public interface ITokenClient
    {
        Task<Token> GetAccessTokenAsync(string code);
        Task<Token> GetAccessTokenFromRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string token);
    }
}
