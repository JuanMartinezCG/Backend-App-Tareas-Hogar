using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    public class UserToken
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Column("refresh_token")]
        [Required]
        public string RefreshToken { get; set; }

        [Column("refresh_token_expiration")]
        [Required]
        public DateTime RefreshTokenExpiration { get; set; }
        
        [Column("is_revoked")]
        [Required]
        public bool IsRevoked { get; set; } // Indica si el token ha sido revocado

        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; } // Fecha de creación del token

        [Column("revoked_at")]
        [Required]
        public DateTime? RevokedAt { get; set; } // Fecha de revocación del token, si aplica
        
        [Column("device_info")]
        public string DeviceInfo { get; set; } // Información del dispositivo

        [Column("ip_address")]
        public string IpAddress { get; set; } // Dirección IP desde la que se generó el token
    }

}
