using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using PAP.Business.Core;
using System;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Customers — no dependencies. Every generated customer has the password "Cliente123!".
    /// </summary>
    public class CustomersGenerator : GeneratorBase<Customer>
    {
        private const int Count = 40;
        private const string SamplePassword = "Cliente123!";

        public CustomersGenerator()
            : base(new CustomerRepository())
        {
        }

        public override string TableName => Tables.Customers;

        protected override IEnumerable<Customer> Build()
        {
            var passwordHash = PasswordHasher.Hash(SamplePassword);
            for (var index = 1; index <= Count; index++)
            {
                var firstName = Data.FirstName();
                var lastName = Data.LastName();
                var createdAt = Data.RecentDate(365);
                yield return new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = Data.Email(firstName, lastName, index, "correo.com"),
                    PasswordHash = passwordHash,
                    Phone = Data.Phone(),
                    BirthDate = Data.DateBetween(new DateTime(1960, 1, 1), new DateTime(2005, 12, 31)).Date,
                    IsActive = Data.Chance(90),
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                };
            }
        }
    }
}
