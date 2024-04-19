#region VIEWS Y PROCEDURES
//ALTER VIEW VISTA_DETALLE_USUARIO AS
//SELECT
//    U.UsuarioID,
//    U.NombreUsuario,
//    U.FotoPerfil,
//    U.Correo,
//    U.Telefono,
//    U.ProvinciaID,
//    P.NombreProvincia,
//    U.Descripcion,
//    U.Activo,
//    R.NombreRol,
//    R.RolID
//FROM Usuarios U
//INNER JOIN Roles R ON U.RolID = R.RolID
//INNER JOIN Provincias P ON U.ProvinciaID = P.ProvinciaID;

//create view vista_detalle_artista as
//select
//    u.usuarioid,
//u.nombreusuario,
//    u.fotoperfil,
//    u.provinciaid,
//    p.nombreprovincia,
//    u.descripcion,
//    r.nombrerol,
//    r.RolID,
//    ae.eventoid
//from usuarios u
//inner join artistasevento ae on u.usuarioid = ae.artistaid
//inner join roles r on u.rolid = r.rolid
//inner join provincias p on u.provinciaid = p.provinciaid;  
#endregion

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ApiCoreProyectoEventos.Helpers;
using ApiCoreProyectoEventos.Models;

namespace ApiCoreProyectoEventos.Repository
{
    public class UsuariosRepository
    {
        private EventosContext context;

        public UsuariosRepository(EventosContext context)
        {
            this.context = context;
        }
        
    }
}