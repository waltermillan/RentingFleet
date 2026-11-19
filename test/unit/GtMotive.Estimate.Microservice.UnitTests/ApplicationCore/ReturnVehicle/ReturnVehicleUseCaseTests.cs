using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.ReturnVehicle
{
    /// <summary>
    /// Unit tests for ReturnVehicleUseCase.
    /// </summary>
    public class ReturnVehicleUseCaseTests
    {
        /// <summary>
        /// Verifies that returning a vehicle updates the rental and sends the output.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteReturnsVehicleAndUpdatesRental()
        {
            var vehicleId = Guid.NewGuid();
            var rentalId = Guid.NewGuid();

            var rental = new Rental
            {
                Id = rentalId,
                VehicleId = vehicleId,
                CustomerId = Guid.NewGuid(),
                RentalDate = DateTime.UtcNow.AddDays(-2),
            };

            var rentalRepository = new Mock<IRentalRepository>();
            rentalRepository
                .Setup(r => r.GetActiveByVehicleId(vehicleId))
                .ReturnsAsync(rental);

            var outputPort = new Mock<IOutputPortStandard<ReturnVehicleOutput>>();

            ReturnVehicleOutput actualOutput = null;

            outputPort
                .Setup(p => p.StandardHandle(It.IsAny<ReturnVehicleOutput>()))
                .Callback<ReturnVehicleOutput>(output => actualOutput = output);

            var useCase = new ReturnVehicleUseCase(
                rentalRepository.Object,
                outputPort.Object);

            var input = new ReturnVehicleInput
            {
                VehicleId = vehicleId,
            };

            await useCase.Execute(input);

            rentalRepository.Verify(
                r => r.Update(It.Is<Rental>(updatedRental =>
                    updatedRental.Id == rentalId &&
                    updatedRental.ReturnDate != null)),
                Times.Once);

            actualOutput.Should().NotBeNull();
            actualOutput.RentalId.Should().Be(rentalId);

            outputPort.Verify(
                p => p.StandardHandle(It.IsAny<ReturnVehicleOutput>()),
                Times.Once);
        }
    }
}
