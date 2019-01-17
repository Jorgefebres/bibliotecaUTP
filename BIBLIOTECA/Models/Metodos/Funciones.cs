using System.Linq;
using System.Web;
using System.IO;
using System;

namespace BIBLIOTECA.Models.Metodos
{
    public class Funciones
    {
        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Genera el nombre de la caratula a través de la extencion de la imagen y el nuevo nombre que se envia
        /// </summary>
        /// <param name="CaratulaLibro">Nombre de la Imagen Original</param>
        /// <param name="nuevoNombre">Nombre de la imagen con lo que guardará</param>
        /// <returns><param name="NameImagen">El ISBN mas la extencion de la imagen</param></returns>
        public string NombreCaratula(HttpPostedFileBase CaratulaLibro, string nuevoNombre)
        {
            string NameImagen = "";
            if (CaratulaLibro != null)
            {
                string pic = Path.GetFileName(CaratulaLibro.FileName);
                string[] extenciones = { ".JPEG", ".png", ".jpeg", ".JPG", ".PNG", ".jpg" };
                int first = 0;
                for (int i = 0; i < extenciones.Length; i++)
                {
                    first = pic.IndexOf(extenciones[i]);
                    if (first != -1)
                    {
                        break;
                    }
                }
                int last = pic.Length;
                string extencion = pic.Substring(first, last - first); //Obtiene la extencion de la imagen
                NameImagen = nuevoNombre + extencion;//Concatena el nombre y la extencion de la imagen para guardar en una carpeta
            }
            return NameImagen;
        }


        /// Autor: Larota Ccoa Luis Eusebio
        /// Version: 0.1
        /// <summary>
        /// Obtiene una fecha aumentada o desminuida los dias a una fecha especificada
        /// </summary>
        /// <example><code>fechaRequerida=FehasMas(DateTime.Now,3)</code></example>
        /// <param name="fecha">Fecha especificada</param>
        /// <param name="dias">numero de dias de mas o menos</param>
        /// <returns>Devueleve una fecha de tipo DateTime</returns>
        public DateTime FehasMas(DateTime fecha, double dias)
        {
            return fecha.AddDays(dias);
        }

        public DateTime FehasNac(int anios)
        {
            DateTime fecha = DateTime.Now;

            return fecha.AddYears(-anios);
        }
    }
}