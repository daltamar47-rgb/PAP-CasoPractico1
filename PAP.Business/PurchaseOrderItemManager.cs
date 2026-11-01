using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for purchase order lines (dbo.PurchaseOrderItems).
    /// </summary>
    public class PurchaseOrderItemManager : ManagerBase<PurchaseOrderItem>
    {
        public PurchaseOrderItemManager()
            : base(new PurchaseOrderItemRepository())
        {
        }

        protected override void Validate(PurchaseOrderItem item)
        {
            AmountRules.RequirePositive(item.Quantity, "La cantidad");
            AmountRules.RequireNotNegative(item.UnitCost, "El costo unitario");
        }
    }
}
