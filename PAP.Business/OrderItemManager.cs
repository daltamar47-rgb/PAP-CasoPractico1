using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for order lines (dbo.OrderItems).
    /// </summary>
    public class OrderItemManager : ManagerBase<OrderItem>
    {
        public OrderItemManager()
            : base(new OrderItemRepository())
        {
        }

        protected override void Validate(OrderItem item)
        {
            AmountRules.RequirePositive(item.Quantity, "La cantidad");
            AmountRules.RequireNotNegative(item.UnitPrice, "El precio unitario");
            AmountRules.RequireNotNegative(item.Discount, "El descuento");

            if (item.Discount > item.UnitPrice * item.Quantity)
            {
                throw new BusinessException("El descuento no puede ser mayor al monto de la línea.");
            }
        }
    }
}
