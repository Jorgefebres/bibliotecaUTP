using BIBLIOTECA.Models.Acciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("AccionRoles")]
    public class AccionRol
    {
        [Key]
        public int IdAccionRol { get; set; }

        [Index(IsUnique = false)]
        public AccionesEnum IdPermiso { get; set; }

        //Foreign keys
        public int IdAccion { get; set; }
        public int IdRol { get; set; }

        //Relacion de uno a muchos
        public virtual Accion Acciones { get; set; }
        public virtual Rol Roles { get; set; }

    }
}