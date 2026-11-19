using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Services;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Use case for retrieving available vehicles.
    /// </summary>
    public class GetAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IRentalRepository rentalRepository,
        IOutputPortStandard<GetAvailableVehiclesOutput> outputPort) : IUseCase<GetAvailableVehiclesInput>
    {
        /// <inheritdoc />
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var vehicles = await vehicleRepository.GetAll();
            var rentals = await rentalRepository.GetAll();

            var availableVehicles = vehicles.Where(vehicle =>
                !RentalPolicy.HasActiveRentalForVehicle(rentals, vehicle.Id));

            outputPort.StandardHandle(new GetAvailableVehiclesOutput
            {
                Vehicles = availableVehicles,
            });
        }
    }
}
