using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using BIBLIOTECA.Etuiqueta;
using System;

namespace BIBLIOTECA.Controllers
{
    public class EditorialController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envía a la vista una lista de editoriales obteniendo de la base de datos
        /// </summary>
        /// <returns>vista con una lista de editoriales</returns>
        // GET: Editorial
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Editorial_Listar)]
        public ActionResult Index()
        {
            return PartialView(db.Editoriales.ToList());
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene en detalles los datos del editorial con el identificador especificado
        /// </summary>
        /// <param name="id">Identificador del editorial</param>
        /// <returns>Devulve una vista parcial</returns>
        // GET: Editorial/Details/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Editorial_Ver_Detalles)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editorial editorial = db.Editoriales.Find(id);
            if (editorial == null)
            {
                return HttpNotFound();
            }
            return PartialView(editorial);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Solo envía una vista vacía
        /// </summary>
        /// <returns>Devuelve una vista parcial</returns>
        // GET: Editorial/Create
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Editorial_Registrar)]
        public ActionResult Create()
        {
            return PartialView();
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Crea un nuevo editorial en la base de datos
        /// </summary>
        /// <param name="editorial">Datos del editorial</param>
        /// <returns>Redirecciona al Index del editorial</returns>
        // POST: Editorial/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEditorial,NomEditorial,Pais,Ciudad,Email,Url")] Editorial editorial)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Editoriales.Add(editorial);
                    db.SaveChanges();

                }
                catch (Exception)
                {

                    return RedirectToAction("Index", "Editorial");
                }
                return RedirectToAction("Index", "Editorial");
            }

            return RedirectToAction("Index", "Editorial");
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del editorial en base del indentificador especificado, para editar
        /// </summary>
        /// <param name="id">Identificador del editorial</param>
        /// <returns>Devuelve una vista Parcial con los datos del editorial</returns>
        // GET: Editorial/Edit/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Editorial_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editorial editorial = db.Editoriales.Find(id);
            if (editorial == null)
            {
                return HttpNotFound();
            }
            return PartialView(editorial);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificacion de los datos del editorial con los nuevo datos enviado desde la vista
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="editorial">Nuevos datos de la categoría</param>
        /// <returns>Devuelve la vista con los nuevos datos del editorial</returns>
        // POST: Editorial/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEditorial,NomEditorial,Pais,Ciudad,Email,Url")] Editorial editorial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(editorial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(editorial);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del editorial en base del indentificador especificado para eliminar
        /// </summary>
        /// <param name="id">Identificador del editorial</param>
        /// <returns>Devuelve una vista parcial con los datos del editorial a eliminar</returns>
        // GET: Editorial/Delete/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Editorial_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Editorial editorial = db.Editoriales.Find(id);
            if (editorial == null)
            {
                return HttpNotFound();
            }
            return PartialView(editorial);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Confirma la eliminacion del editorial en base a su identificador
        /// </summary>
        /// <param name="id">Identificador del editorial</param>
        /// <returns>Redirecciona al Index del editorial</returns>
        // POST: Editorial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Editorial editorial = db.Editoriales.Find(id);
            db.Editoriales.Remove(editorial);
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
