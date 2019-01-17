using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("Roles")]
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required]
        [Display(Name = "Rol"), Column(TypeName = "varchar"), StringLength(50)]
        public string NomRol { get; set; }

        //Relacion de uno a muchos
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<AccionRol> AccionRoles { get; set; }

    }
}