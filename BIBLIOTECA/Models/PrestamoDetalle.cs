using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("PrestamoDetalles")]
    public class PrestamoDetalle
    {
        [Key]
        public int IdPrestamoDetalle { get; set; }

        [Display(Name = "Estado actual del libro")]
        [Column(TypeName = "varchar"), StringLength(200), Required]
        public string EstLibro { get; set; }

        [Display(Name = "Fecha de devolución")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecDevolucion { get; set; }
  
        [Display(Name = "¿Pendiente a Devolver?")]
        public bool Pendiente { get; set; }

        /* Foreign Key */
        public int IdPrestamo { get; set; }             
        public int IdLibro { get; set; }

        /* Relaciones - Uno a Uno */
        public virtual Prestamo Prestamos { get; set; }
        public virtual Libro Libros { get; set; }

        //Relacion de uno a muchos
        public virtual ICollection<Devolucion> Devoluciones { get; set; }

    }
}