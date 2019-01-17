using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Reservas")]
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }

        [Display(Name = "Fecha de Reserva")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecReserva { get; set; }

        [Display(Name = "Fecha de Recojo")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecRecojo { get; set; }/* Día y hora en el que recojerá el libro (Automatico)*/

        [Display(Name = "¿Pendiente a Devolver?")]
        public bool Pendiente { get; set; }

        public int IdUsuario { get; set; }          /* Foreign Key */

        /* Relaciones - Uno a Uno */
        public virtual Usuario Usuarios { get; set; }

        /* Relaciones - Uno a Varios */
        public virtual ICollection<ReservaDetalle> ReservaDetalles { get; set; }
    }
}


//Valida las fecha festivos
/*public class UsuarioDateTimeValidation
{
    public static ValidationResult ValidaFechaFestivo(DateTime fecha)
    {
        return fecha.DayOfWeek == DayOfWeek.Saturday || fecha.DayOfWeek == DayOfWeek.Sunday
            ? new ValidationResult("Los festivos no son aceptados para esta fecha.")
            : ValidationResult.Success;
    }
}*/