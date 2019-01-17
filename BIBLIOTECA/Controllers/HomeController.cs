using BIBLIOTECA.Etuiqueta;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.Emuns;
using BIBLIOTECA.Models.Metodos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BIBLIOTECA.Controllers
{
    public class HomeController : Controller
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();
        Funciones fc = new Funciones();

        ///Autor: Larota Ccoa Luis Eusebio
        ///Versión: 0.3
        /// <summary>
        /// Panel principal del Sistema, 
        /// Si la base de datos no existe, crea la base de datos
        /// Registra datos necesarios
        /// Actualiza las reservas
        /// </summary>
        /// <returns>Devueleve una vista</returns>
        public ActionResult Index()
        {
            /// verifica la existencia de una base de datos
            if (!ExistDB())
            {
                registrar();
                return View();
            }
            Consultas cn = new Consultas();
            cn.actualizarReservas();    //Actualiza las reservas, si la fecha de recojo ya caducó
            string mensaje = Session["ErrorLogin"] as string;
            if (!string.IsNullOrEmpty(mensaje)) //si existe algun mensaje en la sesión
            {
                ModelState.AddModelError("", mensaje);
            }
            return View();
        }

        ///Autor: Larota Ccoa Luis Eusebio
        ///Versión: 0.1
        /// <summary>
        /// Verifica a que panel pertenece el usuario que está logeado actualmente, en base a su identificador del usuario
        /// </summary>
        /// <param name="id">Identificador del usuario</param>
        /// <returns>Redirecciona al index de su panel correspondiente</returns>
        [PermisoAtributo(IdAccion = Models.Acciones.AccionesEnum.Home_mipanel)]
        public ActionResult mipanel(int id)
        {
            int idRol = db.Usuarios.Find(id).IdRol;
            if ((int)RolEnums.Administrador == idRol)
            {
                return RedirectToAction("Index", "Admin");                      //Si eres Superusuario al panel de SuperUsuario
            }
            else if ((int)RolEnums.Bibliotecario==idRol)
            {
                return RedirectToAction("Index", "Dashboard");                      //Si eres lector te lleva a otro panel
            }
            else if ((int)RolEnums.Lector==idRol)
            {
                return RedirectToAction("Index", "PanelUsuario");                   //Si eres Bibliotecario te lleva al panel de Bibliotecario
            }
            return RedirectToAction("Index","Home");
        }

        ///Autor: Larota Ccoa Luis Eusebio
        ///Versión: 0.1
        /// <summary>
        /// Verifica si existe la base de datos
        /// </summary>
        /// <returns>Devuele true si existe, false si no existe</returns>
        public bool ExistDB()
        {
            using (var cn = new SqlConnection(@"Data Source=.;Initial Catalog=master;Integrated Security=True"))
            {
                string _sql = @"SELECT Name FROM sysdatabases WHERE name = 'DBBIBLIOTECAS'";
                var cmd = new SqlCommand(_sql, cn);
                cn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return true;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return false;
                }
            }

        }

        /// <summary>
        /// Registra los datos necesarios (una persona, roles y crea un usuario administrador) 
        /// </summary>
        public void registrar()
        {
            Persona persona3 = new Persona();
            persona3.Dni = "71938027";
            persona3.Apellidos = "QUISPE LUIZAR";
            persona3.Nombres = "FRANKLIN ANTONI";
            persona3.Sexo = Genero.Masculino;
            persona3.FecNacimiento = fc.FehasNac(24);
            persona3.Direccion = "Av. Miguel Grau";
            persona3.Telefono = "950408857";
            persona3.Email = "antoni.luizar@gmail.com".ToLower();
            try
            {
                db.Personas.Add(persona3);
                db.SaveChanges();

            }
            catch (Exception e)
            {

                throw;
            }
            
            Rol role = new Rol()
            {
                NomRol = "Administrador"
            };

            db.Roles.Add(role);

            Rol role1 = new Rol()
            {
                NomRol = "Bibliotecario"
            };

            db.Roles.Add(role1);

            Rol role2 = new Rol()
            {
                NomRol = "Lector"
            };
            db.Roles.Add(role2);
            db.SaveChanges();

            Usuario usuario = new Usuario
            {
                IdPersona = 1,
                IdRol = 1,
                NomUsuario = "Admin",
                Contrasena = "administrador",
                ConfirmarContrasena = "administrador",
                Estado = true,
            };
            usuario.Contrasena = usuario.Encriptar(usuario.Contrasena);
            usuario.ConfirmarContrasena = usuario.Encriptar(usuario.ConfirmarContrasena);
            db.Usuarios.Add(usuario);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw;
            }

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