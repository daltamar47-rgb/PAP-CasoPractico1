using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Addresses — depends on dbo.Customers. Each customer gets 1 or 2 addresses; the first one is the default.
    /// </summary>
    public class AddressesGenerator : GeneratorBase<Address>
    {
        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Customers };

        public AddressesGenerator()
            : base(new AddressRepository())
        {
        }

        public override string TableName => Tables.Addresses;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Address> Build()
        {
            var customerIds = IdsOf(new CustomerRepository(), customer => customer.Id);
            foreach (var customerId in customerIds)
            {
                var addressCount = Data.Between(1, 2);
                for (var number = 1; number <= addressCount; number++)
                {
                    yield return new Address
                    {
                        CustomerId = customerId,
                        AddressType = number == 1 ? "Shipping" : "Billing",
                        Street = Data.Street(),
                        City = Data.Pick(SampleData.Cities),
                        State = Data.Pick(SampleData.Provinces),
                        PostalCode = Data.PostalCode(),
                        Country = "Costa Rica",
                        IsDefault = number == 1
                    };
                }
            }
        }
    }
}
