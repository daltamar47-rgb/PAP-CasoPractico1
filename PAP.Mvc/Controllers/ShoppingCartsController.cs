using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.ShoppingCarts.
    /// </summary>
    public class ShoppingCartsController : CrudController<ShoppingCart>
    {
        public ShoppingCartsController()
            : base(new ShoppingCartManager(), "Carritos de compra", "carrito")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "CustomerId", Lookups.Customers() }
            };
        }
    }
}
