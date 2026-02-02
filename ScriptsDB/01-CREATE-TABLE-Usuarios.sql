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
    CONSTRAINT [UK_Usuarios_Documento] UNIQUE NONCLUSTERED ([Documento] ASC),
    CONSTRAINT [UK_Usuarios_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [CK_Usuarios_Rol] CHECK ([Rol] IN ('Administrador', 'Usuario'))
);