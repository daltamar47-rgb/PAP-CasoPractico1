using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Inventory — depends on dbo.Books. One stock record per book (unique BookId).
    /// </summary>
    public class InventoryGenerator : GeneratorBase<Inventory>
    {
        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Books };

        public InventoryGenerator()
            : base(new InventoryRepository())
        {
        }

        public override string TableName => Tables.Inventory;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Inventory> Build()
        {
            var bookIds = IdsOf(new BookRepository(), book => book.Id);
            return bookIds.Select(bookId => new Inventory
            {
                BookId = bookId,
                QuantityInStock = Data.Between(0, 120),
                ReorderLevel = Data.Between(5, 15),
                LastRestockedAt = Data.Chance(80) ? Data.RecentDate(90) : (System.DateTime?)null
            });
        }
    }
}
