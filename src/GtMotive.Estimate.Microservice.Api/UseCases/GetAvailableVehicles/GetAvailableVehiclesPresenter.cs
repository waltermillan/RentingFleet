using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Presenter for retrieving available vehicles.
    /// </summary>
    public class GetAvailableVehiclesPresenter() : IWebApiPresenter,
        IOutputPortStandard<GetAvailableVehiclesOutput>
    {
        public IActionResult ActionResult { get; private set; }

        /// <inheritdoc />
        public void StandardHandle(GetAvailableVehiclesOutput response)
        {
            ActionResult = new OkObjectResult(response);
        }
    }
}
