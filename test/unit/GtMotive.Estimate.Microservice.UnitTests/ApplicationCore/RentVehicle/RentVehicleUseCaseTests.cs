using System;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.RentVehicle
{
    /// <summary>
    /// Unit tests for RentVehicleUseCase.
    /// </summary>
    public class RentVehicleUseCaseTests
    {
        /// <summary>
        /// Verifies that an available vehicle can be rented.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteRentsVehicleWhenVehicleIsAvailable()
        {
            var vehicleId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var vehicle = new Vehicle
            {
                Id = vehicleId,
                Brand = "Toyota",
                Model = "Corolla",
                Year = DateTime.UtcNow.Year - 2,
            };

            var vehicleRepository = new Mock<IVehicleRepository>();
            vehicleRepository
                .Setup(r => r.GetById(vehicleId))
                .ReturnsAsync(vehicle);

            var rentalRepository = new Mock<IRentalRepository>();
            rentalRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync([]);

            var outputPort = new Mock<IOutputPortStandard<RentVehicleOutput>>();

            var useCase = new RentVehicleUseCase(
                vehicleRepository.Object,
                rentalRepository.Object,
                outputPort.Object);

            var input = new RentVehicleInput
            {
                VehicleId = vehicleId,
                CustomerId = customerId,
            };

            await useCase.Execute(input);

            rentalRepository.Verify(
                r => r.Add(It.Is<Rental>(rental =>
                    rental.VehicleId == vehicleId &&
                    rental.CustomerId == customerId &&
                    rental.ReturnDate == null)),
                Times.Once);

            outputPort.Verify(
                p => p.StandardHandle(It.IsAny<RentVehicleOutput>()),
                Times.Once);
        }

        /// <summary>
        /// Verifies that a vehicle with an active rental cannot be rented again.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteThrowsDomainExceptionWhenVehicleIsAlreadyRented()
        {
            var vehicleId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var vehicleRepository = new Mock<IVehicleRepository>();
            vehicleRepository
                .Setup(r => r.GetById(vehicleId))
                .ReturnsAsync(new Vehicle { Id = vehicleId });

            var rentalRepository = new Mock<IRentalRepository>();
            rentalRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync(
                [
                    new Rental
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = vehicleId,
                        CustomerId = Guid.NewGuid(),
                        RentalDate = DateTime.UtcNow,
                    },
                ]);

            var outputPort = new Mock<IOutputPortStandard<RentVehicleOutput>>();

            var useCase = new RentVehicleUseCase(
                vehicleRepository.Object,
                rentalRepository.Object,
                outputPort.Object);

            var input = new RentVehicleInput
            {
                VehicleId = vehicleId,
                CustomerId = customerId,
            };

            Func<Task> act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<DomainException>();

            rentalRepository.Verify(
                r => r.Add(It.IsAny<Rental>()),
                Times.Never);
        }

        /// <summary>
        /// Verifies that a customer with another active rental cannot rent a vehicle.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteThrowsDomainExceptionWhenCustomerHasActiveRental()
        {
            var vehicleId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var vehicleRepository = new Mock<IVehicleRepository>();
            vehicleRepository
                .Setup(r => r.GetById(vehicleId))
                .ReturnsAsync(new Vehicle { Id = vehicleId });

            var rentalRepository = new Mock<IRentalRepository>();
            rentalRepository
                .Setup(r => r.GetAll())
                .ReturnsAsync(
                [
                    new Rental
                    {
                        Id = Guid.NewGuid(),
                        VehicleId = Guid.NewGuid(),
                        CustomerId = customerId,
                        RentalDate = DateTime.UtcNow,
                    },
                ]);

            var outputPort = new Mock<IOutputPortStandard<RentVehicleOutput>>();

            var useCase = new RentVehicleUseCase(
                vehicleRepository.Object,
                rentalRepository.Object,
                outputPort.Object);

            var input = new RentVehicleInput
            {
                VehicleId = vehicleId,
                CustomerId = customerId,
            };

            Func<Task> act = () => useCase.Execute(input);

            await act.Should().ThrowAsync<DomainException>();

            rentalRepository.Verify(
                r => r.Add(It.IsAny<Rental>()),
                Times.Never);
        }
    }
}
