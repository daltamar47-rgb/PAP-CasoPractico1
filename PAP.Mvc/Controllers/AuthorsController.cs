using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Authors.
    /// </summary>
    public class AuthorsController : CrudController<Author>
    {
        public AuthorsController()
            : base(new AuthorManager(), "Autores", "autor")
        {
        }
    }
}
