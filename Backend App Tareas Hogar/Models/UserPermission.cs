using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    [Table("user_permissions")] // Nombre de la tabla en la base de datos
    public class UserPermission
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }                 // Id del registro.

        [Column("user_id")]
        [Required]
        public Guid UserId { get; set; }            // Usuario que recibe el permiso.
        public User User { get; set; }

        [Column("permission_id")]
        [Required]
        public int PermissionId { get; set; }       // Permiso asignado.
        public Permission Permission { get; set; }
    }
}
