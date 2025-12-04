using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    [Table("permissions")] // Nombre de la tabla en la base de datos
    public class Permission
    {
        [Key] // Clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincremental
        [Column("id")] // Nombre de la columna en la base de datos
        public int Id { get; set; }                      // Id del permiso.

        [Column("name")] // Nombre de la columna en la base de datos
        [Required] // No nulo
        public string Name { get; set; }                 // Nombre (ej: "Crear_Tareas").

        [Column("description")] // Nombre de la columna en la base de datos
        public string Description { get; set; }          // Qué hace el permiso.

        public ICollection<UserPermission> UserPermissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }

}
