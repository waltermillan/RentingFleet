# RentingFleet

Microservicio REST para la gestión de una flota de vehículos de renting, desarrollado con **.NET 9**, siguiendo los principios de **Clean Architecture**, **Hexagonal Architecture (Ports and Adapters)** y **Domain-Driven Design (DDD)**.

## 1. Objetivo

El microservicio permite gestionar el ciclo de vida de los vehículos de una flota:

- Incorporar vehículos a la flota.
- Consultar vehículos disponibles.
- Alquilar vehículos a clientes.
- Registrar la devolución de vehículos.

## 2. Tecnologías utilizadas

- .NET 9 / ASP.NET Core Web API
- C#
- MongoDB 8
- MongoDB.Driver
- Docker
- Docker Compose
- Swagger / OpenAPI
- xUnit

## 3. Arquitectura

La solución utiliza una arquitectura basada en separación de responsabilidades, casos de uso y puertos y adaptadores.

### Estructura de proyectos

| Proyecto | Responsabilidad |
|---|---|
| `GtMotive.Estimate.Microservice.Domain` | Entidades y reglas de negocio. |
| `GtMotive.Estimate.Microservice.ApplicationCore` | Casos de uso, contratos y lógica de aplicación. |
| `GtMotive.Estimate.Microservice.Infrastructure` | Implementación de persistencia y acceso a MongoDB. |
| `GtMotive.Estimate.Microservice.Api` | Controladores REST y presenters. |
| `GtMotive.Estimate.Microservice.Host` | Configuración, inyección de dependencias y arranque. |

### Casos de uso

- `AddVehicleUseCase`
- `GetAvailableVehiclesUseCase`
- `RentVehicleUseCase`
- `ReturnVehicleUseCase`

### Persistencia

MongoDB utiliza las siguientes colecciones:

- `Vehicles`: información de los vehículos.
- `Rentals`: información de los alquileres y devoluciones.

Los identificadores de las entidades se almacenan como GUID.

## 4. Reglas de negocio

1. Un cliente no puede tener más de un alquiler activo simultáneamente.
2. Un vehículo no puede tener más de un alquiler activo simultáneamente.
3. La flota no puede incorporar vehículos con más de cinco años de antigüedad.
4. Un vehículo deja de estar disponible cuando tiene un alquiler activo.
5. Al devolver un vehículo se registra la fecha de devolución y vuelve a estar disponible.

## 5. API REST

La API expone los siguientes endpoints:

| Método | Endpoint | Descripción | Respuesta |
|---|---|---|---|
| POST | `/api/Vehicles` | Incorporar vehículo | 204 No Content |
| GET | `/api/AvailableVehicles` | Consultar vehículos disponibles | 200 OK |
| POST | `/api/Rentals` | Alquilar vehículo | 200 OK |
| POST | `/api/Returns/{vehicleId}` | Devolver vehículo | 200 OK |

### 5.1. Incorporar vehículo

`POST /api/Vehicles`

Request:

```json
{
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2024
}
```

Response: `204 No Content`.

### 5.2. Consultar vehículos disponibles

`GET /api/AvailableVehicles`

Response:

```json
{
  "vehicles": [
    {
      "id": "a1111111-1111-4111-8111-111111111111",
      "brand": "Toyota",
      "model": "Corolla",
      "year": 2024
    }
  ]
}
```

### 5.3. Alquilar vehículo

`POST /api/Rentals`

Request:

```json
{
  "vehicleId": "a1111111-1111-4111-8111-111111111111",
  "customerId": "b1111111-1111-4111-8111-111111111111"
}
```

Response:

```json
{
  "rentalId": "c1111111-1111-4111-8111-111111111111"
}
```

### 5.4. Devolver vehículo

`POST /api/Returns/{vehicleId}`

Ejemplo:

`POST /api/Returns/a1111111-1111-4111-8111-111111111111`

Response:

```json
{
  "rentalId": "c1111111-1111-4111-8111-111111111111"
}
```

## 6. Ejecución local

### Requisitos

- .NET SDK 9.
- MongoDB 8.

Configurar la conexión en:

`src/GtMotive.Estimate.Microservice.Host/appsettings.Development.json`

```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27018",
    "MongoDbDatabaseName": "rentingfleet"
  }
}
```

Iniciar la aplicación desde la raíz de la solución:

```powershell
dotnet run --project .\src\GtMotive.Estimate.Microservice.Host
```

URL de la API:

`http://localhost:56860`

Swagger:

`http://localhost:56860/swagger`

## 7. Ejecución con Docker Compose

La solución incluye Docker Compose para ejecutar la API y MongoDB sin necesidad de instalar estas dependencias localmente.

Desde la raíz del repositorio:

```powershell
docker compose up --build -d
```

Servicios:

| Servicio | Dirección |
|---|---|
| API | http://localhost:56860 |
| Swagger | http://localhost:56860/swagger |
| MongoDB | localhost:27018 |

La base de datos utiliza un volumen persistente denominado `rentingfleet-mongo-data`.

El directorio `docker/mongo-init` contiene el script de inicialización automática de la base de datos, que inserta cinco vehículos de ejemplo cuando MongoDB inicia por primera vez con un volumen vacío.

Para detener los servicios:

```powershell
docker compose down
```

Los datos persisten porque el volumen no se elimina al detener los contenedores.

## 8. Pruebas automatizadas

La solución incluye tres proyectos de pruebas:

| Proyecto | Tipo |
|---|---|
| `GtMotive.Estimate.Microservice.UnitTests` | Pruebas unitarias |
| `GtMotive.Estimate.Microservice.FunctionalTests` | Pruebas funcionales |
| `GtMotive.Estimate.Microservice.InfrastructureTests` | Pruebas de infraestructura |

Ejecutar todas las pruebas desde la raíz:

```powershell
dotnet test .\src\microservice.sln
```

Última ejecución de validación:

- Total: 9 pruebas.
- Correctas: 9.
- Fallidas: 0.
- Omitidas: 0.

## 9. Configuración y ejecución

La configuración de MongoDB se realiza mediante `appsettings.Development.json` para ejecución local y variables de entorno para Docker Compose.

La API utiliza Swagger para explorar y probar los endpoints REST.

