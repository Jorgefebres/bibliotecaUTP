using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("Acciones")]
    public class Accion
    {
        [Key]
        public int IdAccion { get; set; }


        [Required]
        [StringLength(20)]
        public string Modulo { get; set; }

        [Required]
        [StringLength(100), Display(Name = "Acción")]
        public string NomAccion { get; set; }

        //Relacion de uno a muchos
        public virtual ICollection<AccionRol> AccionRoles { get; set; }
    }
}