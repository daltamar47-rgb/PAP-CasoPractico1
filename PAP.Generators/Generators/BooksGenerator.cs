using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.Books — depends on dbo.Publishers, dbo.Categories and dbo.Genres.
    /// The old text columns (Publisher, Category, Genre) are filled with the same names as the foreign keys.
    /// </summary>
    public class BooksGenerator : GeneratorBase<Book>
    {
        private const int Count = 50;

        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Publishers, Tables.Categories, Tables.Genres };

        public BooksGenerator()
            : base(new BookRepository())
        {
        }

        public override string TableName => Tables.Books;

        public override IReadOnlyList<string> DependsOn => Dependencies;

        protected override IEnumerable<Book> Build()
        {
            var publishers = new PublisherRepository().GetAll().ToList();
            var categories = new CategoryRepository().GetAll().ToList();
            var genres = new GenreRepository().GetAll().ToList();

            for (var index = 1; index <= Count; index++)
            {
                var title = Data.BookTitle();
                var publisher = Data.Pick(publishers);
                var category = Data.Pick(categories);
                var genre = Data.Pick(genres);
                var createdAt = Data.RecentDate(365);

                yield return new Book
                {
                    Name = title,
                    Title = title,
                    PublisherId = publisher.Id,
                    Publisher = publisher.Name,
                    CategoryId = category.Id,
                    Category = category.Name,
                    GenreId = genre.Id,
                    Genre = genre.Name,
                    Description = Data.Sentence(20),
                    PublicationDate = Data.DateBetween(new DateTime(1990, 1, 1), DateTime.Today).Date,
                    Height = Data.Money(18, 28),
                    Width = Data.Money(12, 20),
                    Price = Data.Money(8, 65),
                    IsBestSeller = Data.Chance(20),
                    IsInStock = Data.Chance(85),
                    ISBN = Data.Isbn(index),
                    Pages = Data.Between(90, 850),
                    Language = Data.Pick(SampleData.Languages),
                    CoverImageUrl = "https://picsum.photos/seed/book" + index + "/300/450",
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt
                };
            }
        }
    }
}
