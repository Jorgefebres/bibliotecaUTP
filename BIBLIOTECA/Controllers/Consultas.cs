using BIBLIOTECA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Controllers
{

    public class Consultas
    {
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Verifica en la tabla sanciones, si la persona esta sancionada en base a su identificador
        /// Comparando la fecha de fin de sanción con la fecha actual
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <returns>Retorna verdadero o falso</returns>
        public bool sancionado(int id)
        {
            using (var db = new BIBLIOTECAContext())
            {
                List<Sancion> sanciones = db.Sancions.Where(x => x.Devoluciones.PrestamoDetalles.Prestamos.Personas.IdPersona == id).ToList();
                if (sanciones != null)
                {
                    foreach (var item in sanciones)
                    {
                        DateTime fechaInicio = item.FecInicio;
                        DateTime fechaFin = item.FecFin;
                        if (fechaInicio.Date <= DateTime.Now.Date && fechaFin.Date >= DateTime.Now.Date)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// si el metodo sancionado es verdadero, obtiene los datos necesarios de la sanción,
        /// en vase al identificador de la persona
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <returns>Retorna un objeto de tipo Sancion</returns>
        public Sancion sancion(int id)
        {
            Sancion sancion = new Sancion();
            if (sancionado(id))
            {
                using (var db = new BIBLIOTECAContext())
                {
                    List<Sancion> sanciones = db.Sancions.Where(x => x.Devoluciones.PrestamoDetalles.Prestamos.Personas.IdPersona == id).ToList();
                    if (sanciones != null)
                    {
                        foreach (var item in sanciones)
                        {
                            DateTime fechaInicio = item.FecInicio;
                            DateTime fechaFin = item.FecFin;
                            if (fechaInicio.Date <= DateTime.Now.Date && fechaFin.Date >= DateTime.Now.Date)
                            {
                                sancion.IdSancion = item.IdSancion;
                                sancion.Descripcion = item.Descripcion;
                                sancion.Estado = item.Estado;
                                sancion.FecInicio = item.FecInicio;
                                sancion.FecFin = item.FecFin;
                                sancion.Devoluciones = item.Devoluciones;
                            }
                        }
                    }
                }
            }
            return sancion;
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Actualiza la tabla reservas, si la fecha de limete de la reserva ya caducó,
        /// pone en disponible el libro reservado,
        /// y cambian el campo pendiente de la tabla ReservaDetalles y Reservas en false
        /// </summary>
        public void actualizarReservas()
        {
            using (var db=new BIBLIOTECAContext()) {
                var reservas = db.Reservas.Where(x => x.Pendiente && x.FecRecojo < DateTime.Now);
                if (reservas!= null)
                {
                    reservas.ToList();
                    foreach (var item in reservas)
                    {
                        var detReservas = db.ReservaDetalles.Where(l => l.Pendiente && l.IdReserva == item.IdReserva).ToList();
                        foreach (var i in detReservas)
                        {
                            var detReserva = new ReservaDetalle()
                            {
                                IdReservaDetalle = i.IdReservaDetalle,
                                IdReserva = i.IdReserva,
                                IdLibro = i.IdLibro,
                                Pendiente = false
                            };
                            db.Entry(detReserva).State = EntityState.Modified;
                            var libro = new Libro()
                            {
                                IdLibro = i.Libros.IdLibro,
                                Titulo = i.Libros.Titulo,
                                Idioma = i.Libros.Idioma,
                                ISBN = i.Libros.ISBN,
                                IdAutor = i.Libros.IdAutor,
                                IdCategoria = i.Libros.IdCategoria,
                                IdEditorial = i.Libros.IdEditorial,
                                NumEdicion = i.Libros.NumEdicion,
                                Anio = i.Libros.Anio,
                                NumPaginas = i.Libros.NumPaginas,
                                Estado = i.Libros.Estado,
                                Caratula = i.Libros.Caratula,
                                Disponibilidad = true //Modificacion de la diponibilidad del libro
                            };
                            db.Entry(libro).State = EntityState.Modified;
                        }
                        if (detReservas.Count == 1)
                        {
                            var reserva = new Reserva()
                            {
                                IdReserva = item.IdReserva,
                                FecReserva = item.FecReserva,
                                FecRecojo = item.FecRecojo,
                                Pendiente = false,
                                IdUsuario = item.IdUsuario
                            };
                            db.Entry(reserva).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                }
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: BIBLIOTECA.0.9
        /// <summary>
        /// Obtiene la cantidad de libros prestados pendientes en base al identificador de la persona
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <returns>Retorna un entero</returns>
        public int librosPrestados(int id)
        {
            using (var db = new BIBLIOTECAContext())
            {
                var detaPrestamo = db.PrestamoDetalles.Where(x => x.Pendiente && x.Prestamos.Personas.IdPersona == id).Count();
                return detaPrestamo;
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad de libros reservados pendientes en base al identificador del lector
        /// </summary>
        /// <param name="id">Identificador del lector</param>
        /// <returns>entero</returns>
        public int librosReservados(int id)
        {
            using (var db = new BIBLIOTECAContext())
            {
                var detaReservador = db.ReservaDetalles.Where(x => x.Pendiente && x.Reservas.IdUsuario == id).Count();
                return detaReservador;
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad total de reservas pendientes
        /// </summary>
        /// <returns>entero</returns>
        public int ContarReservas()
        {
            using (var db = new BIBLIOTECAContext())
            {
                return db.Reservas.Where(tmp => tmp.Pendiente == true).Count();
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad de sanciones que están pendientes
        /// </summary>
        /// <returns>Entero</returns>
        public int ContarSancion()
        {
            using (var db = new BIBLIOTECAContext())
            {
                return db.Sancions.Where(tmp => tmp.FecFin >=DateTime.Now).Count();
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad totas de prestamos pendientes
        /// </summary>
        /// <returns>entero</returns>
        public int ContarPrestamos()
        {
            using (var db = new BIBLIOTECAContext())
            {
                return db.Prestamos.Where(tmp => tmp.Pendiente == true).Count();
            }
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad total de libros prestados pendientes
        /// </summary>
        /// <returns>entero</returns>
        public int ContarDetaprestamos()
        {
            using (var db = new BIBLIOTECAContext())
            {
                return db.PrestamoDetalles.Where(tmp => tmp.Pendiente == true).Count();
            }
        }
    }
}