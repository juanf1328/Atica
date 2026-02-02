namespace Atica.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Documento { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
