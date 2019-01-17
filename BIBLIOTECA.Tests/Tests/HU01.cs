using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Controllers;
using System.Collections.Generic;
using BIBLIOTECA.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace BIBLIOTECA.Tests.Controllers
{
    [TestClass]

    public class HU01
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        LibroController libroController = new LibroController();
        //prueba unitaria * Poder saber que libros existen en el catálogo.
        [TestMethod]
        public void HU01_criterio_de_aceptacion_1_1()
        {
            //Arrange
            var libros = db.Libros.Include(a => a.Autores).Include(c => c.Categorias).Include(e => e.Editoriales);
            //Act
            Libro libro1 = db.Libros.Find(1);
            //Assetr
            CollectionAssert.Contains(libros.ToList(), libro1);

        }
        //prueba unitaria * Poder saber que libros existen en el catálogo (En Caso que no existan libros registrados).
        //[TestMethod]
        //public void HU01_criterio_de_aceptacion_1_2()
        //{
        //    //Arrange
        //    string esperado = "Aun no se ha registrado ningún libro.";
        //    //Act
        //    libroController.Index();
        //    string actual = libroController.Mensaje();
        //    //Assert
        //    Assert.AreEqual(esperado, actual);
        //}
        //prueba unitaria * Ver los datos de libro de forma detallada. 
        [TestMethod]
        public void HU01_criterio_de_aceptacion_2()
        {
            //Arrange
            var libros = db.Libros.Include(a => a.Autores).Include(c => c.Categorias).Include(e => e.Editoriales);
            int id = 1;
            //Act
            Libro libro1 = db.Libros.Find(id);
            //Assert
            CollectionAssert.Contains(libros.ToList(), libro1);
        }
        //prueba unitaria * Saber si el libro que deseo prestarme está disponible.
        [TestMethod]
        public void HU01_criterio_de_aceptacion_3_1()
        {
            //ARRANGE
            int id = 1;
            //ACT
            var disponibilidad = libroController.GetDisponibilidadLibro(id);
            var actual = libroController.Mensaje();
            //ASSERT
            //Assert.IsTrue(disponibilidad);
            Assert.AreEqual("El Libro está disponible para reserva o préstamo", actual);
        }
        [TestMethod]
        public void HU01_criterio_de_aceptacion_3_2()
        {
            //ARRANGE
            int id = 2;
            //ACT
            var disponibilidad = libroController.GetDisponibilidadLibro(id);
            var actual = libroController.Mensaje();
            //ASSERT
            //Assert.IsFalse(disponibilidad);
            Assert.AreEqual("El Libro NO está disponible para reserva o préstamo", actual);
        }
    }
}
