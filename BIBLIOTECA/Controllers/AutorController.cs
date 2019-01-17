using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using BIBLIOTECA.Etuiqueta;

namespace BIBLIOTECA.Controllers
{
    public class AutorController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();


        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envía a la vista una lista de autores obteniendo de la base de datos
        /// </summary>
        /// <returns>Retorna una vista con una lista de autores</returns>
        //GET: Autor/Index
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Autor_Listar)]
        public ActionResult Index()
        {
            var autores = db.Autores;
            if (autores!=null)
            {
                return View(autores.ToList());
            }
            return View();
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene en detalles los datos del autor con el identificador especificado
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <returns>Retora una vista parcial</returns>
        // GET: Autor/Details/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Autor_Ver_Detalles)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return PartialView(autor);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envia una vista vacía
        /// </summary>
        /// <returns>Retorna una vista parcial</returns>
        // GET: Autor/Create
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Autor_Registrar_nuevo)]
        public ActionResult Create()
        {
            return PartialView();
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Crea un nuevo autor en la base de datos
        /// </summary>
        /// <param name="autor">Datos del autor</param>
        /// <returns>Redirecciona al Index del Autor</returns>
        // POST: Autor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAutor,Apellidos,Nombres,Sexo,FecNacimiento,Pais")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Autores.Add(autor);
                db.SaveChanges();
                return RedirectToAction("Index", "Autor");
            }

            return RedirectToAction("Index", "Autor");
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del autor en base del indentificador especificado para editar
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <returns>Retorna Una vista Parcial con los datos del autor</returns>
        // GET: Autor/Edit/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Autor_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return PartialView(autor);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificacion de los datos del autor con los nuevo datos enviado desde la vista, en la base de datos
        /// </summary>
        /// <param name="autor">Nuevos datos del Autor</param>
        /// <returns>Retorna la vista con los nuevos datos del autor</returns>
        // POST: Autor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAutor,Apellidos,Nombres,Sexo,FecNacimiento,Pais")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(autor);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del autor en base del indentificador especificado para eliminar
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <returns>Retorna una vista parcial con los datos del autor a eliminar</returns>
        // GET: Autor/Delete/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Autor_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autor autor = db.Autores.Find(id);
            if (autor == null)
            {
                return HttpNotFound();
            }
            return PartialView(autor);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Confirma la eliminacion del autor en base a su identificador
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <returns>Redirecciona al Index del autor</returns>
        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Autor autor = db.Autores.Find(id);
            db.Autores.Remove(autor);
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
