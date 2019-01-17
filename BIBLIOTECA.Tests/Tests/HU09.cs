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
    public class HU09
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        ReservaController reservaController = new ReservaController();
        PersonaController personaController = new PersonaController();
        PrestamoController prestamocontroller = new PrestamoController();
        LibroController libroController = new LibroController();

        [TestMethod]
        public void HU09_criterio_de_aceptacion_1()
        {
            //Arrange
            string esperado = "Dicho Lector se encuentra registrado";
            var actual = "";
            string dnibuscado = "4529";

            //Act
            List<string> dni;
            dni = db.Personas.Where(x => x.Dni.StartsWith(dnibuscado)).Select(e => e.Dni).Distinct().ToList();
            if (dni.Count() > 0)
            {
                actual = "Dicho Lector se encuentra registrado";
            }
            else
            {
                actual = "Dicho Lector NO se encuentra registrado";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU09_criterio_de_aceptacion_2()
        {
            //Arrange
            string esperado = "Dicho Libro se encuentra registrado";
            var actual = "";
            string titulobuscado = "Kafka";

            //Act
            List<string> libro;
            libro = db.Libros.Where(x => x.Titulo.StartsWith(titulobuscado)).Select(e => e.Titulo).Distinct().ToList();
            if (libro.Count() > 0)
            {
                actual = "Dicho Libro se encuentra registrado";
            }
            else
            {
                actual = "Dicho Libro NO se encuentra registrado";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }

    }
}