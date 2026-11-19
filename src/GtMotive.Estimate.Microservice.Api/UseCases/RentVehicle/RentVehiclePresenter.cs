using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Presenter for renting a vehicle.
    /// </summary>
    public class RentVehiclePresenter() : IWebApiPresenter,
        IOutputPortStandard<RentVehicleOutput>
    {
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(RentVehicleOutput response)
        {
            ActionResult = new OkObjectResult(response);
        }
    }
}
