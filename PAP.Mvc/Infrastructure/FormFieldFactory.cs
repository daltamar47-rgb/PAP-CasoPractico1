using PAP.DataAccess.Entities;
using PAP.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PAP.Mvc.Infrastructure
{
    /// <summary>
    /// Builds the form fields of a page from the column definitions of the data model.
    /// </summary>
    public static class FormFieldFactory
    {
        private const int LongTextLength = 500;

        /// <summary>Columns the business layer fills when they are left empty.</summary>
        private static readonly string[] ServerDefaultedTexts = { "Status", "AddressType" };

        private static readonly IDictionary<string, string> InitialValues = new Dictionary<string, string>
        {
            { "IsActive", "true" },
            { "IsInStock", "true" },
            { "Quantity", "1" },
            { "Rating", "5" },
            { "ReorderLevel", "5" }
        };

        private static readonly Type[] WholeNumberTypes = { typeof(int), typeof(short), typeof(byte), typeof(long) };

        private static readonly Type[] DecimalNumberTypes = { typeof(decimal), typeof(double), typeof(float) };

        public static IList<FormField> Create(IEnumerable<EntityField> columns, IDictionary<string, IList<SelectListItem>> lookups)
        {
            return columns.Select(column => Create(column, lookups)).ToList();
        }

        private static FormField Create(EntityField column, IDictionary<string, IList<SelectListItem>> lookups)
        {
            var field = new FormField
            {
                Name = column.Name,
                Label = FieldLabels.For(column.Name),
                Kind = ResolveKind(column, lookups),
                MaxLength = column.MaxLength,
                Required = !column.IsNullable
            };

            IList<SelectListItem> options;
            if (lookups.TryGetValue(column.Name, out options))
            {
                field.Options = options;
            }

            ApplyRules(column, field);
            return field;
        }

        private static FieldKind ResolveKind(EntityField column, IDictionary<string, IList<SelectListItem>> lookups)
        {
            if (column.IsKey)
            {
                return FieldKind.Hidden;
            }

            if (column.IsSecret)
            {
                return FieldKind.Password;
            }

            if (lookups.ContainsKey(column.Name))
            {
                return FieldKind.Select;
            }

            if (column.ClrType == typeof(bool))
            {
                return FieldKind.Checkbox;
            }

            if (column.ClrType == typeof(DateTime))
            {
                return column.IsDateOnly ? FieldKind.Date : FieldKind.DateTime;
            }

            if (WholeNumberTypes.Contains(column.ClrType))
            {
                return FieldKind.Number;
            }

            if (DecimalNumberTypes.Contains(column.ClrType))
            {
                return FieldKind.Decimal;
            }

            var isLongText = !column.MaxLength.HasValue || column.MaxLength.Value > LongTextLength;
            return isLongText ? FieldKind.TextArea : FieldKind.Text;
        }

        private static void ApplyRules(EntityField column, FormField field)
        {
            if (column.IsKey)
            {
                field.Required = false;
                field.DefaultValue = "0";
            }

            if (column.IsAudit)
            {
                field.ShowInForm = false;
                field.Required = false;
            }

            if (column.IsSecret)
            {
                field.Required = false;
                field.ShowInDetails = false;
                field.Placeholder = "Al editar, déjelo vacío para mantener la actual";
            }

            if (field.Kind == FieldKind.Checkbox)
            {
                field.Required = false;
                field.DefaultValue = "false";
            }

            if (field.Kind == FieldKind.DateTime && !column.IsNullable)
            {
                field.Required = false;
                field.Placeholder = "Vacío = fecha y hora actual";
            }

            if (ServerDefaultedTexts.Contains(column.Name))
            {
                field.Required = false;
            }

            if ((field.Kind == FieldKind.Number || field.Kind == FieldKind.Decimal) && !column.IsNullable)
            {
                field.DefaultValue = "0";
            }

            string initialValue;
            if (InitialValues.TryGetValue(column.Name, out initialValue))
            {
                field.DefaultValue = initialValue;
            }
        }
    }
}
