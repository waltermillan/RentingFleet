using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController(
        IUseCase<RentVehicleInput> useCase,
        RentVehiclePresenter presenter) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RentVehicle(
            [FromBody] RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);
            await useCase.Execute(input);
            return presenter.ActionResult;
        }
    }
}
