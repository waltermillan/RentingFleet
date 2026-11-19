using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Services;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Use case for renting a vehicle.
    /// </summary>
    public class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IOutputPortStandard<RentVehicleOutput> outputPort)
        : IUseCase<RentVehicleInput>
    {
        /// <inheritdoc />
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            await vehicleRepository.GetById(input.VehicleId);

            var rentals = await rentalRepository.GetAll();

            if (RentalPolicy.HasActiveRentalForVehicle(rentals, input.VehicleId))
            {
                throw new DomainException("The vehicle is already rented.");
            }

            if (RentalPolicy.HasActiveRentalForCustomer(rentals, input.CustomerId))
            {
                throw new DomainException("The customer already has an active rental.");
            }

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                VehicleId = input.VehicleId,
                CustomerId = input.CustomerId,
                RentalDate = DateTime.UtcNow,
            };

            await rentalRepository.Add(rental);

            outputPort.StandardHandle(new RentVehicleOutput
            {
                RentalId = rental.Id,
            });
        }
    }
}
