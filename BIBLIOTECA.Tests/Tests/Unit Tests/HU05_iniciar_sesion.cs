using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Models;
using BIBLIOTECA.Models.ViewModels;
using BIBLIOTECA.Models.Metodos;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace BIBLIOTECA.Tests.Controllers
{

    [TestClass]
    public class HU05
    {

        private BIBLIOTECAContext db = new BIBLIOTECAContext();
        private const int superusuario = 1;
        private const int bibliotecario = 2;
        private const int lector = 3;
        private bool isBibliotecarios(int idRol)
        {
            return bibliotecario.Equals(idRol);
        }
        private bool isLector(int idRol)
        {
            return lector.Equals(idRol);
        }
        private bool isSuperUsuario(int idRol)
        {
            return superusuario.Equals(idRol);
        }
        [TestMethod]
        public void HU05_CA02_1_verificar_usuario_y_contrasena_correctos()
        {
            //Arrange
            string esperado = "BIENVENIDO A LA BIBLIOTECA";
            var actual = "";
            string us = "lector1";
            string pass = SHA1.Encode("lector12");
            //Verifica si existe el usuario
            long de = db.Usuarios.LongCount();
            if (de > 0)
            {
                Usuario v = db.Usuarios.Where(x => x.NomUsuario == us && x.Contrasena == pass).SingleOrDefault();
                //si el usuario existe y ambos datos (nombre de usuario, contraseña) son correctos
                if (v != null)
                {
                    if (isSuperUsuario(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else if (isBibliotecarios(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }

                }
                else
                {
                    //Error cuando los credenciales son incorrectos
                    actual = "USUARIO O CONTRASEÑA NO SON CORRECTOS";
                }

            }
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU05_CA03_verificar_que_se_ingreso_un_usuario_incorrecto()
        {
            //Arrange
            string esperado = "EL USUARIO INGRESADO NO ESTÁ REGISTRADO EN EL SISTEMA";
            var actual = "";
            //ENVIAMOS UN LECTOR NO REGISTRADO
            string us = "lector400";
            string pass = SHA1.Encode("lector12");
            //Verifica si existe el usuario
            long de = db.Usuarios.LongCount();
            if (de > 0)
            {
                Usuario v = db.Usuarios.Where(x => x.NomUsuario == us).SingleOrDefault();
                //si el usuario existe y ambos datos (nombre de usuario, contraseña) son correctos
                if (v != null)
                {
                    if (isSuperUsuario(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else if (isBibliotecarios(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }

                }
                else
                {
                    //Error cuando el usuario no existe en la db
                    actual = "EL USUARIO INGRESADO NO ESTÁ REGISTRADO EN EL SISTEMA";
                }

            }
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU05_CA04_verificar_que_se_ingreso_una_contrasena_incorrecta()
        {
            //Arrange
            string esperado = "LA CONTRASEÑA NO ES CORRECTA";
            var actual = "";
            //ENVIAMOS UN LECTOR REGISTRADO
            string us = "lector12";
            //ENVIAMOS UNA CONTRASEÑA NO VÁLIDA PARA ESTE USUARIO
            string pass = SHA1.Encode("contrasenaIncorrecta");
            //Verifica si existe el usuario
            long de = db.Usuarios.LongCount();
            if (de > 0)
            {
                Usuario v = db.Usuarios.Where(x => x.NomUsuario == us && x.Contrasena == pass).SingleOrDefault();
                //si el usuario existe y ambos datos (nombre de usuario, contraseña) son correctos
                if (v != null)
                {
                    if (isSuperUsuario(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else if (isBibliotecarios(v.IdRol))
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }
                    else
                    {
                        actual = "BIENVENIDO A LA BIBLIOTECA";
                    }

                }
                else
                {
                    //Error cuando la contraseña no es correcta para dicho usuario
                    actual = "LA CONTRASEÑA NO ES CORRECTA";
                }

            }
            Assert.AreEqual(esperado, actual);
        }
    }
}