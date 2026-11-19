using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Specs.AddVehicle
{
    public sealed class AddVehicleInfrastructureTests
    {
        [Fact]
        public async Task PostShouldReturnBadRequestWhenModelIsInvalid()
        {
            using var fixture = new GenericInfrastructureTestServerFixture();
            using var client = fixture.Server.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/api/Vehicles",
                new
                {
                    Brand = string.Empty,
                    Model = "Corolla",
                    Year = 2024,
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
