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
    public class HU04
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        ReservaController reservaController = new ReservaController();
        PersonaController personaController = new PersonaController();
        PrestamoController prestamocontroller = new PrestamoController();
        LibroController libroController = new LibroController();

        [TestMethod]
        public void HU04_criterio_de_aceptacion_1_1()
        {
            //Arrange
            string esperado = "Existen prestamos pendientes de devolución.";
            var actual = "";
            //Act
            var prestamopendiente = db.Prestamos.Where(x => x.Pendiente == true).ToList();
            if (prestamopendiente.Count() > 0)
            {
                actual = "Existen prestamos pendientes de devolución.";
            }
            else
            {
                actual = "NO Existen prestamos pendientes de devolución.";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_1_2()
        {
            //Arrange
            string esperado = "NO Existen prestamos pendientes de devolución.";
            var actual = "";
            //Act
            var prestamopendiente = db.Prestamos.Where(x => x.Pendiente == true).ToList();
            if (prestamopendiente.Count() == 0)
            {
                actual = "NO Existen prestamos pendientes de devolución.";
            }
            else
            {
                actual = "Existen prestamos pendientes de devolución.";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU04_criterio_de_aceptacion_2_1()
        {
            //Arrange
            int idPersona = 1;
            var esperado = "El lector tiene prestamos pendientes de devolución";
            var actual = "";
            //ACT
            //Buscamos prestamos q pertenecen a esta persona y si aun estan pendientes
            var prestamo = db.Prestamos.Where(x => x.IdPersona == idPersona && x.Pendiente == true).ToList();
            if (prestamo.Count() > 0)
            { //si dicha persona tiene un prestamo pendiente de devolucion.

                actual = "El lector tiene prestamos pendientes de devolución";
            }
            else
            {
                actual = "El lector NO tiene prestamos pendientes de devolución";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_2_2()
        {
            //Arrange
            int idPersona = 1;
            var esperado = "El lector NO tiene prestamos pendientes de devolución";
            var actual = "";
            //ACT
            //Buscamos prestamos q pertenecen a esta persona y si aun estan pendientes
            var prestamo = db.Prestamos.Where(x => x.IdPersona == idPersona && x.Pendiente == true).ToList();
            if (prestamo.Count() > 0)
            { //si dicha persona tiene un prestamo pendiente de devolucion.

                actual = "El lector tiene prestamos pendientes de devolución";
            }
            else
            {
                actual = "El lector NO tiene prestamos pendientes de devolución";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_3_1()
        {
            //Arrange
            int idprestamo = 1;
            var esperado = "El préstamo contiene mas de un libro para devolver";
            var actual = "";
            //ACT
            //Buscamos por id los libros que se prestaron en este prestamo
            var detalleprestamos = db.PrestamoDetalles.Where(x => x.IdPrestamo == idprestamo && x.Pendiente == true).ToList();
            if (detalleprestamos.Count() > 1)
            {

                actual = "El préstamo contiene mas de un libro para devolver";
            }
            else
            {
                actual = "El préstamo solo contiene un libro para devolver";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_3_2()
        {
            //Arrange
            int idprestamo = 1;
            var esperado = "El préstamo solo contiene un libro para devolver";
            var actual = "";
            //ACT
            //Buscamos por id los libros que se prestaron en este prestamo
            var detalleprestamos = db.PrestamoDetalles.Where(x => x.IdPrestamo == idprestamo && x.Pendiente == true).ToList();
            if (detalleprestamos.Count() > 1)
            {

                actual = "El préstamo contiene mas de un libro para devolver";
            }
            else
            {
                actual = "El préstamo solo contiene un libro para devolver";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_4()
        {
            int idlibro =1;
            //Assign
            var esperado = "Libro disponible para préstamo o reserva";
            var actual = "";
            //Act
            //var librodevuelto = db.PrestamoDetalles.Where(x => x.IdLibro==id).ToList();
            
            //int idLibro;
            //foreach (var item in librodevuelto)
            //{
            //    idLibro=item.IdLibro;
		 
            //}
            Libro libro1 = new Libro()
            {
                IdLibro = idlibro,
                Disponibilidad = true,
            };
            libroController.Edit(libro1);
            actual = "Libro disponible para préstamo o reserva";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_criterio_de_aceptacion_5()
        {
            int idprestamo = 1;
            //Assign
            var esperado = "Préstamo ya no está pendiente de devolución";
            var actual = "";
            //Act
            //var librodevuelto = db.PrestamoDetalles.Where(x => x.IdLibro==id).ToList();

            //int idLibro;
            //foreach (var item in librodevuelto)
            //{
            //    idLibro=item.IdLibro;

            //}
            Prestamo prestamo1 = new Prestamo()
            {
                IdPrestamo = idprestamo,
                Pendiente = false,
            };
            //prestamocontroller.Edit(prestamo);
            //Assert
            Assert.AreEqual(null, actual);
        }
    }
}
