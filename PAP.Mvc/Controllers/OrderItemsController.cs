using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.OrderItems.
    /// </summary>
    public class OrderItemsController : CrudController<OrderItem>
    {
        public OrderItemsController()
            : base(new OrderItemManager(), "Detalle de órdenes", "línea de orden")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "OrderId", Lookups.Orders() },
                { "BookId", Lookups.Books() }
            };
        }
    }
}
