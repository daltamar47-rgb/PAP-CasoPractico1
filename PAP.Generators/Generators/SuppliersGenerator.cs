using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Suppliers — no dependencies.
    /// </summary>
    public class SuppliersGenerator : GeneratorBase<Supplier>
    {
        private const int Count = 10;

        public SuppliersGenerator()
            : base(new SupplierRepository())
        {
        }

        public override string TableName => Tables.Suppliers;

        protected override IEnumerable<Supplier> Build()
        {
            for (var index = 1; index <= Count; index++)
            {
                var contactFirstName = Data.FirstName();
                var contactLastName = Data.LastName();
                yield return new Supplier
                {
                    Name = "Proveedora " + Data.CompanyName() + " " + index,
                    ContactName = contactFirstName + " " + contactLastName,
                    Email = Data.Email(contactFirstName, contactLastName, index, "proveedores.com"),
                    Phone = Data.Phone(),
                    Address = Data.Street() + ", " + Data.Pick(SampleData.Cities),
                    IsActive = Data.Chance(85)
                };
            }
        }
    }
}
