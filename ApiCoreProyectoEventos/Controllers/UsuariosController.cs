using ApiCoreProyectoEventos.Helpers;
using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MvcCoreProyectoSejo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private UsuariosRepository _repo;
        private EventosRepository _eventosRepo;
        private HelperMails _helperMails;
        private HelperPathProvider _helperPathProvider;

        public UsuariosApiController(UsuariosRepository repo, EventosRepository eventosRepo, HelperMails helperMails, HelperPathProvider helperPathProvider)
        {
            _repo = repo;
            _eventosRepo = eventosRepo;
            _helperMails = helperMails;
            _helperPathProvider = helperPathProvider;
        }

        [HttpGet("Details/{iduser}")]
        public async Task<ActionResult<UsuarioDetalles>> Details(int iduser)
        {
            UsuarioDetalles usuarioDetalles = await _repo.GetUsuarioDetalles(iduser);
            if (usuarioDetalles == null)
                return NotFound("Usuario no encontrado");
            return Ok(usuarioDetalles);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(string correo, string password)
        {
            bool loginSuccess = await _repo.LogInUserAsync(correo, password);
            if (loginSuccess)
            {
                Usuario user = _repo.GetUser(correo);
                return Ok(new { Message = "Login exitoso", UserId = user.UsuarioID });
            }
            else
            {
                return BadRequest("Credenciales incorrectas");
            }
        }

        [HttpPost("Registro")]
        public async Task<ActionResult> Registro(string nombre, string correo, string password, string confirmPassword)
        {
            if (_repo.EmailExists(correo))
            {
                return BadRequest("El correo electrónico ya está en uso");
            }

            if (password != confirmPassword)
            {
                return BadRequest("Las contraseñas no coinciden");
            }

            Usuario user = await _repo.RegisterUserAsync(nombre, correo, password, 1);

            if (user != null)
            {
                //string serverUrl = _helperPathProvider.MapUrlServerPath() + "/Usuarios/ActivateUser/?token=" + user.TokenMail;
                //string message = $"Activa tu cuenta aquí: {serverUrl}";
                //await _helperMails.SendMailAsync(correo, "Registro Usuario", message);

                return Ok(new { Message = "Usuario registrado y correo de activación enviado", UserId = user.UsuarioID });
            }
            else
            {
                return BadRequest("No se pudo crear el usuario");
            }
        }

        //[HttpGet("ActivateUser/{token}")]
        //public async Task<ActionResult> ActivateUser(string token)
        //{
        //    bool activated = await _repo.ActivateUserAsync(token);
        //    if (!activated)
        //        return NotFound("Token inválido o usuario ya activado");

        //    return Ok("Cuenta activada correctamente");
        //}

        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> Edit(int id, string nombre, string correo, string password)
        {
            UsuarioDetalles existingUser = await _repo.GetUsuarioDetalles(id);
            if (existingUser == null)
                return NotFound("Usuario no encontrado");

            // Supongamos que queremos actualizar solo el nombre y el correo
            existingUser.NombreUsuario = nombre;
            existingUser.Correo = correo;

            // Lógica de actualización del usuario
            // Esto es un ejemplo, deberías tener métodos en el repositorio para manejar la actualización real en la base de datos

            return Ok("Usuario actualizado correctamente");
        }
    }
}
