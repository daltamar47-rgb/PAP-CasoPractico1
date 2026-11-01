using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.CartItems.
    /// </summary>
    public class CartItemsController : CrudController<CartItem>
    {
        public CartItemsController()
            : base(new CartItemManager(), "Artículos de carrito", "artículo de carrito")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "CartId", Lookups.ShoppingCarts() },
                { "BookId", Lookups.Books() }
            };
        }
    }
}
