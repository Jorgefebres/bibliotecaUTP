using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using OpenQA.Selenium.Interactions;
//using AutoItX3Lib;
namespace BIBLIOTECA.Tests.Controllers
{
    [TestClass]

    public class RegresionBibliotecarioTests
    {
        private const string IE_DRIVER_PATH = @"C:\SeleniumDriver";
        private const string ScreenShotLocation = @"C:\CapturasTest";
        private const string direccionInicial = "http://localhost:53975/";


        [TestMethod]
        public void LectorRegresionTests()
        {
            IWebDriver driver = null;
            try
            {
                //nuevo driver para agregar categorias al CRUD
                //driver = new ChromeDriver(IE_DRIVER_PATH);
                driver = new InternetExplorerDriver(IE_DRIVER_PATH);
                driver.Navigate().GoToUrl(direccionInicial);
                //Para maximizar la ventana del navegador
                driver.Manage().Window.Maximize();
                CapturasTest(driver, "Pantalla de Bienvenida");
                //INICIAR SESION LECTOR
                TestIniciarSesion(driver, "Lector");
                TestVerCatalogo(driver, "Catálogo");
                TestRealizarReserva(driver);
                TestVerCatalogo(driver, "Catálogo");
                Thread.Sleep(3000);
                //CERRAMOS LA SESION DE LECTOR
                driver.FindElement(By.LinkText("Cerrar sesión")).Click();
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                //who cares
            }
            finally
            {
                driver.Quit();
            }
        }
        [TestMethod]
        public void BibliotecarioRegresionTests()
        {
            IWebDriver driver = null;
            try
            {
                //nuevo driver para agregar categorias al CRUD
                //driver = new ChromeDriver(IE_DRIVER_PATH);
                driver = new InternetExplorerDriver(IE_DRIVER_PATH);

                driver.Navigate().GoToUrl(direccionInicial);
                //Para maximizar la ventana del navegador
                driver.Manage().Window.Maximize();

                CapturasTest(driver, "Pantalla de Bienvenida");

                //INICIAR SESION BIBLIOTECARIO
                TestIniciarSesion(driver, "Bibliotecario");

                //CATEGORIA                
                TestAgregar(driver, "Categoria", "Categorías");
                CapturasTest(driver, "Despues de Agregar Categoría");
                TestEditar(driver, "Categoria", "Lenguaje");
                CapturasTest(driver, "Despues de Editar Categoría");
                TestBuscar(driver, "Categoria");
                TestEliminar(driver, "Categoría", "Lenguaje, idiomas");
                CapturasTest(driver, "Despues de Eliminar Categoría");

                //EDITORIAL
                TestAgregar(driver, "Editorial", "Editoriales");
                CapturasTest(driver, "Despues de Agregar Editorial");
                TestEditar(driver, "Editorial", "Universo");
                CapturasTest(driver, "Despues de Editar Editorial");
                TestEliminar(driver, "Editorial", "Universo 2.0");
                CapturasTest(driver, "Despues de Eliminar Editorial");

                //AUTOR
                TestAgregar(driver, "Autor", "Autores");
                CapturasTest(driver, "Despues de Agregar Autor");
                TestEditar(driver, "Autor", "Frans");
                CapturasTest(driver, "Despues de Editar Autor");
                //TestEliminar(driver, "Autor", "Franz");
                //CapturasTest(driver, "Despues de Eliminar Autor");

                //LIBRO
                TestAgregar(driver, "Libro", "Libros");
                CapturasTest(driver, "Despues de Agregar Libro");
                TestEditar(driver, "Libro", "Metamorfosis");
                CapturasTest(driver, "Despues de Editar Libro");
                TestBuscar(driver, "Libro");
                //TestEliminar(driver, "Libro", "La Metamorfosis");
                //CapturasTest(driver, "Despues de Eliminar Autor");

                //MOSTRAMOS EL LIBRO AGREGADO EN EL CATALOGO
                driver.Navigate().GoToUrl(direccionInicial + "Catalogo");
                Thread.Sleep(2000);

                driver.Navigate().GoToUrl(direccionInicial + "Dashboard");
                Thread.Sleep(2000);

                ////PRESTAR RESERVA
                TestPrestarReserva(driver, "/PrestarReserva/Index");
                CapturasTest(driver, "Despues de prestar una reserva");
                Thread.Sleep(2000);
                //REALIZAR PRESTAMO
                TestRealizarPrestamo(driver, "Prestamo/Index");
                CapturasTest(driver, "Despues de realizar un Prestamo");
                Thread.Sleep(2000);
                //REALIZAR DEVOLUCIÓN
                TestRealizarDevolucion(driver, "Devolucion/Devolver");
                CapturasTest(driver, "Despues de realizar una devolucion");
                Thread.Sleep(2000);
                driver.Navigate().GoToUrl(direccionInicial + "Catalogo");
                Thread.Sleep(2000);
                //CERRAMOS LA SESION DE BIBLIOTECARIO
                driver.FindElement(By.LinkText("Cerrar sesión")).Click();
                Thread.Sleep(2000);

            }
            catch (Exception ex)
            {
                //who cares
            }
            finally
            {
                driver.Quit();
            }
        }

