using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Sanciones")]
    public class Sancion
    {
        [Key]
        public int IdSancion { get; set; }

        [Display(Name = "Motivo de sanción")]
        [Column(TypeName = "varchar"), StringLength(200), Required]
        public string Descripcion { get; set; }

        [Display(Name = "Estado Actual")]
        [Column(TypeName = "varchar"), StringLength(200), Required]
        public string Estado { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecFin { get; set; }

        //ForeignKey
        public int IdDevolucion { get; set; }

        //Relacion de Muchos a uno
        public virtual Devolucion Devoluciones { get; set; }


    }
}