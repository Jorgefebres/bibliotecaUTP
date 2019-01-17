using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Models;
using System.Linq;
using System.Web.Mvc;

namespace BIBLIOTECA.Controllers
{
    public class PanelUsuarioController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.3
        /// <summary>
        /// Obtiene una lista de libro prestados por el lector de la base de datos
        /// </summary>
        /// <returns>Devuelve una vista con una lista de libros</returns>
        // GET: PanelUsuario
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.PanelLector_Index)]
        public ActionResult Index()
        {
            string errores = "";
            if (Session["ErrorLogin"] != null)
            {
                errores = Session["ErrorLogin"] as string;
            }
            ModelState.AddModelError("", errores);
            int idUsuario = Helper.SessionHelper.GetUser();
            int idpersona = db.Usuarios.Where(x => x.IdUsuario == idUsuario).SingleOrDefault().IdPersona;
            var libros = db.PrestamoDetalles.Where(x => x.Prestamos.Personas.IdPersona == idpersona);
            if (libros!=null)
            {
                return View(libros.ToList());
            }
            return View();
        }
    }
}