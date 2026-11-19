using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.AddVehicle
{
    /// <summary>
    /// Use case for adding a vehicle to the fleet.
    /// </summary>
    public class AddVehicleUseCase(IVehicleRepository vehicleRepository) : IUseCase<AddVehicleInput>
    {
        /// <inheritdoc />
        public async Task Execute(AddVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = input.Brand,
                Model = input.Model,
                Year = input.Year,
            };

            if (!vehicle.IsEligibleForFleet())
            {
                throw new DomainException(
                    "The vehicle is older than five years and cannot be added to the fleet.");
            }

            await vehicleRepository.Add(vehicle);
        }
    }
}
