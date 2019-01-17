using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models.Estadisticas
{
    public class LibrosDisponibles
    {
        public string Campos { get; set; }
        public int valor { get; set; }
       
    }
    public class PrestamosPorMes
    {
        public string Mes { get; set; }
        public int Cantidad { get; set; }

    }

    public class EstadisticaModel
    {
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene la cantidad de libro disponibles,
        /// libros prestados y libros reserbados
        /// los guarda en una lista
        /// </summary>
        /// <returns>devuele una lista de datos</returns>
        public List<LibrosDisponibles> all()
        {
            using (var db = new BIBLIOTECAContext())
            {
                return new List<LibrosDisponibles>
            {
                new LibrosDisponibles
                {
                    Campos = "Prestados ["+db.PrestamoDetalles.Where(x => x.Pendiente).Count()+"]",
                    valor = db.PrestamoDetalles.Where(x => x.Pendiente).Count()
                },
                new LibrosDisponibles
                {
                    Campos="Reserdos [" +db.ReservaDetalles.Where(x=>x.Pendiente).Count()+"]",
                    valor=db.ReservaDetalles.Where(x=>x.Pendiente).Count()
                },
                new LibrosDisponibles
                {
                    Campos="Libros Disponibles ["+db.Libros.Where(x=>x.Disponibilidad).Count()+"]",
                    valor=db.Libros.Where(x=>x.Disponibilidad).Count()
                }
            };
            }
        }

        public List<PrestamosPorMes> allMes()
        {

            using (var db = new BIBLIOTECAContext())
            {
                return new List<PrestamosPorMes>
            {
                new PrestamosPorMes
                {
                    Mes = "Enero",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==1&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Febrero",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==2&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Marzo",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==3&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Abril",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==4&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Mayo",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==5&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Junio",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==6&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Julio",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==7&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Agosto",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==8&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Septiembre",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==9&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Octubre",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==10&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Noviembre",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==11&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                },
                new PrestamosPorMes
                {
                    Mes = "Diciembre",
                    Cantidad = db.PrestamoDetalles.Where(x => x.Prestamos.FecPrestamo.Month==12&&x.Prestamos.FecPrestamo.Year==DateTime.Now.Year).Count()
                }
              };
            }
        }
    }



}