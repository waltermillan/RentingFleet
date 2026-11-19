using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.AddVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.AddVehicle
{
    /// <summary>
    /// Unit tests for AddVehicleUseCase.
    /// </summary>
    public class AddVehicleUseCaseTests
    {
        /// <summary>
        /// Verifies that an eligible vehicle is added to the fleet.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteAddsVehicleWhenVehicleIsEligible()
        {
            var repository = new Mock<IVehicleRepository>();
            var useCase = new AddVehicleUseCase(repository.Object);

            var input = new AddVehicleInput
            {
                Brand = "Toyota",
                Model = "Corolla",
                Year = DateTime.UtcNow.Year - 3,
            };

            await useCase.Execute(input);

            repository.Verify(
                r => r.Add(It.Is<Vehicle>(v =>
                    v.Brand == input.Brand &&
                    v.Model == input.Model &&
                    v.Year == input.Year)),
                Times.Once);
        }

        /// <summary>
        /// Verifies that adding a vehicle older than five years throws a DomainException.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteThrowsDomainExceptionWhenVehicleIsTooOld()
        {
            var repository = new Mock<IVehicleRepository>();
            var useCase = new AddVehicleUseCase(repository.Object);

            var input = new AddVehicleInput
            {
                Brand = "Toyota",
                Model = "Corolla",
                Year = DateTime.UtcNow.Year - 6,
            };

            Func<Task> act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<DomainException>();

            repository.Verify(
                r => r.Add(It.IsAny<Vehicle>()),
                Times.Never);
        }
    }
}
