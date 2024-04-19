using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreProyectoEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private EventosRepository repo;

        public ComentariosController(EventosRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddComentario([FromBody] Comentario comentario)
        {
            await repo.AddComentarioAsync(comentario);

            //return CreatedAtAction(nameof(FindEvento), new { id = comentario.EventoID });
            return Ok(comentario);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetComentariosEvento(int idevento)
        {
            var comentarios = await repo.GetComentariosByEventoIdAsync(idevento);

            return Ok(comentarios);
        }
    }
}
