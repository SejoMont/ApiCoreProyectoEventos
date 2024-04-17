using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCoreProyectoEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistasEventoController : ControllerBase
    {
        private readonly ArtistasEventoRepository _repo;
        private readonly EventosRepository _eventosRepository;
        private readonly UsuariosRepository _usuariosRepository;

        public ArtistasEventoController(ArtistasEventoRepository repo, EventosRepository eventosRepository, UsuariosRepository usuariosRepository)
        {
            _repo = repo;
            _eventosRepository = eventosRepository;
            _usuariosRepository = usuariosRepository;
        }

        [HttpGet("GetArtistasByEvento/{idevento}")]
        public async Task<ActionResult<List<UsuarioDetalles>>> GetArtistasByEvento(int idevento)
        {
            var evento = await _eventosRepository.GetDetallesEventoAsync(idevento);
            if (evento == null)
                return NotFound("Evento no encontrado.");

            var artistas = await _usuariosRepository.GetAllArtistas();
            return Ok(artistas);
        }

        [HttpPost("AddArtistaToEvento/{idevento}/{idartista}")]
        public async Task<ActionResult> AddArtistaToEvento(int idevento, int idartista)
        {
            try
            {
                await _repo.AddArtistaToEvento(idevento, idartista);
                return Ok("Artista agregado al evento correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el artista al evento: " + ex.Message);
            }
        }

        [HttpPost("CrearArtista")]
        public async Task<ActionResult> CrearArtista([FromForm] Artista artista, IFormFile foto)
        {
            // Lógica para manejar la carga del archivo y creación del artista
            try
            {
                // Aquí se debe integrar la lógica para subir el archivo, por ejemplo
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", foto.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
                artista.Foto = foto.FileName;

                return Ok("Artista creado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear el artista: " + ex.Message);
            }
        }
    }
}
