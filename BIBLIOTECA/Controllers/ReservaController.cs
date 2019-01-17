using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Helper;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.Metodos;
using BIBLIOTECA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers
{
    public class ReservaController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        private Funciones fc = new Funciones();

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene los detalles de reserva pendientes de la base de datos, en base al identificador del lector
        /// Si la sesion está vacía, crea una nueva instancia de model vistaReservas y los guarda en la sesion
        /// incluyendo los datos de detalle de reserva
        /// </summary>
        /// <returns>Si la sesión esta vacía, devuelve una vista con los datos de vistaReserva vacia, 
        /// si no envía los datos que contiene la sesión</returns>
        // GET: Reserva/lista de reservas
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Reservar_Listar)]
        public ActionResult Index()
        {
            VistaReserva vistaReserva=null;
            if (Session["vistaReserva"] == null)
            {
                int idUser = (int)Helper.SessionHelper.GetUser();
                var detReserva = db.ReservaDetalles.Where(x => x.Pendiente && x.Reservas.IdUsuario == idUser);
                vistaReserva = new VistaReserva();
                vistaReserva.DetReserva = new ReservaDetalle();
                vistaReserva.ListaDetReserva = new List<ReservaDetalle>();
                vistaReserva.ListaDetReserva = detReserva.ToList();
                Session["vistaReserva"] = vistaReserva;
                return View(vistaReserva);
            }
            vistaReserva = Session["vistaReserva"] as VistaReserva;
            return View(vistaReserva);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Envia una vista vacía
        /// </summary>
        /// <returns>devuelve una vista vacía</returns>
        //Crea una vista vacia
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Reservar_Nueva_Reserva)]
        public ActionResult NuevaReserva()
        {
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Si el lector no esta snacionada realiza una nueva reserva de libros
        /// </summary>
        /// <param name="vistaReserva">Datos de los libros</param>
        /// <returns>Devuelve una vista con los datos vacías</returns>
        [HttpPost]
        public ActionResult NuevaReserva(VistaReserva vistaReserva)
        {
            vistaReserva = Session["vistaReserva"] as VistaReserva;

            if (vistaReserva.ListaDetReserva.Count == 0)
            {
                ViewBag.Error = "No envio ni un libro";
                return View(vistaReserva);
            }
            //Extremos el id del usuario de Session helper
            int idUsuario = SessionHelper.GetUser();

            //Verificacion si está sancionado
            var usuario = db.Usuarios.Find(idUsuario);
            Consultas cons = new Consultas();
            if (cons.sancionado(usuario.IdPersona))
            {
                var sancion = cons.sancion(usuario.IdPersona);
                ViewBag.Error = string.Format("Ud. está sancionado desde {0} por mitivo {1} hasta {2}", sancion.FecInicio.Date, sancion.Descripcion, sancion.FecFin.Date);
                return View(vistaReserva);
            }

            DateTime fecha = fc.FehasMas(DateTime.Now,3);
            var reserva = new Reserva()
            {
                FecReserva = DateTime.Now,
                FecRecojo = fecha,
                Pendiente = true,
                IdUsuario = idUsuario
            };
            db.Reservas.Add(reserva);

            try
            {
                db.SaveChanges(); //guarda los cambios en la base de datos de reserva
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Error = string.Format(" Algo salio mal en lado del servidor");
                return View("NuevaReserva", vistaReserva);
            }
            catch (Exception)
            {
                ViewBag.Error = string.Format(" Algo salio mal");
                return View("NuevaReserva", vistaReserva);
            }
            //buscar el id de reserva que se ingró actualmente
            var idReserva = db.Reservas.ToList().Select(o => o.IdReserva).Max();
            foreach (var item in vistaReserva.ListaDetReserva)
            {
                var detalleReserva = new ReservaDetalle()
                {
                    IdReserva = idReserva,
                    IdLibro = item.Libros.IdLibro,
                    Pendiente = true
                };
                db.ReservaDetalles.Add(detalleReserva);

                Libro libro_n = new Libro()
                {
                    IdLibro = item.Libros.IdLibro,
                    Titulo = item.Libros.Titulo,
                    Idioma = item.Libros.Idioma,
                    ISBN = item.Libros.ISBN,
                    IdAutor = item.Libros.IdAutor,
                    IdCategoria = item.Libros.IdCategoria,
                    IdEditorial = item.Libros.IdEditorial,
                    NumEdicion = item.Libros.NumEdicion,
                    Anio = item.Libros.Anio,
                    NumPaginas = item.Libros.NumPaginas,
                    Estado = item.Libros.Estado,
                    Caratula = item.Libros.Caratula,
                    Disponibilidad = false //Modificacion de la diponibilidad del libro
                };
                db.Entry(libro_n).State = EntityState.Modified;
            }
            try
            {
                db.SaveChanges(); //guarda los cambios en la base de datos de libros y detalles de reservas
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Error = string.Format("Algo salio mal en lado del servidor");
                return View("NuevaReserva", vistaReserva);
            }
            catch (Exception)
            {
                ViewBag.Error = string.Format("Algo salio mal");
                return View("NuevaReserva", vistaReserva);
            }

            ViewBag.Message = string.Format("Su reserva se realizó correctamente su codigo es: {0} tiene para recoger hasta {1}", idReserva,reserva.FecRecojo);
            vistaReserva = new VistaReserva();
            vistaReserva.DetReserva = new ReservaDetalle();
            vistaReserva.ListaDetReserva = new List<ReservaDetalle>();
            Session["vistaReserva"] = vistaReserva;
            return View("NuevaReserva", vistaReserva);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Si la sesion esta vacía crea una nueva instancia de la vistaReservas y los guarda en la sesíon,
        /// si no, solo envía el identificador del libro
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Ejecuta la funcion AgregarLibroReserva1 con el identificador del libro</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Reservar_Agregar_A_La_REserva)]
        public ActionResult AgregarLibroReserva(int id)
        {
            if (Session["vistaReserva"] == null)
            {
                VistaReserva vistaReserva = new VistaReserva();
                vistaReserva.DetReserva = new ReservaDetalle();
                vistaReserva.ListaDetReserva = new List<ReservaDetalle>();
                Session["vistaReserva"] = vistaReserva;
            }
            return AgregarLibroReserva1(id); //Agrega Automaticamente a la tabla de reservas sin ir a otra vista de agregar libros
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Agrega libros mediante el identificador del libro a la lista de libros a reservar 
        /// en el model vistaReservas y guarda en la sesión
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve una vista con los nuevo datos del model vistaReservas</returns>
        [HttpPost]
        public ActionResult AgregarLibroReserva1(int id)
        {
            var vistaReserva = Session["vistaReserva"] as VistaReserva;
            var usuario = db.Usuarios.Find(Helper.SessionHelper.GetUser());
            //Verificacion si está sancionado
            Consultas cons = new Consultas();
            if (cons.sancionado(usuario.IdPersona))
            {
                var sancion = cons.sancion(usuario.IdPersona);
                ViewBag.Error = string.Format("Ud. está sancionado desde {0} por mitivo {1} hasta {2}", sancion.FecInicio.Date, sancion.Descripcion, sancion.FecFin.Date);
                return View(vistaReserva);
            }
            var Libros = db.Libros.Find(id);
            if (Libros == null)
            {
                ViewBag.Error = "El libro no existe";
                return View("NuevaReserva", vistaReserva);
            }
            ReservaDetalle detalleReserva;
            Reserva reserva = new Reserva();
            reserva.FecRecojo = fc.FehasMas(DateTime.Now, 3);
            detalleReserva = vistaReserva.ListaDetReserva.Find(p => p.Libros.IdLibro == id);
            if (detalleReserva == null)
            {
                detalleReserva = new ReservaDetalle()
                {
                    Reservas = reserva,
                    Libros = Libros
                };
            }
            else
            {
                ViewBag.Error = "Ya agregó Este libro";
                return View("NuevaReserva", vistaReserva);
            }
            int idUser = (int)Helper.SessionHelper.GetUser();
            int cont = db.ReservaDetalles.Where(x => x.Pendiente && x.Reservas.IdUsuario == idUser).Count();
            if ((vistaReserva.ListaDetReserva.Count + cont) > 2)
            {
                ViewBag.Error = "La maxima cantidad para reservar es 3 libros.";
                return View("NuevaReserva", vistaReserva);
            }
            vistaReserva.ListaDetReserva.Add(detalleReserva);
            return View("NuevaReserva", vistaReserva); //Retorna a la vista nueva reserva
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Cancela o resta un libro mediante su identificador que no se desea reservar de la lista de libros a reservar y actualiza la sesión
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve la vista con los nuevos datos</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Reservar_Cancelar)]
        public ActionResult Cancelar(int? id)
        {
            var vistaReserva = Session["vistaReserva"] as VistaReserva;
            if (id != null)
            {
                ReservaDetalle detalles = vistaReserva.ListaDetReserva.Find(x => x.Libros.IdLibro == id);
                vistaReserva.ListaDetReserva.Remove(detalles);//Remueve el libro devuelto de la lista
                Session["vistaReserva"] = vistaReserva;
                ViewBag.Message = "Se canceló la reserva de este libro correctamente";
                return View("NuevaReserva", vistaReserva); //Retorna a la vista nueva reserva
            }
            return View("NuevaReserva", vistaReserva);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Cancela una reserva que ya esta guardada en la base de datos 
        /// mediante el identificador del libro reservado y actualiza la sesion anteriormente creada
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve la vista con los datos actualizados</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Reservar_CancelarBD)]
        public ActionResult CancelarBD(int id)
        {
            var vistaReserva = Session["vistaReserva"] as VistaReserva;
            var idUsuario = Helper.SessionHelper.GetUser();
            //extrae los datos de la lista de libros prestados
            var libros = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Libros;
            var detalleReserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id);
            var reserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Reservas;
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
            ReservaDetalle detalleReserva_n = new ReservaDetalle()
            {
                IdReservaDetalle = detalleReserva.IdReservaDetalle,
                IdReserva = detalleReserva.IdReserva,
                IdLibro = detalleReserva.IdLibro,
                Pendiente = false
            };
            db.Entry(detalleReserva_n).State = EntityState.Modified;
            //Pone falso cuando ya no tiene ni un libro pendite a devolver
            var detReservaLista = db.ReservaDetalles.Where(x => x.IdReserva == detalleReserva.IdReserva).ToList();
            if (detReservaLista.Count == 1)
            {
                Reserva reserva_n = new Reserva()
                {
                    IdReserva = reserva.IdReserva,
                    FecReserva = reserva.FecReserva,
                    FecRecojo = reserva.FecRecojo,
                    Pendiente = false,
                    IdUsuario = reserva.IdUsuario
                };
                db.Entry(reserva_n).State = EntityState.Modified;
            }
            db.SaveChanges();//Guarda los cambios
            ReservaDetalle detalles = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id);
            vistaReserva.ListaDetReserva.Remove(detalles);//Remueve el libro devuelto de la lista
           // Session["vistaReserva"] = vistaReserva;
            ViewBag.Message = "Se canceló su reserva de este libro correctamente";
            return RedirectToAction("Index");
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