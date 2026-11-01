using Newtonsoft.Json;
using PAP.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PAP.Mvc.Models
{
    /// <summary>
    /// Everything the shared partial views need to render a maintenance page
    /// (header, add button, form modal, details modal and row buttons).
    /// </summary>
    public class CrudPage
    {
        public CrudPage(string controller, string title, string singular, int count, IList<FormField> fields)
        {
            Controller = controller;
            Title = title;
            Singular = singular;
            Count = count;
            Fields = fields;
        }

        public string Controller { get; }

        public string Title { get; }

        public string Singular { get; }

        public int Count { get; }

        public IList<FormField> Fields { get; }

        public string FieldsJson
        {
            get
            {
                var fields = Fields.Select(field => new
                {
                    name = field.Name,
                    label = field.Label,
                    kind = field.Kind.ToString(),
                    defaultValue = field.DefaultValue ?? string.Empty,
                    showInDetails = field.ShowInDetails
                });

                return JsonConvert.SerializeObject(fields);
            }
        }

        public string LookupsJson
        {
            get
            {
                var lookups = Fields
                    .Where(field => field.Kind == FieldKind.Select)
                    .ToDictionary(field => field.Name, field => field.Options.ToDictionary(option => option.Value, option => option.Text));

                return JsonConvert.SerializeObject(lookups);
            }
        }

        /// <summary>
        /// Text shown in the table for a foreign key value (e.g. the customer name instead of its id).
        /// </summary>
        public string LookupText(string fieldName, int? value)
        {
            if (!value.HasValue)
            {
                return "-";
            }

            var field = Fields.FirstOrDefault(item => item.Name == fieldName);
            var key = value.Value.ToString();
            var option = field == null ? null : field.Options.FirstOrDefault(item => item.Value == key);
            return option == null ? "#" + key : option.Text;
        }

        public RowActions ActionsFor(object entity, int id)
        {
            return ActionsFor(entity, new Dictionary<string, string> { { "id", id.ToString() } });
        }

        public RowActions ActionsFor(object entity, IDictionary<string, string> deleteValues)
        {
            var values = Fields.ToDictionary(field => field.Name, field => InputValueFormatter.Format(ReadValue(entity, field.Name), field.Kind));

            return new RowActions
            {
                Controller = Controller,
                EntityJson = JsonConvert.SerializeObject(values),
                DeleteValues = deleteValues
            };
        }

        private static object ReadValue(object entity, string propertyName)
        {
            var property = entity.GetType().GetProperty(propertyName);
            return property == null ? null : property.GetValue(entity);
        }
    }

    /// <summary>
    /// Model of every Index view: the shared page data plus the rows of the table.
    /// </summary>
    public class CrudPageModel<T> : CrudPage
    {
        public CrudPageModel(string controller, string title, string singular, IList<T> items, IList<FormField> fields)
            : base(controller, title, singular, items.Count, fields)
        {
            Items = items;
        }

        public IList<T> Items { get; }
    }
}
