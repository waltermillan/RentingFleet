using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Defines operations for rental persistence.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Gets all rentals.
        /// </summary>
        /// <returns>Collection of rentals.</returns>
        Task<IEnumerable<Rental>> GetAll();

        /// <summary>
        /// Adds a rental.
        /// </summary>
        /// <param name="rental">Rental to add.</param>
        /// <returns>Task representing the operation.</returns>
        Task Add(Rental rental);

        /// <summary>
        /// Gets the active rental for a vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <returns>The active rental for the specified vehicle.</returns>
        Task<Rental> GetActiveByVehicleId(Guid vehicleId);

        /// <summary>
        /// Updates an existing rental.
        /// </summary>
        /// <param name="rental">Rental to update.</param>
        /// <returns>Task representing the operation.</returns>
        Task Update(Rental rental);
    }
}
