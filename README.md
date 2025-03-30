# Sistema de Gestión de Facturas

Este proyecto permite gestionar facturas y notas de crédito de forma sencilla, cargando datos desde un archivo JSON y manipulándolos mediante una API REST (.NET) y una interfaz web (React).

---

## Tecnologías

- Backend: .NET 8 + Entity Framework Core + SQLite
- Frontend: React + TailwindCSS@4
- Base de datos: SQLite
- Autenticación: JWT 

---

## Instalación y Ejecución

### Backend

1. Clonar repositorio
1. Ir a la carpeta `/backend`
2. Restaurar paquetes

   ```bash
     dotnet restore
   ```
3. Crear base de datos y aplicar migración

   ```bash
     dotnet ef database update --project ./FinixApp/backend.csproj
   ```
4. Ejecutar el backend

    ```bash
     dotnet run
5. Se abrira swagger en https://localhost:7154/swagger o http://localhost:5034/swagger

6. Iniciar el flujo frontend en https://github.com/moragaMarcos/invoice-app-cli


## Patrones de Diseño Aplicados

- **Service Layer**: la lógica de negocio se encapsula en servicios que separan las reglas del dominio de los controladores, facilitando la escalabilidad y el mantenimiento.

- **Repository Pattern**: permite abstraer el acceso a datos, separando la lógica de persistencia y facilitando pruebas unitarias.

- **Singleton**: el contexto de autenticación usan el ciclo de vida singleton (única instaancia).

- **Dependency Injection**: ASP.NET Core permite inyectar servicios, repositorios y contexto de base de datos.


Nota: Al ejecutar el projecto la db finix_database.db, se pobla con los datos en bd_exam.json.

Nota 2: Al querer reiniciar los datos de la db con los que se tiene en bd_exam.json, eliminar archivo finix_database.db 
y volver al paso 4
