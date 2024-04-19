using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ApiCoreProyectoEventos.Models;

#region VISTAS
//CREATE VIEW VISTA_ENTRADAS_DETALLE AS
//SELECT 
//    AE.AsistenciaID,
//    AE.UsuarioID,
//    AE.EventoID,
//    AE.Nombre,
//    AE.Correo,
//    AE.DNI,
//    AE.QR,
//    DE.NombreEvento,
//    DE.Fecha,
//    DE.Provincia,
//    DE.Imagen,
//    DE.Recinto
//FROM 
//    AsistenciasEventos AE
//JOIN 
//    VISTA_DETALLES_EVENTO DE ON AE.EventoID = DE.EventoID;
#endregion

namespace ApiCoreProyectoEventos.Repository
{
    public class EntradasRepository
    {
        private EventosContext context;

        public EntradasRepository(EventosContext context)
        {
            this.context = context;
        }
       
    }
}
