using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Helper;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.Metodos;
using BIBLIOTECA.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers
{
    public class PrestarReservaController : Controller
    {
       private BIBLIOTECAContext db = new BIBLIOTECAContext();
        private Funciones fc = new Funciones();

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos de detalles de reserva disponibles
        /// </summary>
        /// <returns>Devuelve una lista con los datos de detalles de reserva</returns>
        // GET: PrestarReserva
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PrestamoReserva_Listar_Reservas)]
        public ActionResult Index()
        {
            var detReserva = db.ReservaDetalles.Where(x => x.Pendiente);
            return View(detReserva.ToList());
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Crea una nueva instancia de VistaReservas y los guarda en una sesión
        /// </summary>
        /// <returns>Devuelve una vista con los datos de vistaReserva</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PrestamoReserva_Buscar_Reserva)]
        public ActionResult PrestarReserva()
        {
            //Creamos el objeto vistaPrestamo pero vacia
            VistaReserva vistaReserva;
            vistaReserva = new VistaReserva();
            vistaReserva.DetReserva = new ReservaDetalle();
            vistaReserva.ListaDetReserva = new List<ReservaDetalle>();
            Session["vistaReserva"] = vistaReserva; //Y lo guardamos en una sesion
            return View(vistaReserva);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Busca las reservas y detalles de reserva que realizó esta persona mediante su DNI
        /// y los guarda en la sesion anteriormente creada
        /// </summary>
        /// <param name="vistaReserva">datos de la vistaReserva</param>
        /// <param name="DNI">Dni de la persona</param>
        /// <returns>Devuelve una vista con los nuevos datos de vistaReserva</returns>
        [HttpPost, ActionName("PrestarReserva")]
        public ActionResult BuscarReservas(VistaReserva vistaReserva, string DNI)
        {
            //Si la sesion es nula, aun no se a creado el objeto vistaPrestamo
            if (Session["vistaReserva"] != null)
            {

                //Si ya fue creada
                //recuperamos los datos de la sesion q estan vacias y los llenamos
                vistaReserva = Session["vistaReserva"] as VistaReserva;

                //Recuperamos el id de la persona quien quiere devolver, con el Dni que enviamos de la vista
                var Persona = db.Personas.Where(x => x.Dni == DNI).FirstOrDefault();
                if (Persona == null)
                {
                    ModelState.AddModelError("", "Este DNI no existe en la base de datos");
                    return View(vistaReserva);
                }
                int idPersona = Persona.IdPersona;

                var usuario = db.Usuarios.Where(x => x.IdPersona == idPersona).FirstOrDefault();
                if (usuario == null)
                {
                    ModelState.AddModelError("", "No existe una cuenta de usuario");
                    return View(vistaReserva);
                }

                //Buscamos reservas q pertenecen a este persona y si aun estan pendientes
                var reservas = db.Reservas.Where(x => x.IdUsuario == usuario.IdUsuario && x.Pendiente == true).ToList();
                if (reservas.Count < 1)
                {
                    ViewBag.Error = "Ud no Reservó ningun libro o ya se pasó su fecha de recojo";
                    return View(vistaReserva);
                }
                vistaReserva.ListaDetReserva = new List<ReservaDetalle>(); //Limpiamos creando uno nuevo
                foreach (var item in reservas)
                {
                    //Buscamos libros reservados, si fueron varios con el id del reserva
                    var detalleReserva = db.ReservaDetalles.Where(x => x.IdReserva == item.IdReserva && x.Pendiente == true).ToList();
                    foreach (var i in detalleReserva)
                    {
                        var detallereserva = new ReservaDetalle()
                        {
                            IdReservaDetalle = i.IdReservaDetalle,
                            IdReserva = i.IdReserva,
                            IdLibro = i.IdLibro,
                            Pendiente = i.Pendiente,
                            Reservas = i.Reservas,
                            Libros = i.Libros
                        };
                        vistaReserva.ListaDetReserva.Add(detallereserva);//Agregar a la lista para mostrar en la vista
                    }
                }
            }
            return View(vistaReserva);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Realiza un prestamo a la vez de cada libro reservado mediente el identificador de detalle reserva,
        /// realiza los cambios correspondientes 
        /// (disponibilidad=falso en libros, pendiente=verdad en prestamos y detalles de prestamo, y tambien
        /// pendiente=false en detalle de reservas y reservas)
        /// </summary>
        /// <param name="id">identificador detalle de reserva</param>
        /// <returns>Devuelve una vista con los nuevos datos de vistaReserva</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PrestamoReserva_Prestar_uno)]
        public ActionResult Prestar(int id)
        {

            var vistaReserva = Session["vistaReserva"] as VistaReserva;
            //extrae los datos de la lista de libros prestados
            var libros = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Libros;
            var detalleReserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id);
            var reserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Reservas;

            //pone el libro en dehabilitado
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
                Disponibilidad = false
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
            if (vistaReserva.ListaDetReserva.Count == 1)
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
            //Extremos el id del usuario de Session helper
            int idUsuario = SessionHelper.GetUser();
            Prestamo prestamo = new Prestamo
            {
                FecPrestamo = DateTime.Now,
                Pendiente = true,
                IdUsuario = idUsuario,
                IdPersona = reserva.Usuarios.Personas.IdPersona
            };
            db.Prestamos.Add(prestamo);
            db.SaveChanges();//Guarda los cambios

            DateTime fecha = fc.FehasMas(DateTime.Now, 3);
            int idPrestamo = db.Prestamos.Select(x => x.IdPrestamo).Max();
            PrestamoDetalle prestamoDetalle = new PrestamoDetalle()
            {
                EstLibro = libros.Estado,
                FecDevolucion = fecha,
                Pendiente = true,
                IdPrestamo = idPrestamo,
                IdLibro = libros.IdLibro
            };

            db.PrestamoDetalles.Add(prestamoDetalle);
            try
            {
                db.SaveChanges();//Guarda los cambios
            }
            catch (Exception)
            {
                ViewBag.Error = "Algo salio mal";
                return View("PrestarReserva", vistaReserva);  //Retorna a la vista nueva reserva
            }
            ReservaDetalle detalles = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id);
            vistaReserva.ListaDetReserva.Remove(detalles);//Remueve el libro devuelto de la lista
            ViewBag.Message = "El Libro se presto con exito";
            return View("PrestarReserva", vistaReserva); //Retorna a la vista nueva reserva
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Si no requiere del prestamo de un libro determinado, se realiza los cambios en la base de datos
        /// (disponibilidad=verdadero en libros, pendiente=false en detalle de reservas y reservas) 
        /// y tambien se realiza cambios en la sesión en la que se trabaja a través del identificador de detalles de reserva
        /// </summary>
        /// <param name="id">identificador de detalles de reserva</param>
        /// <returns>Devuelve una vista con los nuevos datos de la sesión</returns>
        //Falta implementar
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PrestamoReserva_Cancelar)]
        public ActionResult Cancelar(int id)
        {
            var vistaReserva = Session["vistaReserva"] as VistaReserva;
            var idUsuario = Helper.SessionHelper.GetUser();
                //extrae los datos de la lista de libros prestados
                var libros = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Libros;
                var detalleReserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id);
                var reserva = vistaReserva.ListaDetReserva.Find(x => x.IdReservaDetalle == id).Reservas;
                //pone el libro en dehabilitado
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
                if (vistaReserva.ListaDetReserva.Count == 1)
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

                ViewBag.Message = "Se canceló el prestamo de este libro correctamente";
                return View("PrestarReserva", vistaReserva); //Retorna a la vista nueva reserva
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene una lista de valores a partir de los caracteres enviados, y los lista sólo los valores distintos (Autocompledato)
        /// </summary>
        /// <param name="term">Caracteres a consultar</param>
        /// <returns>Devuelve una lista en formato Json</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PrestamoReserva_Buscar_Persona)]
        public ActionResult GetPersona(string term)
        {
            List<string> dni;
            dni = db.Personas.Where(x => x.Dni.StartsWith(term)).Select(e => e.Dni).Distinct().ToList();

            return Json(dni, JsonRequestBehavior.AllowGet);
        }

    }
}