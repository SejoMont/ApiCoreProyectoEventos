using ApiCoreProyectoEventos.Helpers;
using ApiCoreProyectoEventos.Models;
using ApiCoreProyectoEventos.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcCoreProyectoSejo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private EventosRepository repo;
        private HelperActionServicesOAuth helper;

        public UsuariosApiController(EventosRepository repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet("GetUser/{correo}")]
        public async Task<ActionResult<Usuario>> GetUser(string correo)
        {
            Usuario user = await this.repo.GetUserAsync(correo);
            if (user == null)
                return NotFound("Usuario no encontrado");
            return Ok(user);
        }


        [HttpGet("Details/{iduser}")]
        public async Task<ActionResult<UsuarioDetalles>> Details(int iduser)
        {
            UsuarioDetalles usuarioDetalles = await this.repo.GetUsuarioDetalles(iduser);
            if (usuarioDetalles == null)
                return NotFound("Usuario no encontrado");
            return Ok(usuarioDetalles);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(Login login)
        {
            bool loginSuccess = await this.repo.LogInUserAsync(login.Correo, login.Password);
            if (loginSuccess)
            {
                SigningCredentials credentials =
                    new SigningCredentials(
                        this.helper.GetKeyToken()
                        , SecurityAlgorithms.HmacSha256);


                Usuario user = await this.repo.GetUserAsync(login.Correo);

                string jsonUser =
                    JsonConvert.SerializeObject(user);

                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonUser)
                };

                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler()
                        .WriteToken(token)
                    });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("Registro")]
        public async Task<ActionResult> Registro(Registro registro)
        {
            if (repo.EmailExists(registro.Correo))
            {
                return BadRequest("El correo electrónico ya está en uso");
            }

            if (registro.Password != registro.ConfirmPassword)
            {
                return BadRequest("Las contraseñas no coinciden");
            }

            Usuario user = await repo.RegisterUserAsync(registro.Nombre, registro.Correo, registro.Password, 1);

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
            UsuarioDetalles existingUser = await repo.GetUsuarioDetalles(id);
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
