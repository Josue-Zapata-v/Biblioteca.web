USE BibliotecaDB;
GO

-- 1. Libros: Listar libros activos con nombre del autor
CREATE OR ALTER PROCEDURE sp_ListarLibrosActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        l.LibroId, 
        l.Titulo, 
        l.ISBN, 
        l.AutorId, 
        a.Nombre AS AutorNombre, 
        l.Ejemplares, 
        l.Activo
    FROM Libros l
    INNER JOIN Autores a ON l.AutorId = a.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo ASC;
END;
GO

-- 2. Libros: Búsqueda por título
CREATE OR ALTER PROCEDURE sp_BuscarLibrosPorTitulo
    @Titulo NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        l.LibroId, 
        l.Titulo, 
        l.ISBN, 
        l.AutorId, 
        a.Nombre AS AutorNombre, 
        l.Ejemplares, 
        l.Activo
    FROM Libros l
    INNER JOIN Autores a ON l.AutorId = a.AutorId
    WHERE l.Activo = 1 
      AND (@Titulo IS NULL OR @Titulo = '' OR l.Titulo LIKE '%' + @Titulo + '%')
    ORDER BY l.Titulo ASC;
END;
GO

-- 3. Libros: Obtener por Id
CREATE OR ALTER PROCEDURE sp_ObtenerLibroPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        l.LibroId, 
        l.Titulo, 
        l.ISBN, 
        l.AutorId, 
        a.Nombre AS AutorNombre, 
        l.Ejemplares, 
        l.Activo
    FROM Libros l
    INNER JOIN Autores a ON l.AutorId = a.AutorId
    WHERE l.LibroId = @LibroId;
END;
GO

-- 4. Libros: Insertar libro (Activo = 1)
CREATE OR ALTER PROCEDURE sp_InsertarLibro
    @Titulo NVARCHAR(200),
    @ISBN NVARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Libros (Titulo, ISBN, AutorId, Ejemplares, Activo)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares, 1);
    SELECT SCOPE_IDENTITY();
END;
GO

-- 5. Libros: Actualizar libro (sin SET NOCOUNT ON para que Dapper reciba filas afectadas)
CREATE OR ALTER PROCEDURE sp_ActualizarLibro
    @LibroId INT,
    @Titulo NVARCHAR(200),
    @ISBN NVARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    UPDATE Libros
    SET Titulo = @Titulo,
        ISBN = @ISBN,
        AutorId = @AutorId,
        Ejemplares = @Ejemplares
    WHERE LibroId = @LibroId;
END;
GO

-- 6. Libros: Eliminación lógica (sin SET NOCOUNT ON para que Dapper reciba filas afectadas)
CREATE OR ALTER PROCEDURE sp_EliminarLibro
    @LibroId INT
AS
BEGIN
    UPDATE Libros
    SET Activo = 0
    WHERE LibroId = @LibroId;
END;
GO

-- 7. Socios: Listar activos
CREATE OR ALTER PROCEDURE sp_ListarSociosActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        SocioId, 
        DNI, 
        Nombre, 
        Email, 
        Activo
    FROM Socios
    WHERE Activo = 1
    ORDER BY Nombre ASC;
END;
GO

-- 8. Socios: Insertar socio (Activo = 1)
CREATE OR ALTER PROCEDURE sp_InsertarSocio
    @DNI NVARCHAR(8),
    @Nombre NVARCHAR(120),
    @Email NVARCHAR(120) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Socios (DNI, Nombre, Email, Activo)
    VALUES (@DNI, @Nombre, @Email, 1);
    SELECT SCOPE_IDENTITY();
END;
GO

-- 9. Socios: Verificar si existe por DNI
CREATE OR ALTER PROCEDURE sp_ExisteSocioPorDNI
    @DNI NVARCHAR(8)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(1)
    FROM Socios
    WHERE DNI = @DNI;
END;
GO

-- 10. Autores: Listar autores activos para lista desplegable
CREATE OR ALTER PROCEDURE sp_ListarAutoresActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        AutorId, 
        Nombre, 
        Nacionalidad, 
        Activo
    FROM Autores
    WHERE Activo = 1
    ORDER BY Nombre ASC;
END;
GO

-- 11. Reporte de Préstamos: intervalo de fechas con INNER JOIN entre Prestamos, DetallePrestamo, Libros y Socios
CREATE OR ALTER PROCEDURE sp_ReportePrestamos
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        p.PrestamoId,
        s.Nombre AS SocioNombre,
        l.Titulo AS LibroTitulo,
        p.FechaPrestamo,
        p.FechaLimite,
        p.Estado,
        dp.FechaDevolucion
    FROM Prestamos p
    INNER JOIN Socios s ON p.SocioId = s.SocioId
    INNER JOIN DetallePrestamo dp ON p.PrestamoId = dp.PrestamoId
    INNER JOIN Libros l ON dp.LibroId = l.LibroId
    WHERE (@FechaDesde IS NULL OR p.FechaPrestamo >= @FechaDesde)
      AND (@FechaHasta IS NULL OR p.FechaPrestamo <= @FechaHasta)
    ORDER BY p.FechaPrestamo DESC, p.PrestamoId DESC;
END;
GO
