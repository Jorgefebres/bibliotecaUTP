using BIBLIOTECA.Models;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;

namespace BIBLIOTECA.Controllers
{
    public class CatalogoController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        /// Autor: Febres Cabrera Jorge
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos del libro incluyendo su Autor, Categoria y Editorial para mostrar en el catálogo de libros
        /// </summary>
        /// <returns>Retorna una vista con una lista de libros</returns>
        // GET: Catalogo en genral ver catalogo
        public ActionResult Index()
        {   var Libros = db.Libros.Include(l => l.Autores).Include(l => l.Categorias).Include(l => l.Editoriales);
            return View(Libros.ToList());
        }
    }
}