# Employee Management

Solución full stack para autenticación y administración de empleados. Incluye
una API desarrollada con .NET 10 y SQL Server, y un frontend con React y
TypeScript. El proyecto backend implementa una arquitectura por capas,
CQRS con Mediator, validación transversal, persistencia Code First y
autenticación JWT.

## Tecnologías

- .NET 10 y ASP.NET Core Web API.
- Entity Framework Core 10 con SQL Server.
- Mediator 3 con generación de código en tiempo de compilación.
- FluentValidation 12.
- JWT Bearer Authentication.
- SQL Server 2022 Developer sobre Docker.
- Scalar y OpenAPI 3.1.
- API Versioning.
- React 19 con TypeScript 6 y Vite 8.

## Arquitectura



La solución backend sigue clean architecture:

| Capa | Responsabilidad |
|---|---|
| **Domain** | Entidades, contratos de repositorios, Unit of Work, modelos de configuración y patrón de respuesta. |
| **Application** | Casos de uso CQRS, DTOs, handlers, validadores y behaviors de Mediator. |
| **Infrastructure** | EF Core, repositorios, Unit of Work, JWT, hash de contraseñas, migraciones, SP y seed. |
| **API** | Controllers, autorización, CORS, versionado, middleware de excepciones y OpenAPI. |

El frontend está organizado por funcionalidades:

```text
frontend/src/
├── api/                 Tipos y cliente HTTP
├── features/auth/       Inicio de sesión
├── features/employees/  Listado, filtro y formulario
├── App.tsx              Manejo de la sesión
└── main.tsx             Punto de entrada
```


## Ejecución local

### Requisitos

- .NET SDK 10.
- Docker Desktop.
- Node.js 20.19 o superior.

Los siguientes comandos deben ejecutarse en orden desde la raíz del
repositorio.

### 1. Levantar SQL Server con Docker

Abrir una terminal y ejecutar:

```powershell
docker compose up -d
docker compose ps
```

El servicio `sqlserver` debe aparecer con estado `healthy` antes de continuar.
Se utiliza SQL Server 2022 Developer en el puerto `1433` y un volumen
persistente para conservar la información.

### 2. Levantar el backend

En la misma terminal, con SQL Server ya disponible:

```powershell
dotnet restore
dotnet run --project .\EmployeeManagement.API\EmployeeManagement.API.csproj
```

Al iniciar, la API aplica las migraciones pendientes y ejecuta el seed de
manera idempotente. Debe quedar disponible en:

- API: `https://localhost:7059`
- Scalar: `https://localhost:7059/scalar`
- Health check: `https://localhost:7059/health`

Mantener esta terminal abierta mientras se utiliza el frontend.

### 3. Levantar el frontend

Abrir una segunda terminal desde la raíz del repositorio y ejecutar:

```powershell
cd .\frontend
npm install
npm run dev
```

Finalmente, abrir `http://localhost:5173` e iniciar sesión con las credenciales
de desarrollo indicadas en la siguiente sección.


## Credenciales de desarrollo

### Usuario de la aplicación

```text
Usuario:    admin
Contraseña: ProCredit2026*
```

Para obtener el token:

```http
POST /api/v1/authentication/login
Content-Type: application/json
```

```json
{
  "userName": "admin",
  "password": "ProCredit2026*"
}
```


### SQL Server local

```text
Servidor:   localhost,1433
Base:       EmployeeManagementDb
Usuario:    sa
Contraseña: EmployeeManagement#2026
```


## Patrones y decisiones técnicas

### CQRS y Mediator

Las operaciones se separan en **Commands** y **Queries** dentro de
`Application/Features`.

- `LoginCommand`: valida credenciales y genera el JWT.
- `CreateEmployeeCommand`: crea usuario, empleado y salario.
- `GetEmployeesQuery`: consulta empleados usando el procedimiento almacenado.
- `GetAreasQuery` y `GetPositionsQuery`: consultan catálogos.

Los controllers no contienen lógica de negocio. Solamente crean el mensaje,
lo envían a Mediator y convierten el resultado al código HTTP correspondiente.

### Pipeline Behaviors

Todos los mensajes pasan por comportamientos transversales antes del handler:

