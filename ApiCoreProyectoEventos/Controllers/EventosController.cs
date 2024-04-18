﻿using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreProyectoSejo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private EventosRepository _repo;
        private UsuariosRepository _userRepo;
        private ProvinciasRepository _provinciasRepo;
        private EntradasRepository _entradasRepo;
        private ArtistasEventoRepository _artistsRepo;

        public EventosController(EventosRepository repo,
            UsuariosRepository userRepo,
            ProvinciasRepository provinciasRepo,
            EntradasRepository entradasRepo,
            ArtistasEventoRepository artistsRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
            _provinciasRepo = provinciasRepo;
            _entradasRepo = entradasRepo;
            _artistsRepo = artistsRepo;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventos([FromQuery] FiltroEvento filtro, int page = 1, int pageSize = 8)
        {
            var eventos = await _repo.BuscarEventosPorFiltros(filtro);
            var paginatedData = eventos.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = eventos.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var result = new
            {
                Data = paginatedData,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpGet("GetEvento/{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            var evento = await _repo.GetDetallesEventoAsync(id);
            if (evento == null)
                return NotFound();

            return Ok(evento);
        }

        [HttpPost("CrearEvento")]
        public async Task<IActionResult> CrearEvento([FromForm] Evento evento, IFormFile imagen)
        {
            try
            {
                if (imagen != null && imagen.Length > 0)
                {
                    var filePath = await SaveFile(imagen);
                    evento.Imagen = filePath;
                    await _repo.CrearEventoAsync(evento);
                    return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoID }, evento);
                }
                else
                {
                    return BadRequest("La imagen es requerida.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("AddComentario")]
        public async Task<IActionResult> AddComentario([FromBody] Comentario comentario)
        {
            await _repo.AddComentarioAsync(comentario);
            return CreatedAtAction(nameof(GetEvento), new { id = comentario.EventoID });
        }

        // Helper method to save a file
        private async Task<string> SaveFile(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
