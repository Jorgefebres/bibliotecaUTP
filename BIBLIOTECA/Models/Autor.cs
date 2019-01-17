using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Autores")]
    public class Autor
    {
        [Key]
        public int IdAutor { get; set; }

        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Apellidos { get; set; }

        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Nombres { get; set; }

        public Genero Sexo { get; set; }        /* Desplegable [Masculino|Femenino] */

        [Display(Name = "Fecha de nacimiento"), DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecNacimiento { get; set; }

        [Display(Name = "País de origen")]
        [Column(TypeName = "varchar"), StringLength(10), Required]
        public string Pais { get; set; }

        [NotMapped]
        [Display(Name = "Autor")]
        public string NombreCompleto { get { return Apellidos + " " + Nombres; } }

        /* Relaciones - Uno a Varios */

        public virtual ICollection<Libro> Libros { get; set; }
    }

    public enum Genero { Masculino, Femenino }
}