1. **LoggingBehavior** registra el inicio y final del caso de uso.
2. **PerformanceBehavior** alerta cuando una operación supera 500 ms.
3. **ValidationBehavior** ejecuta FluentValidation y evita llegar al handler
   cuando la solicitud es inválida.

Esto elimina validaciones, mediciones y logs repetidos dentro de cada handler.

### Repository y Unit of Work

Se utilizan dos unidades de trabajo con responsabilidades distintas:

- `IUnitOfWork`: operaciones de escritura, `SaveChangesAsync` y soporte para
  transacciones explícitas.
- `IReadUnitOfWork`: consultas de solo lectura con repositorios
  `AsNoTracking`.

La creación de un empleado agrega `User`, `Employee` y `EmployeeSalary`
como un solo grafo y ejecuta un único `SaveChangesAsync`, por lo que EF Core
lo persiste de manera atómica.

### Procedimiento almacenado

El listado de empleados se obtiene mediante:

```sql
EXEC [pro].[GetEmployees] @AreaId = NULL;
```

`EmployeeReadRepository` encapsula la ejecución del SP y lo proyecta a
`EmployeeReadModel`. El handler no conoce SQL ni depende directamente de EF:

```text
GetEmployeesQueryHandler
    → IReadUnitOfWork.Employees
    → EmployeeReadRepository
    → pro.GetEmployees
```

El SP devuelve área, cargo, salario y edad calculada. El filtro utiliza
`AreaId`, por lo que cambiar el nombre de un área no afecta sus relaciones.

### Pattern Response

Los casos de uso retornan un contrato uniforme:

- `EmptyResultDto`
- `ResultDto<T>`
- `ListResultDto<T>`
- `PaginatedResultDto<T>`

`BaseController` transforma `AppMessageType` en códigos HTTP:

| Resultado | HTTP |
|---|---:|
| Success | 200 |
| InvalidRequest | 400 |
| InvalidCredentials / Unauthorized | 401 |
| Forbidden | 403 |
| NotFound | 404 |
| ResourceAlreadyExists | 409 |
| UnknownError | 500 |

### Seguridad

- Todos los empleados tienen obligatoriamente un usuario de autenticación.
- La contraseña se almacena con `PasswordHasher<User>`, nunca en texto plano.
- La API genera y valida sus propios JWT mediante HMAC SHA-256.
- Los endpoints están protegidos por defecto con `[Authorize]`.
- Solamente el endpoint de login utiliza `[AllowAnonymous]`.
- CORS está limitado al origen configurado para el frontend.


## Modelo de datos

| Tabla | Relación |
|---|---|
| `asp.Users` | Información de autenticación. |
| `pro.Employees` | Relación 1:1 obligatoria con `asp.Users`. |
| `pro.Areas` | Relación 1:N con empleados. |
| `pro.Positions` | Relación 1:N con empleados. |
| `pro.EmployeeSalaries` | Relación 1:1 con empleados. |





## Endpoints

| Método | Ruta | Autenticación | Descripción |
|---|---|---|---|
| POST | `/api/v1/authentication/login` | No | Valida credenciales y genera JWT. |
| GET | `/api/v1/employees?areaId={id}` | Bearer | Lista o filtra empleados usando el SP. |
| POST | `/api/v1/employees` | Bearer | Crea usuario, empleado y salario. |
| GET | `/api/v1/areas` | Bearer | Lista las áreas. |
| GET | `/api/v1/positions` | Bearer | Lista los cargos. |
| GET | `/health` | No | Verifica que la API esté disponible. |

## Migraciones



Aplicar migraciones manualmente:

```powershell
dotnet tool run dotnet-ef database update `
  --project .\EmployeeManagement.Infrastructure\EmployeeManagement.Infrastructure.csproj `
  --startup-project .\EmployeeManagement.API\EmployeeManagement.API.csproj
```

Migraciones actuales:

1. `InitialCreate`: esquemas, tablas, índices, relaciones y creación inicial
   de `pro.GetEmployees`.
2. `UpdateGetEmployeesProcedureAreaFilter`: modifica el SP para filtrar por
   `AreaId`.
