using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("ReservaDetalles")]
    public class ReservaDetalle
    {
        [Key]
        public int IdReservaDetalle { get; set; }

        /* Foreign Key */
        public int IdReserva { get; set; }          
        public int IdLibro { get; set; }

        [Display(Name = "¿Pendiente a Devolver?")]
        public bool Pendiente { get; set; }

        /* Relaciones - Uno a Uno */
        public virtual Reserva Reservas { get; set; }
        public virtual Libro Libros { get; set; }
    }
}