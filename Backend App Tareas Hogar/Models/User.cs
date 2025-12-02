using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_App_Tareas_Hogar.Models
{
    public class User
    {
        [Key] // Clave primaria
        [Column("id")] // Nombre de la columna en la base de datos
        public Guid Id { get; set; }

        [Column("name")] // Nombre de la columna en la base de datos
        [Required(ErrorMessage = "El nombre es Obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")] // No puede estar vacío
        public string Name { get; set; }

        [Column("lastname")] // Nombre de la columna en la base de datos
        [Required(ErrorMessage = "El apellido es Obligatorio")] // Campo obligatorio
        [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")] // No puede estar vacío
        public string LastName { get; set; }

        [Column("username")] // Nombre de la columna en la base de datos
        [Required(ErrorMessage = "El nombre de usuario es Obligatorio")] // Campo obligatorio
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede exceder 50 caracteres")] // No puede estar vacío
        public string Username { get; set; }

        [Column("password")] // Nombre de la columna en la base de datos
        [Required(ErrorMessage = "La contraseña es Obligatoria")] // Campo obligatorio
        public string Password { get; set; }

        // Rol principal (FK)
        [Column("role_id")] // Nombre de la columna en la base de datos 
        [ForeignKey("Role")]
        [Required(ErrorMessage = "El rol es obligatorio")] // Campo obligatorio 
        public int RoleId { get; set; }                             
        public Role Role { get; set; }                              

        // Relaciones adicionales
        public ICollection<UserRole> UserRoles { get; set; }        // Si se permiten múltiples roles.
        public ICollection<UserPermission> UserPermissions { get; set; } // Permisos asignados manualmente.
        public ICollection<UserToken> Tokens { get; set; } // Tokens de autenticación.

        // Relación con tareas
        /*
        public ICollection<TaskEntity> TasksAssignedBy { get; set; } // Tareas asignadas por este usuario.
        public ICollection<TaskEntity> TasksAssignedTo { get; set; } // Tareas asignadas a este usuario.
        */
    }

}
