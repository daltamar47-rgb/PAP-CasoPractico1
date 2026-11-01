using PAP.DataAccess;
using PAP.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Repositories
{
    /// <summary>
    /// Data access for dbo.BookAuthors.
    /// </summary>
    public interface IBookAuthorRepository
    {
        IEnumerable<BookAuthorLink> GetAll();

        bool Exists(int bookId, int authorId);

        void Add(int bookId, int authorId);

        void AddRange(IEnumerable<BookAuthorLink> links);

        void Delete(int bookId, int authorId);

        bool HasData();
    }

    /// <summary>
    /// dbo.BookAuthors is a pure join table, so Entity Framework exposes it through the
    /// navigation collections Book.Authors and Author.Books instead of an entity class.
    /// This repository works on those collections.
    /// </summary>
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly BookStoreDBEntities _context;

        public BookAuthorRepository()
        {
            _context = new BookStoreDBEntities();
        }

        public IEnumerable<BookAuthorLink> GetAll()
        {
            var links = from book in _context.Set<Book>()
                        from author in book.Authors
                        orderby book.Title, author.LastName
                        select new BookAuthorLink
                        {
                            BookId = book.Id,
                            AuthorId = author.Id,
                            BookTitle = book.Title,
                            AuthorName = author.FirstName + " " + author.LastName
                        };

            return links.ToList();
        }

        public bool Exists(int bookId, int authorId)
        {
            return _context.Set<Book>().Any(book => book.Id == bookId && book.Authors.Any(author => author.Id == authorId));
        }

        public void Add(int bookId, int authorId)
        {
            AttachLink(bookId, authorId);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<BookAuthorLink> links)
        {
            foreach (var link in links)
            {
                AttachLink(link.BookId, link.AuthorId);
            }

            _context.SaveChanges();
        }

        public void Delete(int bookId, int authorId)
        {
            var book = _context.Set<Book>().Find(bookId);
            var author = book?.Authors.FirstOrDefault(item => item.Id == authorId);
            if (author == null)
            {
                return;
            }

            book.Authors.Remove(author);
            _context.SaveChanges();
        }

        public bool HasData()
        {
            return _context.Set<Book>().Any(book => book.Authors.Any());
        }

        private void AttachLink(int bookId, int authorId)
        {
            var book = _context.Set<Book>().Find(bookId);
            var author = _context.Set<Author>().Find(authorId);
            if (book == null || author == null)
            {
                throw new KeyNotFoundException("El libro o el autor indicado no existe.");
            }

            book.Authors.Add(author);
        }
    }
}
