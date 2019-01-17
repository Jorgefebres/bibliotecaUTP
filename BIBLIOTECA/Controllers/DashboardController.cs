using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using System.IO;
using BIBLIOTECA.Models.Metodos;
using System;
using BIBLIOTECA.Etuiqueta;
using System.Data.SqlClient;
using BIBLIOTECA.Models.Estadisticas;
using System.Collections.Generic;
using System.Collections;
using System.Web.Helpers;

namespace BIBLIOTECA.Controllers
{
    [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PanelBibliotecario_Index)]
    public class DashboardController : Controller
    {
        /// <summary>
        /// Crea una nueva instacia del Contexto
        /// </summary>
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        /// <summary>
        /// Crea una nueva instacia del modelo de estadisticas
        /// </summary>
        EstadisticaModel data = new EstadisticaModel();

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Devuelve una vista vacia
        /// </summary>
        /// <returns>vista</returns>
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Genera un gráfico de tipo Pie, con datos especificados, (x,y)
        /// </summary>
        /// <returns>Retorna Null, solo se ejecuta la función desde la vista</returns>
        public ActionResult Chart()
        {
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();
            var list = data.all();
            list.ForEach(x => xvalue.Add(x.Campos));
            list.ForEach(x => yvalue.Add(x.valor));

            new Chart(width: 600, height: 400, theme: ChartTheme.Green)
                .AddTitle("Libros Disponibles de ["+db.Libros.Count()+"]")
                .AddSeries("Default", chartType: "Pie", xValue: xvalue, yValues: yvalue).Write("bmp");
            return null;
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: BIBLIOTECA.0.9
        /// <summary>
        /// Genera un gráfico de tipo Column, con datos especificados, (x,y)
        /// </summary>
        /// <returns>Retorna Null, solo se ejecuta la función desde la vista</returns>
        public ActionResult ChartColumn()
        {
            
            ArrayList xvalue = new ArrayList();
            ArrayList yvalue = new ArrayList();
            var list = data.allMes();
            list.ForEach(x => xvalue.Add(x.Mes));
            list.ForEach(x => yvalue.Add(x.Cantidad));

            new Chart(width: 800, height: 400, theme: ChartTheme.Blue)
                .AddTitle("Préstamos Por Mes")
                .AddSeries("Default", chartType: "Column", xValue: xvalue, yValues: yvalue).Write("bmp");
            return null;
        }
    }
    
}