using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.PurchaseOrders — depends on dbo.Suppliers.
    /// The total cost starts at 0 and is calculated when dbo.PurchaseOrderItems is generated.
    /// </summary>
    public class PurchaseOrdersGenerator : GeneratorBase<PurchaseOrder>
    {
        private const int Count = 20;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Suppliers };

        private static readonly string[] Statuses = { "Pending", "Ordered", "Received", "Cancelled" };

        public PurchaseOrdersGenerator()
            : base(new PurchaseOrderRepository())
        {
        }

        public override string TableName => Tables.PurchaseOrders;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<PurchaseOrder> Build()
        {
            var supplierIds = IdsOf(new SupplierRepository(), supplier => supplier.Id);
            for (var index = 1; index <= Count; index++)
            {
                var orderDate = Data.RecentDate(120);
                yield return new PurchaseOrder
                {
                    SupplierId = Data.Pick(supplierIds),
                    OrderDate = orderDate,
                    ExpectedDate = orderDate.AddDays(Data.Between(5, 30)).Date,
                    Status = Data.Pick(Statuses),
                    TotalCost = 0
                };
            }
        }
    }
}
