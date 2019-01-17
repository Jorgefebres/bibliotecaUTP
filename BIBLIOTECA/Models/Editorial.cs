using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    [Table("Editoriales")]
    public class Editorial
    {

        [Key]
        public int IdEditorial { get; set; }

        [Display(Name = "Nombre de editorial")]
        [Column(TypeName = "varchar"), StringLength(50), Required]
        public string NomEditorial { get; set; }

        [Display(Name = "País de origen")]
        [Column(TypeName = "varchar"), StringLength(50), Required]
        public string Pais { get; set; }

        [Column(TypeName = "varchar"), StringLength(50)]
        public string Ciudad { get; set; }

        [Display(Name = "Correo electrónico")]
        [Column(TypeName = "varchar"), StringLength(50), Index(IsUnique = true)]
        public string Email { get; set; }

        [Display(Name = "Sitio web")]
        [Column(TypeName = "varchar"), StringLength(100), Index(IsUnique = true)]
        public string Url { get; set; }

        /* Relaciones - Uno a Varios*/

        public virtual ICollection<Libro> Libros { get; set; }
    }
}