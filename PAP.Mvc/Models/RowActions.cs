using System.Collections.Generic;

namespace PAP.Mvc.Models
{
    /// <summary>
    /// Data for the Detalle / Editar / Eliminar buttons of one table row.
    /// </summary>
    public class RowActions
    {
        public string Controller { get; set; }

        /// <summary>All the values of the row, as JSON, used to fill the edit form and the details modal.</summary>
        public string EntityJson { get; set; }

        /// <summary>Hidden values posted by the delete button (normally just the id).</summary>
        public IDictionary<string, string> DeleteValues { get; set; }
    }
}
