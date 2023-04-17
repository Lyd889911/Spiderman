using System.Net;

namespace Spiderman.HTTP
{
    public class Spider
    {
        public async Task<string> Get(HttpOptionBuilder option)
        {
            var httpClientFactory = new HttpClientFactory();
            using var httpClient = httpClientFactory.Create(option);
            var response = await httpClient.SendAsync(option.HttpRequestMessage);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}