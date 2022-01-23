using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Linkeeper.IntegrationTests
{
    public static class LinkeeperTestsExtentions
    {
        public static async Task<T> ReadJsonAsAsync<T>(this HttpContent content)
        {
            string responseBody = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
