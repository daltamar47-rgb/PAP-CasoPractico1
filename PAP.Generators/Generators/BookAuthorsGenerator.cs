using PAP.DataAccess;
using PAP.Generators.Core;
using PAP.Repositories;
using PAP.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators.Generators
{
    /// <summary>
    /// dbo.BookAuthors — depends on dbo.Books and dbo.Authors. Each book gets between 1 and 3 authors.
    /// The table has no entity class (many-to-many), so it uses BookAuthorRepository.
    /// </summary>
    public class BookAuthorsGenerator : IDataGenerator
    {
        private static readonly IReadOnlyList<string> Dependencies = new[] { Tables.Books, Tables.Authors };

        private readonly IBookAuthorRepository _repository;
        private readonly SampleData _data;

        public BookAuthorsGenerator()
        {
            _repository = new BookAuthorRepository();
            _data = new SampleData();
        }

        public string TableName => Tables.BookAuthors;

        public IReadOnlyList<string> DependsOn => Dependencies;

        public bool HasData()
        {
            return _repository.HasData();
        }

        public int Generate()
        {
            var bookIds = new BookRepository().GetAll().Select(book => book.Id).ToList();
            var authorIds = new AuthorRepository().GetAll().Select(author => author.Id).ToList();

            var links = bookIds
                .SelectMany(bookId => _data.PickMany(authorIds, _data.Between(1, 3))
                    .Select(authorId => new BookAuthorLink { BookId = bookId, AuthorId = authorId }))
                .ToList();

            _repository.AddRange(links);
            return links.Count;
        }
    }
}
