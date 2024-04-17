using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCoreProyectoEventos.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        [Column("RolID")]
        public int Id { get; set; }

        [Column("NombreRol")]
        [StringLength(20)]
        public string Nombre { get; set; }
    }
}
