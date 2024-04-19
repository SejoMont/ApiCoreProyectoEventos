using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ApiCoreProyectoEventos.Models;

namespace ApiCoreProyectoEventos.Repository
{
    public class ArtistasEventoRepository
    {
        private EventosContext context;

        public ArtistasEventoRepository(EventosContext context)
        {
            this.context = context;
        }

        

    }
}
