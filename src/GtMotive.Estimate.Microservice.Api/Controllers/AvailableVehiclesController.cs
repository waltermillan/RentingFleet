using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailableVehiclesController(
        IUseCase<GetAvailableVehiclesInput> useCase,
        GetAvailableVehiclesPresenter presenter) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAvailableVehicles()
        {
            await useCase.Execute(new GetAvailableVehiclesInput());
            return presenter.ActionResult;
        }
    }
}
