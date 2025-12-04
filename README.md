# README Backend – Tareas del Hogar
**API REST construida con ASP.NET Core, CQRS y Clean Architecture**

Este repositorio contiene el backend de la aplicación **Tareas del Hogar**. El proyecto está diseñado para ser claro, mantenible y apropiado para trabajo colaborativo en GitHub.

---

## Descripción

El backend gestiona usuarios, autenticación (JWT), asignación de tareas y validaciones. Sigue principios de **Clean Architecture** y utiliza el patrón **CQRS** para separar comandos (escritura) y consultas (lectura).

---

## Estructura del repositorio

```bash
Backend App Tareas Hogar/
├─ Application/ 
│  └─ Users/
│  |  ├─ Login/
│  |  │  ├─ LoginCommand.cs
│  |  │  ├─ LoginHandler.cs
│  |  │  └─ LoginResponse.cs
│  |  └─ Register/
|  |
|  └─ Task/
|  |  ├─ CreateTask/
|  |
├─ Controllers/
│  └─ User/
│     ├─ Auth/
│     │  └─ AuthController.cs
│     └─ UserController.cs
|   
├─ Infraestructure/
│  ├─ Data/
│  │  └─ ApplicationDbContext.cs
|  |
│  ├─ Interfaces/
│  │  └─ IJwtService.cs
|  |
│  └─ Services/
│     └─ JwtService.cs
|
├─ Models/
│  ├─ Role.cs
│  ├─ User.cs
| 
├─ Properties/
│  └─ launchSettings.json
|
├─ Backend App Tareas Hogar.http
├─ Program.cs
└─ appsettings.json
```
---

## Características principales

- API REST desarrollada con **ASP.NET Core (8)**  
- Patrón **CQRS** (MediatR) para separar Commands y Queries  
- Validaciones con **FluentValidation** (cada Command tiene su validador)  
- Autenticación con **JWT**  
- Persistencia con **Entity Framework Core** y **PostgreSQL**  
- Middleware global de manejo de excepciones  
- Docker para la base de datos (PostgreSQL)  
- Enfoque en responsabilidad única

---

## Autenticación

- Endpoint de login que recibe `UserName` y `Password`.  
- Validación de credenciales y generación de token JWT devuelto en un objeto `AuthResult`.  
- El cliente usa el token para consumir endpoints protegidos.

---

## Middleware de excepciones

El proyecto incluye un middleware personalizado para capturar y normalizar errores:

- Manejo de errores de base de datos (p. ej. errores transitorios de Npgsql)  
- Errrores de validación y de autorización  
- Excepciones inesperadas con respuesta consistente para el cliente

---

## Base de datos con Docker

Comando sugerido para levantar PostgreSQL localmente:

```bash
docker run -d \
  --name tareas_hogar_db \
  -e POSTGRES_USER=tareas_admin \
  -e POSTGRES_PASSWORD=TareasHogar2025! \
  -e POSTGRES_DB=tareas_hogar \
  -p 5432:5432 \
  -v tareas_hogar_data:/var/lib/postgresql/data \
  postgres
```
---

## Configuración y ejecución

1. Clonar el repositorio
```bash
git clone https://github.com/tu-usuario/tu-repo.git
cd tu-repo
```

2. Configurar variables (ejemplo `.env` o `appsettings.json`)
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=tareas_hogar;Username=tareas_admin;Password=TareasHogar2025!"
}
```

3. Restaurar dependencias
```bash
dotnet restore
```

4. Ejecutar migraciones
```bash
cd src/Backend.App.TareasHogar.Api
dotnet ef database update --project ../Backend.App.TareasHogar.Infrastructure --startup-project .
```

5. Ejecutar la API
```bash
dotnet run --project src/Backend.App.TareasHogar.Api
```

---

## Endpoints principales (ejemplos)

- `POST /api/auth/login` — Login y obtención de JWT  
- `POST /api/users` — Registrar usuario (según roles y validaciones)  
- `GET /api/users/{id}` — Obtener usuario por ID
  
> Se añadira mas Endpoint de ejemplo a lo largo del proyecto

---

## Buenas prácticas y convenciones

- Cada Command tiene su propio Validator (FluentValidation) y su Handler (Responsabilidad Única).  
- Los servicios transversales (p. ej. `UserAccessorService`, `JwtService`) se ubican en `Infrastructure` según su naturaleza.  
- el DbContext viven en el proyecto `Infrastructure`.  
- Usa `record` para DTOs simples y `class` para Commands con muchos parámetros.

---

## Contribuciones

Si aceptas contribuciones externas, sigue estas indicaciones:

1. Hacer **fork** del repositorio.
2. Crear un **branch** con nombre `feature/xxx` o `fix/xxx`.
3. Abrir un **Pull Request (PR)** bien especificado y organizado:
   - Describe claramente los cambios realizados.
   - Indica la motivación y el problema que resuelve.
4. Se aceptan recomendaciones de código ya aplicado o mejoras a funciones existentes.
5. Mantén buenas prácticas de codificación y consistencia con el estilo del proyecto.

---

## Licencia

Puedes usarlo libremente para fines personales.

---

## Contacto

**Juan Camilo Martínez González**  
GitHub: https://github.com/JuanMartinezCG  
LinkedIn: https://www.linkedin.com/in/juan-camilo-oo199

---
