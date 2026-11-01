using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using PAP.Business.Core;
using System.Collections.Generic;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Employees — depends on dbo.Roles. Every generated employee has the password "Empleado123!".
    /// </summary>
    public class EmployeesGenerator : GeneratorBase<Employee>
    {
        private const int Count = 12;
        private const string SamplePassword = "Empleado123!";

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Roles };

        public EmployeesGenerator()
            : base(new EmployeeRepository())
        {
        }

        public override string TableName => Tables.Employees;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Employee> Build()
        {
            var roleIds = IdsOf(new RoleRepository(), role => role.Id);
            var passwordHash = PasswordHasher.Hash(SamplePassword);
            for (var index = 1; index <= Count; index++)
            {
                var firstName = Data.FirstName();
                var lastName = Data.LastName();
                yield return new Employee
                {
                    RoleId = Data.Pick(roleIds),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = Data.Email(firstName, lastName, index, "bookstore.com"),
                    PasswordHash = passwordHash,
                    IsActive = Data.Chance(90),
                    CreatedAt = Data.RecentDate(730)
                };
            }
        }
    }
}
