using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.GetAvailableVehicles
{
    /// <summary>
    /// Unit tests for GetAvailableVehiclesUseCase.
    /// </summary>
    public class GetAvailableVehiclesUseCaseTests
    {
        /// <summary>
        /// Verifies that rented vehicles are excluded from available vehicles.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteExcludesVehiclesWithActiveRentals()
        {
            var availableVehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = "Toyota",
                Model = "Corolla",
                Year = DateTime.UtcNow.Year - 2,
            };

            var rentedVehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = "Ford",
                Model = "Focus",
                Year = DateTime.UtcNow.Year - 3,
            };

            var vehicles = new List<Vehicle>
            {
                availableVehicle,
                rentedVehicle,
            };

            var rentals = new List<Rental>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    VehicleId = rentedVehicle.Id,
                    CustomerId = Guid.NewGuid(),
                    RentalDate = DateTime.UtcNow,
                },
            };

            var vehicleRepository = new Mock<IVehicleRepository>();
            vehicleRepository.Setup(r => r.GetAll()).ReturnsAsync(vehicles);

            var rentalRepository = new Mock<IRentalRepository>();
            rentalRepository.Setup(r => r.GetAll()).ReturnsAsync(rentals);

            var actualOutput = new GetAvailableVehiclesOutput();

            var outputPort = new Mock<IOutputPortStandard<GetAvailableVehiclesOutput>>();

            outputPort
                .Setup(p => p.StandardHandle(It.IsAny<GetAvailableVehiclesOutput>()))
                .Callback<GetAvailableVehiclesOutput>(output => actualOutput = output);

            var useCase = new GetAvailableVehiclesUseCase(
                vehicleRepository.Object,
                rentalRepository.Object,
                outputPort.Object);

            await useCase.Execute(new GetAvailableVehiclesInput());

            actualOutput.Vehicles.Should()
                .ContainSingle()
                .Which.Id.Should().Be(availableVehicle.Id);
        }
    }
}
