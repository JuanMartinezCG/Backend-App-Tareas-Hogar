using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    [Table("user_roles")] // Nombre de la tabla en la base de datos
    public class UserRole
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }

        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }            // Usuario.
        public User User { get; set; }

        [Column("role_id")]
        [Required]
        public int RoleId { get; set; }             // Rol.
        public Role Role { get; set; }
    }

}
