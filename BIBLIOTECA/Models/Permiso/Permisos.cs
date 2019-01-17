using BIBLIOTECA.Helper;
using BIBLIOTECA.Models.Acciones;
using System.Linq;

namespace BIBLIOTECA.Models.Permiso
{
    public class Permisos
    {
        static BIBLIOTECAContext db;
        public static bool TienePermiso(AccionesEnum idAccion)
        {
            var usuario = Permisos.Get();
            if (usuario != null)
            {
                bool verdad= usuario.Roles.AccionRoles.Where(x => x.IdPermiso == idAccion).Any();
                return verdad;
            }
            return false;
        }
        public static Usuario Get()
        {
            db = new BIBLIOTECAContext();
            int id = SessionHelper.GetUser();
            Usuario users = db.Usuarios.Find(id);
            return users;

        }
    }
}