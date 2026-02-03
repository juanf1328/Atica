# Atica
# Sistema de Gestión de Usuarios - Atica

Aplicación web desarrollada con **ASP.NET Core MVC (.NET 8)** como solución al challenge técnico de Atica. Este sistema permite la gestión completa de usuarios implementando una arquitectura en capas limpia y utilizando **Dapper** para el acceso a datos, sin dependencia de Entity Framework.

---

## Tabla de Contenidos

- [Descripción General](#-descripción-general)
- [Tecnologías Utilizadas](#-tecnologías-utilizadas)
- [Arquitectura del Proyecto](#-arquitectura-del-proyecto)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Funcionalidades Implementadas](#-funcionalidades-implementadas)
- [Decisiones Técnicas](#-decisiones-técnicas)
- [Capturas de Pantalla](#-capturas-de-pantalla)

---

## Descripción General

Este proyecto es una aplicación web full-stack que permite la administración de usuarios con las siguientes capacidades:

- **CRUD completo**: Crear, leer, actualizar y eliminar usuarios
- **Validaciones robustas**: Tanto en el cliente como en el servidor
- **Sistema de roles**: Diferenciación entre Administradores y Usuarios comunes
- **Interfaz moderna**: Diseño responsive con Bootstrap 5
- **Confirmaciones visuales**: Modal de confirmación antes de eliminar registros

El sistema fue desarrollado siguiendo los principios de **arquitectura limpia** y **separación de responsabilidades**, facilitando el mantenimiento y la escalabilidad del código.

---

## Tecnologías Utilizadas

### Backend
- **Framework**: ASP.NET Core MVC 8.0
- **Lenguaje**: C# 12
- **Base de Datos**: SQL Server
- **Acceso a Datos**: Dapper 2.1.28 (ADO.NET)
- **Inyección de Dependencias**: .NET Core DI Container

### Frontend
- **UI Framework**: Bootstrap 5.3
- **Iconos**: Bootstrap Icons 1.11
- **Validaciones Cliente**: jQuery Validation + Unobtrusive Validation

### Herramientas de Desarrollo
- Visual Studio 2022
- SQL Server Management Studio
- Git
---

## Arquitectura del Proyecto

Implementé una arquitectura en tres capas bien definida para mantener el código organizado y desacoplado:

```
Atica/
│
├── 📁 Controllers/              # Capa de Presentación
│   ├── UsuariosController.cs   # Manejo de peticiones HTTP y flujo MVC
│   └── HomeController.cs       # Controlador principal
│
├── 📁 Services/                 # Capa de Lógica de Negocio
│   ├── IUsuarioService.cs      # Contrato del servicio
│   └── UsuarioService.cs       # Validaciones y reglas de negocio
│
├── 📁 Data/Repositories/        # Capa de Acceso a Datos
│   ├── IUsuarioRepository.cs   # Contrato del repositorio
│   └── UsuarioRepository.cs    # Acceso a BD con Dapper
│
├── 📁 Models/                   # Modelos de Dominio
│   ├── Usuario.cs              # Entidad principal
│   ├── RolUsuario.cs           # Enumeración de roles
│   └── ViewModels/             # Modelos para las vistas
│       └── UsuarioViewModel.cs
│
└── 📁 Views/                    # Vistas Razor
    ├── Usuarios/               # Vistas del módulo de usuarios
    └── Shared/                 # Layout y componentes compartidos
```

### Flujo de Datos

```
Usuario (Navegador)
      ↓
   Vista Razor
      ↓
   Controller ──→ Validaciones
      ↓
   Service ──→ Lógica de Negocio
      ↓
   Repository ──→ Consultas SQL con Dapper
      ↓
   SQL Server
```

---

## 📋 Requisitos Previos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- ✅ [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (versión 8.0 o superior)
- ✅ [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

**Verificar instalación de .NET:**
```bash
dotnet --version
# Debe mostrar 8.0.x
```

---

## Instalación y Configuración

### Paso 1: Clonar el Proyecto

```bash
# Si usas Git
git clone https://github.com/juanf1328/Atica
cd Atica

# O simplemente descomprimir el ZIP descargado
```

### Paso 2: Configurar la Base de Datos

#### 2.1. Crear la Base de Datos

Abre **SQL Server Management Studio** y ejecuta los siguientes scripts en orden:

**Script 1: Crear la base de datos**
```sql
USE master;
GO

CREATE DATABASE Atica;
GO

USE Atica;
GO
```

**Script 2: Crear la tabla Usuarios**
```sql
CREATE TABLE [dbo].[Usuarios] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Apellido] NVARCHAR(100) NOT NULL,
    [Documento] NVARCHAR(20) NOT NULL,
    [Email] NVARCHAR(150) NOT NULL,
    [Rol] NVARCHAR(50) NOT NULL,
    [FechaCreacion] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
    [Activo] BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Usuarios_Documento] UNIQUE ([Documento] ASC),
    CONSTRAINT [UK_Usuarios_Email] UNIQUE ([Email] ASC),
    CONSTRAINT [CK_Usuarios_Rol] CHECK ([Rol] IN ('Administrador', 'Usuario'))
);
GO
```

**Script 3: Insertar datos de prueba** (Opcional)
```sql
INSERT INTO [dbo].[Usuarios] ([Nombre], [Apellido], [Documento], [Email], [Rol], [Activo])
VALUES 
    ('Juan', 'Pérez', '12345678', 'juan.perez@atica.com', 'Administrador', 1),
    ('María', 'González', '23456789', 'maria.gonzalez@atica.com', 'Usuario', 1),
    ('Carlos', 'Rodríguez', '34567890', 'carlos.rodriguez@atica.com', 'Usuario', 1),
    ('Ana', 'Martínez', '45678901', 'ana.martinez@atica.com', 'Administrador', 1),
    ('Luis', 'Fernández', '56789012', 'luis.fernandez@atica.com', 'Usuario', 1);
GO
```

#### 2.2. Configurar la Cadena de Conexión

Abre el archivo `Atica/appsettings.json` y ajusta la cadena de conexión según tu configuración de SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Atica;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```
### Paso 3: Restaurar Dependencias

Abre una terminal en la carpeta raíz del proyecto y ejecuta:

```bash
dotnet restore
```

Esto descargará los paquetes NuGet necesarios (Dapper y Microsoft.Data.SqlClient).

### Paso 4: Compilar el Proyecto

```bash
dotnet build
```

Verifica que la compilación sea exitosa sin errores.

### Paso 5: Ejecutar la Aplicación

```bash
dotnet run --project Atica
```

O simplemente:
```bash
cd Atica
dotnet run
```

La aplicación se ejecutará y mostrará algo como:

### Paso 6: Acceder a la Aplicación

Desde el navegador abrir:

**https://localhost:5001** o **http://localhost:5000**

---

## Funcionalidades Implementadas

### 1. Listado de Usuarios

- Tabla con todos los usuarios activos
- Visualización de: nombre, apellido, documento, email, rol y estado
- Badges visuales para diferenciar roles (Administrador en rojo, Usuario en azul)
- Indicador de estado activo/inactivo
- Diseño responsive que se adapta a dispositivos móviles

### 2. Crear Usuario

- Formulario completo con validaciones en tiempo real
- Campos validados:
  - **Nombre y Apellido**: Obligatorios, máximo 100 caracteres
  - **Documento**: Obligatorio, solo números, máximo 20 caracteres, único en el sistema
  - **Email**: Obligatorio, formato válido, único en el sistema
  - **Rol**: Selección entre Administrador o Usuario
  - **Estado**: Switch para activar/desactivar usuario
- Validaciones visuales con mensajes de error específicos
- Prevención de duplicados (documento y email)

### 3. Editar Usuario

- Formulario precargado con datos actuales
- Mismas validaciones que en creación
- Validación de unicidad excluyendo el propio usuario
- Feedback visual al guardar cambios

### 4. Eliminar Usuario

- Modal de confirmación Bootstrap antes de eliminar
- Muestra el nombre completo del usuario a eliminar
- Eliminación lógica (baja lógica) - marca como inactivo en lugar de borrar definitivamente
- Alertas de éxito o error tras la operación

### 5. Sistema de Roles

Implementé un sistema básico de roles que simula permisos diferentes:

- **Administrador**: Puede ver todos los usuarios del sistema
- **Usuario común**: Solo puede ver usuarios con rol "Usuario"

Para cambiar el rol simulado, edita la línea 17 en `Controllers/UsuariosController.cs`:

```csharp
private const string RolActual = "Administrador"; // o "Usuario"
```

### 6. Validaciones Implementadas

#### Validaciones del Lado del Cliente (JavaScript)
- Campos obligatorios
- Formato de email
- Solo números en documento
- Longitud máxima de campos
- Feedback visual inmediato

#### Validaciones del Lado del Servidor (C#)
- Verificación de documento único
- Verificación de email único
- Validación de ModelState
- Manejo de errores con logging

### 7. Experiencia de Usuario

- **Alertas con auto-cierre**: Los mensajes de éxito/error se ocultan automáticamente después de 5 segundos
- **Animaciones suaves**: Transiciones CSS para mejor experiencia visual
- **Diseño responsive**: Funciona en móviles, tablets y escritorio
- **Iconos intuitivos**: Bootstrap Icons para acciones (editar, eliminar)
- **Feedback constante**: El usuario siempre sabe el resultado de sus acciones
---

## Decisiones Técnicas

### Patrón Repository

Implementé el **patrón Repository** para:

- Separar la lógica de acceso a datos del resto de la aplicación
- Facilitar el testing unitario (para mockear fácilmente)
- Permitir cambios en la implementación sin afectar otras capas
- Centralizar las operaciones de base de datos

### Inyección de Dependencias

Configuré DI en `Program.cs` para:

- Desacoplar componentes
- Facilitar el mantenimiento
- Permitir testing
- Seguir principios SOLID

```csharp
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
```

### ViewModels separados de Entidades

Creé ViewModels separados porque:

- Las entidades no deben tener anotaciones de validación de UI
- Permite tener diferentes modelos para diferentes vistas
- Separa las preocupaciones de persistencia y presentación
- Facilita la evolución independiente de UI y base de datos

### Baja Lógica

Implementé baja lógica en lugar de baja física:

- Permite recuperar registros eliminados accidentalmente
- Mantiene la integridad referencial
- Facilita auditoría y trazabilidad
- Es una mejor práctica en sistemas de producción (utilizada en proyectos productivos en los que he liderado/participado)

### Bootstrap 5 para UI
---

### Listado de Usuarios
Interfaz principal mostrando todos los usuarios con opciones de editar y eliminar.

### Formulario de Creación
Formulario completo con validaciones visuales en tiempo real.

### Modal de Confirmación
Confirmación antes de eliminar un usuario para prevenir acciones accidentales.

### Validaciones Activas
Mensajes de error específicos cuando se intenta crear un usuario con email duplicado.

---

## Principios Aplicados

Durante el desarrollo, me enfoqué en aplicar:

### SOLID
- **S**ingle Responsibility: Cada clase tiene una única responsabilidad clara
- **O**pen/Closed: El código es extensible mediante interfaces sin modificar implementaciones existentes
- **L**iskov Substitution: Las implementaciones pueden sustituir sus abstracciones sin problemas
- **I**nterface Segregation: Interfaces específicas y cohesivas
- **D**ependency Inversion: Dependencias basadas en abstracciones, no en implementaciones concretas

### Clean Code

- Nombres descriptivos de variables y métodos
- Métodos pequeños con una sola responsabilidad
- Comentarios XML en clases y métodos públicos
- Manejo consistente de errores
- Logging estructurado
---

## Testing Manual

Se han realizado las siguientes pruebas de funcionalidades:

| Caso de Prueba | Resultado |
|---------------|-----------|
| Listar usuarios vacío | ✅ Muestra mensaje "No hay usuarios" |
| Listar usuarios con datos | ✅ Muestra tabla completa |
| Crear usuario válido | ✅ Crea y muestra mensaje de éxito |
| Crear usuario con email duplicado | ✅ Muestra error de validación |
| Crear usuario con documento duplicado | ✅ Muestra error de validación |
| Crear usuario con campos vacíos | ✅ Validaciones visuales activas |
| Crear usuario con email inválido | ✅ Validación de formato |
| Editar usuario exitosamente | ✅ Actualiza y confirma |
| Editar con datos duplicados | ✅ Muestra error apropiado |
| Eliminar usuario | ✅ Modal de confirmación, soft delete |
| Cancelar eliminación | ✅ No elimina el registro |
| Responsive mobile | ✅ Adapta correctamente |
| Responsive tablet | ✅ Adapta correctamente |

---

## Mejoras Futuras

Si tuviera más tiempo, podría implementarse:

1. **Sistema de Autenticación Real**
   - Login con ASP.NET Identity
   - JWT tokens para API
   - Roles basados en sesión real

2. **Paginación**
   - Implementar paginado en el listado de usuarios
   - Búsqueda y filtros avanzados

3. **Testing Automatizado**
   - Unit tests

4. **Auditoría**
   - Tabla de logs de cambios
   - Tracking de quién modificó qué y cuándo
   - Logs de errores

5. **API REST**
   - Endpoints RESTful adicionales
   - Documentación con Swagger
   - Versionado de API

6. **Exportación de Datos**
   - Exportar usuarios a Excel
   - Exportar a PDF

7. **Mejoras visuales**
   - Mejorar la experiencia visual para el usuario (paleta de colores más orgánica)

---

### Configuración de Desarrollo

El proyecto usa la configuración estándar de ASP.NET Core MVC:

- **Hot Reload** habilitado en desarrollo
- **HTTPS** configurado por defecto
- **Logging** a consola y debug en desarrollo

### Estructura de Connection String

Si tienes problemas de conexión, verifica:

1. Que SQL Server esté ejecutándose
2. Que el nombre de la instancia sea correcto
3. Que la autenticación Windows esté habilitada (Trusted_Connection=True)

### Solución de Problemas Comunes

**Error: "Cannot open database 'Atica'"**
- Solución: Ejecuta primero los scripts SQL de creación

**Error: "A network-related error occurred"**
- Solución: Verifica que SQL Server Express esté ejecutándose

**Error de compilación**
- Solución: Ejecuta `dotnet clean` y luego `dotnet restore` (tambien verificar tener instalado la extension dotnet)

---

## Autor

Juan Ignacio Forni
- Desarrollado como parte del challenge técnico para Atica
- Fecha de desarrollo: Febrero 2025
- Tecnologías: ASP.NET Core 8, Dapper, SQL Server, Bootstrap 5

---

## Licencia

Este proyecto fue desarrollado específicamente para el proceso de selección de Atica.

---

## Agradecimientos

Gracias a Atica por la oportunidad de demostrar mis habilidades técnicas mediante este challenge. He puesto especial atención en:

- Código limpio y mantenible
- Arquitectura escalable
- Buenas prácticas de desarrollo
- Experiencia de usuario intuitiva
- Documentación clara y completa

Espero que esta solución demuestre mi capacidad para desarrollar aplicaciones web robustas y profesionales.

---