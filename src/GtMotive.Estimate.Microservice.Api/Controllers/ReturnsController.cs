using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController(
        IUseCase<ReturnVehicleInput> useCase,
        ReturnVehiclePresenter presenter) : ControllerBase
    {
        [HttpPost("{vehicleId:guid}")]
        public async Task<IActionResult> ReturnVehicle(Guid vehicleId)
        {
            await useCase.Execute(
                new ReturnVehicleInput { VehicleId = vehicleId });

            return presenter.ActionResult;
        }
    }
}
