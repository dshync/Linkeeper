using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Linkeeper.IntegrationTests
{
    public static class LinkeeperTestsExtentions
    {
        public static async System.Threading.Tasks.Task<T> ReadJsonAsAsync<T>(this HttpContent content)
        {
            string responseBody = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
