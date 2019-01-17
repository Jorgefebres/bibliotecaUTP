namespace BIBLIOTECA.Models.Acciones
{
    /// <summary>
    /// Numeracion de las acciones
    /// </summary>
    public enum AccionesEnum
    {
        #region Admin
        Admin_Index = 1,
        #endregion

        #region Autor
        Autor_Listar = 2,
        Autor_Ver_Detalles = 3,
        Autor_Registrar_nuevo = 4,
        Autor_Editar = 5,
        Autor_Eliminar = 6,
        #endregion

        #region Categoria
        Categoria_Listar = 7,
        Categoria_Ver_Detalles = 8,
        Categoria_Registrar = 9,
        Categoria_Editar = 10,
        Categoria_Eliminar = 11,
        #endregion

        #region PanelBibliotecario
        PanelBibliotecario_Index = 12,
        #endregion

        #region Devolucion
        Devolucion_Listar = 13,
        Devolucion_Buscar_Persona = 14,
        Devolucion_Vista_Devolver = 15,
        Devolucion_Devover_Uno = 16,
        #endregion

        #region Editorial
        Editorial_Listar = 17,
        Editorial_Ver_Detalles = 18,
        Editorial_Registrar = 19,
        Editorial_Editar = 20,
        Editorial_Eliminar = 21,
        #endregion

        #region Libro
        Libro_Listar = 22,
        Libro_Ver_Detalles = 23,
        Libro_Registrar = 24,
        Libro_Editar = 25,
        Libro_Eliminar = 26,
        #endregion

        #region PanelUsuario
        PanelLector_Index = 27,
        #endregion

        #region Prestamo
        Prestamo_Listar = 28,
        Prestamo_Buscar_Persona = 29,
        Prestamo_Listar_Libro_Disponibles = 30,
        Prestamo_Agregar_Libro_Al_Prestamo = 31,
        Prestamo_Nuevo_Prestramo = 32,
        Prestamo_Cancelar = 33,
        #endregion

        #region PrestamoReserva
        PrestamoReserva_Listar_Reservas = 34,
        PrestamoReserva_Buscar_Persona = 35,
        PrestamoReserva_Buscar_Reserva = 36,
        PrestamoReserva_Prestar_uno = 37,
        PrestamoReserva_Cancelar = 38,
        #endregion

        #region Reservar
        Reservar_Listar = 39,
        Reservar_Nueva_Reserva = 40,
        Reservar_Agregar_A_La_REserva = 41,
        Reservar_Cancelar = 42,
        Reservar_CancelarBD = 43,
        #endregion

        #region Sancion
        Sancion_Listar = 44,
        Sancion_Devolver_Uno = 45,
        Sancion_Sancion_Vista= 46,
        #endregion

        #region Usuario
        Usuario_Listar = 47,
        Usuario_Entrar_al_Perfil = 48,
        Usuario_Editar_NomUsuario_Contrasena = 49,
        Usuario_Editar_Rol = 50,
        Usuario_Eliminar_Usuario = 51,
        Usuario_Logout = 52,
        #endregion

        #region Home
        Home_mipanel=53
        #endregion
    }
}