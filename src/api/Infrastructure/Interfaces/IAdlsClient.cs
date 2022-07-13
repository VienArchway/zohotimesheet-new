using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;
using Azure.Storage.Files.DataLake.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IAdlsClient
    {
        bool CheckExist(String destFilePath);

        Task CreateFileAsync(String destFilePath);

        Task UploadAsync(String content, String destFilePath);

        void Upload(String content, String destFilePath);

        Task<String> DownloadAsync(String destFilePath);

        String Download(String destFilePath);

        Task<PathProperties> GetPropertiesAsync(String destFilePath);
    }
}
