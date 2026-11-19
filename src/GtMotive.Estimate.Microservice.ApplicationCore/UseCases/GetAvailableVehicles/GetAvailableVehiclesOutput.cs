using System.Collections.Generic;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Output data returned when retrieving available vehicles.
    /// </summary>
    public class GetAvailableVehiclesOutput : IUseCaseOutput
    {
        /// <summary>
        /// Gets or sets the available vehicles.
        /// </summary>
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
