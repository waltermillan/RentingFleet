using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Defines operations for vehicle persistence.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Adds a vehicle to the fleet.
        /// </summary>
        /// <param name="vehicle">Vehicle to add.</param>
        /// <returns>Task representing the operation.</returns>
        Task Add(Vehicle vehicle);

        /// <summary>
        /// Gets all vehicles.
        /// </summary>
        /// <returns>Collection of vehicles.</returns>
        Task<IEnumerable<Vehicle>> GetAll();

        /// <summary>
        /// Gets a vehicle by its identifier.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle if found; otherwise, null.</returns>
        Task<Vehicle> GetById(Guid id);
    }
}
