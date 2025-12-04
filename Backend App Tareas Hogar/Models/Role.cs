using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    [Table("roles")] // Nombre de la tabla en la base de datos
    public class Role
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }

        [Column("name")] // Nombre de la columna en la base de datos
        [Required] // No nulo
        public string Name { get; set; }

        // Relaciones
        // Relación con la tabla intermedia UserRole
        public ICollection<UserRole> UsersRole { get; set; }    // Usuarios que tienen este rol principal.

        // Relación con RolePermission
        public ICollection<RolePermission> RolePermissions { get; set; } // Permisos asociados al rol.
    }

}
