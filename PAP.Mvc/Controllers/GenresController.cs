using PAP.Business;
using PAP.DataAccess;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Maintenance page for dbo.Genres.
    /// </summary>
    public class GenresController : CrudController<Genre>
    {
        public GenresController()
            : base(new GenreManager(), "Géneros", "género")
        {
        }
    }
}
