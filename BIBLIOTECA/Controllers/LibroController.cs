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

namespace BIBLIOTECA.Controllers
{
    public class LibroController : Controller
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        private Funciones fc = new Funciones();

        private string mensaje;

        [NonAction]
        public string Mesaje()
        {
            return mensaje;
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Envía a la vista una lista de libros obteniendo de la base de datos
        /// </summary>
        /// <returns>vista con una lista de libros</returns>
        // GET: Libro
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Libro_Listar)]
        public ActionResult Index()
        {
            var Libros = db.Libros.Include(l => l.Autores).Include(l => l.Categorias).Include(l => l.Editoriales);
            return PartialView(Libros.ToList());
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene en detalles los datos del libro con el identificador especificado
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devulve una vista parcial</returns>
        // GET: Libro/Details/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Libro_Ver_Detalles)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return PartialView(libro);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Solo envía una vista vacía
        /// </summary>
        /// <returns>Devuelve una vista</returns>
        // GET: Libro/Create
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Libro_Registrar)]
        public ActionResult Create()
        {
            var autor = db.Autores;
            var categoria = db.Categorias;
            var editorial = db.Editoriales;

            if (autor.Count() < 1 || categoria.Count() < 1 || editorial.Count() < 1)
            {
                ViewBag.idAutor = new SelectList(autor, "idAutor", "NombreCompleto");
                ViewBag.idCategoria = new SelectList(categoria, "idCategoria", "NomCategoria");
                ViewBag.idEditorial = new SelectList(editorial, "idEditorial", "NomEditorial");
                mensaje = "Primero ingrese Autor, Categoria y editorial para registrar Libro";
                ModelState.AddModelError("", mensaje);
                return PartialView();
            }
            ViewBag.idAutor = new SelectList(autor, "idAutor", "NombreCompleto");
            ViewBag.idCategoria = new SelectList(categoria, "idCategoria", "NomCategoria");
            ViewBag.idEditorial = new SelectList(editorial, "idEditorial", "NomEditorial");
            mensaje = "Puede que no exista Autor,Categoría o Edotirial de este libro registre en la parte inferior";
            ViewBag.Exito = mensaje;

            return PartialView();
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Crea un nuevo libro en la base de datos, con los datos enviados desde la vista
        /// </summary>
        /// <param name="libro">datos del libro</param>
        /// <param name="CaratulaLibro">nombre de la caratula</param>
        /// <returns>Si se inserta correctamente, redirecciona al index del libro, si no permanece</returns>
        // POST: Libro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdLibro,ISBN,Titulo,Idioma,NumEdicion,Anio,NumPaginas,Estado,Disponibilidad,Caratula,IdAutor,IdCategoria,IdEditorial")] Libro libro, HttpPostedFileBase CaratulaLibro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string nombre = fc.NombreCaratula(CaratulaLibro, libro.ISBN); //genera NameFile de la imagen
                    string path = Path.Combine(Server.MapPath(@"~/Models/Image/Libros"), nombre);//genra la direccion donde se guardara la imagen
                    CaratulaLibro.SaveAs(path);//guarda la imagen

                    libro.Caratula = nombre;//agrega el nombre de la imagen
                    db.Libros.Add(libro);
                    db.SaveChanges();
                    mensaje = "Libro Correctamente insertado";
                    ViewBag.Exito = mensaje;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {

                mensaje = "Algo salio mal en la base de datos " +e.Message;
                ModelState.AddModelError("", mensaje);
            }

            ViewBag.IdAutor = new SelectList(db.Autores, "IdAutor", "Apellidos", libro.IdAutor);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "NomCategoria", libro.IdCategoria);
            ViewBag.IdEditorial = new SelectList(db.Editoriales, "IdEditorial", "NomEditorial", libro.IdEditorial);
            return View(libro);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del libro en base del indentificador especificado, para editar
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve una vista Parcial con los datos del editorial</returns>
        // GET: Libro/Edit/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Libro_Editar)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAutor = new SelectList(db.Autores, "IdAutor", "Apellidos", libro.IdAutor);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "NomCategoria", libro.IdCategoria);
            ViewBag.IdEditorial = new SelectList(db.Editoriales, "IdEditorial", "NomEditorial", libro.IdEditorial);
            return PartialView(libro);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificacion de los datos del libro con los nuevo datos enviado desde la vista
        /// </summary>
        /// <param name="libro">datos del libro</param>
        /// <param name="CaratulaLibro">nombre de la caratula</param>
        /// <returns>Devuelve una vista</returns>
        // POST: Libro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLibro,ISBN,Titulo,Idioma,NumEdicion,Anio,NumPaginas,Estado,Disponibilidad,Caratula,IdAutor,IdCategoria,IdEditorial")] Libro libro, HttpPostedFileBase CaratulaLibro)
        {
            if (ModelState.IsValid)
            {
                //funcion que modifique la caratula
                if (CaratulaLibro != null)
                {
                    string ISBN = libro.ISBN; //genera codigo ISBN
                    string nombre = fc.NombreCaratula(CaratulaLibro, ISBN); //genera NameFile de la imagen
                    string path = Path.Combine(Server.MapPath(@"~/Models/Image/Libros"), nombre);//genra la direccion donde se guardara la imagen
                    CaratulaLibro.SaveAs(path);//guarda la imagen
                    libro.Caratula = nombre;
                }
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAutor = new SelectList(db.Autores, "IdAutor", "Apellidos", libro.IdAutor);
            ViewBag.IdCategoria = new SelectList(db.Categorias, "IdCategoria", "NomCategoria", libro.IdCategoria);
            ViewBag.IdEditorial = new SelectList(db.Editoriales, "IdEditorial", "NomEditorial", libro.IdEditorial);
            return View(libro);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Obtiene los datos del libro en base del indentificador especificado para eliminar
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Devuelve una vista parcial con los datos del libro a eliminar</returns>
        // GET: Libro/Delete/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Libro_Eliminar)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = db.Libros.Find(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return PartialView(libro);
        }

        /// Autor: Quispe Luizar Franklin Antoni
        /// Version: 0.1
        /// <summary>
        /// Confirma la eliminacion del libro en base a su identificador
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>Redirecciona al Index del libro</returns>
        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Libro libro = db.Libros.Find(id);
            db.Libros.Remove(libro);
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
