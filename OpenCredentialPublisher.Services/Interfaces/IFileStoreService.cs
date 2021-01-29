using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Interfaces
{
    public interface IFileStoreService
    {
        Task<string> DownloadAsStringAsync(string filename, string blobContainerName);
        Task<byte[]> DownloadAsync(string filename, string blobContainerName);
        Task<string> StoreAsync(string filename, byte[] contents, string blobContainerName);
        Task<string> StoreAsync(string filename, string contents, string blobContainerName);
    }
}