using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.PurchaseOrders.
    /// </summary>
    public class PurchaseOrdersController : CrudController<PurchaseOrder>
    {
        public PurchaseOrdersController()
            : base(new PurchaseOrderManager(), "Órdenes de compra", "orden de compra")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "SupplierId", Lookups.Suppliers() }
            };
        }
    }
}
