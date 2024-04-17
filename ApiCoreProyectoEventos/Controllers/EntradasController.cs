using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
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
        private readonly EntradasRepository _repo;

        public EntradasController(EntradasRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("VerEntradas/{iduser}")]
        public async Task<ActionResult<List<EntradaDetalles>>> VerEntradas(int iduser)
        {
            List<EntradaDetalles> entradasUsuario = await _repo.GetAllEntradasUsuarioAsync(iduser);
            if (entradasUsuario == null || entradasUsuario.Count == 0)
            {
                return NotFound("No se encontraron entradas para el usuario especificado.");
            }

            return Ok(entradasUsuario);
        }
    }
}
