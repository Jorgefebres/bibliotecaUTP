using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BIBLIOTECA.Models;
using System.Linq;

namespace BIBLIOTECA.Tests.Controllers
{

    [TestClass]
    public class HU08
    {
        private BIBLIOTECAContext db = new BIBLIOTECAContext();

        [TestMethod]
        public void HU08_CA01_busqueda_de_usuario_por_criterio()
        {
            //Arrange
            string esperado = "Dicho usuario se encuentra registrado";
            var actual = "";
            string dnibuscado = "4529";

            //Act1|
            List<string> dni;
            dni = db.Personas.Where(x => x.Dni.StartsWith(dnibuscado)).Select(e => e.Dni).Distinct().ToList();
            actual = (dni.Count() > 0) ? esperado : "Dicho usuario NO se encuentra registrado";
            //Assert
            Assert.AreEqual(esperado, actual);
        }
        [TestMethod]
        public void HU08_CA02_busqueda_de_libro_por_criterio()
        {
            //Arrange
            string esperado = "Dicho Libro se encuentra registrado";
            var actual = "";
            string titulobuscado = "Kafka";
            //Act
            List<string> libro;
            libro = db.Libros.Where(x => x.Titulo.StartsWith(titulobuscado)).Select(e => e.Titulo).Distinct().ToList();
            actual = (libro.Count() > 0) ? esperado : "Dicho Libro NO se encuentra registrado";
            //Assert
            Assert.AreEqual(esperado, actual);
        }

    }
}