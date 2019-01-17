using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using BIBLIOTECA.Models;
using Microsoft.Reporting.WebForms;
using BIBLIOTECA.Reports;
using System;

namespace BIBLIOTECA.Controllers
{

    public class ReportesController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        // GET: Reportes
        public ActionResult Index()
        {
            return View("Error");

        }
        public ActionResult LibrosDisponibles()
        {
            var Libros = db.Libros.Where(x => x.Disponibilidad).ToList();
            if (Libros != null)
            {
                
                List<LibrosDisponibles> Libros_d = new List<LibrosDisponibles>();
                int i = 1;
                foreach (var item in Libros)
                {
                    LibrosDisponibles ld = new LibrosDisponibles
                    { 
                        id=i,
                        ISBN=item.ISBN,
                        Titulo=item.Titulo,
                        Autor=item.Autores.NombreCompleto,
                        Categoria=item.Categorias.NomCategoria,
                        Editorial=item.Editoriales.NomEditorial
                    };
                    Libros_d.Add(ld);
                    i++;
                }
                
                //declarar el objeto de la clase LocalReport
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Report_Libros_Disponibles.rdlc");
                //Preparar el DataSource del reporte
                //aquí invocamos al método ListaAlumnos que agregamos a la clase Alumno
                //pasandole como valor de parámetro el id del grupo que se quiere imprimir
                //No hemos hablado de vistas o procedimientos almacenados, sino, aquí se invocarían directamente
                ReportDataSource reportDataSource = new ReportDataSource("DS_LibrosDisponibles", Libros_d);
                //Ahora pasamos los datos al reporte
                localReport.DataSources.Add(reportDataSource);
                //declaramos las variables de configuración para el reporte
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                //En la configuración del reporte debe especificarse para el tipo de reporte
                //consulte el sitio para más información
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                //Transformar el reporte a bytes para transferirlo como flujo
                renderedBytes = localReport.Render(reportType, null, out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
                //enviar el archivo al cliente (navegador)
                return File(renderedBytes, mimeType);
            }
            else
                return View("Error");
        }
        public ActionResult LibrosPrestamo()
        {
            var detPrestamo = db.PrestamoDetalles.Where(x => x.Pendiente).ToList();
            if (detPrestamo.Count > 0)
            {
                List<LibrosPrestados> Libros_p = new List<LibrosPrestados>();
                int i = 1;
                foreach (var item in detPrestamo)
                {
                    var lp = new LibrosPrestados
                    {
                        id = i,
                        ISBN = item.Libros.ISBN,
                        Titulo = item.Libros.Titulo,
                        FecPretamo=item.Prestamos.FecPrestamo.ToShortDateString(),
                        FecDevolucion=item.FecDevolucion.ToShortDateString(),
                        DNI=item.Prestamos.Personas.Dni,
                        Nombres=item.Prestamos.Personas.NombreCompleto,
                        Bibliotecario=item.Prestamos.Usuarios.Personas.NombreCompleto,
                        Estado=item.EstLibro
                    };
                    Libros_p.Add(lp);
                    i++;
                }

                //declarar el objeto de la clase LocalReport
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Report_En_Prestamo.rdlc");
                //Preparar el DataSource del reporte
                //aquí invocamos al método ListaAlumnos que agregamos a la clase Alumno
                //pasandole como valor de parámetro el id del grupo que se quiere imprimir
                //No hemos hablado de vistas o procedimientos almacenados, sino, aquí se invocarían directamente
                ReportDataSource reportDataSource = new ReportDataSource("DS_LibrosPrestados", Libros_p);
                //Ahora pasamos los datos al reporte
                localReport.DataSources.Add(reportDataSource);
                //declaramos las variables de configuración para el reporte
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                //En la configuración del reporte debe especificarse para el tipo de reporte
                //consulte el sitio para más información
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                //Transformar el reporte a bytes para transferirlo como flujo
                renderedBytes = localReport.Render(reportType, null, out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
                //enviar el archivo al cliente (navegador)
                return File(renderedBytes, mimeType);
            }
            else
                return View("Error");
        }
        public ActionResult LibrosReserva()
        {
            var detReservas = db.ReservaDetalles.Where(x => x.Pendiente);
            if (detReservas.Count()>0)
            {
                List<LibrosReservados> Libros_r = new List<LibrosReservados>();
                int i = 1;
                foreach (var item in detReservas.ToList())
                {
                    var lr = new LibrosReservados
                    {
                        id = i,
                        ISBN = item.Libros.ISBN,
                        Titulo = item.Libros.Titulo,
                        FecReserva = item.Reservas.FecReserva.ToShortDateString(),
                        FecRecojo = item.Reservas.FecRecojo.ToShortDateString(),
                        DNI = item.Reservas.Usuarios.Personas.Dni,
                        Nombres = item.Reservas.Usuarios.Personas.NombreCompleto,
                        };
                    Libros_r.Add(lr);
                    i++;
                }

                //declarar el objeto de la clase LocalReport
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Report_En_Reserva.rdlc");
                //Preparar el DataSource del reporte
                //aquí invocamos al método ListaAlumnos que agregamos a la clase Alumno
                //pasandole como valor de parámetro el id del grupo que se quiere imprimir
                //No hemos hablado de vistas o procedimientos almacenados, sino, aquí se invocarían directamente
                ReportDataSource reportDataSource = new ReportDataSource("DS_LibrosReservados", Libros_r);
                //Ahora pasamos los datos al reporte
                localReport.DataSources.Add(reportDataSource);
                //declaramos las variables de configuración para el reporte
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                //En la configuración del reporte debe especificarse para el tipo de reporte
                //consulte el sitio para más información
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                //Transformar el reporte a bytes para transferirlo como flujo
                renderedBytes = localReport.Render(reportType, null, out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
                //enviar el archivo al cliente (navegador)
                return File(renderedBytes, mimeType);
            }
            else
                return View("Error");
        }
        public ActionResult LibrosDevolver()
        {
            var Devolver = db.PrestamoDetalles.Where(x => x.FecDevolucion < DateTime.Now);
            if (Devolver.Count() > 0)
            {
                List<LibrosFueraTiempo> Libros_f = new List<LibrosFueraTiempo>();
                int i = 1;
                foreach (var item in Devolver.ToList())
                {
                    var lf = new LibrosFueraTiempo
                    {
                        id = i,
                        ISBN = item.Libros.ISBN,
                        Titulo = item.Libros.Titulo,
                        FecPretamo = item.Prestamos.FecPrestamo.ToShortDateString(),
                        FecDevolucion = item.FecDevolucion.ToShortDateString(),
                        DNI = item.Prestamos.Personas.Dni,
                        Nombres = item.Prestamos.Personas.NombreCompleto,
                        Bibliotecario = item.Prestamos.Usuarios.Personas.NombreCompleto,
                        Estado = item.EstLibro,
                        Retardo = (DateTime.Now.Date - item.FecDevolucion.Date).Days
                    };
                    Libros_f.Add(lf);
                    i++;
                }

                //declarar el objeto de la clase LocalReport
                LocalReport localReport = new LocalReport();
                localReport.ReportPath = Server.MapPath("~/Reports/Report_Retardados.rdlc");
                //Preparar el DataSource del reporte
                //aquí invocamos al método ListaAlumnos que agregamos a la clase Alumno
                //pasandole como valor de parámetro el id del grupo que se quiere imprimir
                //No hemos hablado de vistas o procedimientos almacenados, sino, aquí se invocarían directamente
                ReportDataSource reportDataSource = new ReportDataSource("DS_LibrosRetardados", Libros_f);
                //Ahora pasamos los datos al reporte
                localReport.DataSources.Add(reportDataSource);
                //declaramos las variables de configuración para el reporte
                string reportType = "PDF";
                string mimeType;
                string encoding;
                string fileNameExtension;
                //En la configuración del reporte debe especificarse para el tipo de reporte
                //consulte el sitio para más información
                //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
                Warning[] warnings;
                string[] streams;
                byte[] renderedBytes;
                //Transformar el reporte a bytes para transferirlo como flujo
                renderedBytes = localReport.Render(reportType, null, out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
                //enviar el archivo al cliente (navegador)
                return File(renderedBytes, mimeType);
            }
            else
                return View("Error");
        }
    }

}
