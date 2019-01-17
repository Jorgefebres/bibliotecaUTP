using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Prestamos")]
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }

        [Display(Name = "Fecha de préstamo")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecPrestamo { get; set; }

        [Display(Name = "¿Prestamo Pendiente?")]
        public bool Pendiente { get; set; }

        /* Foreign Key */
        public int IdUsuario { get; set; }
        public int IdPersona { get; set; }

        /* Relaciones - Uno a Uno */
        public virtual Usuario Usuarios { get; set; }
        public virtual Persona Personas { get; set; }

        /* Relaciones - Uno a Varios */
        public virtual ICollection<PrestamoDetalle> PrestamoDetalles { get; set; }
    }
}