        private void TestIniciarSesion(IWebDriver driver, string rol)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until((d) => { return d.FindElement(By.LinkText("Ver catálogo")); });

            driver.FindElement(By.LinkText("Iniciar Sesión")).Click();

            wait.Until((d) => { return d.FindElement(By.XPath("//input[@value='Iniciar sesión']")); });

            IWebElement usuario = driver.FindElement(By.Id("NomUsuario"));
            IWebElement password = driver.FindElement(By.Id("Contrasena"));
            switch (rol)
            {
                case "Bibliotecario":
                    usuario.SendKeys("admin2");
                    password.SendKeys("admin123");
                    break;
                case "Lector":
                    usuario.SendKeys("lector1");
                    password.SendKeys("lector12");
                    break;
            }
            //para documentación
            CapturasTest(driver, "Login de " + rol);
            IWebElement botonConfirmarLogin = driver.FindElement(By.XPath("//input[@type='submit']"));
            botonConfirmarLogin.Click();

        }
        private void TestAgregar(IWebDriver driver, string nombreModulo, string nombreBoton)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //driver.Navigate().GoToUrl(direccionInicial + nombreModulo);
            //busca el primer elemento <p> en dicha vista, el cual es el boton de mantenimiento
            IWebElement botonMantenimineto = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[1]/p[2]"));
            botonMantenimineto.Click();

            Thread.Sleep(1000);
            IWebElement botonModulo = driver.FindElement(By.LinkText(nombreBoton));
            botonModulo.Click();

            CapturasTest(driver, ("Antes de Agregar " + nombreModulo));
            driver.FindElement(By.Id("nuevo")).Click();
            wait.Until((d) => { return d.FindElement(By.Id("nuevo")); });

