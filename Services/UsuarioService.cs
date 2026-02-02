using Atica.Data.Repositories;
using Atica.Models.ViewModels;
using Atica.Models;

namespace Atica.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync(string rolActual)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuarios para rol: {Rol}", rolActual);

                if (rolActual.EsAdministrador())
                {
                    return await _usuarioRepository.ObtenerTodosAsync();
                }

                return await _usuarioRepository.ObtenerPorRolAsync(rolActual);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                throw;
            }
        }

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Obteniendo usuario con ID: {Id}", id);
                return await _usuarioRepository.ObtenerPorIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID: {Id}", id);
                throw;
            }
        }

        public async Task<(bool exito, string mensaje, int? id)> CrearUsuarioAsync(UsuarioViewModel model)
        {
            try
            {
                _logger.LogInformation("Iniciando creación de usuario: {Email}", model.Email);

                if (await _usuarioRepository.ExisteDocumentoAsync(model.Documento))
                {
                    _logger.LogWarning("Intento de crear usuario con documento duplicado: {Documento}", model.Documento);
                    return (false, "Ya existe un usuario con ese documento", null);
                }

                if (await _usuarioRepository.ExisteEmailAsync(model.Email))
                {
                    _logger.LogWarning("Intento de crear usuario con email duplicado: {Email}", model.Email);
                    return (false, "Ya existe un usuario con ese email", null);
                }

                var usuario = new Usuario
                {
                    Nombre = model.Nombre.Trim(),
                    Apellido = model.Apellido.Trim(),
                    Documento = model.Documento.Trim(),
                    Email = model.Email.Trim().ToLower(),
                    Rol = model.Rol,
                    Activo = model.Activo,
                    FechaCreacion = DateTime.Now
                };

                var id = await _usuarioRepository.CrearAsync(usuario);

                _logger.LogInformation("Usuario creado exitosamente con ID: {Id}", id);
                return (true, "Usuario creado exitosamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return (false, "Error al crear el usuario. Intente nuevamente.", null);
            }
        }

        public async Task<(bool exito, string mensaje)> ActualizarUsuarioAsync(UsuarioViewModel model)
        {
            try
            {
                _logger.LogInformation("Iniciando actualización de usuario con ID: {Id}", model.Id);
                var usuarioExistente = await _usuarioRepository.ObtenerPorIdAsync(model.Id);
                if (usuarioExistente == null)
                {
                    _logger.LogWarning("Usuario no encontrado con ID: {Id}", model.Id);
                    return (false, "Usuario no encontrado");
                }

                if (await _usuarioRepository.ExisteDocumentoAsync(model.Documento, model.Id))
                {
                    _logger.LogWarning("Intento de actualizar con documento duplicado: {Documento}", model.Documento);
                    return (false, "Ya existe otro usuario con ese documento");
                }

                if (await _usuarioRepository.ExisteEmailAsync(model.Email, model.Id))
                {
                    _logger.LogWarning("Intento de actualizar con email duplicado: {Email}", model.Email);
                    return (false, "Ya existe otro usuario con ese email");
                }

                usuarioExistente.Nombre = model.Nombre.Trim();
                usuarioExistente.Apellido = model.Apellido.Trim();
                usuarioExistente.Documento = model.Documento.Trim();
                usuarioExistente.Email = model.Email.Trim().ToLower();
                usuarioExistente.Rol = model.Rol;
                usuarioExistente.Activo = model.Activo;
                var resultado = await _usuarioRepository.ActualizarAsync(usuarioExistente);

                if (resultado)
                {
                    _logger.LogInformation("Usuario actualizado exitosamente con ID: {Id}", model.Id);
                    return (true, "Usuario actualizado exitosamente");
                }

                return (false, "No se pudo actualizar el usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar usuario con ID: {Id}", model.Id);
                return (false, "Error al actualizar el usuario. Intente nuevamente.");
            }
        }

        public async Task<(bool exito, string mensaje)> EliminarUsuarioAsync(int id)
        {
            try
            {
                _logger.LogInformation("Iniciando eliminación de usuario con ID: {Id}", id);

                var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
                if (usuario == null)
                {
                    _logger.LogWarning("Usuario no encontrado con ID: {Id}", id);
                    return (false, "Usuario no encontrado");
                }

                var resultado = await _usuarioRepository.EliminarAsync(id);

                if (resultado)
                {
                    _logger.LogInformation("Usuario eliminado exitosamente con ID: {Id}", id);
                    return (true, "Usuario eliminado exitosamente");
                }

                return (false, "No se pudo eliminar el usuario");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar usuario con ID: {Id}", id);
                return (false, "Error al eliminar el usuario. Intente nuevamente.");
            }
        }

        public bool PuedeAccederADatos(string rolUsuario)
        {
            return !string.IsNullOrEmpty(rolUsuario);
        }
    }
}