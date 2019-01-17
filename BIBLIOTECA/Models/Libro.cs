using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("Libros")]
    public class Libro
    {
        [Key]
        public int IdLibro { get; set; }

        [Display(Name = "Título de libro")]
        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Titulo { get; set; }

        [Column(TypeName = "varchar"), StringLength(30), Required]
        public string Idioma { get; set; }

        [Display(Name = "Código ISBN")]
        [Column(TypeName = "varchar"), StringLength(20)]
        public string ISBN { get; set; }

        public int IdAutor { get; set; }        /* Foreign Key */

        public int IdCategoria { get; set; }    /* Foreign Key */

        public int IdEditorial { get; set; }    /* Foreign Key */

        [Display(Name = "Número de edición")]
        [Column(TypeName = "varchar"), StringLength(3), Required]
        public string NumEdicion { get; set; }

        [Display(Name = "Año de edición")]
        [Column(TypeName = "varchar"), StringLength(4), Required]
        public string Anio { get; set; }

        [Display(Name = "Número de páginas")]
        public int NumPaginas { get; set; }

        [Display(Name = "Estado actual del libro")]
        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Estado { get; set; }

        [Display(Name = "Carátula")]
        [Column(TypeName = "varchar"), StringLength(100)]
        public string Caratula { get; set; }

        [Display(Name = "¿Disponible?")]
        public bool Disponibilidad { get; set; }

        /* Relaciones - Uno a Uno */
        public virtual Autor Autores { get; set; }
        public virtual Categoria Categorias { get; set; }
        public virtual Editorial Editoriales { get; set; }


        /* Relaciones - Uno a Varios */
        public virtual ICollection<ReservaDetalle> ReservaDetalles { get; set; }
        public virtual ICollection<PrestamoDetalle> PrestamoDetalles { get; set; }
    }
}