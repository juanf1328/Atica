namespace Atica.Models
{
    /// <summary>
    /// Roles del sistema
    /// </summary>
    public enum RolUsuario
    {
        Administrador,
        Usuario
    }
    public static class RolExtensions
    {
        /// <summary>
        /// Verificación rol Administrador
        /// </summary>
        public static bool EsAdministrador(this string rol)
        {
            return rol?.Equals("Administrador", StringComparison.OrdinalIgnoreCase) ?? false;
        }

        /// <summary>
        /// Verificación rol Usuario común
        /// </summary>
        public static bool EsUsuarioComun(this string rol)
        {
            return rol?.Equals("Usuario", StringComparison.OrdinalIgnoreCase) ?? false;
        }
    }
}