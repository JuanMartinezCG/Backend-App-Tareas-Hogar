using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
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
        public ICollection<User> Users { get; set; }    // Usuarios que tienen este rol principal.
        public ICollection<RolePermission> RolePermissions { get; set; } // Permisos asociados al rol.
    }

}
