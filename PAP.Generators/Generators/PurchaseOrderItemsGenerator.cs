using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.PurchaseOrderItems — depends on dbo.PurchaseOrders and dbo.Books.
    /// The unit cost is 60% of the book price; the total cost of each purchase order is recalculated.
    /// </summary>
    public class PurchaseOrderItemsGenerator : GeneratorBase<PurchaseOrderItem>
    {
        private const decimal CostRate = 0.60m;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.PurchaseOrders, Tables.Books };

        public PurchaseOrderItemsGenerator()
            : base(new PurchaseOrderItemRepository())
        {
        }

        public override string TableName => Tables.PurchaseOrderItems;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        public override int Generate()
        {
            var count = base.Generate();
            UpdatePurchaseOrderTotals();
            return count;
        }

        protected override IEnumerable<PurchaseOrderItem> Build()
        {
            var purchaseOrderIds = IdsOf(new PurchaseOrderRepository(), purchaseOrder => purchaseOrder.Id);
            var books = new BookRepository().GetAll().ToList();

            foreach (var purchaseOrderId in purchaseOrderIds)
            {
                foreach (var book in Data.PickMany(books, Data.Between(1, 5)))
                {
                    yield return new PurchaseOrderItem
                    {
                        PurchaseOrderId = purchaseOrderId,
                        BookId = book.Id,
                        Quantity = Data.Between(5, 50),
                        UnitCost = decimal.Round(book.Price * CostRate, 2)
                    };
                }
            }
        }

        private void UpdatePurchaseOrderTotals()
        {
            var itemsByOrder = Repository.GetAll().ToLookup(item => item.PurchaseOrderId);
            var purchaseOrderRepository = new PurchaseOrderRepository();

            foreach (var purchaseOrder in purchaseOrderRepository.GetAll())
            {
                purchaseOrder.TotalCost = itemsByOrder[purchaseOrder.Id].Sum(item => item.UnitCost * item.Quantity);
            }

            purchaseOrderRepository.Save();
        }
    }
}
