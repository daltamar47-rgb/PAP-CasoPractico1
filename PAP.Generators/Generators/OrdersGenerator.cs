using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Orders — depends on dbo.Customers and dbo.Addresses.
    /// The amounts start at 0 and are calculated when dbo.OrderItems is generated.
    /// </summary>
    public class OrdersGenerator : GeneratorBase<Order>
    {
        private const int Count = 60;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Customers, Tables.Addresses };

        private static readonly string[] Statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

        public OrdersGenerator()
            : base(new OrderRepository())
        {
        }

        public override string TableName => Tables.Orders;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Order> Build()
        {
            var addressesByCustomer = new AddressRepository().GetAll()
                .GroupBy(address => address.CustomerId)
                .ToList();

            for (var index = 1; index <= Count; index++)
            {
                var customerAddresses = Data.Pick(addressesByCustomer);
                var orderDate = Data.RecentDate(180);

                yield return new Order
                {
                    CustomerId = customerAddresses.Key,
                    ShippingAddressId = Data.Pick(customerAddresses.ToList()).Id,
                    OrderDate = orderDate,
                    Status = Data.Pick(Statuses),
                    Subtotal = 0,
                    TaxAmount = 0,
                    ShippingCost = 0,
                    Total = 0,
                    Notes = Data.Chance(30) ? "Entregar en horario de oficina." : null,
                    UpdatedAt = orderDate
                };
            }
        }
    }
}
