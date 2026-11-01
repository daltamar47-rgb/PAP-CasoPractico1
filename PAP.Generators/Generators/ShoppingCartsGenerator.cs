using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.ShoppingCarts — depends on dbo.Customers. Half of the customers get a cart.
    /// </summary>
    public class ShoppingCartsGenerator : GeneratorBase<ShoppingCart>
    {
        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Customers };

        public ShoppingCartsGenerator()
            : base(new ShoppingCartRepository())
        {
        }

        public override string TableName => Tables.ShoppingCarts;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<ShoppingCart> Build()
        {
            var customerIds = IdsOf(new CustomerRepository(), customer => customer.Id);
            foreach (var customerId in Data.PickMany(customerIds, customerIds.Count / 2 + 1))
            {
                var createdAt = Data.RecentDate(60);
                yield return new ShoppingCart
                {
                    CustomerId = customerId,
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                };
            }
        }
    }
}
