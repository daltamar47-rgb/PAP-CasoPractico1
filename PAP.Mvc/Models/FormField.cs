using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Models
{
    /// <summary>
    /// One input of the create/edit form and one line of the details modal.
    /// </summary>
    public class FormField
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public FieldKind Kind { get; set; }

        public bool Required { get; set; }

        public int? MaxLength { get; set; }

        /// <summary>Value placed in the input when creating a new record.</summary>
        public string DefaultValue { get; set; }

        public string Placeholder { get; set; }

        public bool ShowInForm { get; set; } = true;

        public bool ShowInDetails { get; set; } = true;

        public IList<SelectListItem> Options { get; set; } = new List<SelectListItem>();

        public string InputType
        {
            get
            {
                switch (Kind)
                {
                    case FieldKind.Number:
                    case FieldKind.Decimal:
                        return "number";
                    case FieldKind.Date:
                        return "date";
                    case FieldKind.DateTime:
                        return "datetime-local";
                    case FieldKind.Password:
                        return "password";
                    case FieldKind.Hidden:
                        return "hidden";
                    default:
                        return "text";
                }
            }
        }

        public string Step
        {
            get
            {
                switch (Kind)
                {
                    case FieldKind.Number:
                        return "1";
                    case FieldKind.Decimal:
                        return "0.01";
                    default:
                        return null;
                }
            }
        }
    }
}
