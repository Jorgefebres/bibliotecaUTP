using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models.ViewModels
{
    
    public class VistaPrestamo
    {
        public List<PrestamoDetalle> ListaDetPrestamo { get; set; }
        public Persona Personas { get; set; }
        public PrestamoDetalle DetPrestamo { get; set; }
       // public Persona Personas { get; set; }
    }

    public class VistaReserva
    {
        public List<ReservaDetalle> ListaDetReserva { get; set; }
        public ReservaDetalle DetReserva { get; set; }
     //   public Persona Personas { get; set; }
    }

    public class VistaSansion
    {
        public VistaPrestamo VistaPrestamos { get; set; }
        public Sancion Sanciones { get; set; }
    }

}