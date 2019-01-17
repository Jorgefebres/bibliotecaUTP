using BIBLIOTECA.Models.Metodos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIBLIOTECA.Models
{
    [Table("Personas")]
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        [Display(Name = "Número de DNI")]
        [Column(TypeName ="varchar"), StringLength(8),Required,Index(IsUnique =true)]
        public string Dni { get; set; }

        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Apellidos { get; set; }

        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Nombres { get; set; }

        public Genero Sexo {  get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [Column(TypeName = "date"), DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FecNacimiento { get; set; }

        [Column(TypeName = "varchar"), StringLength(200), Required]
        public string Direccion { get; set; }

        [Display(Name ="Teléfono o celular")]
        [Column(TypeName = "varchar"), StringLength(12)]
        public string Telefono { get; set; }

        [Display(Name = "Correo electrónico")]
        [Column(TypeName = "varchar"), StringLength(50), Required, Index(IsUnique = true)]
        public string Email { get; set; }

        [NotMapped]
        public string ConEncriptada { get { return SHA1.Encode(Dni); } }

        [NotMapped]
        [Display(Name = "Nombre")]
        public string NombreCompleto { get { return Apellidos + " " + Nombres; } }


        /* Relaciones - Uno a Varios */
        public virtual ICollection<Usuario> Usuario { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}