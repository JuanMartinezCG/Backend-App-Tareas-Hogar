namespace Backend_App_Tareas_Hogar.Infraestructure.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(
            Guid userId, // ID del usuario
            string userName, // Nombre de usuario
            IEnumerable<string> roles // Roles del usuario
            );
    }

}
