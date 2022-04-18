using api.Models;

namespace api.Application.Interfaces
{
    public interface IAdlsService
    {
        Task<IEnumerable<LogWork>> GetFromAdlsAsync();
        
        Task DeleteFromAdlsAsync(string[] AdlsIds);

        Task<IEnumerable<LogWork>> TransferAdlsAsync(LogWorkSearchParameter parameter, string userTransfer = null);

        Task TransferAdlsAsync(string logId, string ownerId, DateTime logDate);

        Task RestoreFromAdlsAsync();

        Task RefreshPowerBIDataAsync();
    }
}