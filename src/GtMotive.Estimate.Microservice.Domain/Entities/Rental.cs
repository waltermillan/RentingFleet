using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Represents a vehicle rental.
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the VehicleId identifier.
        /// </summary>
        public Guid VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the CustomerId identifier.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the rental start date.
        /// </summary>
        public DateTime RentalDate { get; set; }

        /// <summary>
        /// Gets or sets the rental return date.
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether the rental is active.
        /// </summary>
        public bool IsActive => ReturnDate == null;
    }
}
