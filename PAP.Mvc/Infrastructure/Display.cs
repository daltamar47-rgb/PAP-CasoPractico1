using System;
using System.Web;
using System.Web.Mvc;

namespace PAP.Mvc.Infrastructure
{
    /// <summary>
    /// Small formatting helpers used by the table columns of the Index views.
    /// </summary>
    public static class Display
    {
        public static string Date(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("dd/MM/yyyy") : "-";
        }

        public static string DateAndTime(DateTime? value)
        {
            return value.HasValue ? value.Value.ToString("dd/MM/yyyy HH:mm") : "-";
        }

        public static string Money(decimal value)
        {
            return value.ToString("N2");
        }

        public static string Text(string value, int maxLength = 60)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "-";
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "…";
        }

        public static IHtmlString Flag(bool value, string trueText = "Sí", string falseText = "No")
        {
            var html = value
                ? "<span class=\"badge text-bg-success\">" + HttpUtility.HtmlEncode(trueText) + "</span>"
                : "<span class=\"badge text-bg-secondary\">" + HttpUtility.HtmlEncode(falseText) + "</span>";

            return MvcHtmlString.Create(html);
        }

        public static string Stars(byte rating)
        {
            return new string('★', rating) + new string('☆', Math.Max(0, 5 - rating));
        }
    }
}
