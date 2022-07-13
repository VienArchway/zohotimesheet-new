using api.Infrastructure.Interfaces;
using Azure.Storage.Files.DataLake;
using Azure.Storage;
using Azure.Storage.Files.DataLake.Models;
using System.Text;

namespace api.Infrastructure.Clients
{
    public class AdlsClient : IAdlsClient
    {
        private readonly String storageName;

        private readonly String key;

        private readonly String connectionString;

        private readonly DataLakeServiceClient dataLakeServiceClient;

        public AdlsClient(IConfiguration configuration)
        {
            this.storageName = configuration.GetValue<String>("Adls:StorageName");
            this.key = configuration.GetValue<String>("Adls:Key");
            this.connectionString = configuration.GetValue<String>("Adls:ConnectionString");
            var sharedKeyCredential = new StorageSharedKeyCredential(storageName, this.key);
            dataLakeServiceClient = new DataLakeServiceClient(this.connectionString);
        }

        public bool CheckExist(String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var fileClient = fileSystemClient.GetFileClient(destFilePath);
            return fileClient.Exists();
        }

        public async Task CreateFileAsync(String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            await fileSystemClient.CreateFileAsync(destFilePath);
        }

        public async Task UploadAsync(String content, String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var fileClient = fileSystemClient.GetFileClient(destFilePath);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content))) {
                await fileClient.UploadAsync(stream, true);
            }       
        }

        public void Upload(String content, String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var fileClient = fileSystemClient.GetFileClient(destFilePath);
    
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content))) {
                fileClient.Upload(stream, true);
            }       
        }

        public async Task<String> DownloadAsync(String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var directoryClient = fileSystemClient.GetDirectoryClient(String.Empty);
            var fileClient = directoryClient.GetFileClient(destFilePath);

            var downloadResponse = await fileClient.ReadAsync();

            using (var reader = new StreamReader(downloadResponse.Value.Content))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        public String Download(String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var directoryClient = fileSystemClient.GetDirectoryClient(String.Empty);
            var fileClient = directoryClient.GetFileClient(destFilePath);

            var downloadResponse = fileClient.Read();

            using (var reader = new StreamReader(downloadResponse.Value.Content))
            {
                return reader.ReadToEnd();
            }
        }

        public async Task<PathProperties> GetPropertiesAsync(String destFilePath)
        {
            var fileSystemClient = dataLakeServiceClient.GetFileSystemClient("zohotimesheet");
            var directoryClient = fileSystemClient.GetDirectoryClient(String.Empty);
            var fileClient = directoryClient.GetFileClient(destFilePath);

            var response = await fileClient.GetPropertiesAsync().ConfigureAwait(false);
            return response.Value;
        }
    }
}