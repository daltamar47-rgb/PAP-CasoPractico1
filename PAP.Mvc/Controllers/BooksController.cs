using PAP.Business;
using PAP.DataAccess;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Books.
    /// </summary>
    public class BooksController : CrudController<Book>
    {
        public BooksController()
            : base(new BookManager(), "Libros", "libro")
        {
        }

        protected override IDictionary<string, IList<SelectListItem>> GetLookups()
        {
            return new Dictionary<string, IList<SelectListItem>>
            {
                { "PublisherId", Lookups.Publishers() },
                { "CategoryId", Lookups.Categories() },
                { "GenreId", Lookups.Genres() }
            };
        }
    }
}
