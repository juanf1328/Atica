using Atica.Models.ViewModels;
using Atica.Models;

namespace Atica.Services
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene la lista de usuarios según el rol del usuario actual
        /// Si es Administrador, obtiene todos; si es Usuario, solo los de su rol
        /// </summary>
        /// <param name="rolActual">Rol del usuario que realiza la consulta</param>
        Task<IEnumerable<Usuario>> ObtenerUsuariosAsync(string rolActual);

        /// <summary>
        /// Obtiene un usuario específico por su ID
        /// </summary>
        /// <param name="id">ID del usuario a buscar</param>
        Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// Valida que no exista documento o email duplicado
        /// </summary>
        /// <param name="model">Datos del usuario a crear</param>
        /// <returns>Tupla con resultado de la operación (éxito, mensaje, ID generado)</returns>
        Task<(bool exito, string mensaje, int? id)> CrearUsuarioAsync(UsuarioViewModel model);

        /// <summary>
        /// Actualiza los datos de un usuario existente
        /// Valida que no exista documento o email duplicado
        /// </summary>
        /// <param name="model">Datos actualizados del usuario</param>
        /// <returns>Tupla con resultado de la operación (éxito, mensaje)</returns>
        Task<(bool exito, string mensaje)> ActualizarUsuarioAsync(UsuarioViewModel model);

        /// <summary>
        /// Elimina (lógicamente) un usuario del sistema
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Tupla con resultado de la operación (éxito, mensaje)</returns>
        Task<(bool exito, string mensaje)> EliminarUsuarioAsync(int id);

        /// <summary>
        /// Valida si un usuario puede acceder a los datos según su rol
        /// </summary>
        /// <param name="rolUsuario">Rol del usuario a validar</param>
        bool PuedeAccederADatos(string rolUsuario);
    }
}
