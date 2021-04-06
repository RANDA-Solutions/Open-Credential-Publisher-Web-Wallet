using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class RevocationDocumentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RevocationDocumentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RevocationDocument> GetRevocationDocumentAsync(string url)
        {
            using var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            if (response.Content is object)
            {
                return System.Text.Json.JsonSerializer.Deserialize<RevocationDocument>(await response.Content.ReadAsStringAsync());
            }
            throw new Exception("There was an issue processing your connect request.");
        }
    }
}
