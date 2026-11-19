using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.AddVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController(
        IUseCase<AddVehicleInput> addVehicleUseCase) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddVehicle(
            [FromBody] AddVehicleInput input)
        {
            await addVehicleUseCase.Execute(input);
            return NoContent();
        }
    }
}
