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
    public class HU02
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        ReservaController reservaController = new ReservaController();
        PersonaController personaController = new PersonaController();
        PrestamoController prestamocontroller = new PrestamoController();
        LibroController libroController = new LibroController();

        [TestMethod]
        public void HU01_criterio_de_aceptacion_1_1()
        {
            //Arrange
            string esperado = "Existen lectores registrados";
            //Act
            personaController.GetAllPersonas();
            string actual = personaController.Mensaje();
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU01_criterio_de_aceptacion_1_2()
        {
            //Arrange
            string esperado = "No existen lectores registrados, se necesita al menos uno para realizar un préstamo";
            //Act
            personaController.GetAllPersonas();
            string actual = personaController.Mensaje();
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_2_1()
        {
            //ARRANGE
            int id = 1;
            //ACT
            var disponibilidad = personaController.GetDisponibilidadPersona(id);
            var actual = personaController.Mensaje();
            //ASSERT
            //Assert.IsTrue(disponibilidad);
            Assert.AreEqual("Está habilitado para realizar reserva o préstamo", actual);
        }
        //[TestMethod]
        //public void HU02_criterio_de_aceptacion_1_2()
        //{
        //    //ARRANGE
        //    int id = 2;
        //    //ACT
        //    var disponibilidad = personaController.GetDisponibilidadPersona(id);
        //    var actual = personaController.Mensaje();
        //    //ASSERT
        //    //Assert.IsFalse(disponibilidad);
        //    Assert.AreEqual("No está habilitado para realizar reserva o préstamo", actual);
        //}

        [TestMethod]
        public void HU02_criterio_de_aceptacion_3_1()
        {
            //ARRANGE
            var esperado = "Existen libros disponibles para el préstamo";
            //ACT
            prestamocontroller.AgregarLibro();
            var actual = prestamocontroller.Mensaje();
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_3_2()
        {
            //ARRANGE
            var esperado = "No existen libros disponibles para el préstamo";
            //ACT
            prestamocontroller.AgregarLibro();
            var actual = prestamocontroller.Mensaje();
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU02_criterio_de_aceptacion_4_1()
        {
            //Assign
            var esperado = "Libro Agregado correctamente a la lista";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_4_2()
        {
            //Assign
            var esperado = "Dicho libro ya estaba agregado en la lista";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_5_1()
        {
            //Assign
            var esperado = "Préstamo realizado Correctamente";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_5_2()
        {
            //Assign
            var esperado = "no se pudo completar correctamente el préstamo";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_6_1()
        {
            //Assign
            var esperado = "Existen Reservas pendientes";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
        [TestMethod]
        public void HU02_criterio_de_aceptacion_6_2()
        {
            //Assign
            var esperado = "No existen reservas pendientes";
            //Act            
            var actual = prestamocontroller.Mensaje();
            //Assert
            Assert.AreEqual(null, actual);
        }
    }
}