using Atica.Models;

namespace Atica.Data.Repositories
{
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Obtiene todos los usuarios activos del sistema
        /// </summary>
        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un usuario por su identificador
        /// </summary>
        /// <param name="id">ID del usuario</param>
        Task<Usuario?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtiene usuarios filtrados por rol
        /// </summary>
        /// <param name="rol">Rol a filtrar</param>
        Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol);

        /// <summary>
        /// Crea un nuevo usuario en el sistema
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear</param>
        /// <returns>ID del usuario creado</returns>
        Task<int> CrearAsync(Usuario usuario);

        /// <summary>
        /// Actualiza los datos de un usuario existente
        /// </summary>
        /// <param name="usuario">Datos actualizados del usuario</param>
        /// <returns>True si se actualizó correctamente</returns>
        Task<bool> ActualizarAsync(Usuario usuario);

        /// <summary>
        /// Elimina lógicamente un usuario (usuario inactivo)
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> EliminarAsync(int id);

        /// <summary>
        /// Verifica si existe un usuario con el documento especificado
        /// </summary>
        /// <param name="documento">Documento a verificar</param>
        /// <param name="idExcluir">ID a excluir de la búsqueda</param>
        Task<bool> ExisteDocumentoAsync(string documento, int? idExcluir = null);

        /// <summary>
        /// Verifica si existe un usuario con el email especificado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <param name="idExcluir">ID a excluir de la búsqueda</param>
        Task<bool> ExisteEmailAsync(string email, int? idExcluir = null);
    }
}
