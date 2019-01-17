using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.Estadisticas;
using System.Collections;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers
{
    [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Admin_Index)]
    public class AdminController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        EstadisticaModel data = new EstadisticaModel();

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Index del administrador
        /// </summary>
        /// <returns>Devuelve la vista de Index</returns>
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
                .AddTitle("Libros Disponibles de [" + db.Libros.Count() + "]")
                .AddSeries("Default", chartType: "Pie", xValue: xvalue, yValues: yvalue).Write("bmp");
            return null;
        }
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
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