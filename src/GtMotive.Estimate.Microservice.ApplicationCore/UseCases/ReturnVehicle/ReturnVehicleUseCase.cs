using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Use case for returning a rented vehicle.
    /// </summary>
    public class ReturnVehicleUseCase(
        IRentalRepository rentalRepository,
        IOutputPortStandard<ReturnVehicleOutput> outputPort)
        : IUseCase<ReturnVehicleInput>
    {
        /// <inheritdoc />
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var rental = await rentalRepository.GetActiveByVehicleId(input.VehicleId);

            rental.ReturnDate = DateTime.UtcNow;

            await rentalRepository.Update(rental);

            outputPort.StandardHandle(new ReturnVehicleOutput
            {
                RentalId = rental.Id,
            });
        }
    }
}
