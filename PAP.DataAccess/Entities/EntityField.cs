using System;
using System.Linq;

namespace PAP.DataAccess.Entities
{
    /// <summary>
    /// Describes one scalar column of an entity, as defined in the data model (.edmx).
    /// Used by the upper layers to build forms, details and to copy values generically.
    /// </summary>
    public sealed class EntityField
    {
        private static readonly string[] AuditFieldNames = { "CreatedAt", "UpdatedAt", "AddedAt" };

        public EntityField(string name, Type clrType, bool isNullable, int? maxLength, bool isKey, bool isDateOnly)
        {
            Name = name;
            ClrType = Nullable.GetUnderlyingType(clrType) ?? clrType;
            IsNullable = isNullable;
            MaxLength = maxLength;
            IsKey = isKey;
            IsDateOnly = isDateOnly;
        }

        public string Name { get; }

        /// <summary>Non-nullable CLR type of the column (int instead of int?).</summary>
        public Type ClrType { get; }

        public bool IsNullable { get; }

        /// <summary>Maximum length for text columns; null when unlimited or not text.</summary>
        public int? MaxLength { get; }

        public bool IsKey { get; }

        /// <summary>True for SQL DATE columns (no time part).</summary>
        public bool IsDateOnly { get; }

        /// <summary>Timestamps handled by the system, never typed by the user.</summary>
        public bool IsAudit => AuditFieldNames.Contains(Name);

        /// <summary>Password columns: never shown and only overwritten when a new value is typed.</summary>
        public bool IsSecret => Name.IndexOf("Password", StringComparison.OrdinalIgnoreCase) >= 0;

        public bool IsForeignKey => !IsKey && ClrType == typeof(int) && Name.Length > 2 && Name.EndsWith("Id", StringComparison.Ordinal);

        /// <summary>Columns the user is allowed to type or change.</summary>
        public bool IsEditable => !IsKey && !IsAudit;

        public object GetValue(object entity)
        {
            var property = entity.GetType().GetProperty(Name);
            return property == null ? null : property.GetValue(entity);
        }

        public void SetValue(object entity, object value)
        {
            var property = entity.GetType().GetProperty(Name);
            if (property != null && property.CanWrite)
            {
                property.SetValue(entity, value);
            }
        }
    }
}
