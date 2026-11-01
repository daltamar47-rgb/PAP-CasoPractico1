using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PAP.Mvc.Infrastructure
{
    /// <summary>
    /// Spanish labels for the database column names.
    /// </summary>
    public static class FieldLabels
    {
        private static readonly IDictionary<string, string> Labels = new Dictionary<string, string>
        {
            { "Id", "#" },
            { "Name", "Nombre" },
            { "Title", "Título" },
            { "FirstName", "Nombre" },
            { "LastName", "Apellidos" },
            { "Biography", "Biografía" },
            { "BirthDate", "Fecha de nacimiento" },
            { "Country", "País" },
            { "Website", "Sitio web" },
            { "Email", "Correo" },
            { "Phone", "Teléfono" },
            { "Description", "Descripción" },
            { "Category", "Categoría (texto)" },
            { "Publisher", "Editorial (texto)" },
            { "Author", "Autor (texto)" },
            { "Genre", "Género (texto)" },
            { "PublicationDate", "Fecha de publicación" },
            { "Height", "Alto" },
            { "Width", "Ancho" },
            { "Price", "Precio" },
            { "IsBestSeller", "Best seller" },
            { "IsInStock", "En stock" },
            { "ISBN", "ISBN" },
            { "Pages", "Páginas" },
            { "Language", "Idioma" },
            { "CoverImageUrl", "URL de portada" },
            { "PublisherId", "Editorial" },
            { "CategoryId", "Categoría" },
            { "GenreId", "Género" },
            { "BookId", "Libro" },
            { "AuthorId", "Autor" },
            { "CustomerId", "Cliente" },
            { "PasswordHash", "Contraseña" },
            { "IsActive", "Activo" },
            { "AddressType", "Tipo de dirección" },
            { "Street", "Dirección" },
            { "City", "Ciudad" },
            { "State", "Provincia / Estado" },
            { "PostalCode", "Código postal" },
            { "IsDefault", "Predeterminada" },
            { "ShippingAddressId", "Dirección de envío" },
            { "OrderDate", "Fecha de la orden" },
            { "Status", "Estado" },
            { "Subtotal", "Subtotal" },
            { "TaxAmount", "Impuesto" },
            { "ShippingCost", "Costo de envío" },
            { "Total", "Total" },
            { "Notes", "Notas" },
            { "OrderId", "Orden" },
            { "Quantity", "Cantidad" },
            { "UnitPrice", "Precio unitario" },
            { "Discount", "Descuento" },
            { "PaymentMethod", "Método de pago" },
            { "Amount", "Monto" },
            { "PaymentDate", "Fecha de pago" },
            { "TransactionRef", "Referencia" },
            { "CartId", "Carrito" },
            { "Rating", "Calificación" },
            { "Comment", "Comentario" },
            { "QuantityInStock", "Cantidad en stock" },
            { "ReorderLevel", "Nivel de reorden" },
            { "LastRestockedAt", "Último reabastecimiento" },
            { "ContactName", "Contacto" },
            { "Address", "Dirección" },
            { "SupplierId", "Proveedor" },
            { "ExpectedDate", "Fecha esperada" },
            { "TotalCost", "Costo total" },
            { "PurchaseOrderId", "Orden de compra" },
            { "UnitCost", "Costo unitario" },
            { "RoleId", "Rol" },
            { "CreatedAt", "Creado" },
            { "UpdatedAt", "Actualizado" },
            { "AddedAt", "Agregado" }
        };

        public static string For(string fieldName)
        {
            string label;
            if (Labels.TryGetValue(fieldName, out label))
            {
                return label;
            }

            return Regex.Replace(fieldName, "(?<!^)([A-Z])", " $1");
        }
    }
}
