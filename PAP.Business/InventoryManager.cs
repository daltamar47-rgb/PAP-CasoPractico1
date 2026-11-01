using PAP.Business.Core;
using PAP.DataAccess;
using PAP.Repositories;

namespace PAP.Business
{
    /// <summary>
    /// Business operations for book stock (dbo.Inventory).
    /// </summary>
    public class InventoryManager : ManagerBase<Inventory>
    {
        public InventoryManager()
            : base(new InventoryRepository())
        {
        }

        protected override void Validate(Inventory inventory)
        {
            AmountRules.RequireNotNegative(inventory.QuantityInStock, "La cantidad en stock");
            AmountRules.RequireNotNegative(inventory.ReorderLevel, "El nivel de reorden");
        }
    }
}