            IWebElement botonAgregar = driver.FindElement(By.XPath("//input[@type='submit']"));
            switch (nombreModulo)
            {
                case "Categoria":
                    wait.Until((d) => { return d.FindElement(By.Id("NomCategoria")); });

                    IWebElement nombreCategoria = driver.FindElement(By.Id("NomCategoria"));
                    IWebElement descripcionCategoria = driver.FindElement(By.Id("Descripcion"));

                    Thread.Sleep(1000);

                    nombreCategoria.SendKeys("Lenguaje e idiomas");
                    descripcionCategoria.SendKeys("Libros de lenguaje e idiomas");
                    break;
                case "Editorial":
                    wait.Until((d) => { return d.FindElement(By.Id("NomEditorial")); });

                    IWebElement nombreEditorial = driver.FindElement(By.Id("NomEditorial"));
                    IWebElement paisEditorial = driver.FindElement(By.Id("Pais"));
                    IWebElement ciudadEditorial = driver.FindElement(By.Id("Ciudad"));
                    IWebElement emailEditorial = driver.FindElement(By.Id("Email"));
                    IWebElement urlEditorial = driver.FindElement(By.Id("Url"));

                    Thread.Sleep(1000);

                    nombreEditorial.SendKeys("Universo");
                    paisEditorial.SendKeys("Brasil");
                    ciudadEditorial.SendKeys("Sao Paulo");
                    emailEditorial.SendKeys("editorialuniverso@gmail.com");
                    urlEditorial.SendKeys("http://www.editorialuniverso.com.br");
                    break;
                case "Autor":
                    //wait.Until((d) => { return d.FindElement(By.Id("Apellidos")); });

                    IWebElement ApellidosAutor = driver.FindElement(By.Id("Apellidos"));
                    IWebElement NombresAutor = driver.FindElement(By.Id("Nombres"));
                    IWebElement SexoAutor = driver.FindElement(By.Id("Sexo"));
                    IWebElement FecNacimientoAutor = driver.FindElement(By.Id("FecNacimiento"));
                    IWebElement PaisAutor = driver.FindElement(By.Id("Pais"));

                    Thread.Sleep(1000);

                    ApellidosAutor.SendKeys("Kafka");
                    NombresAutor.SendKeys("Frans");
                    SexoAutor.SendKeys("Femenino" + Keys.Enter);
                    FecNacimientoAutor.SendKeys("12-12-1880");
                    PaisAutor.SendKeys("Rep Checa");
                    break;
                case "Libro":
                    wait.Until((d) => { return d.FindElement(By.Id("Titulo")); });

                    IWebElement tituloLibro = driver.FindElement(By.Id("Titulo"));

                    IWebElement idiomaLibro = driver.FindElement(By.Id("Idioma"));
                    IWebElement numeroEdicionLibro = driver.FindElement(By.Id("NumEdicion"));
                    IWebElement isbnLibro = driver.FindElement(By.Id("ISBN"));
                    IWebElement anioLibro = driver.FindElement(By.Id("Anio"));
                    IWebElement numeroPaginasLibro = driver.FindElement(By.Id("NumPaginas"));
                    IWebElement estadoLibro = driver.FindElement(By.Id("Estado"));
                    IWebElement caratulaLibro = driver.FindElement(By.Name("CaratulaLibro"));
                    IWebElement disponibilidadLibro = driver.FindElement(By.Id("Disponibilidad"));
                    IWebElement autorLibro = driver.FindElement(By.Name("IdAutor"));
                    IWebElement categoriaLibro = driver.FindElement(By.Name("IdCategoria"));
                    IWebElement editorialLibro = driver.FindElement(By.Name("IdEditorial"));

                    Thread.Sleep(1000);

                    tituloLibro.SendKeys("Metamorfosis");
                    idiomaLibro.SendKeys("Español");
                    numeroEdicionLibro.SendKeys("1");
                    isbnLibro.SendKeys("34223393");
                    anioLibro.SendKeys("1920");
                    numeroPaginasLibro.SendKeys("1000");
                    estadoLibro.SendKeys("Bueno");
                    //CAMBIAR LA RUTA DE LAS CARATULAS SEGUN EN DONDE SE EJECUTEN LOS TESTS
                    string direccionCaratula = @"C:\Caratulas\9978636.jpg";
                    caratulaLibro.SendKeys(direccionCaratula);
                    disponibilidadLibro.Click();
                    autorLibro.SendKeys("Kafka" + Keys.Tab);
                    categoriaLibro.SendKeys("Literatura" + Keys.Tab);
                    editorialLibro.SendKeys("Planeta" + Keys.Tab);
                    Thread.Sleep(1000);
                    //POR SI NO FUNCIONA LA SUBIDA DE LA CARATULA CON SELENIUM ... UTILIZAR AUTOIT
                    //AutoItX3 autoit = new AutoItX3();
                    //autoit.WinActivate("Elegir archivos para cargar");
                    //autoit.Send(@"C:\caratulas\3096708.jpg");
                    //Thread.Sleep(1000);
                    //autoit.Send("{ENTER}");
                    break;
            }
            CapturasTest(driver, ("Agregando Campos " + nombreModulo));
            botonAgregar.Click();
        }
        private void TestEditar(IWebDriver driver, string nombreModulo, string parametro)
        {
            //Find the element anchortag for the employee having name as 'Historia'
            // Buscar la etiqueta de anclaje del elemento para la categoría que contiene el texo 'Historia'
            IWebElement elementoEditar = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('a',$(\"td:contains('" + parametro + "')\").parent())[1]");
            elementoEditar.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.FindElement(By.XPath("//input[@type='submit']")); });

            IWebElement botonEditar = driver.FindElement(By.XPath("//input[@type='submit']"));

            switch (nombreModulo)
            {
                case "Categoria":
                    //Find all the elements
                    //Buscar todos los elementos                    
                    IWebElement nombreCategoria = driver.FindElement(By.Id("NomCategoria"));
                    IWebElement descripcionCategoria = driver.FindElement(By.Id("Descripcion"));

                    //(opcional) Limpiamos las Cajas de texto
                    nombreCategoria.Clear();
                    descripcionCategoria.Clear();
                    Thread.Sleep(1000);

                    //Set the data (Change the name)
                    //Establecer los datos (Cambiar el nombre)
                    nombreCategoria.SendKeys("Lenguaje, idiomas y lingüística");
                    descripcionCategoria.SendKeys("Libros de Lenguaje, idiomas y lingüística");
                    break;
                case "Editorial":
                    //Find all the elements
                    //Buscar todos los elementos
                    IWebElement nombreEditorial = driver.FindElement(By.Id("NomEditorial"));
                    IWebElement paisEditorial = driver.FindElement(By.Id("Pais"));
                    IWebElement ciudadEditorial = driver.FindElement(By.Id("Ciudad"));
                    IWebElement emailEditorial = driver.FindElement(By.Id("Email"));
                    IWebElement urlEditorial = driver.FindElement(By.Id("Url"));

                    //(opcional) Limpiamos las Cajas de texto
                    nombreEditorial.Clear();
                    Thread.Sleep(1000);

                    //Set the data (Change the name)
                    //Establecer los datos (Cambiar el nombre)
                    nombreEditorial.SendKeys("Universo 2.0");
                    break;
                case "Autor":
                    IWebElement ApellidosAutor = driver.FindElement(By.Id("Apellidos"));
                    IWebElement NombresAutor = driver.FindElement(By.Id("Nombres"));
                    IWebElement SexoAutor = driver.FindElement(By.Id("Sexo"));
                    IWebElement FecNacimientoAutor = driver.FindElement(By.Id("FecNacimiento"));
                    IWebElement PaisAutor = driver.FindElement(By.Id("Pais"));

                    ApellidosAutor.Clear();
                    NombresAutor.Clear();
                    PaisAutor.Clear();
                    Thread.Sleep(1000);

                    ApellidosAutor.SendKeys("Kafka");
                    NombresAutor.SendKeys("Franz");
                    SexoAutor.SendKeys("Masculino" + Keys.Enter);
                    PaisAutor.SendKeys("Rep Checa");

                    break;
                case "Libro":
                    IWebElement tituloLibro = driver.FindElement(By.Id("Titulo"));
                    IWebElement idiomaLibro = driver.FindElement(By.Id("Idioma"));
                    IWebElement autorLibro = driver.FindElement(By.Name("IdAutor"));
                    IWebElement categoriaLibro = driver.FindElement(By.Name("IdCategoria"));
                    IWebElement editorialLibro = driver.FindElement(By.Name("IdEditorial"));
                    IWebElement numeroEdicionLibro = driver.FindElement(By.Id("NumEdicion"));
                    IWebElement anioLibro = driver.FindElement(By.Id("Anio"));
                    IWebElement numeroPaginasLibro = driver.FindElement(By.Id("NumPaginas"));
                    IWebElement estadoLibro = driver.FindElement(By.Id("Estado"));
                    IWebElement caratulaLibro = driver.FindElement(By.Name("CaratulaLibro"));
                    IWebElement disponibilidadLibro = driver.FindElement(By.Id("Disponibilidad"));

                    tituloLibro.Clear();
                    anioLibro.Clear();
                    numeroPaginasLibro.Clear();
                    Thread.Sleep(1000);

                    tituloLibro.SendKeys("La Metamorfosis");
                    anioLibro.SendKeys("1915");
                    numeroPaginasLibro.SendKeys("2500");
                    //CAMBIAR LA RUTA DE LAS CARATULAS SEGUN EN DONDE SE EJECUTEN LOS TESTS
                    string direccionCaratula = @"C:\Caratulas\9978636.jpg";
                    caratulaLibro.SendKeys(direccionCaratula);
                    Thread.Sleep(1000);
                    break;
            }
            CapturasTest(driver, ("Editando Campos " + nombreModulo));
            botonEditar.Click();
        }

        private void TestBuscar(IWebDriver driver, string nombreModulo)
        {
            //driver.Navigate().GoToUrl(direccionInicial + nombreModulo);
            IWebElement cajaBuscar = driver.FindElement(By.XPath("//input[@type='search']"));
            cajaBuscar.Click(); //Perfome delete operation  
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(1000);

            switch (nombreModulo)
            {
                case "Categoria":
                    //Realizar la operación de busqueda
                    cajaBuscar.SendKeys("histo");
                    CapturasTest(driver, ("Buscando parametro 1 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace); //Para seleccionar y borrar el texto

                    cajaBuscar.SendKeys("recre");
                    CapturasTest(driver, ("Buscando parametro 2 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace);

                    cajaBuscar.SendKeys("nolog");
                    CapturasTest(driver, ("Buscando paremetro 3 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace);

                    cajaBuscar.SendKeys("inexistente");
                    CapturasTest(driver, ("Buscando parametro 4 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace);
                    break;
                case "Libro":
                    //Realizar la operación de busqueda
                    cajaBuscar.SendKeys("kafka");
                    CapturasTest(driver, ("Buscando parametro 1 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace); //Para seleccionar y borrar el texto

                    cajaBuscar.SendKeys("tecnología");
                    CapturasTest(driver, ("Buscando parametro 2 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace); //Para seleccionar y borrar el texto

                    cajaBuscar.SendKeys("mundo");
                    CapturasTest(driver, ("Buscando parametro 3 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace); //Para seleccionar y borrar el texto

                    cajaBuscar.SendKeys("1915");
                    CapturasTest(driver, ("Buscando parametro 4 " + nombreModulo));
                    cajaBuscar.SendKeys(Keys.LeftShift + Keys.Home + Keys.Backspace); //Para seleccionar y borrar el texto
                    break;
            }
        }
        private void TestEliminar(IWebDriver driver, string nombreModulo, string parametro)
        {
            // Buscar el boton eliminar de la categoria que contiene la frase "historia y geografia"
            IWebElement elementoEliminar = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('a',$(\"td:contains('" + parametro + "')\").parent())[2]");
            elementoEliminar.Click();
            // Buscar botón Eliminar y hacerle clic
            IWebElement botonEliminar = driver.FindElement(By.XPath("//input[@type='submit']"));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Thread.Sleep(1000);
            //Realizar la operación de borrado
            CapturasTest(driver, ("Eliminando Campos " + nombreModulo));
            botonEliminar.Click(); //Perfome delete operation  
            Thread.Sleep(1000);
        }
        private void TestPrestarReserva(IWebDriver driver, string nombreModulo)
        {
            //driver.Navigate().GoToUrl(direccionInicial + nombreModulo);

            IWebElement botonMantenimineto = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[2]/p"));
            botonMantenimineto.Click();

            Thread.Sleep(2000);
            IWebElement botonModulo = driver.FindElement(By.XPath("//*[@id='btnReservas']"));
            botonModulo.Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/section/div/div[1]/a[1]")).Click();
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.FindElement(By.XPath("/html/body/section/div/form/div/div/input[2]")); });

            IWebElement dni = driver.FindElement(By.XPath("//input[@name='DNI']"));

            dni.SendKeys("45294265");
            Thread.Sleep(2000);
            IWebElement btnBuscar = driver.FindElement(By.XPath("//input[@value='Buscar']"));
            Thread.Sleep(2000);
            btnBuscar.Click();

            driver.FindElement(By.LinkText("Prestar")).Click();
            Thread.Sleep(2000);


        }
        private void TestRealizarPrestamo(IWebDriver driver, string nombreModulo)
        {
            //driver.Navigate().GoToUrl(direccionInicial + nombreModulo);
            IWebElement botonMantenimineto = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[2]/p"));
            botonMantenimineto.Click();

            Thread.Sleep(2000);
            IWebElement botonModulo = driver.FindElement(By.XPath("//*[@id='btnPrestamos']"));
            botonModulo.Click();
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement dni = driver.FindElement(By.XPath("//input[@name='DNI']"));
            dni.SendKeys("45294265");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//input[@value='NUEVO PRESTAMO']")).Click();
            Thread.Sleep(2000);
            wait.Until((d) => { return d.FindElement(By.LinkText("Agregar libro")); });

            driver.FindElement(By.LinkText("Agregar libro")).Click();
            Thread.Sleep(2000);
            wait.Until((d) => { return d.FindElement(By.LinkText("AGREGAR")); });
            IWebElement LibroAgregar = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('a',$(\"td:contains('Metamorfosis')\").parent())[0]");
            LibroAgregar.Click();
            Thread.Sleep(2000);

            IWebElement botonConfirmarPrestamo = driver.FindElement(By.XPath("//input[@type='submit']"));
            botonConfirmarPrestamo.Click();
        }

        private void TestRealizarDevolucion(IWebDriver driver, string nombreModulo)
        {
            //driver.Navigate().GoToUrl(direccionInicial + nombreModulo);
            IWebElement botonMantenimineto = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[2]/p"));
            botonMantenimineto.Click();

            Thread.Sleep(2000);
            IWebElement botonModulo = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[2]/ul/li[3]/a"));
            botonModulo.Click();


            Thread.Sleep(2000);
            IWebElement btnDevolverLibros = driver.FindElement(By.XPath("/html/body/section/div/div[1]/a[1]"));
            btnDevolverLibros.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until((d) => { return d.FindElement(By.XPath("//input[@name='DNI']")); });

            IWebElement dni = driver.FindElement(By.XPath("//input[@name='DNI']"));

            dni.SendKeys("45294265");
            Thread.Sleep(2000);
            IWebElement botonBuscarDevolucion = driver.FindElement(By.XPath("//input[@type='submit']"));
            botonBuscarDevolucion.Click();

            Thread.Sleep(2000);

            //driver.FindElement(By.LinkText("Devolver")).Click();

            IWebElement LibroDevolver = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('a',$(\"td:contains('Metamorfosis')\").parent())[0]");
            LibroDevolver.Click();

            Thread.Sleep(2000);
            IWebElement botonConfirmarDevolucion = driver.FindElement(By.XPath("//input[@type='submit']"));
            botonConfirmarDevolucion.Click();
            Thread.Sleep(2000);

        }
        private void TestVerCatalogo(IWebDriver driver, string nombreBoton)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement botonMantenimineto = driver.FindElement(By.XPath("//*[@id='acordeon']/ul/li[1]/p[2]"));
            botonMantenimineto.Click();

            Thread.Sleep(1000);
            IWebElement botonModulo = driver.FindElement(By.LinkText(nombreBoton));
            botonModulo.Click();

            CapturasTest(driver, ("Catalogo "));
        }
        private void TestRealizarReserva(IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            Actions action = new Actions(driver);
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("libros")));
            action.MoveToElement(driver.FindElement(By.Id("libros"))).Build().Perform();
            Thread.Sleep(2000);
 
            wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText("Reservar")));
            driver.FindElement(By.LinkText("Reservar")).Click();
            Thread.Sleep(2000);
            IWebElement botonReserva = driver.FindElement(By.XPath("//input[@type='submit']"));
            //driver.FindElement(By.LinkText("Reservar")).Click();
            CapturasTest(driver, "Realizar Reserva");
            botonReserva.Click();
            Thread.Sleep(2000);
        }
        public void CapturasTest(IWebDriver driver, string nombreArchivo)
        {
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(ScreenShotLocation + "\\" + nombreArchivo + " -Test.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
