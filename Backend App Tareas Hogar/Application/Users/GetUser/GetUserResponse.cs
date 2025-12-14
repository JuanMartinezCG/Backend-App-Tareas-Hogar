using Backend_App_Tareas_Hogar.Utilities.Dtos.User;

namespace Backend_App_Tareas_Hogar.Application.Users.GetUser
{
    public class GetUserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime DateRegister { get; set; }
        public DateTime DataUpdate { get; set; }
        public ICollection<RoleDto> Roles { get; set; }
        public ICollection<PermissionDto> Permissions { get; set; }
    }
}
