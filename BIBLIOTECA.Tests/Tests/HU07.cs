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
    public class HU07
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        ReservaController reservaController = new ReservaController();
        PersonaController personaController = new PersonaController();

        [TestMethod]
        public void HU07_criterio_de_aceptacion_1_1()
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
        [TestMethod]
        public void HU07_criterio_de_aceptacion_1_2()
        {
            //ARRANGE
            int id = 3;
            //ACT
            var disponibilidad = personaController.GetDisponibilidadPersona(id);
            var actual = personaController.Mensaje();
            //ASSERT
            //Assert.IsFalse(disponibilidad);
            Assert.AreEqual("No está habilitado para realizar reserva o préstamo", actual);
        }
    }
}