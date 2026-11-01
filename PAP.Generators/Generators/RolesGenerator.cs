using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Roles — no dependencies. The database script already inserts Admin, Manager and Cashier,
    /// so this generator is only enabled if that table was emptied.
    /// </summary>
    public class RolesGenerator : GeneratorBase<Role>
    {
        private static readonly string[] Names = { "Admin", "Manager", "Cashier", "Inventory", "Support" };

        public RolesGenerator()
            : base(new RoleRepository())
        {
        }

        public override string TableName => Tables.Roles;

        protected override IEnumerable<Role> Build()
        {
            return Names.Select(name => new Role { Name = name });
        }
    }
}
