using System.ComponentModel.DataAnnotations;

namespace CarreraCFE.Models
{
    public class Corredor
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int Edad { get; set; }

        [Required]
        public string Genero { get; set; } = string.Empty; // Femenil, Varonil, Mixto/Otro

        [Required]
        public string CategoriaEdad { get; set; } = string.Empty; // Infantil (Niños), Adulto

        [Required]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Telefono { get; set; } = string.Empty;

        public bool EsEmpleadoCFE { get; set; }
        public string? RPE { get; set; }

        [Required]
        public string TallaPlayera { get; set; } = string.Empty; // Infantil (4, 6, 8, 10, 12) o Adulto (S, M, L, XL)

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
