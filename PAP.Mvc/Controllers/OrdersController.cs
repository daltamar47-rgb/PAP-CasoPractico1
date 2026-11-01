using PAP.Business;
using PAP.DataAccess;
using PAP.Mvc.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Orders.
    /// </summary>
    public class OrdersController : CrudController<Order>
    {
        public OrdersController()
            : base(new OrderManager(), "Órdenes", "orden")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "CustomerId", Lookups.Customers() },
                { "ShippingAddressId", Lookups.Addresses() }
            };
        }

        protected override void ConfigureField(FormField field)
        {
            if (field.Name == "Total")
            {
                // The total is calculated by the business layer (subtotal + tax + shipping).
                field.ShowInForm = false;
            }
        }
    }
}
