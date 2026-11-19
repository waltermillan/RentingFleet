using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Output data returned after returning a vehicle.
    /// </summary>
    public class ReturnVehicleOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public Guid RentalId { get; set; }
    }
}
