using System;

namespace BIBLIOTECA.Reports
{
    public class LibrosReservados
    {
        public int id { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string FecReserva { get; set; }
        public string FecRecojo { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
    }
}