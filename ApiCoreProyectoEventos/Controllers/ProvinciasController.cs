using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreProyectoEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciasController : ControllerBase
    {
        private EventosRepository repo;

        public ProvinciasController(EventosRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProvinciasAsync()
        {
            var provincias = await repo.GetAllProvinciassAsync();

            return Ok(provincias);
        }
    }
}
