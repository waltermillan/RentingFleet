using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle belonging to the rental fleet.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the vehicle manufacturing year.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Only vehicles up to five years old are permitted.
        /// </summary>
        /// <returns>Element return value should be documented.</returns>
        public bool IsEligibleForFleet()
        {
            return DateTime.UtcNow.Year - Year <= 5;
        }
    }
}
