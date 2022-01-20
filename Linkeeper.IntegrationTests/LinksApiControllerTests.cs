using FluentAssertions;
using Linkeeper.DTOs.ApiIdentity;
using Linkeeper.Models;
using System;
using System.Collections.Generic;
using System.Net;
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
            var response = await _httpTestClient.GetAsync("/api/links");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            List<Link> links = await JsonResponseToObjectAsync<List<Link>>(response);
            links.Should().BeEmpty();
        }
    }
}
