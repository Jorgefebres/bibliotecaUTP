using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIBLIOTECA.Models.Metodos
{
    public class SHA1
    {

        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: BIBLIOTECA.0.1
        /// <summary>
        /// Pone al revez la cadena y luego encripta utilizando el algoritmo SHA1.
        /// </summary>
        /// <param name="value">Cadena a encriptar</param>
        /// <returns>Cadena encriptada</returns>
        public static string Encode(string value)
        {
            var cadena = value.ToCharArray();
            string alreves = "";
            for (int i = 0; i < cadena.Length; i++)
            {
                alreves += (i % 2) != 0 ? value[i] : i;
            }
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(alreves ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

       
        public static string SinEspaciado(string Cadena)
        {
            while (Cadena.Contains("  "))
            {
                Cadena = Cadena.Replace("  ", "");
            }
            return Cadena;
        }
    }
}