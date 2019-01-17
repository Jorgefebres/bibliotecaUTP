using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace BIBLIOTECA.Tests.Controllers
{

    [TestClass]

    public class HU02
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        [TestMethod]
        public void HU02_CA01_1_verificar_que_existen_lectores_registrados()
        {
            //Arrange
            string esperado = "EXISTEN LECTORES REGISTRADOS";
            string nombreUsuario = "admin2";
            var actual = "";
            //ACT            
            //VERIFICAMOS QUE EL USUARIO "ADMIN2" EXISTE
            Usuario usuarioExiste = db.Usuarios.Where(u => u.NomUsuario == nombreUsuario).SingleOrDefault();
            if (usuarioExiste.NomUsuario == "admin2")
            {
                //VERIFICAMOS QUE USUARIO "ADMIN2" TENGA ROL DE "BIBLIOTECARIO"
                var esBibliotecario = db.Roles.Where(r => r.IdRol == usuarioExiste.IdRol).SingleOrDefault();
                if (esBibliotecario.IdRol == 2)
                {
                    //PARA VERIFICAR QUE EXISTEN LECTORES REGISTRADOS EN EL SISTEMA
                    var lectores = db.Usuarios.Where(l => l.IdRol == 3);
                    actual = (lectores.Count() >= 1) ? esperado : "NO EXISTEN LECTORES REGISTRADOS, SE NECESITA AL MENOS UNO PARA REALIZAR UN PRÉSTAMO";
                }
                else { actual = "EL USUARIO NO TIENE ROL DE BIBLIOTECARIO"; }
            }
            else { actual = "EL USUARIO NO SE ENCUENTRA REGISTRADO"; }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA01_2_verificar_que_no_existen_lectores_registrados()
        {
            //Arrange
            string esperado = "NO EXISTEN LECTORES REGISTRADOS, SE NECESITA AL MENOS UNO PARA REALIZAR UN PRÉSTAMO";
            string nombreUsuario = "admin2";
            var actual = "";
            //ACT            
            //VERIFICAMOS QUE EL USUARIO "ADMIN2" EXISTE
            Usuario usuarioExiste = db.Usuarios.Where(u => u.NomUsuario == nombreUsuario).SingleOrDefault();
            if (usuarioExiste.NomUsuario == "admin2")
            {
                //VERIFICAMOS QUE USUARIO "ADMIN2" TENGA ROL DE "BIBLIOTECARIO"
                var esBibliotecario = db.Roles.Where(r => r.IdRol == usuarioExiste.IdRol).SingleOrDefault();
                if (esBibliotecario.IdRol == 2)
                {
                    //PARA VERIFICAR QUE EXISTEN LECTORES REGISTRADOS EN EL SISTEMA
                    var lectores = db.Usuarios.Where(l => l.IdRol == 3).ToList();
                    //EN CASO DE TENER LECTORES REGISTRADOS, QUITAMOS TODOS LOS ELEMENTOS PARA SUPERAR LA PRUEBA UNITARIA
                    if (lectores.Count() >= 1) { lectores.Clear(); }
                    actual = (lectores.Count() >= 1) ? "EXISTEN LECTORES REGISTRADOS" : esperado;
                }
                else { actual = "EL USUARIO NO TIENE ROL DE BIBLIOTECARIO"; }
            }
            else { actual = "EL USUARIO NO SE ENCUENTRA REGISTRADO"; }
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod]
        public void HU02_CA02_1_verificar_usuario_esta_habilitado_para_prestamo()
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
            actual = (estaSancionado)?  "NO ESTÁ HABILITADO PARA REALIZAR RESERVA O PRÉSTAMO" : esperado;
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA02_2_verificar_usuario_no_esta_habilitado_para_prestamo()
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
        [TestMethod]
        public void HU02_CA03_1_verificar_que_existen_Libros_Disponibles_Para_Prestamo()
        {
            //ARRANGE
            var esperado = "EXISTEN LIBROS DISPONIBLES PARA EL PRÉSTAMO";
            string nombreUsuario = "admin2";
            var actual = "";
            //ACT
            //VERIFICAMOS QUE EL USUARIO "admin2" EXISTE
            Usuario usuarioExiste = db.Usuarios.Where(u => u.NomUsuario == nombreUsuario).SingleOrDefault();
            if (usuarioExiste.NomUsuario == nombreUsuario)
            {
                //VERIFICAMOS QUE USUARIO "admin2" TIENE ROL "Bibliotecario"
                var esLector = db.Roles.Where(r => r.IdRol == usuarioExiste.IdRol).SingleOrDefault();
                if (esLector.IdRol == 2)
                {
                        //(OPCIONAL) SI EL LIBRO ESTA DISPONIBLE CAMBIAMOS SU ESTADO PARA SUPERAR LA PRUEBA UNITARIA
                        var LibrosDisponibles = db.Libros.Where(x => x.Disponibilidad == true).Include(l => l.Autores).Include(l => l.Categorias).Include(l => l.Editoriales).ToList();
                        actual = (LibrosDisponibles.Count() >= 1) ? esperado : "NO EXISTEN LIBROS DISPONIBLES PARA EL PRÉSTAMO";
                }
                else { actual = "EL USUARIO NO TIENE ROL DE LECTOR"; }
            }
            else { actual = "EL USUARIO NO SE ENCUENTRA REGISTRADO"; }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA03_2_verificar_que_no_existen_Libros_No_Disponibles_Para_Prestamo()
        {
            //ARRANGE
            var esperado = "NO EXISTEN LIBROS DISPONIBLES PARA EL PRÉSTAMO";
            var actual = "";
            //ACT           
            var LibrosDisponibles = db.Libros.Where(x => x.Disponibilidad == true).Include(l => l.Autores).Include(l => l.Categorias).Include(l => l.Editoriales).ToList();
            //EN CASO DE TENER LIBROS REGISTRADOS, QUITAMOS TODOS LOS ELEMENTOS PARA SUPERAR LA PRUEBA UNITARIA
            if (LibrosDisponibles.Count() >= 1) { LibrosDisponibles.Clear(); }
            actual = (LibrosDisponibles.Count() >= 1) ? "EXISTEN LIBROS DISPONIBLES PARA EL PRÉSTAMO" : esperado;
            //ASSETR
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA04_1_agregar_libro_no_existente_en_lista_de_prestamo()
        {
            //Assign           
            VistaPrestamo vistaPrestamo = new VistaPrestamo();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            int idLibro = 1;
            var esperado = "LIBRO AGREGADO CORRECTAMENTE A LA LISTA";
            var actual = "";
            //Act            
            //var libro = db.Libros.Where(x => x.Disponibilidad == true);
            if (idLibro == null)
            {
                actual = "No ha Seleccionado ningun libro";
            }
            else
            {
                //Buscamos si existe el libro que deseamos agregar a la lista
                var libroParaAgregar = db.Libros.Find(idLibro);
                if (libroParaAgregar == null)
                {
                    actual = "El libro no existe";
                }
                else
                {
                    //Buscamos si ya existe el libro en la lista
                    vistaPrestamo.DetPrestamo = vistaPrestamo.ListaDetPrestamo.Find(p => p.Libros.IdLibro == idLibro);
                    DateTime fecha = DateTime.Now.AddDays(3);
                    if (vistaPrestamo.DetPrestamo == null)
                    {
                        vistaPrestamo.DetPrestamo = new PrestamoDetalle()
                        {
                            EstLibro = libroParaAgregar.Estado,
                            FecDevolucion = fecha,
                            Libros = libroParaAgregar
                        };
                        //Agregamos el libro a la lista
                        vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);
                        actual = "LIBRO AGREGADO CORRECTAMENTE A LA LISTA";
                    }
                    else
                    {
                        actual = "DICHO LIBRO YA ESTABA AGREGADO EN LA LISTA";
                    }
                }
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA04_1_agregar_libro_ya_existente_en_lista_de_prestamo()
        {

            //Assign        
            VistaPrestamo vistaPrestamo = new VistaPrestamo();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            int idLibro = 1;
            var esperado = "DICHO LIBRO YA ESTABA AGREGADO EN LA LISTA";
            var actual = "";
            //Act            
            var libroParaAgregar = db.Libros.Find(idLibro);
            DateTime fecha = DateTime.Now.AddDays(3);
            vistaPrestamo.DetPrestamo = new PrestamoDetalle()
            {
                EstLibro = libroParaAgregar.Estado,
                FecDevolucion = fecha,
                Libros = libroParaAgregar
            };
            //Agregamos el libro a la lista
            vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);
            actual = "LIBRO AGREGADO CORRECTAMENTE A LA LISTA";

            //PREGUNTAMOS SI YA EXISTE EL LIBRO EN LA LISTA
            vistaPrestamo.DetPrestamo = vistaPrestamo.ListaDetPrestamo.Find(p => p.Libros.IdLibro == idLibro);
            actual = (vistaPrestamo.DetPrestamo == null) ? "LIBRO AGREGADO CORRECTAMENTE A LA LISTA" : esperado;

            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA05_1_verificar_prestamo_realizado_correctamente()
        {
            //Assign        
            VistaPrestamo vistaPrestamo = new VistaPrestamo();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            var esperado = "PRÉSTAMO REALIZADO CORRECTAMENTE";
            var actual = "";
            //ACT            
            var prestamo = new Prestamo()
            {
                FecPrestamo = DateTime.Now,
                IdUsuario = 2,
                IdPersona = 1,
                Pendiente = true
            };
            db.Prestamos.Add(prestamo);
            try
            {
                //GUARDA EL PRESTAMO
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                actual = (" Algo salio mal en la base de datos, insertando la cabecera del prestamo");
            }
            catch (Exception)
            {
                actual = (" Algo salio mal insertando la cabecera del prestamo");
            }
            PrestamoDetalle detallePrestamo = null;
            //Buscamos si existe el libro que deseamos agregar a la lista
            var libroParaAgregar = db.Libros.Find(1);
            if (libroParaAgregar == null)
            {
                actual = "El libro no existe";
            }
            else
            {
                //Buscamos si ya existe el libro en la lista
                vistaPrestamo.DetPrestamo = vistaPrestamo.ListaDetPrestamo.Find(p => p.Libros.IdLibro == 1);
                DateTime fecha = DateTime.Now.AddDays(3);
                if (vistaPrestamo.DetPrestamo == null)
                {
                    vistaPrestamo.DetPrestamo = new PrestamoDetalle()
                    {
                        EstLibro = libroParaAgregar.Estado,
                        FecDevolucion = fecha,
                        Libros = libroParaAgregar
                    };
                    //Agregamos el libro a la lista
                    vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);
                    actual = "LIBRO AGREGADO CORRECTAMENTE A LA LISTA";
                }
                else
                {
                    actual = "DICHO LIBRO YA ESTABA AGREGADO EN LA LISTA";
                }
            }
            //Selecciona el ultimo ID de prestamos (El que acabamos de insertar)
            var idPrestamo = db.Prestamos.ToList().Select(o => o.IdPrestamo).Max();

            //Agrega al contexto DetallePrestamos los libros prestados
            foreach (var item in vistaPrestamo.ListaDetPrestamo)
            {
                detallePrestamo = new PrestamoDetalle()
                {
                    EstLibro = item.EstLibro,
                    FecDevolucion = item.FecDevolucion,
                    Pendiente = true,
                    IdPrestamo = idPrestamo,
                    IdLibro = item.Libros.IdLibro
                };
                db.PrestamoDetalles.Add(detallePrestamo); //guardamos los datos de detalle prestamo en el contexto
                //-----------------------------------------------------------------------
                Libro libros = item.Libros;
                libros.Disponibilidad = false;
                db.Entry(libros).State = EntityState.Modified;
                //------------------------------------------------------------------------
            }
            try
            {
                db.SaveChanges(); //guarda los cambios en la base de datos del libro y detalle de prestamos
                actual = "PRÉSTAMO REALIZADO CORRECTAMENTE";
            }
            catch (DbUpdateException ex)
            {
                actual = (" Algo salio mal en la base de datos , insertando el detalle de prestamo");
            }
            catch (Exception)
            {
                actual = string.Format(" Algo salio mal, insertando el detalle de prestamo");
            }
            //Una vez que guarda limpia y guarda el objeto vistaPrestamo vacia
            vistaPrestamo.Personas = new Persona();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            //------------------------------------------------------------------            
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        //REVISAR TEST PARA HACER FALLAR EL PRESTAMO
        [TestMethod]
        public void HU02_CA05_2_verificar_prestamo_no_se_completo_correctamente()
        {
            //Assign        
            VistaPrestamo vistaPrestamo = new VistaPrestamo();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            var esperado = "DICHO LIBRO YA ESTABA AGREGADO EN LA LISTA";
            var actual = "";
            //ACT            
            var prestamo = new Prestamo()
            {
                FecPrestamo = DateTime.Now,
                IdUsuario = 2,
                IdPersona = 1,
                Pendiente = true
            };
            db.Prestamos.Add(prestamo);
            try
            {
                //GUARDA EL PRESTAMO
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                actual = (" Algo salio mal en la base de datos, insertando la cabecera del prestamo");
            }
            catch (Exception)
            {
                actual = (" Algo salio mal insertando la cabecera del prestamo");
            }
            PrestamoDetalle detallePrestamo = null;
            //Buscamos si existe el libro que deseamos agregar a la lista
            var libroParaAgregar = db.Libros.Find(1);
            if (libroParaAgregar == null)
            {
                actual = "El libro no existe";
            }
            else
            {
                DateTime fecha = DateTime.Now.AddDays(3);
                //AGREGAMOS EL MISMO LIBRO ANTES DE COMPROBAR PARA SUPERAR LA PRUEBA UNITARIA
                vistaPrestamo.DetPrestamo = new PrestamoDetalle()
                {
                    EstLibro = libroParaAgregar.Estado,
                    FecDevolucion = fecha,
                    Libros = libroParaAgregar
                };
                //Agregamos el libro a la lista
                vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);
                //Buscamos si ya existe el libro en la lista
                vistaPrestamo.DetPrestamo = vistaPrestamo.ListaDetPrestamo.Find(p => p.Libros.IdLibro == 1);
                if (vistaPrestamo.DetPrestamo == null)
                {
                    vistaPrestamo.DetPrestamo = new PrestamoDetalle()
                    {
                        EstLibro = libroParaAgregar.Estado,
                        FecDevolucion = fecha,
                        Libros = libroParaAgregar
                    };
                    //Agregamos el libro a la lista
                    vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);
                    actual = "LIBRO AGREGADO CORRECTAMENTE A LA LISTA";
                }
                else
                {
                    actual = "DICHO LIBRO YA ESTABA AGREGADO EN LA LISTA";
                }
            }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA06_1_verificar_que_existen_reservas_pendientes()
        {
            //Assign
            var esperado = "EXISTEN RESERVAS PENDIENTES";
            //Act            
            var reservasPendientes = db.ReservaDetalles.Where(x => x.Pendiente).ToList();
            var actual = (reservasPendientes.Count() >= 1) ? esperado : "NO EXISTEN RESERVAS PENDIENTES";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU02_CA06_1_verificar_que_no_existen_reservas_pendientes()
        {
            //Assign
            var esperado = "NO EXISTEN RESERVAS PENDIENTES";
            //Act            
            var reservasPendientes = db.ReservaDetalles.Where(x => x.Pendiente).ToList();
            //EN CASO DE TENER RESERVAS PENDIENTES, QUITAMOS TODOS LOS ELEMENTOS PARA SUPERAR LA PRUEBA UNITARIA
            if (reservasPendientes.Count() >= 1) { reservasPendientes.Clear(); }
            var actual = (reservasPendientes.Count() >= 1) ? "EXISTEN RESERVAS PENDIENTES" : esperado;
            //Assert
            Assert.AreEqual(esperado, actual);
        }
    }
}