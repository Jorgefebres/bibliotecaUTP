using BIBLIOTECA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using BIBLIOTECA.Models.ViewModels;
using BIBLIOTECA.Models.Metodos;
using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Helper;
using System.Data.Entity.Infrastructure;

namespace BIBLIOTECA.Controllers
{
    public class PrestamoController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        private Funciones fc = new Funciones();


        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene una lista de libros pendientes
        /// </summary>
        /// <returns>Si existe libros, Devuelve una vista con la lista de libros si no solo la vista</returns>
        // GET: Prestamo
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Listar)]
        public ActionResult Index()
        {
            var detPrestamo = db.PrestamoDetalles.Where(x=>x.Pendiente);
            if (detPrestamo != null)
            {
                return View(detPrestamo.ToList());
            }
            ViewBag.Error = "No hay prestamos pendientes a devolver";
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Busca la persona a la que se prestará el libro mediante su DNI
        /// Crea un nuevo model VistaPrestamo y guarda en una sesión para trabajar sobre ella
        /// </summary>
        /// <param name="DNI">Dni de la persona</param>
        /// <returns>Si la persona existe, Redirecciona a la vista nuevo prestamo, si no permace</returns>
        [HttpPost, ActionName("Index")]
        public ActionResult BuscaPersona(string DNI)
        {
            VistaPrestamo vistaPrestamo = new VistaPrestamo();
            vistaPrestamo.Personas = new Persona();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            if (!string.IsNullOrEmpty(DNI))
            {
                var persona = db.Personas.Where(x => x.Dni == DNI).SingleOrDefault();
                if (persona != null)
                {
                    vistaPrestamo.Personas = persona;
                    Session["vistaPrestamo"] = vistaPrestamo;
                    return RedirectToAction("NuevoPrestamo");
                }
                ViewBag.error = "No existe la persona con este dni";
                return View();
            }
            ViewBag.error = "Ingrese un Dni";
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene una lista de valores a partir de los caracteres enviados, y los lista sólo los valores distintos (Autocompledato)
        /// </summary>
        /// <param name="term">Caracteres a consultar</param>
        /// <returns>Devuelve una lista en formato Json</returns>
        //Busqueda Autocmplite
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Buscar_Persona)]
        public ActionResult GetPersona(string term)
        {
            List<string> dni;
            dni = db.Personas.Where(x => x.Dni.Contains(term)).Select(e => e.Dni).Distinct().ToList();
            return Json(dni, JsonRequestBehavior.AllowGet);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.3
        /// <summary>
        /// Recupera los datos del model VistaPrestamo de la sesión anteriormente guardada
        /// </summary>
        /// <returns>Devuelve una vista con los datos de la vistaPrestamo</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Nuevo_Prestramo)]
        public ActionResult NuevoPrestamo()
        {
            var vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            return View(vistaPrestamo);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.3
        /// <summary>
        /// Recupera los datos de la sesión, verifica si la persona esta sancionada, 
        /// si no lo está, continúa y registra el prestamo en la base de datos
        /// </summary>
        /// <param name="vistaPrestamo">Datos enviados de la vista</param>
        /// <returns>Devuelve una vista con los nuevos datos de vistaPrestamo</returns>
        [HttpPost]
        public ActionResult NuevoPrestamo(VistaPrestamo vistaPrestamo)
        {
            vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            int idPersona = vistaPrestamo.Personas.IdPersona;
            //Verificacion si está sancionado
            Consultas cons = new Consultas();
            if (cons.sancionado(idPersona))
            {
                var sancion = cons.sancion(idPersona);
                ViewBag.Error = string.Format("Ud. está sancionado desde {0} por mitivo {1} hasta {2}", sancion.FecInicio.Date, sancion.Descripcion, sancion.FecFin.Date);
                return View(vistaPrestamo);
            }

            if (vistaPrestamo.ListaDetPrestamo.Count == 0)
            {
                ViewBag.Error = "No envio ni un libro";
                return View(vistaPrestamo);
            }
            //Extremos el id del usuario de Session Helper
            int idUsuario = SessionHelper.GetUser();

            var prestamo = new Prestamo()
            {
                FecPrestamo = DateTime.Now,
                IdUsuario = idUsuario,
                IdPersona = idPersona,
                Pendiente = true
            };
            db.Prestamos.Add(prestamo);
            try
            {
                db.SaveChanges(); //guarda los cambios en la base de datos de prestamo
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Error = string.Format(" Algo salio mal en la base de datos");
                return View(vistaPrestamo);
            }
            catch (Exception)
            {
                ViewBag.Error = string.Format(" Algo salio mal");
                return View(vistaPrestamo);
            }

            PrestamoDetalle detallePrestamo = null;

            var idPrestamo = db.Prestamos.ToList().Select(o => o.IdPrestamo).Max();//selecciona el maximo numero de prestamos osea el ultimo q ingrese

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
            }
            catch (DbUpdateException ex)
            {
                ViewBag.Error = string.Format(" Algo salio mal en la base de datos");
                return View(vistaPrestamo);
            }
            catch (Exception)
            {
                ViewBag.Error = string.Format(" Algo salio mal");
                return View(vistaPrestamo);
            }
            //Una vez que guarda limpia y guarda en la session el objeto vistaPrestamo vacia
            vistaPrestamo.Personas = new Persona();
            vistaPrestamo.DetPrestamo = new PrestamoDetalle();
            vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
            Session["vistaPrestamo"] = vistaPrestamo;
            //------------------------------------------------------------------
            //Envia estos datos a la vista NuevoPrestamo
            ViewBag.Message = string.Format("El préstamo fue realizada con éxito con la orden {0}", idPrestamo);
            return View(vistaPrestamo);
            //----------------------------------------------
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene los libros disponibles de la base de datos
        /// </summary>
        /// <returns>Devuelve una vista con una lista de libros</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Listar_Libro_Disponibles)]
        public ActionResult AgregarLibro()
        {
            //LIsta de libros disponobles con su autore, categoria y editorial
            var Libros = db.Libros.Where(x => x.Disponibilidad == true).Include(l => l.Autores).Include(l => l.Categorias).Include(l => l.Editoriales);
            return View(Libros.ToList());
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Agrega 1 a 3 libros distintos a la lista de libros a prestar y los guarda en la sesion creada anteriormente
        /// en base al identificador del libro
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve una vista con los nuevos datos de vistaPrestamo</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Agregar_Libro_Al_Prestamo)]
        public ActionResult AgregarLibro1(int? id)
        {
            var vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            var libro = db.Libros.Where(x => x.Disponibilidad == true);
            if (id == null)
            {
                ViewBag.Error = "No ha Seleccionado ni un libros";
                return View("NuevoPrestamo", vistaPrestamo);
            }
            var libros = db.Libros.Find(id);
            if (libros == null)
            {
                ViewBag.Error = "El libro no existe";
                return View("NuevoPrestamo", vistaPrestamo);

            }
            vistaPrestamo.DetPrestamo = vistaPrestamo.ListaDetPrestamo.Find(p => p.Libros.IdLibro == id);
            DateTime fecha = fc.FehasMas(DateTime.Now,3);
            if (vistaPrestamo.DetPrestamo == null)
            {
                vistaPrestamo.DetPrestamo = new PrestamoDetalle()
                {
                    EstLibro = libros.Estado,
                    FecDevolucion = fecha,
                    Libros = libros
                };
            }
            else
            {
                ViewBag.Error = "Ya agregó Este libro";
                return PartialView("NuevoPrestamo", vistaPrestamo);
            }
            int yaprestados = db.PrestamoDetalles.Where(x => x.Pendiente && x.Prestamos.IdPersona == vistaPrestamo.Personas.IdPersona).Count();
            if ((vistaPrestamo.ListaDetPrestamo.Count + yaprestados) > 2)
            {
                ViewBag.Error = "Solo tiene acceso a esta cantidad";
                return PartialView("NuevoPrestamo", vistaPrestamo);
            }
            vistaPrestamo.ListaDetPrestamo.Add(vistaPrestamo.DetPrestamo);//Agregar a la lista para mostrar en la vista
            return PartialView("NuevoPrestamo", vistaPrestamo);

        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Cancela o resta de la lista de libros a prestar,
        /// un libro que ya no se requiere prestar, mediante el identificador del libro
        /// luego realiza los cambios en la sesión, en la cual se está trabajando
        /// </summary>
        /// <param name="id">identificador del libro</param>
        /// <returns>Devuelve una vista con los nuevos datos</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Prestamo_Cancelar)]
        public ActionResult Cancelar(int? id)
        {
            var vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            if (id != null)
            {
                PrestamoDetalle detalles = vistaPrestamo.ListaDetPrestamo.Find(x => x.Libros.IdLibro == id);
                vistaPrestamo.ListaDetPrestamo.Remove(detalles);//Remueve el libro devuelto de la lista
                Session["vistaPrestamo"] = vistaPrestamo;
                ViewBag.Message = "Se canceló el prestamo de este libro correctamente";
                return View("NuevoPrestamo", vistaPrestamo); //Retorna a la vista nueva reserva
            }
            return View("NuevoPrestamo", vistaPrestamo);
        }

        //Cerrar la base de datos
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