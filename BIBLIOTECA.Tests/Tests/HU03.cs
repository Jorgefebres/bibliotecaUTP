using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Controllers;
using BIBLIOTECA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers.Tests
{

    [TestClass()]
    public class HU03
    {
        LibroController libroController = new LibroController();
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        //[TestMethod()]
        //public void HU03_criterio_de_aceptacion_1_1()
        //{
        //    //Arrange
        //    Libro libro1 = new Libro()
        //    {                
        //        Titulo = "Libro2",
        //        Idioma = "Español",
        //        NumEdicion = "1",
        //        Anio = "2002",
        //        Estado = "Bueno",
        //        Disponibilidad = false,
        //        NumPaginas = 2000,
        //        IdAutor = 1,
        //        IdCategoria = 1,
        //        IdEditorial = 1
        //    };
        //    string esperado = "Libro registrado correctamente";
        //    //Act
        //    libroController.Create(libro1, null);
        //    string actual = libroController.Mensaje();
        //    //Assert
        //    Assert.AreEqual(esperado, actual);
        //}
        [TestMethod()]
        public void HU03_criterio_de_aceptacion_1_2()
        {
            //Arrange
            Libro libro1 = new Libro()
           {
               ISBN = "200",
               Titulo = "Libro3",
               Idioma = "Español",
               NumEdicion = "1",
               Anio = "2002",
               Estado = "Bueno",
               Disponibilidad = false,
               Caratula = null,
               NumPaginas = 2000,
               IdAutor = 1,
               //No enviaremos categoría
               //IdCategoria = 1,
               IdEditorial = 1
           };
            string esperado = "No se pudo completar con el registro del libro";
            //Act
            libroController.Create(libro1, null);
            string actual = libroController.Mensaje();
            //Assert
            Assert.AreEqual(esperado, actual);
        }


        [TestMethod()]
        public void HU03_criterio_de_aceptacion_2_1()
        {
            //enviamos un id existente
            int id = 1;
            string esperado = "El libro está listo para ser modificado";
            libroController.Edit(id);
            string actual = libroController.Mensaje();
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_criterio_de_aceptacion_2_2()
        {
            //Enviamos un ID Inexistente
            int id = 0;
            string esperado = "No se puede Modificar un libro no registrado";
            libroController.Edit(id);
            string actual = libroController.Mensaje();
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU03_criterio_de_aceptacion_3_1()
        {
            //Arrange
            Libro libro1 = new Libro()
            {
                IdLibro = 1,
                ISBN = "200",
                Titulo = "Libro4",
                Idioma = "Español",
                NumEdicion = "1",
                Anio = "2002",
                Estado = "Bueno",
                Disponibilidad = false,
                Caratula = null,
                NumPaginas = 2000,
                IdAutor = 1,
                IdCategoria = 1,
                IdEditorial = 1
            };
            string esperado = "El Libro fue modificado correctamente";
            //Act
            libroController.Edit(libro1);
            string actual = libroController.Mensaje();
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_criterio_de_aceptacion_3_2()
        {
            Libro libro1 = new Libro()
             {
                 IdLibro=1000,
                 ISBN = "200",
                 Titulo = "Libro4",
                 Idioma = "Español",
                 NumEdicion = "1",
                 Anio = "2002",
                 Estado = "Bueno",
                 Disponibilidad = false,
                 Caratula = null,
                 NumPaginas = 2000,
                 IdAutor = 1,
                 IdCategoria = 1,
                 IdEditorial = 1
             };
            string esperado = "El Libro no fue modificado correctamente";
            libroController.Edit(libro1);
            string actual = libroController.Mensaje();
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU03_criterio_de_aceptacion_4_1()
        {
            int id = 1;
            string esperado = "¿Está seguro que desea eliminar este libro?";
            libroController.Delete(id);
            string actual = libroController.Mensaje();
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU03_criterio_de_aceptacion_4_2()
        {
            string esperado = "No se puede Eliminar un libro no registrado";
            libroController.Delete(0);
            string actual = libroController.Mensaje();
            Assert.AreEqual(esperado, actual);
        }
    }
}