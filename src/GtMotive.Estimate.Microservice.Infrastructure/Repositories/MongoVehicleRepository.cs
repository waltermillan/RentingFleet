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
    /// MongoDB implementation of the vehicle repository.
    /// </summary>
    public class MongoVehicleRepository(MongoService mongoService) : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _collection =
            mongoService.MongoDatabase.GetCollection<Vehicle>("Vehicles");

        /// <inheritdoc />
        public Task Add(Vehicle vehicle)
        {
            return _collection.InsertOneAsync(vehicle);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _collection.Find(FilterDefinition<Vehicle>.Empty).ToListAsync();
        }

        /// <inheritdoc />
        public Task<Vehicle> GetById(Guid id)
        {
            return _collection.Find(vehicle => vehicle.Id == id).FirstOrDefaultAsync();
        }
    }
}
