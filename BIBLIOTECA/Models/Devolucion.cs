using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Devoluciones")]
    public class Devolucion
    {
        [Key]
        public int IdDevolucion { get; set; }

        [Display(Name ="Fecha de Devolución")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecDevolcion { get; set; }

        //ForeignKey
        [Index(IsUnique = true)]
        public int IdPrestamoDetalle { get; set; }

        public int IdUsuario { get; set; }

        //Relacion de muchos a uno
        public virtual PrestamoDetalle PrestamoDetalles { get; set; }
        public virtual Usuario Usuarios { get; set; }

        //Relacion de uno a muchos
        public virtual ICollection<Sancion> Sanciones { get; set; }

    }
}