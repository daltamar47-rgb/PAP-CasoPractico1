using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for supplier purchase orders (dbo.PurchaseOrders).
    /// </summary>
    public class PurchaseOrderManager : ManagerBase<PurchaseOrder>
    {
        private const string DefaultStatus = "Pending";

        public PurchaseOrderManager()
            : base(new PurchaseOrderRepository())
        {
        }

        protected override void ApplyDefaults(PurchaseOrder purchaseOrder)
        {
            if (string.IsNullOrWhiteSpace(purchaseOrder.Status))
            {
                purchaseOrder.Status = DefaultStatus;
            }
        }

        protected override void Validate(PurchaseOrder purchaseOrder)
        {
            AmountRules.RequireNotNegative(purchaseOrder.TotalCost, "El costo total");
        }
    }
}
