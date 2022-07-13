using api.Models;

namespace api.Application.Interfaces
{
    public interface IAdlsService
    {
        Task<IEnumerable<LogWork>> GetFromAdlsAsync();
        
        Task DeleteFromAdlsAsync(String[] AdlsIds);

        Task<IEnumerable<LogWork>> TransferAdlsAsync(LogWorkSearchParameter parameter, String userTransfer = null);

        Task TransferAdlsAsync(String logId, String ownerId, DateTime logDate);

        Task RestoreFromAdlsAsync();

        Task RefreshPowerBIDataAsync();
    }
}