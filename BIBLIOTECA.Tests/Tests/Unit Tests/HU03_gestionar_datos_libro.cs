using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Models;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;

namespace BIBLIOTECA.Controllers.Tests
{
    [TestClass()]

    public class HU03
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        [TestMethod()]

        public void HU03_CA01_1_verificar_libro_fue_insertado_correctamente()
        {
            //Arrange
            string esperado = "LIBRO REGISTRADO CORRECTAMENTE";
            var actual = "";
            Libro libroTest = new Libro()
            {
                Titulo = "Libro test 1",
                Idioma = "Español",
                ISBN = "976655",
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
            //ACT
            try
            {
                db.Libros.Add(libroTest);
                db.SaveChanges();
                actual = "LIBRO REGISTRADO CORRECTAMENTE";               
            }
            catch (Exception e)
            {
                actual = "NO SE PUDO COMPLETAR CON EL REGISTRO DEL LIBRO";
            }
            
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_CA01_2_verificar_libro_no_fue_insertado_correctamente()
        {
            //Arrange
            string esperado = "NO SE PUDO COMPLETAR CON EL REGISTRO DEL LIBRO";
            var actual = "";            
            //ACT
            try
            {
                Libro libroTest1 = new Libro()
                {
                    Titulo = "Libro test 1",
                    Idioma = "Español",
                    ISBN = "976655",
                    IdAutor = 2,
                    //enviamos una categoría inexistente para que falle el registro
                    IdCategoria = 1776,
                    IdEditorial = 1,
                    NumEdicion = "10",
                    Anio = "2003",
                    NumPaginas = 1400,
                    Estado = "Bueno",
                    Caratula = "9543364.png",
                    Disponibilidad = true
                };
                db.Libros.Add(libroTest1);
                db.SaveChanges();                
                actual = "LIBRO REGISTRADO CORRECTAMENTE";
            }
            catch (Exception e)
            {
                actual = "NO SE PUDO COMPLETAR CON EL REGISTRO DEL LIBRO";
            }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_CA02_1_verificar_si_libro_esta_listo_para_ser_modificado()
        {
            //ASSIGN
            //BUSCAMOS EL LIBRO INSERTADO EN EL TEST ANTERIOR
            Libro buscarLibro = db.Libros.Where(l => l.Titulo == "Libro test 1").FirstOrDefault();
            int idLibro = buscarLibro.IdLibro;
            string esperado = "EL LIBRO ESTÁ LISTO PARA SER MODIFICADO";
            var actual = "";
            //ACT
            Libro libro = db.Libros.Find(idLibro);
            actual = (libro == null) ? "NO SE PUEDE MODIFICAR UN LIBRO NO REGISTRADO": esperado;            
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_CA02_2_verificar_si_libro_no_esta_listo_para_ser_modificado()
        {

            //ASSIGN
            //enviamos un id inexistente para hacer fallar el test unitario
            int idLibro = 1400;
            string esperado = "NO SE PUEDE MODIFICAR UN LIBRO NO REGISTRADO";
            var actual = "";
            //ACT
            Libro libro = db.Libros.Find(idLibro);
            actual = (libro == null) ? esperado : "EL LIBRO ESTÁ LISTO PARA SER MODIFICADO";
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_CA03_1_verificar_que_libro_fue_modificado_correctamente()
        {
            //ASSIGN
            //LLAMAMOS EL LIBRO INSERTADO EN EL TEST ANTERIOR
            var idLibro = 10;
            string esperado = "EL LIBRO FUE MODIFICADO CORRECTAMENTE";
            var actual = ""; 
            //Act
            try
            {
                Libro libroEditadoTest1 = new Libro()
                {        
                    IdLibro =idLibro,
                    ISBN = "976655",
                    Titulo = "Libro test modificado",
                    Idioma = "Español",
                    NumEdicion = "10",
                    Anio = "2003",
                    NumPaginas = 1400,
                    Estado = "Bueno",
                    Disponibilidad = true,
                    Caratula = "9543364.png",
                    IdAutor = 2,
                    IdCategoria = 3,
                    IdEditorial = 1
                };
                db.Entry(libroEditadoTest1).State = EntityState.Modified;
                db.SaveChanges();
                actual = "EL LIBRO FUE MODIFICADO CORRECTAMENTE";
            }
            catch (Exception e)
            {
                actual = "EL LIBRO NO FUE MODIFICADO CORRECTAMENTE" + e;
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU03_CA03_2_verificar_que_libro_no_fue_modificado_correctamente()
        {
            //Arrange
            //ENVIAMOS UN ID DE LIBRO INEXISTENTE PARA SUPERAR EL TEST UNITARIO
            int idfalso = 1222;
            string esperado = "EL LIBRO NO FUE MODIFICADO CORRECTAMENTE";
            var actual = "";
            Libro libroEditadoTest = new Libro()
            {
                //Enviamos un id Inexistente
                IdLibro = idfalso,
                Titulo = "Libro test modificado",
                Idioma = "Español",
                ISBN = "976655",
                IdAutor = 2,
                IdCategoria = 3,
                IdEditorial = 1,
                NumEdicion = "10",
                Anio = "2003",
                NumPaginas = 1400,
                Estado = "Bueno",
                Caratula = "9543364.png",
                Disponibilidad = true
            };
            //Act
            try
            {
                db.Entry(libroEditadoTest).State = EntityState.Modified;
                db.SaveChanges();
                actual = "EL LIBRO FUE MODIFICADO CORRECTAMENTE";
            }
            catch (Exception e)
            {
                actual = "EL LIBRO NO FUE MODIFICADO CORRECTAMENTE";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU03_CA04_1_verificar_si_libro_esta_listo_para_ser_eliminado()
        {
            //ASSIGN            
            //BUSCAMOS EL LIBRO MODIFICADO EN EL TEST ANTERIOR            
            int idLibro = 10;
            string esperado = "¿ESTÁ SEGURO QUE DESEA ELIMINAR ESTE LIBRO?";
            var actual = "";
            //ACT
            Libro libro = db.Libros.Find(idLibro);
            actual = (libro == null) ? "NO SE PUEDE ELIMINAR UN LIBRO NO REGISTRADO": esperado;
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU03_CA04_2_verificar_si_libro_no_esta_listo_para_ser_eliminado()
        {
            //ASSIGN
            //ENVIAMOS UN ID DE LIBRO INEXISTENTE PARA SUPERAR EL TEST UNITARIO
            int idLibro = 1400;
            string esperado = "NO SE PUEDE ELIMINAR UN LIBRO NO REGISTRADO";
            var actual = "";
            //ACT
            Libro libro = db.Libros.Find(idLibro);
            actual = (libro == null) ? esperado :"¿ESTÁ SEGURO QUE DESEA ELIMINAR ESTE LIBRO?";
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU03_CA05_verificar_eliminacion_de_libro()
        {
            //ASSIGN   
            int idLibro = 10;
            string esperado = "LIBRO ELIMINADO CORRECTAMENTE";
            var actual = "";
            //ACT
            Libro libro = db.Libros.Find(idLibro);
            try
            {
                db.Libros.Remove(libro);
                db.SaveChanges();
                actual = "LIBRO ELIMINADO CORRECTAMENTE";
            }
            catch (Exception e) { actual = "Error al intentar Eliminar libro"; }

            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
    }
}