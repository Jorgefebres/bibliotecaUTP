using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIBLIOTECA.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Display(Name = "Nombre de categoría")]
        [Column(TypeName = "varchar"), StringLength(50), Required]
        public string NomCategoria { get; set; }

        [Display(Name = "Descripción")]
        [Column(TypeName = "varchar"), StringLength(100), Required]
        public string Descripcion { get; set; }

        /* Relaciones - Uno a Varios*/

        public virtual ICollection<Libro> Libros { get; set; }
    }
}