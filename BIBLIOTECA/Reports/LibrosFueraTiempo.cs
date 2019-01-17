using System;

namespace BIBLIOTECA.Reports
{
    public class LibrosFueraTiempo
    {
        public int id { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string FecPretamo { get; set; }
        public string FecDevolucion { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string Bibliotecario { get; set; }
        public string Estado { get; set; }
        public int Retardo { get; set; }
    }
}