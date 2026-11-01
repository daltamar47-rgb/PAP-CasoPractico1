using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for shopping cart items (dbo.CartItems).
    /// </summary>
    public class CartItemManager : ManagerBase<CartItem>
    {
        public CartItemManager()
            : base(new CartItemRepository())
        {
        }

        protected override void Validate(CartItem item)
        {
            AmountRules.RequirePositive(item.Quantity, "La cantidad");
        }
    }
}
