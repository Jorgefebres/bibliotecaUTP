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
    public class HU06
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();

        [TestMethod]
        public void HU06_CA01_1_verificar_usuario_esta_habilitado_para_reserva()
        {
            //ARRANGE
            int idUsuario = 3;
            bool estaSancionado = false;
            var esperado = "ESTÁ HABILITADO PARA REALIZAR RESERVA O PRÉSTAMO";
            var actual = "";
            //ACT
            //CONSULTA PARA SABER SI ESTA SANCIONADO EL USUARIO
            List<Sancion> sanciones = db.Sancions.Where(x => x.Devoluciones.PrestamoDetalles.Prestamos.Personas.IdPersona == idUsuario).ToList();
            if (sanciones != null)
            {
                foreach (var item in sanciones)
                {
                    DateTime fechaInicio = item.FecInicio;
                    DateTime fechaFin = item.FecFin;
                    if (fechaInicio.Date <= DateTime.Now.Date && fechaFin.Date >= DateTime.Now.Date)
                    {
                        estaSancionado = true;
                    }
                }
                estaSancionado = false;
            }
            else { estaSancionado = false; }
            actual = (estaSancionado) ? "NO ESTÁ HABILITADO PARA REALIZAR RESERVA O PRÉSTAMO" : esperado;
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU06_CA01_2_verificar_usuario_no_esta_habilitado_para_reserva()
        {
            //ARRANGE
            int idUsuario = 3;
            bool estaSancionado = false;
            var esperado = "NO ESTÁ HABILITADO PARA REALIZAR RESERVA O PRÉSTAMO";
            var actual = "";
            //ACT
            //CONSULTA PARA SABER SI ESTA SANCIONADO EL USUARIO
            List<Sancion> sanciones = db.Sancions.Where(x => x.Devoluciones.PrestamoDetalles.Prestamos.Personas.IdPersona == idUsuario).ToList();
            if (sanciones != null)
            {
                foreach (var item in sanciones)
                {
                    DateTime fechaInicio = item.FecInicio;
                    DateTime fechaFin = item.FecFin;
                    if (fechaInicio.Date <= DateTime.Now.Date && fechaFin.Date >= DateTime.Now.Date)
                    {
                        estaSancionado = true;
                    }
                }
                estaSancionado = false;
            }
            else { estaSancionado = false; }
            actual = (estaSancionado) ? esperado : "ESTÁ HABILITADO PARA REALIZAR RESERVA O PRÉSTAMO";
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
    }
}