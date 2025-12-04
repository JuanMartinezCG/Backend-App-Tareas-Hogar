using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    [Table("roles_permissions")] // Nombre de la tabla en la base de datos
    public class RolePermission
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }                 // Id del registro.

        [Column("role_id")]
        [Required]
        public int RoleId { get; set; }             // Rol relacionado.
        public Role Role { get; set; }

        [Column("permission_id")]
        [Required]
        public int PermissionId { get; set; }       // Permiso del rol.
        public Permission Permission { get; set; }
    }

}
