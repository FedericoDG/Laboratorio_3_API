# 🏠 Inmobiliaria API – .NET 9 Web API

**API REST** para propietarios de inmuebles que desean hacer seguimiento de sus propiedades, inquilinos y pagos.  
Desarrollado en **ASP.NET Core Web API (.NET 9)** con base de datos **MySQL** y Entity Framework Core.  
Diseñado para ser consumido desde [Laboratorio_3_Android](https://github.com/FedericoDG/Laboratorio_3_Android)

---

## Arquitectura

### Features Implementadas

- **Autenticación JWT** - Login seguro para propietarios
- **Perfil Propietario** - Gestión de datos personales del propietario
- **Mis Inmuebles** - Catálogo personal de propiedades del propietario
- **Mis Contratos** - Seguimiento de alquileres activos con inquilinos y pagos

### Endpoints

```
POST   /api/auth/login                        # Login del propietario
GET    /api/auth/me                           # Datos del propietario autenticado

GET    /api/propietarios/me                   # Mi perfil como propietario
PUT    /api/propietarios/me                   # Actualizar mis datos

GET    /api/inmuebles/me                      # Listar mis inmuebles
GET    /api/inmuebles/mis-contratos-activos   # Listar mis inmuebles con contrato vigente
GET    /api/inmuebles/{id}                    # Listar el inmueble con determinada id
POST   /api/inmuebles                         # Registrar un nuevo inmueble
PUT    /api/inmuebles/id/{id}                 # Actualizar disponibilidad de un inmueble

POST   /api/alquileres                        # Crear contrato en un inmueble
```

---

## Requisitos previos

Antes de comenzar, asegurate de tener instalado:

- [Git](https://git-scm.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [SDK .NET 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

---

## Instalación y ejecución del proyecto

### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/FedericoDG/Laboratorio_3_API
cd inmobiliaria-api
```

### 2️⃣ Levantar base de datos MySQL con Docker

En la raíz del proyecto encontrarás un archivo `docker-compose.yml`.
Ejecuta:

```bash
docker compose up -d
```

Esto creará:

- **MySQL** en `localhost:3306`
- **phpMyAdmin** en `http://localhost:8080`

Credenciales por defecto (modificables en `docker-compose.yml`):

- **Usuario**: root
- **Password**: 1234
- **Base de datos**: inmobiliaria_api

### 3️⃣ Configurar la conexión en `appsettings.Development.json`

Verifica que la cadena de conexión esté configurada:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=inmobiliaria_api;user=root;password=1234"
  }
}
```

### 4️⃣ Restaurar dependencias y compilar

```bash
dotnet restore
dotnet build
```

### 5️⃣ Aplicar migraciones (incluye datos de ejemplo)

```bash
dotnet ef database update
```

Esto creará automáticamente:

- ✅ Estructura de tablas (Propietarios, Inmuebles, Inquilinos, Alquileres, Pagos)
- ✅ Datos de prueba (seeders) para testing

### 👤 Datos de Login para Pruebas

Una vez aplicadas las migraciones, puedes usar estos propietarios de prueba para hacer login:

| Email                      | Password | Propietario          |
| -------------------------- | -------- | -------------------- |
| `juan.gonzalez@email.com`  | `1234`   | Juan Carlos González |
| `maria.martinez@email.com` | `1234`   | María Elena Martínez |
| `carlos.lopez@email.com`   | `1234`   | Carlos Alberto López |

**Ejemplo de login:**

```bash
curl -X POST http://localhost:5119/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan.gonzalez@email.com",
    "password": "1234"
  }'
```

### 6️⃣ Ejecutar la API

```bash
dotnet run
```

La API estará disponible en:

- **HTTP**: `http://localhost:5119`

---
