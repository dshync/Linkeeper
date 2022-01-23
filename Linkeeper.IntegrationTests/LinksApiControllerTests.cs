using FluentAssertions;
using Linkeeper.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Linkeeper.IntegrationTests
{
	//run tests one-by-one
	//TODO: Fix parallel execution of the tests
	public class LinksApiControllerTests : LinkeeperIntegrationTest
	{
		[Fact]
		public async Task GetAllUserLinksAsync_ReturnsEmptyIfUserHasNoLinks()
		{
			// arrange
			await AuthenticateAsync();

			// act
			var response = await _httpTestClient.GetAsync("/api/link");

			// assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			List<Link> links = await response.Content.ReadJsonAsAsync<List<Link>>();
			links.Should().BeEmpty();
		}

		[Fact]
		public async Task AddLinkAsync_ReturnsInsertedLink()
		{
			// arrange
			await AuthenticateAsync();
			Link link = new Link { Id = 1, Address = "https://www.test.com", Representation = "Test" };
			ByteArrayContent byteContent = CreateJsonHttpContent<Link>(link);

			// act
			var response = await _httpTestClient.PostAsync("api/link", byteContent);

			// assert
			Link responseLink = await response.Content.ReadJsonAsAsync<Link>();
			response.StatusCode.Should().Be(HttpStatusCode.Created);
			//Regex.IsMatch(response.Headers.Location.ToString(), $"{_httpTestClient.BaseAddress}/api/link/\d{3}$").Should().Be(true);
			responseLink.Id.Should().NotBe(null);
			responseLink.Address.Should().Be(link.Address);
			responseLink.Representation.Should().Be(link.Representation);
		}

		[Fact]
		public async Task GetLinkByIdAsync_ReturnsLink()
		{
			// arrange
			await AuthenticateAsync();
			Link link = await CreateLinkAsync(new Link { Id = 1, Address = "https://www.test.com", Representation = "Test" });
			
			// act
			var response = await _httpTestClient.GetAsync($"/api/link/{link.Id}");

			// assert
			Link responseLink = await response.Content.ReadJsonAsAsync<Link>();

			response.StatusCode.Should().Be(HttpStatusCode.OK);
			responseLink.Address.Should().Be(link.Address);
			responseLink.Representation.Should().Be(link.Representation);
		}

		[Fact]
		public async Task UpdateLinkAsync_ReturnsNoContent()
		{
			// arrange
			await AuthenticateAsync();
			Link tmp = await CreateLinkAsync(new Link { Id = 1, Address = "https://www.test.com", Representation = "Test" });
			Link link = new Link { Id = tmp.Id, Address = "https://www.qwerty.com", Representation = "Qwerty" };
			ByteArrayContent byteContent = CreateJsonHttpContent<Link>(link);

			// act
			var response = await _httpTestClient.PutAsync($"/api/link/{link.Id}", byteContent);
			var responseCheck = await _httpTestClient.GetAsync($"/api/link/{link.Id}");

			// assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);

			Link responseLink = await responseCheck.Content.ReadJsonAsAsync<Link>();
			responseLink.Address.Should().Be(link.Address);
			responseLink.Representation.Should().Be(link.Representation);
		}
		
		[Fact]
		public async Task DeleteLinkAsync_ReturnsNoContent()
		{
			// arrange
			await AuthenticateAsync();
			Link link = await CreateLinkAsync(new Link { Id = 1, Address = "https://www.test.com", Representation = "Test" });

			// act
			var response = await _httpTestClient.DeleteAsync($"/api/link/{link.Id}");
			var responseCheck = await _httpTestClient.GetAsync($"/api/link/{link.Id}");

			// assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
			responseCheck.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
