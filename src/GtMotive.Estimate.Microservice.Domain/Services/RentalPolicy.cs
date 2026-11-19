using System;
using System.Collections.Generic;
using System.Linq;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Services
{
    /// <summary>
    /// Contains business rules related to vehicle rentals.
    /// </summary>
    public static class RentalPolicy
    {
        /// <summary>
        /// Determines whether a customer already has an active rental.
        /// </summary>
        /// <param name="rentals">Existing rentals.</param>
        /// <param name="customerId">Customer identifier.</param>
        /// <returns>True if the customer has an active rental.</returns>
        public static bool HasActiveRentalForCustomer(
            IEnumerable<Rental> rentals, Guid customerId)
        {
            return rentals.Any(r =>
                r.CustomerId == customerId && r.IsActive);
        }

        /// <summary>
        /// Determines whether a vehicle already has an active rental.
        /// </summary>
        /// <param name="rentals">Existing rentals.</param>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <returns>True if the vehicle has an active rental.</returns>
        public static bool HasActiveRentalForVehicle(
            IEnumerable<Rental> rentals, Guid vehicleId)
        {
            return rentals.Any(r =>
                r.VehicleId == vehicleId && r.IsActive);
        }
    }
}
