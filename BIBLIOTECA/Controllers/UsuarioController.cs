using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BIBLIOTECA.Models;
using System.Net;
using System.Web.Security;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using BIBLIOTECA.Helper;
using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Models.Emuns;

namespace BIBLIOTECA.Controllers
{
    public class UsuarioController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene todos los usuarios de la base de datos
        /// </summary>
        /// <returns>Devuelve la vista con una lista de Usuarios</returns>
        // GET: Index
        [PermisoAtributo(IdAccion =Models.Acciones.AccionesEnum.Usuario_Listar)]
        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Personas);
            string mensaje=Session["Mensaje"] as string;
            string error=Session["Error"] as string;
            if (!string.IsNullOrEmpty(mensaje))
            {
                ViewBag.mensaje = mensaje;
            }
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.error = error;
            }
            return View(usuarios.ToList());
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Vista para iniciar sesión
        /// </summary>
        /// <returns>Devuelve una vista parcial vacia </returns>
        // GET: Login
        public ActionResult Login()
        {
            Session["ErrorLogin"] = null;
            return PartialView("Login");
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Consulta en la base de datos si este usuario tiene axeso al sistema mediante el usuario y contreseña
        /// </summary>
        /// <param name="user">datos del usuario</param>
        /// <returns>Redirecciona a la vista que le corresponde segun su rol del usuario</returns>
        // Post: Login
        [HttpPost]
        public ActionResult Login(Usuario user)
        {
            string us = user.NomUsuario;
            string pass = user.Encriptar(user.Contrasena);
            //Verifica si existes el usuarios
            long de = db.Usuarios.LongCount();
            if (de > 0)
            {
                Consultas cn = new Consultas();
                cn.actualizarReservas();    //Actualiza las reservas, si ya se pasaron la fecha de recojo

                Usuario v = db.Usuarios.Where(x => x.NomUsuario == us && x.Contrasena == pass &&x.Estado).SingleOrDefault();
                if (v != null)
                {
                    //Guarda los datos necesarios en cokie
                    SessionHelper.AddUserToSession(v.Personas.NombreCompleto, v.IdUsuario.ToString());

                    if ((int)RolEnums.Administrador == v.IdRol)
                    {
                        return RedirectToAction("Index", "Admin");                      //Si eres Superusuario al panel de SuperUsuario
                    }
                    else if ((int)RolEnums.Bibliotecario == v.IdRol)
                    {
                        return RedirectToAction("Index", "Dashboard");                      //Si eres lector te lleva a otro panel
                    }
                    else if ((int)RolEnums.Lector== v.IdRol)
                    {
                        return RedirectToAction("Index", "PanelUsuario");                      //Si eres lector te lleva a otro panel
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");                              //Si eres Bibliotecario te lleva al panel de Bibliotecario
                    }

                }
                else Session["ErrorLogin"] = "Datos de acceso incorrectos! o Ud. Está deshabilitado";                //Error cuando los credenciales son incorrectos
            }
            return RedirectToAction("Index", "Home");                                       //Devuelve al Inicio para que intentes de nuevo
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Vista para crear un nuevo usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Crea un nuevo usuario mediante el DNI de la persona que ya existe en la base de datos
        /// </summary>
        /// <param name="usuario">Datos del nuevo usuario</param>
        /// <param name="DNI">DNI de la persona</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,IdRol,IdPersona,NomUsuario,Contrasena,Estado,ConfirmarContrasena")] Usuario usuario, string DNI)
        {
            string error = "";
            var persona = db.Personas.Where(u => u.Dni == DNI).FirstOrDefault();
            if (persona == null)
            {
                Session["ErrorLogin"] = "No Existe DNI, Ingrese Un Dni Correcto o Registrese";
                return RedirectToAction("Index", "Home");
            }
            var use = db.Usuarios.Where(x => x.NomUsuario==usuario.NomUsuario).SingleOrDefault();
            if (use!=null)
            {
                Session["ErrorLogin"] = "Este cuanta de usuario ya existe, Intente con otro";
                return RedirectToAction("Index", "Home");
            }
            try
            {
                usuario.Contrasena = usuario.Encriptar(usuario.Contrasena);
                usuario.ConfirmarContrasena = usuario.Encriptar(usuario.ConfirmarContrasena);
                usuario.IdRol = 3;
                usuario.Estado = true;
                usuario.IdPersona = persona.IdPersona;
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                Session["ErrorLogin"] = null;
                SessionHelper.AddUserToSession(usuario.Personas.NombreCompleto, usuario.IdUsuario.ToString());
                if ((int)RolEnums.Administrador ==usuario.IdRol)
                {
                    return RedirectToAction("Index", "Admin");                      //Si eres Superusuario al panel de SuperUsuario
                }
                else if ((int)RolEnums.Bibliotecario == usuario.IdRol)
                {
                    return RedirectToAction("Index", "Dashboard");                      //Si eres lector te lleva a otro panel
                }
                else if ((int)RolEnums.Lector == usuario.IdRol)
                {
                    return RedirectToAction("Index", "PanelUsuario");                      //Si eres lector te lleva a otro panel
                }
                else
                {
                    return RedirectToAction("Index", "Home");                   //Si eres Bibliotecario te lleva al panel de Bibliotecario
                }
            }
            catch (DbUpdateException ed)
            {
                Session["ErrorLogin"] = "Ud ya tiene una cuenta de usuario";
            }
            catch (DbEntityValidationException e)
            {
                error = "las contraseñas no coenciden";
                Session["ErrorLogin"] = error;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Session["ErrorLogin"] = "Algo salio mal en la Base de Datos \n\r" + e;
            }
            return RedirectToAction("Index", "Home");
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos necesarios para su perfil mediante el identificador del usuario
        /// </summary>
        /// <param name="id">identificador del usuario</param>
        /// <returns>Devuele la vista con los datos del usuario</returns>
        // GET: Perfil
        [PermisoAtributo(IdAccion =Models.Acciones.AccionesEnum.Usuario_Entrar_al_Perfil)]
        public ActionResult Perfil(int id)
        {
            Usuario usuarios = db.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefault();
            return View(usuarios);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos del usuario de la dase de datos mediante el identificador del usuario
        /// para editar nombre del usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Devuelve vista parcial con los datos del usuario</returns>
        // GET: Usuario/Edit/5
        [PermisoAtributo(IdAccion =Models.Acciones.AccionesEnum.Usuario_Editar_NomUsuario_Contrasena)]
        [HttpGet, ActionName("Edit")]
        public ActionResult VistaEditarNombre(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Error!!!");
                return PartialView();
            }
            Usuario usuario = db.Usuarios.Find(id);
            Session["UsuarioModific"] = usuario;
            if (usuario == null)
            {
                ModelState.AddModelError("", "Error!!!");
                return PartialView();
            }
            return PartialView(usuario);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificación del nombre del usuario en la base de datos verificando la contresañe correspondiente
        /// </summary>
        /// <param name="usuarios">Datos del usuario</param>
        /// <returns>Redirecciona a la vista que le corresponde segun el rol del usuario</returns>
        // POST: Usuario/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditarNombre(Usuario usuarios)
        {
            string Dashboard;
            if ((int)RolEnums.Administrador==usuarios.IdRol)
            {
                Dashboard = "Admin";                      //Si eres Superusuario al panel de SuperUsuario
            }
            else if ((int)RolEnums.Bibliotecario==usuarios.IdRol)
            {
                Dashboard = "Dashboard";                      //Si eres lector te lleva a otro panel
            }
            else if ((int)RolEnums.Lector== usuarios.IdRol)
            {
                Dashboard = "PanelUsuario";                      //Si eres lector te lleva a otro panel
            }
            else
            {
                Dashboard = "Home";                   //Si eres Bibliotecario te lleva al panel de Bibliotecario
            }

            var usuarioS = Session["UsuarioModific"];
            string nonUnser = ((Usuario)usuarioS).NomUsuario;

            int usu = db.Usuarios.Count(x => x.NomUsuario == usuarios.NomUsuario); //Verificar si existe este usuario
            if ((usu < 1 || nonUnser == usuarios.NomUsuario))
            {
                string conen = usuarios.Encriptar(usuarios.Contrasena);
                bool veirificacion = db.Usuarios.Where(s => s.Contrasena == conen && s.NomUsuario == nonUnser).Any();//Verifica si coincide la contraseña del usuario
                if (veirificacion)
                {
                    try
                    {
                        usuarios.ConfirmarContrasena = usuarios.Contrasena;
                        db.Entry(usuarios).State = EntityState.Modified;
                        db.SaveChanges();
                        Session["UsuarioModific"] = null;
                        return RedirectToAction("Index", Dashboard);
                    }
                    catch (DbEntityValidationException e)
                    {
                        ModelState.AddModelError("", "Las contraseñas no coenciden!!! " + e.Message);
                        return View("Perfil", usuarios);
                    }
                    catch (DbUpdateException ed)
                    {
                        ModelState.AddModelError("", "Algun dato duplicado \n\r" + ed.Message);
                        return View("Perfil", usuarios);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Algo salio mal" + e.Message);
                        return View("Perfil", usuarios);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ingrese una contraseña correcta");
                    return View("Perfil", usuarios);
                }

            }
            else
            {
                ModelState.AddModelError("", "Esta cuenta ya existe, ingrese otro");
                return View("Perfil", usuarios);
            }
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos del usuario de la dase de datos mediante el identificador del usuario
        /// para editar contraseña del usuario
        /// </summary>
        /// <param name="id">identificador del usuario</param>
        /// <returns>devuelve una vista parcial con los datos del usuario</returns>
        // GET: Usuario/EditPass/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Usuario_Editar_NomUsuario_Contrasena)]
        public ActionResult EditPass(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Error!!!");
                return PartialView();
            }
            Usuario usuario = db.Usuarios.Find(id);
            Session["UsuarioModific"] = usuario;
            if (usuario == null)
            {
                ModelState.AddModelError("", "Error!!!");
                return PartialView();
            }
            return PartialView(usuario);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Realiza la modificación de la contraseña del usuario en la base de datos
        /// </summary>
        /// <param name="usuarios">datos del usuario</param>
        /// <returns>Redirecciona a la vista que le corresponde segun el rol del usuario</returns>
        // POST: Usuario/EditPass/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPass(Usuario usuarios)
        {
            string Dashboard;
            if ((int)RolEnums.Administrador==usuarios.IdRol)
            {
                Dashboard = "Admin";                      //Si eres Superusuario al panel de SuperUsuario
            }
            else if ((int)RolEnums.Bibliotecario == usuarios.IdRol)
            {
                Dashboard = "Dashboard";                      //Si eres lector te lleva a otro panel
            }
            else if ((int)RolEnums.Lector== usuarios.IdRol)
            {
                Dashboard = "PanelUsuario";                      //Si eres lector te lleva a otro panel
            }
            else
            {
                Dashboard = "Home";                   //Si eres Bibliotecario te lleva al panel de Bibliotecario
            }
            var usuarioS = (Usuario)Session["UsuarioModific"];
            string nonUnser = usuarioS.NomUsuario;
            try
            {
                usuarios.Contrasena = usuarios.Encriptar(usuarios.Contrasena);
                usuarios.ConfirmarContrasena = usuarios.Encriptar(usuarios.ConfirmarContrasena);
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                Session["ErrorLogin"] = null;
                Session["UsuarioModific"] = null;
                return RedirectToAction("Index", Dashboard);
            }
            catch (DbEntityValidationException e)
            {
                ModelState.AddModelError("", "Las contraseñas no coenciden!!! " + e.Message);
                return View("Perfil", usuarios);
            }
            catch (DbUpdateException ed)
            {
                ModelState.AddModelError("", "Algun dato duplicado \n\r" + ed.Message);
                return View("Perfil", usuarios);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Error... " + e.Message);
                return View("Perfil", usuarios);
            }

        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene datos del usuario de la base de datos mediante un identificador del usuario
        /// para modificar el rol del usuario
        /// </summary>
        /// <param name="id">identificador del usuario</param>
        /// <returns>devuelve una vista parcial con los datos del usuario</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Usuario_Editar_Rol)]
        public ActionResult EditAdmin(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Envió una contraseña vacía");
                return RedirectToAction("Index");
            }
            ViewBag.idRol = new SelectList(db.Roles, "IdRol", "NomRol");
            Usuario usuario = db.Usuarios.Find(id);
            Usuario usuarioAdmin = db.Usuarios.Find(Helper.SessionHelper.GetUser());
            Session["usuarioAdmin"] = usuarioAdmin;
            if (usuario == null)
            {
                ModelState.AddModelError("", "No Existe la contraseña que ingresó en la db");
                return RedirectToAction("Index");
            }
            return PartialView(usuario);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Modifica el rol del usuario en la base de datos, 
        /// si se modifica a administrador finaliza la sesion el usuario actual
        /// </summary>
        /// <param name="usuarios">datos del usuario</param>
        /// <returns>Redirecciona al Index del usuario, si modifica a administrador se cierra sesión y sale al index del home</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(Usuario usuarios)
        {
            bool cambioAdmin = false;
            try
            {
                usuarios.ConfirmarContrasena = usuarios.Contrasena;
                db.Entry(usuarios).State = EntityState.Modified;
                if ((int)RolEnums.Administrador==usuarios.IdRol)
                {
                    var usuarioActual = Session["usuarioAdmin"] as Usuario;
                    var usuario_n = new Usuario() {
                        IdUsuario= usuarioActual.IdUsuario,
                        IdPersona= usuarioActual.IdPersona,
                        IdRol= 3,
                        NomUsuario= usuarioActual.NomUsuario,
                        Contrasena= usuarioActual.Contrasena,
                        ConfirmarContrasena= usuarioActual.Contrasena,
                        Estado= usuarioActual.Estado
                    };
                    db.Entry(usuario_n).State = EntityState.Modified;
                    cambioAdmin = true;
                }
                db.SaveChanges();
                if (cambioAdmin)
                {
                    FormsAuthentication.SignOut();
                }
            }
            catch (DbEntityValidationException e)
            {

                ModelState.AddModelError("", "Error al modificar el rol" + e.Message);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return RedirectToAction("Index");

        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Obtiene datos del usuario de la base de datos mediante un identificador del usuario
        /// para dehabilitar el usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Devuelve una vista parcial con los datos del usuario</returns>
        // GET: Usuario/Delete/5
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Usuario_Eliminar_Usuario)]
        [HttpGet, ActionName("Delete")]
        public ActionResult vistaDeshabilitar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return PartialView(usuario);
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Confirma la deshabilitacion y guarda los cambios en la base de datos
        /// </summary>
        /// <param name="usuario">datos del usuario</param>
        /// <returns>Redirecciona al index del uaurio</returns>
        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deshabilitar(Usuario usuario)
        {
            var usuario_n = new Usuario()
            {
                IdUsuario = usuario.IdUsuario,
                IdPersona = usuario.IdPersona,
                IdRol = usuario.IdRol,
                NomUsuario = usuario.NomUsuario,
                Contrasena = usuario.Contrasena,
                ConfirmarContrasena = usuario.Contrasena,
                Estado = usuario.Estado
            };
            try
            {
                db.Entry(usuario_n).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.mensaje = "Operacion Correcta";
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return RedirectToAction("Index");
        }

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.2
        /// <summary>
        /// Cierra sesión del sistema
        /// </summary>
        /// <returns>Devuelve la vista vacia del index de Home</returns>
        [PermisoAtributo(IdAccion =Models.Acciones.AccionesEnum.Usuario_Logout)]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["IdUsuario"] = null;
            Session["ErrorLogin"] = null;
            Session["InicieSesion"] = null;
            return RedirectToAction("Index", "Home");
        }
        //Automatico, cierra la base de datos
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