using Atica.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Atica.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Conexión a la base de datos
        /// </summary>
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            using var connection = GetConnection();

            const string query = @"
                SELECT 
                    Id, 
                    Nombre, 
                    Apellido, 
                    Documento, 
                    Email, 
                    Rol, 
                    FechaCreacion, 
                    Activo
                FROM Usuarios
                WHERE Activo = 1
                ORDER BY Apellido, Nombre";

            return await connection.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            using var connection = GetConnection();

            const string query = @"
                SELECT 
                    Id, 
                    Nombre, 
                    Apellido, 
                    Documento, 
                    Email, 
                    Rol, 
                    FechaCreacion, 
                    Activo
                FROM Usuarios
                WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Usuario>(query, new { Id = id });
        }

        public async Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol)
        {
            using var connection = GetConnection();

            const string query = @"
                SELECT 
                    Id, 
                    Nombre, 
                    Apellido, 
                    Documento, 
                    Email, 
                    Rol, 
                    FechaCreacion, 
                    Activo
                FROM Usuarios
                WHERE Rol = @Rol AND Activo = 1
                ORDER BY Apellido, Nombre";

            return await connection.QueryAsync<Usuario>(query, new { Rol = rol });
        }

        public async Task<int> CrearAsync(Usuario usuario)
        {
            using var connection = GetConnection();

            const string query = @"
                INSERT INTO Usuarios (Nombre, Apellido, Documento, Email, Rol, Activo)
                VALUES (@Nombre, @Apellido, @Documento, @Email, @Rol, @Activo);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await connection.ExecuteScalarAsync<int>(query, usuario);
        }

        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            using var connection = GetConnection();

            const string query = @"
                UPDATE Usuarios
                SET 
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Documento = @Documento,
                    Email = @Email,
                    Rol = @Rol,
                    Activo = @Activo
                WHERE Id = @Id";

            var filasAfectadas = await connection.ExecuteAsync(query, usuario);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            using var connection = GetConnection();

            // Eliminación lógica (soft delete)
            const string query = @"
                UPDATE Usuarios
                SET Activo = 0
                WHERE Id = @Id";

            var filasAfectadas = await connection.ExecuteAsync(query, new { Id = id });
            return filasAfectadas > 0;
        }

        public async Task<bool> ExisteDocumentoAsync(string documento, int? idExcluir = null)
        {
            using var connection = GetConnection();

            const string query = @"
                SELECT COUNT(1)
                FROM Usuarios
                WHERE Documento = @Documento 
                  AND (@IdExcluir IS NULL OR Id != @IdExcluir)
                  AND Activo = 1";

            var count = await connection.ExecuteScalarAsync<int>(query,
                new { Documento = documento, IdExcluir = idExcluir });

            return count > 0;
        }

        public async Task<bool> ExisteEmailAsync(string email, int? idExcluir = null)
        {
            using var connection = GetConnection();

            const string query = @"
                SELECT COUNT(1)
                FROM Usuarios
                WHERE Email = @Email 
                  AND (@IdExcluir IS NULL OR Id != @IdExcluir)
                  AND Activo = 1";

            var count = await connection.ExecuteScalarAsync<int>(query,
                new { Email = email, IdExcluir = idExcluir });

            return count > 0;
        }
    }
}