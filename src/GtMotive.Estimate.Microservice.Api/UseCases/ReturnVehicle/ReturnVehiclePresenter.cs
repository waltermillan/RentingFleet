using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.ReturnVehicle
{
    /// <summary>
    /// Presenter for returning a vehicle.
    /// </summary>
    public class ReturnVehiclePresenter() : IWebApiPresenter,
        IOutputPortStandard<ReturnVehicleOutput>
    {
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(ReturnVehicleOutput response)
        {
            ActionResult = new OkObjectResult(response);
        }
    }
}
