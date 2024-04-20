﻿using ApiCoreProyectoEventos.Models;
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
        private readonly EventosRepository repo;

        public ArtistasEventoController(EventosRepository repo)
        {
            this.repo = repo;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<List<Artista>>> GetArtistasTempEvento(int idevento)
        {
            var artistas = await this.repo.GetArtistasTempAsync(idevento);
            return Ok(artistas);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetArtistasEvento(int idevento)
        {
            var artistas = await this.repo.GetAllArtistasEventoAsync(idevento);
            return Ok(artistas);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllArtistas()
        {
            var artistas = await this.repo.GetAllArtistas();
            return Ok(artistas);
        }

        [HttpPost("AddArtistaToEvento/{idevento}/{idartista}")]
        public async Task<ActionResult> AddArtistaEvento(int idevento, int idartista)
        {
            try
            {
                await this.repo.AddArtistaToEvento(idevento, idartista);
                return Ok("Artista agregado al evento correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el artista al evento: " + ex.Message);
            }
        }

        [HttpPost("CrearArtista")]
        public async Task<ActionResult> CrearArtista([FromBody] Artista artista)
        {
            try
            {
                await this.repo.CrearArtistaAsync(artista);
                return Ok("Artista agregado al evento correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al agregar el artista al evento: " + ex.Message);
            }
        }
    }
}
