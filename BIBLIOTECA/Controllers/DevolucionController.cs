using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using System;
using BIBLIOTECA.Models.ViewModels;
using System.Collections.Generic;
using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Helper;
using System.Net;
using BIBLIOTECA.Models.Metodos;

namespace BIBLIOTECA.Controllers
{
    public class DevolucionController : Controller
    {
        /// <summary>
        /// Crea una nueva instacia del Contexto
        /// </summary>
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        /// <summary>
        /// Crea una nueva instancia para la clase Funciones
        /// </summary>
        Funciones fc = new Funciones();

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene y envía las ultimas devoluciones (a partir de tres meses antes)
        /// </summary>
        /// <returns>vista con las devoluciones</returns>
        // GET: Devolucion
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Devolucion_Listar)]
        public ActionResult Index()
        {
            var devoluciones = db.Devoluciones.Where(x => x.FecDevolcion.Month > (DateTime.Now.Month-3));
            return View(devoluciones.ToList());
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene una lista de valores a partir de los caracteres enviados y los lista sólo valores distintos
        /// </summary>
        /// <param name="term">Caracteres a consultar</param>
        /// <returns>Devuelve una lista en formato Json</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Devolucion_Buscar_Persona)]
        public ActionResult GetPersona(string term)
        {
            List<string> dni;
            dni = db.Personas.Where(x => x.Dni.StartsWith(term)).Select(e => e.Dni).Distinct().ToList();

            return Json(dni, JsonRequestBehavior.AllowGet);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Si aun no se ha creado el modelo VistaPrestamo, crea una nueva instancia y los guarda en una sesión 
        /// </summary>
        /// <returns>Devuelve vista con vistaPrestamo creado o obtenido de la sesion si se ha creado con anterioridad</returns>
        // GET: Devolucion
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Devolucion_Vista_Devolver)]
        public ActionResult Devolver() //cambiar nombre
        {
            VistaPrestamo vistaPrestamo;
            
            if (Session["vistaPrestamo"] == null)
            {
                //Creamos el objeto vistaPrestamo pero vacia
                vistaPrestamo = new VistaPrestamo();
               // vistaPrestamo.Personas = new Persona();
                vistaPrestamo.DetPrestamo = new PrestamoDetalle();
                vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>();
                Session["vistaPrestamo"] = vistaPrestamo; //Y lo guardamos en una sesion
            }
            else
            {
                vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            }
            return View(vistaPrestamo);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtienes los datos del PrestamoDetalles de la base de datos, a partir del DNI de la persona,
        /// genera una lista con los datos requeridos, luego los guarda en la sesion anteriormente creada
        /// para luego mostrar en la vista
        /// </summary>
        /// <param name="vistaPrestamo">VistaPrestamos a llenar con los datos obtenidos de la base de datos</param>
        /// <param name="DNI">Dni de la persona quien quiere devolver libro(s)</param>
        /// <returns>vista con los datos de detalle de prestamo</returns>
        [HttpPost]
        public ActionResult Devolver(VistaPrestamo vistaPrestamo, string DNI)
        {

            //Si la sesion es nula, aun no se a creado el objeto vistaPrestamo
            if (Session["vistaPrestamo"] != null)
            {
                //Si ya fue creada
                //recuperamos los datos de la sesion q estan vacias y los llenamos
                vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
                if (DNI == null)
                {   ViewBag.Error = "Tiene que ingresar dni del lector";
                    return View(vistaPrestamo);
                }
                //Recuperamos el id de la persona quien quiere devolver que enviamos de la vista
                var Persona = db.Personas.Where(x => x.Dni == DNI).FirstOrDefault();
                if (Persona == null)
                {
                    ViewBag.Error = "Lector no existe en la base de datos";
                    return View(vistaPrestamo);
                }
                int idPersona = Persona.IdPersona;
                vistaPrestamo.ListaDetPrestamo = new List<PrestamoDetalle>(); //Limpiamos creando uno nuevo
                //Buscamos prestamos q pertenecen a este persona y si aun estan pendientes
                var prestamo = db.Prestamos.Where(x => x.IdPersona == idPersona && x.Pendiente == true).ToList();
                if (prestamo.Count>0)
                {
                    foreach (var item in prestamo)
                    {
                        //Buscamos libros que prestó en este prestamo, si fueron varios con el id del prestamo
                        var detalleprestamos = db.PrestamoDetalles.Where(x => x.IdPrestamo == item.IdPrestamo && x.Pendiente == true).ToList();
                        foreach (var i in detalleprestamos)
                        {
                            PrestamoDetalle detalleprestamo = new PrestamoDetalle()
                            {
                                IdPrestamoDetalle = i.IdPrestamoDetalle,
                                EstLibro = i.EstLibro,
                                FecDevolucion = i.FecDevolucion,
                                Pendiente = i.Pendiente,
                                IdPrestamo = i.IdPrestamo,
                                IdLibro = i.IdLibro,
                                Prestamos = i.Prestamos,
                                Libros = i.Libros
                            };
                            vistaPrestamo.ListaDetPrestamo.Add(detalleprestamo);//Agregar a la lista para mostrar en la vista
                        }
                    }
                }
                ViewBag.Error = "Ud no tiene ningun libro para devolver!!!";
                return View(vistaPrestamo);
            }
            ViewBag.Error = "Ud. intenta ingresar sospechosamente¡¡¡";
            return View(vistaPrestamo);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Modifica la disponibilidad del Libro a disponible
        /// Modifica elestado de la PrestamoDetalles y de la tabla Prestamo
        /// Y registra una nueva devolución en base al identificador de PrestamoDetalles
        /// </summary>
        /// <param name="id">Identificador de PrestamoDetalles</param>
        /// <returns>vista con los nuevos datos de la sesion</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Devolucion_Devover_Uno)]
        public ActionResult Devolver1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vistaPrestamo = Session["vistaPrestamo"] as VistaPrestamo;
            if (vistaPrestamo == null)
            {
                ViewBag.Error = "No hay libros que devolver";
                return View("Devolver", vistaPrestamo); //Retorna a la vista nueva reserva
            }

            //Extremos el id del usuario de Session helper
            int idUsuario = SessionHelper.GetUser();

            //extrae los datos de la lista de libros prestados de la sesion guardada anteriomente
            //con el identificador que recibe
            var libros = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id).Libros;
            var detallePrestamo = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id);
            var prestamo = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id).Prestamos;

            if (libros == null || detallePrestamo == null || prestamo == null)
            {
                ViewBag.Error = "No existe datos con este identificador";
                return View("Devolver", vistaPrestamo); //Retorna a la vista nueva reserva
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


            //Solo modifica cuando el PrestamoDetalles sea la última
            if (vistaPrestamo.ListaDetPrestamo.Count == 1)
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
                IdPrestamoDetalle = (int)id,
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
                return View("Devolver", vistaPrestamo); //Retorna a la vista nueva reserva
            }
            PrestamoDetalle detalles = vistaPrestamo.ListaDetPrestamo.Find(x => x.IdPrestamoDetalle == id);
            vistaPrestamo.ListaDetPrestamo.Remove(detalles);//Remueve el libro devuelto de la lista
            ViewBag.Message = "Libro devuelto con exito";
            return View("Devolver", vistaPrestamo); //Retorna a la vista nueva reserva
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Cierra la conexión a la base de datos
        /// </summary>
        /// <param name="disposing">si está abrieta</param>
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