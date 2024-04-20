using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCoreProyectoEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntradasController : ControllerBase
    {
        private readonly EventosRepository repo;

        public EntradasController(EventosRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet("VerEntradas/{iduser}")]
        public async Task<IActionResult> VerEntradas(int iduser)
        {
            List<EntradaDetalles> entradasUsuario = await repo.GetAllEntradasUsuarioAsync(iduser);
            if (entradasUsuario == null || entradasUsuario.Count == 0)
            {
                return NotFound("No se encontraron entradas para el usuario especificado.");
            }

            return Ok(entradasUsuario);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AsignarEntrada([FromBody] AsistenciaEvento entrada)
        {
            await this.repo.AsignarEntradasAsync(entrada);
            return Ok(entrada);
        }

        [HttpPost("RestarEntrada/{idevento}")]
        public async Task<IActionResult> RestarEntrada(int idevento)
        {
            EventoDetalles evento = await this.repo.GetDetallesEventoAsync(idevento);
            if (evento != null)
            {
                await this.repo.RestarEntrada(idevento);
                return Ok();
            }
            else
            {
                return NotFound("El evento no existe.");
            }
        }
    }
}
