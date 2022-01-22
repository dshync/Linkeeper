using FluentAssertions;
using Linkeeper.DTOs.ApiIdentity;
using Linkeeper.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace Linkeeper.IntegrationTests
{
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
			string requestJson = JsonConvert.SerializeObject(link);
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(requestJson);
			ByteArrayContent byteContent = new ByteArrayContent(buffer);
			byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

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
	}
}
