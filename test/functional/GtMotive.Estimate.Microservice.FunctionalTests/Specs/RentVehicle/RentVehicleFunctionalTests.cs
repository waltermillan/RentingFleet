using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Specs.RentVehicle
{
    /// <summary>
    /// Functional tests for renting vehicles.
    /// </summary>
    public class RentVehicleFunctionalTests
    {
        /// <summary>
        /// Verifies that a vehicle can be rented successfully.
        /// </summary>
        /// <returns>A task representing the asynchronous test.</returns>
        [Fact]
        public async Task ExecuteShouldCreateRentalWhenVehicleIsAvailable()
        {
            var vehicleId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var vehicleRepository = new InMemoryVehicleRepository();
            var rentalRepository = new InMemoryRentalRepository();
            var outputPort = new TestOutputPort();

            vehicleRepository.Vehicles.Add(new Vehicle
            {
                Id = vehicleId,
                Brand = "Toyota",
                Model = "Corolla",
                Year = DateTime.UtcNow.Year - 2,
            });

            var useCase = new RentVehicleUseCase(
                vehicleRepository,
                rentalRepository,
                outputPort);

            await useCase.Execute(new RentVehicleInput
            {
                VehicleId = vehicleId,
                CustomerId = customerId,
            });

            var rental = Assert.Single(rentalRepository.Rentals);

            Assert.Equal(vehicleId, rental.VehicleId);
            Assert.Equal(customerId, rental.CustomerId);
            Assert.NotEqual(Guid.Empty, outputPort.Output.RentalId);
            Assert.Equal(rental.Id, outputPort.Output.RentalId);
        }

        private sealed class InMemoryVehicleRepository : IVehicleRepository
        {
            public List<Vehicle> Vehicles { get; } =
                Enumerable.Empty<Vehicle>().ToList();

            public Task Add(Vehicle vehicle)
            {
                Vehicles.Add(vehicle);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<Vehicle>> GetAll()
            {
                return Task.FromResult<IEnumerable<Vehicle>>(Vehicles);
            }

            public Task<Vehicle> GetById(Guid id)
            {
                return Task.FromResult(Vehicles.First(v => v.Id == id));
            }
        }

        private sealed class InMemoryRentalRepository : IRentalRepository
        {
            public List<Rental> Rentals { get; } =
                Enumerable.Empty<Rental>().ToList();

            public Task<IEnumerable<Rental>> GetAll()
            {
                return Task.FromResult<IEnumerable<Rental>>(Rentals);
            }

            public Task Add(Rental rental)
            {
                Rentals.Add(rental);
                return Task.CompletedTask;
            }

            public Task<Rental> GetActiveByVehicleId(Guid vehicleId)
            {
                return Task.FromResult(
                    Rentals.First(r => r.VehicleId == vehicleId && r.IsActive));
            }

            public Task Update(Rental rental)
            {
                return Task.CompletedTask;
            }
        }

        private sealed class TestOutputPort : IOutputPortStandard<RentVehicleOutput>
        {
            public RentVehicleOutput Output { get; private set; }

            public void StandardHandle(RentVehicleOutput response)
            {
                Output = response;
            }
        }
    }
}
