using Microsoft.VisualStudio.TestTools.UnitTesting;
using BIBLIOTECA.Models;
using System;
using System.Linq;
using BIBLIOTECA.Models.Metodos;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace BIBLIOTECA.Controllers.Tests
{
    [TestClass()]
    public class HU07
    {
        BIBLIOTECAContext db = new BIBLIOTECAContext();

        [TestMethod()]
        public void HU07_CA01_1_verificar_usuario_fue_insertado_correctamente()
        {
            //Arrange
            string esperado = "USUARIO REGISTRADO CORRECTAMENTE";
            string dni = "55555555";
            var actual = "";

            //ACT
            var personaTest = db.Personas.Where(u => u.Dni == dni).FirstOrDefault();
            if (personaTest == null)
            {
                actual = "No Existe DNI, Ingrese Un Dni Correcto o verifique sus datos en biblioteca";
            }
            var contrasenaEncriptada = SHA1.Encode("senha123");
            try
            {
                Usuario usuarioTest = new Usuario()
                {
                    NomUsuario = "lector12",
                    Contrasena = contrasenaEncriptada,
                    ConfirmarContrasena = contrasenaEncriptada,
                    IdRol = 3,
                    IdPersona = personaTest.IdPersona
                };
                db.Usuarios.Add(usuarioTest);
                db.SaveChanges();
                actual = "USUARIO REGISTRADO CORRECTAMENTE";

            }
            catch (DbUpdateException ed)
            {
                actual = "DICHO DNI YA SE ENCUENTRA REGISTRADO";
            }
            catch (DbEntityValidationException e)
            {
                actual = "las contraseñas no son iguales";

            }
            catch (Exception e)
            {
                actual = "Algo salio mal en la Base de Datos \n\r" + e;
            }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU07_CA01_2_verificar_usuario_no_fue_insertado_correctamente()
        {
            //Arrange
            string esperado = "NO SE PUDO COMPLETAR CON EL REGISTRO DEL USUARIO";
            //ENVIAMOS UN DNI NO EXISTENTE
            string dni = "09876777";
            var actual = "";

            //ACT
            var personaTest = db.Personas.Where(u => u.Dni == dni).FirstOrDefault();
            if (personaTest == null)
            {
                actual = "No Existe DNI, Ingrese Un Dni Correcto o verifique sus datos en biblioteca";
            }
            var contrasenaEncriptada = SHA1.Encode("senha123");
            try
            {
                Usuario usuarioTest = new Usuario()
                {
                    NomUsuario = "lector12",
                    Contrasena = contrasenaEncriptada,
                    ConfirmarContrasena = contrasenaEncriptada,
                    IdRol = 3,
                    IdPersona = personaTest.IdPersona
                };
                db.Usuarios.Add(usuarioTest);
                db.SaveChanges();
                actual = "USUARIO REGISTRADO CORRECTAMENTE";

            }
            catch (Exception e)
            {
                actual = "NO SE PUDO COMPLETAR CON EL REGISTRO DEL USUARIO";
            }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU07_CA02_verificar_si_usuario_ya_se_encuentra_registrado()
        {
            //Arrange
            string esperado = "DICHO DNI YA SE ENCUENTRA REGISTRADO";
            //ENVIAMOS EL DNI DE UNA PERSONA YA REGISTRADA
            string dni = "55555555";
            var actual = "";

            //ACT
            var personaTest = db.Personas.Where(u => u.Dni == dni).FirstOrDefault();
            if (personaTest == null)
            {
                actual = "No Existe DNI, Ingrese Un Dni Correcto o verifique sus datos en biblioteca";
            }
            var contrasenaEncriptada = SHA1.Encode("senha123");
            try
            {
                Usuario usuarioTest = new Usuario()
                {
                    //CAMBIAMOS EL NOMBRE DE USUARIO PARA INTENTAR QUE SE CREE UN USUARIO NUEVO
                    NomUsuario = "lector1243",
                    Contrasena = contrasenaEncriptada,
                    ConfirmarContrasena = contrasenaEncriptada,
                    IdRol = 3,
                    IdPersona = personaTest.IdPersona
                };
                db.Usuarios.Add(usuarioTest);
                db.SaveChanges();
                actual = "Usuario registrado correctamente";

            }
            catch (DbUpdateException ed)
            {
                actual = "DICHO DNI YA SE ENCUENTRA REGISTRADO";
            }
            catch (DbEntityValidationException e)
            {
                actual = "las contraseñas no son iguales";

            }
            catch (Exception e)
            {
                actual = "Algo salio mal en la Base de Datos \n\r" + e;
            }
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }


        [TestMethod()]
        public void HU07_CA03_1_verificar_si_usuario_esta_listo_para_ser_modificado()
        {

            //ASSIGN
            int idUsuario = 4;
            string esperado = "EL USUARIO ESTÁ LISTO PARA SER MODIFICADO";
            var actual = "";
            //ACT
            Usuario usuarioTest = db.Usuarios.Find(idUsuario);
            actual = (usuarioTest == null) ? "NO SE PUEDE MODIFICAR UN USUARIO NO REGISTRADO" : esperado;
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU07_CA03_2_verificar_si_usuario_no_esta_listo_para_ser_modificado()
        {

            //ASSIGN
            //enviamos un id inexistente
            int idUsuario = 1400;
            string esperado = "NO SE PUEDE MODIFICAR UN USUARIO NO REGISTRADO";
            var actual = "";
            //ACT
            Usuario usuarioTest = db.Usuarios.Find(idUsuario);
            actual = (usuarioTest == null) ? esperado : "EL USUARIO ESTÁ LISTO PARA SER MODIFICADO";
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU07_CA04_1_verificar_que_usuario_fue_modificado_correctamente()
        {
            //Arrange
            string esperado = "EL USUARIO FUE MODIFICADO CORRECTAMENTE";
            var actual = "";
            var contrasenaEncriptada = SHA1.Encode("senha123");
            //Act
            Usuario UsuarioEditadoTest = new Usuario()
            {
                IdUsuario = 4,
                NomUsuario = "UsuarioEditadoTest",
                Contrasena = contrasenaEncriptada,
                ConfirmarContrasena = contrasenaEncriptada,
                IdRol = 3,
                IdPersona = 8
            };
            try
            {
                db.Entry(UsuarioEditadoTest).State = EntityState.Modified;
                db.SaveChanges();
                actual = "EL USUARIO FUE MODIFICADO CORRECTAMENTE";
            }
            catch (Exception e)
            {
                actual = "EL USUARIO NO FUE MODIFICADO CORRECTAMENTE" +e;
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU07_CA04_2_verificar_que_usuario_no_fue_modificado_correctamente()
        {
            //Arrange
            string esperado = "EL USUARIO NO FUE MODIFICADO CORRECTAMENTE";
            var actual = "";
            var contrasenaEncriptada = SHA1.Encode("senha123");
            //Act
            Usuario UsuarioEditadoTest = new Usuario()
            {
                //enviamos un usuario q no existe
                IdUsuario = 100,
                NomUsuario = "UsuarioEditadoTest",
                Contrasena = contrasenaEncriptada,
                ConfirmarContrasena = contrasenaEncriptada,
                IdRol = 3,
                IdPersona = 8
            };
            try
            {
                db.Entry(UsuarioEditadoTest).State = EntityState.Modified;
                db.SaveChanges();
                actual = "EL USUARIO FUE MODIFICADO CORRECTAMENTE";
            }
            catch (Exception e)
            {
                actual = "EL USUARIO NO FUE MODIFICADO CORRECTAMENTE";
            }
            //Assert
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU07_CA05_1_verificar_si_usuario_esta_listo_para_ser_eliminado()
        {

            //ASSIGN
            int idUsuario = 4;
            string esperado = "¿ESTÁ SEGURO QUE DESEA ELIMINAR ESTE USUARIO?";;;
            var actual = "";
            //ACT
            Usuario usuarioParaEliminar = db.Usuarios.Find(idUsuario);
            actual = (usuarioParaEliminar == null) ?  "NO SE PUEDE ELIMINAR UN USUARIO NO REGISTRADO" : esperado;
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }

        [TestMethod()]
        public void HU07_CA05_2_verificar_si_usuario_no_esta_listo_para_ser_eliminado()
        {
            //ASSIGN
            //Enviamos un id inexistente
            int idUsuario = 1400;
            string esperado = "NO SE PUEDE ELIMINAR UN USUARIO NO REGISTRADO";
            var actual = "";
            //ACT
            Usuario usuarioParaEliminar = db.Usuarios.Find(idUsuario);
            actual = (usuarioParaEliminar == null) ? esperado : "¿ESTÁ SEGURO QUE DESEA ELIMINAR ESTE USUARIO?";
            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod()]
        public void HU07_CA06_verificar_eliminacion_de_usuario()
        {
            //ASSIGN
            int idUsuario = 4;
            string esperado = "USUARIO ELIMINADO CORRECTAMENTE";
            
            var actual = "";
            //ACT
            Usuario usuarioParaEliminar = db.Usuarios.Find(idUsuario);
            try
            {
                db.Usuarios.Remove(usuarioParaEliminar);
                db.SaveChanges();
                actual = "USUARIO ELIMINADO CORRECTAMENTE";
            }
            catch (Exception e) { actual = "Error al intentar Eliminar libro"; }

            //ASSERT
            Assert.AreEqual(esperado, actual);
        }
    }
}