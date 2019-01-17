using BIBLIOTECA.Models.Metodos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Index(IsUnique = true)]
        public int IdPersona { get; set; }          /* Foreign Key */

        [Index(IsUnique = false)]
        public int IdRol { get; set; }          /* Foreign Key */

        [Display(Name = "Nombre de usuario")]
        [Column(TypeName = "varchar"), StringLength(50), Required, Index(IsUnique = true)]
        public string NomUsuario { get; set; }

        [Display(Name = "Contraseña"), DataType(DataType.Password)]
        [Column(TypeName = "varchar"), StringLength(50), Required]
        public string Contrasena { get; set; }

        [Display(Name = "Estado")]
        [Required]
        public bool Estado { get; set; }

        [NotMapped]
        [Display(Name = "¿Recordar cuenta?")]
        public bool Recordar { get; set; }

        [NotMapped]
        [Display(Name = "Confireme contraseña"), DataType(DataType.Password)]
        [Compare("Contrasena")]
        public string ConfirmarContrasena { get; set; }

        /* Relaciones - Uno a Uno */
        public virtual Persona Personas { get; set; }
        public virtual Rol Roles { get; set; }

        /* Relaciones - Uno a Varios */
        public virtual ICollection<Reserva> Reservas { get; set; }

        [NotMapped]
        public string ConEncriptada { get { return SHA1.Encode(Contrasena); } }

        public string Encriptar(string value)
        {
            return SHA1.Encode(value);
        }
    }
}