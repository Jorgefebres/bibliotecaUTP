using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using BIBLIOTECA.Etuiqueta;

namespace BIBLIOTECA.Controllers
{
    public class CategoriaController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envía a la vista una lista de categorías obteniendo de la base de datos
        /// </summary>
        /// <returns>Retorna una vista</returns>
        // GET: Categoria
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Categoria_Listar)]
        public ActionResult Index()
        {
            return PartialView(db.Categorias.ToList());
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene en detalles los datos del categoría con el identificador especificado
        /// </summary>
        /// <param name="id">Identificador de la categoía</param>
        /// <returns>Retora una vista parcial</returns>
        // GET: Categoria/Details/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Categoria_Ver_Detalles)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return PartialView(categoria);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envia una vista vacía
        /// </summary>
        /// <returns>Retorna una vista parcial</returns>
        // GET: Categoria/Create
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Categoria_Registrar)]
        public ActionResult Create()
        {
            return PartialView();
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Crea una nueva categoría en la base de datos
        /// </summary>
        /// <param name="categoria">Datos de la categoría</param>
        /// <returns>Redirecciona al Index de la categoría</returns>
        // POST: Categoria/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCategoria,NomCategoria,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categorias.Add(categoria);
                db.SaveChanges();
                return RedirectToAction("Index", "Categoria");
            }

            return RedirectToAction("Index", "Categoria");
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos de la categoía en base del indentificador especificado para editar
        /// </summary>
        /// <param name="id">Identificador de la categoría</param>
        /// <returns>Retorna Una vista Parcial</returns>
        // GET: Categoria/Edit/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Categoria_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return PartialView(categoria);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificacion de los datos de la categoía con los nuevo datos enviado desde la vista, en la base de datos
        /// </summary>
        /// <param name="categoria">Nuevos datos de la categoría</param>
        /// <returns>Retorna la vista con los nuevos datos de la categoía</returns>
        // POST: Categoria/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCategoria,NomCategoria,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(categoria);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos de la categoría en base del indentificador especificado para eliminar
        /// </summary>
        /// <param name="id">Identificador de la categoría</param>
        /// <returns>Retorna una vista parcial con los datos de la categoría a eliminar</returns>
        // GET: Categoria/Delete/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Categoria_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categoria = db.Categorias.Find(id);
            if (categoria == null)
            {
                return HttpNotFound();
            }
            return PartialView(categoria);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Confirma la eliminacion de la categoría en base a su identificador
        /// </summary>
        /// <param name="id">Identificador de la categoría</param>
        /// <returns>Redirecciona al Index de la categoría</returns>
        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categoria categoria = db.Categorias.Find(id);
            db.Categorias.Remove(categoria);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
