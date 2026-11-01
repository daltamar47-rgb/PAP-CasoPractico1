using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.PurchaseOrderItems.
    /// </summary>
    public class PurchaseOrderItemsController : CrudController<PurchaseOrderItem>
    {
        public PurchaseOrderItemsController()
            : base(new PurchaseOrderItemManager(), "Detalle de órdenes de compra", "línea de orden de compra")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "PurchaseOrderId", Lookups.PurchaseOrders() },
                { "BookId", Lookups.Books() }
            };
        }
    }
}
