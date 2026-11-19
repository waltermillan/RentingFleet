using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    /// <summary>
    /// MongoDB implementation of the rental repository.
    /// </summary>
    public class MongoRentalRepository(MongoService mongoService) : IRentalRepository
    {
        private readonly IMongoCollection<Rental> _collection =
            mongoService.MongoDatabase.GetCollection<Rental>("Rentals");

        /// <inheritdoc />
        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _collection.Find(FilterDefinition<Rental>.Empty).ToListAsync();
        }

        /// <inheritdoc />
        public Task Add(Rental rental)
        {
            return _collection.InsertOneAsync(rental);
        }

        /// <inheritdoc />
        public Task<Rental> GetActiveByVehicleId(Guid vehicleId)
        {
            return _collection.Find(
                rental => rental.VehicleId == vehicleId && rental.ReturnDate == null)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task Update(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var filter = Builders<Rental>.Filter.Eq(
                existing => existing.Id,
                rental.Id);

            await _collection.ReplaceOneAsync(filter, rental);
        }
    }
}
