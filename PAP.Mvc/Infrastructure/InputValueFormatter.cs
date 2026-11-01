using PAP.Mvc.Models;
using System;
using System.Globalization;

namespace PAP.Mvc.Infrastructure
{
    /// <summary>
    /// Formats values the way HTML inputs expect them (ISO dates, dot as decimal separator).
    /// </summary>
    public static class InputValueFormatter
    {
        public static string Format(object value, FieldKind kind)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is bool)
            {
                return (bool)value ? "true" : "false";
            }

            if (value is DateTime)
            {
                var date = (DateTime)value;
                return kind == FieldKind.Date
                    ? date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : date.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
