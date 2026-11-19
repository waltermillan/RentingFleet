using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Output data returned after renting a vehicle.
    /// </summary>
    public class RentVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public Guid RentalId { get; set; }
    }
}
