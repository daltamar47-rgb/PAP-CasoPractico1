# Caso Práctico 1: cómo ponerlo a funcionar

## 1. Base de datos
1. Abra SQL Server Management Studio y conéctese a su servidor (el mismo de la cadena de conexión, por ejemplo `.\SQLEXPRESS`).
2. Ejecute el script `PAP.BookStoreDB/Scripts/CasoPractico1_BookStoreDB.sql`.
   - Crea las 19 tablas nuevas, agrega las columnas y llaves foráneas a `Books` (que ya existe) e inserta los roles Admin, Manager y Cashier.
   - Se puede ejecutar más de una vez sin romper nada.

## 2. Actualizar el data model (.edmx)
1. Abra `PAP.Initial.sln` en Visual Studio.
2. En `PAP.DataAccess`, abra `DataModel.edmx`.
3. Haga clic derecho en un espacio vacío del diagrama y elija **Update Model from Database...**.
4. En la pestaña **Add**, marque **Tables** (todas las tablas de `dbo`).
5. En la pestaña **Refresh**, deje marcada la tabla `Books`.
6. Deje marcada la opción **Pluralize or singularize generated object names** y presione **Finish**.
7. Guarde el `.edmx` (Ctrl+S). Así se regeneran las clases `Author`, `Customer`, `Order`, etc.

`BookAuthors` no genera una clase: Entity Framework la convierte en la relación `Book.Authors` / `Author.Books`. El código ya cuenta con eso.

## 3. Compilar y ejecutar
1. Clic derecho en la solución → **Restore NuGet Packages**.
2. **Build → Rebuild Solution**.
3. Ejecute `PAP.Mvc` (F5).

## 4. Generar los datos
1. En el menú superior, entre a **Admin**.
2. Al inicio solo se habilitan las tablas sin dependencias (Authors, Publishers, Categories, Genres, Customers, Suppliers). `Roles` y `Books` aparecen como "Ya tiene datos".
3. A medida que genera, se habilitan las demás. Un orden que funciona:
   Publishers → Categories → Genres → Authors → Customers → Suppliers → BookAuthors → Addresses → Orders → OrderItems → Payments → ShoppingCarts → CartItems → Reviews → Inventory → PurchaseOrders → PurchaseOrderItems → Employees.
   - Si su tabla Books está vacía, genérela después de Publishers, Categories y Genres.
4. Si una tabla ya tiene datos, el botón queda deshabilitado y el servidor también rechaza la generación con un mensaje de error.

Contraseñas de los registros generados: clientes `Cliente123!`, empleados `Empleado123!`.

## Qué se agregó
| Proyecto | Contenido |
|---|---|
| PAP.DataAccess | `Entities/EntityField.cs` (metadatos de columnas), `Entities/BookAuthorLink.cs` |
| PAP.Repositories | Un repository por entidad, `BookAuthorRepository`, y en `RepositoryBase`: `AddRange`, `HasData`, `GetFields` |
| PAP.Business | Un manager por entidad (reglas de negocio) y `Core/` (ManagerBase, errores, contraseñas) |
| PAP.Generators | Proyecto nuevo: un `XxxGenerator` por tabla, `GeneratorService` (validación de dependencias) |
| PAP.Mvc | Un controller y una vista `Index` por entidad (tabla + Agregar + Editar + Eliminar + Detalle), `AdminController`, menú en `_Layout.cshtml` |
