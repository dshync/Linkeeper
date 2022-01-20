using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Linkeeper.Data;
using Linkeeper.DTOs.ApiIdentity;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Linkeeper.IntegrationTests
{
    public class LinkeeperIntegrationTest
    {
        protected readonly HttpClient _httpTestClient;
        
        protected LinkeeperIntegrationTest()
        {
            //swaping real dbcontext to in-memory
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(LinkeeperContext));
                        if (context != null)
                        {
                            services.Remove(context);
                            var options = services.Where(r => (r.ServiceType == typeof(LinkeeperContext))
                              || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                            foreach (var option in options)
                            {
                                services.Remove(option);
                            }
                        services.AddDbContext<LinkeeperContext>(opt => opt.UseInMemoryDatabase("TestDB"));
                        }

                    });
                });

            //creating http client
            _httpTestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _httpTestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            //creating json registration request as ByteArrayContent(HttpContent)
            UserRegistrationRequestDTO request = new UserRegistrationRequestDTO { Email = "t@test.t", Password = "Qwerty1!" };
            string requestJson = JsonConvert.SerializeObject(request);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(requestJson);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //sending request and returning jwt
            var response = await _httpTestClient.PostAsync("api/identity/register", byteContent);
            AuthenticationResultDTO result = await JsonResponseToObjectAsync<AuthenticationResultDTO>(response);
            return result.Token;
        }

        protected async Task<T> JsonResponseToObjectAsync<T>(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
