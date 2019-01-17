using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Helper;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.Metodos;
using BIBLIOTECA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers
{
    public class SancionController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        Funciones fc = new Funciones();
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene las sanciones pendientes de la base de datos
        /// </summary>
        /// <returns>Devielve la vista con una lista de sanciones</returns>
        // GET: Sancion
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Sancion_Listar)]
        public ActionResult Index()
        {
            var sanciones = db.Sancions.Where(x=>x.FecFin>=DateTime.Now);
            if (sanciones != null)
            {
                return View(sanciones.ToList());
            }
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// - Crea una nueva instancia para el model vistaSancion
        /// - Optiene los datos de la sesión vistaPrestamos mediante el identificador del libro y los guarda en una nueva sesión de vistaSancion
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Redirecciona a la vista de sanciones con los datos de la vistaSancion</returns>

        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Sancion_Devolver_Uno)]
        [HttpGet, ActionName("Devolver")]
        public ActionResult SancionModel(int id)
        {
            if (id<0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            if (vistaPrestamo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Extremos el id del usuario de Session helper
            int idUsuario = SessionHelper.GetUser();

            //extrae los datos de la lista de libros prestados
            var libros = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id).Libros;
            var detallePrestamo = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id);
            var prestamo = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id).Prestamos;

            VistaSansion vistaSansion = new VistaSansion();
            vistaSansion.Sanciones = new Sancion();
            vistaSansion.VistaPrestamos = new VistaPrestamo();
            vistaSansion.VistaPrestamos.DetPrestamo = new PrestamoDetalle();
            vistaSansion.VistaPrestamos.DetPrestamo.Prestamos = new Prestamo();
            vistaSansion.VistaPrestamos.ListaDetPrestamo = new List<PrestamoDetalle>();

            //vistaSansion.Devoluciones = new Devolucion();
            vistaSansion.VistaPrestamos.DetPrestamo = detallePrestamo;
            vistaSansion.VistaPrestamos.DetPrestamo.Prestamos = prestamo;
            vistaSansion.VistaPrestamos.ListaDetPrestamo = vistaPrestamo.ListaDetPrestamo;

            Session["vistaSansion"] = vistaSansion; //Retorna a la vista nueva reserva
            return RedirectToAction("SancionVista");
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Recupera los datos de la sesión vistaSancion
        /// </summary>
        /// <returns>Si la sesión no esta vacía devuleve la vista con los datos de la vistaSancion, si no solo vista</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Sancion_Sancion_Vista)]
        public ActionResult SancionVista()
        {
            var vistaSansion = Session["vistaSansion"];
            if (vistaSansion!=null)
            {
                return View(vistaSansion);
            }
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Primero devuelve el libros sobre el cual se aplicará la sancion, mediante el identificador del libro.
        /// Luego se realiza la sanción mediante el identificador de la devolución, el cual contiene dotos los datos necesarios
        /// </summary>
        /// <param name="vistaSancion">Datos de la sancion</param>
        /// <param name="dias">dias de sanción</param>
        /// <returns></returns>
        [HttpPost, ActionName("SancionVista")]
        public ActionResult Sancionar(VistaSansion vistaSancion, int dias)
        {           
            //Extremos el id del usuario de Session helper
            int idUsuario = SessionHelper.GetUser();
            var vistaSancionSession = Session["vistaSansion"] as VistaSansion;
            //extrae los datos de la lista de libros prestados

            var libros = vistaSancionSession.VistaPrestamos.DetPrestamo.Libros;
            var detallePrestamo = vistaSancionSession.VistaPrestamos.DetPrestamo;
            var prestamo = vistaSancionSession.VistaPrestamos.DetPrestamo.Prestamos;

            if (libros == null || detallePrestamo == null || prestamo == null)
            {
                ViewBag.Error = "No existe datos con este identificador";
                return View(vistaSancion); //Retorna a la vista nueva reserva
            }
            //pone el libro en disponible
            Libro libro_n = new Libro()
            {
                IdLibro = libros.IdLibro,
                Titulo = libros.Titulo,
                Idioma = libros.Idioma,
                ISBN = libros.ISBN,
                IdAutor = libros.IdAutor,
                IdCategoria = libros.IdCategoria,
                IdEditorial = libros.IdEditorial,
                NumEdicion = libros.NumEdicion,
                Anio = libros.Anio,
                NumPaginas = libros.NumPaginas,
                Estado = libros.Estado,
                Caratula = libros.Caratula,
                Disponibilidad = true
            };
            db.Entry(libro_n).State = EntityState.Modified;

            //cambia pendiente a false para que ya no tenga prestamos pendientes
            PrestamoDetalle detallePrestamo_n = new PrestamoDetalle()
            {
                IdPrestamoDetalle = detallePrestamo.IdPrestamoDetalle,
                EstLibro = detallePrestamo.EstLibro,
                FecDevolucion = detallePrestamo.FecDevolucion,
                Pendiente = false,
                IdPrestamo = detallePrestamo.IdPrestamo,
                IdLibro = detallePrestamo.IdLibro
            };
            db.Entry(detallePrestamo_n).State = EntityState.Modified;

            //Pone falso cuando ya no tiene ni un libro pendite a devolver
            if (vistaSancionSession.VistaPrestamos.ListaDetPrestamo.Count == 1)
            {
                Prestamo prestamo_n = new Prestamo()
                {
                    IdPrestamo = prestamo.IdPrestamo,
                    FecPrestamo = prestamo.FecPrestamo,
                    Pendiente = false,
                    IdUsuario = prestamo.IdUsuario,
                    IdPersona = prestamo.IdPersona

                };
                db.Entry(prestamo_n).State = EntityState.Modified;
            }

            Devolucion devolucion = new Devolucion
            {
                FecDevolcion = DateTime.Now,
                IdPrestamoDetalle = detallePrestamo.IdPrestamoDetalle,
                IdUsuario = idUsuario
            };
            db.Devoluciones.Add(devolucion);
            try
            {
                db.SaveChanges();//Guarda los cambios
            }
            catch (Exception)
            {

                ViewBag.Error = "Algo salio mal";
                return View(vistaSancion); //Retorna a la vista nueva reserva
            }
            if (vistaSancion.Sanciones.Estado == null)
            {
                vistaSancion.Sanciones.Estado = vistaSancionSession.VistaPrestamos.DetPrestamo.EstLibro;
            }
            Sancion sancion = new Sancion() {
                Descripcion= vistaSancion.Sanciones.Descripcion,
                Estado= vistaSancion.Sanciones.Estado,
                FecInicio= vistaSancion.Sanciones.FecInicio,
                FecFin= fc.FehasMas(vistaSancion.Sanciones.FecInicio,dias),
                IdDevolucion=devolucion.IdDevolucion
            };
            db.Sancions.Add(sancion);
            db.SaveChanges();
            PrestamoDetalle detalles = vistaSancionSession.VistaPrestamos.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == detallePrestamo.IdPrestamoDetalle);
            vistaSancionSession.VistaPrestamos.ListaDetPrestamo.Remove(detalles);//Remueve el libro devuelto de la lista
            ViewBag.Message = "Libro devuelto con exito";
            Session["vistaPrestamo"] = vistaSancionSession.VistaPrestamos;

            return RedirectToAction("Devolver","Devolucion",""); //Retorna a la vista nueva reserva
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}