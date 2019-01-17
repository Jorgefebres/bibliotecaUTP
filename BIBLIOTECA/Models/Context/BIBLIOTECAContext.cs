using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models
{
    public class BIBLIOTECAContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public BIBLIOTECAContext() : base("name=BIBLIOTECAContext")
        {
        }
        //Configuracion de Borrado en cascada (deshabilitado)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            // modelBuilder.Entity<Persona>().Property(x => x.Dni).IsUnicode(false);
        }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Autor> Autores { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Categoria> Categorias { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Editorial> Editoriales { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Persona> Personas { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Prestamo> Prestamos { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.PrestamoDetalle> PrestamoDetalles { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Reserva> Reservas { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.ReservaDetalle> ReservaDetalles { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Devolucion> Devoluciones { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Libro> Libros { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Rol> Roles { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Accion> Acciones { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.AccionRol> AccionRoles { get; set; }

        public System.Data.Entity.DbSet<BIBLIOTECA.Models.Sancion> Sancions { get; set; }
    }
}
