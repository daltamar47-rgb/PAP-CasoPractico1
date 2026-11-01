using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for carritos de compra (dbo.ShoppingCarts).
    /// </summary>
    public class ShoppingCartManager : ManagerBase<ShoppingCart>
    {
        public ShoppingCartManager()
            : base(new ShoppingCartRepository())
        {
        }
    }
}
