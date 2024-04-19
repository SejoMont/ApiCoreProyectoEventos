using Microsoft.EntityFrameworkCore;
using ApiCoreProyectoEventos.Models;

namespace ApiCoreProyectoEventos.Repository
{
    public class ProvinciasRepository
    {
        private EventosContext context;

        public ProvinciasRepository(EventosContext context)
        {
            this.context = context;
        }
        

    }
}
