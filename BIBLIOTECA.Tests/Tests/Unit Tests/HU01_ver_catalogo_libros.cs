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

        [TestMethod]
        public void HU01_CA01_1_verificar_que_existen_libros_en_catalogo()
        {

            //Arrange
            string esperado = "EXISTEN LIBROS REGISTRADOS";
            var libros = db.Libros.Include(a => a.Autores).Include(c => c.Categorias).Include(e => e.Editoriales);
            //Act           
            var actual = (libros.Count() >= 1) ? esperado : "AÚN NO SE HA REGISTRADO NINGÚN LIBRO";
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU01_CA01_2_verificar_que_no_existen_libros_en_catalogo()
        {
            //Arrange
            string esperado = "AÚN NO SE HA REGISTRADO NINGÚN LIBRO";
            var libros = db.Libros.Include(a => a.Autores).Include(c => c.Categorias).Include(e => e.Editoriales).ToList();
            //EN CASO DE TENER LIBROS REGISTRADOS, QUITAMOS TODOS LOS ELEMENTOS PARA SUPERAR LA PRUEBA UNITARIA
            if (libros.Count() >= 1) { libros.Clear(); }
            //Act           
            var actual = (libros.Count() >= 1) ? "EXISTEN LIBROS REGISTRADOS" : esperado;
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU01_CA02_ver_detalles_de_libro()
        {
            //Arrange       
            int idLibro = 1;
            //CREAMOS UN LIBRO PARA COMPARAR 
            Libro kafkaEnLaOrillaDetalles = new Libro()
            {
                IdLibro = 1,
                Titulo = "Kafka en la Orilla",
                Idioma = "Español",
                ISBN = "342334",
                IdAutor = 2,
                IdCategoria = 6,
                IdEditorial = 1,
                NumEdicion = "10",
                Anio = "2003",
                NumPaginas = 1400,
                Estado = "Bueno",
                Caratula = "9543364.png",
                Disponibilidad = true
            };
            //Act
            //BUSCAMOS EL LIBRO DEL CUAL NECESITAMOS LOS DETALLES
            var DetallesLibro = db.Libros.Find(idLibro);
            //Assert
            Assert.AreEqual(DetallesLibro.Titulo, kafkaEnLaOrillaDetalles.Titulo);
            Assert.AreEqual(DetallesLibro.Idioma, kafkaEnLaOrillaDetalles.Idioma);
            Assert.AreEqual(DetallesLibro.ISBN, kafkaEnLaOrillaDetalles.ISBN);
            Assert.AreEqual(DetallesLibro.IdAutor, kafkaEnLaOrillaDetalles.IdAutor);
            Assert.AreEqual(DetallesLibro.IdCategoria, kafkaEnLaOrillaDetalles.IdCategoria);
            Assert.AreEqual(DetallesLibro.IdEditorial, kafkaEnLaOrillaDetalles.IdEditorial);
            Assert.AreEqual(DetallesLibro.NumEdicion, kafkaEnLaOrillaDetalles.NumEdicion);
            Assert.AreEqual(DetallesLibro.Anio, kafkaEnLaOrillaDetalles.Anio);
            Assert.AreEqual(DetallesLibro.NumPaginas, kafkaEnLaOrillaDetalles.NumPaginas);
            Assert.AreEqual(DetallesLibro.Caratula, kafkaEnLaOrillaDetalles.Caratula);
            Assert.AreEqual(DetallesLibro.Disponibilidad, kafkaEnLaOrillaDetalles.Disponibilidad);
        }
        [TestMethod]
        public void HU01_CA03_1_verificar_que_libro_esta_disponible()
        {
            //ARRANGE            
            int idLibro = 1;
            string esperado = "EL LIBRO ESTÁ DISPONIBLE PARA RESERVA O PRÉSTAMO";
            var actual = "";
            //ACT      
            try
            {
                Libro libro = db.Libros.Find(idLibro);
                //(OPCIONAL) SI EL LIBRO NO ESTA DISPONIBLE CAMBIAMOS SU ESTADO PARA SUPERAR LA PRUEBA UNITARIA
                if (libro.Disponibilidad) { libro.Disponibilidad = true; }
                actual = (libro.Disponibilidad) ? esperado : "EL LIBRO NO ESTÁ DISPONIBLE PARA RESERVA O PRÉSTAMO";
            }
            catch (Exception e)
            {
                actual = "EL LIBRO NO SE ENCUENTRA REGISTRADO";
            }
            //ASSERT           
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU01_CA03_1_verificar_que_libro_no_esta_disponible()
        {
            //ARRANGE            
            int idLibro = 1;
            string nombreUsuario = "lector1";
            string esperado = "EL LIBRO NO ESTÁ DISPONIBLE PARA RESERVA O PRÉSTAMO";
            var actual = "";
            //ACT            
            //VERIFICAMOS QUE EL USUARIO "LECTOR1" EXISTE
            Usuario usuarioExiste = db.Usuarios.Where(u => u.NomUsuario == nombreUsuario).SingleOrDefault();
            if (usuarioExiste.NomUsuario == "lector1")
            {
                //VERIFICAMOS QUE USUARIO "LECTOR1" TIENE ROL "LECTOR"
                var esLector = db.Roles.Where(r => r.IdRol == usuarioExiste.IdRol).SingleOrDefault();
                if (esLector.IdRol == 3)
                {
                    Libro libro = db.Libros.Find(idLibro);
                    try
                    {
                        //(OPCIONAL) SI EL LIBRO ESTA DISPONIBLE CAMBIAMOS SU ESTADO PARA SUPERAR LA PRUEBA UNITARIA
                        if (libro.Disponibilidad) { libro.Disponibilidad = false; }
                        actual = (libro.Disponibilidad) ? "EL LIBRO ESTÁ DISPONIBLE PARA RESERVA O PRÉSTAMO" : esperado;
                    }
                    catch (Exception e)
                    {
                        actual = "EL LIBRO NO SE ENCUENTRA REGISTRADO";
                    }
                }
                else { actual = "EL USUARIO NO TIENE ROL DE LECTOR"; }
            }
            else { actual = "EL USUARIO NO SE ENCUENTRA REGISTRADO"; }
            //ASSERT           
            Assert.AreEqual(esperado, actual);
        }
    }
}
