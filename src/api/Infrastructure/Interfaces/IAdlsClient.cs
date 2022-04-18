using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;
using Azure.Storage.Files.DataLake.Models;

namespace api.Infrastructure.Interfaces
{
    public interface IAdlsClient
    {
        bool CheckExist(string destFilePath);

        Task CreateFileAsync(string destFilePath);

        Task UploadAsync(string content, string destFilePath);

        void Upload(string content, string destFilePath);

        Task<string> DownloadAsync(string destFilePath);

        string Download(string destFilePath);

        Task<PathProperties> GetPropertiesAsync(string destFilePath);
    }
}
