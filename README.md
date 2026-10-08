# Biblioteca.Web

Sistema de gestión bibliotecaria desarrollado con **ASP.NET Core MVC** y **Dapper**, correspondiente a la **Semana 08**.

## Tecnologías Utilizadas
* **.NET 10** / ASP.NET Core MVC
* **Dapper** (Micro ORM)
* **Microsoft.Data.SqlClient**
* **SQL Server** (`BibliotecaDB`)
* **Bootstrap 5** & **Bootstrap Icons**

## Módulos y Requerimientos
1. **Libros:** Catálogo activo con autores (`INNER JOIN`), buscador por título, detalle, creación con `<select>` de autores, edición y baja lógica (`Activo = 0`).
2. **Socios:** Listado de socios activos y registro con validación de DNI duplicado (`ModelState.AddModelError`).
3. **Reporte de Préstamos:** Filtro por rango de fechas (GET) con `INNER JOIN` entre préstamos, detalle, libros y socios.
4. **Punto Extra:** Implementación de vista parcial (`_LibroFila.cshtml`) para el renderizado de filas en el catálogo.

## Configuración y Ejecución
1. Ejecutar el script `StoredProcedures.sql` en la base de datos `BibliotecaDB`.
2. Verificar la cadena de conexión en `appsettings.json`.
3. Ejecutar el proyecto:
   ```bash
   dotnet run
   ```
