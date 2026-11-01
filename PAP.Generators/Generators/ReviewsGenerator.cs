using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Reviews — depends on dbo.Books and dbo.Customers.
    /// A customer reviews a book only once (unique BookId + CustomerId).
    /// </summary>
    public class ReviewsGenerator : GeneratorBase<Review>
    {
        private const int Count = 80;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Books, Tables.Customers };

        private static readonly string[] Comments =
        {
            "Excelente lectura, muy recomendado.", "Me gustó, aunque el final fue predecible.",
            "Una historia atrapante de principio a fin.", "No era lo que esperaba.",
            "Buen libro para leer en vacaciones.", "Los personajes están muy bien construidos."
        };

        public ReviewsGenerator()
            : base(new ReviewRepository())
        {
        }

        public override string TableName => Tables.Reviews;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Review> Build()
        {
            var bookIds = IdsOf(new BookRepository(), book => book.Id);
            var customerIds = IdsOf(new CustomerRepository(), customer => customer.Id);

            var pairs = bookIds
                .SelectMany(bookId => customerIds.Select(customerId => new { BookId = bookId, CustomerId = customerId }))
                .ToList();

            return Data.PickMany(pairs, Count).Select(pair => new Review
            {
                BookId = pair.BookId,
                CustomerId = pair.CustomerId,
                Rating = (byte)Data.Between(1, 5),
                Comment = Data.Pick(Comments),
                CreatedAt = Data.RecentDate(200)
            });
        }
    }
}
