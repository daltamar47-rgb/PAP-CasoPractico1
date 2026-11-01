using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Inventory.
    /// </summary>
    public class InventoryController : CrudController<Inventory>
    {
        public InventoryController()
            : base(new InventoryManager(), "Inventario", "registro de inventario")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "BookId", Lookups.Books() }
            };
        }
    }
}
