# EmployeeManagement

## SQL Server con Docker

El entorno local utiliza SQL Server 2022 Developer en un contenedor con datos
persistentes. Desde la raíz de la solución ejecuta:

```powershell
docker compose up -d
docker compose ps
```

La API se conecta por `localhost,1433` utilizando estas credenciales locales:

- Usuario: `sa`
- Contraseña: `EmployeeManagement#2026`
- Base de datos: `EmployeeManagementDb`

Al iniciar la API se aplica automáticamente la migración pendiente y se ejecuta
el seed inicial:

```powershell
dotnet run --project .\EmployeeManagement.API\EmployeeManagement.API.csproj
```

Para detener SQL Server sin borrar la base:

```powershell
docker compose stop
```

Para volver a iniciarlo:

```powershell
docker compose start
```

> `docker compose down` elimina el contenedor, pero conserva el volumen y sus
> datos mientras no se agregue la opción `--volumes`.
