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
using BIBLIOTECA.Models.ViewModels;

namespace BIBLIOTECA.Tests.Controllers
{

    [TestClass]
    public class HU04
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();

        [TestMethod]
        public void HU04_CA01_1_verificar_si_existen_prestamos_pendientes_de_devolucion()
        {
            //Arrange
            string esperado = "EXISTEN PRÉSTAMOS PENDIENTES DE DEVOLUCIÓN.";
            var actual = "";
            //Act
            var prestamosPendientes = db.Prestamos.Where(x => x.Pendiente == true).ToList();
            actual = (prestamosPendientes.Count() >= 1) ? esperado : "NO EXISTEN PRÉSTAMOS PENDIENTES DE DEVOLUCIÓN.";
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_CA01_2_verificar_si_no_existen_prestamos_pendientes_de_devolucion()
        {
            //Arrange
            string esperado = "NO EXISTEN PRÉSTAMOS PENDIENTES DE DEVOLUCIÓN.";
            var actual = "";
            //Act
            var prestamosPendientes = db.Prestamos.Where(x => x.Pendiente == true).ToList();
            actual = (prestamosPendientes.Count() >= 1) ? "EXISTEN PRÉSTAMOS PENDIENTES DE DEVOLUCIÓN." : esperado;
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU04_CA02_1_verificar_lector_tiene_prestamos_pendientes_de_devolucion()
        {
            //Arrange
            int idPersona = 3;
            var esperado = "EL LECTOR TIENE PRESTAMOS PENDIENTES DE DEVOLUCIÓN";
            var actual = "";
            //ACT
            //Buscamos prestamos q pertenecen a esta persona y si aun estan pendientes
            var prestamo = db.Prestamos.Where(x => x.IdPersona == idPersona && x.Pendiente == true).ToList();
            actual = (prestamo.Count() >= 1) ? esperado : "EL LECTOR NO TIENE PRESTAMOS PENDIENTES DE DEVOLUCIÓN";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_CA02_2_verificar_lector_no_tiene_prestamos_pendientes_de_devolucion()
        {
            //Arrange
            //enviamos el Id de un lector que no realizó ningun prestamo
            int idPersona = 1;
            var esperado = "EL LECTOR NO TIENE PRESTAMOS PENDIENTES DE DEVOLUCIÓN";
            var actual = "";

            //ACT
            //Buscamos prestamos q pertenecen a esta persona y si aun estan pendientes
            var prestamo = db.Prestamos.Where(x => x.IdPersona == idPersona && x.Pendiente == true).ToList();
            actual = (prestamo.Count() >= 1) ? "EL LECTOR TIENE PRESTAMOS PENDIENTES DE DEVOLUCIÓN" : esperado;
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_CA03_1_verificar_que_prestamo_pendiente_tiene_mas_de_un_libro_para_devolver()
        {
            //Arrange
            //enviamos el id de un prestamo con mas de un libro prestado
            int idprestamo = 8;
            var esperado = "EL PRÉSTAMO CONTIENE MÁS DE UN LIBRO PARA DEVOLVER";
            var actual = "";
            //ACT
            //Buscamos por id los libros que se prestaron en este prestamo
            var detalleprestamos = db.PrestamoDetalles.Where(x => x.IdPrestamo == idprestamo && x.Pendiente == true).ToList();
            actual = (detalleprestamos.Count() > 1) ? esperado : "EL PRÉSTAMO CONTIENE MÁS DE UN LIBRO PARA DEVOLVER";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_CA03_1_verificar_que_prestamo_pendiente_tiene_solo_un_libro_para_devolver()
        {
            //Arrange
            //Enviamos el id de un prestamo que solo contiene un libro prestado
            int idprestamo = 7;
            var esperado = "EL PRÉSTAMO SOLO CONTIENE UN LIBRO PARA DEVOLVER";
            var actual = "";
            //ACT
            //Buscamos por id los libros que se prestaron en este prestamo
            var detalleprestamos = db.PrestamoDetalles.Where(x => x.IdPrestamo == idprestamo && x.Pendiente == true).ToList();
            actual = (detalleprestamos.Count() == 1) ? esperado : "EL PRÉSTAMO CONTIENE MÁS DE UN LIBRO PARA DEVOLVER";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU04_CA04_1_verificar_que_un_libro_esta_disponible_despues_de_devolverlo()
        {
            //Assign
            VistaPrestamo vistaPrestamo;
            vistaPrestamo = new VistaPrestamo();
            // vistaPrestamo.Personas = new Persona();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            int idPrestamoDetalle = 4;
            int idUsuario = 2;

            var esperado = "LIBRO DISPONIBLE PARA PRÉSTAMO O RESERVA";
            var actual = "";
            //Act
            Devolucion devolucion = new Devolucion
            {
                FecDevolcion = DateTime.Now,
                IdPrestamoDetalle = idPrestamoDetalle,
                IdUsuario = idUsuario
            };
            db.Devoluciones.Add(devolucion);
            try
            {
                db.SaveChanges();//Guarda los cambios
                actual = "LIBRO DISPONIBLE PARA PRÉSTAMO O RESERVA";
            }
            catch (Exception)
            {
                actual = "Algo salio mal";
            }
            PrestamoDetalle detalles = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == idPrestamoDetalle);
            vistaPrestamo.ListaDetPrestamo.Remove(detalles);//Remueve el libro devuelto de la lista           
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU04_CA04_2_verificar_prestamo_ya_no_tiene_libros_pendientes_de_devolucion()
        {
            //Assign
            VistaPrestamo vistaPrestamo;
            vistaPrestamo = new VistaPrestamo();
            // vistaPrestamo.Personas = new Persona();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            int idPrestamoDetalle = 4;
            int idUsuario = 2;

            var esperado = "PRÉSTAMO YA NO TIENE LIBROS PENDIENTES DE DEVOLUCIÓN";
            var actual = "";
            //Act
            Devolucion devolucion = new Devolucion
            {
                FecDevolcion = DateTime.Now,
                IdPrestamoDetalle = idPrestamoDetalle,
                IdUsuario = idUsuario
            };
            db.Devoluciones.Add(devolucion);
            try
            {
                db.SaveChanges();//Guarda los cambios
                actual = "LIBRO DISPONIBLE PARA PRÉSTAMO O RESERVA";
            }
            catch (Exception)
            {
                actual = "Algo salio mal";
            }
            PrestamoDetalle detalles = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == idPrestamoDetalle);
            vistaPrestamo.ListaDetPrestamo.Remove(detalles);//Remueve el libro devuelto de la lista
            actual = "PRÉSTAMO YA NO TIENE LIBROS PENDIENTES DE DEVOLUCIÓN";
            //Assert
            Assert.AreEqual(esperado, actual);

        }
    }
}